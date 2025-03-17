using CommonExt;
using ILITek_ITS_Tool_Lib;
using ITS_Studio.AllForms;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ITS_Console;
using System.IO;
using FuncMethodCmd;
using System.Reflection;
using ITS_Studio.Components;

namespace ITS_Studio
{
    public enum STRESS_BUILD_Show
    {
        NG,
        OK,
        Ready,
    }

    internal static class Program
    {
        public static bool KeepRunning
        {
            get;
            set;
        }
        
#if STRESS_BUILD
        public static bool bSTRESS_BUILD = true;
#else
        public static bool bSTRESS_BUILD = false;
#endif
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll", EntryPoint = "GetSystemDefaultLCID")]
        public static extern int GetSystemDefaultLCID();
        //20220726 Steven
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);
        private static bool ui = true;
        //
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        /// 
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyPath = "";
            string assemblyName = new AssemblyName(args.Name).Name;
            if(assemblyName.Contains(".resources") || assemblyName.Contains(".XmlSerializers"))
                return null;

            assemblyPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\dll\" + assemblyName + ".dll";

            if(File.Exists(assemblyPath))
                return Assembly.LoadFrom(assemblyPath);

            string errdll_list = string.Format("{0}\r\n", assemblyPath);
            if(ui)
                MessageBox.Show(new Form
                {
                    TopMost = true
                }, string.Format("Dll not found! ===> {0}", errdll_list), "Load .dll Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Console.Write(string.Format("Dll not found! ===> {0}", errdll_list));
            return null;
        }

        static Program()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        [STAThread]
        private static void Main(string[] args)
        {           
            if(Environment.OSVersion.Version.Major <= 5) //Steven : 從Windows Vista開始Major都會>5(XP不能使用，待觀察))
            {
                MessageBox.Show("Windows XP is no longer supported\r\nplease use a version of Windows 7 or higher.", "Ilitek", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            //20220726 Steven
            var myPath = new Uri(typeof(ILITek_ITS_Tool).Assembly.CodeBase).LocalPath;
            var myFolder = Path.GetDirectoryName(myPath);
            var is64 = IntPtr.Size == 8;
            string _FolderPath = string.Format(@"{0}\{1}\{2}", myFolder, "dll", (is64 ? @"" : @"x86\"));
            LoadLibrary(_FolderPath + "msvcr120.dll");
            LoadLibrary(_FolderPath + "msvcp120.dll");
            LoadLibrary(_FolderPath + "GenLog.dll");
            LoadLibrary(_FolderPath + "CommonFlow.dll");
            SetCommonFlowFlag();
            
            ThirdPartyTools.INI_Tool MY_INI = new ThirdPartyTools.INI_Tool(ITS_Directory.Setting_INI);
            var bDetectLanguage = MY_INI.IniReadBool("Main", "DetectLanguage", false);
            if(bDetectLanguage)
            {
                int lc = GetSystemDefaultLCID();
                //簡中:0x804，繁中:0x404
                if((lc == 0x804) || (lc == 0x404))
                    StaticVar.g_Lang = enLangTable.LANG_TRADITIONAL_CHINESE;
                else
                    StaticVar.g_Lang = enLangTable.LANG_ENGLISH;
            }
            if(args.Length == 0)
            {                
                ExecuteITS_UI();                
            }
            else if(args[0] == enITS_WakeUpMode.AUTO_BARCODE_MODE.ToString()
                || args[0] == enITS_WakeUpMode.PIPE_MODE.ToString()
                || args[0] == enITS_WakeUpMode.SOCKET_MODE.ToString())
            {
                ExecuteITS_UI(args);
            }
            else
            {
                ExecuteConsole(args);
            }            
       }

        private static void SetCommonFlowFlag()
        {
            string execPath = Assembly.GetEntryAssembly().Location;
            execPath = Path.GetDirectoryName(execPath);
            
            // 先尝试读取操作系统环境变量
            //string debugSetting = Environment.GetEnvironmentVariable("STRESS_BUILD");
            // 如果操作系统没有设置环境变量，则从配置文件中读取
            //if(string.IsNullOrEmpty(debugSetting))
            //{
            //    debugSetting = ConfigurationManager.AppSettings["DebugMode"];
            //}
            if(File.Exists(string.Format(@"{0}\STRESS_BUILD", execPath)))
                bSTRESS_BUILD = true;

            if (Directory.Exists(execPath + "\\Bypass_Console_Print"))
            {
                CommonFlow.set_print_en(false);
            }

            if (Directory.Exists(execPath + "\\CommonFlowMsg"))
            {
                CommonFlow.set_log_level((int)CommonFlow.ilitek_log_level.log_level_msg);
                CommonFlow.set_log_fopen(string.Format("{0}/CommonFlowMsg/CommonFlow_{1}.txt", execPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            }
            else if (Directory.Exists(execPath + "\\CommonFlowDbg"))
            {
                CommonFlow.set_log_level((int)CommonFlow.ilitek_log_level.log_level_dbg);
                CommonFlow.set_log_fopen(string.Format("{0}/CommonFlowDbg/CommonFlow_{1}.txt", execPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            }
            else if (Directory.Exists(execPath + "\\CommonFlowPkt"))
            {
                CommonFlow.set_log_level((int)CommonFlow.ilitek_log_level.log_level_pkt);
                CommonFlow.set_log_fopen(string.Format("{0}/CommonFlowPkt/CommonFlow_{1}.txt", execPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            }
            else if (Directory.Exists(execPath + "\\CommonFlowErr"))
            {
                CommonFlow.set_log_level((int)CommonFlow.ilitek_log_level.log_level_err);
                CommonFlow.set_log_fopen(string.Format("{0}/CommonFlowErr/CommonFlow_{1}.txt", execPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            }
        }
        
        private static void ExecuteITS_UI(params object[] parms)
        {
            CultureManager.ApplicationUICulture = CultureInfo.CurrentCulture;
            if(CultureManager.ApplicationUICulture.Name != "en-US")
                CultureManager.ApplicationUICulture = new CultureInfo("en-US");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(!StaticVar.InitBridgeSettings())
                return;            
            UI_GBV.mDev = new CtrlILIDevice[StaticVar.iMaxSupportDev];
            LegalNotice Test = new LegalNotice();
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {                
                Application.Run(Test);
            });
            UI_GBV.fmITS_Tool = new ILITek_ITS_Tool(parms);
            KeepRunning = false;
            var _task = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Test.Invoke(new Action(() =>
                {
                    Test.Close();
                    Test = null;
                }));
            });

            if(_task.Status != System.Threading.Tasks.TaskStatus.RanToCompletion)
                _task.Wait();
            Application.Run(UI_GBV.fmITS_Tool);
        }

        private static void ExecuteConsole(string[] args)
        {
            AllocConsole();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            //Console.Beep();
            ConsoleColor oriColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("* Do not close this console window, otherwise the application will also close.");
            Console.ForegroundColor = oriColor;
            ConsoleClass console = new ConsoleClass();
            console.Run(args);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }

    }
}
