using SIE.WorkBenchChartBase.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace SIE.MES.Workbench.KeyPerformances.Commons
{
    /// <summary>
    /// 图表通用ViewModel
    /// </summary>
    [Serializable]
    public class ChartCommonViewModel : BaseChartViewModel
    {
        /// <summary>
        /// 图表数据源
        /// </summary>
        public override object ChartDataSource
        {
            get
            {
                return BaseValues;
            }
        }

        /// <summary>
        /// 图表数据源
        /// </summary>
        public ObservableCollection<BaseValueViewModel> BaseValues { get; set; }
    }
}
