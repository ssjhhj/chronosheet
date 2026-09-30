using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Chronosheet
{
    /// <summary>
    /// 迷你计时器窗口：置顶、无边框、可拖动、只显示时间+还原/关闭
    /// </summary>
    public partial class MiniTimerForm : Form
    {
        private readonly MainForm _owner;

        // 拖动窗口用的 Win32 消息
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public MiniTimerForm(MainForm owner)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
            InitializeComponent();
        }

        private void MiniTimerForm_Load(object sender, EventArgs e)
        {
            // 首次刷新一次显示
            string text = _owner.GetTimerDisplayText();
            UpdateDisplayText(text, _owner.TimerIsCountdown, _owner.TimerIsRunning);

            // 画一圈边框（因为 FormBorderStyle=None）
            this.Paint += (s, ev) =>
            {
                using var pen = new Pen(Color.FromArgb(45, 55, 72), 2);
                ev.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            };
        }

        /// <summary>
        /// 由主窗体每秒调用刷新显示
        /// </summary>
        public void UpdateDisplayText(string hhmmss, bool isCountdown, bool isRunning)
        {
            if (this.IsDisposed) return;
            if (lblTime.Text != hhmmss) lblTime.Text = hhmmss;
            string mode = isCountdown ? "倒计时" : "正计时";
            string status = isRunning ? "运行中" : "已暂停";
            string newStatus = $"{mode} · {status}";
            if (lblStatus.Text != newStatus) lblStatus.Text = newStatus;
        }

        /// <summary>
        /// 拖动窗口：点住 lblTime/lblStatus/窗体空白都能拖
        /// </summary>
        private void MiniTimerForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// 还原主窗口：把主窗体激活前置；迷你窗本身不关（用户想关再点关闭）
        /// </summary>
        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (_owner.IsDisposed) return;
            if (_owner.WindowState == FormWindowState.Minimized)
                _owner.WindowState = FormWindowState.Normal;
            _owner.Show();
            _owner.Activate();
            _owner.BringToFront();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
