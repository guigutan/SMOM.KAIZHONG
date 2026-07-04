using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// ESD报表数据
    /// </summary>
    [Serializable]
    public class EquipCostAnalysesInfo
    {
        /// <summary>
        /// ESD检测通过趋势率报表数据
        /// </summary>
        public EntityList<EquipCostInfo> EquipCostInfoInfo { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 设备数
        /// </summary>
        public string EquipmentCount { get; set; }

        /// <summary>
        /// 表格数据
        /// </summary>
        public EntityList<MonthlyCostInfo> MonthlyCostInfoList { get; set; }
    }
}
