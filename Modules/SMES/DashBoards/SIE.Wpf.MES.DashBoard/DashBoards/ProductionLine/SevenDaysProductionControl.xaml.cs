using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.MES.DashBoard.DashBoards.ProductionLine;
using SIE.MES.WorkOrders;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.DashBoard.DashBoards.Editors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace SIE.Wpf.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// SevenDaysProductionControl.xaml 的交互逻辑
    /// </summary>
    [Category("生产")]
    public partial class SevenDaysProductionControl : ComponentItem
    {
        #region 当日投入产出集合 InputAndOutputs
        /// <summary>
        /// 当日投入产出集合
        /// </summary>
        private ObservableCollection<InputAndOutput> _inputAndOutputs;

        /// <summary>
        /// 当日投入产出集合
        /// </summary>
        public ObservableCollection<InputAndOutput> InputAndOutputs
        {
            get
            {
                if (_inputAndOutputs == null)
                {
                    _inputAndOutputs = new ObservableCollection<InputAndOutput>();
                }

                return _inputAndOutputs;
            }
        }
        #endregion

        /// <summary>
        /// 投入柱形图
        /// </summary>
        private List<SeriesPoint> _lsBarPoint = new List<SeriesPoint>();

        /// <summary>
        /// 产出柱形图
        /// </summary>
        private List<SeriesPoint> _lsBar2Point = new List<SeriesPoint>();

        /// <summary>
        /// 产线id
        /// </summary>
        double _lineId;

        /// <summary>
        /// 数据刷新器
        /// </summary>
        DispatcherTimer _timer;

        /// <summary>
        /// 当前时间
        /// </summary>
        DateTime _currentTime = DateTime.Now;

        /// <summary>
        /// 数据区属性
        /// </summary>
        SevenDaysProductionControlProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SevenDaysProductionControl()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _property = this.UseProperty<SevenDaysProductionControlProperty>();
        }

        /// <summary>
        /// 运行时事件
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            InitSeries();
            BindingChart();
            Dispatch();
        }

        /// <summary>
        /// 初始化图形
        /// </summary>
        private void InitSeries()
        {
            ////BarSideSerie.BarWidth = 0.1; //柱形图宽度
            ////BarSideSerie2.BarWidth = 0.1;
            BarSideSerie.Animate(); //动画
            BarSideSerie2.Animate();
            chartControl.UpdateData();
        }

        /// <summary>
        /// 获取产线id
        /// </summary>
        /// <returns>产线id</returns>
        public double GetLineId()
        {
            double lineId = 0;
            if (_property == null
               || _property.ShopAndLine == null
               || _property.ShopAndLine.Lines.IsNullOrEmpty())
            {
                return lineId;
            }

            var lineIds = this._property.ShopAndLine.Lines?.Split(';');
            if (lineIds != null)
            {
                string lId = lineIds[0];
                double.TryParse(lId, out lineId);
            }

            return lineId;
        }

        /// <summary>
        /// 运行时异步加载
        /// </summary>
        void Dispatch()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _timer.Tick += Timer_Tick;
                if (_property.Interval > 0)
                {
                    _timer.Interval = TimeSpan.FromSeconds(_property.Interval); //设置刷新的间隔时间
                }
                else
                {
                    _timer.Interval = TimeSpan.FromSeconds(60);
                }

                _timer.Start();
            }));
        }

        /// <summary>
        /// 设置时间间隔器动作
        /// </summary>
        /// <param name="sender">源对象</param>
        /// <param name="e">源参数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            BindingChart();
        }

        /// <summary>
        /// 绑定图表
        /// </summary>
        public void BindingChart()
        {
            GetData();
            BarSideSerie.Points.Clear();
            BarSideSerie.Points.AddRange(_lsBarPoint);
            BarSideSerie.ArgumentScaleType = ScaleType.Auto;
            BarSideSerie.ValueScaleType = ScaleType.Numerical;
            BarSideSerie2.Points.Clear();
            BarSideSerie2.Points.AddRange(_lsBar2Point);
            BarSideSerie2.ArgumentScaleType = ScaleType.Auto;
            BarSideSerie.Animate();
            BarSideSerie2.Animate();
            chartControl.UpdateData();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public void GetData()
        {
            InputAndOutputs.Clear();
            _lineId = GetLineId();
            if (_lineId == 0)
            {
                return;
            }

            _currentTime = RF.Find<WorkOrder>().GetDbTime();
            DateTime currentDay = _currentTime.Date;

            //包含今天一共最近7天，所以是减去6
            DateTime beforeSevenDay = currentDay.AddDays(-6);
            var resourceStatisticsList = RT.Service.Resolve<ProductionLineController>().GetResourceStatisticsListByShiftDate(_lineId, beforeSevenDay);
            if (resourceStatisticsList != null && resourceStatisticsList.Any())
            {
                DateTime beginDate = beforeSevenDay;
                while (beginDate <= currentDay)
                {
                    decimal onlineQty = 0;  //当日投入
                    decimal offlineQty = 0;
                    onlineQty = resourceStatisticsList.Where(p => p.CollectDate.Date == beginDate.Date).Sum(p => p.OnlineQty);
                    offlineQty = resourceStatisticsList.Where(p => p.CollectDate.Date == beginDate.Date && p.OfflineQty.HasValue).Sum(p => p.OfflineQty.Value);

                    InputAndOutput model = new InputAndOutput();
                    model.CollectDate = beginDate;
                    model.OnlineQty = onlineQty;
                    model.OfflineQty = offlineQty;
                    InputAndOutputs.Add(model);
                    beginDate = beginDate.AddDays(1);
                }

                if (InputAndOutputs.Any())
                {
                    _lsBarPoint.Clear();
                    _lsBar2Point.Clear();
                    foreach (var item in InputAndOutputs)
                    {
                        _lsBarPoint.Add(new SeriesPoint() { Argument = "{0}月{1}日_".FormatArgs(item.CollectDate.Month, item.CollectDate.Day), Value = (double)item.OnlineQty.Value });
                        _lsBar2Point.Add(new SeriesPoint() { Argument = "{0}月{1}日_".FormatArgs(item.CollectDate.Month, item.CollectDate.Day), Value = (double)item.OfflineQty.Value });
                    }
                }
                else
                {
                    _lsBarPoint.Clear();
                    _lsBar2Point.Clear();
                }
            }
        }
    }

    /// <summary>
    /// 看板属性配置类
    /// </summary>
    public class SevenDaysProductionControlProperty : ComponentProperty<SevenDaysProductionControl>
    {
        /// <summary>
        /// 刷新频率(秒)
        /// </summary>
        private int _interval = 300;

        /// <summary>
        /// 刷新频率
        /// </summary>
        [Category("数据过滤"), DisplayName("刷新频率(秒)"), Description("刷新频率(秒)")]
        public int Interval
        {
            get
            {
                return _interval;
            }

            set
            {
                if (value < 0)
                {
                    _interval = 300;
                }
                else
                {
                    _interval = value;
                }
            }
        }

        /// <summary>
        /// 车间产线联动对象
        /// </summary>
        ShopAndLine _shopAndLine = new ShopAndLine();

        /// <summary>
        /// 车间过滤
        /// </summary>
        [Category("数据过滤"), DisplayName("车间产线"), Description("车间产线"), PropertyEditor(typeof(DashBoardShopToLineLookupEdit))]
        public ShopAndLine ShopAndLine
        {
            get
            {
                return _shopAndLine;
            }

            set
            {
                _shopAndLine = value;
            }
        }
    }
}
