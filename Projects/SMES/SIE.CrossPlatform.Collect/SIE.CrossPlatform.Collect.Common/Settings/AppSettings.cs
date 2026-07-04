using Newtonsoft.Json;

namespace SIE.CrossPlatform.Collect.Common.Settings
{
    public class AppSettings
    {
        private static AppSettings _instance;
        public static AppSettings Instance
        {
            get
            {
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
        public string AttachUrl { get; set; }
        /// <summary>
        /// Api服务地址
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// API调用超时时间
        /// </summary>
        public int ApiTimeOut { get; set; } = 30000;

        /// <summary>
        /// 是否记录API调用日志
        /// </summary>
        public bool ApiLog { get; set; } = true;

        /// <summary>
        /// 是否记录Get请求
        /// </summary>
        public bool ApiGetLog { get; set; } = true;

        /// <summary>
        /// BarTender执行文件安装的路径
        /// </summary>
        public string BarTenderExePath { get; set; }

        /// <summary>
        /// 收集文化控制 默认启用
        /// </summary>

        public bool CollectionCulture { get; set; }

        /// <summary>
        /// 开发环境 用于控制何种文化收集本地界面语言
        /// </summary>
        public string DevCulture { get; set; }

        public static AppSettings LoadFromFile()
        {
            var appSettingsPath = Path.Combine(Global.ExecutingPath, "AppSettings.json");
            if (File.Exists(appSettingsPath))
            {
                string text = File.ReadAllText(appSettingsPath);
                return JsonConvert.DeserializeObject<AppSettings>(text);
            }
            return new AppSettings();
        }

        public void SaveToFile()
        {
            var appSettingsPath = Path.Combine(Global.ExecutingPath, "AppSettings.json");
            File.WriteAllText(appSettingsPath, JsonConvert.SerializeObject(this));
        }
    }
}
