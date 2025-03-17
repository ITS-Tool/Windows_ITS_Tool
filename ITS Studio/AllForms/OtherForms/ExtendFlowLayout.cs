using System;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public partial class ExtendFlowLayout : Form
    {
        public delegate void delClick();
        
        public event delClick HexToIliClickEvent;
        public event delClick HexToBinClickEvent;
        public event delClick FrequencySpectrumClickEvent;
        public event delClick ChargeCurveClickEvent;
        public event delClick CModelClickEvent;
        public event delClick EncryptClickEvent;
        public event delClick GenProfileClickEvent;

        private bool _isClosebyBtn = true;

        public bool IsCloseByBtn
        {
            get { return _isClosebyBtn; }
            set { _isClosebyBtn = value; }    
        }

        public ExtendFlowLayout()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
        }
            
        private void m_btHexToIli_Click(object sender, EventArgs e)
        {
            if (HexToIliClickEvent != null)
                HexToIliClickEvent();
                
        }

        private void m_btFreSpectrum_Click(object sender, EventArgs e)
        {
            if (FrequencySpectrumClickEvent != null)
                FrequencySpectrumClickEvent();
        }

        private void m_btChargCurve_Click(object sender, EventArgs e)
        {
            if (ChargeCurveClickEvent != null)
                ChargeCurveClickEvent();
        }

        private void ExtendFlowLayout_Deactivate(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Visible = false;
                _isClosebyBtn = false;
            }     
        }

        private void m_btCModel_Click(object sender, EventArgs e)
        {
            if (CModelClickEvent != null)
                CModelClickEvent();
        }

        private void m_btHexToBin_Click(object sender, EventArgs e)
        {
            if (HexToBinClickEvent != null)
                HexToBinClickEvent();
        }

        private void m_btEncrypt_Click(object sender, EventArgs e)
        {
            if(EncryptClickEvent != null)
                EncryptClickEvent();
        }

        private void m_btGenProfile_Click(object sender, EventArgs e)
        {
            if (GenProfileClickEvent != null)
                GenProfileClickEvent();
        }
  
    }
}
