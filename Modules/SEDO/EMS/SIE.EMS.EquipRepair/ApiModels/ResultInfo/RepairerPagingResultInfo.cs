using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 维修人信息查询结果实体
    /// </summary>
    [Serializable]
    public class RepairerPagingResultInfo : PagingResultInfo
    {
        /// <summary>
        /// 故障信息查询结果明细实体
        /// </summary>
        public List<RepairerResultInfo> RepairerResultInfos { get; set; } = new List<RepairerResultInfo>();
    }

    /// <summary>
    /// 维修人信息查询结果明细实体
    /// </summary>
    [Serializable]
    public class RepairerResultInfo
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 员工编码
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 待维修任务数
        /// </summary>
        public int WaitRepairCount { get; set; }

        /// <summary>
        /// 维修中任务数
        /// </summary>
        public int RepairingCount { get; set; }
    }
}
