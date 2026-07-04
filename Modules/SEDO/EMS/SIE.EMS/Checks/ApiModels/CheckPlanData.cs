using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 点检计划数据
    /// </summary>
    [Serializable]
    public class CheckPlanData
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 点检计划列表数据
        /// </summary>
        public List<CheckPlanInfos> CheckPlanInfos { get; set; } = new List<CheckPlanInfos>();
    }
}
