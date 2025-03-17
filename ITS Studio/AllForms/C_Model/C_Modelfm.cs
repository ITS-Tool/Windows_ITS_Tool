using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ThirdPartyTools;
using CommonExt;
using FuncMethodCmd;
using System.Runtime.InteropServices;
using Sunny.UI;
using System.Drawing.Drawing2D;

namespace ITS_Studio.AllForms
{
    public partial class C_Modelfm : Form
    {
        private enum C_MODEL_FLIP_OPTION : int
        {
            [Description("Revert CDC X")]
            REVERT_CDC_X = 0,
            [Description("Revert CDC Y")]
            REVERT_CDC_Y,
            [Description("Revert Paint X")]
            REVERT_PAINT_X,
            [Description("Revert Paint Y")]
            REVERT_PAINT_Y,
        }

        private bool revert_paint_x = false;
        private bool revert_paint_y = false;

        private enum C_MODEL_RECORD_METHOD : byte
        {
            [Description("Interrupt")]
            Finger_Interrupt = 0x1,
            [Description("Ctrl")]
            Figer_Ctrl = 0x2,
            [Description("Interrupt")]
            Pen_Interrupt = 0x4,
            [Description("Ctrl")]
            Pen_Ctrl = 0x8,
        }

        private enum C_MODEL_DATA_TYPE : uint
        {
            [Description("MC")]
            MC = 0x1,
            [Description("SC-X")]
            SC_X = 0x2,
            [Description("SC-Y")]
            SC_Y = 0x4,
            [Description("PEN-TIP")]
            PEN_TIP = 0x8,
            [Description("PEN-RING")]
            PEN_RING = 0x10,
        }

        private enum C_MODEL_FUNCTION : byte
        {
            [Description("Locate")]
            Locate = 0,
            [Description("DeLCM")]
            DeLCM = 1,
            [Description("Palm")]
            Palm = 2,
            [Description("Thumb")]
            Thumb = 3,
            [Description("Interpolation")]
            Interpolation = 4,
            [Description("Tracking")]
            Tracking = 5,
            [Description("Filter")]
            Filter = 6,
        }

        private enum C_MODEL_DATA_LEN : byte
        {
            [Description("256 bytes:USB:MC/SC/PEN")]
            _256 = 0,
            [Description("1024 bytes:USB/HID-I2C:MC/SC/PEN")]
            _1024 = 1,
            [Description("2048 bytes:USB:MC/SC")]
            _2048 = 2,
        }

        private enum C_MODEL_ROI_LIMIT : byte
        {
            [Description("1")]
            _1 = 1,
            [Description("2")]
            _2 = 2,
            [Description("3")]
            _3 = 3,
            [Description("4")]
            _4 = 4,
            [Description("5")]
            _5 = 5,
            [Description("6")]
            _6 = 6,
            [Description("7")]
            _7 = 7,
            [Description("8")]
            _8 = 8,
            [Description("9")]
            _9 = 9,
            [Description("10")]
            _10 = 10,
        }

        private enum C_MODEL_DATA_INFO : int
        {
            [Description("Report Rate:0")]
            Report_Rate = 0,
        }

        private Random rnd = new Random();

        private BackgroundWorker c_model_stop_bgworker;

        public struct GridData
        {
            public Steema.TeeChart.Styles.ColorGrid colorgrid;

            public int x_size, y_size;

            public int[] x_arr;
            public int[] y_arr;
        }
        private GridData MC_raw_GridData;
        private GridData SC_x_raw_GridData;
        private GridData SC_y_raw_GridData;
        private GridData Pen_x_raw_GridData;
        private GridData Pen_y_raw_GridData;

        public struct _finger
        {
            public int line_point_cnt;

            public bool need_handled;
            public double x, y;

            public Color color;
            public PointF prev_point;
            public PointF curr_point;

            public Pen p;
            public System.Windows.Forms.Panel point;

            public Steema.TeeChart.Styles.HorizLine line;
            public Steema.TeeChart.Styles.Points pt;
        }
        private _finger[] fingers = new _finger[10];

        public struct _pen
        {
            public int state;
            public int pressure;
            public int tilt_x, tilt_y;
            public double x, y;

            public int line_point_cnt;

            public Color color;
            public PointF prev_point;
            public PointF curr_point;

            public Pen p;
            public System.Windows.Forms.Panel point;

            public Steema.TeeChart.Styles.HorizLine line;
            public Steema.TeeChart.Styles.Points pt;
        }
        private _pen pen;

        private bool is_debuging = false;
        private byte data_type;

        private bool show_ui_colorgrid = false;

        /* ITS UI general parameter */
        private int dev_idx = 0;
        public int SelectedDev { set { dev_idx = value; } get { return dev_idx; } }
        protected DynamicTool MyTool = new DynamicTool();

        private delegate void function(Message msg);
        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();
        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if (mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](msg);
            base.WndProc(ref msg);
        }

        private UIPage MyUIPage = new UIPage();

        public C_Modelfm()
        {
            InitializeComponent();
            Flip_MenuItem_Update();

            dmsg_touch_dict_total = ParseIniDmsgInfo(ITS_Directory.PaintTool_INI, "DebugInfoName");
            dmsg_pen_dict_total = ParseIniDmsgInfo(ITS_Directory.PaintTool_INI, "PenDebugInfoName");
            dmsg_touchAlgo = ParseIniAlgoInfo(ITS_Directory.PaintTool_INI, "DataColumnName_V6");

            this.TopLevel = false;

            ColorGrid_Marks_init(ref this.MC_raw_ColorGrid);
            ColorGrid_Marks_init(ref this.SC_x_raw_ColorGrid);
            ColorGrid_Marks_init(ref this.SC_y_raw_ColorGrid);
            ColorGrid_Marks_init(ref this.Pen_x_raw_ColorGrid);
            ColorGrid_Marks_init(ref this.Pen_y_raw_ColorGrid);

            this.uiComboTreeView_data_type.TreeView.AfterCheck += new TreeViewEventHandler(this.uiComboTreeView_data_type_AfterCheck);
            this.uiComboTreeView_dmsg.TreeView.AfterCheck += new TreeViewEventHandler(this.uiComboTreeView_dmsg_AfterCheck);

            uiComboTreeView_data_type_Update();
            uiComboTreeView_dmsg_Update();
            uiComboBox_record_method_Update();
            uiComboBox_data_length_Update();
            uiComboBox_RoI_limit_Update();
            uiComboBox_function_Update();
            uiTextBox_RoI_size_Update();
            uiDataGridView_info_Update();

            data_type_Update();

            StaticVar.C_Model_FormHandle = this.Handle;

            is_debuging = false;

            mMessageReceiver.Add(enWM_MSG.WM_CModel_UpdateColorGrid, OnUpdateColorGrid);
            mMessageReceiver.Add(enWM_MSG.WM_CModel_UpdatePainting, OnUpdatePaint);
            mMessageReceiver.Add(enWM_MSG.WM_CModel_UpdateRecordTag, OnUpdateRecordTag);

            mMessageReceiver.Add(enWM_MSG.WM_CModel_ChangeSetting, OnChangeUISetting);
            mMessageReceiver.Add(enWM_MSG.WM_CModel_StartStopRecord, OnStartStopRecord);
        }

        private Dictionary<string, string> dmsg_touch_dict_total;
        private Dictionary<string, string> dmsg_pen_dict_total;
        private Dictionary<string, string> dmsg_touchAlgo;
        public Dictionary<string, string> dmsg_dict_selected = new Dictionary<string, string>();

        private Dictionary<enPenSNRX, List<float>> DicSNRXCalculate = new Dictionary<enPenSNRX, List<float>>();
        private Dictionary<enPenSNRY, List<float>> DicSNRYCalculate = new Dictionary<enPenSNRY, List<float>>();

        private Dictionary<string, string> ParseIniDmsgInfo(string ini_path, string section)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            INI_Tool MyINI = new INI_Tool(ini_path);

