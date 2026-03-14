using StreamingDVR.Services;

namespace StreamingDVR.Forms
{
    /// <summary>
    /// Owner-drawn EPG grid. Channels on the Y axis, time on the X axis.
    /// </summary>
    public class EpgGridControl : UserControl
    {
        // ── layout constants ────────────────────────────────────────────────
        private const int ChannelColWidth = 160;
        private const int HeaderHeight    = 48;
        private const int RowHeight       = 46;
        private const int MinutePx        = 4;          // pixels per minute
        private const int HoursVisible    = 3;

        // ── data ────────────────────────────────────────────────────────────
        private List<EpgRow>  _rows      = new();
        private DateTime      _viewStart = DateTime.Now.Date.AddHours(DateTime.Now.Hour);
        private EpgProgram?   _hovered;
        private EpgProgram?   _selected;
        private ToolTip    _toolTip    = new();

        // ── scrollbars ───────────────────────────────────────────────────────
        private HScrollBar _hScroll = new();
        private VScrollBar _vScroll = new();

        // ── header panel (standard WinForms Panel — guaranteed to render) ────
        private Panel _headerPanel = new();

        public event EventHandler<EpgProgram>? ProgramDoubleClicked;

        // ── public surface ───────────────────────────────────────────────────
        public DateTime ViewStart
        {
            get => _viewStart;
            set { _viewStart = value; _hScroll.Value = 0; _headerPanel.Invalidate(); Invalidate(); }
        }

        public EpgGridControl()
        {
            DoubleBuffered = true;
            ResizeRedraw   = true;
            MinimumSize    = new Size(ChannelColWidth + 200, HeaderHeight + RowHeight + 40);

            // ── header panel ─────────────────────────────────────────────────
            _headerPanel.Dock        = DockStyle.Top;
            _headerPanel.Height      = HeaderHeight;
            _headerPanel.BackColor   = Color.FromArgb(42, 52, 88);
            _headerPanel.Paint      += HeaderPanel_Paint;

            // ── scrollbars ───────────────────────────────────────────────────
            _hScroll.Dock        = DockStyle.Bottom;
            _hScroll.Minimum     = 0;
            _hScroll.Maximum     = 100;
            _hScroll.SmallChange = 15;
            _hScroll.LargeChange = 60;
            _hScroll.Scroll     += (_, __) => { _headerPanel.Invalidate(); Invalidate(); };

            _vScroll.Dock        = DockStyle.Right;
            _vScroll.Minimum     = 0;
            _vScroll.Maximum     = 100;
            _vScroll.SmallChange = 1;
            _vScroll.LargeChange = 5;
            _vScroll.Scroll     += (_, __) => Invalidate();

            Controls.Add(_headerPanel);
            Controls.Add(_hScroll);
            Controls.Add(_vScroll);
        }

        // ── load data ────────────────────────────────────────────────────────
        public void LoadData(List<EpgRow> rows)
        {
            _rows     = rows;
            _hovered  = null;
            _selected = null;

            _vScroll.Value   = 0;

            // horizontal scroll covers ±12 hours from view start in minutes
            _hScroll.Maximum    = 12 * 60;
            _hScroll.Value      = 0;
            _hScroll.LargeChange = HoursVisible * 60;

            Invalidate();
        }

        // ── coordinate helpers ───────────────────────────────────────────────
        // GridHeight excludes the header panel (it's a separate child control now)
        private int GridWidth  => Width  - ChannelColWidth - _vScroll.Width;
        private int GridHeight => Height - HeaderHeight    - _hScroll.Height;

        private int MinutesOffset => _hScroll.Value;

        private DateTime WindowStart => _viewStart.AddMinutes(MinutesOffset);
        private DateTime WindowEnd   => WindowStart.AddHours(HoursVisible);

        private int TimeToX(DateTime t)
            => ChannelColWidth + (int)((t - WindowStart).TotalMinutes * MinutePx);

