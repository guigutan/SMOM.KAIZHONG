using SIE.CrossPlatform.Collect.Common.Controls;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SIE.CrossPlatform.Collect.Common.Settings
{
    public static class Global
    {
        /// <summary>
        /// 全局多线程锁
        /// </summary>
        public static readonly object lockObj = new object();

        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }

        public static string VersionName { get; set; }

        private static string _executingPath;

        /// <summary>
        /// 当前的执行路径
        /// </summary>
        public static string ExecutingPath
        {
            get
            {
                if (string.IsNullOrEmpty(_executingPath))
                {
                    _executingPath = Directory.GetCurrentDirectory();
                    // var assemblyPath = typeof(Global).Assembly.Location;
                    // char split = '/';
                    // if (OperatingSystem.IsWindows())
                    // {
                    //     split='\\';
                    // }
                    //await MessageBox.ShowMessage(assemblyPath);
                    // assemblyPath= assemblyPath.Substring(0, assemblyPath.LastIndexOf(split));
                    // Console.Write("assemblyPath:" + assemblyPath);
                    // _executingPath = Path.GetDirectoryName(assemblyPath);
                    // if (string.IsNullOrEmpty(_executingPath))
                    // {
                    //     Console.Write("_executingPath1:" + _executingPath);
                    //     _executingPath = Directory.GetCurrentDirectory();
                    //     Console.Write("_executingPath2:" + _executingPath);
                    // }
                }
                return _executingPath;
            }
        }

        private static LanguageSettings _language;

        public static LanguageSettings Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
                if (_language != null)
                {
                    _language.InitLanguageDictionary(_language);
                }
            }
        }



    }
}
