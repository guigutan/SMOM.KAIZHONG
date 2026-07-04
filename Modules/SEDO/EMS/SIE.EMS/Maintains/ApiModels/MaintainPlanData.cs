using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养计划数据
    /// </summary>
    [Serializable]
    public class MaintainPlanData
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 保养计划列表数据
        /// </summary>
        public List<MaintainPlanInfos> MaintainPlanInfos { get; set; } = new List<MaintainPlanInfos>();
    }
}
