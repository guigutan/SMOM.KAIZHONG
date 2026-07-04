using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.ConfigsSetting
{
    [Serializable]
    public class ConfigSettingBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigSettingBase()
        {
            SerialPorts = new List<SerialPort>();
        }

        /// <summary>
        /// 串口
        /// </summary>
        public List<SerialPort> SerialPorts { get; set; }


        /// <summary>
        /// 设备端口 0:USB 1 串口
        /// </summary>
        public bool DevPort { get; set; }

        /// <summary>
        ///打印机设置
        /// </summary>
        public string Printer { get; set; }
    }
}
