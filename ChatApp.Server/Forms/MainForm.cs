using System.Text;
using ChatApp.Common;
using ChatApp.Server.Network;

namespace ChatApp.Server.Forms;

/// <summary>Sunucu penceresi: baslat/durdur, bagli istemciler ve gunluk.</summary>
public partial class MainForm : Form
{
    private readonly ChatServer _server = new();
    private readonly string _historyPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YMH219-ChatApp", "chat_history.txt");

    public MainForm()
    {
        InitializeComponent();

        _server.Log += text => UiInvoke(() => AddLog(text));
        _server.ClientsChanged += () => UiInvoke(RefreshClients);
        _server.MessageReceived += (time, user, text) => UiInvoke(() =>
        {
            AddLog($"{user}: {text}");
            SaveHistory(time, user, text);
        });
    }

    /// <summary>Arka plan thread'lerinden gelen olaylari arayuz thread'ine tasir.</summary>
    private void UiInvoke(Action action)
    {
        if (IsDisposed || !IsHandleCreated) return;
        try { BeginInvoke(action); }
        catch (InvalidOperationException) { /* form kapanirken */ }
    }

    private void btnStart_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtPort.Text, out int port) || port is < 1 or > 65535)
        {
            MessageBox.Show(this, "Port 1-65535 arasında bir sayı olmalıdır.", "Geçersiz port",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_server.TryStart(port, out string? error))
        {
            MessageBox.Show(this, error, "Sunucu başlatılamadı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        UpdateUi();
    }

    private void btnStop_Click(object? sender, EventArgs e)
    {
        _server.Stop();
        UpdateUi();
    }

    private void UpdateUi()
    {
        bool on = _server.IsRunning;
        btnStart.Enabled = !on;
        btnStop.Enabled = on;
        txtPort.Enabled = !on;
        lblIndicator.Text = on ? "● SUNUCU ÇALIŞIYOR" : "● SUNUCU KAPALI";
        lblIndicator.BackColor = on ? Color.FromArgb(46, 160, 67) : Color.Gray;
        lblStatus.Text = on ? $"Port {_server.Port} dinleniyor." : "Sunucu kapalı.";
        RefreshClients();
    }

    private void RefreshClients()
    {
        lvClients.BeginUpdate();
        lvClients.Items.Clear();
        foreach (ClientSession c in _server.GetClients())
        {
            lvClients.Items.Add(new ListViewItem(new[]
            {
                c.UserName, c.RemoteEndPoint?.ToString() ?? "-", c.ConnectedAt.ToString("HH:mm:ss")
            }));
        }
        lvClients.EndUpdate();
        lblClients.Text = $"Bağlı İstemciler ({lvClients.Items.Count})";
    }

    private void AddLog(string text)
    {
        lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {text}");
        lstLog.TopIndex = lstLog.Items.Count - 1;
    }

    /// <summary>Mesaj gecmisini dosyaya ekler; hata sunucuyu durdurmaz.</summary>
    private void SaveHistory(DateTime time, string user, string text)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_historyPath)!);
            File.AppendAllText(_historyPath, $"[{time:yyyy-MM-dd HH:mm:ss}] {user}: {text}{Environment.NewLine}", Encoding.UTF8);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            lblStatus.Text = "Geçmiş dosyaya yazılamadı: " + ex.Message;
        }
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e) => _server.Dispose();
}
