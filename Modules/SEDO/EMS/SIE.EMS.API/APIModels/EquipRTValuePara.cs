using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.API.APIModels
{
    /// <summary>
    /// 设备实时值参数
    /// </summary>
    [Serializable]
    public class EquipRTValuePara
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备参数(参数编码，参数名称)
        /// </summary>
        public Dictionary<string, string> Paras { get; set; }
    }
}
