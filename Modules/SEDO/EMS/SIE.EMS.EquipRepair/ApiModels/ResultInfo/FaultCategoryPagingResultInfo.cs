using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 设备故障类别信息分页查询实体
    /// </summary>
    [Serializable]
    public class FaultCategoryPagingResultInfo : PagingResultInfo
    {
        /// <summary>
        /// 设备故障类别
        /// </summary>
        public List<BaseDataInfo> FaultCategoryResultInfos { get; set; } = new List<BaseDataInfo>();
    }
}
