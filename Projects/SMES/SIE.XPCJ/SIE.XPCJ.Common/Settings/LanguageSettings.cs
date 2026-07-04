using Newtonsoft.Json;
using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Common.Settings;
using SIE.XPCJ.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.Common
{
    public class LanguageSettings
    {

        /// <summary>
        /// 语言key-value 分隔符
        /// </summary>
        private static readonly string spltString = "#*#";

        /// <summary>
        /// 语言包文件夹
        /// </summary>
        private static readonly string languageFolder = "Language";
        /// <summary>
        /// 全局语言文件
        /// </summary>
        public static Dictionary<string, string> LanguageDic { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 文化Id
        /// </summary>
        public double CultureId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }

        private static List<LanguageSettings> _all;
        public static List<LanguageSettings> All
        {
            get
            {
                if (_all == null)
                {
                    _all = LanguageSettings.LoadFromFile();
                }
                return _all;
            }
            set
            {
                _all = value;
            }
        }

        public static List<LanguageSettings> LoadFromFile()
        {
            var LanguageSettingFile = Path.Combine(Global.ExecutingPath, "LanguageSettings.json");
            if (File.Exists(LanguageSettingFile))
            {
                string text = File.ReadAllText(LanguageSettingFile);
                return JsonConvert.DeserializeObject<List<LanguageSettings>>(text);
            }
            return new List<LanguageSettings>();
        }

        /// <summary>
        /// 更新语言配置
        /// </summary>
        /// <param name="cultures"></param>
        public static void UpdateLanguageSettings()
        {
            try
            {
                var cultures = CultureService.GetAllCulture();
                List<LanguageSettings> languageSettings = new List<LanguageSettings>();

                foreach (var culture in cultures)
                {
                    languageSettings.Add(new LanguageSettings()
                    {
                        CultureId = culture.Id,
                        Code = culture.Code,
                        Name = culture.Name,
                        FileName = culture.Code + ".language"
                    });
                }

                if (languageSettings.Any())
                {
                    var text = JsonConvert.SerializeObject(languageSettings);
                    var languageSettingFile = Path.Combine(Global.ExecutingPath, "LanguageSettings.json");
                    File.WriteAllText(languageSettingFile, text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 获取语言值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLanguageValue(string key, out bool isExsitKey)
        {
            isExsitKey = false;
            if (LanguageDic.ContainsKey(key))
            {
                isExsitKey = true;
                return string.IsNullOrEmpty(LanguageDic[key]) ? key : LanguageDic[key];
            }
            return key;
        }

        /// <summary>
        /// 添加到本地
        /// </summary>
        /// <param name="resource"></param>

        public void AddLocalResource(Resource resource)
        {
            if (Global.Language != null && !LanguageDic.ContainsKey(resource.Key))
            {
                LanguageDic.Add(resource.Key, "");
            }
        }

        /// <summary>
        /// 初始化语言字典
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folder"></param>
        public void InitLanguageDictionary(LanguageSettings languageSettings)
        {
            LanguageDic.Clear();

            var path = languageFolder + "//" + languageSettings.FileName;
            path = Path.Combine(Global.ExecutingPath, path);
            //全量读取 网络获取失败不应该影响下面正常获取
            ReadLanguage(path);

            Task.Factory.StartNew(() =>
            {
                lock (Global.lockObj)
                {
                    try
                    {
                        //获取远端的对应文化编码的资源 全量更新
                        var result = CultureService.DownLoadCultureResource(languageSettings.Code);
                        if (result != null && result.Any())
                        {
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            List<string> sb = new List<string>();
                            foreach (var item in result)
                            {
                                sb.Add(string.Format("{0}" + spltString + "{1}", item.Key, item.Value));
                            }
                            if (sb.Count > 1)
                            {
                                File.WriteAllLines(path, sb);
                            }
                            //ReadLanguage(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            });
        }

        /// <summary>
        /// 读取语言项目
        /// </summary>
        /// <param name="path"></param>
        private void ReadLanguage(string path)
        {
            if (File.Exists(path))
            {
                var textLine = File.ReadAllLines(path);
                if (textLine.Length > 0)
                {
                    foreach (var item in textLine)
                    {
                        var arr = item.Split(new string[] { spltString }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length == 2)
                        {
                            LanguageDic[arr[0]] = arr[1];
                        }
                    }
                }
            }
        }
    }
}
