using DevExpress.Xpf.Charts;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.KeyPerformances
{
    /// <summary>
    /// CommonLineChartControl.xaml 的交互逻辑
    /// </summary>
    public partial class CommonLineChartControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="obj">数据上下文</param>
        public CommonLineChartControl(string title, object obj)
        {
            InitializeComponent();
            this.DataContext = obj;
            _title.Content = title;
        }

        /// <summary>
        /// 添加Series
        /// 若想将Series添加到Legend里面需将ShowInLegend属性设为true
        /// 此处需设置传入Series的Path
        /// </summary>
        public void AddSeries(List<Series> seriesList)
        {
            _diagram.Series.AddRange(seriesList);
        }

        /// <summary>
        /// 添加Series
        /// 若想将ConstantLine添加到Legend里面需设置LegendText属性
        /// </summary>
        public void AddYConstantLines(List<ConstantLine> constantLineList)
        {
            _axisY.ConstantLinesInFront.AddRange(constantLineList);
        }

        /// <summary>
        /// 设置Y轴范围
        /// </summary>
        /// <param name="minVal">最大值</param>
        /// <param name="maxVal">最小值</param>
        public void SetYRange(double minVal, double maxVal)
        {
            var range = new ChartHelper().GetYRange(minVal, maxVal);
            _axisY.WholeRange = range;
            _axisY.VisualRange = range;
        }
    }
}
