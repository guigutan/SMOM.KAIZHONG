using DocumentFormat.OpenXml.EMMA;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// ReportMainControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportManualControl : UserControl
    {

        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;

        /// <summary>
        /// 
        /// </summary>
        public ReportManualControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public ReportManualControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            this.Loaded += ReportManualControl_Loaded;
            this.Unloaded -= ReportManualControl_Loaded;
        }

        private void ReportManualControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.mainPanel.Child == null)
            {
                this.mainPanel.Child = new ReportContentControl(model, true);
            }
            model.DispatchTask = model.LoadTask(model.DispatchTaskId);

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

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            kZReportHelper.ShowNumberInput(KZTaskReportViewModel.SuspectQtyProperty);
        }

        private void TextBox_TouchUp(object sender, TouchEventArgs e)
        {
            kZReportHelper.ShowNumberInput(KZTaskReportViewModel.SuspectQtyProperty);

        }

        private void TextBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            kZReportHelper.ShowNumberInput(KZTaskReportViewModel.SuspectQtyProperty);

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (model.OkQty == 0 && model.SuspectQty == 0)
            {
                CRT.MessageService.ShowInstantMessage("请输入报工数或可疑品数");
                return;
            }
            if (CRT.MessageService.AskQuestion("确认要提交报工良品[{0}]可疑品[{1}]吗?".L10nFormat(model.OkQty, model.SuspectQty), "确认"))
            {
                var labels = this.model.TaskReport(model.OkQty, model.SuspectQty);
                if (labels.Count > 0)
                {
                    model.PrintLabels(labels);
                }
                CRT.MessageService.ShowInstantMessage("报工成功".L10N(), "提示".L10N(), 5);
            }

        }

        private void btnSelectTask_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowSelectTaskList();
        }
    }
}
