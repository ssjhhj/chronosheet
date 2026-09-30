using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Chronosheet
{
    partial class MainForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tabControl = new TabControl();
            this.tabTimer = new TabPage();
            this.btnMiniMode = new Button();
            this.btnReset = new Button();
            this.btnPause = new Button();
            this.btnStart = new Button();
            this.lblTimeDisplay = new Label();
            this.nudSecond = new NumericUpDown();
            this.nudMinute = new NumericUpDown();
            this.nudHour = new NumericUpDown();
            this.lblSec = new Label();
            this.lblMin = new Label();
            this.lblHr = new Label();
            this.rdoCountdown = new RadioButton();
            this.rdoCountup = new RadioButton();
            this.tabRecords = new TabPage();
            this.dgvRecords = new DataGridView();
            this.colHour = new DataGridViewTextBoxColumn();
            this.colContent = new DataGridViewTextBoxColumn();
            this.btnUnmerge = new Button();
            this.btnMergeFill = new Button();
            this.btnMerge = new Button();
            this.btnCopyClipboard = new Button();
            this.btnExportExcel = new Button();
            this.dtpDate = new DateTimePicker();
            this.tabCalendar = new TabPage();
            this.txtDayDetail = new TextBox();
            this.monthCalendar = new MonthCalendar();
            this.tmrTick = new Timer(this.components);
            this.tabControl.SuspendLayout();
            this.tabTimer.SuspendLayout();
            ((ISupportInitialize)(this.nudSecond)).BeginInit();
            ((ISupportInitialize)(this.nudMinute)).BeginInit();
            ((ISupportInitialize)(this.nudHour)).BeginInit();
            this.tabRecords.SuspendLayout();
            ((ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.tabCalendar.SuspendLayout();
            this.SuspendLayout();

            // tabControl
            this.tabControl.Controls.Add(this.tabTimer);
            this.tabControl.Controls.Add(this.tabRecords);
            this.tabControl.Controls.Add(this.tabCalendar);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Font = new Font("Microsoft YaHei UI", 9F);
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(784, 561);
            this.tabControl.TabIndex = 0;

            // tabTimer - 计时器
            this.tabTimer.BackColor = Color.White;
            this.tabTimer.Controls.Add(this.btnMiniMode);
            this.tabTimer.Controls.Add(this.btnReset);
            this.tabTimer.Controls.Add(this.btnPause);
            this.tabTimer.Controls.Add(this.btnStart);
            this.tabTimer.Controls.Add(this.lblTimeDisplay);
            this.tabTimer.Controls.Add(this.nudSecond);
            this.tabTimer.Controls.Add(this.nudMinute);
            this.tabTimer.Controls.Add(this.nudHour);
            this.tabTimer.Controls.Add(this.lblSec);
            this.tabTimer.Controls.Add(this.lblMin);
            this.tabTimer.Controls.Add(this.lblHr);
            this.tabTimer.Controls.Add(this.rdoCountdown);
            this.tabTimer.Controls.Add(this.rdoCountup);
            this.tabTimer.Location = new Point(4, 31);
            this.tabTimer.Name = "tabTimer";
            this.tabTimer.Padding = new Padding(3);
            this.tabTimer.Size = new Size(776, 526);
            this.tabTimer.TabIndex = 0;
            this.tabTimer.Text = "计时器";

            // rdoCountup - 正计时
            this.rdoCountup.AutoSize = true;
            this.rdoCountup.Checked = true;
            this.rdoCountup.Font = new Font("Microsoft YaHei UI", 9F);
            this.rdoCountup.Location = new Point(40, 30);
            this.rdoCountup.Name = "rdoCountup";
            this.rdoCountup.Size = new Size(68, 25);
            this.rdoCountup.TabIndex = 0;
            this.rdoCountup.TabStop = true;
            this.rdoCountup.Text = "正计时";
            this.rdoCountup.UseVisualStyleBackColor = true;
            this.rdoCountup.CheckedChanged += new System.EventHandler(this.rdoCountup_CheckedChanged);

            // rdoCountdown - 倒计时
            this.rdoCountdown.AutoSize = true;
            this.rdoCountdown.Font = new Font("Microsoft YaHei UI", 9F);
            this.rdoCountdown.Location = new Point(130, 30);
            this.rdoCountdown.Name = "rdoCountdown";
            this.rdoCountdown.Size = new Size(68, 25);
            this.rdoCountdown.TabIndex = 1;
            this.rdoCountdown.Text = "倒计时";
            this.rdoCountdown.UseVisualStyleBackColor = true;
            this.rdoCountdown.CheckedChanged += new System.EventHandler(this.rdoCountdown_CheckedChanged);

            // lblHr
            this.lblHr.AutoSize = true;
            this.lblHr.Font = new Font("Microsoft YaHei UI", 9F);
            this.lblHr.Location = new Point(40, 80);
            this.lblHr.Name = "lblHr";
            this.lblHr.Size = new Size(32, 20);
            this.lblHr.TabIndex = 2;
            this.lblHr.Text = "时：";
            this.lblHr.Visible = false;

            // nudHour
            this.nudHour.Font = new Font("Microsoft YaHei UI", 9F);
            this.nudHour.Location = new Point(78, 78);
            this.nudHour.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
            this.nudHour.Name = "nudHour";
            this.nudHour.Size = new Size(70, 27);
            this.nudHour.TabIndex = 3;
            this.nudHour.Visible = false;
            this.nudHour.Value = new decimal(new int[] { 0, 0, 0, 0 });

            // lblMin
            this.lblMin.AutoSize = true;
            this.lblMin.Font = new Font("Microsoft YaHei UI", 9F);
            this.lblMin.Location = new Point(170, 80);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new Size(32, 20);
            this.lblMin.TabIndex = 4;
            this.lblMin.Text = "分：";
            this.lblMin.Visible = false;

            // nudMinute
            this.nudMinute.Font = new Font("Microsoft YaHei UI", 9F);
            this.nudMinute.Location = new Point(208, 78);
            this.nudMinute.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            this.nudMinute.Name = "nudMinute";
            this.nudMinute.Size = new Size(70, 27);
            this.nudMinute.TabIndex = 5;
            this.nudMinute.Visible = false;

            // lblSec
            this.lblSec.AutoSize = true;
            this.lblSec.Font = new Font("Microsoft YaHei UI", 9F);
            this.lblSec.Location = new Point(300, 80);
            this.lblSec.Name = "lblSec";
            this.lblSec.Size = new Size(32, 20);
            this.lblSec.TabIndex = 6;
            this.lblSec.Text = "秒：";
            this.lblSec.Visible = false;

            // nudSecond
            this.nudSecond.Font = new Font("Microsoft YaHei UI", 9F);
            this.nudSecond.Location = new Point(338, 78);
            this.nudSecond.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            this.nudSecond.Name = "nudSecond";
            this.nudSecond.Size = new Size(70, 27);
            this.nudSecond.TabIndex = 7;
            this.nudSecond.Visible = false;

            // lblTimeDisplay - 大时间显示
            this.lblTimeDisplay.BackColor = Color.FromArgb(245, 247, 250);
            this.lblTimeDisplay.BorderStyle = BorderStyle.FixedSingle;
            this.lblTimeDisplay.Font = new Font("Microsoft YaHei UI", 28F, FontStyle.Bold);
            this.lblTimeDisplay.ForeColor = Color.FromArgb(45, 55, 72);
            this.lblTimeDisplay.Location = new Point(40, 140);
            this.lblTimeDisplay.Name = "lblTimeDisplay";
            this.lblTimeDisplay.Size = new Size(696, 160);
            this.lblTimeDisplay.TabIndex = 8;
            this.lblTimeDisplay.Text = "00:00:00";
            this.lblTimeDisplay.TextAlign = ContentAlignment.MiddleCenter;

            // btnStart
            this.btnStart.BackColor = Color.FromArgb(66, 153, 225);
            this.btnStart.FlatStyle = FlatStyle.Flat;
            this.btnStart.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnStart.ForeColor = Color.White;
            this.btnStart.Location = new Point(170, 330);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new Size(100, 42);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            // btnPause
            this.btnPause.BackColor = Color.FromArgb(237, 137, 54);
            this.btnPause.FlatStyle = FlatStyle.Flat;
            this.btnPause.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnPause.ForeColor = Color.White;
            this.btnPause.Location = new Point(300, 330);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new Size(100, 42);
            this.btnPause.TabIndex = 10;
            this.btnPause.Text = "暂停";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);

            // btnReset
            this.btnReset.BackColor = Color.FromArgb(160, 174, 192);
            this.btnReset.FlatStyle = FlatStyle.Flat;
            this.btnReset.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnReset.ForeColor = Color.White;
            this.btnReset.Location = new Point(430, 330);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new Size(100, 42);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            // btnMiniMode - 迷你模式按钮
            this.btnMiniMode.BackColor = Color.FromArgb(102, 126, 234);
            this.btnMiniMode.FlatStyle = FlatStyle.Flat;
            this.btnMiniMode.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnMiniMode.ForeColor = Color.White;
            this.btnMiniMode.Location = new Point(560, 330);
            this.btnMiniMode.Name = "btnMiniMode";
            this.btnMiniMode.Size = new Size(140, 42);
            this.btnMiniMode.TabIndex = 12;
            this.btnMiniMode.Text = "迷你模式（置顶）";
            this.btnMiniMode.UseVisualStyleBackColor = false;
            this.btnMiniMode.Click += new System.EventHandler(this.btnMiniMode_Click);

            // tabRecords - 小时记录
            this.tabRecords.BackColor = Color.White;
            this.tabRecords.Controls.Add(this.dgvRecords);
            this.tabRecords.Controls.Add(this.btnUnmerge);
            this.tabRecords.Controls.Add(this.btnMergeFill);
            this.tabRecords.Controls.Add(this.btnMerge);
            this.tabRecords.Controls.Add(this.btnCopyClipboard);
            this.tabRecords.Controls.Add(this.btnExportExcel);
            this.tabRecords.Controls.Add(this.dtpDate);
            this.tabRecords.Location = new Point(4, 31);
            this.tabRecords.Name = "tabRecords";
            this.tabRecords.Padding = new Padding(3);
            this.tabRecords.Size = new Size(776, 526);
            this.tabRecords.TabIndex = 1;
            this.tabRecords.Text = "小时记录";

            // dtpDate
            this.dtpDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDate.Font = new Font("Microsoft YaHei UI", 9F);
            this.dtpDate.Format = DateTimePickerFormat.Custom;
            this.dtpDate.Location = new Point(20, 20);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.ShowUpDown = false;
            this.dtpDate.Size = new Size(160, 27);
            this.dtpDate.TabIndex = 0;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);

            // btnMerge - 合并所选
            this.btnMerge.BackColor = Color.FromArgb(246, 173, 85);
            this.btnMerge.FlatStyle = FlatStyle.Flat;
            this.btnMerge.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnMerge.ForeColor = Color.White;
            this.btnMerge.Location = new Point(200, 14);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new Size(100, 38);
            this.btnMerge.TabIndex = 1;
            this.btnMerge.Text = "合并所选";
            this.btnMerge.UseVisualStyleBackColor = false;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);

            // btnMergeFill - 同步内容
            this.btnMergeFill.BackColor = Color.FromArgb(102, 126, 234);
            this.btnMergeFill.FlatStyle = FlatStyle.Flat;
            this.btnMergeFill.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnMergeFill.ForeColor = Color.White;
            this.btnMergeFill.Location = new Point(320, 14);
            this.btnMergeFill.Name = "btnMergeFill";
            this.btnMergeFill.Size = new Size(110, 38);
            this.btnMergeFill.TabIndex = 2;
            this.btnMergeFill.Text = "同步内容到所选";
            this.btnMergeFill.UseVisualStyleBackColor = false;
            this.btnMergeFill.Click += new System.EventHandler(this.btnMergeFill_Click);

            // btnUnmerge - 拆分所选
            this.btnUnmerge.BackColor = Color.FromArgb(237, 100, 100);
            this.btnUnmerge.FlatStyle = FlatStyle.Flat;
            this.btnUnmerge.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnUnmerge.ForeColor = Color.White;
            this.btnUnmerge.Location = new Point(450, 14);
            this.btnUnmerge.Name = "btnUnmerge";
            this.btnUnmerge.Size = new Size(100, 38);
            this.btnUnmerge.TabIndex = 3;
            this.btnUnmerge.Text = "拆分所选";
            this.btnUnmerge.UseVisualStyleBackColor = false;
            this.btnUnmerge.Click += new System.EventHandler(this.btnUnmerge_Click);

            // btnExportExcel
            this.btnExportExcel.BackColor = Color.FromArgb(130, 87, 229);
            this.btnExportExcel.FlatStyle = FlatStyle.Flat;
            this.btnExportExcel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnExportExcel.ForeColor = Color.White;
            this.btnExportExcel.Location = new Point(20, 60);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new Size(150, 32);
            this.btnExportExcel.TabIndex = 4;
            this.btnExportExcel.Text = "导出当天为 Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);

            // btnCopyClipboard
            this.btnCopyClipboard.BackColor = Color.FromArgb(49, 151, 217);
            this.btnCopyClipboard.FlatStyle = FlatStyle.Flat;
            this.btnCopyClipboard.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnCopyClipboard.ForeColor = Color.White;
            this.btnCopyClipboard.Location = new Point(190, 60);
            this.btnCopyClipboard.Name = "btnCopyClipboard";
            this.btnCopyClipboard.Size = new Size(160, 32);
            this.btnCopyClipboard.TabIndex = 5;
            this.btnCopyClipboard.Text = "复制当天到剪贴板";
            this.btnCopyClipboard.UseVisualStyleBackColor = false;
            this.btnCopyClipboard.Click += new System.EventHandler(this.btnCopyClipboard_Click);

            // dgvRecords
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToDeleteRows = false;
            this.dgvRecords.AllowUserToResizeRows = false;
            this.dgvRecords.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                | AnchorStyles.Left)
                | AnchorStyles.Right)));
            this.dgvRecords.BackgroundColor = Color.White;
            this.dgvRecords.BorderStyle = BorderStyle.FixedSingle;
            this.dgvRecords.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgvRecords.ColumnHeadersHeight = 36;
            this.dgvRecords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRecords.Columns.AddRange(new DataGridViewColumn[] {
            this.colHour,
            this.colContent});
            this.dgvRecords.Font = new Font("Microsoft YaHei UI", 9F);
            this.dgvRecords.GridColor = Color.FromArgb(226, 232, 240);
            this.dgvRecords.Location = new Point(20, 110);
            this.dgvRecords.MultiSelect = true;
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.ReadOnly = false;
            this.dgvRecords.RowHeadersVisible = false;
            this.dgvRecords.RowTemplate.Height = 32;
            this.dgvRecords.ScrollBars = ScrollBars.Vertical;
            this.dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecords.Size = new Size(736, 400);
            this.dgvRecords.TabIndex = 6;
            this.dgvRecords.CellValueChanged += new DataGridViewCellEventHandler(this.dgvRecords_CellValueChanged);
            this.dgvRecords.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvRecords_CellFormatting);

            // colHour
            this.colHour.HeaderText = "小时";
            this.colHour.Name = "colHour";
            this.colHour.ReadOnly = true;
            this.colHour.Width = 130;

            // colContent
            this.colContent.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colContent.HeaderText = "内容";
            this.colContent.Name = "colContent";

            // tabCalendar - 日历视图
            this.tabCalendar.BackColor = Color.White;
            this.tabCalendar.Controls.Add(this.txtDayDetail);
            this.tabCalendar.Controls.Add(this.monthCalendar);
            this.tabCalendar.Location = new Point(4, 31);
            this.tabCalendar.Name = "tabCalendar";
            this.tabCalendar.Padding = new Padding(3);
            this.tabCalendar.Size = new Size(776, 526);
            this.tabCalendar.TabIndex = 2;
            this.tabCalendar.Text = "日历视图";

            // monthCalendar
            this.monthCalendar.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom)
                | AnchorStyles.Left)));
            this.monthCalendar.Font = new Font("Microsoft YaHei UI", 9F);
            this.monthCalendar.Location = new Point(20, 20);
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 0;
            this.monthCalendar.DateChanged += new DateRangeEventHandler(this.monthCalendar_DateChanged);

            // txtDayDetail
            this.txtDayDetail.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                | AnchorStyles.Left)
                | AnchorStyles.Right)));
            this.txtDayDetail.BackColor = Color.White;
            this.txtDayDetail.BorderStyle = BorderStyle.FixedSingle;
            this.txtDayDetail.Font = new Font("Microsoft YaHei UI", 9F);
            this.txtDayDetail.Location = new Point(320, 20);
            this.txtDayDetail.Multiline = true;
            this.txtDayDetail.Name = "txtDayDetail";
            this.txtDayDetail.ReadOnly = true;
            this.txtDayDetail.ScrollBars = ScrollBars.Vertical;
            this.txtDayDetail.Size = new Size(436, 486);
            this.txtDayDetail.TabIndex = 1;

            // tmrTick
            this.tmrTick.Interval = 1000;
            this.tmrTick.Tick += new System.EventHandler(this.tmrTick_Tick);

            // MainForm
            this.AutoScaleDimensions = new SizeF(7F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(784, 561);
            this.Controls.Add(this.tabControl);
            this.Font = new Font("Microsoft YaHei UI", 9F);
            this.MinimumSize = new Size(800, 600);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Chronosheet - 个人时间记录工具";
            try { this.Icon = new Icon("chronsheet.ico"); } catch { }
            this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabTimer.ResumeLayout(false);
            this.tabTimer.PerformLayout();
            ((ISupportInitialize)(this.nudSecond)).EndInit();
            ((ISupportInitialize)(this.nudMinute)).EndInit();
            ((ISupportInitialize)(this.nudHour)).EndInit();
            this.tabRecords.ResumeLayout(false);
            ((ISupportInitialize)(this.dgvRecords)).EndInit();
            this.tabCalendar.ResumeLayout(false);
            this.tabCalendar.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabTimer;
        private TabPage tabRecords;
        private TabPage tabCalendar;

        // Tab 1 计时器
        private RadioButton rdoCountup;
        private RadioButton rdoCountdown;
        private Label lblHr;
        private NumericUpDown nudHour;
        private Label lblMin;
        private NumericUpDown nudMinute;
        private Label lblSec;
        private NumericUpDown nudSecond;
        private Label lblTimeDisplay;
        private Button btnStart;
        private Button btnPause;
        private Button btnReset;
        private Button btnMiniMode;
        private Timer tmrTick;

        // Tab 2 小时记录
        private DateTimePicker dtpDate;
        private Button btnExportExcel;
        private Button btnCopyClipboard;
        private Button btnMerge;
        private Button btnMergeFill;
        private Button btnUnmerge;
        private DataGridView dgvRecords;
        private DataGridViewTextBoxColumn colHour;
        private DataGridViewTextBoxColumn colContent;

        // Tab 3 日历视图
        private MonthCalendar monthCalendar;
        private TextBox txtDayDetail;
    }
}
