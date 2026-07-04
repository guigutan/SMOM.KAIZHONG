using SIE.ComponentModel;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.WorkBenchCommon.Workbench.Tasks;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.WorkBenchCommon.Workbench.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.WorkBenchCommon.Workbench
{
    /// <summary>
    /// [任务闭环管理]工作台
    /// </summary>
    public partial class TaskManager : ComponentItem
    {
        /// <summary>
        /// 任务关闭率对象
        /// </summary>
        private TaskStatistics taskStatistics;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskManager()
        {
            InitializeComponent();
            UseProperty<ComponentProperty>();
        }

        /// <summary>
        /// 组件运行时
        /// </summary>
        protected override void OnRun()
        {
            taskStatistics = TrackableInterceptor.Create<TaskStatistics>();
            taskStatisticsPanel.DataContext = taskStatistics;
            LoadData();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var ctl = RT.Service.Resolve<TaskController>();
                tasksCreated.ItemsSource = ctl.GetMyLatestCreated();
                tasksPadding.ItemsSource = ctl.GetMyPadding();
                tasksDelay.ItemsSource = ctl.GetMyDelayed();
                tasksCopyTo.ItemsSource = ctl.GetCopyto();
                taskStatistics.Load(ctl.GetMyLatest());
            }));
        }

        /// <summary>
        /// 弹出任务编辑窗口
        /// </summary>
        /// <param name="task">任务对象</param>
        /// <param name="title">标题</param>
        /// <param name="viewGroup">视图配置</param>
        /// <param name="isClearCommand">是否清空命令</param>
        public static void ShowTaskWindows(TaskInfo task, string title, string viewGroup = ViewConfig.DetailsView, bool isClearCommand = false)
        {
            if (task == null) return;
            string key = CRT.Workbench.CreateKey(viewGroup, task.GetType(), task);
            var tmpl = new DetailsUITemplate(task.GetType(), viewGroup, key);
            var ui = tmpl.CreateUI();
            if (ui.MainView.CommandsContainer != null)
                ui.MainView.CommandsContainer.Visibility = System.Windows.Visibility.Collapsed;
            ui.MainView.Data = task;
            CRT.Workbench.ShowDialog(key, ui.Control, w =>
            {
                if (isClearCommand)
                {
                    w.Commands.Clear();
                }

                w.Title = title.L10N();
                w.Width = 900;
                w.Height = 430;
                w.Closing += (x, y) =>
                {
                    if (w.Result == 0)
                    {
                        y.Cancel = true;
                        RF.Save(task);
                        y.Cancel = false;
                    }
                };
            });
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            TaskInfo task = new TaskInfo();
            ShowTaskWindows(task, "添加任务");
        }

        /// <summary>
        /// 新建任务
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void TasksCreatedEdit_Click(object sender, RoutedEventArgs e)
        {
            var task = tasksCreated.SelectedItem as TaskInfo;
            ShowTaskWindows(task, "编辑任务");
        }

        /// <summary>
        /// 对待处理的任务进行编辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void TasksPaddingEdit_Click(object sender, RoutedEventArgs e)
        {
            var task = tasksPadding.SelectedItem as TaskInfo;
            ShowTaskWindows(task, "编辑任务");
        }

        /// <summary>
        /// 对已延期的任务进行编辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void TasksDelayEdit_Click(object sender, RoutedEventArgs e)
        {
            var task = tasksDelay.SelectedItem as TaskInfo;
            ShowTaskWindows(task, "编辑任务");
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var task = tasksCreated.SelectedItem as TaskInfo;
            bool result = CRT.MessageService.AskQuestion("你确定要取消该任务吗?".L10N());
            if (result)
            {
                task.Status = TaskStatus.Cancel;
                RF.Save(task);
            }
        }

        /// <summary>
        /// 查看任務
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void TasksCopyToView_Click(object sender, RoutedEventArgs e)
        {
            var task = tasksCopyTo.SelectedItem as TaskInfo;
            ShowTaskWindows(task, "查看任务", TaskInfoViewConfig.ReadonlyViewGroup, true);
        }

        /// <summary>
        /// 刷新任务
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }

    /// <summary>
    /// 任务关闭率对象
    /// </summary>
    class TaskStatistics : TrackableBase
    {
        /// <summary>
        /// 任务系列集合
        /// </summary>
        public ObservableCollection<TaskCategoryStatistics> TaskCategoryStatistics { get; } = new ObservableCollection<TaskCategoryStatistics>();

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="tasks"></param>
        public void Load(EntityList<TaskInfo> tasks)
        {
            Total = tasks.Count;
            Closed = tasks.Count(p => p.Status == TaskStatus.Closed);
            TaskCategoryStatistics.Clear();
            foreach (var cate in tasks.GroupBy(p => p.TypeName))
            {
                var m = TrackableInterceptor.Create<TaskCategoryStatistics>();
                m.Name = cate.Key;
                m.Qty = cate.Count();
                TaskCategoryStatistics.Add(m);
            }
        }

        /// <summary>
        /// 总任务数量
        /// </summary>
        public virtual int Total { get; set; }

        /// <summary>
        /// 任务关闭数
        /// </summary>
        public virtual int Closed { get; set; }

        /// <summary>
        /// 任务关闭率
        /// </summary>
        public double ClosedRate
        {
            get
            {
                if (Total > 0)
                    return Math.Round((double)Closed / Total * 100, 2);
                return 100;
            }
        }

        /// <summary>
        /// 属性变更事件触发
        /// </summary>
        /// <param name="propertyName">变更的属性名称</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Total) || propertyName == nameof(Closed))
                OnPropertyChanged(nameof(ClosedRate));
        }
    }

    /// <summary>
    /// 任务系列
    /// </summary>
    class TaskCategoryStatistics : TrackableBase
    {
        /// <summary>
        /// 任务系列名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 任务数量
        /// </summary>
        public int Qty { get; set; }
    }

    [SIE.ManagedProperty.CompiledPropertyDeclarer]
    static class TaskInfoExt
    {
        #region 任务状态 StatusDisplay
        /// <summary>
        /// 任务状态
        /// </summary>
        [Label("任务状态")]
        public static readonly Property<string> StatusDisplayProperty = P<TaskInfo>.RegisterExtensionReadOnly("StatusDisplay", typeof(TaskInfoExt),
            GetStatusDisplay, TaskInfo.StatusProperty);

        /// <summary>
        /// 任务状态
        /// </summary>
        public static string GetStatusDisplay(TaskInfo me)
        {
            return me.Status.ToLabel();
        }
        #endregion
    }

    /// <summary>
    /// tab标题宽
    /// </summary>
    public class TabItemWidthConverterMarkupExtension : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 当值从绑定源传播给绑定目标
        /// </summary>
        /// <param name="value">绑定源值</param>
        /// <param name="targetType">绑定目标属性的类型</param>
        /// <param name="parameter">使用的转换器参数</param>
        /// <param name="culture">转换器中使用的文化</param>
        /// <returns>返回转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = value.ConvertTo<double>();
            return Math.Floor((width - 5) / 5);
        }

        /// <summary>
        /// 当值从绑定目标传播给绑定源
        /// </summary>
        /// <param name="value">绑定目标的值</param>
        /// <param name="targetType">绑定目标属性的类型</param>
        /// <param name="parameter">使用的转换器参数</param>
        /// <param name="culture">转换器中使用的文化</param>
        /// <returns>返回转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// 为此标记扩展的目标属性值提供的对象
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// 任务颜色 转换器
    /// </summary>
    public class TaskColorConvertMarkupExtension : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 当值从绑定源传播给绑定目标
        /// </summary>
        /// <param name="value">绑定源值</param>
        /// <param name="targetType">绑定目标属性的类型</param>
        /// <param name="parameter">使用的转换器参数</param>
        /// <param name="culture">转换器中使用的文化</param>
        /// <returns>返回转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var task = value as TaskInfo;
            if (task != null && task.Status == TaskStatus.Padding)
            {
                if (task.PlanEnd.HasValue && task.PlanEnd.Value.Date == DateTime.Today)
                    return Application.Current.FindResource(new Themes.ColorBrushesKeyExtension { ResourceKey = Themes.ColorBrushesKeys.DarkYellowBrush });
                if (task.PlanEnd <= DateTime.Today.AddDays(1))
                    return Application.Current.FindResource(new Themes.ColorBrushesKeyExtension { ResourceKey = Themes.ColorBrushesKeys.RedBrush });
            }
            return Binding.DoNothing;
        }

        /// <summary>
        /// 当值从绑定目标传播给绑定源
        /// </summary>
        /// <param name="value">绑定目标的值</param>
        /// <param name="targetType">绑定目标属性的类型</param>
        /// <param name="parameter">使用的转换器参数</param>
        /// <param name="culture">转换器中使用的文化</param>
        /// <returns>返回转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// 为此标记扩展的目标属性值提供的对象
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
