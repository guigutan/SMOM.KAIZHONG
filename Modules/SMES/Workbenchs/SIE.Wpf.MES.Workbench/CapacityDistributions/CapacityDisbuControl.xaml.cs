using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.MES.Statistics.Entities;
using SIE.MES.Statistics.WIP;
using SIE.MES.Workbench.CapacityDistributions;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.Helper;
using SIE.Wpf.MES.Workbench.KeyPerformances;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace SIE.Wpf.MES.Workbench.CapacityDistributions
{
    /// <summary>
    /// CapacityDisbuControl.xaml 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class CapacityDisbuControl : ComponentItem
    {
        /// <summary>
        /// X轴范围最小值
        /// </summary>
        private double _minValueX = 0.5;

        /// <summary>
        /// X轴范围最大值
        /// </summary>
        private double _maxValueX = 0.5;

        /// <summary>
        /// Y轴范围最小值
        /// </summary>
        private double _minValueY = 1;

        /// <summary>
        /// Y轴范围最大值
        /// </summary>
        private double _maxValueY = 1;

        /// <summary>
        /// 数据源集合
        /// </summary>
        private ObservableCollection<SeriesViewData> seriesDatas = new ObservableCollection<SeriesViewData>();

        /// <summary>
        /// 异常停线数据源集合
        /// </summary>
        private EntityList<AbnormalCause> lineChartDatas = new EntityList<AbnormalCause>();

        /// <summary>
        /// 产线资源Id
        /// </summary>
        private double _resourceId;

        /// <summary>
        /// 班次Id
        /// </summary>
        private double _shiftId;

        /// <summary>
        /// 输入参数类
        /// </summary>
        private CapacityDistributionInput _input;

        /// <summary>
        /// 计时器
        /// </summary>
        private DispatcherTimer _timer;

        /// <summary>
        /// 属性定义
        /// </summary>
        CapacityDisbuControlProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CapacityDisbuControl()
        {
            InitializeComponent();
            _input = UseInput<CapacityDistributionInput>();
            _input.PropertyChanged += CapacityDistributionInput_PropertyChanged;
            _property = UseProperty<CapacityDisbuControlProperty>();
            _timer = new DispatcherTimer();
        }

        /// <summary>
        /// 工作台运行后的方法
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            LoadResourceShift();
            _timer.Tick += (o, e) =>
            {
                SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
                {
                    RefreshData();
                });
            };

            _timer.Interval = TimeSpan.FromMinutes(_property.TimeSpan <= 0 ? 3 : _property.TimeSpan);
            _timer.Start();
        }

        /// <summary>
        /// 工作台关闭后执行方法
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            _timer.Stop();
        }

        /// <summary>
        /// 加载本地设置的资源Id、班次Id
        /// </summary>
        private void LoadResourceShift()
        {
            var resourceShift = SettingsHelper.GetResourceShift();
            _resourceId = resourceShift.ResourceId;
            _shiftId = resourceShift.ShiftId;
            /*_resourceId = 86; //测试使用
            _shiftId = 42;*/

            RefreshData();
        }

        /// <summary>
        /// 资源、班次输入值变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void CapacityDistributionInput_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _resourceId = _input.ResourceId;
            _shiftId = _input.ShiftId;
            RefreshData();
        }

        /// <summary>
        /// 刷新组件
        /// </summary>
        void RefreshData()
        {
            if (_resourceId <= 0 || _shiftId <= 0) return;
            if (RF.GetById<WipResource>(_resourceId) == null || RF.GetById<Shift>(_shiftId) == null)
                return;
            var nowTime = RF.Find<WorkOrderStatistics>().GetDbTime();
            InitCptyChart(nowTime);
            InitLineData(nowTime);
        }

        #region 当日产能分布图形图形数据初始化
        /// <summary>
        /// 初始化当日产能分布图
        /// </summary>
        void InitCptyChart(DateTime nowTime)
        {
            SetDataSource(nowTime);
            SetYRange();
            SetChartDiagramProperty();
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        void SetDataSource(DateTime nowTime)
        {
            seriesDatas.Clear();

            var capaStats = GetWorkOrderStatisticsList(_resourceId, _shiftId, nowTime);
            var dicYDataPlan = GetSeriesYDataPlan(capaStats, nowTime);
            var dicYDataActual = GetSeriesYDataActual(capaStats, nowTime);
            dicYDataPlan.ForEach(p =>
            {
                var viewData = new SeriesViewData()
                {
                    Argu = p.Key,
                    StartValue = 0,
                    PlanValue = p.Value,
                    ActualValue = dicYDataActual.ContainsKey(p.Key) ? dicYDataActual[p.Key] : 0
                };
                viewData.PlanValueTwo = viewData.ActualValue >= viewData.PlanValue ? viewData.PlanValue : 0;
                seriesDatas.Add(viewData);
            });
            cptyChart.DataContext = seriesDatas;
        }

        /// <summary>
        /// 设置Y轴范围
        /// </summary>
        void SetYRange()
        {
            if (seriesDatas == null || seriesDatas.Count == 0) return;
            _minValueX = seriesDatas.Min(p => p.Argu);
            _maxValueX = seriesDatas.Max(p => p.Argu);
            var rangeX = new ChartHelper().GetYRange(_minValueX - 1, _maxValueX + 1);
            rangeX.SideMarginsValue = 0;
            AxisY2D.SetAlwaysShowZeroLevel(rangeX, false);
            axisX.WholeRange = rangeX;
            axisX.VisualRange = rangeX;
            lineAxisY.WholeRange = rangeX;
            lineAxisY.VisualRange = rangeX;

            if (seriesDatas.Min(p => p.ActualValue) > seriesDatas.Min(p => p.PlanValue))
                _minValueY = seriesDatas.Min(p => p.PlanValue);
            else
                _minValueY = seriesDatas.Min(p => p.ActualValue);
            _minValueY -= Math.Ceiling(_minValueY / 3);

            if (seriesDatas.Max(p => p.ActualValue) > seriesDatas.Max(p => p.PlanValue))
                _maxValueY = seriesDatas.Max(p => p.ActualValue);
            else
                _maxValueY = seriesDatas.Max(p => p.PlanValue);
            _maxValueY += Math.Ceiling(_maxValueY / 3);

            var rangeY = new ChartHelper().GetYRange(_minValueY - 500 < 0 ? _minValueY : _minValueY - 500, _maxValueY);
            rangeY.SideMarginsValue = 0;
            AxisY2D.SetAlwaysShowZeroLevel(rangeY, false);
            axisY.WholeRange = rangeY;
            axisY.VisualRange = rangeY;
            var numericScaleOptions = new ContinuousNumericScaleOptions();
            numericScaleOptions.GridSpacing = _minValueY > 0 ? _minValueY : 1;
            numericScaleOptions.AutoGrid = false;
            axisY.NumericScaleOptions = numericScaleOptions;
            lineAxisX.CustomLabels.Add(new CustomAxisLabel
            {
                Content = _maxValueY,
                Value = 0,
                Visible = true,
            });
        }

        /// <summary>
        /// 根据资源(产线)Id获取目标小时产能
        /// </summary>
        /// <param name="lineId">产线Id</param>
        /// <returns>目标小时产能</returns>
        private decimal GetPlanPerHourCapacity(double lineId)
        {
            decimal planPerHourCapacity = 0;
            var scheduleResource = RT.Service.Resolve<WipResourceController>().GetWipResource(lineId);
            if (scheduleResource == null || !scheduleResource.TaktTime.HasValue || scheduleResource.TaktTime.Value == 0)
                planPerHourCapacity = 0;
            else
                planPerHourCapacity = Convert.ToDecimal(3600 / scheduleResource.TaktTime.Value); //目标小时产能
            return planPerHourCapacity;
        }

        /* /// <summary>
        /// 根据资源(产线)Id获取实际产能
        /// </summary>
        /// <param name="lineId">产线Id</param>
        /// <param name="capaDate">日期</param>
        /// <returns>产能统计列表</returns>
        private EntityList<CapacityDistribution> GetCapaStats(double lineId, DateTime capaDate)
        {
            var capaStats = RT.Service.Resolve<CapacityDistributionController>().GetCapacityDistributions(lineId, capaDate);
            return capaStats;
        }*/

        /// <summary>
        /// 获取资源、班次、日期的生产采集信息
        /// </summary>
        /// <param name="resourceId">资源</param>
        /// <param name="shiftId">班次</param>
        /// <returns>生产采集集合</returns>
        private EntityList<WorkOrderStatistics> GetWorkOrderStatisticsList(double resourceId, double shiftId, DateTime nowTime)
        {
            ////var nowTime = RF.Find<WorkOrderStatistics>().GetDbTime();
            ////var capaStats = GetCapaStats(_resourceId, nowTime.Date);

            var curShift = RF.GetById<Shift>(shiftId);
            if (curShift == null)
                throw new EntityNotFoundException(typeof(Shift), shiftId);
            var shiftDate = RT.Service.Resolve<ShiftTypeController>().GetShiftDate(curShift, nowTime); ////班次日期
            var workOrderStatisticsList = RT.Service.Resolve<WipStatisticsController>().GetWorkOrderStatics(resourceId, shiftId, shiftDate);
            return workOrderStatisticsList;
        }

        /// <summary>
        /// 获取柱状图的Y轴计划数据集合
        /// </summary>
        /// <param name="capaStats">实际产能集合</param>
        /// <returns>Y轴计划数据集合</returns>
        private Dictionary<int, double> GetSeriesYDataPlan(EntityList<WorkOrderStatistics> capaStats, DateTime nowTime)
        {
            var dicYDataPlan = new Dictionary<int, double>();
            decimal planPerHourCapacity = GetPlanPerHourCapacity(_resourceId);
            foreach (var cata in capaStats)
            {
                if (cata.Hour <= nowTime.Hour)
                {
                    if (!dicYDataPlan.ContainsKey(cata.Hour))
                        dicYDataPlan.Add(cata.Hour, (double)planPerHourCapacity);
                    else
                        dicYDataPlan[cata.Hour] += (double)planPerHourCapacity;
                }
            }

            return dicYDataPlan;
        }

        /// <summary>
        /// 获取柱状图的Y轴实际数据集合
        /// </summary>
        /// <param name="capaStats">实际产能集合</param>
        /// <returns>Y轴实际数据集合</returns>
        private Dictionary<int, double> GetSeriesYDataActual(EntityList<WorkOrderStatistics> capaStats, DateTime nowTime)
        {
            var dicYDataActual = new Dictionary<int, double>();
            foreach (var cata in capaStats)
            {
                if (cata.Hour <= nowTime.Hour)
                {
                    if (!dicYDataActual.ContainsKey(cata.Hour))
                        dicYDataActual.Add(cata.Hour, (double)cata.QtyPass);
                    else
                        dicYDataActual[cata.Hour] += (double)cata.QtyPass;
                }
            }

            return dicYDataActual;
        }
        #endregion

        #region 异常停线状态图形数据初始化
        /// <summary>
        /// 设置异常停线数据源
        /// </summary>
        void InitLineData(DateTime nowTime)
        {
            if (planSeries.Points.Count == 0) return;
            lineStutDiagram.Series.Clear();
            lineChartDatas.Clear();
            const double argumnet = 0.1d;
            var series = new RangeBarOverlappedSeries2D
            {
                DisplayName = ExceptionStopType.Normal.ToLabel().L10N(),
                ArgumentScaleType = ScaleType.Numerical,
                BarWidth = 0.15, ////0.25,
                CrosshairLabelPattern = "{S}:{V1:F2}-{V2:F2}",
                LabelsVisibility = false,
                CrosshairLabelVisibility = false,
                ShowInLegend = false
            };
            series.SetResourceReference(RangeBarSeries2D.ModelProperty, "greenbarModel");
            series.AddPoint(argumnet, _minValueX - 0.5, _maxValueX + 0.5);
            lineStutDiagram.Series.Add(series);

            var abnormalCauses = RT.Service.Resolve<CapacityDistributionController>().GetAbnormalCauseList(_resourceId, _shiftId, ExceptionStopType.Normal, nowTime);
            lineChartDatas.AddRange(abnormalCauses);
            foreach (var p in lineChartDatas)
            {
                var tempSer = new RangeBarOverlappedSeries2D
                {
                    DisplayName = p.ExceptionStopType.ToLabel().L10N(),
                    ArgumentScaleType = ScaleType.Numerical,
                    BarWidth = 0.15,
                    CrosshairLabelPattern = "{S}:{V1:F2}-{V2:F2}",
                    LabelsVisibility = false,
                    ShowInLegend = false
                };
                switch (p.ExceptionStopType)
                {
                    case ExceptionStopType.Normal:
                        tempSer.CrosshairLabelVisibility = false;
                        tempSer.SetResourceReference(RangeBarSeries2D.ModelProperty, "greenbarModel");
                        break;
                    case ExceptionStopType.Maintain:
                        tempSer.Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCC00"));
                        tempSer.SetResourceReference(RangeBarSeries2D.ModelProperty, "yellowbarModel");
                        break;
                    case ExceptionStopType.StopLine:
                        tempSer.Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E33043"));
                        tempSer.SetResourceReference(RangeBarSeries2D.ModelProperty, "redbarModel");
                        break;
                }

                double startTime = TimeToDoubleConvert(p.BeginDate);
                double endTime = _maxValueX;
                if (p.EndDate != null)
                {
                    var actualEnd = TimeToDoubleConvert((DateTime)p.EndDate);
                    endTime = _maxValueX <= actualEnd ? _maxValueX : actualEnd;
                    tempSer.AddPoint(argumnet, startTime - 0.5, endTime - 0.5);
                    lineStutDiagram.Series.Add(tempSer);
                }
                else
                {
                    tempSer.AddPoint(argumnet, startTime - 0.5, endTime + 0.5);
                    lineStutDiagram.Series.Add(tempSer);
                    break;
                }
            }
        }

        /// <summary>
        /// 将DateTime转换成double类型
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>double时间</returns>
        double TimeToDoubleConvert(DateTime dateTime)
        {
            double time = dateTime.Hour;
            time += (double)dateTime.Minute / 60;
            time += (double)dateTime.Second / 3600;
            return time;
        }

        /// <summary>
        /// 时间double转换时间string
        /// </summary>
        /// <param name="timeNum">时间值</param>
        /// <returns>时间字符串</returns>
        string DoubleToTimeStringConvert(double timeNum)
        {
            ////整数部分小时
            var h = Math.Truncate(timeNum);
            ////小数部分转换分
            var mm = (timeNum - h) * 60;
            var m = Math.Truncate(mm);
            var disMin = m.ToString().Length == 1 ? "0" + m : m.ToString();

            var ss = (mm - m) * 60;
            var s = Math.Truncate(ss);
            var disSec = s.ToString().Length == 1 ? "0" + s : s.ToString();

            return "{0}:{1}:{2}".FormatArgs(h, disMin, disSec);
        }
        #endregion

        #region 设置柱子缩放程度
        /// <summary>
        /// 设置柱状图的宽度等属性
        /// </summary>
        private void SetChartDiagramProperty()
        {
            var count = actualSeries.Points.Count + 2;
            var width = cptyDiagram.AxisX.ActualWidth;
            var barWidth = (double)(25 * count) / width;
            planSeries.BarWidth = barWidth;
            actualSeries.BarWidth = barWidth;
            planSeriesTwo.BarWidth = barWidth;

            /*double group_width = 50;
            double barWidth = actualSeries.Points.Count * 1.0 / 10;
            barWidth = Math.Min(0.2, barWidth);
            barWidth = Math.Max(0.1, barWidth);
            actualSeries.BarWidth = barWidth;
            planSeries.BarWidth = barWidth;
            cptyDiagram.SetAxisXZoomRatio(0);
            if (actualSeries.Points.Count > 0 && cptyDiagram.ActualHeight > 0)
            {
                var unit_w = cptyDiagram.ActualWidth * 0.7 / actualSeries.Points.Count;
                if (unit_w < group_width)
                {
                    var p = (group_width - unit_w) / group_width;
                    cptyDiagram.SetAxisXZoomRatio(p);
                }
            }
            cptyDiagram.ScrollAxisXTo(0);*/

            planSeries.Animate();
            actualSeries.Animate();
        }
        #endregion

        /// <summary>
        /// 异常停线的鼠标悬停方法
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">执行参数</param>
        private void LineStutChart_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            foreach (var element in e.CrosshairElements)
            {
                if (element.Visible)
                {
                    var arg = element.Series.DisplayName;
                    var value1 = DoubleToTimeStringConvert(element.SeriesPoint.Value + 0.5);
                    var value2 = DoubleToTimeStringConvert(RangeBarSeries2D.GetValue2(element.SeriesPoint) + 0.5);

                    string description = string.Empty;
                    lineChartDatas.ForEach(p =>
                    {
                        var doubletime = TimeToDoubleConvert(p.BeginDate);
                        if (doubletime == (element.SeriesPoint.Value + 0.5))
                            description = p.AbnormalReason;
                    });
                    element.LabelElement.Text = "{0}:{1} - {2} {3}:{4}".FormatArgs(arg, value1, value2, "停线原因".L10N(), description);
                }
            }
        }
    }

    /// <summary>
    /// 当日产能属性
    /// </summary>
    public class CapacityDisbuControlProperty : ComponentProperty<CapacityDisbuControl>
    {
        /// <summary>
        /// 刷新时间（分钟）
        /// </summary>
        [DisplayName("刷新间隔(分钟)"), Description("默认刷新时间为3分钟"), Category("自定义")]
        public double TimeSpan { get; set; }
    }

    /// <summary>
    /// 当日产能分布圆角转换器
    /// </summary>
    internal class RadiusConverterCapacity : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// OneWay的转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = 7;
            if (value != null && System.Convert.ToDouble(value) > 5)
                val = System.Convert.ToDouble(value) / 2;

            return new CornerRadius(val, val, val, val);
        }

        /// <summary>
        /// TwoWay的转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
