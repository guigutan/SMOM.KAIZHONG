using DevExpress.Xpf.Charts;
using SIE.Wpf.Common.Diagram;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// BarLineSeriesControl.xaml 的交互逻辑
    /// </summary>
    public class BarLineSeriesReportChart : ComponentItem
    {
        /// <summary>
        /// 柱形图例
        /// </summary>
        protected CustomLegendItem LegendBatch;

        /// <summary>
        /// 折线图例
        /// </summary>
        protected CustomLegendItem LegendPercenta;

        /// <summary>
        /// Y轴
        /// </summary>
        protected AxisY2D _AxisY;

        /// <summary>
        /// 第二Y轴
        /// </summary>
        protected SecondaryAxisY2D _SecondaryAxisY2D;

        /// <summary>
        /// 柱形系列
        /// </summary>
        protected BarSideBySideSeries2D BarSeries;

        /// <summary>
        /// 折线系列
        /// </summary>
        protected LineSeries2D LineSeries;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BarLineSeriesReportChart()
        {
            BuildViewContent();

            BarSeries.DisplayName = "不良批次数".L10N();
            Brush batchBrush = new SolidColorBrush(Color.FromRgb(18, 139, 239));
            LegendBatch.MarkerBrush = batchBrush;   //图例颜色
            BarSeries.Brush.Color = Color.FromRgb(18, 139, 239);  //柱状颜色

            LineSeries.DisplayName = "占比".L10N();
            Brush percentaBrush = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            LegendPercenta.MarkerBrush = percentaBrush; //图例颜色
            LineSeries.Brush.Color = Color.FromRgb(255, 165, 0);  //折线颜色
            LineSeries.CrosshairLabelPattern = "{S}: {V}%";
        }

        /// <summary>
        /// 加载UI页面
        /// </summary>
        private void BuildViewContent()
        {
            const string defpath = "pack://application:,,,/SIE.Wpf.WorkBenchChartBase;component/ChartControls/BarLineSeriesControl.xaml";
            const string uri = defpath;
            var grid = XamlParseHelper.LoadEmbeddedXaml<ComponentItem>(uri, UriKind.Absolute);
            this.Content = grid;

            LegendBatch = grid.FindName("LegendBatch") as CustomLegendItem;
            LegendPercenta = grid.FindName("LegendPercenta") as CustomLegendItem;
            _AxisY = grid.FindName("_AxisY") as AxisY2D;
            _SecondaryAxisY2D = grid.FindName("_SecondaryAxisY2D") as SecondaryAxisY2D;
            BarSeries = grid.FindName("BarSeries") as BarSideBySideSeries2D;
            LineSeries = grid.FindName("LineSeries") as LineSeries2D;

            this.Loaded += Grid_Loaded;
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        protected virtual void InitControl()
        {
            BarSeries.DisplayName = "不良批次数".L10N();
            Brush batchBrush = new SolidColorBrush(Color.FromRgb(18, 139, 239));
            LegendBatch.MarkerBrush = batchBrush;   //图例颜色
            BarSeries.Brush.Color = Color.FromRgb(18, 139, 239);  //柱状颜色

            LineSeries.DisplayName = "占比".L10N();
            Brush percentaBrush = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            LegendPercenta.MarkerBrush = percentaBrush; //图例颜色
            LineSeries.Brush.Color = Color.FromRgb(255, 165, 0);  //折线颜色
            LineSeries.CrosshairLabelPattern = "{S}: {V}%";
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="barSeriesTable">柱形的数据集合(第一列为X轴显示的内容，第二列为Y轴显示内容)</param>
        /// <param name="lineSeriesTable">折线的数据集合(第一列为X轴显示的内容，第二列为Y轴显示内容)</param>
        protected virtual void GetData(out DataTable barSeriesTable, out DataTable lineSeriesTable)
        {
            barSeriesTable = new DataTable();
            lineSeriesTable = new DataTable();
        }

        /// <summary>
        /// 初始加載
        /// </summary>
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
        /// 柏拉图按维度显示
        /// </summary>
        public void BindingChart()
        {
            double maxY;
            double _secondaryAxisYMinValue = 0;
            DataTable barSeriesTable = null;
            DataTable lineSeriesTable = null;

            GetData(out barSeriesTable, out lineSeriesTable);
            if (barSeriesTable != null && barSeriesTable.Rows.Count > 0)
            {
                BarSeries.Points.Clear();
                DataRow[] barSeriesRows = barSeriesTable.Select();
                maxY = barSeriesRows.Max(p => Convert.ToDouble(p[1]));
                BarSeries.DataSource = barSeriesTable;
                BarSeries.ArgumentDataMember = barSeriesTable.Columns[0].ColumnName; //绑定图表的横坐标  
                BarSeries.ValueDataMember = barSeriesTable.Columns[1].ColumnName; //绑定图表的纵坐标坐标

                LineSeries.Points.Clear();
                DataRow[] lineSeriesRows = lineSeriesTable.Select();
                _secondaryAxisYMinValue = lineSeriesRows.Min(p => Convert.ToDouble(p[1]));
                LineSeries.DataSource = lineSeriesTable;
                LineSeries.ArgumentDataMember = lineSeriesTable.Columns[0].ColumnName; //绑定图表的横坐标  
                LineSeries.ValueDataMember = lineSeriesTable.Columns[1].ColumnName; //绑定图表的纵坐标坐标  

                _AxisY.WholeRange = new DevExpress.Xpf.Charts.Range() { MinValue = 0, MaxValue = (Math.Round((maxY * 1.1 * 10), 1) / 10) };
                _AxisY.VisualRange = new DevExpress.Xpf.Charts.Range() { MinValue = 0, MaxValue = (Math.Round((maxY * 1.1 * 10), 1) / 10) };
                _SecondaryAxisY2D.WholeRange = new DevExpress.Xpf.Charts.Range() { MinValue = (Math.Round((_secondaryAxisYMinValue * 0.8 * 10), 1) / 10), MaxValue = 100 };
                _SecondaryAxisY2D.VisualRange = new DevExpress.Xpf.Charts.Range() { MinValue = (Math.Round((_secondaryAxisYMinValue * 0.8 * 10), 1) / 10), MaxValue = 100 };

                BarSeries.Animate();
                LineSeries.Animate();
            }
            else
            {
                _SecondaryAxisY2D.WholeRange = new DevExpress.Xpf.Charts.Range() { MinValue = 0, MaxValue = 100 };
                _SecondaryAxisY2D.VisualRange = new DevExpress.Xpf.Charts.Range() { MinValue = 0, MaxValue = 100 };
            }
        }
    }
}