        private Rectangle ProgramRect(EpgProgram prog, int rowIndex)
        {
            int visibleRow = rowIndex - _vScroll.Value;
            int x1 = Math.Max(ChannelColWidth, TimeToX(prog.Start));
            int x2 = Math.Min(ChannelColWidth + GridWidth, TimeToX(prog.End));
            if (x2 <= x1) return Rectangle.Empty;
            int y  = HeaderHeight + visibleRow * RowHeight;
            return new Rectangle(x1, y, x2 - x1, RowHeight);
        }

        // ── painting ────────────────────────────────────────────────────────

        // Suppress the default background fill — we own every pixel in OnPaint.
        protected override void OnPaintBackground(PaintEventArgs e) { }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.ResetClip();
            g.Clear(Color.FromArgb(28, 28, 38));

            if (GridWidth <= 0 || GridHeight <= 0)
                return;

            DrawChannelColumn(g);
            DrawTimeGridLines(g);
            DrawPrograms(g);
            DrawCurrentTimeLine(g);
        }

        // ── header panel paint (standard WinForms Panel — always renders) ────
        private void HeaderPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g     = e.Graphics;
            int right = _headerPanel.Width - _vScroll.Width;

            // Corner background
            using var cornerBrush = new SolidBrush(Color.FromArgb(30, 34, 56));
            g.FillRectangle(cornerBrush, 0, 0, ChannelColWidth, HeaderHeight);

