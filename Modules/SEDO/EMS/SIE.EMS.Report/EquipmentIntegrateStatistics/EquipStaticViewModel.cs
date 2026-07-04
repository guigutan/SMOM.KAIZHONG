using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// 设备综合统计结果
    /// </summary>
    [Serializable]
    public class EquipStaticViewModel
    {
        /// <summary>
        /// 统计值列表
        /// </summary>
        public EquipStaticMatrix EquipStaticMatrix { get; set; }

        /// <summary>
        /// 图表数据列表
        /// </summary>
        public EquipStaticChart EquipStaticChart { get; set; }

        /// <summary>
        /// 设备台数
        /// </summary>
        public int EquipmentCount { get; set; }

        /// <summary>
        /// MTTR(故障平均修复时间）
        /// </summary>
        public decimal Mttr { get; set; }

        /// <summary>
        /// MTBF(平均无故障工作时间）
        /// </summary>
        public decimal Mtbf { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
    }

    /// <summary>
    /// 统计值列表
    /// </summary>
    [Serializable]
    public class EquipStaticMatrix
    {
        /// <summary>
        /// 统计值列表
        /// </summary>
        public List<EquipmentIntegrateStatisticViewModel> EquipmentIntegrateStatisticViewModelList
        {
            get; set;
        }
    }

    /// <summary>
    /// 图表数据列表
    /// </summary>
    [Serializable]
    public class EquipStaticChart
    {
        /// <summary>
        /// 图表数据列表
        /// </summary>
        public List<EquipmentIntegrateStatisticInfoModel> EquipmentIntegrateStatisticInfoModelList
        {
            get; set;
        }
    }
}
