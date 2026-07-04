using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.MES.Workbench.KeyPerformances;
using SIE.MES.Workbench.KeyPerformances.Commons;
using SIE.WorkBenchChartBase.Commons;
using SIE.WorkBenchChartBase.ViewModels;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.WorkBenchChartBase.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SIE.Wpf.MES.Workbench.KeyPerformances
{
    /// <summary>
    /// 车间计划达成率组件
    /// </summary>
    [QuotaAttribute(_groupName, _title)]
    [ChartDefinitionAttribute("关键指标", _title, GroupName = _groupName, Module = ModuleFlag.MES, Url = "/SIE.Wpf.MES.Workbench;component/Images/PlanReached.png")]
    public class FinishedPtgOfPlanControl : LineChart
    {
        /// <summary>
        /// 指标名称
        /// </summary>
        private const string _title = "生产计划达成率";

        /// <summary>
        /// 指标分类
        /// </summary>
        private const string _groupName = "效率类";

        /// <summary>
        /// 视图模块键
        /// </summary>
        private string _viewModuleKey;

        /// <summary>
        /// 构建数据上下文
        /// </summary>
        /// <returns>数据MODEL</returns>
        protected override BaseChartViewModel BuildDataContext()
        {
            if (!_workShopId.HasValue)
            {
                return new SIE.MES.Workbench.KeyPerformances.Commons.ChartCommonViewModel() { Title = _title };
            }

            var model = RT.Service.Resolve<CommonController>().BuildShopPRModel(_title, _workShopId.Value, _dbTime);
            if (model != null)
            {
                var points = model.BaseValues.Select(p => new SeriesPoint(p.Date.Month + "/" + p.Date.Day, p.Efficiency * 100)).ToList();
                model.ChartAlertLevel = GetChartAlertLevel(points, _groupName, _title);
            }
            var range = new ChartHelper().GetYRange(0, 1);

            _diagram.AxisY.WholeRange = range;
            _diagram.AxisY.VisualRange = range;
            return model;
        }

        /// <summary>
        /// 创建X轴
        /// </summary>
        /// <returns>X轴</returns>
        protected override AxisX2D CreateAxisx()
        {
            var axisX = new AxisX2D
            {
                Name = "_AxisX",
                TickmarksMinorVisible = false,
                TickmarksLength = 3,
                GridLinesMinorVisible = false,
                GridLinesVisible = false,
                CrosshairAxisLabelOptions = new CrosshairAxisLabelOptions(),
            };

            axisX.Label = new AxisLabel
            {
                Background = Brushes.Transparent,
                BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FF343246"),
                Padding = new Thickness(0, 1, 0, 0),
                FontSize = 12,
                TextPattern = "{V:M/d}",
            };

            axisX.DateTimeScaleOptions = new ContinuousDateTimeScaleOptions()
            {
                AutoGrid = false,
                GridSpacing = 1,
            };
            axisX.SetValue(Axis2D.ResolveOverlappingOptionsProperty, new ChartHelper().CreateXOptions());
            return axisX;
        }

        /// <summary>
        /// 创建Y轴
        /// </summary>
        /// <returns>Y轴</returns>
        protected override AxisY2D CreateAxisY()
        {
            var axisY = new AxisY2D
            {
                Name = "_AxisY",
                GridLinesMinorVisible = false,
                GridLinesVisible = true,
                TickmarksMinorVisible = true,
                TickmarksLength = 3,
                Interlaced = false
            };

            axisY.Label = new AxisLabel
            {
                Background = Brushes.Transparent,
                BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FF343246"),
                Padding = new Thickness(0, 1, 0, 0),
                FontSize = 12,
                TextPattern = "{V:P0}"
            };

            var range = new DevExpress.Xpf.Charts.Range
            {
                MaxValue = 1,
                MinValue = 0
            };
            AxisY2D.SetAlwaysShowZeroLevel(range, false);

            axisY.WholeRange = range;

            return axisY;
        }

        /// <summary>
        /// 创建折线
        /// </summary>
        /// <returns>折线</returns>
        protected override List<Series> CreateSeries()
        {
            var seriesList = base.CreateSeries();
            SolidColorBrush scb = new SolidColorBrush();
            scb.Color = Color.FromRgb(30, 144, 255); //道奇蓝
            var seriesActual = new AreaSeries2D()
            {
                Name = "_LineSerieActual",
                DisplayName = "实际".L10N(),
                Brush = scb,
                Foreground = Brushes.SkyBlue,
                Transparency = 0.8,
                MarkerVisible = true,
                LabelsVisibility = true,
                CrosshairLabelVisibility = true,
                AnimationAutoStartMode = AnimationAutoStartMode.SetStartState,
                ArgumentScaleType = ScaleType.DateTime,
                ArgumentDataMember = "Date",
                ValueDataMember = "Efficiency",
                MarkerModel = new CircleMarker2DModel(),
                CrosshairLabelPattern = "{A:M/d}: {V:P1}",
                ShowInLegend = true
            };

            seriesActual.Label = new SeriesLabel
            {
                Background = Brushes.Transparent,
                Opacity = 0.75,
                ConnectorVisible = false,
                ResolveOverlappingMode = ResolveOverlappingMode.Default,
                Indent = 1,
                TextPattern = "{V:P1}"
            };

            seriesActual.Label.ElementTemplate = new ChartHelper().CreateLabelElementTemplate();
            seriesList.Add(seriesActual);

            return seriesList;
        }

        /// <summary>
        /// 自定义Legend
        /// </summary>
        /// <returns>Legend</returns>
        protected override Legend CreateLegend()
        {
            var legend = base.CreateLegend();
            legend.BorderThickness = new Thickness(0);
            legend.Padding = new Thickness(0, -2, 0, 0);
            legend.Margin = new Thickness(-20, -6.8, -10, -15);
            legend.VerticalPosition = VerticalPosition.TopOutside;
            legend.Orientation = Orientation.Horizontal;
            legend.HorizontalPosition = HorizontalPosition.Right;
            return legend;
        }

        /// <summary>
        /// 获取限制值
        /// </summary>
        /// <returns>限制值列表</returns>
        protected override List<ConstantLine> GetConstantLines()
        {
            var constantLineList = base.GetConstantLines();
            if (_workShopId.HasValue)
                constantLineList.Add(new ChartHelper().CreateConstantLine(_groupName, _title, _dbTime.Month, _workShopId.Value));
            return constantLineList;
        }

        /// <summary>
        /// 图标双击事件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        protected override void Chart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((_baseModel as SIE.MES.Workbench.KeyPerformances.Commons.ChartCommonViewModel).BaseValues?.Count > 0)
                ShowDrillView(_title + "详细");
        }

        /// <summary>
        /// 显示下钻产线达成率图表
        /// </summary>
        /// <param name="title">标题</param>
        private void ShowDrillView(string title)
        {
            if (_viewModuleKey.IsNullOrEmpty())
                _viewModuleKey = Guid.NewGuid().ToString();

            CRT.Workbench.ShowView(_viewModuleKey, v =>
            {
                v.Title = title.L10N();

                var helper = new ChartHelper();
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                var lineRpInfos = RT.Service.Resolve<CommonController>().GetLinePRInfos(_workShopId.Value, _dbTime);
                var dics = RT.Service.Resolve<TargetSettingController>().GetLineTargetSettings(lineRpInfos.Select(p => p.LineId).Distinct().ToList(), TargetSettingType.PlanReachedPercent).ToDictionary(p => p.LineId);
                int col = 0;
                int row = 0;
                lineRpInfos.GroupBy(p => p.LineName).ForEach(p =>
                  {
                      var datasource = p.ToList();
                      var key = datasource.FirstOrDefault().LineId;
                      LineTargetSetting setting;
                      var control = new CommonLineChartControl(_title + " - ".L10N() + p.Key, datasource);
                      control.AddSeries(new List<Series>
                      {
                        helper.CreateSeries2D("实际达成率", "Date", "Percentage")
                      });
                      if (dics.TryGetValue(key, out setting))
                      {
                          control.AddYConstantLines(new List<ConstantLine>
                          {
                            helper.CreateConstantLine(setting, "目标达成率")
                          });
                      }

                      double minVal = datasource.Min(q => q.Percentage);
                      double maxVal = datasource.Max(q => q.Percentage);
                      control.SetYRange(minVal, maxVal);

                      if (col > 2)
                      {
                          col = 0;
                          row++;
                      }

                      if (row == 0)
                          grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                      else if (row > 1)
                          grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                      Grid.SetColumn(control, col++);
                      Grid.SetRow(control, row);
                      grid.Children.Add(control);
                  });

                return grid;
            });
        }
    }
}
