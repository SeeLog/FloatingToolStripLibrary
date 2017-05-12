using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace FloatingToolStripLibrary
{
    public partial class FloatingToolStripGradientPanel : ToolStripPanel
    {
        /// <summary>
        /// FloatingToolStripLibrary.FloatingToolStripGradientPanel クラスの新しいインスタンスを初期化します．
        /// </summary>
        public FloatingToolStripGradientPanel()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw,
                          true
                          );
            BackColor = Color.Lavender;
            BackColor2 = Color.CornflowerBlue;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }

        #region BackColor

        private Color _BackColor;

        /// <summary>
        /// コンポーネントの背景色のグラデーションの開始色です．
        /// </summary>
        [Category("表示")]
        [DefaultValue(typeof(Color), "Color.Lavender")]
        [Description("コンポーネントの背景色のグラデーションの開始色です．")]
        public new Color BackColor
        {
            get
            {
                if (this._BackColor != Color.Empty)
                {
                    return this._BackColor;
                }

                if (this.Parent != null)
                {
                    return this.Parent.BackColor;
                }

                return Control.DefaultBackColor;
            }
            set
            {
                this._BackColor = value;
                this.Refresh();
            }
        }

        public override void ResetBackColor()
        {
            this.BackColor = Color.Empty;
        }

        private Boolean ShouldSerializeBackColor()
        {
            return this._BackColor != Color.Empty;
        }

        #endregion

        #region BackColor2

        private Color _BackColor2;
        /// <summary>
        /// コンポーネントの背景色のグラデーションの終了色です．
        /// </summary>
        [Category("表示")]
        [DefaultValue(typeof(Color), "Color.CornflowerBlue")]
        [Description("コンポーネントの背景色のグラデーションの終了色です．")]
        public Color BackColor2
        {
            get
            {
                if (this._BackColor2 != Color.Empty)
                {
                    return this._BackColor2;
                }
                if (this.Parent != null)
                {
                    return this.Parent.BackColor;
                }

                return Control.DefaultBackColor;
            }
            set
            {
                this._BackColor2 = value;
                this.Refresh();
            }
        }

        public void ResetBackColor2()
        {
            this.BackColor2 = Color.Empty;
        }

        private Boolean ShouldSerializeBackColor2()
        {
            return this._BackColor2 != Color.Empty;
        }

        #endregion

        private LinearGradientMode _GradientMode;
        /// <summary>
        /// コンポーネントの背景色のグラデーションの方向です．
        /// </summary>
        [Category("表示")]
        [DefaultValue(typeof(LinearGradientMode), "Horizontal")]
        [Description("コンポーネントの背景色のグラデーションの方向です．")]
        public LinearGradientMode GradientMode
        {
            get { return this._GradientMode; }
            set
            {
                this._GradientMode = value;
                this.Invalidate();
            }
        }

        private void FloatingToolStripGradientPanel_Paint(object sender, PaintEventArgs e)
        {
            if ((this.BackColor == Color.Transparent) || (this.BackColor2 == Color.Transparent))
            {
                base.OnPaintBackground(e);
            }
            if (this.ClientRectangle.Width != 0 && this.ClientRectangle.Height != 0)
            {
                using (LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, this.BackColor, this.BackColor2, this.GradientMode))
                {
                    e.Graphics.FillRectangle(lgb, this.ClientRectangle);
                }
            }
        }
    }
}
