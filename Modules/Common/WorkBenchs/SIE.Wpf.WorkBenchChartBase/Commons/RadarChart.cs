using DevExpress.Xpf.Charts;
using SIE.WorkBenchChartBase.ViewModels;
using SIE.WorkBenchCommon.Workbench.Tasks;
using SIE.Wpf.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 雷达图
    /// </summary>
    //[ChartDefinitionAttribute("E:\\壁纸\\guanlan_gaoshou-007.jpg", "RadarDiagram2DChart", RadarChart._title)]
    public class RadarChart : UserControl, IDisposable
    {
        private const string _title = "雷达图";

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
        protected RadarDiagram2D _diagram;

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
        public RadarChart()
        {
            BuildViewContent();
            _baseModel = new TestChartViewModel { Title = _title.L10N(), ChartAlertLevel = SIE.WorkBenchChartBase.Commons.ChartAlertLevel.Green };
            NetworkChangeMonitor();
            _timer.Tick += new EventHandler(Timer_Tick);

            this.Loaded += (o, e) =>
            {
                if (_diagram.Series.Count == 0)
                {
                    _headerControl.NewAction += NewCommandBinding_Executed;
                    _diagram.AxisX = CreateAxisx();
                    _diagram.AxisY =  CreateAxisY();

                    _control.MouseDoubleClick += Chart_MouseDoubleClick;
                    _diagram.Series.AddRange(CreateSeries());
                }

                _headerControl.DataContext = _baseModel;
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

        /// <summary>
        /// 图表鼠标双击事件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">时间参数</param>
        protected virtual void Chart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
        /// 构建数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        protected virtual BaseChartViewModel BuildDataContext()
        {
            return new TestChartViewModel { Title = _title.L10N(), ChartAlertLevel = SIE.WorkBenchChartBase.Commons.ChartAlertLevel.Green };
        }

        /// <summary>
        /// 构建视图
        /// </summary>
        private void BuildViewContent()
        {
            const string defpath = "pack://application:,,,/SIE.Wpf.WorkBenchChartBase;component/ChartControls/RadarDiagramControl.xaml";
            const string uri = defpath;
            var grid = XamlParseHelper.LoadEmbeddedXaml<Grid>(uri, UriKind.Absolute);
            this.Content = grid;
            _headerControl = grid.FindName("header") as ChartHeaderControl;
            _control = _headerControl.Content as ChartControl;
            _control.Legend = CreateLegend();
            _diagram = (_headerControl.Content as ChartControl).Diagram as RadarDiagram2D;
        }

        /// <summary>
        /// 新建命令执行方法
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">命令参数</param>
        protected virtual void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        /// <summary>
        /// 创建X轴
        /// </summary>
        /// <returns>X轴</returns>
        protected virtual RadarAxisX2D CreateAxisx()
        {
            return null;
        }

        /// <summary>
        /// 创建Y轴
        /// </summary>
        /// <returns>Y轴</returns>
        protected virtual RadarAxisY2D CreateAxisY()
        {
            return null;
        }

        /// <summary>
        /// 创建Series
        /// </summary>
        protected virtual List<Series> CreateSeries()
        {
            return new List<Series> { new RadarLineSeries2D() };
        }

        /// <summary>
        /// 定制Legend位置
        /// </summary>
        protected virtual Legend CreateLegend()
        {
            return new Legend();
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
        public void SetTimerInterval(double timeSpan = 3)
        {
            _timer.Stop();
            _timer.Interval = TimeSpan.FromMinutes(timeSpan);
            if (_timer.Interval.TotalSeconds < MinDispatchTimeSpanMilSecs)
                CRT.MessageService.ShowInstantMessage("刷新时间间隔少于{0}秒，建议在控制台设计中调整刷新时间间隔!".L10nFormat(MinDispatchTimeSpanMilSecs), "提示信息".L10N(), 3);
            _timer.Start();
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            _timer.Stop();
        }
    }
}
