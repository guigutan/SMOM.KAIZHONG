using DevExpress.Xpf.Core.Native;
using DevExpress.XtraDiagram.Base;
using DocumentFormat.OpenXml.EMMA;
using SIE.Data.SqlTree;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.Items;
using SIE.Items.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.WIP;
using SIE.Warehouses;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// ScanReportControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportProcessControl : UserControl
    {

        KZReportHelper kZReportHelper;
        KZTaskReportProcessViewModel model;
        DetailLogicalView workstationView;
        /// <summary>
        /// 扫码类型 1、加入  2 移除
        /// </summary>
        public int ScanType { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        protected virtual ObservableCollection<ScanDetailInfo> labelInfoList { get; }


        /// <summary>
        /// 
        /// </summary>
        public ReportProcessControl()
        {
            InitializeComponent();
            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public ReportProcessControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model as KZTaskReportProcessViewModel;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;

            InitModelScan();


            //加载事件
            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;

            this.radioBtnTurnIn.Checked += RadioButton_Checked;
            this.radioBtnRemove.Checked += RadioButton_Checked;

            //var binding = new Binding("ResourceId") { Source = model.Workstation };
            //this.SetBinding(ScanReportControl.ResourceIdProperty, binding);

            //标签列表数据源
            //labelInfoList = new ObservableCollection<ScanDetailInfo>();
            //this.dataGrid.ItemsSource = labelInfoList;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            //创建工作单元控件
            if (workstationView == null)
            {
                workstationView = AutoUI.ViewFactory.CreateDetailView(typeof(Workstation));
                workstationView.Data = model.Workstation;
                workstationView.Current = model.Workstation;
                workstationView.Control.Margin = new Thickness(0, 10, 0, 10);
                DockPanel.SetDock(workstationView.Control, Dock.Bottom); //将工作站信息置底部
                dockPanel.Children.Insert(1, workstationView.Control);
            }

            reset();

            if (model.Workstation.Process == null && !WorkstationSelector.SelectOperation(model.Workstation, model.ReportEmployeeId))
            {
                showError("请先选择工作单元");
            }

            ScanType = 1;
        }

        void InitModelScan()
        {

            //model = new KZScanReportViewModel();
            //model.Resource = model.Resource;
            ////if (model.DispatchTask != null)
            ////{
            ////    var parentItem = RT.Service.Resolve<ItemController>().GetParentItemByItemId(model.DispatchTask.ProductId);
            ////    if (parentItem != null)
            ////        model.DispatchTask.ParShortDescription = parentItem.Bismt;
            ////}

            ////modelScan.DispatchTask = model.DispatchTask;
            //model.ReportEmployee = model.ReportEmployee;

            //this.DataContext = model;
            //kZReportHelper = model.kZReportHelper;

            model.Workstation.Resource = model.Resource;
            model.Workstation.EmployeeId = model.ReportEmployeeId;
            //限定资源列表
            model.Workstation.Resources.Clear();
            model.Workstation.Resources.Add(model.Resource);
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            showTip("");
            showError("正在加班加点开发中...");

            if (labelInfoList.Count == 0)
            {
                showError("没有要提交的数据");
                return;
            }
            try
            {
                if (CRT.MessageService.AskQuestion("确认要提交吗?".L10nFormat(), "确认"))
                {
                    var task = model.DispatchTask;
                    var workstation = model.Workstation;
                    var list = labelInfoList.ToList();
                    PdaScanSubmitInfo submitInfo = new PdaScanSubmitInfo()
                    {
                        ScanType = 1,
                        ResourceId = workstation.ResourceId ?? 0,
                        ProcessId = task.ProcessId ?? 0,
                        DispatchTaskId = task.Id,
                        WorkOrderId = task?.WorkOrderId ?? 0,
                        DetailInfos = labelInfoList.ToList(),
                        ReportEmployeeId = model.ReportEmployeeId ?? 0,
                        IsTaskFinish = true
                    };
                    var msg = RT.Service.Resolve<ReportController>().SubmitScanValid(submitInfo);
                    if (!msg.IsNullOrEmpty())
                    {
                        if (CRT.MessageService.AskQuestion(msg, "确认"))
                        {
                            submitInfo.IsTaskFinish = true;
                        }
                        else
                        {
                            submitInfo.IsTaskFinish = false;
                        }
                    }
                    var printInfos = RT.Service.Resolve<ReportController>().SubmitScanInfo(submitInfo);

                    model.DispatchTask = model.LoadTask(model.DispatchTaskId);

                    showTip("提交成功".L10N());

                    labelInfoList.Clear();

                    if (printInfos.Any())
                    {
                        model.PrintLabels(printInfos);
                    }
                }
            }
            catch (Exception ex)
            {
                showError(ex.GetBaseException().Message);
            }
            finally
            {
                this.txtBarcode.Text = "";
                this.txtBarcode.Focus();
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    showTip("");


                    var task = model.DispatchTask;
                    var workstation = model.Workstation;
                    if (workstation == null || workstation.ResourceId == null || workstation.ProcessId == null || workstation.StationId == null)
                        throw new ValidationException("请先选择工作单元");
                    string barcode = this.txtBarcode.Text;

                    //移除标签模式
                    if (ScanType == 2)
                    {
                        var toRemove = model.WipBatchQueueList.FirstOrDefault(p => p.BatchNo == barcode);
                        if (toRemove != null)
                        {
                            var d = RT.Service.Resolve<DispatchController>().RemoveQueueWipBatch(new List<double>() { toRemove.Id });
                            model.WipBatchQueueList.Remove(toRemove);

                            showTip("[{0}]已移除".L10nFormat(barcode));
                        }
                        else
                            showError("[{0}]不在明细列表,无法移除".L10nFormat(barcode));
                        return;
                    }

                    //加入标签模式
                    if (model.WipBatchQueueList.Any(p => p.BatchNo == barcode))
                        throw new ValidationException("[{0}]已在明细列表,请勿重复扫描".L10nFormat(barcode));

                    var scanInfo = new PdaScanInfo()
                    {
                        Sn = barcode,
                        ScanType = ScanType,
                        IsFirstSn = true,
                        DispatchTaskId = task?.Id,
                        WorkOrderId = task?.WorkOrderId,
                        ResourceId = workstation.ResourceId ?? 0,
                        ProcessId = workstation.ProcessId ?? 0,
                        StationId = workstation.StationId ?? 0,
                    };
                    var ret = RT.Service.Resolve<ReportController>().CheckScanInfo(scanInfo);
                    model.AddWipBatchQueue(ret.LabelNo);
                    //labelInfoList.Add(new ScanDetailInfo()
                    //{
                    //    Sn = barcode,
                    //    Qty = ret.LabelQty,
                    //    GoodQty = ret.LabelQty,
                    //    SuspectQty = 0
                    //});
                    //if (ret.DispatchTaskId > 0)
                    //    model.DispatchTask = model.LoadTask(ret.DispatchTaskId);


                    showTip("[{0}]加入成功".L10nFormat(barcode));
                }
                catch (Exception ex)
                {
                    showError(ex.GetBaseException().Message);
                }
                finally
                {
                    this.txtBarcode.Text = "";
                    this.txtBarcode.Focus();
                }
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        void showTip(string msg)
        {
            this.txtTip.Foreground = new SolidColorBrush(Colors.Green);
            this.txtTip.Text = msg;
        }

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="msg"></param>
        void showError(string msg)
        {
            this.txtTip.Foreground = new SolidColorBrush(Colors.Red);
            this.txtTip.Text = msg;
        }

        /// <summary>
        /// 重置
        /// </summary>
        void reset()
        {
            model.DispatchTask = null;
            labelInfoList?.Clear();
            this.txtBarcode.Text = "";
            this.txtBarcode.Focus();
            showTip("请扫描工序标签");
            kZReportHelper.SwitchToEnglishMode();
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        void close()
        {
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }
        /// <summary>
        /// 扫码模式切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton)?.Content.ToString() == "加入")
            {
                ScanType = 1;
                showTip("当前已切换为 [加入] 模式");
            }
            else if ((sender as RadioButton)?.Content.ToString() == "移除")
            {
                ScanType = 2;
                showError("当前已切换为 [移除] 模式");
            }
            this.txtBarcode.Text = "";
            this.txtBarcode.Focus();
            kZReportHelper.SwitchToEnglishMode();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        /// <summary>
        /// 编辑良品数 
        /// </summary>
        /// <param name="sender"></param>
        void showGoodQtyInput(object sender)
        {

            Task.Run(() =>
            {

                CRT.MainThread.InvokeIfRequired(() =>
                {
                    var textBox = sender as Button;
                    var value = textBox.Content.ToString();
                    if (value.IsNullOrEmpty()) value = "0";
                    kZReportHelper.ShowCalculatorEditor((value) =>
                    {
                        var data = this.dataGrid.SelectedItem as ScanDetailInfo;
                        data.GoodQty = value;
                        textBox.Content = value.ToString();
                        return value;
                    }, decimal.Parse(value));
                    this.txtBarcode.Focus();
                });
            });
        }

        /// <summary>
        /// 编辑可疑品数
        /// </summary>
        /// <param name="sender"></param>
        void showSuspectQtyInput(object sender)
        {
            if (model.WipBatchQueue == null)
                return;
            Task.Run(() =>
            {

                CRT.MainThread.InvokeIfRequired(() =>
                {
                    var textBox = sender as Button;
                    var value = model.SuspectQty.ToString();
                    if (value.IsNullOrEmpty()) value = "0";
                    kZReportHelper.ShowCalculatorEditor((value) =>
                    {
                        var iotQty = model.WipBatchQueue.IotQty > model.WipBatchQueue.BatchQty ? model.WipBatchQueue.BatchQty : model.WipBatchQueue.IotQty;
                        if (value > iotQty)
                            value = model.WipBatchQueue.IotQty;
                        model.SuspectQty = value;
                        model.OkQty = iotQty - model.SuspectQty;

                        return value;
                    }, decimal.Parse(value));
                    this.txtBarcode.Focus();
                });
            });
        }

        private void btnOkQty_Click(object sender, RoutedEventArgs e)
        {
            showGoodQtyInput(sender);
        }

        private void btnSuspectQty_Click(object sender, RoutedEventArgs e)
        {
            showSuspectQtyInput(sender);
        }

        private void btnTaskList_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowViewTaskList(model.Workstation.ResourceId, model.Workstation.ProcessId);
        }


        private void btnStartWork_Click(object sender, RoutedEventArgs e)
        {
            this.model.StartWork();
            this.dataGrid.SelectedItems.Clear();
        }
        private void btnResource_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowSelectResourceList();
        }

        private void btnSelectTask_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowViewTaskList(model.Workstation.ResourceId, model.Workstation.ProcessId);
        }

        private void btnScanReport_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowScanReport();
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
        /// 余料称重
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SWRecordScan_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowSWRecordScan();
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
            //进入任务列表
            kZReportHelper.ShowViewTaskList(model.ResourceId,model.ProcessId,true);
        }
        

    }
}
