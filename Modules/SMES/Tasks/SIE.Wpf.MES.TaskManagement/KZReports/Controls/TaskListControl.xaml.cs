using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.Threading;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// TaskListControl.xaml 的交互逻辑
    /// </summary>
    public partial class TaskListControl : UserControl
    {
        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;

        public double TaskId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TaskListControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public TaskListControl(KZTaskReportViewModelBase _model)
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
            model.DispatchTaskList.Clear();
            Task.Run(new Action(() =>
            {
                model.LoadTaskList();
            }).WithCurrentThreadContext());

            if (model.IsReportManual)
            {
                btnTaskQueueList.IsEnabled = false;
                btnAddQueue.Visibility = Visibility.Hidden;
            }
            else
            {
                btnTaskQueueList.IsEnabled = true;
                btnAddQueue.Visibility = Visibility.Visible;
            }
        }

        void close()
        {
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridRecord.SelectedItem == null)
            {
                CRT.MessageService.ShowInstantMessage("请选择一行数据".L10N(), "提示", 3);
                return;
            }
            try
            {
                var dispatchTask = this.dataGridRecord.SelectedItem as DispatchTask;
                this.model.PauseDispatchTask(dispatchTask, model.Resource);
                this.model.LoadTaskList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.L10N());
            }
        }

        private void btnExcute_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridRecord.SelectedItem == null)
            {
                CRT.MessageService.ShowInstantMessage("请选择一行数据".L10N(), "提示", 3);
                return;
            }
            try
            {
                //var dispatchTask = this.dataGridRecord.SelectedItem as DispatchTask;
                //this.model.StartDispatchTask(dispatchTask);
                //this.model.LoadTaskList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.L10N());
            }
        }

        private void dataGridRecord_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var task = this.dataGridRecord.SelectedItem as DispatchTask;
            if (task == null) return;
            TaskId = task.Id;
            close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (model.IsReportManual)
            {
                var task = this.dataGridRecord.SelectedItem as DispatchTask;
                if (task == null)
                {
                    MessageBox.Show("请选择一行数据".L10N());
                    return;
                }
                TaskId = task.Id;
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
            }
            model.DispatchTask = RF.GetById<DispatchTask>(TaskId, new EagerLoadOptions().LoadWithViewProperty());
            if (model.DispatchTask != null)
            {
                var parentItem = RT.Service.Resolve<ItemController>().GetParentItemByItemId(model.DispatchTask.ProductId);
                if (parentItem != null)
                    model.DispatchTask.ParShortDescription = parentItem.Bismt;
            }
            close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }


        private void btnAddQueue_Click(object sender, RoutedEventArgs e)
        {
            var task = this.dataGridRecord.SelectedItem as DispatchTask;
            if (task == null)
            {
                CRT.MessageService.ShowInstantMessage("请选择一行数据".L10N(), "提示", 3);
                return;
            }
            model.AddQueueTask(task);
        }

        private void btnTaskQueueList_Click(object sender, RoutedEventArgs e)
        {
            close();
            kZReportHelper.ShowTaskQueueList();
        }
    }
}
