using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ITS_Studio
{
    public static class LoadResoureDll
    {
        /// <summary> 已載入DLL
        /// </summary>
        private static Dictionary<string, Assembly> LoadedDlls = new Dictionary<string, Assembly>();
        /// <summary> 已處理程式集
        /// </summary>
        private static Dictionary<string, object> Assemblies = new Dictionary<string, object>();
        /// <summary> 在對程式集解釋失敗時觸發
        /// </summary>
        /// <param name="sender">AppDomain</param>
        /// <param name="args">事件引數</param>
        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                //程式集
                Assembly ass;
                //獲取載入失敗的程式集的全名
                var assName = new AssemblyName(args.Name).FullName;
                //判斷Dlls集合中是否有已載入的同名程式集
                if (LoadedDlls.TryGetValue(assName, out ass) && ass != null)
                {
                    LoadedDlls[assName] = null;//如果有則置空並返回
                    return ass;
                }
                else
                {
                    throw new DllNotFoundException(assName);//否則丟擲載入失敗的異常
                }
            }
            catch (Exception)
            {
                return null;
                //MessageBox.Show("error:\n位置：AssemblyResolve()！\n描述：" + ex.Message);
                
            }
        }

        /// <summary> 註冊資源中的dll
        /// </summary>
        /// <param name="pattern">*表示連續的未知字元,_表示單個未知字元,如*.dll</param>
        public static void RegistDLL(string pattern = "*.dll")
        {
            System.IO.Directory.GetFiles("", "");
            //獲取呼叫者的程式集
            var ass = new StackTrace(0).GetFrame(1).GetMethod().Module.Assembly;
            //判斷程式集是否已經處理
            if (Assemblies.ContainsKey(ass.FullName))
            {
                return;
            }
            //程式集加入已處理集合
            Assemblies.Add(ass.FullName, null);
            //繫結程式集載入失敗事件(這裡我測試了,就算重複綁也是沒關係的)
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            //獲取所有資原始檔檔名
            var res = ass.GetManifestResourceNames();
            var regex = new Regex("^" + pattern.Replace(".", "\\.").Replace("*", ".*").Replace("_", ".") + "$", RegexOptions.IgnoreCase);
            foreach (var r in res)
            {
                //如果是dll,則載入
                if (regex.IsMatch(r))
                {
                    try
                    {
                        var s = ass.GetManifestResourceStream(r);
                        var bts = new byte[s.Length];
                        s.Read(bts, 0, (int)s.Length);
                        var da = Assembly.Load(bts);
                        //判斷是否已經載入
                        if (LoadedDlls.ContainsKey(da.FullName))
                        {
                            continue;
                        }
                        LoadedDlls[da.FullName] = da;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error:載入dll失敗\n位置：RegistDLL()！\n描述：" + ex.Message);
                    }
                }
            }
        }
    }
}