            // "Channel" label
            using var cornerFont = new Font("Segoe UI", 8f, FontStyle.Italic);
            TextRenderer.DrawText(g, "Channel", cornerFont,
                new Rectangle(4, 0, ChannelColWidth - 8, HeaderHeight),
                Color.FromArgb(180, 190, 215),
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            // Bottom separator
            using var sepPen = new Pen(Color.FromArgb(80, 100, 160), 2);
            g.DrawLine(sepPen, 0, HeaderHeight - 1, _headerPanel.Width, HeaderHeight - 1);

            // Time marks
            using var hourFont   = new Font("Segoe UI", 9f, FontStyle.Bold);
            using var halfFont   = new Font("Segoe UI", 8f);
            using var hourPen    = new Pen(Color.FromArgb(140, 160, 220), 1);
            using var halfPen    = new Pen(Color.FromArgb(90, 105, 150), 1);
            using var quarterPen = new Pen(Color.FromArgb(60,  70, 105), 1);

            var windowStart = WindowStart;
            int totalMins   = HoursVisible * 60;

            for (int m = 0; m <= totalMins; m += 15)
            {
                var t = windowStart.AddMinutes(m);
                int x = TimeToX(t);
                if (x < ChannelColWidth || x >= right) continue;

                if (t.Minute == 0)
                {
                    g.DrawLine(hourPen, x, 2, x, HeaderHeight - 3);

                    string lbl    = t.Hour == 0 ? t.ToString("d MMM HH:mm") : t.ToString("HH:mm");
                    int    nextX  = TimeToX(t.AddHours(1));
                    int    labelW = Math.Min(nextX - x - 4, right - x - 4);
                    if (labelW > 10)
                        TextRenderer.DrawText(g, lbl, hourFont,
                            new Rectangle(x + 4, 2, labelW, HeaderHeight - 4),
                            Color.White,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left |
                            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
                }
                else if (t.Minute == 30)
                {
                    int tickY  = HeaderHeight / 2;
                    g.DrawLine(halfPen, x, tickY, x, HeaderHeight - 3);

                    int nextX  = TimeToX(t.AddMinutes(15));
                    int labelW = Math.Min(nextX - x - 4, right - x - 4);
                    if (labelW > 20)
                        TextRenderer.DrawText(g, t.ToString("HH:mm"), halfFont,
                            new Rectangle(x + 3, tickY, labelW, HeaderHeight - tickY - 2),
                            Color.FromArgb(210, 220, 245),
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left |
                            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
                }
                else
                {
                    g.DrawLine(quarterPen, x, HeaderHeight * 3 / 4, x, HeaderHeight - 3);
                }
            }
        }

        /// <summary>Draws faint vertical lines at hour and half-hour boundaries through the program grid.</summary>
        private void DrawTimeGridLines(Graphics g)
        {
            if (GridHeight <= 0) return;

            g.SetClip(new Rectangle(ChannelColWidth, HeaderHeight, GridWidth, GridHeight));

            using var hourPen = new Pen(Color.FromArgb(55, 55, 75), 1);
            using var halfPen = new Pen(Color.FromArgb(42, 42, 58), 1);

            int totalMinutes = HoursVisible * 60;
            var windowStart  = WindowStart;
            int bottom       = HeaderHeight + GridHeight;

            for (int m = 0; m <= totalMinutes; m += 15)
            {
                var t = windowStart.AddMinutes(m);
                int x = TimeToX(t);
                if (x < ChannelColWidth || x > ChannelColWidth + GridWidth) continue;

                bool isHour = t.Minute == 0;
                bool isHalf = t.Minute == 30;

                if (isHour)
                    g.DrawLine(hourPen, x, HeaderHeight, x, bottom);
                else if (isHalf)
                    g.DrawLine(halfPen, x, HeaderHeight, x, bottom);
            }

            g.ResetClip();
        }

        private void DrawChannelColumn(Graphics g)
        {
            using var bg      = new SolidBrush(Color.FromArgb(40, 40, 55));
            using var fg      = new SolidBrush(Color.White);
            using var divPen  = new Pen(Color.FromArgb(70, 70, 90), 1);
            using var font    = new Font("Segoe UI", 9f);
            using var clipRgn = new Region(new Rectangle(0, HeaderHeight, ChannelColWidth, GridHeight));

            g.SetClip(clipRgn, System.Drawing.Drawing2D.CombineMode.Replace);
            g.FillRectangle(bg, 0, HeaderHeight, ChannelColWidth, GridHeight);

            int startRow = _vScroll.Value;
            int endRow   = Math.Min(_rows.Count - 1, startRow + GridHeight / RowHeight + 1);

            for (int i = startRow; i <= endRow; i++)
            {
                int visRow = i - startRow;
                int y = HeaderHeight + visRow * RowHeight;
                string name = _rows[i].ChannelName;
                var rf = new RectangleF(4, y + 2, ChannelColWidth - 8, RowHeight - 4);
                g.DrawString(name, font, fg, rf,
                    new StringFormat { Trimming = StringTrimming.EllipsisCharacter,
                                       LineAlignment = StringAlignment.Center });
                g.DrawLine(divPen, 0, y + RowHeight - 1, ChannelColWidth, y + RowHeight - 1);
            }

            g.ResetClip();
        }

        private void DrawPrograms(Graphics g)
        {
            using var clipRgn = new Region(
                new Rectangle(ChannelColWidth, HeaderHeight, GridWidth, GridHeight));
            g.SetClip(clipRgn, System.Drawing.Drawing2D.CombineMode.Replace);

            using var normalBrush   = new SolidBrush(Color.FromArgb(60, 80, 120));
            using var currentBrush  = new SolidBrush(Color.FromArgb(80, 110, 160));
            using var hoveredBrush  = new SolidBrush(Color.FromArgb(100, 140, 200));
            using var selectedBrush = new SolidBrush(Color.FromArgb(180, 120, 50));
            using var borderPen     = new Pen(Color.FromArgb(30, 30, 30), 1);
            using var textBrush     = new SolidBrush(Color.White);
            using var font          = new Font("Segoe UI", 8.5f);
            using var boldFont      = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var gridLinePen   = new Pen(Color.FromArgb(50, 50, 65), 1);

            var now = DateTime.Now;
            int startRow = _vScroll.Value;
            int endRow   = Math.Min(_rows.Count - 1, startRow + GridHeight / RowHeight + 1);

            for (int i = startRow; i <= endRow; i++)
            {
                int y = HeaderHeight + (i - startRow) * RowHeight;
                // row separator
                g.DrawLine(gridLinePen, ChannelColWidth, y + RowHeight - 1,
                           ChannelColWidth + GridWidth, y + RowHeight - 1);

                foreach (var prog in _rows[i].Programs)
                {
                    if (prog.End <= WindowStart || prog.Start >= WindowEnd) continue;

                    var rect = ProgramRect(prog, i);
                    if (rect == Rectangle.Empty || rect.Width < 2) continue;

                    bool isCurrent  = now >= prog.Start && now < prog.End;
                    bool isHovered  = prog == _hovered;
                    bool isSelected = prog == _selected;

                    Brush fill = isSelected ? selectedBrush
                               : isHovered  ? hoveredBrush
                               : isCurrent  ? currentBrush
                                            : normalBrush;

                    g.FillRectangle(fill, rect);
                    g.DrawRectangle(borderPen, rect);

                    if (rect.Width > 20)
                    {
                        var innerRect = new RectangleF(
                            rect.X + 4, rect.Y + 2, rect.Width - 8, rect.Height - 4);
                        var sf = new StringFormat
                        {
                            Trimming      = StringTrimming.EllipsisCharacter,
                            LineAlignment = StringAlignment.Center
                        };
                        g.DrawString(prog.Title, isCurrent ? boldFont : font,
                                     textBrush, innerRect, sf);
                    }
                }
            }

            g.ResetClip();
        }

        private void DrawCurrentTimeLine(Graphics g)
        {
            var now = DateTime.Now;
            if (now < WindowStart || now > WindowEnd) return;

            int x = TimeToX(now);
            using var pen = new Pen(Color.OrangeRed, 2);
            g.DrawLine(pen, x, 0, x, HeaderHeight + GridHeight);

            // triangle marker at top
            var tri = new Point[] {
                new(x - 5, 0), new(x + 5, 0), new(x, 10)
            };
            using var brush = new SolidBrush(Color.OrangeRed);
            g.FillPolygon(brush, tri);
        }

        // ── mouse interaction ────────────────────────────────────────────────
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var prog = HitTest(e.Location);
            if (prog != _hovered)
            {
                _hovered = prog;
                Invalidate();
                if (prog != null)
                    _toolTip.SetToolTip(this,
                        $"{prog.Title}\n{prog.Start:HH:mm} – {prog.End:HH:mm}" +
                        (string.IsNullOrEmpty(prog.Description) ? "" : $"\n{prog.Description}"));
                else
                    _toolTip.SetToolTip(this, "");
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            _selected = HitTest(e.Location);
            Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            var prog = HitTest(e.Location);
            if (prog != null) ProgramDoubleClicked?.Invoke(this, prog);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hovered = null;
            Invalidate();
        }

        private EpgProgram? HitTest(Point p)
        {
            if (p.X < ChannelColWidth || p.Y < HeaderHeight) return null;

            int visRow = (p.Y - HeaderHeight) / RowHeight;
            int row    = visRow + _vScroll.Value;
            if (row < 0 || row >= _rows.Count) return null;

            foreach (var prog in _rows[row].Programs)
            {
                var rect = ProgramRect(prog, row);
                if (rect != Rectangle.Empty && rect.Contains(p)) return prog;
            }
            return null;
        }

        // ── scroll wheel ─────────────────────────────────────────────────────
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int delta = e.Delta > 0 ? -3 : 3;
            _vScroll.Value = Math.Max(_vScroll.Minimum,
                             Math.Min(_vScroll.Maximum, _vScroll.Value + delta));
            Invalidate();
        }

        // ── resize ────────────────────────────────────────────────────────────
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _hScroll.LargeChange = Math.Max(1, GridWidth / MinutePx);
            Invalidate();
        }
    }

    // ── data models ──────────────────────────────────────────────────────────
    public class EpgRow
    {
        public string ChannelName { get; init; } = string.Empty;
        public int    StreamId    { get; init; }
        public List<EpgProgram> Programs { get; init; } = new();
    }

    public class EpgProgram
    {
        public string  Title       { get; init; } = string.Empty;
        public string? Description { get; init; }
        public DateTime Start      { get; init; }
        public DateTime End        { get; init; }
    }
}