            List<string> keys = MyINI.IniReadAllKeys(section);
            List<string> values = MyINI.IniReadAllValues(section);
            if (keys.Count == 0)
                return null;

            UI_GBV.mDev[dev_idx].LocalVariable.stuReportIndex.iSNR_FrameCount = MyINI.IniReadInt("V6ReportIendex", "SNR_FrameCount", 100);
            UI_GBV.mDev[dev_idx].LocalVariable.stuReportIndex.iPen_TermCount = MyINI.IniReadInt("V6ReportIendex", "Pen_TermCount", 6);

            for (int i = 0; i < keys.Count(); i++)
            {
                List<string> items = MyINI.IniReadAllValues(keys[i]);

                if (items.GroupBy(j => j).Where(g => g.Count() == 8).Count() > 0)
                {
                    string item = items[0];

                    if (!dict.ContainsKey(item))
                    {
                        dict.Add(item, string.Format("B'{0},", values[i].ToString()));
                        continue;
                    }

                    dict[item] += string.Format("B'{0},", values[i]);
                }
                else
                {
                    foreach (string item in items)
                    {
                        if (dict.ContainsKey(item))
                            continue;

                        int start_bit = items.FindIndex(x => x == item);
                        int end_bit = items.FindLastIndex(x => x == item);

                        dict.Add(item, string.Format("B'{0}:{1}-{2},", values[i], start_bit, end_bit));
                    }
                }
            }

