using DevExpress.Xpf.Charts;
using SIE.Wpf.Common.Diagram;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// BarSeriesControl.xaml 交互逻辑
    /// </summary>
    [Category("绩效透视")]
    public  class BarSeriesReportChart : ComponentItem
    {
        /// <summary>
        /// 柱形图例
        /// </summary>
        protected CustomLegendItem LegendBatch;

        /// <summary>
        /// Y轴
        /// </summary>
        protected AxisY2D _AxisY;

        /// <summary>
        /// 柱形系列
        /// </summary>
        protected BarSideBySideSeries2D BarSeries;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BarSeriesReportChart()
        {
            BuildViewContent();

            BarSeries.DisplayName = "合格率".L10N();
            Brush batchBrush = new SolidColorBrush(Color.FromRgb(18, 139, 239));
            LegendBatch.MarkerBrush = batchBrush;   //图例颜色
            BarSeries.Brush.Color = Color.FromRgb(18, 139, 239);  //柱状颜色
            BarSeries.CrosshairLabelPattern = "{S}: {V}%";
        }

        /// <summary>
        /// 加载UI页签
        /// </summary>
        private void BuildViewContent()
        {
            const string defpath = "pack://application:,,,/SIE.Wpf.WorkBenchChartBase;component/ChartControls/BarSeriesControl.xaml";
            const string uri = defpath;
            var grid = XamlParseHelper.LoadEmbeddedXaml<ComponentItem>(uri, UriKind.Absolute);
            this.Content = grid;
            LegendBatch = grid.FindName("LegendBatch") as CustomLegendItem;
            _AxisY = grid.FindName("_AxisY") as AxisY2D;
            BarSeries = grid.FindName("BarSeries") as BarSideBySideSeries2D;

            this.Loaded += Grid_Loaded;
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        protected virtual void InitControl()
        {
            BarSeries.DisplayName = "合格率".L10N();
            Brush batchBrush = new SolidColorBrush(Color.FromRgb(18, 139, 239));
            LegendBatch.MarkerBrush = batchBrush;   //图例颜色
            BarSeries.Brush.Color = Color.FromRgb(18, 139, 239);  //柱状颜色
            BarSeries.CrosshairLabelPattern = "{S}: {V}%";
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            BindingChart();
        }

        /// <summary>
        /// 运行后
        /// </summary>
        protected override void OnRun()
        {
            Refresh();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void Refresh()
        {
            BindingChart();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="barSeriesTable">柱形的数据集合(第一列为X轴显示的内容，第二列为Y轴显示内容)</param>
        protected virtual void GetData(out DataTable barSeriesTable)
        {
            barSeriesTable = new DataTable();
        }

        /// <summary>
        /// 柏拉图按维度显示
        /// </summary>
        public void BindingChart()
        {
            double minY = 0;
            double maxY;
            DataTable barSeriesTable = null;
            GetData(out barSeriesTable);

            if (barSeriesTable != null && barSeriesTable.Rows.Count > 0)
            {
                DataRow[] barSeriesRows = barSeriesTable.Select();
                maxY = barSeriesRows.Max(p => Convert.ToDouble(p[1]));  // Y轴的最大值
                minY = barSeriesRows.Min(p => Convert.ToDouble(p[1]));  // Y轴的最小值

                BarSeries.Points.Clear();
                BarSeries.DataSource = barSeriesTable;
                BarSeries.ArgumentDataMember = barSeriesTable.Columns[0].ColumnName; //绑定图表的横坐标  
                BarSeries.ValueDataMember = barSeriesTable.Columns[1].ColumnName; //绑定图表的纵坐标坐标  

                _AxisY.WholeRange = new DevExpress.Xpf.Charts.Range() { MinValue = (Math.Round((minY * 0.8 * 10), 1) / 10), MaxValue = (Math.Round((maxY * 1.1 * 10), 1) / 10) };
                _AxisY.VisualRange = new DevExpress.Xpf.Charts.Range() { MinValue = (Math.Round((minY * 0.8 * 10), 1) / 10), MaxValue = (Math.Round((maxY * 1.1 * 10), 1) / 10) };
                BarSeries.Animate();
            }
            else
            {
                _AxisY.WholeRange = new DevExpress.Xpf.Charts.Range() { MinValue = 0, MaxValue = 100 };
                _AxisY.VisualRange = new DevExpress.Xpf.Charts.Range() { MinValue = 0, MaxValue = 100 };
            }
        }
    }
}
