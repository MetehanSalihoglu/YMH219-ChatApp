using System.Net;
using System.Net.Sockets;
using ChatApp.Common;

namespace ChatApp.Server.Network;

/// <summary>
/// TcpListener tabanli sohbet sunucusu. Her istemci icin ayri bir async dongu calisir.
/// Olaylar arka plan thread'lerinden tetiklenir; arayuz tarafinda Invoke gerekir.
/// </summary>
public sealed class ChatServer : IDisposable
{
    private readonly List<ClientSession> _sessions = new();
    private readonly object _lock = new();
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;

    public bool IsRunning { get; private set; }

    public int Port { get; private set; }

    /// <summary>Sunucu gunlugu (baglanti, ayrilma, hata).</summary>
    public event Action<string>? Log;

    /// <summary>Genel sohbet mesaji: (zaman, gonderen, metin).</summary>
    public event Action<DateTime, string, string>? MessageReceived;

    /// <summary>Baglanan istemci listesi degistiginde tetiklenir.</summary>
    public event Action? ClientsChanged;

    public IReadOnlyList<ClientSession> GetClients()
    {
        lock (_lock)
        {
            return _sessions.Where(s => s.IsJoined).ToList();
        }
    }

    /// <summary>Sunucuyu baslatir. Port kullanimdaysa istisna yerine false doner.</summary>
    public bool TryStart(int port, out string? error)
    {
        if (IsRunning)
        {
            error = "Sunucu zaten çalışıyor.";
            return false;
        }

        try
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
        }
        catch (Exception ex) when (ex is SocketException or ArgumentOutOfRangeException)
        {
            error = ex is SocketException
                ? $"Port {port} açılamadı (başka bir uygulama kullanıyor olabilir): {ex.Message}"
                : "Geçersiz port numarası.";
            _listener = null;
            return false;
        }

        Port = port;
        IsRunning = true;
        _cts = new CancellationTokenSource();
        _ = AcceptLoopAsync(_listener, _cts.Token);
        Log?.Invoke($"Sunucu {port} portunda dinlemeye başladı.");
        error = null;
        return true;
    }

    public void Stop()
    {
        if (!IsRunning) return;

        IsRunning = false;
        _cts?.Cancel();
        _listener?.Stop();

        List<ClientSession> copy;
        lock (_lock)
        {
            copy = _sessions.ToList();
            _sessions.Clear();
        }
        foreach (ClientSession s in copy) s.Dispose();

        ClientsChanged?.Invoke();
        Log?.Invoke("Sunucu durduruldu.");
    }

    private async Task AcceptLoopAsync(TcpListener listener, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                TcpClient tcp = await listener.AcceptTcpClientAsync(token);
                _ = HandleClientAsync(new ClientSession(tcp), token);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex) when (ex is SocketException or ObjectDisposedException or InvalidOperationException)
            {
                if (!token.IsCancellationRequested) Log?.Invoke("Bağlantı kabul hatası: " + ex.Message);
                break;
            }
        }
    }

    private async Task HandleClientAsync(ClientSession session, CancellationToken token)
    {
        lock (_lock) _sessions.Add(session);

        try
        {
            while (!token.IsCancellationRequested)
            {
                string? line = await session.Reader.ReadLineAsync(token);
                if (line is null) break;   // istemci baglantiyi kapatti

                if (Packet.TryParse(line, out Packet? packet) && packet is not null)
                {
                    await ProcessPacketAsync(session, packet);
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or SocketException)
        {
            Log?.Invoke($"{Describe(session)} bağlantısı koptu: {ex.Message}");
        }
        finally
        {
            await RemoveSessionAsync(session);
        }
    }

    private async Task ProcessPacketAsync(ClientSession session, Packet packet)
    {
        if (!session.IsJoined)
        {
            if (packet.Type == PacketType.Join) await HandleJoinAsync(session, packet.Field(0).Trim());
            return;
        }

        string text = packet.Field(packet.Type == PacketType.Private ? 1 : 0).Trim();
        if (text.Length == 0) return;
        if (text.Length > ChatDefaults.MaxMessageLength) text = text[..ChatDefaults.MaxMessageLength];

        switch (packet.Type)
        {
            case PacketType.Message:
                DateTime now = DateTime.Now;
                MessageReceived?.Invoke(now, session.UserName, text);
                await BroadcastAsync(new Packet(PacketType.Message, now.ToString("o"), session.UserName, text));
                break;

            case PacketType.Private:
                await HandlePrivateAsync(session, packet.Field(0), text);
                break;
        }
    }

    private async Task HandleJoinAsync(ClientSession session, string name)
    {
        if (!ChatDefaults.IsValidUserName(name, out string error))
        {
            await session.SendAsync(new Packet(PacketType.Reject, error));
            return;
        }

        bool taken;
        lock (_lock)
        {
            taken = _sessions.Any(s => s.IsJoined && s.UserName.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (!taken) session.UserName = name;
        }

        if (taken)
        {
            await session.SendAsync(new Packet(PacketType.Reject, "Bu kullanıcı adı zaten kullanımda."));
            return;
        }

        Log?.Invoke($"{Describe(session)} katıldı.");
        ClientsChanged?.Invoke();
        await session.SendAsync(new Packet(PacketType.System, $"Hoş geldin {name}!"));
        await BroadcastAsync(new Packet(PacketType.System, $"{name} sohbete katıldı."));
        await BroadcastUserListAsync();
    }

    private async Task HandlePrivateAsync(ClientSession from, string target, string text)
    {
        ClientSession? to;
        lock (_lock)
        {
            to = _sessions.FirstOrDefault(s => s.IsJoined && s.UserName.Equals(target, StringComparison.OrdinalIgnoreCase));
        }

        if (to is null)
        {
            await from.SendAsync(new Packet(PacketType.System, $"'{target}' çevrimiçi değil."));
            return;
        }

        var packet = new Packet(PacketType.Private, DateTime.Now.ToString("o"), from.UserName, to.UserName, text);
        await to.SendAsync(packet);
        await from.SendAsync(packet);
        Log?.Invoke($"Özel mesaj: {from.UserName} → {to.UserName}");
    }

    private async Task RemoveSessionAsync(ClientSession session)
    {
        bool wasJoined = session.IsJoined;
        lock (_lock) _sessions.Remove(session);
        session.Dispose();

        if (wasJoined && IsRunning)
        {
            Log?.Invoke($"{session.UserName} ayrıldı.");
            ClientsChanged?.Invoke();
            await BroadcastAsync(new Packet(PacketType.System, $"{session.UserName} sohbetten ayrıldı."));
            await BroadcastUserListAsync();
        }
    }

    private Task BroadcastUserListAsync()
    {
        string names = string.Join(",", GetClients().Select(c => c.UserName));
        return BroadcastAsync(new Packet(PacketType.Users, names));
    }

    private async Task BroadcastAsync(Packet packet)
    {
        foreach (ClientSession s in GetClients())
        {
            await s.SendAsync(packet);
        }
    }

    private static string Describe(ClientSession s) =>
        $"{(s.IsJoined ? s.UserName : "(katılmamış)")} [{s.RemoteEndPoint}]";

    public void Dispose()
    {
        Stop();
        _cts?.Dispose();
    }
}
