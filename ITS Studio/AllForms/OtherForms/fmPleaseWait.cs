using System;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public partial class fmPleaseWait : Form
    {
        public fmPleaseWait()
        {
            this.TopMost = true;
            InitializeComponent();
            this.pictureBox1.Parent = this;
        }

        public fmPleaseWait(IntPtr ParentForm)
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Minimized;
            this.TopMost = false;
            this.pictureBox1.Parent = GetForm(ParentForm);
            this.Dock = DockStyle.Fill;
            this.pictureBox1.Visible = true;
        }

        static public Control GetForm(IntPtr handle)
        {
            return handle == IntPtr.Zero ?
                null : Control.FromHandle(handle) as Control;
        }

        private void fmPleaseWait_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.pictureBox1.Parent = this;
        }
    }
}