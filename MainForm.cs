using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace Chronosheet
{
    /// <summary>
    /// 主窗体：计时器 / 小时记录 / 日历视图
    /// </summary>
    public partial class MainForm : Form
    {
        // ===== 计时器状态 =====
        private bool _isRunning = false;
        private TimeSpan _timerElapsed = TimeSpan.Zero;
        private TimeSpan _countdownInitial = TimeSpan.Zero;
        private DateTime _lastTick;

        // ===== 小时记录：合并组 =====
        // 单个合并区间 [StartHour, EndHour] 闭区间，StartHour < EndHour 才表示合并
        private List<MergeRange> _mergeRanges = new List<MergeRange>();
        private DateTime _lastLoadedDate;

        // ===== 迷你计时器窗口 =====
        private MiniTimerForm? _miniForm;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitDataGridViewRows();
            dtpDate.Value = DateTime.Today;
            LoadDailyRecords(dtpDate.Value);
            monthCalendar.SelectionStart = DateTime.Today;
            RefreshCalendarDetail(DateTime.Today);
            UpdateCountdownInputsVisibility();
            UpdateTimerDisplay();
        }

        #region ===== Tab 1：计时器 =====

        private void rdoCountup_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCountdownInputsVisibility();
            ResetTimer();
        }

        private void rdoCountdown_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCountdownInputsVisibility();
            ResetTimer();
        }

        private void UpdateCountdownInputsVisibility()
        {
            bool visible = rdoCountdown.Checked;
            lblHr.Visible = visible;
            nudHour.Visible = visible;
            lblMin.Visible = visible;
            nudMinute.Visible = visible;
            lblSec.Visible = visible;
            nudSecond.Visible = visible;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_isRunning) return;

            if (rdoCountdown.Checked)
            {
                if (_timerElapsed <= TimeSpan.Zero && _countdownInitial <= TimeSpan.Zero)
                {
                    int h = (int)nudHour.Value;
                    int m = (int)nudMinute.Value;
                    int s = (int)nudSecond.Value;
                    _countdownInitial = new TimeSpan(h, m, s);
                    _timerElapsed = _countdownInitial;
                    if (_timerElapsed <= TimeSpan.Zero)
                    {
                        MessageBox.Show("请设置大于 0 的倒计时时长。", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                nudHour.Enabled = false;
                nudMinute.Enabled = false;
                nudSecond.Enabled = false;
            }

            _isRunning = true;
            _lastTick = DateTime.Now;
            tmrTick.Start();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!_isRunning) return;
            _isRunning = false;
            tmrTick.Stop();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetTimer();
        }

        private void ResetTimer()
        {
            _isRunning = false;
            tmrTick.Stop();
            _timerElapsed = TimeSpan.Zero;
            _countdownInitial = TimeSpan.Zero;

            if (rdoCountdown.Checked)
            {
                nudHour.Enabled = true;
                nudMinute.Enabled = true;
                nudSecond.Enabled = true;
                int h = (int)nudHour.Value;
                int m = (int)nudMinute.Value;
                int s = (int)nudSecond.Value;
                _timerElapsed = new TimeSpan(h, m, s);
            }
            else
            {
                _timerElapsed = TimeSpan.Zero;
            }
            UpdateTimerDisplay();
        }

        private void tmrTick_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            double deltaMs = (now - _lastTick).TotalMilliseconds;
            _lastTick = now;

            if (rdoCountup.Checked)
            {
                _timerElapsed = _timerElapsed.Add(TimeSpan.FromMilliseconds(deltaMs));
            }
            else
            {
                _timerElapsed = _timerElapsed.Subtract(TimeSpan.FromMilliseconds(deltaMs));
                if (_timerElapsed <= TimeSpan.Zero)
                {
                    _timerElapsed = TimeSpan.Zero;
                    _isRunning = false;
                    tmrTick.Stop();
                    UpdateTimerDisplay();
                    SystemSounds.Beep.Play();
                    MessageBox.Show("倒计时结束！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nudHour.Enabled = true;
                    nudMinute.Enabled = true;
                    nudSecond.Enabled = true;
                    return;
                }
            }
            UpdateTimerDisplay();
        }

        /// <summary>
        /// 格式化当前时间为 HH:mm:ss
        /// </summary>
        internal string GetTimerDisplayText()
        {
            int total = (int)Math.Round(_timerElapsed.TotalSeconds);
            if (total < 0) total = 0;
            int hh = total / 3600;
            int mm = (total % 3600) / 60;
            int ss = total % 60;
            return $"{hh:00}:{mm:00}:{ss:00}";
        }

        internal bool TimerIsRunning => _isRunning;
        internal bool TimerIsCountdown => rdoCountdown.Checked;

        private void UpdateTimerDisplay()
        {
            string text = GetTimerDisplayText();
            lblTimeDisplay.Text = text;
            if (_miniForm != null && !_miniForm.IsDisposed)
                _miniForm.UpdateDisplayText(text, rdoCountdown.Checked, _isRunning);
        }

        #endregion

        #region ===== Tab 1.1：迷你模式切换 =====

        private void btnMiniMode_Click(object sender, EventArgs e)
        {
            if (_miniForm != null && !_miniForm.IsDisposed)
            {
                _miniForm.Activate();
                return;
            }
            _miniForm = new MiniTimerForm(this);
            _miniForm.FormClosed += (s, _) => { _miniForm = null; };
            _miniForm.Show(this);
            // 把迷你窗口定位到主窗体右上角外面
            if (this.WindowState != FormWindowState.Minimized)
            {
                _miniForm.Location = new Point(this.Right - _miniForm.Width - 20, this.Top + 20);
            }
            else
            {
                _miniForm.StartPosition = FormStartPosition.CenterScreen;
            }
            UpdateTimerDisplay();
        }

        #endregion

        #region ===== Tab 2：小时记录（自动保存 + 合并/拆分） =====

        private void InitDataGridViewRows()
        {
            dgvRecords.Rows.Clear();
            for (int h = 0; h < 24; h++)
            {
                dgvRecords.Rows.Add($"{h:00}:00", string.Empty);
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            // 切换日期前自动保存当前日期
            SaveDailyRecordsImpl();
            LoadDailyRecords(dtpDate.Value);
        }

        private void LoadDailyRecords(DateTime date)
        {
            string dateStr = date.ToString("yyyy-MM-dd");
            var records = DbHelper.GetDailyRecords(dateStr);

            // 加载前先清合并组（DB 里只存 24 条独立记录，合并不持久化）
            _mergeRanges.Clear();

            for (int h = 0; h < 24; h++)
            {
                var row = dgvRecords.Rows[h];
                row.Cells[0].Value = $"{h:00}:00";
                row.Cells[1].Value = records[h] ?? string.Empty;
            }
            _lastLoadedDate = date.Date;

            RefreshMergeVisuals();
        }

        /// <summary>
        /// 单元格值变化：① 若此单元格属于合并组，同步内容到组内所有行；② 立即自动保存全量；③ 刷新日历右侧
        /// </summary>
        private void dgvRecords_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= 24) return;
            if (e.ColumnIndex != 1) return;

            int hour = e.RowIndex;
            string? newVal = dgvRecords.Rows[hour].Cells[1].Value?.ToString() ?? string.Empty;

            // 1. 如果这行属于某个合并组，同步到组内所有行
            var range = FindRangeContaining(hour);
            if (range != null)
            {
                for (int h = range.StartHour; h <= range.EndHour; h++)
                {
                    if (h != hour)
                        dgvRecords.Rows[h].Cells[1].Value = newVal;
                }
            }

            // 2. 立即自动保存（简单同步即可，24 行 upsert 很快）
            SaveDailyRecordsImpl();

            // 3. 如果当前日期与日历选中日期一致，刷新右侧
            if (_lastLoadedDate == monthCalendar.SelectionStart.Date)
                RefreshCalendarDetail(_lastLoadedDate);
        }

        /// <summary>
        /// 合并组视觉：背景色、小时列显示区间/空
        /// </summary>
        private void dgvRecords_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= 24) return;
            int hour = e.RowIndex;
            var row = dgvRecords.Rows[hour];
            var range = FindRangeContaining(hour);

            Color mergedBg = Color.FromArgb(240, 246, 255);

            if (range == null)
            {
                row.DefaultCellStyle.BackColor = Color.White;
                if (e.ColumnIndex == 0)
                {
                    e.Value = $"{hour:00}:00";
                    e.FormattingApplied = true;
                }
            }
            else
            {
                row.DefaultCellStyle.BackColor = mergedBg;
                if (e.ColumnIndex == 0)
                {
                    if (hour == range.StartHour)
                    {
                        e.Value = $"{range.StartHour:00}:00 ~ {range.EndHour:00}:00";
                    }
                    else
                    {
                        e.Value = string.Empty;
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        private void RefreshMergeVisuals()
        {
            dgvRecords.Invalidate();
        }

        /// <summary>
        /// 获取当前选中的小时集合（FullRowSelect 模式下读取 SelectedRows 即可）
        /// </summary>
        private List<int> GetSelectedHours()
        {
            var hs = new HashSet<int>();
            foreach (DataGridViewRow r in dgvRecords.SelectedRows)
            {
                if (r.Index >= 0 && r.Index < 24) hs.Add(r.Index);
            }
            // 如果用户没选行，就退而看选中的单元格
            if (hs.Count == 0)
            {
                foreach (DataGridViewCell c in dgvRecords.SelectedCells)
                {
                    if (c.RowIndex >= 0 && c.RowIndex < 24) hs.Add(c.RowIndex);
                }
            }
            var list = hs.ToList();
            list.Sort();
            return list;
        }

        /// <summary>
        /// 检查是否连续
        /// </summary>
        private static bool IsConsecutive(IList<int> hours)
        {
            if (hours.Count <= 1) return true;
            for (int i = 1; i < hours.Count; i++)
            {
                if (hours[i] != hours[i - 1] + 1) return false;
            }
            return true;
        }

        /// <summary>
        /// 合并所选：要求连续且至少 2 行
        /// </summary>
        private void btnMerge_Click(object sender, EventArgs e)
        {
            var list = GetSelectedHours();
            if (list.Count < 2)
            {
                MessageBox.Show("请至少选中连续 2 行再合并。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!IsConsecutive(list))
            {
                MessageBox.Show("只能合并连续的时间段，请重新选择。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int start = list[0];
            int end = list[list.Count - 1];

            // 先把这个区间内的原有 range 拆出来（避免重叠）
            RemoveRangesOverlapping(start, end);

            // 内容同步：以 startHour 行为准（若 start 空则找组内第一个非空行）
            string unified = dgvRecords.Rows[start].Cells[1].Value?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(unified))
            {
                for (int h = start; h <= end; h++)
                {
                    string c = dgvRecords.Rows[h].Cells[1].Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(c)) { unified = c; break; }
                }
            }
            for (int h = start; h <= end; h++)
            {
                dgvRecords.Rows[h].Cells[1].Value = unified;
            }

            // 加入新合并区间
            _mergeRanges.Add(new MergeRange(start, end));
            _mergeRanges = _mergeRanges.OrderBy(r => r.StartHour).ToList();

            SaveDailyRecordsImpl();
            RefreshMergeVisuals();

            if (_lastLoadedDate == monthCalendar.SelectionStart.Date)
                RefreshCalendarDetail(_lastLoadedDate);
        }

        /// <summary>
        /// 同步内容到所选：把所选区间首行内容覆盖到所选其余各行（无需是已合并组）
        /// </summary>
        private void btnMergeFill_Click(object sender, EventArgs e)
        {
            var list = GetSelectedHours();
            if (list.Count < 1)
            {
                MessageBox.Show("请先选中至少一行。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!IsConsecutive(list))
            {
                MessageBox.Show("同步内容请选择连续的时间段。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int start = list[0];
            string content = dgvRecords.Rows[start].Cells[1].Value?.ToString() ?? string.Empty;
            for (int i = 1; i < list.Count; i++)
            {
                int h = list[i];
                dgvRecords.Rows[h].Cells[1].Value = content;
            }
            SaveDailyRecordsImpl();
            RefreshMergeVisuals();
            if (_lastLoadedDate == monthCalendar.SelectionStart.Date)
                RefreshCalendarDetail(_lastLoadedDate);
        }

        /// <summary>
        /// 拆分所选：把所选小时所在的所有合并组拆掉
        /// </summary>
        private void btnUnmerge_Click(object sender, EventArgs e)
        {
            var list = GetSelectedHours();
            if (list.Count < 1)
            {
                MessageBox.Show("请先选中要拆分的行。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool changed = false;
            foreach (var h in list)
            {
                var r = FindRangeContaining(h);
                if (r != null)
                {
                    _mergeRanges.Remove(r);
                    changed = true;
                }
            }
            if (changed)
            {
                RefreshMergeVisuals();
                // 拆分后内容不变，但是视觉变了；仍然保存一次（保证数据一致，实际上内容没改）
                SaveDailyRecordsImpl();
            }
            else
            {
                MessageBox.Show("所选行不在任何合并组中。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private MergeRange? FindRangeContaining(int hour)
        {
            foreach (var r in _mergeRanges)
            {
                if (r.StartHour <= hour && hour <= r.EndHour) return r;
            }
            return null;
        }

        private void RemoveRangesOverlapping(int start, int end)
        {
            _mergeRanges.RemoveAll(r =>
                !(r.EndHour < start || r.StartHour > end));
        }

        private void SaveDailyRecordsImpl()
        {
            string dateStr = _lastLoadedDate.ToString("yyyy-MM-dd");
            var dict = new Dictionary<int, string>();
            for (int h = 0; h < 24; h++)
            {
                object val = dgvRecords.Rows[h].Cells[1].Value;
                dict[h] = (val?.ToString() ?? string.Empty).Trim();
            }
            DbHelper.SaveDailyRecords(dateStr, dict);
        }

        private void btnCopyClipboard_Click(object sender, EventArgs e)
        {
            SaveDailyRecordsImpl();
            string dateStr = _lastLoadedDate.ToString("yyyy-MM-dd");
            var lines = new List<string> { dateStr };
            for (int h = 0; h < 24; h++)
            {
                // 合并组：仅在组首行输出一次，格式 08:00~10:00 xxx；非合并组按原格式 08:00 xxx
                var r = FindRangeContaining(h);
                if (r != null)
                {
                    if (h != r.StartHour) continue;
                    string content = dgvRecords.Rows[h].Cells[1].Value?.ToString()?.Trim() ?? string.Empty;
                    if (string.IsNullOrEmpty(content)) continue;
                    lines.Add($"{r.StartHour:00}:00~{r.EndHour:00}:00 {content}");
                }
                else
                {
                    string content = dgvRecords.Rows[h].Cells[1].Value?.ToString()?.Trim() ?? string.Empty;
                    if (!string.IsNullOrEmpty(content))
                    {
                        lines.Add($"{h:00}:00 {content}");
                    }
                }
            }
            Clipboard.SetText(string.Join("\n", lines));
            MessageBox.Show("已复制到剪贴板。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveDailyRecordsImpl();
            string dateStr = _lastLoadedDate.ToString("yyyy-MM-dd");
            using var sfd = new SaveFileDialog
            {
                Filter = "Excel 文件 (*.xlsx)|*.xlsx",
                FileName = $"时间记录_{dateStr}.xlsx",
                Title = "导出当天时间记录"
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                using var wb = new XLWorkbook();
                var ws = wb.AddWorksheet(dateStr);

                ws.Cell(1, 1).Value = "小时";
                ws.Cell(1, 2).Value = "内容";
                ws.Range(1, 1, 1, 2).Style.Font.Bold = true;

                int rowIdx = 2;
                for (int h = 0; h < 24; h++)
                {
                    var r = FindRangeContaining(h);
                    if (r != null)
                    {
                        if (h != r.StartHour) continue;
                        ws.Cell(rowIdx, 1).Value = $"{r.StartHour:00}:00~{r.EndHour:00}:00";
                        ws.Cell(rowIdx, 2).Value =
                            dgvRecords.Rows[h].Cells[1].Value?.ToString() ?? string.Empty;
                        rowIdx++;
                    }
                    else
                    {
                        ws.Cell(rowIdx, 1).Value = $"{h:00}:00";
                        ws.Cell(rowIdx, 2).Value =
                            dgvRecords.Rows[h].Cells[1].Value?.ToString() ?? string.Empty;
                        rowIdx++;
                    }
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(sfd.FileName);
                MessageBox.Show($"导出成功：\n{sfd.FileName}", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ===== Tab 3：日历视图 =====

        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            RefreshCalendarDetail(e.Start);
        }

        private void RefreshCalendarDetail(DateTime date)
        {
            string dateStr = date.ToString("yyyy-MM-dd");
            var lines = new List<string>
            {
                "━━━━━━━━━━━━━━━━━━━━━━",
                $"  {dateStr}  当日记录",
                "━━━━━━━━━━━━━━━━━━━━━━",
                ""
            };
            var records = DbHelper.GetNonEmptyRecords(dateStr);
            if (records.Count == 0)
            {
                lines.Add("（当日无记录）");
            }
            else
            {
                // 若就是当前加载日期，则先从 _mergeRanges 计算合并视图
                if (date.Date == _lastLoadedDate && _mergeRanges.Count > 0)
                {
                    var visited = new HashSet<int>();
                    foreach (var r in _mergeRanges)
                    {
                        // 以 StartHour 行内容为准（合并过肯定一致）
                        string c = dgvRecords.Rows[r.StartHour].Cells[1].Value
                            ?.ToString()?.Trim() ?? string.Empty;
                        for (int h = r.StartHour; h <= r.EndHour; h++) visited.Add(h);
                        if (!string.IsNullOrEmpty(c))
                            lines.Add($"{r.StartHour:00}:00~{r.EndHour:00}:00  {c}");
                    }
                    foreach (var (h, c) in records)
                    {
                        if (visited.Contains(h)) continue;
                        if (string.IsNullOrWhiteSpace(c)) continue;
                        lines.Add($"{h:00}:00  {c}");
                    }
                    // 排序输出（按起始小时）
                    lines = lines.Take(4).Concat(
                        lines.Skip(4).OrderBy(s =>
                        {
                            if (string.IsNullOrEmpty(s) || s.StartsWith("（")) return int.MaxValue;
                            int idx = s.IndexOf(':');
                            if (idx < 2) return int.MaxValue;
                            if (int.TryParse(s.Substring(0, 2), out int v)) return v;
                            return int.MaxValue;
                        })).ToList();
                }
                else
                {
                    foreach (var (h, c) in records)
                    {
                        lines.Add($"{h:00}:00  {c}");
                    }
                }
            }
            txtDayDetail.Text = string.Join("\r\n", lines);
            txtDayDetail.SelectionStart = 0;
            txtDayDetail.SelectionLength = 0;
        }

        #endregion

        #region ===== 退出（自动保存） =====

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 自动保存，不再提示
            SaveDailyRecordsImpl();
            // 关掉迷你窗
            if (_miniForm != null && !_miniForm.IsDisposed)
            {
                _miniForm.Close();
            }
        }

        #endregion
    }

    /// <summary>
    /// 连续小时合并区间（闭区间）
    /// </summary>
    internal sealed class MergeRange
    {
        public int StartHour { get; }
        public int EndHour { get; }
        public MergeRange(int start, int end)
        {
            if (start < 0 || end > 23 || start > end)
                throw new ArgumentOutOfRangeException(nameof(start));
            StartHour = start;
            EndHour = end;
        }
    }
}
