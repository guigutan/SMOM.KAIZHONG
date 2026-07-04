using SIE.ObjectModel;
using SIE.WorkBenchChartBase.Commons;
using System;

namespace SIE.WorkBenchChartBase.ViewModels
{
    /// <summary>
    /// 报表ViewModel基类
    /// </summary>
    [Serializable]
    public abstract class BaseChartViewModel : ObservableObject
    {
        /// <summary>
        /// 图表标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标预警级别
        /// </summary>
        public ChartAlertLevel ChartAlertLevel { get; set; }

        /// <summary>
        /// 是否被收藏
        /// </summary>
        public bool IsCollect { get; set; }

        /// <summary>
        /// 图表数据源
        /// </summary>
        public abstract object ChartDataSource { get; }
    }

    /// <summary>
    /// 报表ViewModel基类
    /// </summary>
    public class TestChartViewModel : BaseChartViewModel
    {
        /// <summary>
        /// 图表数据源
        /// </summary>
        public override object ChartDataSource
        {
            get
            {
                return null;
            }
        }
    }
}
