using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件出库单查询实体
    /// </summary>
    [Serializable]
    public class SparePartOutQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 查询类型
        /// 1:维修
        /// 2:保养
        /// 3:点检
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 设备台账ID
        /// </summary>
        public double EquipAccountId { get; set; }

        /// <summary>
        /// 设备型号ID
        /// </summary>
        public double ModelId { get; set; }
    }
}
