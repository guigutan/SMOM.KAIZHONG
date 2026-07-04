using SIE.Domain;
using SIE.MES.DashBoard.DashBoards.ProductionLine;
using SIE.MES.WorkOrders;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.DashBoard.DashBoards.Editors;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace SIE.Wpf.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// UnCloseWorkOrderControl.xaml 的交互逻辑
    /// </summary>
    [Category("生产")]
    public partial class UnCloseWorkOrderControl : ComponentItem
    {
        #region 未关闭工单集合 WorkOrderInfos
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
        DispatcherTimer _timer;

        /// <summary>
        /// 数据滚动器
        /// </summary>
        DispatcherTimer _scrollTimer;

        /// <summary>
        /// 当前时间
        /// </summary>
        DateTime _currentTime = RF.Find<WorkOrder>().GetDbTime();

        /// <summary>
        /// 数据区属性
        /// </summary>
        UnCloseWorkOrderControlProperty _property;

        /// <summary>
        /// 显示页
        /// </summary>                   
        int _position = 1;

        /// <summary>
        /// 滚动频率
        /// </summary>
        int _scrollInterval = 10;

        /// <summary>
        /// 未关闭工单构造函数
        /// </summary>
        public UnCloseWorkOrderControl()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _scrollTimer = new DispatcherTimer();
            _property = this.UseProperty<UnCloseWorkOrderControlProperty>();
        }

        /// <summary>
        /// 运行时事件
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            Load();
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
        /// 刷新未关闭工单
        /// </summary>
        public void RefreshWorkOrderInfos()
        {
            WorkOrderInfos.Clear();
            DateTime now = RF.Find<WorkOrder>().GetDbTime();
            double lineId = 0;
            lineId = GetLineId();
            if (lineId == 0)
            {
                return;
            }

            _currentTime = RF.Find<WorkOrder>().GetDbTime();
            DateTime currentDay = _currentTime.Date;
            var list = RT.Service.Resolve<ProductionLineController>().GetUnCloseWorkOrderList(lineId, currentDay);
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
                    if (entity.PlanQty != 0)
                    {
                        //完成率=工单累计完工数/工单总数
                        entity.CompletionRate = Math.Round(entity.FinishQty / entity.PlanQty, 4);
                    }

                    entity.DelayDay = Math.Round((now - wo.PlanEndDate).TotalDays, 2);
                    WorkOrderInfos.Add(entity);
                }

                //分页显示
                PagingDisplay();
            }
        }

        /// <summary>
        /// 分页显示
        /// </summary>
        public void PagingDisplay()
        {
            int scrollNum = (int)(this.ActualHeight / 48);
            if (scrollNum == 0)
            {
                return;
            }

            _scrollTimer.Tick += (o, e) =>
            {
                var rowCount = displayGrid.Items.Count;
                var pages = Math.Ceiling(rowCount / (decimal)scrollNum);

                if (rowCount <= 0 || _position >= pages)
                {
                    return;
                }
                if (_position == 0)
                {
                    displayGrid.ScrollIntoView(displayGrid.Items[0]);
                    _position = 1;
                }
                else
                {
                    ++_position;
                    if ((_position * scrollNum) < rowCount)
                    {
                        displayGrid.ScrollIntoView(displayGrid.Items[_position * scrollNum - 2]);
                    }
                    else if ((_position * scrollNum) >= rowCount)
                    {
                        displayGrid.ScrollIntoView(displayGrid.Items[rowCount - 1]);
                        _position = 0;
                    }
                    else
                    {
                        //
                    }
                }
            };
            _scrollTimer.Start();
        }

        /// <summary>
        /// 运行时异步加载
        /// </summary>
        void Load()
        {
            if (_property.ScrollInterval > 0)
            {
                _scrollInterval = _property.ScrollInterval;
            }
            else
            {
                _scrollInterval = 10;
            }

            _scrollTimer.Interval = TimeSpan.FromSeconds(_scrollInterval);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _timer.Tick += Timer_Tick;
                if (_property.Interval > 0)
                {
                    _timer.Interval = TimeSpan.FromSeconds(_property.Interval); //设置刷新的间隔时间
                }
                else
                {
                    _timer.Interval = TimeSpan.FromSeconds(300);
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
    public class UnCloseWorkOrderControlProperty : ComponentProperty<UnCloseWorkOrderControl>
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
        /// 滚动频率(秒)
        /// </summary>
        private int _scrollInterval = 30;

        /// <summary>
        /// 刷新频率
        /// </summary>
        [Category("数据过滤"), DisplayName("滚动频率(秒)"), Description("滚动频率(秒)")]
        public int ScrollInterval
        {
            get
            {
                return _scrollInterval;
            }

            set
            {
                if (value < 0)
                {
                    _scrollInterval = 30;
                }
                else
                {
                    _scrollInterval = value;
                }
            }
        }

        /// <summary>
        /// 车间过滤
        /// </summary>
        [Category("数据过滤"), DisplayName("车间产线"), Description("车间产线"), PropertyEditor(typeof(DashBoardShopToLineLookupEdit))]
        public ShopAndLine ShopAndLine { get; set; } = new ShopAndLine();
    }
}
