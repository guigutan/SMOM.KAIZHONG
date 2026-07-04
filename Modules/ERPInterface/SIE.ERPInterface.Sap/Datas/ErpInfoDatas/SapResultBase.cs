using SapNwRfc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// SAP返回基类
    /// </summary>
    public class SapResultBase
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        [SapName("E_CODE")]
        public string E_CODE { get; set; }
        /// <summary>
        /// 反馈操作消息
        /// </summary>
        [SapName("E_MSG")]
        public string E_MSG { get; set; }
    }
}
