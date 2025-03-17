namespace ITS_Studio.AllForms
{
    partial class fmExternalPaint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // fmExternalPaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = false;
            this.ClientSize = new System.Drawing.Size(536, 384);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "fmExternalPaint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fmExternalPaint";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Load += new System.EventHandler(this.fmExternalPaint_Load);            
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fmExternalPaint_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

    }
}