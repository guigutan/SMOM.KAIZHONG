using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.MES.Workbench;
using SIE.MES.Workbench.AlertLights;
using SIE.MES.Workbench.AndonAbnormals;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.ShiftProductions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SIE.Wpf.MES.Workbench.AndonAbnormals
{
    /// <summary>
    /// 安灯异常
    /// </summary>
    [Category("过程分析")]
    public partial class AndonAbnormalControl : ComponentItem, IfaceKeyEvent
    {
        /// <summary>
        /// 今日异常分布
        /// </summary>
        private const int TodayExcepPublishAxisYOffset = 1;

        /// <summary>
        /// 异常工时
        /// </summary>
        private const int TodayExcepWorkHoursAxisYOffset = 1;

        /// <summary>
        /// 车间Id--从车间组件获取输入值
        /// </summary>
        private double? _workShopId = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AndonAbnormalControl()
        {
            InitializeComponent();
            AndonAbnormalControl owner = this;
            RT.EventBus.Subscribe<WorkShopChangedEvent>(owner, AndonAbnormalSubscribeHandle);
        }

        /// <summary>
        /// 车间变更事件处理函数
        /// </summary>
        /// <param name="obj">车间变更事件</param>
        private void AndonAbnormalSubscribeHandle(WorkShopChangedEvent obj)
        {
            _workShopId = obj.WorkShopId;
            var workShop = RF.GetById<Enterprise>(_workShopId);
            if (workShop == null)
                return;
            ComponentControlsIni();
        }

        /// <summary>
        /// 关闭后处理方法
        /// </summary>
        protected override void OnClose()
        {
            RT.EventBus.Unsubscribe<WorkShopChangedEvent>(this);
            base.OnClose();
        }

        /// <summary>
        /// 数据刷新方法
        /// </summary>
        public override void Refresh()
        {
            ComponentControlsIni();
        }

        /// <summary>
        /// WPF组件初始化
        /// </summary>
        private void ComponentControlsIni()
        {
            EntityList<AndonAbnormal> andonCtls = GetAndonAbnormals();
            IniGridControlAndonListValue(andonCtls);
            InitTodayExcepPublishSerie(andonCtls);
            InitTodayExcepWorkHoursSerie(andonCtls);
        }

        /// <summary>
        /// 获取指定日期、指定车间下所有产线的安灯异常信息
        /// </summary>
        /// <returns>安灯异常集合</returns>
        private EntityList<AndonAbnormal> GetAndonAbnormals()
        {
            var date = RF.Find<AndonAbnormal>().GetDbTime();
            var processStatusTypes = new List<ProcessStatusType>() { ProcessStatusType.Waitting, ProcessStatusType.Processing, ProcessStatusType.Closed };
            EntityList<AndonAbnormal> andonCtls = RT.Service.Resolve<AlertLightsController>().GetAndonAbnormals(date.Date, processStatusTypes, _workShopId);
            return andonCtls;
        }

        /// <summary>
        /// 获取安灯异常的列表数据
        /// </summary>
        /// <param name="andonCtls">安灯异常集合</param>
        private void IniGridControlAndonListValue(EntityList<AndonAbnormal> andonCtls)
        {
            var andonAbnorViewDatas = new ObservableCollection<AndonAbnormalViewData>();
            if (andonCtls != null)
            {
                foreach (var curEntity in andonCtls)
                {
                    AndonAbnormalViewData curViewData = new AndonAbnormalViewData();
                    curViewData.ProductLineId = curEntity.ProductLineId;
                    curViewData.ProductLineCode = curEntity.ProductLine.Code;
                    curViewData.ProductLineName = curEntity.ProductLine.Name;
                    curViewData.ExceptionType = curEntity.ExceptionType.Name;
                    curViewData.CallTime = curEntity.TriggerTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    curViewData.ProcesserId = curEntity.ProcessEmployeeId == null ? double.NaN : (curEntity.ProcessEmployeeId.Value);
                    curViewData.ProcesserCode = curEntity.ProcessEmployee?.Code ?? (string.Empty);
                    curViewData.ProcesserName = curEntity.ProcessEmployee?.Name ?? (string.Empty);
                    curViewData.SignTime = curEntity.SignTime == null ? string.Empty : curEntity.SignTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    curViewData.ProcessStatus = curEntity.ProcessStatus == null ? string.Empty : curEntity.ProcessStatus.Value.ToLabel();

                    andonAbnorViewDatas.Add(curViewData);
                }
            }

            gcAndonList.ItemsSource = andonAbnorViewDatas;
        }

        /// <summary>
        /// 今日异常公布柱状图初始化
        /// </summary>
        /// <param name="andonCtls">安灯异常集合</param>
        private void InitTodayExcepPublishSerie(EntityList<AndonAbnormal> andonCtls)
        {
            List<double> listSeriesYs = new List<double>();
            todayExcepPublishSerie.Points.Clear();
            if (andonCtls != null)
            {
                var prdLineIds = andonCtls.Select(p => p.ProductLineId).Distinct();
                foreach (var prdId in prdLineIds)
                {
                    var curAndonCtls = andonCtls.Where(p => p.ProductLineId == prdId).ToList();
                    var curSum = curAndonCtls.Count;
                    var curLineName = curAndonCtls.FirstOrDefault()?.ProductLine.Name;
                    todayExcepPublishSerie.AddPoint(curLineName, curSum);
                    listSeriesYs.Add(curSum);
                }

                SetSeriesYMinMax(listSeriesYs, todayExcepPublishAxisY, TodayExcepPublishAxisYOffset);
                SetChartDiagramProperty(todayExcepPublishGram, todayExcepPublishSerie);
            }
        }

        /// <summary>
        /// 异常工时柱状图初始化
        /// </summary>
        /// <param name="andonCtls">安灯异常集合</param>
        private void InitTodayExcepWorkHoursSerie(EntityList<AndonAbnormal> andonCtls)
        {
            List<double> listSeriesYs = new List<double>();
            todayExcepWorkHoursSerie.Points.Clear();
            if (andonCtls != null)
            {
                var date = RF.Find<AndonAbnormal>().GetDbTime();
                var prdLineIds = andonCtls.Select(p => p.ProductLineId).Distinct();
                foreach (var prdId in prdLineIds)
                {
                    var curAndonCtls = andonCtls.Where(p => p.ProductLineId == prdId).ToList();
                    var curLineName = andonCtls.FirstOrDefault(p => p.ProductLineId == prdId)?.ProductLine.Name;
                    var excepWorkHours = 0.0;
                    foreach (var item in curAndonCtls)
                    {
                        if (item.ProcessStatus == ProcessStatusType.Closed)
                        {
                            var curSecond = ((DateTime)item.RestoreTime - (DateTime)item.TriggerTime).TotalMinutes;
                            excepWorkHours += curSecond;
                        }
                        else
                        {
                            var curSecond = (date - (DateTime)item.TriggerTime).TotalMinutes;
                            excepWorkHours += curSecond;
                        }
                    }

                    excepWorkHours = Math.Round(excepWorkHours, 1);
                    todayExcepWorkHoursSerie.AddPoint(curLineName, excepWorkHours);
                    listSeriesYs.Add(excepWorkHours);
                }

                SetSeriesYMinMax(listSeriesYs, todayExcepWorkHoursAxisY, TodayExcepWorkHoursAxisYOffset);
                SetChartDiagramProperty(todayExcepWorkHoursGram, todayExcepWorkHoursSerie);
            }
        }

        /// <summary>
        /// 设置柱状图的宽度等属性
        /// </summary>
        /// <param name="diagram2D">柱状图对象</param>
        /// <param name="series2D">柱状图点集合</param>
        private void SetChartDiagramProperty(XYDiagram2D diagram2D, BarSideBySideSeries2D series2D)
        {
            const double group_width = 80;
            double barWidth = series2D.Points.Count * 1.0 / 10;
            barWidth = Math.Min(0.2, barWidth);
            barWidth = Math.Max(0.1, barWidth);
            series2D.BarWidth = barWidth; //柱形图宽度
            diagram2D.SetAxisXZoomRatio(0);
            if (series2D.Points.Count > 0 && diagram2D.ActualHeight > 0)
            {
                var unit_w = diagram2D.ActualWidth * 0.7 / series2D.Points.Count;
                if (unit_w < group_width)
                {
                    var p = (group_width - unit_w) / group_width;
                    diagram2D.SetAxisXZoomRatio(p);
                }
            }

            diagram2D.ScrollAxisXTo(0);
            series2D.Animate();
        }

        /// <summary>
        /// 设置柱状图的Y轴的最小、最大值的offset
        /// </summary>
        /// <param name="listSeriesYs">Y轴数据集合</param>
        /// <param name="axisY">柱状图Y轴</param>
        /// <param name="offset">Y轴偏移量</param>
        private void SetSeriesYMinMax(List<double> listSeriesYs, AxisY2D axisY, int offset)
        {
            listSeriesYs.Sort();
            ////axisY.VisualRange.MinValue = listSeriesYs.FirstOrDefault() - offset;
            ////axisY.WholeRange.MinValue = axisY.VisualRange.MinValue;
            listSeriesYs.Reverse();
            axisY.VisualRange.MaxValue = listSeriesYs.FirstOrDefault() + offset;
            axisY.WholeRange.MaxValue = axisY.VisualRange.MaxValue;
        }
    }

    /// <summary>
    /// 安灯异常属性
    /// </summary>
    public class AndonAbnormalControlProperty : ComponentProperty<AndonAbnormalControl>
    {
    }
}