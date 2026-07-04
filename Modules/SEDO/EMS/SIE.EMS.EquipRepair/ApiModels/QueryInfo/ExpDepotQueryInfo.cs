using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 维修经验库查询实体
    /// </summary>
    [Serializable]
    public class ExpDepotQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 设备台账ID
        /// </summary>
        public double? EquipAccountId { get; set; }

        /// <summary>
        /// 设备型号ID
        /// </summary>
        public double? EquipModelId { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 故障现象ID
        /// </summary>
        public double? DeviceAbnormalId { get; set; }

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string DeviceAbnormalRemark { get; set; }

        /// <summary>
        /// 故障描述ID
        /// </summary>
        public double? FaultDescriptionId { get; set; }
        /// <summary>
        /// 故障描述(备注)
        /// </summary>
        public string FaultDescriptionRemark { get; set; }
    }
}
