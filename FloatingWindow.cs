using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FloatingToolStripLibrary
{
    public partial class FloatingWindow : Form
    {
        public FloatingWindow(ToolStrip SourceToolStrip, ToolStripPanel[] ParentPanels, Form ParentForm, string Text, FloatingToolStrip ParentClass)
        {
            InitializeComponent(); 
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            //this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            _toolstrip = SourceToolStrip;
            _parentpanels = ParentPanels;
            _parentform = ParentForm;
            _parentclass = ParentClass;
            //_toolstripcontainer = ParentContainer;
            /*if (_toolstrip.Dock == DockStyle.None)
            {
                this.Controls.Add(_toolstrip);
                _toolstrip.Top = 16;
            }*/

            this.Text = Text;

            _toolstrip.MouseDown += new MouseEventHandler(_toolstrip_MouseDown);
            _toolstrip.MouseMove += new MouseEventHandler(_toolstrip_MouseMove);
            _toolstrip.MouseUp += new MouseEventHandler(_toolstrip_MouseUp);

            _toolstrip.BeginDrag += new EventHandler(_toolstrip_BeginDrag);
            _toolstrip.EndDrag += new EventHandler(_toolstrip_EndDrag);

            _parentform.Leave += new EventHandler(_parentform_Leave);
            _parentform.Enter += new EventHandler(_parentform_Enter);
            _parentform.FormClosed += new FormClosedEventHandler(_parentform_FormClosed);

            parent = _toolstrip.Parent;
            parentrect = parent.RectangleToScreen(parent.ClientRectangle);
            Point ShowLocation = MousePosition;
            if (!CheckCloss(parentrect, ShowLocation))
            {
                _toolstrip.GripStyle = ToolStripGripStyle.Hidden;
                //MessageBox.Show(ShowLocation.ToString());
                this.Location = ShowLocation;
                _toolstrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                this.Size = new Size(_toolstrip.Width + GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2,
                    _toolstrip.Height /*+ 20 + GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2*/);
                this.Controls.Add(_toolstrip);
                _toolstrip.Location = new Point(0, 20);
                //_toolstrip.Dock = DockStyle.Top;
                this.Show();
                _parentclass.showingform = true;
            }

            rectform = new RectForm(_parentclass.RangeColor, _parentclass.EdgeColor);

            //_toolstrip.DockChanged += new EventHandler(_toolstrip_DockChanged);
            //_toolstrip.LocationChanged += new EventHandler(_toolstrip_LocationChanged);
        }


        FloatingToolStrip _parentclass;

        public ToolStripPanel LastParent;
        public Point LastPoint;
        public ToolStripLayoutStyle LastStyle;

        void _parentform_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        void _parentform_Enter(object sender, EventArgs e)
        {
            this.Show();
        }

        void _parentform_Leave(object sender, EventArgs e)
        {
            this.Hide();
        }

        Form _parentform;

        public RectForm rectform;

        Rectangle CloseButtonRect;

        Control parent;
        Rectangle parentrect;
        Color PreviewBorderColor;
        void _toolstrip_BeginDrag(object sender, EventArgs e)
        {
            parent = _toolstrip.Parent;
            parentrect = parent.RectangleToScreen(parent.ClientRectangle);
        }

        void _toolstrip_EndDrag(object sender, EventArgs e)
        {/*
            //if (parent == _toolstrip.Parent)
            //{
                // フローティング開始
            parent = _toolstrip.Parent;
            parentrect = parent.RectangleToScreen(parent.ClientRectangle);
                Point ShowLocation = MousePosition;
                if (!CheckCloss(parentrect, ShowLocation))
                {
                    //MessageBox.Show(ShowLocation.ToString());
                    this.Location = ShowLocation;
                    _toolstrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                    this.Size = new Size(_toolstrip.Width + GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2,
                        _toolstrip.Height /*+ 20 + GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2*/
            //);
            /*       this.Controls.Add(_toolstrip);
                   _toolstrip.Location = new Point(0, 20);
                   //_toolstrip.Dock = DockStyle.Top;
                   this.Show();
               }*/
            //}
        }

        void _toolstrip_LocationChanged(object sender, EventArgs e)
        {

        }

        void _toolstrip_DockChanged(object sender, EventArgs e)
        {

        }

        private bool CheckCloss(Rectangle ctrlrect, Point CheckPoint)
        {
            // Rectangle ctrlrect = control.ClientRectangle;
            return ctrlrect.IntersectsWith(new Rectangle(CheckPoint, new Size(1, 1)));
            /*
            // 以下ボツだけど一応残す
            if (ctrlrect.X <= CheckPoint.X && ctrlrect.X + ctrlrect.Width >= CheckPoint.X &&    // X座標
                ctrlrect.Y <= CheckPoint.Y && ctrlrect.Y + ctrlrect.Height >= CheckPoint.Y)     // Y座標
            {
                return true;
            }
            else
            {
                return false;
            }*/
        }

        ToolStrip _toolstrip;
        ToolStripPanel[] _parentpanels;
        //FloatingToolStripContainer _toolstripcontainer;

        #region サイズ変更枠のみのフォーム(移動処理, スナップ機能込)
        const int CS_DROPSHADOW = 0x00020000;
        const int WS_BORDER = 0x00800000;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (this.FormBorderStyle != FormBorderStyle.None)
                {
                    cp.Style = cp.Style & (~WS_BORDER);
                }
                return cp;
            }
        }

        #region サイズ調整
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric smIndex);
        public enum SystemMetric : int
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
            SM_CYVSCROLL = 2,
            SM_CXVSCROLL = 3,
            SM_CYCAPTION = 4,
            SM_CXBORDER = 5,
            SM_CYBORDER = 6,
            SM_CXDLGFRAME = 7,
            SM_CYDLGFRAME = 8,
            SM_CYVTHUMB = 9,
            SM_CXHTHUMB = 10,
            SM_CXICON = 11,
            SM_CYICON = 12,
            SM_CXCURSOR = 13,
            SM_CYCURSOR = 14,
            SM_CYMENU = 15,
            SM_CXFULLSCREEN = 16,
            SM_CYFULLSCREEN = 17,
            SM_CYKANJIWINDOW = 18,
            SM_MOUSEWHEELPRESENT = 75,
            SM_CYHSCROLL = 20,
            SM_CXHSCROLL = 21,
            SM_DEBUG = 22,
            SM_SWAPBUTTON = 23,
            SM_RESERVED1 = 24,
            SM_RESERVED2 = 25,
            SM_RESERVED3 = 26,
            SM_RESERVED4 = 27,
            SM_CXMIN = 28,
            SM_CYMIN = 29,
            SM_CXSIZE = 30,
            SM_CYSIZE = 31,
            SM_CXFRAME = 32,
            SM_CYFRAME = 33,
            SM_CXMINTRACK = 34,
            SM_CYMINTRACK = 35,
            SM_CXDOUBLECLK = 36,
            SM_CYDOUBLECLK = 37,
            SM_CXICONSPACING = 38,
            SM_CYICONSPACING = 39,
            SM_MENUDROPALIGNMENT = 40,
            SM_PENWINDOWS = 41,
            SM_DBCSENABLED = 42,
            SM_CMOUSEBUTTONS = 43,
            SM_CXFIXEDFRAME = SM_CXDLGFRAME,
            SM_CYFIXEDFRAME = SM_CYDLGFRAME,
            SM_CXSIZEFRAME = SM_CXFRAME,
            SM_CYSIZEFRAME = SM_CYFRAME,
            SM_SECURE = 44,
            SM_CXEDGE = 45,
            SM_CYEDGE = 46,
            SM_CXMINSPACING = 47,
            SM_CYMINSPACING = 48,
            SM_CXSMICON = 49,
            SM_CYSMICON = 50,
            SM_CYSMCAPTION = 51,
            SM_CXSMSIZE = 52,
            SM_CYSMSIZE = 53,
            SM_CXMENUSIZE = 54,
            SM_CYMENUSIZE = 55,
            SM_ARRANGE = 56,
            SM_CXMINIMIZED = 57,
            SM_CYMINIMIZED = 58,
            SM_CXMAXTRACK = 59,
            SM_CYMAXTRACK = 60,
            SM_CXMAXIMIZED = 61,
            SM_CYMAXIMIZED = 62,
            SM_NETWORK = 63,
            SM_CLEANBOOT = 67,
            SM_CXDRAG = 68,
            SM_CYDRAG = 69,
            SM_SHOWSOUNDS = 70,
            SM_CXMENUCHECK = 71,
            SM_CYMENUCHECK = 72,
            SM_SLOWMACHINE = 73,
            SM_MIDEASTENABLED = 74,
            SM_MOUSEPRESENT = 19,
            SM_XVIRTUALSCREEN = 76,
            SM_YVIRTUALSCREEN = 77,
            SM_CXVIRTUALSCREEN = 78,
            SM_CYVIRTUALSCREEN = 79,
            SM_CMONITORS = 80,
            SM_SAMEDISPLAYFORMAT = 81,
            SM_IMMENABLED = 82,
            SM_CXFOCUSBORDER = 83,
            SM_CYFOCUSBORDER = 84,
            SM_TABLETPC = 86,
            SM_MEDIACENTER = 87,
            SM_CMETRICS_OTHER = 76,
            SM_CMETRICS_2000 = 83,
            SM_CMETRICS_NT = 88,
            SM_REMOTESESSION = 0x1000,
            SM_SHUTTINGDOWN = 0x2000,
            SM_REMOTECONTROL = 0x2001,
        }
        #endregion

        const int WM_NCHITTEST = 0x0084;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_NCMOUSEMOVE = 0x00A0;
        const int WM_NCLBUTTONUP = 0x00A2;
        const int WM_NCLBUTTONDBLCLK = 0x00A3;
        const int HTCLIENT = 1;
        const int HTCAPTION = 2;
        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 16;
        const int HTBOTTOMRIGHT = 17;

        const int WM_MOVING = 0x0216;
        const int WM_ENTERSIZEMOVE = 0x0231;

        //Point mouseStart;
        //Point rectStart;
        Size Gap = new Size(12, 12);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        protected override void WndProc(ref Message m)
        {
            #region move
            //Console.WriteLine(m.Msg);
            /*
            if (m.Msg == WM_MOVING)
            {
                Rectangle rect1 = new Rectangle(rectStart.X + Cursor.Position.X - mouseStart.X,
                rectStart.Y + Cursor.Position.Y - mouseStart.Y,
                Gap.Width, Gap.Height);

                Rectangle WorkingArea = Screen.GetWorkingArea(this);

                Rectangle rect_x = new Rectangle(0, 0, Gap.Width, WorkingArea.Height);
                Rectangle rect_x_e = new Rectangle(WorkingArea.Width - Gap.Width, 0,
                    Gap.Width, WorkingArea.Height);

                Rectangle rect_y = new Rectangle(0, 0, WorkingArea.Width, Gap.Height);
                Rectangle rect_y_e = new Rectangle(0, WorkingArea.Height - Gap.Height,
                    WorkingArea.Width, Gap.Height);

                RECT r = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));

                // 横方向
                bool do1 = false;
                rect1.Width = Gap.Width;
                rect1.Height = this.Height;
                if (rect1.IntersectsWith(rect_x))
                {
                    r.Left = rect_x.Left;
                    r.Right = r.Left + this.Width;
                    Cursor.Position = new Point(Cursor.Position.X - (this.Left - r.Left), Cursor.Position.Y);
                    Gap.Width = 48;
                    do1 = true;
                }

                if (new Rectangle(rect1.X + this.Width - Gap.Width, rect1.Y, Gap.Width, rect1.Height).IntersectsWith(rect_x_e) && !do1)
                {
                    r.Left = rect_x_e.Right - this.Width;
                    r.Right = r.Left + this.Width;
                    Cursor.Position = new Point(Cursor.Position.X - (this.Right - r.Right), Cursor.Position.Y);
                    Gap.Width = 48;
                    do1 = true;
                }

                if (!do1)
                {
                    r.Left = rect1.Left;
                    r.Right = r.Left + this.Width;
                    Gap.Width = 12;
                }

                // 縦方向
                do1 = false;
                rect1.Width = this.Width;
                rect1.Height = Gap.Height;
                if (rect1.IntersectsWith(rect_y))
                {
                    r.Top = rect_y.Top;
                    r.Bottom = r.Top + this.Height;
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - (this.Top - r.Top));
                    Gap.Height = 48;
                    do1 = true;
                }

                if (new Rectangle(rect1.X, rect1.Y + this.Height - Gap.Height, rect1.Width, Gap.Height).IntersectsWith(rect_y_e) && !do1)
                {
                    r.Top = rect_y_e.Bottom - this.Height;
                    r.Bottom = r.Top + this.Height;
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - (this.Bottom - r.Bottom));
                    Gap.Height = 48;
                    do1 = true;
                }

                if (!do1)
                {
                    r.Top = rect1.Top;
                    r.Bottom = r.Top + this.Height;
                    Gap.Height = 12;
                }
                
                Marshal.StructureToPtr(r, m.LParam, false);
            }
            else if (m.Msg == WM_ENTERSIZEMOVE)
            {
                mouseStart = Cursor.Position;
                rectStart = this.Location;
            }*/
            #endregion

            base.WndProc(ref m);
            /*
            if (m.Msg == WM_NCMOUSEMOVE)
            {
                if (OnCloseButton && (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
                {
                    this.Hide();
                }
            }
            */
            if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                // OnDoubleClick
                _toolstrip.GripStyle = ToolStripGripStyle.Visible;
                _toolstrip.LayoutStyle = LastStyle;

                _toolstrip.BeginDrag -= this._toolstrip_BeginDrag;
                _toolstrip.EndDrag -= this._toolstrip_EndDrag;
                _toolstrip.MouseDown -= this._toolstrip_MouseDown;
                _toolstrip.MouseMove -= this._toolstrip_MouseMove;
                _toolstrip.MouseUp -= this._toolstrip_MouseUp;

                _toolstrip.Location = LastPoint;
                LastParent.Controls.Add(_toolstrip);
                rectform.Hide();
                this.Close();
            }

            if (m.Msg == WM_NCHITTEST)
            {            // 0x84 : WM_NCHITTEST
                //MessageBox.Show(m.LParam.ToString());
                //Console.WriteLine(m.LParam.ToString() + "  Result  " + m.Result.ToString());
                if (m.Result == (IntPtr)HTLEFT ||
                    m.Result == (IntPtr)HTRIGHT ||
                    m.Result == (IntPtr)HTTOP ||
                    m.Result == (IntPtr)HTBOTTOM ||
                    m.Result == (IntPtr)HTTOPLEFT ||
                    m.Result == (IntPtr)HTTOPRIGHT ||
                    m.Result == (IntPtr)HTBOTTOMLEFT ||
                    m.Result == (IntPtr)HTBOTTOMRIGHT)
                {
                    m.Result = (IntPtr)HTCAPTION;
                    OnCloseButton = false;//CloseButtonRect.IntersectsWith(new Rectangle(this.PointToClient(Cursor.Position), new Size(1, 1)));
                    Refresh();
                }
                else if (m.Result.ToInt32() == HTCLIENT)
                {
                    //if (this.PointToClient(MousePosition).Y < 16)
                    //{
                    if (!OnCloseButton)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }
                    OnCloseButton = CloseButtonRect.IntersectsWith(new Rectangle(this.PointToClient(Cursor.Position), new Size(1, 1)));
                    Refresh();

                    //}
                }
            }

        }

        [DllImport("user32.dll")]
        extern static System.IntPtr SendMessage(
        System.IntPtr hWnd, // 送信先ウィンドウのハンドル
        System.UInt32 Msg, // メッセージ
        System.IntPtr wParam, // メッセージの最初のパラメータ
        System.UIntPtr lParam // メッセージの 2 番目のパラメータ
        );

        private Point mousePoint;
        void _toolstrip_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }
        }

        void _toolstrip_MouseMove(object sender, MouseEventArgs e)
        {

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //_toolstrip.Left += e.X - mousePoint.X;
                //_toolstrip.Top += e.Y - mousePoint.Y;
                //または、つぎのようにする
                /*if (_toolstrip.Dock == DockStyle.None)
                {
                    this._toolstrip.Location = new Point(this._toolstrip.Location.X + e.X - mousePoint.X,
                        this._toolstrip.Location.Y + e.Y - mousePoint.Y);
                }
                else */
                /*if (_toolstrip.Dock == DockStyle.Top)
                {
                    this.Location = new Point(
                        this.Location.X + e.X - mousePoint.X,
                        this.Location.Y + e.Y - mousePoint.Y);
                }*/
            }

        }

        void _toolstrip_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                _toolstrip.Left = 0;
                _toolstrip.Top = 0;
                _toolstrip.Dock = DockStyle.Top;
            }
            */
        }
        #endregion

        bool OnCloseButton = false;

        private void FloatingToolStrip_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(SystemColors.WindowFrame), new Rectangle(0, 0, this.Width, 20));

            g.DrawString(this.Text, this.Font, new SolidBrush(Color.FromArgb(128, Color.DarkGray)), 5, 5);
            g.DrawString(this.Text, this.Font, new SolidBrush(SystemColors.Window), 4, 4);

            CloseButtonRect = new Rectangle(this.Width - 36, 1, 16, 16);
            if (OnCloseButton)
            {
                g.FillRectangle(new SolidBrush(SystemColors.GradientActiveCaption), CloseButtonRect);
                g.DrawRectangle(new Pen(SystemColors.MenuHighlight, 1), CloseButtonRect);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawLine(new Pen(SystemColors.WindowText, 2), new Point(CloseButtonRect.Left + 4, CloseButtonRect.Top + 4), new Point(CloseButtonRect.Right - 4, CloseButtonRect.Bottom - 4));
                g.DrawLine(new Pen(SystemColors.WindowText, 2), new Point(CloseButtonRect.Left + 4, CloseButtonRect.Bottom - 4), new Point(CloseButtonRect.Right - 4, CloseButtonRect.Top + 4));
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
            else
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawLine(new Pen(SystemColors.Window, 2), new Point(CloseButtonRect.Left + 4, CloseButtonRect.Top + 4), new Point(CloseButtonRect.Right - 4, CloseButtonRect.Bottom - 4));
                g.DrawLine(new Pen(SystemColors.Window, 2), new Point(CloseButtonRect.Left + 4, CloseButtonRect.Bottom - 4), new Point(CloseButtonRect.Right - 4, CloseButtonRect.Top + 4));
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
        }

        enum DockTo
        {
            Top,
            Bottom,
            Left,
            Right,
            Float,
            None
        }

        DockTo dockto;
        bool candockcheck = false;
        int dockindex = -1;

        private void FloatingToolStrip_Move(object sender, EventArgs e)
        {
            _toolstrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            bool CanDock = false;
            if (candockcheck)
            {
                for (int i = 0; i < _parentpanels.Length; i++)
                {

                    Point ParentPoint = _parentpanels[i].Parent.PointToScreen(_parentpanels[i].Location);
                    Rectangle ParentRect = _parentpanels[i].Parent.RectangleToScreen(_parentpanels[i].ClientRectangle);
                    /*
                    Rectangle toprect = new Rectangle(ParentPoint, new Size(_parentpanels[i].Width, _parentpanels[i].Height + _toolstrip.Height)); //_parentcontainer.RectangleToScreen(_parentcontainer.TopToolStripPanel.ClientRectangle);

                    Rectangle bottomrect = new Rectangle(new Point(ParentPoint.X, ParentPoint.Y + _parentcontainer.Height - _toolstrip.Height - _parentcontainer.BottomToolStripPanel.Height),
                        new Size(_parentcontainer.Width, _parentcontainer.BottomToolStripPanel.Height + _toolstrip.Height));//_parentcontainer.RectangleToScreen(_parentcontainer.BottomToolStripPanel.ClientRectangle);

                    Rectangle leftrect = new Rectangle(ParentPoint, new Size(_parentcontainer.LeftToolStripPanel.Width + _toolstrip.Height, _parentcontainer.Height)); //_parentcontainer.RectangleToScreen(_parentcontainer.LeftToolStripPanel.ClientRectangle);

                    Rectangle rightrect = new Rectangle(new Point(ParentPoint.X + _parentcontainer.Width - _toolstrip.Height - _parentcontainer.RightToolStripPanel.Width, ParentPoint.Y/*_parentcontainer.Height),
                        new Size(_parentcontainer.RightToolStripPanel.Width + _toolstrip.Height, _parentcontainer.Height));//_parentcontainer.RectangleToScreen(_parentcontainer.RightToolStripPanel.ClientRectangle);
                    */
                    //Console.WriteLine(bottomrect.ToString());

                    bool Virtual = false;

                    dockto = DockTo.None;

                    switch (_parentpanels[i].Dock)
                    {
                        case DockStyle.Top:
                            ParentRect.Height += _toolstrip.Height;
                            dockto = DockTo.Top;
                            break;
                        case DockStyle.Bottom:
                            ParentRect.Height += _toolstrip.Height;
                            ParentRect.Y += _parentpanels[i].Parent.Height - ParentRect.Height;
                            if (_parentpanels[i].Parent is Form)
                            {
                                ParentRect.Y = ParentRect.Y - GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2 - GetSystemMetrics(SystemMetric.SM_CYCAPTION);
                            }
                            dockto = DockTo.Bottom;
                            break;
                        case DockStyle.Left:
                            ParentRect.Width += _toolstrip.Height;
                            Virtual = true;
                            dockto = DockTo.Left;
                            break;
                        case DockStyle.Right:
                            ParentRect.Width += _toolstrip.Height;
                            ParentRect.X += _parentpanels[i].Parent.Width - ParentRect.Width;
                            Virtual = true;
                            if (_parentpanels[i].Parent is Form)
                            {
                                ParentRect.X = ParentRect.X - GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2;
                            }
                            dockto = DockTo.Right;
                            break;
                    }
                    if (CheckCloss(ParentRect, MousePosition))
                    {
                        DrawRectangleToScreen(_parentpanels[i], Virtual, ParentRect);
                        CanDock = true;
                        dockindex = i;
                        break;
                    }
                    else
                    {
                        dockindex = -1;
                        dockto = DockTo.Float;
                    }
                    #region botsu
                    /*
                    if (CheckCloss(toprect, MousePosition) &&
                        _parentcontainer.TopToolStripPanelVisible)
                    {
                        dockto = DockTo.Top;
                        DrawRectangleToScreen(_parentcontainer.TopToolStripPanel, false, toprect);
                        //MessageBox.Show("TOP");
                    }
                    else if (CheckCloss(bottomrect, MousePosition) &&
                        _parentcontainer.BottomToolStripPanelVisible)
                    {
                        dockto = DockTo.Bottom;
                        DrawRectangleToScreen(_parentcontainer.BottomToolStripPanel, false, bottomrect);
                        //MessageBox.Show("Bottom");
                    }
                    else if (CheckCloss(leftrect, MousePosition) &&
                        _parentcontainer.LeftToolStripPanelVisible)
                    {
                        dockto = DockTo.Left;
                        DrawRectangleToScreen(_parentcontainer.LeftToolStripPanel, true, leftrect);
                        //MessageBox.Show("Left");
                    }
                    else if (CheckCloss(rightrect, MousePosition) &&
                        _parentcontainer.RightToolStripPanelVisible)
                    {
                        dockto = DockTo.Right;
                        DrawRectangleToScreen(_parentcontainer.RightToolStripPanel, true, rightrect);
                        //MessageBox.Show("Right");
                    }
                    else
                    {
                        dockto = DockTo.Float;
                        rectform.Hide();
                        this.Opacity = 1.0;
                    }
                     * */
                    #endregion

                }
                if (CanDock)
                {

                }
                else
                {
                    rectform.Hide();
                    this.Opacity = 1.0;
                }
            }
            else
            {
                candockcheck = true;
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        private static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport("user32.dll")]
        private static extern bool UpdateWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool InvalidateRect(
                       IntPtr hWnd,
                       ref Rectangle lpRect,
                       bool bErase);
        [DllImport("user32.dll")]
        public extern static IntPtr GetDesktopWindow();


        //RedrawWindow() flags
        private enum RDW : int
        {
            INVALIDATE = 0x1,
            INTERNALPAINT = 0x2,
            ERASE = 0x4,
            VALIDATE = 0x8,
            NOINTERNALPAINT = 0x10,
            NOERASE = 0x20,
            NOCHILDREN = 0x40,
            ALLCHILDREN = 0x80,
            UPDATENOW = 0x100,
            ERASENOW = 0x200,
            FRAME = 0x400,
            NOFRAME = 0x800
        }


        private void DrawRectangleToScreen(ToolStripPanel parent, bool Virtual, Rectangle rect)
        {
            #region 邪魔
            /*  // 失せろ
            IntPtr hDC = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(hDC);
            //IntPtr hDC = g.GetHdc();

            g.DrawRectangle(Pens.BlueViolet, new Rectangle(this.Left + this.PointToClient(MousePosition).X, this.Top + this.PointToClient(MousePosition).Y, this.Width, this.Height));

            //g.ReleaseHdc(hDC);
            g.Dispose();
            *
            this.Hide();
            RectangleForm rectf = new RectangleForm(_parentcontainer, this);
            rectf.Location = new Point(this.Left + this.PointToClient(MousePosition).X, this.Top + this.PointToClient(MousePosition).Y);
            rectf.Size = _toolstrip.Size;

            rectf.Show();
            rectf.Activate();
            */
            //RefreshDesktop();
            //_parentcontainer.TopToolStripPanel.Refresh();
            //_parentform.Refresh();
            //InvalidateRect(IntPtr.Zero, ref rect, true);
            //UpdateWindow(IntPtr.Zero);

            /*
            IntPtr hdc = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(hdc);

            g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.BlueViolet)), rect);

            g.Dispose();
             */
            //NativeWindow n = new NativeWindow();


            // ReleaseDC(IntPtr.Zero, hdc);
            #endregion

            rectform.Location = rect.Location;//parent.Parent.PointToScreen(parent.Location);
            rectform.Size = rect.Size;//viewsize;
            rectform.Show();
            rectform.Refresh();
            this.Opacity = 0.3;
        }

        private void RefreshDesktop()
        {
            RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero,
                (uint)(/*RDW.ALLCHILDREN | */RDW.UPDATENOW | RDW.ERASE | RDW.INVALIDATE | RDW.INTERNALPAINT));
            UpdateWindow(IntPtr.Zero);
        }

        private void FloatingToolStrip_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void FloatingToolStrip_ResizeEnd(object sender, EventArgs e)
        {
            //for (int i = 0; i < _parentpanels.Length; i++)
            //{
            if (dockindex != -1)
            {
                if (dockto == DockTo.Top)
                {
                    this.Opacity = 1.0;
                    _toolstrip.GripStyle = ToolStripGripStyle.Visible;
                    _toolstrip.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;

                    _toolstrip.BeginDrag -= this._toolstrip_BeginDrag;
                    _toolstrip.EndDrag -= this._toolstrip_EndDrag;
                    _toolstrip.MouseDown -= this._toolstrip_MouseDown;
                    _toolstrip.MouseMove -= this._toolstrip_MouseMove;
                    _toolstrip.MouseUp -= this._toolstrip_MouseUp;

                    _toolstrip.Location = _parentpanels[dockindex].PointToClient(Cursor.Position);
                    _parentpanels[dockindex].Controls.Add(_toolstrip);

                    _parentclass.showingform = false;
                    _parentclass._parentform.Activate();
                    rectform.Hide();
                    this.Hide();
                    //this.Close();
                }
                else if (dockto == DockTo.Bottom)
                {
                    this.Opacity = 1.0;
                    _toolstrip.GripStyle = ToolStripGripStyle.Visible;
                    _toolstrip.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;

                    _toolstrip.BeginDrag -= this._toolstrip_BeginDrag;
                    _toolstrip.EndDrag -= this._toolstrip_EndDrag;
                    _toolstrip.MouseDown -= this._toolstrip_MouseDown;
                    _toolstrip.MouseMove -= this._toolstrip_MouseMove;
                    _toolstrip.MouseUp -= this._toolstrip_MouseUp;

                    _toolstrip.Location = _parentpanels[dockindex].PointToClient(Cursor.Position);
                    _parentpanels[dockindex].Controls.Add(_toolstrip);

                    _parentclass.showingform = false;
                    _parentclass._parentform.Activate();
                    rectform.Hide();
                    this.Hide();
                    //this.Close();
                }
                else if (dockto == DockTo.Left)
                {
                    this.Opacity = 1.0;
                    _toolstrip.GripStyle = ToolStripGripStyle.Visible;
                    _toolstrip.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;

                    _toolstrip.BeginDrag -= this._toolstrip_BeginDrag;
                    _toolstrip.EndDrag -= this._toolstrip_EndDrag;
                    _toolstrip.MouseDown -= this._toolstrip_MouseDown;
                    _toolstrip.MouseMove -= this._toolstrip_MouseMove;
                    _toolstrip.MouseUp -= this._toolstrip_MouseUp;

                    _toolstrip.Location = _parentpanels[dockindex].PointToClient(Cursor.Position);
                    _parentpanels[dockindex].Controls.Add(_toolstrip);

                    _parentclass.showingform = false;
                    _parentclass._parentform.Activate();
                    rectform.Hide();
                    this.Hide();
                    //this.Close();
                }
                else if (dockto == DockTo.Right)
                {
                    this.Opacity = 1.0;
                    _toolstrip.GripStyle = ToolStripGripStyle.Visible;
                    _toolstrip.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;

                    _toolstrip.BeginDrag -= this._toolstrip_BeginDrag;
                    _toolstrip.EndDrag -= this._toolstrip_EndDrag;
                    _toolstrip.MouseDown -= this._toolstrip_MouseDown;
                    _toolstrip.MouseMove -= this._toolstrip_MouseMove;
                    _toolstrip.MouseUp -= this._toolstrip_MouseUp;

                    _toolstrip.Location = _parentpanels[dockindex].PointToClient(Cursor.Position);
                    _parentpanels[dockindex].Controls.Add(_toolstrip);

                    _parentclass.showingform = false;
                    _parentclass._parentform.Activate();
                    rectform.Hide();
                    this.Hide();
                    //this.Close();
                }
                else if (dockto == DockTo.Float)
                {
                    this.Opacity = 1.0;
                    rectform.Hide();
                }
            }
            // }
        }

        private void FloatingWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parentform.RemoveOwnedForm(this);
            rectform.Close();
            _parentform.Activate();
        }

        private void FloatingWindow_Shown(object sender, EventArgs e)
        {
            this.ResizeEnd += this.FloatingToolStrip_ResizeEnd;
        }

        private void FloatingWindow_MouseMove(object sender, MouseEventArgs e)
        {
            //CloseButtonRect = new Rectangle(this.Width - 30, 4, 12, 12);
        }

        private void FloatingWindow_MouseLeave(object sender, EventArgs e)
        {
            OnCloseButton = false;//CloseButtonRect.IntersectsWith(new Rectangle(this.PointToClient(Cursor.Position), new Size(1, 1)));
            Refresh();
        }

        private void FloatingWindow_MouseClick(object sender, MouseEventArgs e)
        {
            _parentclass.HideToolStrip();
            _parentform.Activate();
        }
    }
}
