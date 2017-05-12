namespace FloatingToolStripLibrary
{
    partial class FloatingWindow
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FloatingWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 45);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FloatingWindow";
            this.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FloatingToolStrip_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FloatingToolStrip_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FloatingWindow_MouseClick);
            this.Shown += new System.EventHandler(this.FloatingWindow_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FloatingWindow_FormClosed);
            this.MouseLeave += new System.EventHandler(this.FloatingWindow_MouseLeave);
            this.Move += new System.EventHandler(this.FloatingToolStrip_Move);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FloatingWindow_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
