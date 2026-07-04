using System.IO;
using System.Reflection;

namespace SIE.XPCJ.Common.Settings
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
                    _executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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
