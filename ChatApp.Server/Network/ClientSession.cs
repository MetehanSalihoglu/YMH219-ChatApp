using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatApp.Common;

namespace ChatApp.Server.Network;

/// <summary>Sunucuya bagli tek bir istemcinin bilgileri ve baglantisi.</summary>
public sealed class ClientSession : IDisposable
{
    private readonly TcpClient _tcp;
    private readonly StreamWriter _writer;
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    public ClientSession(TcpClient tcp)
    {
        _tcp = tcp;
        var stream = tcp.GetStream();
        Reader = new StreamReader(stream, Encoding.UTF8);
        _writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
        RemoteEndPoint = tcp.Client.RemoteEndPoint as IPEndPoint;
    }

    public StreamReader Reader { get; }

    public string UserName { get; set; } = string.Empty;

    public IPEndPoint? RemoteEndPoint { get; }

    public DateTime ConnectedAt { get; } = DateTime.Now;

    /// <summary>Katilma onaylandiysa true.</summary>
    public bool IsJoined => UserName.Length > 0;

    /// <summary>Paketi istemciye gonderir; hata olursa false doner.</summary>
    public async Task<bool> SendAsync(Packet packet)
    {
        await _writeLock.WaitAsync();
        try
        {
            await _writer.WriteLineAsync(packet.Serialize());
            return true;
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or SocketException)
        {
            return false;
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public void Dispose()
    {
        try { _tcp.Close(); } catch (SocketException) { }
        _writeLock.Dispose();
    }
}
