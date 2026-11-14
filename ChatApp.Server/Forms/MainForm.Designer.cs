namespace ChatApp.Server.Forms;

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
        btnStop = new Button();
        btnStart = new Button();
        txtPort = new TextBox();
        lblPort = new Label();
        split = new TableLayoutPanel();
        pnl1 = new Panel();
        pnl2 = new Panel();
        lvClients = new ListView();
        colUser = new ColumnHeader();
        colIp = new ColumnHeader();
        colTime = new ColumnHeader();
        lblClients = new Label();
        lstLog = new ListBox();
        lblLog = new Label();
        statusStrip = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        panelTop.SuspendLayout();
        pnl1.SuspendLayout();
        pnl2.SuspendLayout();
        split.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // panelTop
        // 
        panelTop.BackColor = Color.FromArgb(45, 52, 64);
        panelTop.Controls.Add(lblIndicator);
        panelTop.Controls.Add(btnStop);
        panelTop.Controls.Add(btnStart);
        panelTop.Controls.Add(txtPort);
        panelTop.Controls.Add(lblPort);
        panelTop.Dock = DockStyle.Top;
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(860, 56);
        panelTop.TabIndex = 0;
        // 
        // lblPort
        // 
        lblPort.AutoSize = true;
        lblPort.Font = new Font("Segoe UI", 10F);
        lblPort.ForeColor = Color.White;
        lblPort.Location = new Point(14, 18);
        lblPort.Name = "lblPort";
        lblPort.Text = "Port:";
        // 
        // txtPort
        // 
        txtPort.Font = new Font("Segoe UI", 10F);
        txtPort.Location = new Point(56, 14);
        txtPort.Name = "txtPort";
        txtPort.Size = new Size(70, 25);
        txtPort.TabIndex = 0;
        txtPort.Text = "5000";
        // 
        // btnStart
        // 
        btnStart.BackColor = Color.FromArgb(46, 160, 67);
        btnStart.FlatStyle = FlatStyle.Flat;
        btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnStart.ForeColor = Color.White;
        btnStart.Location = new Point(140, 11);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(130, 34);
        btnStart.TabIndex = 1;
        btnStart.Text = "▶ Sunucuyu Başlat";
        btnStart.UseVisualStyleBackColor = false;
        btnStart.Click += btnStart_Click;
        // 
        // btnStop
        // 
        btnStop.BackColor = Color.FromArgb(207, 34, 46);
        btnStop.Enabled = false;
        btnStop.FlatStyle = FlatStyle.Flat;
        btnStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnStop.ForeColor = Color.White;
        btnStop.Location = new Point(278, 11);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(110, 34);
        btnStop.TabIndex = 2;
        btnStop.Text = "■ Durdur";
        btnStop.UseVisualStyleBackColor = false;
        btnStop.Click += btnStop_Click;
        // 
        // lblIndicator
        // 
        lblIndicator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblIndicator.BackColor = Color.Gray;
        lblIndicator.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        lblIndicator.ForeColor = Color.White;
        lblIndicator.Location = new Point(650, 11);
        lblIndicator.Name = "lblIndicator";
        lblIndicator.Size = new Size(198, 34);
        lblIndicator.Text = "● SUNUCU KAPALI";
        lblIndicator.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // split
        // 
        split.Dock = DockStyle.Fill;
        split.Location = new Point(0, 56);
        split.Name = "split";
        pnl1.Controls.Add(lvClients);
        pnl1.Controls.Add(lblClients);
        pnl1.Padding = new Padding(8);
        pnl2.Controls.Add(lstLog);
        pnl2.Controls.Add(lblLog);
        pnl2.Padding = new Padding(8);
        split.Size = new Size(860, 450);
        split.ColumnCount = 2;
        split.RowCount = 1;
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47F));
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 53F));
        split.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        split.Controls.Add(pnl1, 0, 0);
        split.Controls.Add(pnl2, 1, 0);
        pnl1.Dock = DockStyle.Fill;
        pnl2.Dock = DockStyle.Fill;
        split.TabIndex = 1;
        // 
        // lblClients
        // 
        lblClients.Dock = DockStyle.Top;
        lblClients.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblClients.Name = "lblClients";
        lblClients.Size = new Size(384, 24);
        lblClients.Text = "Bağlı İstemciler (0)";
        // 
        // lvClients
        // 
        lvClients.Columns.AddRange(new ColumnHeader[] { colUser, colIp, colTime });
        lvClients.Dock = DockStyle.Fill;
        lvClients.FullRowSelect = true;
        lvClients.GridLines = true;
        lvClients.Name = "lvClients";
        lvClients.TabIndex = 1;
        lvClients.UseCompatibleStateImageBehavior = false;
        lvClients.View = View.Details;
        // 
        // colUser
        // 
        colUser.Text = "Kullanıcı";
        colUser.Width = 120;
        // 
        // colIp
        // 
        colIp.Text = "IP : Port";
        colIp.Width = 150;
        // 
        // colTime
        // 
        colTime.Text = "Bağlanma";
        colTime.Width = 90;
        // 
        // lblLog
        // 
        lblLog.Dock = DockStyle.Top;
        lblLog.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblLog.Name = "lblLog";
        lblLog.Size = new Size(436, 24);
        lblLog.Text = "Sunucu Günlüğü";
        // 
        // lstLog
        // 
        lstLog.Dock = DockStyle.Fill;
        lstLog.Font = new Font("Consolas", 9.5F);
        lstLog.HorizontalScrollbar = true;
        lstLog.IntegralHeight = false;
        lstLog.Name = "lstLog";
        lstLog.TabIndex = 1;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip.Name = "statusStrip";
        statusStrip.TabIndex = 2;
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Text = "Sunucu kapalı.";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(860, 528);
        Controls.Add(split);
        Controls.Add(panelTop);
        Controls.Add(statusStrip);
        MinimumSize = new Size(720, 420);
        Name = "MainForm";
        StartPosition = FormStartPosition.Manual;
        Text = "Chat Sunucusu - YMH219";
        FormClosing += MainForm_FormClosing;
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        pnl1.ResumeLayout(false);
        pnl2.ResumeLayout(false);
        split.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel panelTop;
    private Label lblPort;
    private TextBox txtPort;
    private Button btnStart;
    private Button btnStop;
    private Label lblIndicator;
    private TableLayoutPanel split;
    private Panel pnl1;
    private Panel pnl2;
    private Label lblClients;
    private ListView lvClients;
    private ColumnHeader colUser;
    private ColumnHeader colIp;
    private ColumnHeader colTime;
    private Label lblLog;
    private ListBox lstLog;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblStatus;
}



