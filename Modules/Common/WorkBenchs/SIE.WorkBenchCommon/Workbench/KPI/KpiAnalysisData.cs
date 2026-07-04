using System;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 绩效分析对象
    /// </summary>
    [Serializable]
    public class KpiAnalysisData
    {
        /// <summary>
        /// KPI
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public string GoalFormat { get; set; }

        /// <summary>
        /// 实际达成
        /// </summary>
        public string ActualFormat { get; set; }

        /// <summary>
        /// 预警标识
        /// </summary>
        public KpiGrade? KpiGrade { get; set; }
    }
}