using System;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养总结
    /// </summary>
    [Serializable]
    public class MaintainSummary
    {
        /// <summary>
        /// 上次保养总结
        /// </summary>
        public string UpMaintainSummary { get; set; }

        /// <summary>
        /// 指定计划开始日期
        /// </summary>
        public DateTime? PrecisePlanBeginDate { get; set; }

        /// <summary>
        /// 指定计划结束日期
        /// </summary>
        public DateTime? PrecisePlanEndDate { get; set; }
    }
}
