using ChatApp.Client.Network;
using ChatApp.Common;

namespace ChatApp.Client.Forms;

/// <summary>Istemci penceresi: baglan, mesaj gonder, baglantiyi kes.</summary>
public partial class MainForm : Form
{
    private readonly ChatClient _client = new();

    public MainForm()
    {
        InitializeComponent();

        _client.PacketReceived += p => UiInvoke(() => HandlePacket(p));
        _client.Disconnected += reason => UiInvoke(() =>
        {
            AppendLine($"[{DateTime.Now:HH:mm:ss}] Bağlantı kesildi: {reason}", Color.Firebrick);
            UpdateUi();
        });
    }

    private void UiInvoke(Action action)
    {
        if (IsDisposed || !IsHandleCreated) return;
        try { BeginInvoke(action); }
        catch (InvalidOperationException) { /* form kapanirken */ }
    }

    #region Dugme olaylari

    private async void btnConnect_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtPort.Text, out int port) || port is < 1 or > 65535)
        {
            MessageBox.Show(this, "Port 1-65535 arasında bir sayı olmalıdır.", "Geçersiz port",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnConnect.Enabled = false;
        lblStatus.Text = "Bağlanılıyor...";

        string? error = await _client.ConnectAsync(txtHost.Text.Trim(), port, txtUser.Text);
        if (error is not null)
        {
            MessageBox.Show(this, error, "Bağlantı hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            AppendLine($"[{DateTime.Now:HH:mm:ss}] {error}", Color.Firebrick);
        }
        UpdateUi();
        if (_client.IsConnected) txtMessage.Focus();
    }

    private void btnDisconnect_Click(object? sender, EventArgs e)
    {
        _client.Disconnect();
        AppendLine($"[{DateTime.Now:HH:mm:ss}] Bağlantı kesildi.", Color.Gray);
        lstUsers.Items.Clear();
        UpdateUi();
    }

    private async void btnSend_Click(object? sender, EventArgs e) => await SendCurrentMessageAsync();

    private async void txtMessage_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await SendCurrentMessageAsync();
        }
    }

    /// <summary>Kullanici listesinde cift tiklayinca ozel mesaj komutunu hazirlar.</summary>
    private void lstUsers_DoubleClick(object? sender, EventArgs e)
    {
        if (lstUsers.SelectedItem is string name && name != _client.UserName && txtMessage.Enabled)
        {
            txtMessage.Text = $"/w {name} ";
            txtMessage.Focus();
            txtMessage.SelectionStart = txtMessage.TextLength;
        }
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e) => _client.Dispose();

    #endregion

    private async Task SendCurrentMessageAsync()
    {
        string text = txtMessage.Text.Trim();
        if (text.Length == 0 || !_client.IsConnected) return;

        Packet packet;
        if (text.StartsWith("/w ", StringComparison.OrdinalIgnoreCase))
        {
            string[] parts = text.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3)
            {
                lblStatus.Text = "Kullanım: /w kullanıcı mesaj";
                return;
            }
            packet = new Packet(PacketType.Private, parts[1], parts[2]);
        }
        else
        {
            packet = new Packet(PacketType.Message, text);
        }

        if (await _client.SendAsync(packet))
        {
            txtMessage.Clear();
        }
    }

    private void HandlePacket(Packet p)
    {
        switch (p.Type)
        {
            case PacketType.Message:
                AppendMessage(p.Field(0), p.Field(1), p.Field(2), Color.Black);
                break;

            case PacketType.Private:
                string from = p.Field(1), to = p.Field(2);
                string label = from == _client.UserName ? $"(özel → {to})" : "(özel)";
                AppendMessage(p.Field(0), from + " " + label, p.Field(3), Color.DarkMagenta);
                break;

            case PacketType.System:
                AppendLine($"[{DateTime.Now:HH:mm:ss}] * {p.Field(0)}", Color.SeaGreen);
                break;

            case PacketType.Users:
                lstUsers.Items.Clear();
                foreach (string name in p.Field(0).Split(',', StringSplitOptions.RemoveEmptyEntries))
                    lstUsers.Items.Add(name);
                lblUsers.Text = $"Çevrimiçi ({lstUsers.Items.Count})";
                break;

            case PacketType.Reject:
                _client.Disconnect();
                MessageBox.Show(this, p.Field(0), "Katılım reddedildi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AppendLine($"[{DateTime.Now:HH:mm:ss}] Katılım reddedildi: {p.Field(0)}", Color.Firebrick);
                UpdateUi();
                break;
        }
    }

    private void AppendMessage(string isoTime, string user, string text, Color color)
    {
        string time = DateTime.TryParse(isoTime, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime t)
            ? t.ToString("HH:mm:ss") : DateTime.Now.ToString("HH:mm:ss");
        bool mine = user.StartsWith(_client.UserName + " ") || user == _client.UserName;
        AppendLine($"[{time}] {user}: {text}", mine && color == Color.Black ? Color.RoyalBlue : color);
    }

    private void AppendLine(string line, Color color)
    {
        rtbChat.SelectionStart = rtbChat.TextLength;
        rtbChat.SelectionColor = color;
        rtbChat.AppendText(line + Environment.NewLine);
        rtbChat.ScrollToCaret();
    }

    private void UpdateUi()
    {
        bool on = _client.IsConnected;
        btnConnect.Enabled = !on;
        btnDisconnect.Enabled = on;
        txtHost.Enabled = txtPort.Enabled = txtUser.Enabled = !on;
        txtMessage.Enabled = btnSend.Enabled = on;
        lblIndicator.Text = on ? $"● BAĞLI: {_client.UserName}" : "● BAĞLI DEĞİL";
        lblIndicator.BackColor = on ? Color.FromArgb(46, 160, 67) : Color.Gray;
        lblStatus.Text = on ? $"{txtHost.Text}:{txtPort.Text} sunucusuna bağlı." : "Bağlı değil.";
        if (!on)
        {
            lstUsers.Items.Clear();
            lblUsers.Text = "Çevrimiçi (0)";
        }
    }
}
