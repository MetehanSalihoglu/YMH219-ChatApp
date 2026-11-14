using System.Net.Sockets;
using System.Text;
using ChatApp.Common;

namespace ChatApp.Client.Network;

/// <summary>
/// TcpClient tabanli sohbet istemcisi. Mesaj alma dongusu arka planda async calisir;
/// olaylar arka plan thread'lerinden tetiklenir.
/// </summary>
public sealed class ChatClient : IDisposable
{
    private TcpClient? _tcp;
    private StreamWriter? _writer;
    private CancellationTokenSource? _cts;

    public bool IsConnected { get; private set; }

    public string UserName { get; private set; } = string.Empty;

    /// <summary>Sunucudan paket geldiginde tetiklenir.</summary>
    public event Action<Packet>? PacketReceived;

    /// <summary>Baglanti beklenmedik sekilde koptugunda (mesajla) tetiklenir.</summary>
    public event Action<string>? Disconnected;

    /// <summary>
    /// Sunucuya baglanir ve kullanici adiyla katilma istegi yollar.
    /// Hata durumunda istisna firlatmaz; hata metnini dondurur.
    /// </summary>
    public async Task<string?> ConnectAsync(string host, int port, string userName)
    {
        if (IsConnected) return "Zaten bağlısınız.";

        if (!ChatDefaults.IsValidUserName(userName, out string nameError)) return nameError;

        try
        {
            _tcp = new TcpClient();
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await _tcp.ConnectAsync(host, port, timeout.Token);

            var stream = _tcp.GetStream();
            _writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
            var reader = new StreamReader(stream, Encoding.UTF8);

            UserName = userName.Trim();
            IsConnected = true;
            _cts = new CancellationTokenSource();
            _ = ReceiveLoopAsync(reader, _cts.Token);

            await SendAsync(new Packet(PacketType.Join, UserName));
            return null;
        }
        catch (OperationCanceledException)
        {
            Cleanup();
            return "Bağlantı zaman aşımına uğradı. Sunucu adresini ve portu kontrol edin.";
        }
        catch (Exception ex) when (ex is SocketException or IOException or ArgumentException)
        {
            Cleanup();
            return "Sunucuya bağlanılamadı: " + ex.Message;
        }
    }

    /// <summary>Paketi sunucuya gonderir; bagli degilse veya hata olursa false doner.</summary>
    public async Task<bool> SendAsync(Packet packet)
    {
        if (!IsConnected || _writer is null) return false;

        try
        {
            await _writer.WriteLineAsync(packet.Serialize());
            return true;
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or SocketException)
        {
            HandleLost("Mesaj gönderilemedi: " + ex.Message);
            return false;
        }
    }

    /// <summary>Baglantiyi kullanici istegiyle kapatir.</summary>
    public void Disconnect()
    {
        Cleanup();
    }

    private async Task ReceiveLoopAsync(StreamReader reader, CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                string? line = await reader.ReadLineAsync(token);
                if (line is null)
                {
                    HandleLost("Sunucu bağlantıyı kapattı.");
                    return;
                }

                if (Packet.TryParse(line, out Packet? packet) && packet is not null)
                {
                    PacketReceived?.Invoke(packet);
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or SocketException)
        {
            if (!token.IsCancellationRequested) HandleLost("Bağlantı koptu: " + ex.Message);
        }
    }

    private void HandleLost(string reason)
    {
        if (!IsConnected) return;
        Cleanup();
        Disconnected?.Invoke(reason);
    }

    private void Cleanup()
    {
        IsConnected = false;
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        try { _tcp?.Close(); } catch (SocketException) { }
        _tcp = null;
        _writer = null;
    }

    public void Dispose() => Cleanup();
}
