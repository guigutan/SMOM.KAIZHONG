using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 维修员查询实体
    /// </summary>
    [Serializable]
    public class RepairerQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipId { get; set; }

        /// <summary>
        /// 备件Id
        /// </summary>
        public double? SparePartId { get; set; }
    }
}
