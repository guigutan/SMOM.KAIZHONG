using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.AndonStatisticsReports
{

    /// <summary>
    /// 安灯报表数据
    /// </summary>
    [Serializable]
    public class AndonReportInfos
    {

        /// <summary>
        /// 统计列表数据
        /// </summary>
        public EntityList<AndonStatisticsViewModel> StatisticsResultList { get; set; } = new EntityList<AndonStatisticsViewModel>();


        /// <summary>
        /// 表格统计数据
        /// </summary>
        public List<ChartsStatistics> ChartsStatisticsDatas { get; set; } = new List<ChartsStatistics>();

        /// <summary>
        /// 饼图表格统计数据
        /// </summary>
        public List<ChartsStatistics> PieChartsStatisticsDatas { get; set; } = new List<ChartsStatistics>();
    }
}
