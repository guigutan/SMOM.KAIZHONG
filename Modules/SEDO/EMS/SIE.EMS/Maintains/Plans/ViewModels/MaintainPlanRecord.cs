using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.Plans.ViewModels
{
    /// <summary>
    /// 保养计划主列信息
    /// </summary>
    [Serializable]
    public class MaintainPlanRecord
    {
        /// <summary>
        /// 计划Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId { get; set; }

        /// <summary>
        /// 年/月
        /// </summary>
        public DateTime? YearAndMonth { get; set; }

        /// <summary>
        /// 周期
        /// </summary>
        public int Cycle { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 保养类型
        /// </summary>
        public MaintainType MaintainType { get; set; }

        /// <summary>
        /// 指定计划开始时间
        /// </summary>
        public DateTime? PrecisePlanBeginDate { get; set; }

        /// <summary>
        /// 指定计划开始时间
        /// </summary>
        public DateTime? PrecisePlanEndDate { get; set; }

        /// <summary>
        /// 计划保养时长(H)
        /// </summary>
        public decimal? MaintainTime { get; set; }

        /// <summary>
        /// 保养间隔时间限制（天）
        /// </summary>
        public int? IntervalTime { get; set; }

        /// <summary>
        /// 保养周期类型
        /// </summary>
        public MaintainCycleType MaintainCycleType { get; set; }

        /// <summary>
        /// 保养类型Id
        /// </summary>
        public int MaintainTypeInfoId { get; set; }
        /// <summary>
        /// 保养类型Value
        /// </summary>
        public string MaintainTypeInfoValue { get; set; }

        /// <summary>
        /// 是否已报修
        /// </summary>
        public YesNo WhetherRepair { get; set; }
    }
}
