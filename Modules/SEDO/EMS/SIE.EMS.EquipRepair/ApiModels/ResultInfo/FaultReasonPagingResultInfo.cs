using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 故障原因信息查询分页结果实体
    /// </summary>
    [Serializable]
    public class FaultReasonPagingResultInfo : PagingResultInfo
    {
        /// <summary>
        /// 故障原因信息查询结果实体
        /// </summary>
        public List<FaultReasonResultInfo> FaultReasonResultInfos { get; set; } = new List<FaultReasonResultInfo>();
    }

    /// <summary>
    /// 故障原因信息查询结果实体
    /// </summary>
    [Serializable]
    public class FaultReasonResultInfo : BaseDataInfo
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
