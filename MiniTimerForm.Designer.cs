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
            this.lblStatus = new Label();
            this.panelTitle = new Panel();
            this.btnTopMost = new Button();
            this.btnRestore = new Button();
            this.btnClose = new Button();
            this.btnStartPause = new Button();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();

            // panelTitle - 顶部标题条（Dock=Top，专门承载右上角3个按钮）
            // 用 Dock 容器可以保证按钮永远画在 Dock 堆叠的最顶层，不会被 lblTime 覆盖
            this.panelTitle.BackColor = Color.FromArgb(45, 55, 72);
            this.panelTitle.Controls.Add(this.btnTopMost);
            this.panelTitle.Controls.Add(this.btnRestore);
            this.panelTitle.Controls.Add(this.btnClose);
            this.panelTitle.Dock = DockStyle.Top;
            this.panelTitle.Location = new Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new Size(320, 32);
            this.panelTitle.TabIndex = 6;
            this.panelTitle.MouseDown += new MouseEventHandler(this.MiniTimerForm_MouseDown);

            // btnClose - 右上角：关闭（最右侧，Dock=Right）
            this.btnClose.BackColor = Color.FromArgb(229, 62, 62);
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 101, 101);
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Margin = new Padding(0);
            this.btnClose.Size = new Size(28, 32);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // btnRestore - 右上角：返回主窗（在 btnClose 左边）
            this.btnRestore.BackColor = Color.FromArgb(74, 85, 104);
            this.btnRestore.Dock = DockStyle.Right;
            this.btnRestore.FlatAppearance.BorderSize = 0;
            this.btnRestore.FlatAppearance.MouseOverBackColor = Color.FromArgb(99, 115, 139);
            this.btnRestore.FlatStyle = FlatStyle.Flat;
            this.btnRestore.Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold);
            this.btnRestore.ForeColor = Color.White;
            this.btnRestore.Margin = new Padding(0);
            this.btnRestore.Size = new Size(28, 32);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Text = "←";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);

            // btnTopMost - 右上角：置顶开关（在 btnRestore 左边）
            this.btnTopMost.BackColor = Color.FromArgb(74, 85, 104);
            this.btnTopMost.Dock = DockStyle.Right;
            this.btnTopMost.FlatAppearance.BorderSize = 0;
            this.btnTopMost.FlatAppearance.MouseOverBackColor = Color.FromArgb(99, 115, 139);
            this.btnTopMost.FlatStyle = FlatStyle.Flat;
            this.btnTopMost.Font = new Font("Segoe UI Symbol", 9F, FontStyle.Bold);
            this.btnTopMost.ForeColor = Color.White;
            this.btnTopMost.Margin = new Padding(0);
            this.btnTopMost.Size = new Size(28, 32);
            this.btnTopMost.TabIndex = 2;
            this.btnTopMost.Text = "📌";
            this.btnTopMost.UseVisualStyleBackColor = false;
            this.btnTopMost.Click += new System.EventHandler(this.btnTopMost_Click);

            // lblTime（Dock=Top，在 panelTitle 之下）
            this.lblTime.BackColor = Color.FromArgb(45, 55, 72);
            this.lblTime.Dock = DockStyle.Top;
            this.lblTime.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Bold);
            this.lblTime.ForeColor = Color.White;
            this.lblTime.Location = new Point(0, 32);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new Size(320, 84);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "00:00:00";
            this.lblTime.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTime.MouseDown += new MouseEventHandler(this.MiniTimerForm_MouseDown);

            // lblStatus（Dock=Top，在 lblTime 之下）
            this.lblStatus.BackColor = Color.FromArgb(45, 55, 72);
            this.lblStatus.Dock = DockStyle.Top;
            this.lblStatus.Font = new Font("Microsoft YaHei UI", 9F);
            this.lblStatus.ForeColor = Color.FromArgb(226, 232, 240);
            this.lblStatus.Location = new Point(0, 116);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(320, 22);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "正计时 · 未开始";
            this.lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            this.lblStatus.MouseDown += new MouseEventHandler(this.MiniTimerForm_MouseDown);

            // btnStartPause - 开始/停止切换按钮（绝对定位在底部区最上方；最后 Add => Z 最前）
            this.btnStartPause.BackColor = Color.FromArgb(66, 153, 225);
            this.btnStartPause.FlatAppearance.BorderSize = 0;
            this.btnStartPause.FlatStyle = FlatStyle.Flat;
            this.btnStartPause.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            this.btnStartPause.ForeColor = Color.White;
            this.btnStartPause.Location = new Point(80, 150);
            this.btnStartPause.Name = "btnStartPause";
            this.btnStartPause.Size = new Size(160, 32);
            this.btnStartPause.TabIndex = 5;
            this.btnStartPause.Text = "开始";
            this.btnStartPause.UseVisualStyleBackColor = false;
            this.btnStartPause.Click += new System.EventHandler(this.btnStartPause_Click);

            // MiniTimerForm
            // Controls.Add 顺序（从先到后 => Dock 堆叠从下到上；最后 Add 的绝对定位控件 Z 最前）：
            //   底部绝对定位 btnStartPause（最后 Add => Z 最前，不会被 Dock 盖住）
            //   lblStatus (Dock=Top, 最下面的 Dock)
            //   lblTime   (Dock=Top, 在 lblStatus 之上)
            //   panelTitle(Dock=Top, 最顶部的 Dock, 三个按钮在它里面一定可见)
            this.AutoScaleDimensions = new SizeF(7F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(45, 55, 72);
            this.ClientSize = new Size(320, 200);
            this.Controls.Add(this.btnStartPause);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.panelTitle);
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
            this.Load += new System.EventHandler(this.MiniTimerForm_Load);
            this.panelTitle.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Label lblTime;
        private Label lblStatus;
        private Panel panelTitle;
        private Button btnClose;
        private Button btnRestore;
        private Button btnTopMost;
        private Button btnStartPause;
    }
}
