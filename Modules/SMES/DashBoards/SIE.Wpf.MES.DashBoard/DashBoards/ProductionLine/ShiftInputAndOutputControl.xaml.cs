using SIE.Domain;
using SIE.MES.DashBoard.DashBoards.ProductionLine;
using SIE.MES.Statistics.Entities;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.DashBoard.DashBoards.Editors;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace SIE.Wpf.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// ShiftInputAndOutputControl.xaml 的交互逻辑
    /// </summary>
    [Category("生产")]
    public partial class ShiftInputAndOutputControl : ComponentItem
    {
        #region 班次日统计
        /// <summary>
        /// 班次日统计
        /// </summary>
        private ObservableCollection<InputAndOutput> _shiftInputAndOutputs;

        /// <summary>
        /// 班次日统计
        /// </summary>
        public ObservableCollection<InputAndOutput> ShiftInputAndOutputs
        {
            get
            {
                if (_shiftInputAndOutputs == null)
                {
                    _shiftInputAndOutputs = new ObservableCollection<InputAndOutput>();
                }

                return _shiftInputAndOutputs;
            }
        }
        #endregion

        /// <summary>
        /// 产线时间段内产能采集统计
        /// </summary>
        private ObservableCollection<ResourceStatistics> _resourceStatisticsList;

        /// <summary>
        /// 产线时间段内产能采集统计
        /// </summary>
        public ObservableCollection<ResourceStatistics> ResourceStatisticsList
        {
            get
            {
                if (_resourceStatisticsList == null)
                {
                    _resourceStatisticsList = new ObservableCollection<ResourceStatistics>();
                }

                return _resourceStatisticsList;
            }
        }

        /// <summary>
        /// 达成率异常范围
        /// </summary>
        DataRange _cRateExceptionDataRange;

        /// <summary>
        /// 达成率预警范围
        /// </summary>
        DataRange _cRateAlertDataRange;

        /// <summary>
        /// 达成率异常背景色
        /// </summary>
        string _cRateExceptionColorStr;

        /// <summary>
        /// 达成率预警背景色
        /// </summary>
        string _cRateAlertColorStr;

        /// <summary>
        /// 直通率异常范围
        /// </summary>
        DataRange _rRateExceptionDataRange;

        /// <summary>
        /// 直通率预警范围
        /// </summary>
        DataRange _rRateAlertDataRange;

        /// <summary>
        /// 直通率异常背景色
        /// </summary>
        string _rRateExceptionColorStr;

        /// <summary>
        /// 直通率率预警背景色
        /// </summary>
        string _rRateAlertColorStr;

        /// <summary>
        /// 产线
        /// </summary>
        double _lineId;

        /// <summary>
        /// 当班班次
        /// </summary>
        Shift _shift;

        /// <summary>
        /// 当前时间
        /// </summary>
        DateTime _currentTime = DateTime.Now;

        /// <summary>
        /// 产能监控间隔(分钟)
        /// </summary>
        int _timeBetweenTimeSpan;

        /// <summary>
        /// 目标产能
        /// </summary>
        decimal _targetCapacity;

        /// <summary>
        /// 数据刷新器
        /// </summary>
        DispatcherTimer _timer;

        /// <summary>
        /// 数据区属性
        /// </summary>
        ShiftInputAndOutputControlProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftInputAndOutputControl()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _property = this.UseProperty<ShiftInputAndOutputControlProperty>();
        }

        /// <summary>
        /// 运行时事件
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            Dispatch();
            InitData();
            RefreshResourceStatisticsList();
        }

        /// <summary>
        /// 初始化看板属性配置
        /// </summary>
        public void InitData()
        {
            string defaultBackgroundStr = ProductionLineController.DefaultBackgroundStr;
            _cRateExceptionColorStr = defaultBackgroundStr;
            _cRateAlertColorStr = defaultBackgroundStr;
            _rRateExceptionColorStr = defaultBackgroundStr;
            _rRateAlertColorStr = defaultBackgroundStr;

            if (_property != null)
            {
                //达成率范围、背景色
                if (_property.CExceptionDataRange != null)
                {
                    _cRateExceptionDataRange = _property.CExceptionDataRange;
                    _cRateExceptionColorStr = _property.CExceptionBackColor.ToString(); //将Color对象转为string对象

                }

                if (_property.CAlerttionDataRange != null)
                {
                    _cRateAlertDataRange = _property.CAlerttionDataRange;
                    _cRateAlertColorStr = _property.CAlertBackColor.ToString();

                }

                //直通率范围、异常背景色、预警背景色
                if (_property.RExceptionDataRange != null)
                {
                    _rRateExceptionDataRange = _property.RExceptionDataRange;
                    _rRateExceptionColorStr = _property.RExceptionBackColor.ToString();
                }

                if (_property.RAlertDataRange != null)
                {
                    _rRateAlertDataRange = _property.RAlertDataRange;
                    _rRateAlertColorStr = _property.RAlertBackColor.ToString();
                }
            }
        }

        /// <summary>
        /// 获取达成率\直通率背景颜色
        /// </summary>
        /// <param name="rate">（达成率/直通率）率值</param>
        /// <param name="name">（达成率/直通率）名字</param>
        /// <returns>颜色字符串</returns>
        public string GetColorStringByDataRange(double rate, string name)
        {
            rate = rate * 100;
            string colorStr = ProductionLineController.DefaultBackgroundStr;
            if (_property == null)
            {
                return colorStr;
            }

            if (name == "达成率")
            {
                if (_cRateExceptionDataRange.LowerLimitValue != 0 && _cRateExceptionDataRange.UpperLimitValue != 0)
                {
                    if (rate >= _cRateExceptionDataRange.LowerLimitValue && rate <= _cRateExceptionDataRange.UpperLimitValue)
                    {
                        colorStr = _cRateExceptionColorStr;
                    }
                }

                if (_cRateAlertDataRange.LowerLimitValue != 0 && _cRateAlertDataRange.UpperLimitValue != 0)
                {
                    if (rate >= _cRateAlertDataRange.LowerLimitValue && rate <= _cRateAlertDataRange.UpperLimitValue)
                    {
                        colorStr = _cRateAlertColorStr;
                    }
                }
            }
            else if (name == "直通率")
            {
                if (_rRateExceptionDataRange.LowerLimitValue != 0 && _rRateExceptionDataRange.UpperLimitValue != 0)
                {
                    if (rate >= _rRateExceptionDataRange.LowerLimitValue && rate <= _rRateExceptionDataRange.UpperLimitValue)
                    {
                        colorStr = _rRateExceptionColorStr;
                    }
                }

                if (_rRateAlertDataRange.LowerLimitValue != 0 && _rRateAlertDataRange.UpperLimitValue != 0)
                {
                    if (rate >= _rRateAlertDataRange.LowerLimitValue && rate <= _rRateAlertDataRange.UpperLimitValue)
                    {
                        colorStr = _rRateAlertColorStr;
                    }
                }
            }
            else
            {
                return colorStr;
            }

            return colorStr;
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
        /// 数据校验
        /// </summary>
        /// <returns>校验结果</returns>
        public bool Validation()
        {
            if (_property == null
               || _property.ShopAndLine == null
               || _property.ShopAndLine.Lines.IsNullOrEmpty())
            {
                return false;
            }

            _targetCapacity = _property.TargetCapacity;
            _lineId = GetLineId();
            if (_lineId == 0)
            {
                return false;
            }

            _timeBetweenTimeSpan = _property.TimeBetweenTimeSpan;
            if (_timeBetweenTimeSpan <= 0)
            {
                return false; //未设置产能监控间隔
            }

            _currentTime = RF.Find<Shift>().GetDbTime();
            var shiftType = RT.Service.Resolve<WipResourceController>().GetWipResourceShiftType(_lineId, _currentTime);
            if (shiftType == null)
            {
                return false; //未找到班制
            }

            _shift = RT.Service.Resolve<ProductionLineController>().GetRealShiftByShiftTypeId(_currentTime, shiftType.Id);
            if (_shift == null)
            {
                return false; //未找到班次
            }

            return true;
        }

        /// <summary>
        ///  刷新班次日统计数据
        /// </summary>
        public void RefreshResourceStatisticsList()
        {
            ResourceStatisticsList.Clear();
            ShiftInputAndOutputs.Clear();
            if (!Validation())
            {
                return;
            }

            DateTime shiftBeginTime = _shift.BeginTime;
            DateTime shiftEndTime = _shift.EndTime;     //半小时产能计算需要时间,让当班最后一次结束时间落在班次的开始时间和结束时间中

            var resourceStatisticsList = RT.Service.Resolve<StatisticsController>().GetResourceStatisticsList(_lineId, shiftBeginTime, shiftEndTime);

            if (resourceStatisticsList == null || !resourceStatisticsList.Any())
            {
                //当班没有生产采集数据
                SetNoWipDatas();
                return;
            }

            while (shiftBeginTime <= shiftEndTime)
            {
                DateTime nextTime = shiftBeginTime.AddMinutes(_timeBetweenTimeSpan);
                if (nextTime > shiftEndTime)
                {
                    ////nextTime = shiftEndTime;
                    break;
                }

                decimal onlineQty = 0;    //投入
                decimal offlineQty = 0;   //产出
                decimal ngQty = 0;       //不良数

                var tempStatistics = resourceStatisticsList.Where(p => p.StartTime >= shiftBeginTime
                                                                && p.StartTime <= nextTime);
                if (tempStatistics != null && tempStatistics.Any())
                {
                    onlineQty = tempStatistics.Sum(p => p.OnlineQty);
                    offlineQty = tempStatistics.Where(p => p.OfflineQty.HasValue).Sum(p => p.OfflineQty.Value);
                    ngQty = tempStatistics.Where(p => p.NgQty.HasValue).Sum(p => p.NgQty.Value);
                }

                InputAndOutput model = new InputAndOutput();
                model.TimeBetween = "{0}:{1}-{2}:{3}".FormatArgs(shiftBeginTime.Hour.ToString().PadLeft(2, '0'), shiftBeginTime.Minute.ToString().PadLeft(2, '0'), nextTime.Hour.ToString().PadLeft(2, '0'), nextTime.Minute.ToString().PadLeft(2, '0'));
                model.PlanQty = _targetCapacity;
                model.OnlineQty = onlineQty;
                model.OfflineQty = offlineQty;
                model.NgQty = ngQty;
                model.CompletionRate = 0;
                if (model.OfflineQty.HasValue && model.PlanQty != 0)
                {
                    model.CompletionRate = (double)Math.Round(model.OfflineQty.Value / model.PlanQty, 4);
                }

                model.RolledYield = 0;
                if (model.OnlineQty.HasValue && model.OnlineQty.Value != 0)
                {
                    model.RolledYield = (double)Math.Round((model.OnlineQty.Value - ngQty) / model.OnlineQty.Value, 4);
                }

                model.CRateBackgroundStr = GetColorStringByDataRange(model.CompletionRate, "达成率");
                model.RolledYieldBackgroundStr = GetColorStringByDataRange(model.RolledYield, "直通率");
                ShiftInputAndOutputs.Add(model);

                shiftBeginTime = shiftBeginTime.AddMinutes(_timeBetweenTimeSpan);
            }

            //合计行   
            var loadQtyList = ShiftInputAndOutputs.Where(p => p.OnlineQty.HasValue).ToList();
            var downQtyList = ShiftInputAndOutputs.Where(p => p.OfflineQty.HasValue).ToList();

            decimal totalPlanQty = 0;
            decimal totalLoadQty = 0;
            decimal totalDownQty = 0;
            decimal totalNgQty = 0;

            if (ShiftInputAndOutputs.Count > 0)
            {
                totalPlanQty = ShiftInputAndOutputs.Sum(p => p.PlanQty);
                totalNgQty = ShiftInputAndOutputs.Sum(p => p.NgQty);
            }

            if (loadQtyList.Count > 0)
            {
                totalLoadQty = loadQtyList.Sum(p => p.OnlineQty.Value);
            }

            if (downQtyList.Count > 0)
            {
                totalDownQty = downQtyList.Sum(p => p.OfflineQty.Value);
            }

            InputAndOutput entity = new InputAndOutput();
            entity.TimeBetween = "合计";
            entity.PlanQty = totalPlanQty;
            entity.OnlineQty = totalLoadQty;
            entity.OfflineQty = totalDownQty;
            entity.NgQty = totalNgQty;
            if (totalPlanQty != 0)
            {
                entity.CompletionRate = (double)Math.Round(totalDownQty / totalPlanQty, 4);
            }

            if (totalLoadQty != 0)
            {
                entity.RolledYield = (double)Math.Round((totalLoadQty - entity.NgQty) / totalLoadQty, 4);
            }

            entity.CRateBackgroundStr = GetColorStringByDataRange(entity.CompletionRate, "达成率");
            entity.RolledYieldBackgroundStr = GetColorStringByDataRange(entity.RolledYield, "直通率");
            ShiftInputAndOutputs.Add(entity);
        }

        /// <summary>
        /// (设置)没生成采集记录时
        /// </summary>
        public void SetNoWipDatas()
        {
            DateTime shiftBeginTime = _shift.BeginTime;
            DateTime shiftEndTime = _shift.EndTime;

            while (shiftBeginTime <= shiftEndTime)
            {
                DateTime nextTime = shiftBeginTime.AddMinutes(_timeBetweenTimeSpan);
                if (nextTime > shiftEndTime)
                {
                    nextTime = shiftEndTime;
                }

                InputAndOutput model = new InputAndOutput();
                model.TimeBetween = "{0}:{1}-{2}:{3}".FormatArgs(shiftBeginTime.Hour.ToString().PadLeft(2, '0'), shiftBeginTime.Minute.ToString().PadLeft(2, '0'), nextTime.Hour.ToString().PadLeft(2, '0'), nextTime.Minute.ToString().PadLeft(2, '0'));
                model.PlanQty = _targetCapacity;
                model.OnlineQty = null;
                model.OfflineQty = null;
                model.CompletionRate = 0;
                model.RolledYield = 0;
                model.CRateBackgroundStr = ProductionLineController.DefaultBackgroundStr;
                model.RolledYieldBackgroundStr = ProductionLineController.DefaultBackgroundStr;
                ShiftInputAndOutputs.Add(model);
                shiftBeginTime = shiftBeginTime.AddMinutes(_timeBetweenTimeSpan);
            }

            InputAndOutput entity = new InputAndOutput();
            entity.TimeBetween = "合计";
            entity.PlanQty = _targetCapacity;
            entity.OnlineQty = null;
            entity.OfflineQty = null;
            entity.CompletionRate = 0;
            entity.RolledYield = 0;
            entity.CRateBackgroundStr = ProductionLineController.DefaultBackgroundStr;
            entity.RolledYieldBackgroundStr = ProductionLineController.DefaultBackgroundStr;
            ShiftInputAndOutputs.Add(entity);
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
            RefreshResourceStatisticsList();
        }
    }

    /// <summary>
    /// 数据表格属性值设置
    /// </summary>
    public class ShiftInputAndOutputControlProperty : ComponentProperty<ShiftInputAndOutputControl>
    {
        #region 直通率/达成率背景色配置
        /// <summary>
        /// 达成率异常背景色
        /// </summary>
        Color _cExceptionBackColor;

        /// <summary>
        /// 达成率异常背景色
        /// </summary>
        [Category("达成率颜色配置"), DisplayName("异常背景色"), Description("异常背景色")]
        public Color CExceptionBackColor
        {
            get
            {
                return _cExceptionBackColor;
            }

            set
            {
                _cExceptionBackColor = value;
            }
        }

        /// <summary>
        /// 达成率异常范围对象
        /// </summary>
        DataRange _cExceptionDataRange = new DataRange();

        /// <summary>
        /// 达成率异常范围对象
        /// </summary>
        [Category("达成率颜色配置"), DisplayName("异常范围"), Description("异常范围"), PropertyEditor(typeof(DataRangeEdit))]
        public DataRange CExceptionDataRange
        {
            get
            {
                return _cExceptionDataRange;
            }

            set
            {
                _cExceptionDataRange = value;
            }
        }

        /// <summary>
        /// 达成率预警背景色
        /// </summary>
        Color _cAlertBackColor;

        /// <summary>
        /// 达成率异常背景色
        /// </summary>
        [Category("达成率颜色配置"), DisplayName("预警背景色"), Description("预警背景色")]
        public Color CAlertBackColor
        {
            get
            {
                return _cAlertBackColor;
            }

            set
            {
                _cAlertBackColor = value;
            }
        }

        /// <summary>
        /// 达成率预警范围对象
        /// </summary>
        DataRange _cAlertDataRange = new DataRange();

        /// <summary>
        /// 达成率异常范围对象
        /// </summary>
        [Category("达成率颜色配置"), DisplayName("预警范围"), Description("预警范围"), PropertyEditor(typeof(DataRangeEdit))]
        public DataRange CAlerttionDataRange
        {
            get
            {
                return _cAlertDataRange;
            }

            set
            {
                _cAlertDataRange = value;
            }
        }

        /// <summary>
        /// 直通率异常背景色
        /// </summary>
        Color _rExceptionBackColor;

        /// <summary>
        /// 直通率异常背景色
        /// </summary>
        [Category("直通率颜色配置"), DisplayName("异常背景色"), Description("异常背景色")]
        public Color RExceptionBackColor
        {
            get
            {
                return _rExceptionBackColor;
            }

            set
            {
                _rExceptionBackColor = value;
            }
        }

        /// <summary>
        /// 直通率异常范围对象
        /// </summary>
        DataRange _rExceptionDataRange = new DataRange();

        /// <summary>
        /// 直通率异常范围对象
        /// </summary>
        [Category("直通率颜色配置"), DisplayName("异常范围"), Description("异常范围"), PropertyEditor(typeof(DataRangeEdit))]
        public DataRange RExceptionDataRange
        {
            get
            {
                return _rExceptionDataRange;
            }

            set
            {
                _rExceptionDataRange = value;
            }
        }

        /// <summary>
        /// 直通率预警背景色
        /// </summary>
        Color _rAlertBackColor;

        /// <summary>
        /// 直通率预警背景色
        /// </summary>
        [Category("直通率颜色配置"), DisplayName("预警背景色"), Description("预警背景色")]
        public Color RAlertBackColor
        {
            get
            {
                return _rAlertBackColor;
            }

            set
            {
                _rAlertBackColor = value;
            }
        }

        /// <summary>
        /// 直通率预警范围对象
        /// </summary>
        DataRange _rAlertDataRange = new DataRange();

        /// <summary>
        /// 直通率预警范围对象
        /// </summary>
        [Category("直通率颜色配置"), DisplayName("预警范围"), Description("预警范围"), PropertyEditor(typeof(DataRangeEdit))]
        public DataRange RAlertDataRange
        {
            get
            {
                return _rAlertDataRange;
            }

            set
            {
                _rAlertDataRange = value;
            }
        }
        #endregion

        /// <summary>
        /// 背景色
        /// </summary>
        Color _backColor;

        /// <summary>
        /// 前景色
        /// </summary>
        Color _forceColor;

        /// <summary>
        /// 表格背景颜色
        /// </summary>
        [Category("表格设置"), DisplayName("背景色设置"), Description("背景色设置")]
        public Color BackColor
        {
            get
            {
                return _backColor;
            }

            set
            {
                _backColor = value;
                Item.displayGrid.Background = new SolidColorBrush(_backColor);
            }
        }

        /// <summary>
        /// 表格前景色
        /// </summary>
        [Category("表格设置"), DisplayName("前景色设置"), Description("前景色设置")]
        public Color ForceColor
        {
            get
            {
                return _forceColor;
            }

            set
            {
                _forceColor = value;
                Item.displayGrid.Foreground = new SolidColorBrush(_forceColor);
            }
        }

        /// <summary>
        /// 字体类型
        /// </summary>
        [Category("表格设置"), DisplayName("字体类型"), Description("字体类型")]
        public FontFamily FontFamily
        {
            get { return Item.displayGrid.FontFamily; }
            set { Item.displayGrid.FontFamily = value; }
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        [Category("表格设置"), DisplayName("字体大小"), Description("字体大小")]
        public double FontSize
        {
            get { return Item.displayGrid.FontSize; }
            set { Item.displayGrid.FontSize = value; }
        }

        /// <summary>
        /// 是否加粗
        /// </summary>
        [Category("表格设置"), DisplayName("是否加粗"), Description("是否加粗")]
        public FontWeight FontWeight
        {
            get { return Item.displayGrid.FontWeight; }
            set { Item.displayGrid.FontWeight = value; }
        }

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
        /// 产能监控间隔(分钟)
        /// </summary>
        private int _timeBetweenTimeSpan = 60;

        /// <summary>
        /// 刷新频率
        /// </summary>
        [Category("数据过滤"), DisplayName("产能监控间隔(分钟)"), Description("产能监控间隔(分钟)")]
        public int TimeBetweenTimeSpan
        {
            get
            {
                return _timeBetweenTimeSpan;
            }

            set
            {
                if (value < 60)
                {
                    _timeBetweenTimeSpan = 60;
                }
                else
                {
                    _timeBetweenTimeSpan = value;
                }
            }
        }

        /// <summary>
        /// 目标产能
        /// </summary>
        private decimal _targetCapacity;

        /// <summary>
        /// 目标产能
        /// </summary>
        [Category("数据过滤"), DisplayName("目标产能"), Description("目标产能")]
        public decimal TargetCapacity
        {
            get
            {
                return _targetCapacity;
            }

            set
            {
                if (value < 0)
                {
                    _targetCapacity = 0;
                }
                else
                {
                    _targetCapacity = value;
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
