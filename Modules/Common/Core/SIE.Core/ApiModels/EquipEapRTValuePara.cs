using System;
using System.Collections.Generic;

namespace SIE.Core.ApiModels
{
    /// <summary>
    /// 设备获取EAP实时值参数
    /// </summary>
    [Serializable]
    public class EquipEapRTValuePara
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备参数
        /// </summary>
        public List<string> Paras { get; set; }
    }
}
