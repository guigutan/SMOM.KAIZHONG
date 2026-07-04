using SIE.Domain;
using SIE.MES.Statistics.Fpy;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SIE.Wpf.MES.DashBoard.Reports.Commons
{
    /// <summary>
    /// ProceessDirectRateStatistics.xaml 的交互逻辑
    /// </summary>
    public partial class ProceessDirectRateStatistics : UserControl
    {
        /// <summary>
        /// 缺陷统计数据源
        /// </summary>
        private EntityList<DefectStatistics> _topFiveSouece;

        /// <summary>
        /// 直通率统计数据源
        /// </summary>
        private EntityList<ProcessFpyStatistics> _fpySouece;

        /// <summary>
        /// 缺陷代码块颜色
        /// </summary>
        private Brush[] _brushes = new Brush[] { Brushes.Red, Brushes.OrangeRed, Brushes.Orange, Brushes.Yellow, Brushes.LightYellow };

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="topFiveSouece">缺陷统计数据源</param>
        /// <param name="fpySouece">直通率统计数据源</param>
        /// <param name="ftyTitle">直通率标题</param>
        public ProceessDirectRateStatistics(EntityList<DefectStatistics> topFiveSouece, EntityList<ProcessFpyStatistics> fpySouece, string ftyTitle = "")
        {
            InitializeComponent();
            this._topFiveSouece = topFiveSouece;
            this._fpySouece = fpySouece;
            fpyChartTitle.Content = ftyTitle.L10N();
            this.defectRecChart.Loaded += (o, e) =>
            {

                InitDefectCodeChart();
            };

            InitfpyStatisticsChartSeries();
            InitPassQtyStatisticsChartSeries();
        }

        /// <summary>
        /// 初始化工序直通率柱形图
        /// </summary>
        private void InitfpyStatisticsChartSeries()
        {
            var groups = _fpySouece.GroupBy(p => p.ProcessName).ToList();

            for (int i = 0; i < groups.Count; i++)
            {
                var argument = groups[i].Key;
                if (groups[i].Sum(p => p.InputQty) == 0)
                {
                    continue;
                }

                var value = groups[i].Sum(p => p.PassQty) / groups[i].Sum(p => p.InputQty);
                rateSerie.AddPoint(argument, (double)value);
            }

            const double group_width = 80;
            double barWidth = rateSerie.Points.Count * 1.0 / 10;
            barWidth = Math.Min(0.7, barWidth);
            barWidth = Math.Max(0.1, barWidth);
            rateSerie.BarWidth = barWidth; //柱形图宽度
            fpyStatisticsChartGram.SetAxisXZoomRatio(0);
            if (rateSerie.Points.Count > 0 && fpyStatisticsChartGram.ActualHeight > 0)
            {
                var unit_w = fpyStatisticsChartGram.ActualWidth * 0.7 / rateSerie.Points.Count;
                if (unit_w < group_width)
                {
                    var p = (group_width - unit_w) / group_width;
                    fpyStatisticsChartGram.SetAxisXZoomRatio(p);
                }
            }

            fpyStatisticsChartGram.ScrollAxisXTo(0);
            rateSerie.Animate();
        }

        /// <summary>
        /// 初始化缺陷代码TOP5统计图
        /// </summary>
        private void InitDefectCodeChart()
        {
            double ctgX = 0;
            double ctgY = 0;
            double width = defectRecChart.ActualWidth;
            double height = defectRecChart.ActualHeight;
            ////var totalDefectCount = _topFiveSouece.Sum(p => p.Qty);

            var ctgGroups = _topFiveSouece.GroupBy(p => p.CategoryName).OrderByDescending(p => p.Sum(q => q.Qty)).ToList();
            ////decimal totalDefectCount = 0M;


            var count = ctgGroups.Count > 5 ? 5 : ctgGroups.Count;
            ////for (int i = 0; i < count; i++)
            ////{
            ////    totalDefectCount += ctgGroups[i].Sum(p => p.Qty);
            ////}

            ////ctgGroups.Take(5).ForEach(p => { totalDefectCount += p.Sum(q => q.Qty); });

            decimal totalDefectCount = ctgGroups.Take(5).Sum(p => p.Sum(q => q.Qty));

            for (int i = 0; i < count; i++)
            {
                double defX = ctgX;
                double defY = ctgY;
                var defectCtgQty = ctgGroups[i].Sum(q => q.Qty);
                var dectGroups = ctgGroups[i].GroupBy(p => p.DefectName).OrderByDescending(p => p.Sum(q => q.Qty)).ToList();

                //若i = 0、2、3时画的方块
                if (i % 2 != 0)
                {
                    var tempWidth = (double)(defectCtgQty / totalDefectCount) * width;

                    for (int j = 0; j < dectGroups.Count; j++)
                    {
                        var deftQty = dectGroups[j].Sum(q => q.Qty);
                        var tempHeight = (double)(deftQty / defectCtgQty) * height;

                        string title = dectGroups[j].Key;
                        Brush backGround = _brushes[i];
                        TextBox rec = CreateDefectBox(defX, defY, tempHeight, tempWidth, title, backGround);
                        defectRecChart.Children.Add(rec);

                        if (j == 0)
                        {
                            string ctgTitle = ctgGroups[i].Key;
                            Label categoryLabel = CreateDefectCtgTitle(ctgTitle, defX, defY);
                            defectRecChart.Children.Add(categoryLabel);
                        }

                        defY += tempHeight;
                    }

                    ctgX += tempWidth;
                    width -= tempWidth;
                    totalDefectCount -= defectCtgQty;
                }
                else ////if (i % 2 == 0)
                {
                    var tempHeight = (double)(defectCtgQty / totalDefectCount) * height;

                    for (int j = 0; j < dectGroups.Count; j++)
                    {
                        var deftQty = dectGroups[j].Sum(q => q.Qty);
                        var tempWidth = (double)(deftQty / defectCtgQty) * width;

                        string title = dectGroups[j].Key;
                        Brush backGround = _brushes[i];
                        TextBox rec = CreateDefectBox(defX, defY, tempHeight, tempWidth, title, backGround);
                        defectRecChart.Children.Add(rec);

                        if (j == 0)
                        {
                            string ctgTitle = ctgGroups[i].Key;
                            Label categoryLabel = CreateDefectCtgTitle(ctgTitle, defX, defY);
                            defectRecChart.Children.Add(categoryLabel);
                        }

                        defX += tempWidth;
                    }

                    ctgY += tempHeight;
                    height -= tempHeight;
                    totalDefectCount -= defectCtgQty;
                }
            }
        }

        /// <summary>
        /// 绘制缺陷块
        /// </summary>
        /// <param name="defX">坐标X</param>
        /// <param name="defY">坐标Y</param>
        /// <param name="tempHeight">高度</param>
        /// <param name="tempWidth">宽度</param>
        /// <param name="title">标题</param>
        /// <param name="background">背景颜色</param>
        /// <returns>文本框</returns>
        private TextBox CreateDefectBox(double defX, double defY, double tempHeight, double tempWidth, string title, Brush background)
        {
            TextBox rec = new TextBox()
            {
                Text = title,
                Width = tempWidth,
                Height = tempHeight,
                FontSize = 18,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = background,
                IsReadOnly = true,
                Margin = new Thickness(1, 1, 0, 0)
            };

            Canvas.SetTop(rec, defY);
            Canvas.SetLeft(rec, defX);
            return rec;
        }

        /// <summary>
        /// 绘制缺陷分类标题
        /// </summary>
        /// <param name="title">分类标题（即分类名称）</param>
        /// <param name="defX">坐标X</param>
        /// <param name="defY">坐标Y</param>
        /// <returns>标题Label</returns>
        private Label CreateDefectCtgTitle(string title, double defX, double defY)
        {
            Label categoryLabel = new Label()
            {
                Content = title,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                Margin = new Thickness(1, 1, 0, 0)
            };

            Canvas.SetTop(categoryLabel, defY);
            Canvas.SetLeft(categoryLabel, defX);
            return categoryLabel;
        }

        /// <summary>
        /// 初始化一次良品/不良品数柱形图
        /// </summary>
        private void InitPassQtyStatisticsChartSeries()
        {
            var groups = _fpySouece.GroupBy(p => p.ProcessName).ToList();

            for (int i = 0; i < groups.Count; i++)
            {
                var argument = groups[i].Key;
                if (groups[i].Sum(p => p.InputQty) == 0)
                {
                    continue;
                }

                var passQty = groups[i].Sum(p => p.PassQty);
                var failedQty = groups[i].Sum(p => p.FailedQty);
                passQtySerie.AddPoint(argument, (double)passQty);
                failedQtySerie.AddPoint(argument, (double)failedQty);
            }

            const double group_width = 80;
            double barWidth = passQtySerie.Points.Count * 1.0 / 10;
            barWidth = Math.Min(0.7, barWidth);
            barWidth = Math.Max(0.1, barWidth);
            passQtySerie.BarWidth = barWidth;
            failedQtySerie.BarWidth = barWidth; //柱形图宽度
            qtyStatisticsCharttGram.SetAxisXZoomRatio(0);
            if (passQtySerie.Points.Count > 0 && qtyStatisticsCharttGram.ActualHeight > 0)
            {
                var unit_w = qtyStatisticsCharttGram.ActualWidth * 0.7 / passQtySerie.Points.Count;
                if (unit_w < group_width)
                {
                    var p = (group_width - unit_w) / group_width;
                    qtyStatisticsCharttGram.SetAxisXZoomRatio(p);
                }
            }

            qtyStatisticsCharttGram.ScrollAxisXTo(0);
            failedQtySerie.Animate();
            passQtySerie.Animate();
        }
    }
}
