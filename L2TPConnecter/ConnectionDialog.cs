using L2TPConnecter.Properties;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L2TPConnecter
{
    public partial class ConnectionDialog : Form
    {
        public VpnSettingModel VpnSetting { get; set; }

        public ConnectionDialog()
        {
            InitializeComponent();
        }

        private void ConnectionDialog_Load(object sender, EventArgs e)
        {
            IsCollapsed = false;
            titleLabel.Text = "";
            label1.Text = "";
        }

        private bool isProcessing = false;
        private async void ConnectionDialog_Shown(object sender, EventArgs e)
        {
            if (VpnSetting == null) return;
            isProcessing = true;
            closeButton.Enabled = false;
            this.ControlBox = false;

            logTextBox.Clear();

            progressBar1.Style = ProgressBarStyle.Marquee; // 開始時
            var result = false;

            if (!VpnSetting.IsConnected)
            {
                titleLabel.Text = Resources.ResourceManager.GetString("Connecting");
                // await and ignore the result is allowed, but capture it if needed
                result = await ConnectVpn(VpnSetting);
            }
            else
            {
                titleLabel.Text = Resources.ResourceManager.GetString("Disconnecting");
                result = await DisconnectVpn(VpnSetting);
            }

            progressBar1.Style = ProgressBarStyle.Blocks;  // 完了時

            isProcessing = false;

            this.ControlBox = true;
            closeButton.Enabled = true;
            closeButton.Refresh();

            if (result)//成功した場合3秒後に閉じる
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    try
                    {
                        if (this.IsDisposed || this.Disposing) return;
                        if (this.IsHandleCreated)
                        {
                            this.BeginInvoke((Action)(() =>
                            {
                                if (!this.IsDisposed && !this.Disposing)
                                    this.Close();
                            }));
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // ハンドルが無効／既に破棄されている場合、安全に無視できる
                    }
                });
            }
        }

        private async Task<bool> ConnectVpn(VpnSettingModel model)
        {
            var script = PowerShellScript.GetConnectScript(model);

            await PowerShell.Run(script,
                output =>AppendLog(output, Color.WhiteSmoke),
                error =>AppendLog(error, Color.Pink)
                );

            await ConnectionCheck(model);

            if (!model.IsConnected)
            {
                script = PowerShellScript.GetDisconnectScript(model);
                await PowerShell.Run(script, output => { }, error => { });

                titleLabel.Text = Resources.ResourceManager.GetString("Error");
                return false;
            }
            else
            {
                titleLabel.Text = Resources.ResourceManager.GetString("ConnectionComplete");
                label1.Text = "";
                return true;
            }
        }

        private async Task<bool> DisconnectVpn(VpnSettingModel model)
        {
            var script = PowerShellScript.GetDisconnectScript(model);

            await PowerShell.Run(script,
                output => AppendLog(output, Color.WhiteSmoke),
                error => AppendLog(error, Color.Pink)
                );

            await ConnectionCheck(model);

            titleLabel.Text = Resources.ResourceManager.GetString("DisconnectionComplete");
            label1.Text = "";

            // Return true if disconnected (i.e. not connected)
            return !model.IsConnected;
        }
        private async Task ConnectionCheck(VpnSettingModel model)
        {
            var script = PowerShellScript.GetStatusScript(model);

            bool isConnected = false;

            await PowerShell.Run(script,
                output =>
                {
                    if (output.Contains("Connected"))
                        isConnected = true;
                },
                error =>
                {
                    // 必要ならログ出力やエラー処理
                });
            model.IsConnected = isConnected;
        }

        public void AppendLog(string text, Color color)
        {
            try
            {
                logTextBox.Invoke((Action)(() =>
                {
                    logTextBox.SelectionStart = logTextBox.TextLength;
                    logTextBox.SelectionLength = 0;
                    logTextBox.SelectionColor = color;
                    logTextBox.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] " + text + "\r\n");
                    logTextBox.SelectionColor = logTextBox.ForeColor; // 元に戻す
                }));
                label1.Invoke((Action)(() =>
                {
                    label1.Text = text;
                }));
            }
            catch { }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConnectionDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isProcessing) e.Cancel = true;

        }


        private bool isCollapsed = true;
        public bool IsCollapsed
        {
            get { return isCollapsed; }
            set
            {
                isCollapsed = value;
                if (isCollapsed)
                {
                    //this.Height = 500;
                    panel3.Visible = true;
                    collapseButton.Text = "▲";
                }
                else
                {
                    //this.Height = 190;
                    panel3.Visible = false;
                    collapseButton.Text = "▼";
                }
            }
        }

        private void collapseButton_Click(object sender, EventArgs e)
        {
            IsCollapsed=!IsCollapsed;
        }
    }
}
