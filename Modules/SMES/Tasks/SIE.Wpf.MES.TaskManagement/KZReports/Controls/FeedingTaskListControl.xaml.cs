using SIE.MES.TaskManagement.Dispatchs;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// FeedingTaskListControl.xaml 的交互逻辑
    /// </summary>
    public partial class FeedingTaskListControl : UserControl
    {

        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;

        public double? TaskId { get; set; }

        public FeedingTaskListControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public FeedingTaskListControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            this.Loaded += FeedingTaskListControl_Loaded;
            this.Unloaded -= FeedingTaskListControl_Loaded;

        }

        private void FeedingTaskListControl_Loaded(object sender, RoutedEventArgs e)
        {
            model.FeedingDispatchTaskList.Clear();
            Task.Run(new Action(() =>
            {
                model.LoadFeedingTaskList();
            }).WithCurrentThreadContext());
        }

        void close()
        {
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }

        //private void dataGridRecord_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    //var task = this.dataGridRecord.SelectedItem as DispatchTask;
        //    //if (task == null) return;
        //    //TaskId = task.Id;
        //    //close();
        //}

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var task = this.dataGridRecord.SelectedItem as DispatchTask;
            if (task == null)
            {
                MessageBox.Show("请选择一行数据".L10N());
                return;
            }
            TaskId = task.Id;
            kZReportHelper.ShowFeedingScan(task.Id);
            model.LoadFeedingTaskList();
            this.TaskId = null;
            //close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.model.LoadTaskList();
        }

    }
}