            return dict;
        }

        private Dictionary<string, string> ParseIniAlgoInfo(string ini_path, string section)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            INI_Tool MyINI = new INI_Tool(ini_path);

            List<string> keys = MyINI.IniReadAllKeys(section);
            List<string> values = MyINI.IniReadAllValues(section);
            if (keys.Count == 0)
                return null;

            for (int i = 0; i < keys.Count(); i++)
            {

                if (dict.ContainsKey(keys[i]) || !keys[i].Contains("_Bit"))
                    continue;
                dict.Add(keys[i], values[i]);
            }

            return dict;
        }

        private void Flip_MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            item.Checked = !item.Checked;

            C_MODEL_FLIP_OPTION val = EnumTool.GetValueFromDescription<C_MODEL_FLIP_OPTION>(item.Text);
            switch (val)
            {
                case C_MODEL_FLIP_OPTION.REVERT_CDC_X:
                    MC_raw.Axes.Bottom.Inverted = item.Checked;
                    SC_x_raw.Axes.Bottom.Inverted = item.Checked;
                    Pen_x_raw.Axes.Bottom.Inverted = item.Checked;
                    break;
                case C_MODEL_FLIP_OPTION.REVERT_CDC_Y:
                    MC_raw.Axes.Left.Inverted = item.Checked;
                    SC_y_raw.Axes.Left.Inverted = item.Checked;
                    Pen_y_raw.Axes.Left.Inverted = item.Checked;
                    break;
                case C_MODEL_FLIP_OPTION.REVERT_PAINT_X:
                    revert_paint_x = item.Checked;
                    break;
                case C_MODEL_FLIP_OPTION.REVERT_PAINT_Y:
                    revert_paint_y = item.Checked;
                    break;
            }
        }

        private void Flip_MenuItem_Update()
        {
            foreach (C_MODEL_FLIP_OPTION vtest in Enum.GetValues(typeof(C_MODEL_FLIP_OPTION)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = item.Text = vtest.Desc();
                item.Click += Flip_MenuItem_Click;
                Flip_MenuItem.DropDownItems.Add(item);
            }
        }

        private void uiDataGridView_info_Update()
        {
            uiDataGridView_info.ClearRows();

            foreach (C_MODEL_DATA_INFO item in Enum.GetValues(typeof(C_MODEL_DATA_INFO)))
            {
                string[] desc = item.DescArr(':');
                string name = desc[0];
                string val = desc[1];

                uiDataGridView_info.AddRow(new object[] { name, val });
            }

            dmsg_dict_selected.Clear();
            foreach (C_MODEL_DMSG_TYPE item in Enum.GetValues(typeof(C_MODEL_DMSG_TYPE)))
            {
                if ((CommonFlow.C_Model.setting.dmsg & (int)item) == 0)
                    continue;

                string desc = item.Desc();
                string idx = (desc.Contains(':')) ? desc.Split(':').Last() : desc;

                /* Algo */
                if (!idx.Contains("B'"))
                {
                    if (item == C_MODEL_DMSG_TYPE.TOUCH_ALGO)
                    {
                        dmsg_dict_selected.Add(idx, "");
                        foreach (var itemAlgo in dmsg_touchAlgo)
                        {                         
                            uiDataGridView_info.AddRow(new object[] { itemAlgo.Value, "" });
                        }
                    }
                    else //Pen Algo
                    {
                        dmsg_dict_selected.Add(idx, "");
                        uiDataGridView_info.AddRow(new object[] { idx, "" });
                    }
                    continue;
                }

                object dict = (item < C_MODEL_DMSG_TYPE.PEN_ALGO) ?
                    dmsg_touch_dict_total : dmsg_pen_dict_total;

                foreach (var v in dict as Dictionary<string, string>)
                {
                    if (!v.Value.Contains(idx))
                        continue;

                    if (dmsg_dict_selected.ContainsKey(v.Key))
                        break;


                    if (!v.Key.Contains("SNR_X") && !v.Key.Contains("SNR_Y"))
                    {
                        dmsg_dict_selected.Add(v.Key, v.Value);
                        uiDataGridView_info.AddRow(new object[] { v.Key, "" });

                    }
                    else
                    {
                        for (int i = 0; i < UI_GBV.mDev[dev_idx].LocalVariable.stuReportIndex.iPen_TermCount; i++)
                        {
                            if (!dmsg_dict_selected.ContainsKey(v.Key + "_" + i.ToString()))
                            {
                                dmsg_dict_selected.Add(v.Key + "_" + i.ToString(), v.Value);
                                uiDataGridView_info.AddRow(new object[] { v.Key + "_" + i.ToString(), "" });
                            }

                        }
                    }
                }
            }
        }

        private void uiComboTreeView_data_type_Update()
        {
            foreach (C_MODEL_DATA_TYPE item in Enum.GetValues(typeof(C_MODEL_DATA_TYPE)))
            {
                string desc = item.Desc();

                if (!uiComboTreeView_data_type.Nodes.ContainsKey(desc))
                    uiComboTreeView_data_type.Nodes.Add(desc, desc);
            }
        }

        private void uiComboTreeView_dmsg_Update()
        {
            uiComboTreeView_dmsg.Nodes.Clear();

            bool is_pen = is_Pen_data_type(data_type);

            uiComboTreeView_dmsg.Text = "0x0";

            foreach (C_MODEL_DMSG_TYPE item in Enum.GetValues(typeof(C_MODEL_DMSG_TYPE)))
            {
                string desc = item.Desc();

                if (!desc.Contains(":"))
                {
                    uiComboTreeView_dmsg.Nodes.Add(new TreeNode(desc));
                    continue;
                }

                string key = desc.Split(":")[0];
                string val = desc.Split(":")[1];

                if ((key.Equals("TOUCH") && is_pen) ||
                    (key.Equals("PEN") && !is_pen))
                    continue;

                TreeNode parent, child = new TreeNode(val);
                if (!uiComboTreeView_dmsg.Nodes.ContainsKey(key))
                {
                    parent = new TreeNode(key);
                    parent.Name = key;
                    uiComboTreeView_dmsg.Nodes.Add(parent);
                }
                else
                {
                    parent = uiComboTreeView_dmsg.Nodes.Find(key, false)[0];
                }

                parent.Nodes.Add(child);
            }
        }

        private void uiComboBox_function_Update()
        {
            foreach (C_MODEL_FUNCTION item in Enum.GetValues(typeof(C_MODEL_FUNCTION)))
            {
                string[] str = item.DescArr(':');
                string desc = str[0];

                if (uiComboBox_function.Items.Contains(desc))
                    continue;

                uiComboBox_function.Items.Add(desc);
                uiComboBox_function.SelectedIndex = 0;
            }

            uiComboBox_function.Enabled = false;
        }

        private byte get_output_function()
        {
            if (uiComboBox_function.SelectedItem == null)
                return 0;

            return (byte)EnumTool.GetValueFromDescription<C_MODEL_FUNCTION>(uiComboBox_function.SelectedItem.ToString());
        }

        private void uiComboBox_data_length_Update()
        {
            uiComboBox_data_length.Items.Clear();

            foreach (C_MODEL_DATA_LEN item in Enum.GetValues(typeof(C_MODEL_DATA_LEN)))
            {
                string[] str = item.DescArr(':');
                string desc = str[0];

                if (uiComboBox_data_length.Items.Contains(desc))
                    continue;

                uiComboBox_data_length.Items.Add(desc);
                uiComboBox_data_length.SelectedIndex = 0;
            }
        }

        private void uiComboBox_record_method_Update()
        {
            uiComboBox_record_method.Items.Clear();

            foreach (C_MODEL_RECORD_METHOD item in Enum.GetValues(typeof(C_MODEL_RECORD_METHOD)))
            {
                string desc = item.Desc();

                if (uiComboBox_record_method.Items.Contains(desc))
                    continue;

                uiComboBox_record_method.Items.Add(desc);
                uiComboBox_record_method.SelectedIndex = 0;
            }
        }

        private void uiComboBox_RoI_limit_Update()
        {
            bool pen_checked = is_Pen_data_type(data_type);
            bool sc_checked = ((data_type & ((byte)C_MODEL_DATA_TYPE.SC_X | (byte)C_MODEL_DATA_TYPE.SC_Y)) != 0);
            bool mc_checked = ((data_type & (byte)C_MODEL_DATA_TYPE.MC) != 0);

            uiComboBox_MC_RoI_limit.Items.Clear();
            uiComboBox_SC_RoI_limit.Items.Clear();

            /* Set RoI limit of Pen to 1 forcely */
            if (pen_checked)
            {
                uiTableLayoutPanel_SC_fingers.Visible = false;
                uiTableLayoutPanel_MC_fingers.Visible = false;
                uiLine_fingers.Visible = false;
                uiPanel_fingers.Visible = false;

                uiComboBox_MC_RoI_limit.Items.Add(C_MODEL_ROI_LIMIT._1.Desc());
                uiComboBox_MC_RoI_limit.SelectedIndex = 0;
                return;
            }

            uiTableLayoutPanel_SC_fingers.Visible = sc_checked;
            uiTableLayoutPanel_MC_fingers.Visible = mc_checked;
            uiLine_fingers.Visible = (mc_checked || sc_checked);
            uiPanel_fingers.Visible = (mc_checked || sc_checked);

            if (mc_checked)
            {
                foreach (C_MODEL_ROI_LIMIT item in Enum.GetValues(typeof(C_MODEL_ROI_LIMIT)))
                {
                    string desc = item.Desc();

                    if (!uiComboBox_MC_RoI_limit.Items.Contains(desc))
                    {
                        uiComboBox_MC_RoI_limit.Items.Add(desc);
                        uiComboBox_MC_RoI_limit.SelectedIndex = 0;
                    }
                }
            }

            if (sc_checked)
            {
                foreach (C_MODEL_ROI_LIMIT item in Enum.GetValues(typeof(C_MODEL_ROI_LIMIT)))
                {
                    string desc = item.Desc();

                    if (!uiComboBox_SC_RoI_limit.Items.Contains(desc))
                    {
                        uiComboBox_SC_RoI_limit.Items.Add(desc);
                        uiComboBox_SC_RoI_limit.SelectedIndex = 0;
                    }
                }
            }
        }

        public void Reset_GridData(int X, int Y, ref GridData grid,
                                   ref Steema.TeeChart.Styles.ColorGrid ColorGrid)
        {
            grid.x_size = X;
            grid.y_size = Y;
            grid.colorgrid = ColorGrid;

            Axis_init(X, Y, out grid.x_arr, out grid.y_arr);

            Int32[] data = new Int32[X * Y];
            Array.Clear(data, 0, data.Length);
            Update_ColorGridData(ref data, ref grid);
        }

        public void Reset_GridColor(ref Steema.TeeChart.Styles.ColorGrid ColorGrid, Int32[] data)
        {
            if (data.Length == 0)
                return;
            int max = data.Max(), min = data.Min();

            ColorGrid.PaletteMin = min;

            if (max - min < 5)
            {
                ColorGrid.PaletteStep = 1;
                ColorGrid.PaletteSteps = 5;
            }
            else
            {
                if (max - min > 256)
                {
                    ColorGrid.PaletteStep = (max - min) / 256;
                    ColorGrid.PaletteSteps = 256;
                }
                else
                {
                    ColorGrid.PaletteStep = 1;
                    ColorGrid.PaletteSteps = max - min;
                }
            }
            ColorGrid.UsePaletteMin = true;
        }

        public void Update_ColorGridData(ref Int32[] data, ref GridData grid)
        {
            Reset_GridColor(ref grid.colorgrid, data);
            grid.colorgrid.Add(grid.x_arr, data, grid.y_arr);
        }

        /* x/y should be in range of FW channel count, and start from 1 */
        private void DrawLine_AddRectangle(int x_ch_start, int y_ch_start, int x_ch_end, int y_ch_end)
        {
            Steema.TeeChart.Tools.DrawLineItem rectangle = new Steema.TeeChart.Tools.DrawLineItem(DrawLine);

            rectangle.Style = Steema.TeeChart.Tools.DrawLineStyle.Rectangle;
            rectangle.Pen.Color = System.Drawing.Color.Red;
            rectangle.Pen.Width = 5;
            rectangle.StartPos = new Steema.TeeChart.Drawing.PointDouble(x_ch_start - 0.5f, y_ch_start - 0.5f);
            rectangle.EndPos = new Steema.TeeChart.Drawing.PointDouble(x_ch_end + 0.5f, y_ch_end + 0.5f);

            this.DrawLine.Lines.Add(rectangle);
        }

        private void Draw_Fingers(ref _finger[] fingers, double x_res, double y_res)
        {
            for (int i = 0; i < 10; i++)
            {
                //fingers[i].pt.Clear();

                /* No touch report data */
                if (!fingers[i].need_handled)
                {
                    fingers[i].line_point_cnt = 0;
                    continue;
                }
                fingers[i].need_handled = false;

                float x = (float)((fingers[i].x / x_res) * MC_raw.Width);
                float y = (float)((fingers[i].y / y_res) * MC_raw.Height);
                x = (revert_paint_x) ? x : MC_raw.Width - x;
                y = (revert_paint_y) ? y : MC_raw.Height - y;

                fingers[i].curr_point = new PointF(x, y);

                //x = ((double)fingers[i].x / x_res) * x_size + 0.5f;
                //y = ((double)fingers[i].y / y_res) * y_size + 0.5f;
                //fingers[i].pt.Add(x, y);

                if (fingers[i].line_point_cnt != 0)
                {
                    Graphics g;
                    using (g = MC_raw.CreateGraphics())
                        g.DrawLine(fingers[i].p, fingers[i].prev_point, fingers[i].curr_point);

                    //fingers[i].line.Add(x, y, System.Drawing.Color.Transparent);
                }
                else
                {
                    //fingers[i].line.Add(x, y, fingers[i].pt.Color);
                }

                fingers[i].prev_point = fingers[i].curr_point;

                fingers[i].line_point_cnt++;
            }
        }

        private void Draw_Pen(ref _pen pen, double x_res, double y_res)
        {
            //pen.pt.Clear();

            if (pen.state == 0)
            {
                pen.line_point_cnt = 0;
                return;
            }

            float x = (float)((pen.x / x_res) * MC_raw.Width);
            float y = (float)((pen.y / y_res) * MC_raw.Height);
            y = MC_raw.Height - y;
            pen.curr_point = new PointF(x, y);

            //x = ((double)pen.x / x_res) * x_size + 0.5f;
            //y = ((double)pen.y / y_res) * y_size + 0.5f;
            //pen.pt.Add(x, y);

            float width = 2 + ((float)pen.pressure / (float)4096) * 10;

            if (pen.line_point_cnt != 0)
            {
                pen.p.Color = pen.color;
                pen.p.Width = width;

                //pen.pt.Pointer.Brush.Visible = true;
                //pen.pt.Pointer.Pen.Visible = false;
                //pen.pt.Pointer.VertSize = 5;
                //pen.pt.Pointer.HorizSize = 5;

                if (pen.state == 0x10)
                {
                    pen.p.Color = Color.Red;

                    //pen.line.Color = Color.Red;
                    //pen.pt.Pointer.Brush.Visible = false;
                    //pen.pt.Pointer.Pen.Visible = true;
                    //pen.pt.Pointer.VertSize = 20;
                    //pen.pt.Pointer.HorizSize = 20;
                    //line_color = System.Drawing.Color.Transparent;
                }
                else if ((pen.state & 0x2) == 0x2)
                {
                    pen.p.Color = Color.Yellow;
                }
                else if ((pen.state & 0x4) == 0x4)
                {
                    pen.p.Color = Color.Blue;
                }

                Graphics g;
                using (g = MC_raw.CreateGraphics())
                    g.DrawLine(pen.p, pen.prev_point, pen.curr_point);

                //pen.line.Add(x, y, pen.line.Color);
            }
            else
            {
                //pen.line.Add(x, y, System.Drawing.Color.Transparent);
            }

            pen.prev_point = pen.curr_point;

            pen.line_point_cnt++;
        }

        private void Axis_init(int X, int Y, out int[] X_arr, out int[] Y_arr)
        {
            X_arr = new int[X * Y];
            Y_arr = new int[X * Y];

            for (int y = 0, i = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                {
                    X_arr[i] = x + 1;
                    Y_arr[i++] = y + 1;
                }
            }
        }

        private void Chart_init(int X, int Y)
        {
            for (int i = 0; i < fingers.Length; i++)
            {
                fingers[i].need_handled = false;
                fingers[i].line_point_cnt = 0;
                fingers[i].color = System.Drawing.Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));

                fingers[i].p = new Pen(fingers[i].color, 2);
                fingers[i].p.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
                fingers[i].p.LineJoin = LineJoin.Round;
            }
            pen.color = System.Drawing.Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
            pen.p = new Pen(pen.color, 2);
            pen.p.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
            pen.p.LineJoin = LineJoin.Round;

            Reset_GridData(X, Y, ref MC_raw_GridData, ref MC_raw_ColorGrid);
            Reset_GridData(X, 1, ref SC_x_raw_GridData, ref SC_x_raw_ColorGrid);
            Reset_GridData(1, Y, ref SC_y_raw_GridData, ref SC_y_raw_ColorGrid);
            Reset_GridData(X, 8, ref Pen_x_raw_GridData, ref Pen_x_raw_ColorGrid);
            Reset_GridData(8, Y, ref Pen_y_raw_GridData, ref Pen_y_raw_ColorGrid);

            if (show_ui_colorgrid && !is_Pen_data_type(data_type))
                MC_raw_ColorGrid.Visible = true;
            else
                MC_raw_ColorGrid.Visible = false;
        }

        private short get_le16(ref byte[] buf, int idx)
        {
            return (short)((short)(buf[idx + 1] << 8) + (short)buf[idx]);
        }

        private void UpdateDmsg(ref byte[] buf)
        {
            int row_start = Enum.GetNames(typeof(C_MODEL_DATA_INFO)).Length;

            for (int i = 0, idx = 0; i < dmsg_dict_selected.Count; i++)
            {
                string key = dmsg_dict_selected.ElementAt(i).Key;
                string format = dmsg_dict_selected.ElementAt(i).Value;
                int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();
                uint val = 0;
              

                if (format.Contains(':'))
                {
                    format = format.Replace(",", "");
                    int start_bit = int.Parse(format.Split(':')[1].Split('-')[0]);
                    int end_bit = int.Parse(format.Split(':')[1].Split('-')[1]);
                    uint mask = (uint)0xFF >> (8 - (end_bit - start_bit + 1));

                    val = buf[idx];
                    val = val >> start_bit;
                    val = val & mask;

                    idx = idx + ((end_bit == 7) ? 1 : 0);
                }
                else
                {
                    val = buf[idx++];
                    for (int j = 1; j < cnt; j++)
                        val |= (uint)(buf[idx++] << (j * 8));
                    if (format.IsNullOrEmpty())
                    {
                        int iBitCnts = 0;
                        int iIndex = 0;
                        foreach (var item in EnumTool.EnumToList<PaintTool.enAlgoNameRowIndex>())
                        {
                            if (item == PaintTool.enAlgoNameRowIndex._Report_Rate)
                                continue;
                            if (dmsg_touchAlgo.ElementAt(iBitCnts).Value != "NA")
                            {
                                int iValue = MyTool.GetIntegerSomeBit((int)val, iBitCnts);
                                iIndex = row_start + i + iBitCnts;
                                Set_DataGridView_Val(iIndex, string.Format("{0}", (iValue == 1) ? "Active" : "Inactive"));
                            }
                            iBitCnts++;
                        }
                        continue;
                    }
                   
                }
                if (key.Equals("SNR"))
                {
                    UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts++;

                    UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Add((float)val);

                    if (UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts >= UI_GBV.mDev[dev_idx].LocalVariable.stuReportIndex.iSNR_FrameCount)
                    {
                        //ProgressBar_SNR.Value = ProgressBar_SNR.Maximum;
                        var tmp = UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._PaintTool.CalculateSNR(UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value);
                        Set_DataGridView_Val(row_start + i + dmsg_touchAlgo.Count - 1, string.Format("{0}", tmp));
                        UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Clear();
                        UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts = 0;
                        continue;
                    }
                    else
                    {
                        continue;
                    }

                }
                Set_DataGridView_Val(row_start + i + dmsg_touchAlgo.Count - 1, string.Format("{0}", val));
            }
        }

        private void UpdatePenDmsg(ref byte[] buf)
        {
            int row_start = Enum.GetNames(typeof(C_MODEL_DATA_INFO)).Length;
            int iIndex = 0;

            if (dmsg_dict_selected.ContainsKey("Algo_Pen_Stage"))
                iIndex = buf[buf.Length - 1];

            for (int i = 0, idx = 0; i < dmsg_dict_selected.Count; i++)
            {
                string key = dmsg_dict_selected.ElementAt(i).Key;
                string format = dmsg_dict_selected.ElementAt(i).Value;
                uint val = 0;

                if (key.Contains("Pen_SNR_X"))
                {
                    enPenSNRX PenSNRXIndex = (enPenSNRX)Enum.Parse(typeof(enPenSNRX), key);
                    if ((int)PenSNRXIndex != iIndex)
                        continue;

                    UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex]++;
                    if (!DicSNRXCalculate.ContainsKey(PenSNRXIndex))
                        DicSNRXCalculate.Add(PenSNRXIndex, new List<float>());

                    int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();


                    val = buf[idx++];
                    for (int j = 1; j < cnt; j++)
                        val |= (uint)(buf[idx++] << (j * 8));

                    DicSNRXCalculate[PenSNRXIndex].Add(val);

                    if (UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex] >= UI_GBV.mDev[dev_idx].LocalVariable.stuReportIndex.iSNR_FrameCount)
                    {
                        var tmp = UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._PaintTool.CalculateSNR(DicSNRXCalculate[PenSNRXIndex]);
                        Set_DataGridView_Val(row_start + i, string.Format("{0}", tmp));
                        DicSNRXCalculate[PenSNRXIndex].Clear();
                        UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex] = 0;
                    }
                    else
                    {
                        continue;
                    }

                }
                else if (key.Contains("Pen_SNR_Y"))
                {
                    enPenSNRY PenSNRYIndex = (enPenSNRY)Enum.Parse(typeof(enPenSNRY), key);
                    if ((int)PenSNRYIndex != iIndex)
                        continue;

                    UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex]++;
                    if (!DicSNRYCalculate.ContainsKey(PenSNRYIndex))
                        DicSNRYCalculate.Add(PenSNRYIndex, new List<float>());

                    int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();

                    val = buf[idx++];
                    for (int j = 1; j < cnt; j++)
                        val |= (uint)(buf[idx++] << (j * 8));

                    DicSNRYCalculate[PenSNRYIndex].Add(val);

                    if (UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex] >= UI_GBV.mDev[dev_idx].LocalVariable.stuReportIndex.iSNR_FrameCount)
                    {
                        var tmp = UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._PaintTool.CalculateSNR(DicSNRYCalculate[PenSNRYIndex]);
                        Set_DataGridView_Val(row_start + i, string.Format("{0}", tmp));
                        DicSNRYCalculate[PenSNRYIndex].Clear();
                        UI_GBV.mDev[dev_idx].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex] = 0;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();

                    val = buf[idx++];
                    for (int j = 1; j < cnt; j++)
                        val |= (uint)(buf[idx++] << (j * 8));

                    Set_DataGridView_Val(row_start + i, string.Format("{0}", val));
                }

            }
        }


        private void _OnUpdateColorGrid(ref MultiCModel_VAR.c_model_data data)
        {
            if (data == null)
                return;

            if (data.dmsg_touch.need_update)
                UpdateDmsg(ref data.dmsg_touch.buf);
            else if (data.dmsg_pen.need_update)
                UpdatePenDmsg(ref data.dmsg_pen.buf);

            if (!show_ui_colorgrid)
                return;

            if (data.pen_x.need_update)
                Update_ColorGridData(ref data.pen_x.data, ref Pen_x_raw_GridData);

            if (data.pen_y.need_update)
                Update_ColorGridData(ref data.pen_y.data, ref Pen_y_raw_GridData);

            if (data.mc.need_update)
                Update_ColorGridData(ref data.mc.data, ref MC_raw_GridData);

            if (data.sc_x.need_update)
                Update_ColorGridData(ref data.sc_x.data, ref SC_x_raw_GridData);

            if (data.sc_y.need_update)
                Update_ColorGridData(ref data.sc_y.data, ref SC_y_raw_GridData);

            Application.DoEvents();
        }

        private void _OnUpdatePainting(MultiCModel_VAR.c_model_data data)
        {
            int idx;

            if (data == null)
                return;

            if (data.touch.need_update)
            {
                for (int cnt = 0; cnt < data.touch.buf[61]; cnt++)
                {
                    idx = 5 * cnt + 1;
                    if (idx >= data.touch.buf.Length)
                        break;

                    if ((data.touch.buf[idx] & 0x40) == 0)
                        continue;
                    int id = data.touch.buf[idx] & 0x3F;
                    if (id >= fingers.Length)
                        break;

                    fingers[id].need_handled = true;
                    fingers[id].x = get_le16(ref data.touch.buf, idx + 1);
                    fingers[id].y = get_le16(ref data.touch.buf, idx + 3);
                }

                Draw_Fingers(ref fingers,
                         CommonFlow.info[dev_idx].tp.x_resolution,
                         CommonFlow.info[dev_idx].tp.y_resolution);
            }

            if (data.pen.need_update)
            {
                idx = 1;
                pen.state = data.pen.buf[idx];
                pen.x = get_le16(ref data.pen.buf, idx + 1);
                pen.y = get_le16(ref data.pen.buf, idx + 3);
                pen.pressure = get_le16(ref data.pen.buf, idx + 5);
                pen.tilt_x = get_le16(ref data.pen.buf, idx + 7);
                pen.tilt_y = get_le16(ref data.pen.buf, idx + 9);

                Draw_Pen(ref pen,
                         CommonFlow.info[dev_idx].tp.x_resolution,
                         CommonFlow.info[dev_idx].tp.y_resolution);
            }
        }

        private void OnUpdateColorGrid(Message msg)
        {
            this.BeginInvoke(new Action(() =>
            {
                _OnUpdateColorGrid(ref UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.grid_data);
                UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.grid_data = null;
            }));
        }

        private void OnUpdatePaint(Message msg)
        {
            _OnUpdatePainting(UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.report_data);
            UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.report_data = null;
        }

        private void OnUpdateRecordTag(Message msg)
        {
            this.Invoke(new Action(() =>
            {
                uiLabel_record.Text = string.Format("CDC: {0}, Paint: {1}",
                    Marshal.PtrToStringAuto(msg.WParam), Marshal.PtrToStringAuto(msg.LParam));
            }));
        }

        private void OnChangeUISetting(Message msg)
        {
            int iCmodelTypeSelect = (int)(msg.WParam);
            uiComboBox_record_method.SelectedIndex = iCmodelTypeSelect;
            //uiComboTreeView_data_type.Nodes[3].Checked = true;
            //uiComboTreeView_data_type.Nodes[4].Checked = true;
            uiCheckBox_trigger_type.Checked = false;

            uiComboTreeView_data_type.Text = "";        

            string[] sSettingVar = Marshal.PtrToStringAuto(msg.LParam).Split(';');
            uiTextBox_RoI_Tip.Text = sSettingVar[0];
            uiTextBox_RoI_Ring.Text = sSettingVar[1];         
            uiCheckBox_trigger_type.Checked = Convert.ToBoolean(sSettingVar[2]);
            uiComboBox_data_length.SelectedIndex = Convert.ToInt32(sSettingVar[3]);

            int index = 0;
            int iSettingValue = Convert.ToInt32(sSettingVar[4]);

            uiTextBox_RoI_MC_X.Text = sSettingVar[5];
            uiTextBox_RoI_MC_Y.Text = sSettingVar[6];
            uiTextBox_RoI_SC_X.Text = sSettingVar[7];
            uiTextBox_RoI_SC_Y.Text = sSettingVar[8];

            foreach (C_MODEL_DATA_TYPE item in Enum.GetValues(typeof(C_MODEL_DATA_TYPE)))
            {
                if ((iSettingValue & (int)item) == 0)
                {
                    index++;
                    continue;
                }
                uiComboTreeView_data_type.Nodes[index++].Checked = true;
            }

            foreach (TreeNode node in uiComboTreeView_data_type.Nodes)
            {
                if (!node.Checked)
                    continue;

                uiComboTreeView_data_type.Text += string.Format("{0};", node.Text);
            }

            Application.DoEvents();
        }

        private void OnStartStopRecord(Message msg)
        {
            int iStatus = (int)(msg.WParam);
            if (iStatus == 0)
            {
                uiButton_start_Click(this,null);
                Record_MenuItem_Click(this, null);
            }
            else
            {
                uiButton_start_Click(this, null);              
            }
        }

        private void ShowMarks_MenuItem_Click(object sender, EventArgs e)
        {
            if (ShowMarks_MenuItem.Checked)
            {
                ShowMarks_MenuItem.Checked = false;
                MC_raw_ColorGrid.Marks.Visible = false;
                SC_x_raw_ColorGrid.Marks.Visible = false;
                SC_y_raw_ColorGrid.Marks.Visible = false;
                Pen_x_raw_ColorGrid.Marks.Visible = false;
                Pen_y_raw_ColorGrid.Marks.Visible = false;
            }
            else
            {
                ShowMarks_MenuItem.Checked = true;
                MC_raw_ColorGrid.Marks.Visible = true;
                SC_x_raw_ColorGrid.Marks.Visible = true;
                SC_y_raw_ColorGrid.Marks.Visible = true;
                Pen_x_raw_ColorGrid.Marks.Visible = true;
                Pen_y_raw_ColorGrid.Marks.Visible = true;
            }
        }

        private void Record_MenuItem_Click(object sender, EventArgs e)
        {
            Record_CDC_and_Painting();

            if (UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.is_recording)
                Record_MenuItem.Text = "Stop Record";
            else
                Record_MenuItem.Text = "Start Record";
        }

        private void ColorGrid_Marks_init(ref Steema.TeeChart.Styles.ColorGrid _ColorGrid)
        {
            _ColorGrid.Marks.Arrow.Visible = false;
            _ColorGrid.Marks.ArrowLength = 0;
            _ColorGrid.Marks.Bevel.StringColorOne = "FFFFFFFF";
            _ColorGrid.Marks.Bevel.StringColorTwo = "FF808080";
            _ColorGrid.Marks.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
            _ColorGrid.Marks.Brush.Visible = false;
            _ColorGrid.Marks.Pen.Visible = false;
            _ColorGrid.Marks.Shadow.Visible = false;
            _ColorGrid.Marks.Symbol.Bevel.StringColorOne = "FFFFFFFF";
            _ColorGrid.Marks.Symbol.Bevel.StringColorTwo = "FF808080";
            _ColorGrid.Marks.Symbol.Pen.Visible = false;
            _ColorGrid.Marks.Symbol.Shadow.Visible = false;
            _ColorGrid.Marks.Transparent = true;
            _ColorGrid.Marks.Visible = false;
        }

        public void InitialDebugMode()
        {
            UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.bRunningStatus = true;

            UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
            UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi.m_Flow.m_HID_API.HID_CreateHandle(StaticVar.lsMain_USBDevInfo[dev_idx].strDevPath);

            CommonFlow.C_Model.c_model_cb.update_grid = UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._C_ModelTool_.update_grid;
            CommonFlow.C_Model.c_model_cb.update_report = UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._C_ModelTool_.update_report;
            CommonFlow.C_Model.c_model_cb.update_report_rate = update_report_rate;
            CommonFlow.C_Model.c_model = CommonFlow.C_Model.c_model_init(
                UI_GBV.mDev[dev_idx].LocalVariable.tpdev.dev, ref CommonFlow.C_Model.c_model_cb, IntPtr.Zero);

            setting_Update();
        }

        public void StopDebugMode()
        {
            Stop_DebugMode();

            if (CommonFlow.C_Model.c_model != IntPtr.Zero)
            {
                CommonFlow.C_Model.c_model_exit(CommonFlow.C_Model.c_model);
                CommonFlow.C_Model.c_model = IntPtr.Zero;
            }

            UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

            UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.bRunningStatus = false;
        }

        public void Clear_Painting()
        {
            MC_raw.Refresh();
        }

        public void Record_CDC_and_Painting()
        {
            if (!is_debuging)
                return;

            if (!UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.is_recording)
            {
                if (show_ui_colorgrid)
                {
                    MessageBox.Show(string.Format("[Warning] Please disable showing CDC ColorGrid for recording data with better performance."), "",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                MultiCModel_VAR.C_MODEL_CDC_RECORD_TYPE type = MultiCModel_VAR.C_MODEL_CDC_RECORD_TYPE.MC_SC;

                if (is_Pen_data_type(data_type))
                    type = MultiCModel_VAR.C_MODEL_CDC_RECORD_TYPE.PEN;
                else if (data_type == (byte)C_MODEL_DATA_TYPE.MC)
                    type = MultiCModel_VAR.C_MODEL_CDC_RECORD_TYPE.MC;

                UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._C_ModelTool_.Start_Record_Worker(
                    Application.ProductVersion, type);

                Record_MenuItem.Text = "Stop Record";
                UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.is_recording = true;

                uiPanel_record.Visible = true;

                return;
            }

            UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.is_recording = false;

            UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._C_ModelTool_.Stop_Record_Worker();

            Record_MenuItem.Text = "Start Record";
            uiPanel_record.Visible = false;
        }

        private void setting_Update()
        {
            if (UI_GBV.mDev[dev_idx].LocalVariable.tpdev == null ||
                CommonFlow.C_Model.c_model == IntPtr.Zero)
                return;

            CommonFlow.C_Model.setting.ctrl_len = get_data_length();

            CommonFlow.C_Model.setting.record_method = (byte)get_record_method();
            CommonFlow.C_Model.setting.data_type = data_type;

            try
            {
                CommonFlow.C_Model.setting.mc_x = (byte)((uiTextBox_RoI_MC_X.Text.Length > 0) ? Byte.Parse(uiTextBox_RoI_MC_X.Text) : 0);
                CommonFlow.C_Model.setting.mc_y = (byte)((uiTextBox_RoI_MC_Y.Text.Length > 0) ? Byte.Parse(uiTextBox_RoI_MC_Y.Text) : 0);
                CommonFlow.C_Model.setting.sc_x = (byte)((uiTextBox_RoI_SC_X.Text.Length > 0) ? Byte.Parse(uiTextBox_RoI_SC_X.Text) : 0);
                CommonFlow.C_Model.setting.sc_y = (byte)((uiTextBox_RoI_SC_Y.Text.Length > 0) ? Byte.Parse(uiTextBox_RoI_SC_Y.Text) : 0);
                CommonFlow.C_Model.setting.tip = (byte)((uiTextBox_RoI_Tip.Text.Length > 0) ? Byte.Parse(uiTextBox_RoI_Tip.Text) : 0);
                CommonFlow.C_Model.setting.ring = (byte)((uiTextBox_RoI_Ring.Text.Length > 0) ? Byte.Parse(uiTextBox_RoI_Ring.Text) : 0);
            }
            catch (Exception ErrMsg)
            {
                MyUIPage.ShowErrorDialog("系统提示", "Invalid RoI setting, please reset it\r\n" + ErrMsg.ToString(),
                    UIStyle.Red, true);
            }

            CommonFlow.C_Model.setting.roi_limit = (byte)get_RoI_limit(true);
            CommonFlow.C_Model.setting.function = (byte)get_output_function();

            CommonFlow.C_Model.setting.trigger_type = (byte)((uiCheckBox_trigger_type.Checked) ? 1 : 0);
            CommonFlow.C_Model.setting.sc_roi_limit = (byte)get_RoI_limit(false);
            CommonFlow.C_Model.setting.dmsg = get_dmsg();
            CommonFlow.C_Model.setting.report_en = (byte)((uiCheckBox_report_en.Checked) ? 1 : 0);

            CommonFlow.C_Model.c_model_setting(CommonFlow.C_Model.c_model, ref CommonFlow.C_Model.setting);

            uiTextBox_estimated_data_bytes.Text = CommonFlow.C_Model.setting.total_data_bytes.ToString();
            uiTextBox_estimated_packets.Text = CommonFlow.C_Model.setting.total_packet_counts.ToString();
            uiTextBox_estimated_report_rate.Text = CommonFlow.C_Model.setting.estimated_report_rate.ToString();
        }

        private bool is_Interrupt_method()
        {
            if (uiComboBox_record_method.SelectedItem == null)
                return false;

            return uiComboBox_record_method.SelectedItem.ToString().Contains(C_MODEL_RECORD_METHOD.Finger_Interrupt.Desc());
        }

        private void c_model_stop_dowork(object sender, DoWorkEventArgs e)
        {
            UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._C_ModelTool_.Stop_Debug_Worker();

            this.Invoke(new Action(() =>
            {
                uiButton_start.Text = "Start";
                uiButton_start.Enabled = true;
            }));
        }

        private void Start_DebugMode()
        {
            uiButton_start.Enabled = false;

            do
            {
                if (is_debuging)
                    break;

                setting_Update();

                /* check if unexpected setting */
                if (is_Interrupt_method() && CommonFlow.C_Model.setting.total_packet_counts > 4)
                {
                    MessageBox.Show(string.Format("[ERROR] unexpected packet counts: {0} > 4",
                        CommonFlow.C_Model.setting.total_packet_counts), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                else if (!is_Interrupt_method() && CommonFlow.C_Model.setting.total_packet_counts > 1)
                {
                    MessageBox.Show(string.Format("[ERROR] unexpected packet counts: {0} > 1",
                        CommonFlow.C_Model.setting.total_packet_counts), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                else if (is_Interrupt_method() && is_Pen_data_type(CommonFlow.C_Model.setting.data_type) &&
                         CommonFlow.C_Model.setting.total_packet_counts > 1)
                {
                    MessageBox.Show(string.Format("[ERROR] unexpected packet counts: {0} > 1",
                        CommonFlow.C_Model.setting.total_packet_counts), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                else if (is_Pen_data_type(CommonFlow.C_Model.setting.data_type) &&
                         (CommonFlow.C_Model.setting.tip % 2 == 0 || CommonFlow.C_Model.setting.ring % 2 == 0))
                {
                    MessageBox.Show(string.Format("[ERROR] unexpected tip: {0}, ring: {1}, should be odd number",
                        CommonFlow.C_Model.setting.tip, CommonFlow.C_Model.setting.ring), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                else if ((CommonFlow.C_Model.setting.data_type & (byte)C_MODEL_DATA_TYPE.MC) != 0 &&
                         (CommonFlow.C_Model.setting.mc_x % 2 == 0 || CommonFlow.C_Model.setting.mc_y % 2 == 0))
                {
                    MessageBox.Show(string.Format("[ERROR] unexpected mc_x: {0}, mc_y: {1}, should be odd number",
                        CommonFlow.C_Model.setting.mc_x, CommonFlow.C_Model.setting.mc_y), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                else if ((CommonFlow.C_Model.setting.data_type & (byte)C_MODEL_DATA_TYPE.SC_X) != 0 &&
                         CommonFlow.C_Model.setting.sc_x % 2 == 0)
                {
                    MessageBox.Show(string.Format("[ERROR] unexpected sc_x: {0}, should be odd number",
                        CommonFlow.C_Model.setting.sc_x), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                else if ((CommonFlow.C_Model.setting.data_type & (byte)C_MODEL_DATA_TYPE.SC_Y) != 0 &&
                         CommonFlow.C_Model.setting.sc_y % 2 == 0)
                {
                    MessageBox.Show(string.Format("[ERROR] unexpected sc_y: {0}, should be odd number",
                        CommonFlow.C_Model.setting.sc_y), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }

                uiDataGridView_info_Update();

                is_debuging = true;
                uiButton_start.Text = "Stop";

                UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.grid_data = null;
                UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.report_data = null;

                uiCheckBox_show_colorgrid.Enabled = false;
                uiComboTreeView_data_type.Enabled = false;
                uiComboBox_record_method.Enabled = false;
                uiComboBox_data_length.Enabled = false;
                uiComboBox_MC_RoI_limit.Enabled = false;
                uiComboBox_SC_RoI_limit.Enabled = false;
                uiTextBox_RoI_MC_X.Enabled = false;
                uiTextBox_RoI_MC_Y.Enabled = false;
                uiTextBox_RoI_SC_X.Enabled = false;
                uiTextBox_RoI_SC_Y.Enabled = false;
                uiTextBox_RoI_Tip.Enabled = false;
                uiTextBox_RoI_Ring.Enabled = false;
                uiCheckBox_report_en.Enabled = false;
                uiCheckBox_trigger_type.Enabled = false;

                /* Disable other MainForm UI button */
                UI_GBV.fmITS_Tool.MainButton_Disable();

                UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(
                    UI_GBV.mDev[dev_idx].LocalVariable.tpdev.dev,
                    ref CommonFlow.info[dev_idx]);

                Chart_init(CommonFlow.info[dev_idx].tp.x_ch, CommonFlow.info[dev_idx].tp.y_ch);

                UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._C_ModelTool_.Start_Debug_Worker();
            } while (false);

            uiButton_start.Enabled = true;
        }

        private void Stop_DebugMode()
        {
            do
            {
                if (!is_debuging)
                    break;

                uiButton_start.Text = "Task Cancelling";
                uiButton_start.Enabled = false;

                if (UI_GBV.mDev[dev_idx].LocalVariable.VarCModel_Multi.is_recording)
                    Record_CDC_and_Painting();

                c_model_stop_bgworker = new BackgroundWorker();
                c_model_stop_bgworker.DoWork += new DoWorkEventHandler(c_model_stop_dowork);
                c_model_stop_bgworker.WorkerSupportsCancellation = true;
                c_model_stop_bgworker.RunWorkerAsync();

                uiComboTreeView_data_type.Enabled = true;
                uiComboBox_record_method.Enabled = true;
                uiComboBox_data_length.Enabled = true;
                uiComboBox_MC_RoI_limit.Enabled = true;
                uiComboBox_SC_RoI_limit.Enabled = true;
                uiTextBox_RoI_MC_X.Enabled = true;
                uiTextBox_RoI_MC_Y.Enabled = true;
                uiTextBox_RoI_SC_X.Enabled = true;
                uiTextBox_RoI_SC_Y.Enabled = true;
                uiTextBox_RoI_Tip.Enabled = true;
                uiTextBox_RoI_Ring.Enabled = true;
                uiCheckBox_show_colorgrid.Enabled = true;
                uiCheckBox_report_en.Enabled = true;
                uiCheckBox_trigger_type.Enabled = true;

                /* Enable other MainForm UI button */
                UI_GBV.fmITS_Tool.Reset_UI();

                is_debuging = false;
                UI_GBV.mDev[dev_idx].m_GBV.Globe_Multi._C_ModelTool_.cmodel_auto_format = string.Empty;

            } while (false);
        }

        private void uiButton_start_Click(object sender, EventArgs e)
        {
            if (!is_debuging)
                Start_DebugMode();
            else
                Stop_DebugMode();
        }

        private bool is_Pen_data_type(byte data_type)
        {
            return ((data_type & (byte)C_MODEL_DATA_TYPE.PEN_RING) != 0 ||
                    (data_type & (byte)C_MODEL_DATA_TYPE.PEN_TIP) != 0);
        }

        private void data_type_Update()
        {
            data_type = 0;

            foreach (TreeNode node in uiComboTreeView_data_type.Nodes)
            {
                if (!node.Checked)
                    continue;

                data_type |= (byte)EnumTool.GetValueFromDescription<C_MODEL_DATA_TYPE>(node.Text);
            }
        }

        private uint get_dmsg()
        {
            uint dmsg = 0;

            foreach (TreeNode root in uiComboTreeView_dmsg.Nodes)
            {
                if (root.Nodes != null)
                {
                    foreach (TreeNode child in root.Nodes)
                    {
                        if (!child.Checked)
                            continue;

                        string str = string.Format("{0}:{1}", root.Text, child.Text);
                        dmsg |= (uint)EnumTool.GetValueFromDescription<C_MODEL_DMSG_TYPE>(str);
                    }
                }
            }

            return dmsg;
        }

        private uint get_data_length()
        {
            if (uiComboBox_data_length.SelectedItem == null)
                return 0;

            /* uiComboBox_data_length format: XXXX bytes */
            return UInt32.Parse(uiComboBox_data_length.SelectedItem.ToString().Split(' ')[0]);
        }

        private byte get_record_method()
        {
            if (uiComboBox_record_method.SelectedItem == null)
                return 0;

            bool is_pen = is_Pen_data_type(data_type);

            if (is_Interrupt_method())
                return (is_pen) ? (byte)C_MODEL_RECORD_METHOD.Pen_Interrupt : (byte)C_MODEL_RECORD_METHOD.Finger_Interrupt;

            return (is_pen) ? (byte)C_MODEL_RECORD_METHOD.Pen_Ctrl : (byte)C_MODEL_RECORD_METHOD.Figer_Ctrl;
        }

        private UInt16 get_RoI_limit(bool is_MC)
        {
            if (is_MC)
            {
                if (uiComboBox_MC_RoI_limit.SelectedItem == null)
                    return 0;

                return UInt16.Parse(uiComboBox_MC_RoI_limit.SelectedItem.ToString());
            }

            if (uiComboBox_SC_RoI_limit.SelectedItem == null)
                return 0;

            return UInt16.Parse(uiComboBox_SC_RoI_limit.SelectedItem.ToString());
        }

        private void uiTextBox_RoI_size_Update()
        {
            bool pen_checked = is_Pen_data_type(data_type);
            bool sc_checked = ((data_type & ((byte)C_MODEL_DATA_TYPE.SC_X | (byte)C_MODEL_DATA_TYPE.SC_Y)) != 0);
            bool mc_checked = ((data_type & (byte)C_MODEL_DATA_TYPE.MC) != 0);

            uiTableLayoutPanel_RoI_Pen.Visible = pen_checked ? true : false;
            uiTableLayoutPanel_RoI_SC.Visible = sc_checked ? true : false;
            uiTableLayoutPanel_RoI_MC.Visible = mc_checked ? true : false;
            uiPanel_RoI_size.Visible = (pen_checked || sc_checked || mc_checked) ? true : false;
        }

        private void uiComboTreeView_data_type_DropDownClosed(object sender, EventArgs e)
        {
            uiComboTreeView_data_type.Text = "";

            foreach (TreeNode node in uiComboTreeView_data_type.Nodes)
            {
                if (!node.Checked)
                    continue;

                uiComboTreeView_data_type.Text += string.Format("{0};", node.Text);
            }
        }

        private void uiComboTreeView_data_type_AfterCheck(object sender, TreeViewEventArgs e)
        {
            string mc_desc = C_MODEL_DATA_TYPE.MC.Desc();
            string sc_x_desc = C_MODEL_DATA_TYPE.SC_X.Desc();
            string sc_y_desc = C_MODEL_DATA_TYPE.SC_Y.Desc();
            string pen_tip_desc = C_MODEL_DATA_TYPE.PEN_TIP.Desc();
            string pen_ring_desc = C_MODEL_DATA_TYPE.PEN_RING.Desc();

            do
            {
                if (!e.Node.Checked)
                    break;

                string text = e.Node.Text;

                /* Select MC or SC */
                if (text.Contains(mc_desc) ||
                    text.Contains(sc_x_desc) ||
                    text.Contains(sc_y_desc))
                {
                    if (uiComboTreeView_data_type.Nodes.ContainsKey(pen_tip_desc))
                    {
                        int idx = uiComboTreeView_data_type.Nodes.IndexOfKey(pen_tip_desc);
                        uiComboTreeView_data_type.Nodes[idx].Checked = false;
                    }
                    if (uiComboTreeView_data_type.Nodes.ContainsKey(pen_ring_desc))
                    {
                        int idx = uiComboTreeView_data_type.Nodes.IndexOfKey(pen_ring_desc);
                        uiComboTreeView_data_type.Nodes[idx].Checked = false;
                    }
                    break;
                }

                /* Select Pen */
                if (text.Contains(pen_tip_desc) || text.Contains(pen_ring_desc))
                {
                    if (uiComboTreeView_data_type.Nodes.ContainsKey(mc_desc))
                    {
                        int idx = uiComboTreeView_data_type.Nodes.IndexOfKey(mc_desc);
                        uiComboTreeView_data_type.Nodes[idx].Checked = false;
                    }

                    if (uiComboTreeView_data_type.Nodes.ContainsKey(sc_x_desc))
                    {
                        int idx = uiComboTreeView_data_type.Nodes.IndexOfKey(sc_x_desc);
                        uiComboTreeView_data_type.Nodes[idx].Checked = false;
                    }

                    if (uiComboTreeView_data_type.Nodes.ContainsKey(sc_y_desc))
                    {
                        int idx = uiComboTreeView_data_type.Nodes.IndexOfKey(sc_y_desc);
                        uiComboTreeView_data_type.Nodes[idx].Checked = false;
                    }
                    break;
                }
            } while (false);

            data_type_Update();
            uiTextBox_RoI_size_Update();
            uiComboBox_RoI_limit_Update();
            uiComboTreeView_dmsg_Update();
            setting_Update();
        }

        private void uiComboTreeView_dmsg_DropDownClosed(object sender, EventArgs e)
        {
            uiComboTreeView_dmsg.Text = string.Format("0x{0:X}", get_dmsg());
        }

        private void uiComboTreeView_dmsg_AfterCheck(object sender, TreeViewEventArgs e)
        {
            uiComboTreeView_dmsg.Text = string.Format("0x{0:X}", get_dmsg());
            setting_Update();
        }

        private void uiComboBox_data_length_SelectedIndexChanged(object sender, EventArgs e)
        {
            setting_Update();
        }

        private void uiComboBox_RoI_limit_SelectedIndexChanged(object sender, EventArgs e)
        {
            setting_Update();
        }

        private void Set_DataGridView_Val(int row, string val)
        {
            uiDataGridView_info.Rows[row].Cells[1].Value = val;
        }

        private void update_report_rate(uint report_rate)
        {
            this.Invoke(new Action(() =>
            {
                Set_DataGridView_Val((int)C_MODEL_DATA_INFO.Report_Rate, report_rate.ToString());
            }));
        }

        private void check_digit(KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (!Char.IsDigit(c) && c != 8 && c != 46)
            {
                e.Handled = true;
            }
        }

        private void uiTextBox_RoI_MC_X_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_digit(e);
        }

        private void uiTextBox_RoI_MC_Y_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_digit(e);
        }

        private void uiTextBox_RoI_SC_X_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_digit(e);
        }

        private void uiTextBox_RoI_SC_Y_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_digit(e);
        }

        private void uiTextBox_RoI_Tip_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_digit(e);
        }

        private void uiTextBox_RoI_Ring_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_digit(e);
        }

        private void uiTextBox_RoI_MC_X_TextChanged(object sender, EventArgs e)
        {
            setting_Update();
        }

        private void uiTextBox_RoI_MC_Y_TextChanged(object sender, EventArgs e)
        {

            setting_Update();
        }

        private void uiTextBox_RoI_SC_X_TextChanged(object sender, EventArgs e)
        {
            setting_Update();
        }

        private void uiTextBox_RoI_SC_Y_TextChanged(object sender, EventArgs e)
        {
            setting_Update();
        }

        private void uiTextBox_RoI_Tip_TextChanged(object sender, EventArgs e)
        {
            setting_Update();
        }

        private void uiTextBox_RoI_Ring_TextChanged(object sender, EventArgs e)
        {
            setting_Update();
        }

        private void uiComboBox_record_method_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (is_Interrupt_method())
            {
                uiComboBox_data_length.Visible = false;
                uiLine_data_len.Visible = false;
            }
            else
            {
                uiComboBox_data_length.Visible = true;
                uiLine_data_len.Visible = true;
            }
            try
            {
                setting_Update();
            }
            catch (Exception ErrMsg)
            {
                MyUIPage.ShowErrorDialog("系统提示", ErrMsg.ToString(), UIStyle.Red, true);
            }
        }

        private void uiCheckBox_show_colorgrid_CheckedChanged(object sender, EventArgs e)
        {
            show_ui_colorgrid = uiCheckBox_show_colorgrid.Checked;
        }

        private void uiCheckBox_report_en_CheckedChanged(object sender, EventArgs e)
        {
            setting_Update();
        }

    }
}
