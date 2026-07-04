using SIE.WorkBenchCommon.Workbench.Tasks;
using SIE.Wpf.Common.Diagram;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.WorkBenchCommon.Workbench
{
    /// <summary>
    /// MyTask.xaml 的交互逻辑
    /// </summary>
    public partial class MyTask : ComponentItem
    {
        TaskOutput _output;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MyTask()
        {
            InitializeComponent();
            UseProperty<ComponentProperty>();
            _output = UseOutput<TaskOutput>();
        }

        /// <summary>
        /// 运行后
        /// </summary>
        protected override void OnRun()
        {
            taskListBox.SelectionChanged += TaskListBox_SelectionChanged;
            _output.RefreshAction = LoadData;
            LoadData();
        }

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = taskListBox.SelectedItem as TaskInfo;
            if (selected != null)
            {
                _output.SelectedTask = selected;
            }
        }

        void LoadData()
        {
            taskListBox.ItemsSource = RT.Service.Resolve<TaskController>().GetMyPadding();
            taskListBox.SelectedIndex = 0;
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }

    /// <summary>
    /// 输出参数
    /// </summary>
    public class TaskOutput : ComponentOutput<MyTask>
    {
        /// <summary>
        /// 选中的任务
        /// </summary>
        [DisplayName("选中的任务")]
        public virtual TaskInfo SelectedTask { get; set; }

        /// <summary>
        /// 刷新任务
        /// </summary>
        [DisplayName("刷新任务")]
        public virtual Action RefreshAction { get; set; }
    }
}
