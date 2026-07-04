using SIE.CrossPlatform.Collect.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.CrossPlatform.Collect.Common.Controls
{
    /// <summary>
    /// 代替原
    /// </summary>
    public static class PropertiesSettingsUlits
    {
        private const string propertiesSettingsDriPath = "SieProperties";
        /// <summary>
        /// 工作单元信息 缓存值
        /// </summary>
        public static string Workcell { get; set; }


        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetProperties(string key)
        {
            var fileName = key + ".Properties";
            var ioPath = Path.Combine(Global.ExecutingPath, propertiesSettingsDriPath, fileName);
            if (!File.Exists(ioPath))
            {
                return "";
            }
            else
            {
                return File.ReadAllText(ioPath);
            }

        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void SetProperties(string key, string content)
        {
            var fileName = key+".Properties";
            var writePath =Path.Combine(Global.ExecutingPath, propertiesSettingsDriPath);
            if (!Directory.Exists(writePath))
            {
                Directory.CreateDirectory(writePath);
            }

            var ioFilePath = Path.Combine(Global.ExecutingPath, propertiesSettingsDriPath, fileName);
            if (File.Exists(ioFilePath))
            {
                File.Delete(ioFilePath);
            }
            File.WriteAllText(ioFilePath, content);

        }
    }
}
