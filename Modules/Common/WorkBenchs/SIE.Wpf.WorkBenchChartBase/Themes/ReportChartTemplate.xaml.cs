using DevExpress.Xpf.Charts;
using System.Windows.Controls;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// ReportChartTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class ReportChartTemplate : Grid
    {
        /// <summary>
        /// 线条系列标签
        /// </summary>
        public SeriesLabel LineSeriesLabel
        {
            get
            {
                return seriesLabel;
            }
        }

        /// <summary>
        /// 线条系列标签2
        /// </summary>
        public SeriesLabel LineSeriesLabel2
        {
            get
            {
                return seriesLabel2;
            }
        }

        /// <summary>
        /// 线条系列标签3
        /// </summary>
        public SeriesLabel LineSeriesLabel3
        {
            get
            {
                return seriesLabel3;
            }
        }

        /// <summary>
        /// 边框系列标签
        /// </summary>
        public BarSideBySideSeries2D SideSeriesLabel
        {
            get
            {
                return barSideBySideSeries2D;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportChartTemplate()
        {

            InitializeComponent();
        }
    }
}
