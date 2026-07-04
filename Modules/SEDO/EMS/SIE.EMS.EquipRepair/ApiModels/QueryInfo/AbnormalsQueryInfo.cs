using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 故障现象查询实体
    /// </summary>
    [Serializable]
    public class AbnormalsQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 故障类型
        /// 5:异常现象
        /// 10：故障描述
        /// </summary>
        public int AbnormalType { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public int RepairType { get; set; }
    }
}
