using L2TPConnecter.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L2TPConnecter
{
    public partial class MainForm : Form
    {
        private Timer timer = new Timer();
        private List<VpnSettingModel> settings = new List<VpnSettingModel>();
        private BindingSource bindingSource = new BindingSource();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            settings = VpnSettingModel.LoadSettings("settings.json");
            var t = AllConnectionCheck(sender, e);

            dataGridView1.AutoGenerateColumns = false;

            bindingSource.DataSource = settings;
            dataGridView1.DataSource = bindingSource;

            timer.Tick += Timer_Tick;
            timer.Interval = 10000;
            timer.Start();

            _ = AllConnectionCheck(sender, e);

            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor;

            EnableDoubleBuffering(dataGridView1);
        }

        public static void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null,
                dgv,
                new object[] { true }
            );
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _ = AllConnectionCheck(sender, e);
        }

        private async Task AllConnectionCheck(object sender, EventArgs e)
        {
            foreach (var item in settings)
            {
                await ConnectionCheck(item);
            }

            //bindingSource.ResetBindings(false);

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
                });
            model.IsConnected = isConnected;
        }

        private void addSettingButton_Click(object sender, EventArgs e)
        {
            var frm = new SettingForm();
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.OK)
            {
                settings.Add(frm.Settings);
                bindingSource.ResetBindings(false);
            }

        }

        public async void DisconnectVpn(VpnSettingModel model)
        {

            var script = PowerShellScript.GetDisconnectScript(model);

            await PowerShell.Run(script,
                output => {  },
                error => { });

            _ = ConnectionCheck(model);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            foreach (var model in settings)
            {
                if (model.IsConnected)
                {
                    DisconnectVpn(model);
                }

            }
            VpnSettingModel.SaveSettings(settings, "settings.json");
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dataGridView1.Rows[e.RowIndex].DataBoundItem as VpnSettingModel;
            if (row == null) return;

            if (e.ColumnIndex == statusColumn.Index)
            {
                if (row.IsConnected)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.Value = Resources.ResourceManager.GetString("Connected");
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(102, 205, 170); // ミントグリーン
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);    // ダークグレー
                    e.CellStyle.Font = new System.Drawing.Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.Value = Resources.ResourceManager.GetString("Disconnected");
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(255, 182, 193); // ライトピンク
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(123, 36, 28);   // ブラウン
                    e.CellStyle.Font = new System.Drawing.Font(e.CellStyle.Font, System.Drawing.FontStyle.Italic);
                }
            }
        }

        private void editSettingButton_Click(object sender, EventArgs e)
        {
            var row = bindingSource.Current as VpnSettingModel;
            if (row == null) return;

            if (row.IsConnected) return;

            var newSetting = new VpnSettingModel();
            newSetting.CopyFrom(row);

            var frm = new SettingForm();
            frm.Settings = newSetting;
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                row.CopyFrom(frm.Settings);
                bindingSource.ResetBindings(false);
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            var row = bindingSource.Current as VpnSettingModel;
            if (row == null) return;

            var dlg = new ConnectionDialog();
            dlg.VpnSetting = row;
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog();
        }

        private void deleteSettingButton_Click(object sender, EventArgs e)
        {
            var row = bindingSource.Current as VpnSettingModel;
            if (row == null) return;

            if (row.IsConnected) return;

            var result = MessageBox.Show(
                Resources.ResourceManager.GetString("DeleteSettingConfirmationMessage").Replace("{VpnName}", row.VpnName),
                Resources.ResourceManager.GetString("Confirmation"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            settings.Remove(row);
            bindingSource.ResetBindings(false);
        }

        private int dragIndex = -1;

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = dataGridView1.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.RowHeader)
            {
                dragIndex = hit.RowIndex;

                dataGridView1.ClearSelection();
                dataGridView1.Rows[dragIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[dragIndex].Cells[0];

                dataGridView1.DoDragDrop(bindingSource[dragIndex], DragDropEffects.Move);
            }

        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));
            var hit = dataGridView1.HitTest(clientPoint.X, clientPoint.Y);

            if (clientPoint.Y < dataGridView1.ColumnHeadersHeight)
            {
                dropPreviewIndex = 0;
            }
            else if (hit.RowIndex == -1 || hit.RowIndex >= dataGridView1.Rows.Count)
            {
                dropPreviewIndex = dataGridView1.Rows.Count;
            }
            else
            {
                dropPreviewIndex = hit.RowIndex;
            }

            dataGridView1.Invalidate(); // 再描画
            e.Effect = DragDropEffects.Move;
        }
        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            int scrollTop = dataGridView1.FirstDisplayedScrollingRowIndex;

            Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));
            var hit = dataGridView1.HitTest(clientPoint.X, clientPoint.Y);

            int dropIndex;

            if (clientPoint.Y < dataGridView1.ColumnHeadersHeight)
            {
                // カラムヘッダーにドロップ
                dropIndex = 0;
            }
            else if (hit.RowIndex == -1 || hit.RowIndex >= settings.Count)
            {
                // 最下行の下にドロップ
                dropIndex = settings.Count;
            }
            else
            {
                // 通常の行
                dropIndex = hit.RowIndex;
            }

            if (dragIndex >= 0 && dragIndex != dropIndex)
            {
                var item = settings[dragIndex];
                settings.RemoveAt(dragIndex);
                if (dropIndex > dragIndex) dropIndex--;
                settings.Insert(dropIndex, item);

                bindingSource.ResetBindings(false);

                // 選択処理を描画後に遅延実行
                this.BeginInvoke(new Action(() =>
                {
                    dataGridView1.ClearSelection();

                    if (dropIndex >= 0 && dropIndex < dataGridView1.Rows.Count)
                    {
                        dataGridView1.Rows[dropIndex].Selected = true;
                        dataGridView1.CurrentCell = dataGridView1.Rows[dropIndex].Cells[0];
                        dataGridView1.FirstDisplayedScrollingRowIndex = dropIndex;
                    }
                    // スクロール位置を復元
                    if (scrollTop >= 0 && scrollTop < dataGridView1.Rows.Count)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = scrollTop;
                    }
                }));
            }

            dropPreviewIndex = -1;
            dataGridView1.Invalidate();
        }

        private int dropPreviewIndex = -1;

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            if (dropPreviewIndex >= 0)
            {
                //移動先の描画
                int y;

                if (dropPreviewIndex < dataGridView1.Rows.Count)
                {
                    Rectangle rowRect = dataGridView1.GetRowDisplayRectangle(dropPreviewIndex, true);
                    y = rowRect.Top;
                }
                else
                {
                    Rectangle lastRowRect = dataGridView1.GetRowDisplayRectangle(dataGridView1.Rows.Count - 1, true);
                    y = lastRowRect.Bottom;
                }

                using (Pen mainPen = new Pen(Color.FromArgb(100, 100, 100), 2))
                {
                    e.Graphics.DrawLine(mainPen, 0, y, dataGridView1.Width, y);
                }
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;

            //行番号
            string rowNumber = (e.RowIndex + 1).ToString();

            Rectangle headerBounds = new Rectangle(
                e.RowBounds.Left,
                e.RowBounds.Top,
                grid.RowHeadersWidth,
                e.RowBounds.Height);

            var font = grid.Font;
            var color = Color.FromArgb(100, 100, 100);

            TextRenderer.DrawText(
                e.Graphics,
                rowNumber,
                font,
                headerBounds,
                color,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex>=0) // ヘッダー行でないことを確認
            {
                var row = dataGridView1.Rows[e.RowIndex].DataBoundItem as VpnSettingModel;
                if (row == null) return;

                var dlg = new ConnectionDialog();
                dlg.VpnSetting = row;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog();
            }
        }
    }
}
