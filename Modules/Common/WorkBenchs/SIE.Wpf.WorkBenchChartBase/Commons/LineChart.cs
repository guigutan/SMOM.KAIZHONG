using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using SIE.WorkBenchChartBase.Commons;
using SIE.WorkBenchChartBase.ViewModels;
using SIE.WorkBenchCommon.SwitchWorkShop;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using SIE.WorkBenchCommon.Workbench.Tasks;
using SIE.Wpf.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 折线图
    /// </summary>
    //[ChartDefinitionAttribute("E:\\壁纸\\guanlan_gaoshou-007.jpg", "XYDiagram2DChart", LineChart._title)]
    public class LineChart : UserControl, IChart, IDisposable
    {
        /// <summary>
        /// 控件标题
        /// </summary>
        private const string _title = "折线图";


        /// <summary>
        /// 时间刷新器
        /// </summary>
        readonly DispatcherTimer _timer = new DispatcherTimer();

        /// <summary>
        /// 图表头控件
        /// </summary>
        protected ChartHeaderControl _headerControl;

        /// <summary>
        /// 图表控件
        /// </summary>
        protected ChartControl _control;

        /// <summary>
        /// 图表
        /// </summary>
        protected XYDiagram2D _diagram;

        /// <summary>
        /// 数据上下文Model
        /// </summary>
        protected BaseChartViewModel _baseModel;

        /// <summary>
        /// 最小刷新时间间隔（秒）
        /// </summary>
        readonly double MinDispatchTimeSpanMilSecs = 10;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LineChart()
        {
            BuildViewContent();
            NetworkChangeMonitor();
            _timer.Tick += new EventHandler(Timer_Tick);
            _headerControl.NewAction += NewCommandBinding_Executed;

            this.Loaded += LineChart_Loaded;

            _headerControl.DataContextChanged += (o, e) =>
            {
                if (_diagram.AxisY != null)
                {
                    _diagram.AxisY.ConstantLinesInFront.Clear();
                    _diagram.AxisY.ConstantLinesInFront.AddRange(GetConstantLines());
                }
            };
        }

        /// <summary>
        /// 网络监听
        /// </summary>
        private void NetworkChangeMonitor()
        {
            var log = SIE.Logging.LogManager.Logger;
            var networkChangeMonitor = RT.Service.Resolve<INetworkChangeMonitor>();
            networkChangeMonitor.NetworkAvailableEvent += (sender, e) =>
            {
                log.Debug($"module NetworkAvailableEvent");
                //定时器start
                _timer.Start();
            };
            networkChangeMonitor.NetworkNotAvailableEvent += (sender, e) =>
            {
                log.Debug($"module NetworkAvailableEvent");
                //定时器stop
                _timer.Stop();
            };
        }

        private void LineChart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_diagram.Series.Count == 0)
            {
                _diagram.AxisX = CreateAxisx();
                var secondarAxisXs = CreateSecondarAxisx();
                if (secondarAxisXs != null && secondarAxisXs.Count > 0)
                {
                    _diagram.SecondaryAxesX.AddRange(secondarAxisXs);
                }

                _diagram.AxisY = CreateAxisY();
                var secondarAxisYs = CreateSecondarAxisY();
                if (secondarAxisYs != null && secondarAxisYs.Count > 0)
                {
                    _diagram.SecondaryAxesY.AddRange(secondarAxisYs);
                }

                _control.MouseDoubleClick += Chart_MouseDoubleClick;
                _diagram.Series.AddRange(CreateSeries());
            }

            SubscribeWorkShopChanged();
        }

        /// <summary>
        /// 图表鼠标双击事件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">时间参数</param>
        protected virtual void Chart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// 构建数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        protected virtual BaseChartViewModel BuildDataContext()
        {
            return new TestChartViewModel { Title = _title.L10N(), ChartAlertLevel = ChartAlertLevel.Red };
        }

        /// <summary>
        /// 构建视图
        /// </summary>
        private void BuildViewContent()
        {
            const string defpath = "pack://application:,,,/SIE.Wpf.WorkBenchChartBase;component/ChartControls/XYDiagramControl.xaml";
            const string uri = defpath;
            var grid = XamlParseHelper.LoadEmbeddedXaml<Grid>(uri, UriKind.Absolute);
            this.Content = grid;

            _headerControl = grid.FindName("header") as ChartHeaderControl;
            Grid tempGrid = _headerControl.Content as Grid;
            _control = tempGrid.Children[0] as ChartControl;
            _control.Legend = CreateLegend();
            _diagram = _control.Diagram as XYDiagram2D;
        }

        /// <summary>
        /// 创建X轴
        /// </summary>
        /// <returns>X轴</returns>
        protected virtual AxisX2D CreateAxisx()
        {
            return null;
        }

        /// <summary>
        /// 创建第二X轴
        /// </summary>
        /// <returns>第二X轴</returns>
        protected virtual List<SecondaryAxisX2D> CreateSecondarAxisx()
        {
            return new List<SecondaryAxisX2D>();
        }

        /// <summary>
        /// 创建Y轴
        /// </summary>
        /// <returns>Y轴</returns>
        protected virtual AxisY2D CreateAxisY()
        {
            return null;
        }

        /// <summary>
        /// 创建第二Y轴
        /// </summary>
        /// <returns>第二Y轴</returns>
        protected virtual List<SecondaryAxisY2D> CreateSecondarAxisY()
        {
            return new List<SecondaryAxisY2D>();
        }

        /// <summary>
        /// 创建Series
        /// </summary>
        protected virtual List<Series> CreateSeries()
        {
            return new List<Series> { new LineSeries2D() };
        }

        /// <summary>
        /// 新建命令执行方法
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">命令参数</param>
        protected virtual void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var task = new TaskInfo();
            using (var stream = new MemoryStream())
            {
                _control.ExportToImage(stream);
                task.Pic = stream.ToArray();
            }

            WorkBenchCommon.Workbench.TaskManager.ShowTaskWindows(task, "新建任务");
        }

        /// <summary>
        /// 创建限制线
        /// </summary>
        /// <returns>限制线列表</returns>
        protected virtual List<ConstantLine> GetConstantLines()
        {
            return new List<ConstantLine>();
        }

        /// <summary>
        /// 定制Legend位置
        /// </summary>
        protected virtual Legend CreateLegend()
        {
            return new Legend();
        }

        /// <summary>
        /// 设置预警级别
        /// </summary>
        /// <param name="dayPoints">显示点</param>
        /// <param name="groupName">指标分类</param>
        /// <param name="title">指标名称</param>
        protected ChartAlertLevel GetChartAlertLevel(List<SeriesPoint> dayPoints, string groupName, string title)
        {
            ChartAlertLevel result = ChartAlertLevel.None;
            EntityList<TargetWarnDetail> targetWarnDetailList = RT.Service.Resolve<TargetWarnSettingController>().GetTargetWarnDetail(groupName, title);
            if (targetWarnDetailList != null && targetWarnDetailList.Count > 0)
            {
                var rate_avg = 0d;
                if (dayPoints != null && dayPoints.Count > 0)
                {
                    rate_avg = dayPoints.Average(p => Convert.ToDouble(p.Value));
                }

                foreach (TargetWarnDetail detail in targetWarnDetailList)
                {
                    if (detail.TargetOpetators == TargetOpetators.GreaterOrEqual && rate_avg >= (double)detail.MaxValue)
                    {
                        result = ChartAlertLevel.Green;
                        break;
                    }
                    else if (detail.TargetOpetators == TargetOpetators.Between && rate_avg > (double)detail.MinValue && rate_avg < (double)detail.MaxValue)
                    {
                        result = ChartAlertLevel.Yellow;
                        break;
                    }
                    else if (detail.TargetOpetators == TargetOpetators.LessOrEqual && rate_avg <= (double)detail.MinValue)
                    {
                        result = ChartAlertLevel.Red;
                        break;
                    }
                    else
                    {
                        //
                    }
                }
            }
            else
            {
                result = ChartAlertLevel.NoConfig;
            }

            return result;
        }

        /// <summary>
        /// 异步刷新时间
        /// </summary>
        /// <param name="sender">时间对象</param>
        /// <param name="e">时间参数</param>
        protected virtual void Timer_Tick(object sender, EventArgs e)
        {
            SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
            {
                _baseModel = BuildDataContext();
                _headerControl.DataContext = _baseModel;
            });
        }

        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="timeSpan">时间间隔</param>
        public void SetTimerInterval(double timeSpan)
        {
            _timer.Stop();
            _timer.Interval = TimeSpan.FromMinutes(timeSpan);
            if (_timer.Interval.TotalSeconds < MinDispatchTimeSpanMilSecs)
            {
                CRT.MessageService.ShowInstantMessage("刷新时间间隔少于{0}秒，建议在控制台设计中调整刷新时间间隔!".L10nFormat(MinDispatchTimeSpanMilSecs), "提示信息".L10N(), 3);
            }

            _timer.Start();
        }

        /// <summary>
        /// 订阅车间变更事件
        /// </summary>
        void SubscribeWorkShopChanged()
        {
            RT.EventBus.Subscribe<WorkShopChangedEvent>(this, SubscribeHandle);
            _workShop = RT.Service.Resolve<TargetWorkShopController>().GetTargetWorkShop(RT.IdentityId);
            _workShopId = _workShop?.Id;
            _dbTime = RF.Find<Enterprise>().GetDbTime();
            LoadData();
        }

        /// <summary>
        /// 释放事件
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Stop();
                RT.EventBus.Unsubscribe<WorkShopChangedEvent>(this);
                this.Loaded -= LineChart_Loaded;
            }
        }
        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData()
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _baseModel = BuildDataContext();
                _headerControl.DataContext = _baseModel;
            }));
        }

        #region 初始化车间、数据库时间
        /// <summary>
        /// 车间Id
        /// </summary>
        public double? _workShopId { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise _workShop { get; set; }

        /// <summary>
        /// 数据库时间
        /// </summary>
        public DateTime _dbTime { get; set; }

        /// <summary>
        /// 订阅事件处理
        /// </summary>
        /// <param name="e">e</param>
        private void SubscribeHandle(WorkShopChangedEvent e)
        {
            _workShopId = e.WorkShopId;
            if (_workShopId.HasValue)
            {
                _dbTime = RF.Find<Enterprise>().GetDbTime();
                _workShop = RF.GetById<Enterprise>(_workShopId);
                LoadData();
            }
        }


        #endregion
    }
}
