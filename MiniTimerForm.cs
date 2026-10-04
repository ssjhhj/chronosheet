using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Chronosheet
{
    /// <summary>
    /// 迷你计时器窗口：置顶、无边框、可拖动、时间显示+控制
    /// 顶部 panelTitle(Dock=Top 最上层)：📌 置顶 / ← 返回主窗 / ✕ 关闭
    /// 中部下方：开始 / 暂停 切换按钮（同位置）
    /// 支持动态设置背景色 + 不透明度（来自 AppSettings）
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

        // 按钮颜色常量
        private static readonly Color StartColor = Color.FromArgb(66, 153, 225);
        private static readonly Color PauseColor = Color.FromArgb(237, 137, 54);
        private static readonly Color TopMostActiveColor = Color.FromArgb(246, 173, 85);
        private static readonly Color TopMostInactiveColor = Color.FromArgb(74, 85, 104);
        private static readonly Color BtnRestoreInactiveColor = Color.FromArgb(74, 85, 104);
        private static readonly Color BtnCloseColor = Color.FromArgb(229, 62, 62);

        // 背景色深浅判断阈值（亮度 0..1，小于 0.5 视为深色 => 用浅色前景）
        private const float LuminanceThreshold = 0.5f;

        public MiniTimerForm(MainForm owner)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
            InitializeComponent();
        }

        private void MiniTimerForm_Load(object sender, EventArgs e)
        {
            // 根据当前设置初始化外观
            ApplySettings(AppSettings.Load());

            // 初次同步：时间 + 状态栏 + 开始/暂停按钮状态 + 置顶按钮视觉
            RefreshFromOwnerState();

            // 画一圈边框（因为 FormBorderStyle=None）
            this.Paint += (s, ev) =>
            {
                Color borderColor = PickBorderColor(this.BackColor);
                using var pen = new Pen(borderColor, 2);
                ev.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            };
        }

        /// <summary>
        /// 动态应用设置（背景色、不透明度）；设置改变后实时调用
        /// </summary>
        public void ApplySettings(AppSettings settings)
        {
            if (this.IsDisposed || settings == null) return;

            // 1. 背景色：窗体 + panelTitle + lblTime + lblStatus 统一颜色
            Color bc = settings.MiniBackColor;
            this.BackColor = bc;
            panelTitle.BackColor = bc;
            lblTime.BackColor = bc;
            lblStatus.BackColor = bc;

            // 2. 文本颜色（根据背景深浅自适应，保证可读）
            Color fore = IsDark(bc) ? Color.White : Color.FromArgb(45, 55, 72);
            Color foreSubtle = IsDark(bc) ? Color.FromArgb(226, 232, 240) : Color.FromArgb(113, 128, 150);
            lblTime.ForeColor = fore;
            lblStatus.ForeColor = foreSubtle;

            // 返回按钮 & 置顶按钮（未激活态）：跟随「深/浅」自适应颜色，避免和背景同色
            Color btnInactiveBg = IsDark(bc)
                ? Color.FromArgb(74, 85, 104)      // 深背景下按钮稍微亮一点点
                : Color.FromArgb(203, 213, 225);    // 浅背景下按钮稍微暗一点点
            if (!this.TopMost)
                btnTopMost.BackColor = btnInactiveBg;
            btnRestore.BackColor = btnInactiveBg;

            // 3. 不透明度（范围 0.3 ~ 1.0）
            double op = settings.MiniOpacity;
            if (op < 0.3) op = 0.3; else if (op > 1.0) op = 1.0;
            this.Opacity = op;

            this.Invalidate();
        }

        private static Color PickBorderColor(Color backColor)
        {
            return IsDark(backColor)
                ? Color.FromArgb(30, 41, 59)
                : Color.FromArgb(148, 163, 184);
        }

        private static bool IsDark(Color c)
        {
            // 标准亮度公式（Rec. 709）
            float lum = (0.2126f * c.R + 0.7152f * c.G + 0.0722f * c.B) / 255f;
            return lum < LuminanceThreshold;
        }

        private void RefreshFromOwnerState()
        {
            string text = _owner.GetTimerDisplayText();
            bool isCountdown = _owner.TimerIsCountdown;
            bool isRunning = _owner.TimerIsRunning;
            UpdateDisplayText(text, isCountdown, isRunning);
            UpdateStartPauseButton(isRunning);
            UpdateTopMostButton();
        }

        /// <summary>
        /// 由主窗体每秒调用刷新显示
        /// </summary>
        public void UpdateDisplayText(string hhmmss, bool isCountdown, bool isRunning)
        {
            if (this.IsDisposed) return;
            if (lblTime.Text != hhmmss) lblTime.Text = hhmmss;
            string mode = isCountdown ? "倒计时" : "正计时";
            string status;
            if (hhmmss == "00:00:00" && !isRunning)
                status = "未开始";
            else
                status = isRunning ? "运行中" : "已暂停";
            string newStatus = $"{mode} · {status}";
            if (lblStatus.Text != newStatus) lblStatus.Text = newStatus;

            UpdateStartPauseButton(isRunning);
        }

        private void UpdateStartPauseButton(bool isRunning)
        {
            if (this.IsDisposed) return;
            if (isRunning)
            {
                if (btnStartPause.Text != "暂停") btnStartPause.Text = "暂停";
                if (btnStartPause.BackColor != PauseColor) btnStartPause.BackColor = PauseColor;
            }
            else
            {
                if (btnStartPause.Text != "开始") btnStartPause.Text = "开始";
                if (btnStartPause.BackColor != StartColor) btnStartPause.BackColor = StartColor;
            }
        }

        private void UpdateTopMostButton()
        {
            if (this.IsDisposed) return;
            btnTopMost.BackColor = this.TopMost ? TopMostActiveColor : BtnRestoreInactiveColor;
        }

        /// <summary>
        /// 拖动窗口：点住标题条 / lblTime / lblStatus / 窗体空白 都能拖（子控件按钮不会触发）
        /// </summary>
        private void MiniTimerForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // ===== 右上角：置顶开关 =====
        private void btnTopMost_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            UpdateTopMostButton();
        }

        // ===== 右上角：返回主窗口 =====
        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (_owner.IsDisposed) return;
            if (_owner.WindowState == FormWindowState.Minimized)
                _owner.WindowState = FormWindowState.Normal;
            _owner.Show();
            _owner.Activate();
            _owner.BringToFront();
        }

        // ===== 右上角：关闭迷你窗 =====
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ===== 开始 / 暂停 切换按钮 =====
        private void btnStartPause_Click(object sender, EventArgs e)
        {
            if (_owner.IsDisposed) return;
            _owner.ToggleTimerStartPause();
            RefreshFromOwnerState();
        }
    }

    /// <summary>
    /// 临时辅助（避免引用循环/向前引用问题）：给 MiniTimerForm 提供 LoadOrDefault 方法
    /// </summary>
    internal static class MiniSettingsCompat
    {
        // 给 MiniTimerForm.Load 调用，语义等于 AppSettings.Load()
        public static AppSettings LoadOrDefaultCompat() => AppSettings.Load();
    }
}
