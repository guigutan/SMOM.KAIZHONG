using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Devices.Abnormals.ApiModels
{
    /// <summary>
    /// 设备异常信息基础数据
    /// </summary>
    [Serializable]
    public class AbnormalBaseInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double DeviceAbnormalId { get; set; }

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double EquipTypeId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
