using SIE.Domain;
using SIE.MES.DashBoard.DashBoards.ProductionLine;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.DashBoard.DashBoards.Editors;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace SIE.Wpf.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// DailyWorkOrderControl.xaml 的交互逻辑
    /// </summary>
    [Category("生产")]
    public partial class DailyWorkOrderControl : ComponentItem
    {
        #region 当日计划工单集合 WorkOrderInfos
        /// <summary>
        /// 当日计划工单集合
        /// </summary>
        private ObservableCollection<WorkOrderInfo> _workOrderInfos;

        /// <summary>
        /// 当日计划工单集合
        /// </summary>
        public ObservableCollection<WorkOrderInfo> WorkOrderInfos
        {
            get
            {
                if (_workOrderInfos == null)
                {
                    _workOrderInfos = new ObservableCollection<WorkOrderInfo>();
                }

                return _workOrderInfos;
            }
        }
        #endregion

        /// <summary>
        /// 数据刷新器
        /// </summary>
      readonly  DispatcherTimer _timer;

        /// <summary>
        /// 当前时间
        /// </summary>
        DateTime _currentTime = DateTime.Now;

        /// <summary>
        /// 数据区属性
        /// </summary>
        readonly DailyWorkOrderGridProperty _property;

        /// <summary>
        /// 当日计划工单构造函数
        /// </summary>
        public DailyWorkOrderControl()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _property = this.UseProperty<DailyWorkOrderGridProperty>();
        }

        /// <summary>
        /// 运行时事件
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            Dispatch();
            RefreshWorkOrderInfos();
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
                if (double.TryParse(lId, out lineId))
                {
                    //
                }
            }

            return lineId;
        }

        /// <summary>
        /// 刷新当日计划工单
        /// </summary>
        public void RefreshWorkOrderInfos()
        {
            WorkOrderInfos.Clear();
            double lineId = 0;
            lineId = GetLineId();
            if (lineId == 0)
            {
                return;
            }

            _currentTime = RF.Find<WorkOrder>().GetDbTime();
            DateTime currentDate = _currentTime.Date;
            var list = RT.Service.Resolve<ProductionLineController>().GetCurrentDayWorkOrderList(lineId, currentDate);
            if (list != null && list.Count > 0)
            {
                foreach (var wo in list)
                {
                    WorkOrderInfo entity = new WorkOrderInfo();
                    entity.No = wo.No;
                    entity.WorkOrderType = wo.Type.ToLabel();
                    entity.ProductCode = wo.Product?.Code;
                    entity.PlanEndDate = wo.PlanEndDate;
                    entity.PlanQty = wo.PlanQty;
                    entity.FinishQty = wo.FinishQty;
                    entity.State = wo.State;
                    entity.IsProducing = false;
                    if (entity.PlanQty != 0)
                    {
                        //完成率=工单累计完工数/工单总数
                        entity.CompletionRate = Math.Round(entity.FinishQty / entity.PlanQty, 4);
                    }

                    WorkOrderInfos.Add(entity);
                }

                //获取产线在制工单
                var wipResourceWorkOrder = RT.Service.Resolve<WipController>().GetWipResourceWorkOrder(lineId);
                var producingWo = wipResourceWorkOrder?.WorkOrder;
                if (producingWo != null)
                {
                    var wo = WorkOrderInfos.FirstOrDefault(p => p.No == producingWo.No);
                    if (wo != null)
                    {
                        wo.IsProducing = true;
                    }
                }
            }
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
            RefreshWorkOrderInfos();
        }
    }

    /// <summary>
    /// 数据表格属性值设置
    /// </summary>
    public class DailyWorkOrderGridProperty : ComponentProperty<DailyWorkOrderControl>
    {
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

    /// <summary>
    /// 当日计划工单，正在生产的工单行用绿色标识
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        /// <summary>
        /// 字符串转换为颜色
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>转换后的颜色</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (value == null)
            {
                return (Brush)converter.ConvertFromString("#333434".L10N());
            }

            bool isProducing = (bool)value;
            if (isProducing)
            {
                //正在生产的工单行用绿色标识
                return (Brush)converter.ConvertFromString("#008000".L10N());
            }
            else
            {
                return (Brush)converter.ConvertFromString("#333434".L10N());
            }
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>转换后的结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
