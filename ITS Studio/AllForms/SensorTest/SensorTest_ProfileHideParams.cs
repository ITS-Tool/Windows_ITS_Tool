using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Sunny.UI;
using ThirdPartyTools;

namespace ITS_Studio.AllForms.SensorTest
{
    public enum SectionNames : int
    {
        [Description("Default_FW_Fixed")]
        FW_Fixed = 0,
        [Description("Default_Power_Test")]
        Power_Test,
        [Description("Default_P2P_Test")]
        P2P_Test,
        [Description("Default_SRAM_Test")]
        SRAM_Test,
        [Description("Default_Current_Test")]
        Current_Test,
    }

    public partial class SensorTest_ProfileHideParams : Form
    {
        private string ProfilePath = "";
        private DefaultProfileValue _ProfileItems = new DefaultProfileValue();
        private INI_Tool My_INI;
        private string[] TestSections = EnumTool.EnumToList<SectionNames>().Select(x => x.ToString()).ToArray();
        private UIPage MyUIPage = new UIPage();
        public DialogResult ReturnDialogResult = DialogResult.Cancel;

        public SensorTest_ProfileHideParams(string _path)
        {
            InitializeComponent();

            ProfilePath = _path;
            My_INI = new INI_Tool(ProfilePath);
            ReturnDialogResult = DialogResult.Cancel;
            //Console.WriteLine("ProfilePath = {0}", ProfilePath);
        }

        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            uiCheckBoxGroup1.SelectAll();
            int i = 0;
            foreach(var item in uiCheckBoxGroup1.Items)
                uiCheckBoxGroup1_ValueChanged(sender, i++, item.ToString(), true);
        }

        private void btn_UnSelectAll_Click(object sender, EventArgs e)
        {
            uiCheckBoxGroup1.UnSelectAll();
            int i = 0;
            foreach(var item in uiCheckBoxGroup1.Items)
                uiCheckBoxGroup1_ValueChanged(sender, i++, item.ToString(), false);
        }

        private void btn_Conform_Click(object sender, EventArgs e)
        {
            var Allitems = _ProfileItems.GetEnableTestitems();
            foreach(var item in Allitems)
            {
                if(item.Item2 == false)
                {
                    My_INI.ClearSector(item.Item1.ToString());
                }
                else
                {
                    var arr = _ProfileItems.GetValuePair(item.Item1);
                    foreach(var value in arr)                    
                        My_INI.IniWriteValue(item.Item1.ToString(), value.Key, value.Value);
                }
            }
            //MyUIPage.ShowWarningDialog("注意", "請重新讀取Profile讓設定生效", UIStyle.Green, true);
            ReturnDialogResult = DialogResult.OK;
            this.Close();
        }

        private void uiCheckBoxGroup1_ValueChanged(object sender, int index, string text, bool isChecked)
        {
            _ProfileItems.SetTestitem_En(index, isChecked);
        }

        private void SensorTest_ProfileHideParams_Load(object sender, EventArgs e)
        {
            uiCheckBoxGroup1.Items.Clear();
            uiCheckBoxGroup1.Items.AddRange(TestSections);

            StringCollection ProfileSections = new StringCollection();
            My_INI.ReadAllSections(ref ProfileSections);

            List<int> ProfileInt = new List<int>();
            foreach(string item in uiCheckBoxGroup1.Items)
            {
                if(ProfileSections.IndexOf(item) == -1)
                    continue;
                int num = (int)(item.ToEnum<SectionNames>());
                ProfileInt.Add(num);
            }
            uiCheckBoxGroup1.SelectedIndexes = ProfileInt;
        }
    }

    internal class DefaultProfileValue
    {
        private Dictionary<string, string> Default_FW_Fixed = new Dictionary<string, string>();
        private Dictionary<string, string> Default_Power_Test = new Dictionary<string, string>();
        private Dictionary<string, string> Default_P2P_Test = new Dictionary<string, string>();
        private Dictionary<string, string> Default_SRAM_Test = new Dictionary<string, string>();
        private Dictionary<string, string> Default_Current_Test = new Dictionary<string, string>();

        public DefaultProfileValue()
        {
            Default_FW_Fixed.Add("Enable", bool.FalseString);
            Default_FW_Fixed.Add("Path", "");
            Default_FW_Fixed.Add("Master_CRC", "0");
            Default_FW_Fixed.Add("Slave_CRC", "0");
            Default_FW_Fixed.Add("FW_Ver", "0");

            Default_Power_Test.Add("Enable", bool.FalseString);
            Default_Power_Test.Add("VDD33_LVD_Level_Sel", "0x02");
            Default_Power_Test.Add("VDD33_LVD_Flag_BitMask", "0xFF");

            Default_P2P_Test.Add("Enable", bool.FalseString);
            Default_P2P_Test.Add("Frame_Count", "50");
            Default_P2P_Test.Add("#P2P_Delta_Threshold ", "Touch Level");
            Default_P2P_Test.Add("P2P_Delta_Threshold", "100");
            Default_P2P_Test.Add("P2P_Type", "0x84");
            Default_P2P_Test.Add("Hopping_Enable", "False");
            Default_P2P_Test.Add("Frequency", "100");

            Default_SRAM_Test.Add("Enable",bool.FalseString);
            Default_SRAM_Test.Add("Return_Value", "0x00000007");
            Default_SRAM_Test.Add("TestFinishChipReset", bool.FalseString);
            Default_SRAM_Test.Add("TestFinishPowerReset", bool.TrueString);
            Default_SRAM_Test.Add("Power_Off_DelayTime", "200");
            Default_SRAM_Test.Add("Power_On_DelayTime", "300");
            Default_SRAM_Test.Add("ICE_Mode_I2C_Address", "0x62");

            Default_Current_Test.Add("Enable", bool.FalseString);
            Default_Current_Test.Add("SamplingTime", "200");
            Default_Current_Test.Add("TestCounts", "5");
            Default_Current_Test.Add("Threshold", "290");


            foreach(var item in EnumTool.EnumToList<SectionNames>())
                ItemSectors.Add(new Tuple<SectionNames, bool, string>(item, false, item.Desc()));
        }

        private List<Tuple<SectionNames, bool, string>> ItemSectors = new List<Tuple<SectionNames, bool, string>>();

        public bool SetTestitem_En(int index, bool bChecked)
        {
            if(index == -1)
                return false;
            ItemSectors[index] = new Tuple<SectionNames, bool, string>(ItemSectors[index].Item1, bChecked, ItemSectors[index].Item3);
            return true;
        }

        public List<Tuple<SectionNames, bool, string>> GetEnableTestitems()
        {
            return ItemSectors;
        }
        public Dictionary<string, string> GetValuePair(SectionNames DicName)
        {
            switch(DicName)
            {
                case SectionNames.FW_Fixed:
                    return Default_FW_Fixed;
                case SectionNames.P2P_Test:
                    return Default_P2P_Test;
                case SectionNames.Power_Test:
                    return Default_Power_Test;
                case SectionNames.SRAM_Test:
                    return Default_SRAM_Test;
                case SectionNames.Current_Test:
                    return Default_Current_Test;
                default:
                    return null;
            }
        }
        //public List<KeyValuePair<string,string>> GetAllDe
    }
}