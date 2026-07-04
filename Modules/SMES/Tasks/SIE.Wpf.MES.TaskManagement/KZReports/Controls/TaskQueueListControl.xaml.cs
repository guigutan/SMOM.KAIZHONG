using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// TaskQueueListControl.xaml 的交互逻辑
    /// </summary>
    public partial class TaskQueueListControl : UserControl
    {
        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;

        public double TaskId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TaskQueueListControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public TaskQueueListControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            this.Loaded += TaskListControl_Loaded;
            this.Unloaded -= TaskListControl_Loaded;
        }

        private void TaskListControl_Loaded(object sender, RoutedEventArgs e)
        {
            var colCheck = dataGridRecord.Columns.FirstOrDefault(p => p.Header.ToString() == "选中");
            if (model.IotMode == IotMode.CommonMode)
            {
                btnCommonMode.Visibility = Visibility.Visible;
                dataGridRecord.SelectionMode = DataGridSelectionMode.Extended;
                if (colCheck != null)
                    colCheck.Visibility = Visibility.Visible;
            }
            else
            {
                btnCommonMode.Visibility = Visibility.Hidden;
                dataGridRecord.SelectionMode = DataGridSelectionMode.Single;
                if (colCheck != null)
                    colCheck.Visibility = Visibility.Hidden;
            }


            model.DispatchTaskQueueList.Clear();
            var view = CollectionViewSource.GetDefaultView(model.DispatchTaskQueueList);
            view.SortDescriptions.Add(new SortDescription("Seq", ListSortDirection.Ascending));
            dataGridRecord.ItemsSource = view;
            view.Refresh();

            Task.Run(new Action(() =>
            {
                model.LoadTaskQueueList();
            }).WithCurrentThreadContext());
        }

        void close()
        {
            if (model.DispatchTaskQueueList.IsDirty)
            {
                if (CRT.MessageService.AskQuestion("生产顺序有调整,需要保存当前生产顺序吗".L10N()))
                {
                    RF.Save(model.DispatchTaskQueueList);
                }
            }
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }

        DispatchTaskQueue GetSelectTaskQueue()
        {
            var taskQueue = this.dataGridRecord.SelectedItem as DispatchTaskQueue;
            if (taskQueue == null)
            {
                CRT.MessageService.ShowInstantMessage("请选择一行数据".L10N(), "提示", 3);
                return null;
            }
            return taskQueue;
        }

        List<DispatchTaskQueue> GetSelectTaskQueueList()
        {
            var list = model.DispatchTaskQueueList.Where(p => p.IsSelected).ToList();

            return list;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sort"></param>
        void SortDatas(int sort)
        {
            var queue = GetSelectTaskQueue();
            if (queue == null)
                return;
            var currSeq = queue.Seq;
            if (queue.TaskStatus == DispatchTaskStatus.Executing)   //执行中 不允许调整
                return;
            DispatchTaskQueue taskQueue;
            if (sort >= 0)
                taskQueue = model.DispatchTaskQueueList.FirstOrDefault(p => p.Seq > currSeq && p.TaskStatus != DispatchTaskStatus.Executing);
            else
                taskQueue = model.DispatchTaskQueueList.LastOrDefault(p => p.Seq < currSeq && p.TaskStatus != DispatchTaskStatus.Executing);
            if (taskQueue == null)
                return;
            queue.Seq = currSeq + sort;
            taskQueue.Seq = currSeq;

            var view = CollectionViewSource.GetDefaultView(model.DispatchTaskQueueList);
            view.Refresh();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            RF.Save(model.DispatchTaskQueueList);

            if (model.IotMode == IotMode.CommonMode)
            {
                (model as KZTaskReportCommonModeViewModel)?.LoadFirstQueueTask();

            }
            else
            {
                var queue = model.DispatchTaskQueueList.OrderBy(p => p.Seq).FirstOrDefault();
                if (queue == null)
                {
                    CRT.MessageService.ShowInstantMessage("生产队列中没有生产任务,请确认".L10N(), "提示", 3);
                    return;
                }
                TaskId = queue.DispatchTaskId;
                model.DispatchTask = model.LoadTask(TaskId);
            }
            close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }


        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            SortDatas(-10);
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            SortDatas(10);
        }

        private void btnTaskList_Click(object sender, RoutedEventArgs e)
        {
            close();
            kZReportHelper.ShowSelectTaskList();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var queue = GetSelectTaskQueue();
            if (queue != null)
            {
                this.model.RemoveQueueTask(queue);
                if (model.DispatchTaskId == queue.DispatchTaskId)
                    model.DispatchTask = null;
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            var queue = GetSelectTaskQueue();
            if (queue != null)
            {
                if (model.IotMode == IotMode.CommonMode)
                {
                    var taskIds = new List<double>() { queue.DispatchTaskId };
                    var dispatchTasks = RT.Service.Resolve<ReportController>().PauseIOTWorkTask(taskIds, model.Resource);
                    queue.LoadProperty(DispatchTaskQueue.TaskStatusProperty, dispatchTasks.FirstOrDefault().TaskStatus);
                }
                else
                {
                    var dispatchTask = RT.Service.Resolve<ReportController>().PauseIOTWorkTask(queue.DispatchTask, model.Resource);
                    queue.LoadProperty(DispatchTaskQueue.TaskStatusProperty, dispatchTask.TaskStatus);
                }
                var view = CollectionViewSource.GetDefaultView(model.DispatchTaskQueueList);
                view.Refresh();
            }
        }

        private void btnExcute_Click(object sender, RoutedEventArgs e)
        {
            var queue = GetSelectTaskQueue();
            if (queue != null)
            {
                var dispatchTask = RT.Service.Resolve<ReportController>().StartIOTWorkTask(queue.DispatchTask, model.Resource, false);
                queue.LoadProperty(DispatchTaskQueue.TaskStatusProperty, dispatchTask?.TaskStatus);

                var view = CollectionViewSource.GetDefaultView(model.DispatchTaskQueueList);
                view.Refresh();
            }

        }

        private void btnCommonMode_Click(object sender, RoutedEventArgs e)
        {
            var list = GetSelectTaskQueueList();
            if (list.Count == 0)
                return;
            var minSeq = list.Min(p => p.Seq);
            list.ForEach(p => p.Seq = minSeq);

            var view = CollectionViewSource.GetDefaultView(model.DispatchTaskQueueList);
            view.Refresh();
        }
    }
}
