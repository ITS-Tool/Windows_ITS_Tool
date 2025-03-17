namespace ITS_Studio.AllForms.SensorTest
{
    partial class SensorTest_MPResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorTest_MPResult));
            this.richTB_MPResultMesg = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTB_MPResultMesg
            // 
            this.richTB_MPResultMesg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.richTB_MPResultMesg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTB_MPResultMesg.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTB_MPResultMesg.Location = new System.Drawing.Point(0, 0);
            this.richTB_MPResultMesg.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.richTB_MPResultMesg.Name = "richTB_MPResultMesg";
            this.richTB_MPResultMesg.ReadOnly = true;
            this.richTB_MPResultMesg.Size = new System.Drawing.Size(981, 875);
            this.richTB_MPResultMesg.TabIndex = 2;
            this.richTB_MPResultMesg.Text = "";
            // 
            // SensorTest_MPResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ClientSize = new System.Drawing.Size(981, 875);
            this.Controls.Add(this.richTB_MPResultMesg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SensorTest_MPResult";
            this.Text = "SensorTest_MPResult";
            this.Load += new System.EventHandler(this.SensorTest_MPResult_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTB_MPResultMesg;
    }
}