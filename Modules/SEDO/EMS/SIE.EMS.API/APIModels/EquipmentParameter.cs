using System;

namespace SIE.EMS.API.APIModels
{
    /// <summary>
    /// 设备参数
    /// </summary>
    [Serializable]
    public class EquipmentParameter
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 实时值
        /// </summary>
        public string RealTimeValue { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }
}