namespace ChatApp.Client.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        panelTop = new Panel();
        lblIndicator = new Label();
        btnDisconnect = new Button();
        btnConnect = new Button();
        txtUser = new TextBox();
        lblUser = new Label();
        txtPort = new TextBox();
        lblPort = new Label();
        txtHost = new TextBox();
        lblHost = new Label();
        panelBottom = new Panel();
        btnSend = new Button();
        txtMessage = new TextBox();
        panelRight = new Panel();
        lstUsers = new ListBox();
        lblUsers = new Label();
        rtbChat = new RichTextBox();
        statusStrip = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        panelTop.SuspendLayout();
        panelBottom.SuspendLayout();
        panelRight.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // panelTop
        // 
        panelTop.BackColor = Color.FromArgb(45, 52, 64);
        panelTop.Controls.Add(lblIndicator);
        panelTop.Controls.Add(btnDisconnect);
        panelTop.Controls.Add(btnConnect);
        panelTop.Controls.Add(txtUser);
        panelTop.Controls.Add(lblUser);
        panelTop.Controls.Add(txtPort);
        panelTop.Controls.Add(lblPort);
        panelTop.Controls.Add(txtHost);
        panelTop.Controls.Add(lblHost);
        panelTop.Dock = DockStyle.Top;
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(820, 90);
        panelTop.TabIndex = 0;
        // 
        // lblHost
        // 
        lblHost.AutoSize = true;
        lblHost.Font = new Font("Segoe UI", 9.5F);
        lblHost.ForeColor = Color.White;
        lblHost.Location = new Point(12, 14);
        lblHost.Name = "lblHost";
        lblHost.Text = "Sunucu:";
        // 
        // txtHost
        // 
        txtHost.Font = new Font("Segoe UI", 10F);
        txtHost.Location = new Point(70, 10);
        txtHost.Name = "txtHost";
        txtHost.Size = new Size(130, 25);
        txtHost.TabIndex = 0;
        txtHost.Text = "127.0.0.1";
        // 
        // lblPort
        // 
        lblPort.AutoSize = true;
        lblPort.Font = new Font("Segoe UI", 9.5F);
        lblPort.ForeColor = Color.White;
        lblPort.Location = new Point(212, 14);
        lblPort.Name = "lblPort";
        lblPort.Text = "Port:";
        // 
        // txtPort
        // 
        txtPort.Font = new Font("Segoe UI", 10F);
        txtPort.Location = new Point(250, 10);
        txtPort.Name = "txtPort";
        txtPort.Size = new Size(60, 25);
        txtPort.TabIndex = 1;
        txtPort.Text = "5000";
        // 
        // lblUser
        // 
        lblUser.AutoSize = true;
        lblUser.Font = new Font("Segoe UI", 9.5F);
        lblUser.ForeColor = Color.White;
        lblUser.Location = new Point(322, 14);
        lblUser.Name = "lblUser";
        lblUser.Text = "Kullanıcı adı:";
        // 
        // txtUser
        // 
        txtUser.Font = new Font("Segoe UI", 10F);
        txtUser.Location = new Point(410, 10);
        txtUser.Name = "txtUser";
        txtUser.Size = new Size(130, 25);
        txtUser.TabIndex = 2;
        // 
        // btnConnect
        // 
        btnConnect.BackColor = Color.FromArgb(46, 160, 67);
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(12, 46);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(110, 34);
        btnConnect.TabIndex = 3;
        btnConnect.Text = "Bağlan";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += btnConnect_Click;
        // 
        // btnDisconnect
        // 
        btnDisconnect.BackColor = Color.FromArgb(207, 34, 46);
        btnDisconnect.Enabled = false;
        btnDisconnect.FlatStyle = FlatStyle.Flat;
        btnDisconnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnDisconnect.ForeColor = Color.White;
        btnDisconnect.Location = new Point(130, 46);
        btnDisconnect.Name = "btnDisconnect";
        btnDisconnect.Size = new Size(140, 34);
        btnDisconnect.TabIndex = 4;
        btnDisconnect.Text = "Bağlantıyı Kes";
        btnDisconnect.UseVisualStyleBackColor = false;
        btnDisconnect.Click += btnDisconnect_Click;
        // 
        // lblIndicator
        // 
        lblIndicator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblIndicator.BackColor = Color.Gray;
        lblIndicator.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        lblIndicator.ForeColor = Color.White;
        lblIndicator.Location = new Point(610, 46);
        lblIndicator.Name = "lblIndicator";
        lblIndicator.Size = new Size(198, 34);
        lblIndicator.Text = "● BAĞLI DEĞİL";
        lblIndicator.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // panelBottom
        // 
        panelBottom.BackColor = Color.FromArgb(236, 239, 244);
        panelBottom.Controls.Add(txtMessage);
        panelBottom.Controls.Add(btnSend);
        panelBottom.Dock = DockStyle.Bottom;
        panelBottom.Name = "panelBottom";
        panelBottom.Size = new Size(820, 52);
        panelBottom.TabIndex = 2;
        // 
        // txtMessage
        // 
        txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.Enabled = false;
        txtMessage.Font = new Font("Segoe UI", 11F);
        txtMessage.Location = new Point(12, 12);
        txtMessage.MaxLength = 500;
        txtMessage.Name = "txtMessage";
        txtMessage.PlaceholderText = "Mesaj yazın... (özel mesaj: /w kullanıcı mesaj)";
        txtMessage.Size = new Size(690, 27);
        txtMessage.TabIndex = 0;
        txtMessage.KeyDown += txtMessage_KeyDown;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSend.Enabled = false;
        btnSend.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnSend.Location = new Point(712, 10);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(96, 32);
        btnSend.TabIndex = 1;
        btnSend.Text = "Gönder";
        btnSend.UseVisualStyleBackColor = true;
        btnSend.Click += btnSend_Click;
        // 
        // panelRight
        // 
        panelRight.Controls.Add(lstUsers);
        panelRight.Controls.Add(lblUsers);
        panelRight.Dock = DockStyle.Right;
        panelRight.Padding = new Padding(6);
        panelRight.Name = "panelRight";
        panelRight.Size = new Size(190, 360);
        panelRight.TabIndex = 1;
        // 
        // lblUsers
        // 
        lblUsers.Dock = DockStyle.Top;
        lblUsers.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblUsers.Name = "lblUsers";
        lblUsers.Size = new Size(178, 24);
        lblUsers.Text = "Çevrimiçi (0)";
        // 
        // lstUsers
        // 
        lstUsers.Dock = DockStyle.Fill;
        lstUsers.Font = new Font("Segoe UI", 10F);
        lstUsers.IntegralHeight = false;
        lstUsers.Name = "lstUsers";
        lstUsers.TabIndex = 1;
        lstUsers.DoubleClick += lstUsers_DoubleClick;
        // 
        // rtbChat
        // 
        rtbChat.BackColor = Color.White;
        rtbChat.BorderStyle = BorderStyle.None;
        rtbChat.Dock = DockStyle.Fill;
        rtbChat.Font = new Font("Segoe UI", 10.5F);
        rtbChat.Name = "rtbChat";
        rtbChat.ReadOnly = true;
        rtbChat.TabIndex = 3;
        rtbChat.TabStop = false;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip.Name = "statusStrip";
        statusStrip.TabIndex = 4;
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Text = "Bağlı değil.";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(820, 540);
        Controls.Add(rtbChat);
        Controls.Add(panelRight);
        Controls.Add(panelBottom);
        Controls.Add(panelTop);
        Controls.Add(statusStrip);
        MinimumSize = new Size(760, 460);
        Name = "MainForm";
        StartPosition = FormStartPosition.Manual;
        Text = "Chat İstemcisi - YMH219";
        FormClosing += MainForm_FormClosing;
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        panelBottom.ResumeLayout(false);
        panelBottom.PerformLayout();
        panelRight.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel panelTop;
    private Label lblHost;
    private TextBox txtHost;
    private Label lblPort;
    private TextBox txtPort;
    private Label lblUser;
    private TextBox txtUser;
    private Button btnConnect;
    private Button btnDisconnect;
    private Label lblIndicator;
    private Panel panelBottom;
    private TextBox txtMessage;
    private Button btnSend;
    private Panel panelRight;
    private Label lblUsers;
    private ListBox lstUsers;
    private RichTextBox rtbChat;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblStatus;
}


