using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Starter
{
    public class AppSettings
    {
        public static string AppPath { get; set; }

        private static AppSettings _instance;
        public static AppSettings Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = AppSettings.LoadFromFile();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 配置文件地址
        /// </summary>
        public string AttachUrl { get; set; } = "";
       

        private static AppSettings LoadFromFile()
        {
            string fileName = Path.Combine(AppPath, "AppSettings.json");
            if (File.Exists(fileName))
            {
                string text = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<AppSettings>(text);
            }
            return new AppSettings();
        }
    }
}
