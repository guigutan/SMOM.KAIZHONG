using SIE.Domain.Validation;
using Stimulsoft.Report.Wpf.ControlsV2;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// ReportMulitStationControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportMulitStationControl : UserControl
    {

        KZTaskReportMultiStationViewModel model;
        KZReportHelper kZReportHelper;

        /// <summary>
        /// 
        /// </summary>
        public ReportMulitStationControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public ReportMulitStationControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model as KZTaskReportMultiStationViewModel;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            this.Loaded += ReportMainControl_Loaded;
            this.Unloaded -= ReportMainControl_Loaded;
        }

        private void ReportMainControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var vm in model.ReportViewModelList)
            {
                vm.Error = null; vm.Tips = null;
                if (vm.Resource == null)
                    continue;
                Task.Run(() =>
                {
                    if (vm.DispatchTaskQueueList.Count > 0)
                        vm.LoadFirstQueueTask();
                });
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

        private void btnStartWork_Click(object sender, RoutedEventArgs e)
        {
            foreach (var vm in model.ReportViewModelList)
            {
                vm.Error = null; vm.Tips = null;
                if (vm.Resource == null)
                    continue;
                Task.Run(async () =>
                {
                    if (vm.ReportTimer == null || !vm.ReportTimer.Enabled)
                        await vm.StartWork();
                });
            }
        }

        private void btnPauseWork_Click(object sender, RoutedEventArgs e)
        {
            foreach (var vm in model.ReportViewModelList)
            {
                vm.Error = null; vm.Tips = null;
                if (vm.Resource == null)
                    continue;
                Task.Run(() =>
                {
                    if (vm.ReportTimer != null && vm.ReportTimer.Enabled)
                        vm.PauseWork();

                    if (vm.DispatchTaskQueueList.Count > 0)
                        vm.LoadFirstQueueTask();
                });
            }
        }

        private void btnResource_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowMulitSelectResourceList(null, null);
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        KZTaskReportViewModel GetSelectItem(object sender)
        {
            var btn = sender as System.Windows.Controls.Button;
            var tagString = btn.Tag.ToString();
            if (tagString.IsNullOrEmpty())
            {
                return null;
            }
            var vmId = tagString;
            var vm = (model as KZTaskReportMultiStationViewModel)?.ReportViewModelList.FirstOrDefault(p => p.Id == vmId);

            if (vm != null)
            {
                vm.ReportEmployee = model.ReportEmployee;
                if (vm.kZReportHelper == null)
                    vm.kZReportHelper = new KZReportHelper(vm);
            }
            return vm;
        }

        private void btnSelectResource_Click(object sender, RoutedEventArgs e)
        {
            var vm = GetSelectItem(sender);
            if (vm != null)
                kZReportHelper.ShowMulitSelectResourceList(vm);

            //if (vm.DispatchTaskQueueList.Count > 0)
            //    vm.LoadFirstQueueTask();
        }

        private void btnOperation_Click(object sender, RoutedEventArgs e)
        {
            var vm = GetSelectItem(sender);
            if (vm != null)
            {
                vm.IotMode = IotMode.Normal;
                kZReportHelper.ShowReportDetail(vm);
            }

            vm.IotMode = IotMode.MultiStation;
        }

        private void btnClearResource_Click(object sender, RoutedEventArgs e)
        {
            var vm = ctlMenu.SelectedItem as KZTaskReportViewModel;
            if (vm != null && vm.Resource != null)
            {
                if (vm.ReportTimer != null && vm.ReportTimer.Enabled)
                {
                    CRT.MessageService.ShowInstantMessage("当前工位正在进行数采任务,请先暂停任务后再操作", "提示", 3);
                    return;
                }
                vm.Resource = null;
            }
        }

        /// <summary>
        /// 区域上料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFeedingArea_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowFeedingAreaScan();
        }
    }
}
