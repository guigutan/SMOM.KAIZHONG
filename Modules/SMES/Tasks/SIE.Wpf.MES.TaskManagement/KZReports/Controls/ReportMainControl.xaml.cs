using SIE.Domain.Validation;
using SIE.Wpf.MES.WIP;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// ReportMainControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportMainControl : UserControl
    {

        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;

        #region 员工
        /// <summary>
        /// 员工
        /// </summary>
        public string Employee
        {
            get { return (string)GetValue(EmployeeProperty); }
            set { SetValue(EmployeeProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly DependencyProperty EmployeeProperty =
            DependencyProperty.Register("Employee", typeof(string), typeof(ReportMainControl), new PropertyMetadata(string.Empty));
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ReportMainControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public ReportMainControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            this.Loaded += ReportMainControl_Loaded;
            this.Unloaded -= ReportMainControl_Loaded;
        }

        private void ReportMainControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.mainPanel.Child == null)
            {
                if (model.IotMode == IotMode.CommonMode)
                {
                    this.mainPanel.Child = new ReportContentCommonModeControl(model as KZTaskReportCommonModeViewModel, false);
                    this.btnExceptionReport.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(this.btnResource, 2);
                    Grid.SetColumn(this.btnTaskQueueList, 3);
                    Grid.SetColumn(this.Weighing, 4);
                    Grid.SetColumn(this.taskList, 5);
                    Grid.SetColumn(this.btnAndon, 6);
                }
                else
                {
                    this.mainPanel.Child = new ReportContentControl(model as KZTaskReportViewModel, false);
                }
            }

            //分辨率适应优化
            var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            var screenHeight = (int)SystemParameters.PrimaryScreenHeight;
            if (screenWidth <= 1024 || screenHeight <= 800)
            {
                this.topPanel.Margin = new Thickness(10, 0, 10, 0);
                this.mainPanel.Margin = new Thickness(10, 0, 10, 0);
                this.bottomPanel.Margin = new Thickness(10, 10, 10, 5);
                var parent = topPanel.GetLogicalParent<Grid>();
                if (parent != null)
                {
                    parent.Margin = new Thickness(5);
                }
            }
            else
            {
                this.topPanel.Margin = new Thickness(50, 0, 50, 0);
                this.mainPanel.Margin = new Thickness(50, 0, 50, 0);
                this.bottomPanel.Margin = new Thickness(50, 10, 50, 30);
                var parent = topPanel.GetLogicalParent<Grid>();
                if (parent != null)
                {
                    parent.Margin = new Thickness(30);
                }
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

        /// <summary>
        /// 安灯管理a
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAndon_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowViewAndon(model.Workstation);
        }

        private void btnResource_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowSelectResourceList();
        }

        private void btnExceptionReport_Click(object sender, RoutedEventArgs e)
        {
            this.model.ExceptionReport();
        }

        private void btnStartWork_Click(object sender, RoutedEventArgs e)
        {
            this.model.StartWork();
        }

        private void btnSelectTask_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowSelectTaskList();
        }

        private void btnManualReport_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowReportManual();
        }

        private void btnScanReport_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowScanReport();
        }

        /// <summary>
        /// 余料称重
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SWRecordScan_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowSWRecordScan();
        }

        /// <summary>
        /// 下料按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnDeduction_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowDeductionList();
        }

        /// <summary>
        /// 上料按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFeeding_Click(object sender, RoutedEventArgs e)
        {
            if (model.DispatchTaskId > 0)
            {
                kZReportHelper.ShowFeedingScan(model.DispatchTaskId.Value);
            }
            else
                kZReportHelper.ShowFeedingTaskList();
        }

        /// <summary>
        /// 开机准备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartPreparation_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowPreparationNav();
        }

        /// <summary>
        /// 任务队列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTaskQueueList_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowTaskQueueList();
        }

        /// <summary>
        /// 称重按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWeighing_Click(object sender, RoutedEventArgs e)
        {
            //进入称重
            kZReportHelper.ShowWeighing();
        }

        /// <summary>
        /// 任务列表按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void taskList_Click(object sender, RoutedEventArgs e)
        {
            //任务列表
            kZReportHelper.ShowViewTaskList(model.ResourceId,model.ProcessId);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }
    }
}
