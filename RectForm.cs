using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FloatingToolStripLibrary
{
    internal partial class RectForm : Form
    {
        public RectForm(Color RangeColor, Color EdgeColor)
        {
            InitializeComponent();
            //this.TopMost = true;
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint, true);
            this.BackColor = RangeColor;
            this.EdgeColor = EdgeColor;
        }

        public Color EdgeColor
        {
            get
            {
                return _rectcolor;
            }
            set
            {
                _rectcolor = value;
                Refresh();
            }
        }

        Color _rectcolor = Color.BlueViolet;

        private void RectangleForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(_rectcolor), new Rectangle(0, 0, this.Width - 1, this.Height - 1));
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        private void RectangleForm_Move(object sender, EventArgs e)
        {

        }
    }
}
