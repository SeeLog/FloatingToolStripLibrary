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
    public partial class FloatingToolStrip
    {
        /// <summary>
        /// FloatingToolStripLibrary.FloatingToolStrip クラスの新しいインスタンスを初期化します．
        /// </summary>
        /// <param name="SourceToolStrip">対象の ToolStrip コントロール</param>
        /// <param name="ParentPanels">ドッキング先のパネルの配列</param>
        /// <param name="ParentForm">親フォーム</param>
        /// <param name="Text">フローティング時に表示するテキスト</param>
        public FloatingToolStrip(ToolStrip SourceToolStrip, ToolStripPanel[] ParentPanels, Form ParentForm, string Text)
        {
            _toolstrip = SourceToolStrip;
            _parentpanels = ParentPanels;
            //_toolstrip.BeginDrag += new EventHandler(_toolstrip_BeginDrag);
            _toolstrip.EndDrag += new EventHandler(_toolstrip_EndDrag_);
            //   _toolstrip.MouseMove += new MouseEventHandler(_toolstrip_MouseMove);
            _parentform = ParentForm;
            text = Text;
        }

        bool _showing = true;

        /// <summary>
        /// ToolStripが表示されているかどうかを取得します．
        /// </summary>
        public bool Showing
        {
            get
            {
                return _showing;
            }
        }

        void _toolstrip_MouseMove(object sender, MouseEventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
            {
                parent = _toolstrip.Parent;
                parentrect = parent.RectangleToScreen(parent.ClientRectangle);
                Point ShowLocation = Cursor.Position;
                // フローティング開始
                if (!CheckCloss(parentrect, ShowLocation))
                {
                    _showing = true;
                    FloatingWindow form = new FloatingWindow(_toolstrip, _parentpanels, _parentform, text, this);
                    form.rectform.EdgeColor = EdgeColor;
                    form.rectform.BackColor = RangeColor;
                    _parentform.AddOwnedForm(form);
                    //form.Show();
                    //MessageBox.Show(ShowLocation.ToString());
                    /*this.Location = ShowLocation;
                    _toolstrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                    this.Size = new Size(_toolstrip.Width + GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2,
                        _toolstrip.Height /*+ 20 + GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2);
                    this.Controls.Add(_toolstrip);
                    _toolstrip.Location = new Point(0, 20);
                    //_toolstrip.Dock = DockStyle.Top;
                    this.Show();*/
                }
            }
        }

        string text;

        Control parent;
        Rectangle parentrect;

        /// <summary>
        /// FloatingToolStrip を非表示にします．
        /// </summary>
        public void HideToolStrip()
        {
            _toolstrip.Visible = false;
            if (form != null)
            {
                form.Hide();
            }
            _showing = false;
        }

        internal bool showingform = false;


        /// <summary>
        /// FloatingToolStrip を表示します．
        /// </summary>
        public void ShowToolStrip()
        {
            _toolstrip.Visible = true;
            //form = new FloatingWindow(_toolstrip, _parentpanels, _parentform, text, this);
            if (form != null && showingform)
            {
                form.Show();
            }
            _showing = true;
        }


        /// <summary>
        /// フローティング時に表示するテキストを取得，設定します．
        /// </summary>
        public string Text
        {
            set
            {
                text = value;
                if (form != null)
                {
                    form.Text = text;
                }
            }
            get
            {
                return text;
            }
        }

        FloatingWindow form;

        Color _rectcolor = Color.BlueViolet;
        Color _backcolor = Color.Thistle;


        /// <summary>
        /// ドッキング先の領域を示す矩形の縁の色を指定します．
        /// </summary>
        public Color EdgeColor
        {
            set
            {
                _rectcolor = value;
                if (form != null)
                {
                    if (form.rectform != null)
                    {
                        form.rectform.EdgeColor = value;
                    }
                }
            }
            get
            {
                if (form != null)
                {
                    if (form.rectform != null)
                    {
                        _rectcolor = form.rectform.EdgeColor;
                    }
                }
                return _rectcolor;
            }
        }


        /// <summary>
        /// ドッキング先の領域を示す矩形の色を指定します．
        /// </summary>
        public Color RangeColor
        {
            set
            {
                _backcolor = value;
                if (form != null)
                {
                    if (form.rectform != null)
                    {
                        form.rectform.BackColor = value;
                    }
                }
            }
            get
            {
                if (form != null)
                {
                    if (form.rectform != null)
                    {
                        _backcolor = form.rectform.BackColor;
                    }
                }
                return _backcolor;
            }
        }

        void _toolstrip_EndDrag_(object sender, EventArgs e)
        {
            //if (parent == _toolstrip.Parent)
            //{

            parent = _toolstrip.Parent;
            parentrect = parent.RectangleToScreen(parent.ClientRectangle);
            Point ShowLocation = Cursor.Position;
            // フローティング開始
            if (!CheckCloss(parentrect, ShowLocation))
            {
                _showing = true;
                ToolStripPanel tsp = (ToolStripPanel)_toolstrip.Parent;
                Point lp = _toolstrip.Location;
                ToolStripLayoutStyle tsls = _toolstrip.LayoutStyle;

                form = new FloatingWindow(_toolstrip, _parentpanels, _parentform, text, this);
                form.rectform.EdgeColor = EdgeColor;
                form.rectform.BackColor = RangeColor;
                form.LastParent = tsp;
                form.LastPoint = lp;
                form.LastStyle = tsls;
                _parentform.AddOwnedForm(form);
                //form.Show();
                //MessageBox.Show(ShowLocation.ToString());
                /*this.Location = ShowLocation;
                _toolstrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                this.Size = new Size(_toolstrip.Width + GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2,
                    _toolstrip.Height /*+ 20 + GetSystemMetrics(SystemMetric.SM_CXSIZEFRAME) * 2);
                this.Controls.Add(_toolstrip);
                _toolstrip.Location = new Point(0, 20);
                //_toolstrip.Dock = DockStyle.Top;
                this.Show();*/
            }
            //}
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
        internal Form _parentform;
    }
}
