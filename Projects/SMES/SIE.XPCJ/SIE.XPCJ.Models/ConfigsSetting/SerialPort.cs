using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.ConfigsSetting
{
    [Serializable]
   public class SerialPort
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SerialPort()
        {
            PortName = "COM1";
            BaudRate = 9600;
        }

        /// <summary>
        /// 串口名称
        /// </summary>
        public string PortName
        {
            get;set;
        }
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get; set;
        }
    }
}
