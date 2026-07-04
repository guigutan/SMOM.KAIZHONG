using SIE.Domain;
using SIE.MES.Workbench.Experiences;
using SIE.Wpf.Common.Diagram;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SIE.Wpf.MES.Workbench.Experiences
{
    /// <summary>
    /// 历史经验库 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class ExperienceControl : ComponentItem
    {
        /// <summary>
        /// 上一个命令
        /// </summary>
        public static RoutedUICommand Previous { get; } = new RoutedUICommand();

        /// <summary>
        /// 下一个命令
        /// </summary>
        public static RoutedUICommand Next { get; } = new RoutedUICommand();

        /// <summary>
        /// 上一页命令
        /// </summary>
        public static RoutedUICommand PreviousPage { get; } = new RoutedUICommand();

        /// <summary>
        /// 下一页命令
        /// </summary>
        public static RoutedUICommand NextPage { get; } = new RoutedUICommand();

        /// <summary>
        /// 当前页命令
        /// </summary>
        public static RoutedUICommand CenterPage { get; } = new RoutedUICommand();

        /// <summary>
        /// 当前显示组序号
        /// </summary>
        int _currentIndex;

        /// <summary>
        /// 页数
        /// </summary>
        int _pageCount;

        /// <summary>
        /// 自定义属性
        /// </summary>
        ExperienceProperty _property;

        /// <summary>
        /// 历史经验列表
        /// </summary>
        private EntityList<ExperienceDetail> _historyExpeList = new EntityList<ExperienceDetail>();

        /// <summary>
        /// 颜色值
        /// </summary>
        private readonly string _colour128BEF = "#128BEF";

        /// <summary>
        /// 颜色值
        /// </summary>
        private readonly string _colourA6A6A6 = "#A6A6A6";

        /// <summary>
        /// 当前显示历史经验列表
        /// </summary>
        public EntityList<ExperienceDetail> Datasource
        {
            get { return (EntityList<ExperienceDetail>)GetValue(DatasourceProperty); }
            set { SetValue(DatasourceProperty, value); }
        }

        /// <summary>
        /// 数据属性
        /// </summary>
        public static readonly DependencyProperty DatasourceProperty =
            DependencyProperty.Register("Datasource", typeof(EntityList<ExperienceDetail>), typeof(ExperienceControl), new PropertyMetadata(null));

        /// <summary>
        /// 构造函数
        /// </summary>      
        public ExperienceControl()
        {
            InitializeComponent();
            _property = UseProperty<ExperienceProperty>();
            if (_property.ChangeSeconds <= 0) _property.ChangeSeconds = 5;
            var input = UseInput<ExperienceControlInput>();
            input.PropertyChanged += Input_PropertyChanged;
            this.Loaded += (o, e) =>
            {
                if (_currentIndex > 0) _currentIndex--;
                NextPageDataSource();
                _historyExpeList.CollectionChanged += HistoryExpeList_CollectionChanged;
            };
        }

        /// <summary>
        /// 输入参数变更事件
        /// </summary>
        /// <param name="sender">输入参数对象</param>
        /// <param name="e">事件参数</param>
        private void Input_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ExperienceControlInput input = sender as ExperienceControlInput;
            if (e.PropertyName == nameof(ExperienceControlInput.ItemId))
                ResetHistoryExpeList(RT.Service.Resolve<HistoryExperienceController>().GetHistoryExperienceList(input.ItemId, null));
            if (_currentIndex > 0) _currentIndex--;
            NextPageDataSource();
        }

        /// <summary>
        /// 历史经验列表重新设置
        /// </summary>
        /// <param name="dataSource">历史经验列表</param>
        public void ResetHistoryExpeList(EntityList<ExperienceDetail> dataSource)
        {
            _historyExpeList.Clear();
            _historyExpeList.AddRange(dataSource);

            _currentIndex = 0;
            _pageCount = _historyExpeList.Count / 2;
            if (_historyExpeList.Count % 2 != 0)
                _pageCount++;
        }

        /// <summary>
        /// 上一页设置数据源
        /// </summary>
        void PrePageDataSource()
        {
            var collection = new EntityList<ExperienceDetail>();
            collection.AddRange(_historyExpeList.Skip(2 * (_currentIndex - 2)).Take(2));
            _currentIndex--;
            this.DataContext = collection;
        }

        /// <summary>
        /// 下一页设置数据源
        /// </summary>
        /// <param name="i">跳转页面</param>
        void NextPageDataSource()
        {
            var collection = new EntityList<ExperienceDetail>();
            collection.AddRange(_historyExpeList.Skip(2 * (_currentIndex)).Take(2));
            _currentIndex++;
            this.DataContext = collection;
        }

        /// <summary>
        /// 历史经验列表变更事件
        /// </summary>
        /// <param name="sender">列表对象</param>
        /// <param name="e">事件参数</param>
        private void HistoryExpeList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NextPageDataSource();
        }

        /// <summary>
        /// 上一个命令执行条件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void PreviousBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_currentIndex == 1)
                e.CanExecute = false;
            else
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 上一个
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void PreviousCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrePageDataSource();
            if (_currentIndex == 1)
            {
                prePgBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colour128BEF));
                cetPgBtbn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colourA6A6A6));
                nxetPgBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colourA6A6A6));
            }
            else
            {
                prePgBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colourA6A6A6));
                cetPgBtbn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colour128BEF));
                nxetPgBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colourA6A6A6));
            }
        }

        /// <summary>
        /// 下一个命令执行条件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void NextBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_currentIndex >= _pageCount)
                e.CanExecute = false;
            else
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 下一个
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void NextCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NextPageDataSource();

            if (_currentIndex == _pageCount)
            {
                prePgBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colourA6A6A6));
                cetPgBtbn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colourA6A6A6));
                nxetPgBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colour128BEF));
            }
            else
            {
                prePgBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colourA6A6A6));
                cetPgBtbn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colour128BEF));
                nxetPgBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_colourA6A6A6));
            }
        }

        /// <summary>
        /// 上一页命令执行条件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void PrePageBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 上一页命令执行条件
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void PrePageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 上一页
        }

        /// <summary>
        /// 下一页命令执行条件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void NextPageBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 下一页命令执行条件
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void NextPageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 下一页
        }

        /// <summary>
        /// 当前页命令执行条件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void CenterPageBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 当前页命令执行条件
        }

        /// <summary>
        /// 当前页
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        private void CenterPageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 当前页
        }

        /// <summary>
        /// 计时器
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        /// Timeer计时器初始化
        /// </summary>
        private void TimerIni()
        {
            if (timer == null)
                timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(_property.ChangeSeconds <= 0 ? 5 : _property.ChangeSeconds);
            timer.IsEnabled = true;
            timer.Start();
        }

        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            timer?.Stop();
            timer = null;
        }

        /// <summary>
        /// 运行调用
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            if (_property.IsAutoChange)
                TimerIni();
        }

        /// <summary>
        /// 计时器事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_pageCount > 1)
            {
                SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
                {
                    if (_currentIndex > _pageCount)
                    {
                        _currentIndex = 1;
                    }

                    NextPageDataSource();
                });
            }
        }
    }

    /// <summary>
    /// 历史经验库输入参数
    /// </summary>
    public class ExperienceControlInput : ComponentInput<ExperienceControl>
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        [DisplayName("产品ID")]
        [Description("检验采集产品切换时传入")]
        public virtual double ItemId { get; set; }
    }

    /// <summary>
    /// 面板属性
    /// </summary>
    public class ExperienceProperty : ComponentProperty<ExperienceControl>
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        [DisplayName("图片自动切换"), CategoryAttribute("自定义")]
        public bool IsAutoChange { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        [DisplayName("图片自动切换时间(秒)"), CategoryAttribute("自定义")]
        [Description("图片切换时间间隔(秒)，必须大于0默认刷新时间5秒")]
        public int ChangeSeconds { get; set; }
    }
}