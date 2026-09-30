using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Chronosheet
{
    partial class MiniTimerForm
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
            this.lblTime = new Label();
            this.btnRestore = new Button();
            this.btnClose = new Button();
            this.lblStatus = new Label();
            this.SuspendLayout();

            // lblTime
            this.lblTime.BackColor = Color.FromArgb(45, 55, 72);
            this.lblTime.Dock = DockStyle.Top;
            this.lblTime.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Bold);
            this.lblTime.ForeColor = Color.White;
            this.lblTime.Location = new Point(0, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new Size(320, 88);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "00:00:00";
            this.lblTime.TextAlign = ContentAlignment.MiddleCenter;

            // lblStatus
            this.lblStatus.BackColor = Color.FromArgb(45, 55, 72);
            this.lblStatus.Dock = DockStyle.Top;
            this.lblStatus.Font = new Font("Microsoft YaHei UI", 9F);
            this.lblStatus.ForeColor = Color.FromArgb(226, 232, 240);
            this.lblStatus.Location = new Point(0, 88);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(320, 24);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "正计时 · 未开始";
            this.lblStatus.TextAlign = ContentAlignment.MiddleCenter;

            // btnRestore
            this.btnRestore.BackColor = Color.FromArgb(72, 187, 120);
            this.btnRestore.FlatAppearance.BorderSize = 0;
            this.btnRestore.FlatStyle = FlatStyle.Flat;
            this.btnRestore.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnRestore.ForeColor = Color.White;
            this.btnRestore.Location = new Point(40, 126);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new Size(100, 30);
            this.btnRestore.TabIndex = 2;
            this.btnRestore.Text = "还原主窗口";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);

            // btnClose
            this.btnClose.BackColor = Color.FromArgb(229, 62, 62);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(180, 126);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭迷你窗";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // MiniTimerForm
            this.AutoScaleDimensions = new SizeF(7F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(320, 172);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTime);
            this.Font = new Font("Microsoft YaHei UI", 9F);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MiniTimerForm";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "迷你计时器";
            this.TopMost = true;
            try { this.Icon = new Icon("chronsheet.ico"); } catch { }
            this.MouseDown += new MouseEventHandler(this.MiniTimerForm_MouseDown);
            this.lblTime.MouseDown += new MouseEventHandler(this.MiniTimerForm_MouseDown);
            this.lblStatus.MouseDown += new MouseEventHandler(this.MiniTimerForm_MouseDown);
            this.Load += new System.EventHandler(this.MiniTimerForm_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private Label lblTime;
        private Label lblStatus;
        private Button btnRestore;
        private Button btnClose;
    }
}
