using DevExpress.Xpf.Core.Native;
using DevExpress.Xpo.Helpers;
using DevExpress.XtraDiagram.Base;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using SIE.Core.RedisUtil;
using SIE.Data.SqlTree;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.Items;
using SIE.Items.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.WIP;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
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
    public partial class ScanReportControl : UserControl
    {

        KZReportHelper kZReportHelper;
        KZTaskReportViewModelBase model;
        public KZScanReportViewModel modelScan;
        DetailLogicalView workstationView;
        /// <summary>
        /// 扫码类型 1、报工模式  2转入模式
        /// </summary>
        public int ScanType { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        protected virtual ObservableCollection<ScanDetailInfo> labelInfoList { get; }

        /// <summary>
        /// 任务列表
        /// </summary>
        protected virtual ObservableCollection<DispatchTask> dispatchTaskList { get; }

        #region 资源Id
        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetValue(ResourceIdProperty); }
            set { SetValue(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源Id
        /// </summary>
        public static readonly DependencyProperty ResourceIdProperty =
            DependencyProperty.Register("ResourceId", typeof(double), typeof(ScanReportControl), new PropertyMetadata(0d, (s, e) => { ((ScanReportControl)s).OnValueChanged(e); }));

        /// <summary>
        /// OnValueChanged
        /// </summary>
        /// <param name="e">e</param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var resourceId = double.Parse(e.NewValue.ToString());
            Task.Run(() =>
            {
                CRT.MainThread.InvokeIfRequired(() =>
                {
                    reset();
                });
            });
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ScanReportControl()
        {
            InitializeComponent();
            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public ScanReportControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;

            InitModelScan();


            //加载事件
            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;

            this.radioBtnTurnIn.Checked += RadioButton_Checked;
            this.radioBtnReport.Checked += RadioButton_Checked;

            var binding = new Binding("ResourceId") { Source = modelScan.Workstation };
            this.SetBinding(ScanReportControl.ResourceIdProperty, binding);

            //标签列表数据源
            labelInfoList = new ObservableCollection<ScanDetailInfo>();
            this.dataGrid.ItemsSource = labelInfoList;
            //任务列表数据源
            dispatchTaskList = new ObservableCollection<DispatchTask>();
            this.dataGridRecord.ItemsSource = dispatchTaskList;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            //创建工作单元控件
            if (workstationView == null)
            {
                workstationView = AutoUI.ViewFactory.CreateDetailView(typeof(Workstation));
                workstationView.Data = modelScan.Workstation;
                workstationView.Current = modelScan.Workstation;
                workstationView.Control.Margin = new Thickness(0, 10, 0, 10);
                DockPanel.SetDock(workstationView.Control, Dock.Bottom); //将工作站信息置底部
                dockPanel.Children.Insert(0, workstationView.Control);
            }

            reset();

            //找出当前上次缓存的数据
            string id_key = "ScanReportControl_" + model.IotMode + RT.IdentityId;
            var str = RT.Service.Resolve<RedisUtilController>().RedisUtilGet(id_key);
            Workstation workstation = null;
            if (!str.IsNullOrEmpty())
            {
                var strs = str.Split("_");
                var resource = RT.Service.Resolve<WipResourceController>().GetWipResourceById(Convert.ToDouble(strs[0]));
                var process = RT.Service.Resolve<ProcessController>().GetProcessByIds(new List<double>() { Convert.ToDouble(strs[1]) }).FirstOrDefault();
                var station = RT.Service.Resolve<StationController>().GetStation(Convert.ToDouble(strs[2]));
                //按照原来弹窗选择的方式，判断，防止数据被删除修改造成问题
                if (resource != null && process != null && station != null)
                {
                    workstation = new Workstation(null);
                    var processes = RT.Service.Resolve<ProcessController>().GetProcesssByUserId(modelScan.ReportEmployeeId ?? 0, null, workstation.ProcessTypes);
                    var stations = RT.Service.Resolve<StationController>().GetStationsByResourceId(resource.Id, process.Id);

                    workstation.ResourceId = resource?.Id;
                    workstation.ProcessId = process?.Id;
                    workstation.StationId = station?.Id;

                    if (processes.All(p => p.Id != process.Id) || stations.All(p => p.Id != station.Id))
                    {
                        workstation = null;
                    }
                }
            }

            //上次资源不能为空，上次工序不能为空，上次工位不能为空，上次的资源和当前选择的资源要相同
            if (workstation != null && workstation.ResourceId != null && workstation.ProcessId != null && workstation.StationId != null && workstation.ResourceId == modelScan.Workstation.ResourceId)
            {
                //赋值顺序不能更换，工位信息选择触发相应的事件
                modelScan.Workstation.Resource = workstation.Resource;
                modelScan.Workstation.Process = workstation.Process;
                modelScan.Workstation.Station = workstation.Station;
            }
            else
            {
                if (!WorkstationSelector.SelectOperation(modelScan.Workstation, modelScan.ReportEmployeeId))
                {
                    showError("请先选择工作单元");
                }
                //redis缓存将当前选择的给记录下来
                //只保存七天
                //RT.Service.Resolve<RedisUtilController>().RedisUtilSet(id_key, (modelScan.Workstation.ResourceId ?? 0) + "_" + (modelScan.Workstation.ProcessId ?? 0) + "_" + (modelScan.Workstation.StationId ?? 0), 10080);
            }


            LoadTaskList();
            ScanType = 1;
        }


        void InitModelScan()
        {

            modelScan = new KZScanReportViewModel();
            modelScan.Resource = model.Resource;
            //if (model.DispatchTask != null)
            //{
            //    var parentItem = RT.Service.Resolve<ItemController>().GetParentItemByItemId(model.DispatchTask.ProductId);
            //    if (parentItem != null)
            //        model.DispatchTask.ParShortDescription = parentItem.Bismt;
            //}

            //modelScan.DispatchTask = model.DispatchTask;
            modelScan.ReportEmployee = model.ReportEmployee;

            this.DataContext = modelScan;
            kZReportHelper = model.kZReportHelper;

            modelScan.Workstation.Resource = model.Resource;
            modelScan.Workstation.EmployeeId = model.ReportEmployeeId;
            //限定资源列表
            modelScan.Workstation.Resources.Clear();
            modelScan.Workstation.Resources.Add(model.Resource);

            modelScan.Workstation.PropertyChanged -= Workstation_PropertyChanged;
            modelScan.Workstation.PropertyChanged += Workstation_PropertyChanged;

        }

        private void Workstation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Workstation.StationIdProperty.Name || e.PropertyName == Workstation.ProcessIdProperty.Name || e.PropertyName == Workstation.ResourceIdProperty.Name)
            {
                var workstation = sender as Workstation;
                if (workstation.ResourceId > 0 && workstation.ProcessId > 0 && workstation.StationId > 0)
                {
                    //找出当前上次缓存的数据
                    string id_key = "ScanReportControl_" + model.IotMode + RT.IdentityId;
                    //redis缓存将当前选择的给记录下来
                    //只保存七天
                    RT.Service.Resolve<RedisUtilController>().RedisUtilSet(id_key, (modelScan.Workstation.ResourceId ?? 0) + "_" + (modelScan.Workstation.ProcessId ?? 0) + "_" + (modelScan.Workstation.StationId ?? 0), 10080);
                }
            }
        }


        /// <summary>
        /// 获取任务单列表
        /// </summary>
        public virtual void LoadTaskList()
        {
            TaskQueryInfo info = new TaskQueryInfo()
            {
                EmployeeId = model.ReportEmployee.Id,
                ResourceId = ResourceId,
                TaskType = 1,
                ProcessArray = modelScan.Workstation.ProcessId==null?"": modelScan.Workstation.ProcessId.ToString()
            };
            var status = new List<DispatchTaskStatus>() { DispatchTaskStatus.Dispatched, DispatchTaskStatus.Dispatching, DispatchTaskStatus.ToDispatch, DispatchTaskStatus.Executing, DispatchTaskStatus.Pause/*, DispatchTaskStatus.Finished, DispatchTaskStatus.Closed*/ };
            PagingInfo pagingInfo = new PagingInfo(info.PageNumber ?? 1, info.PageSize ?? int.MaxValue - 1, true);
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByEmployee(info, status, pagingInfo, false).OrderBy(p => p.ProductCode).ThenBy(p => p.PlanEndTime).ToList();

            CRT.MainThread.InvokeIfRequired(() =>
            {
                dispatchTaskList.Clear();
                foreach (var item in tasks)
                {
                    dispatchTaskList.Add(item);
                }
                //dispatchTaskList.MarkSaved();
            });
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
            if (labelInfoList.Count == 0)
            {
                showError("没有要提交的数据");
                return;
            }
            try
            {
                if (CRT.MessageService.AskQuestion("确认要提交吗?".L10nFormat(), "确认"))
                {
                    var task = modelScan.DispatchTask;
                    var workstation = modelScan.Workstation;
                    var list = labelInfoList.ToList();
                    PdaScanSubmitInfo submitInfo = new PdaScanSubmitInfo()
                    {
                        ScanType = 1,
                        ResourceId = workstation.ResourceId ?? 0,
                        ProcessId = task.ProcessId ?? 0,
                        DispatchTaskId = task.Id,
                        WorkOrderId = task?.WorkOrderId ?? 0,
                        DetailInfos = labelInfoList.ToList(),
                        ReportEmployeeId = modelScan.ReportEmployeeId ?? 0,
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

                    modelScan.DispatchTask = modelScan.LoadTask(modelScan.DispatchTaskId);
                    var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(modelScan.DispatchTask, SIE.MES.TaskManagement.Reports.Enums.SourceType.Report_Manual);
                    modelScan.MaxRemainQty = tuple.Item2;
                    modelScan.ProcessMaxRemainQty = tuple.Item2;

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
                    var task = modelScan.DispatchTask;
                    var workstation = modelScan.Workstation;
                    if (workstation == null || workstation.ResourceId == null || workstation.ProcessId == null || workstation.StationId == null)
                        throw new ValidationException("请先选择工作单元");
                    string barcode = this.txtBarcode.Text;
                    //modelScan.Barcode = barcode;

                    if (labelInfoList.Any(p => p.Sn == barcode))
                        throw new ValidationException("[{0}]已在明细列表,请勿重复扫描".L10nFormat(barcode));

                    var scanInfo = new PdaScanInfo()
                    {
                        Sn = barcode,
                        ScanType = ScanType,
                        IsFirstSn = labelInfoList.Count == 0,
                        DispatchTaskId = task?.Id,
                        WorkOrderId = task?.WorkOrderId,
                        ResourceId = workstation.ResourceId ?? 0,
                        ProcessId = workstation.ProcessId ?? 0,
                        StationId = workstation.StationId ?? 0,
                    };
                    var ret = RT.Service.Resolve<ReportController>().CheckScanInfo(scanInfo);
                    labelInfoList.Add(new ScanDetailInfo()
                    {
                        Sn = barcode,
                        Qty = ret.LabelQty,
                        GoodQty = ret.LabelQty,
                        SuspectQty = 0
                    });
                    if (ret.DispatchTaskId > 0)
                    {
                        modelScan.DispatchTask = modelScan.LoadTask(ret.DispatchTaskId);
                        modelScan.MaxRemainQty = ret.MaxRemainQty;
                        modelScan.ProcessMaxRemainQty = ret.ProcessMaxRemainQty;
                        if (modelScan.DispatchTask != null)
                        {
                            //获取工序分单数量
                            if (modelScan.DispatchTask.WorkOrder != null)
                            {
                                var layoutInfo = modelScan.DispatchTask.WorkOrder.LayoutInfoList.Where(p => p.ProcessCode == modelScan.DispatchTask.ProcessCode).FirstOrDefault();
                                modelScan.Zcode = layoutInfo?.Zcode ?? 0;
                            }
                        }
                    }

                    showTip("[{0}]扫描成功".L10nFormat(barcode));
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
            modelScan.DispatchTask = null;
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
            if ((sender as RadioButton)?.Content.ToString() == "报工")
            {
                ScanType = 1;
            }
            else if ((sender as RadioButton)?.Content.ToString() == "转入")
            {
                ScanType = 2;
            }
            reset();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void txtGoodQty_TouchUp(object sender, TouchEventArgs e)
        {
            showGoodQtyInput(sender);
        }

        private void txtGoodQty_MouseUp(object sender, MouseButtonEventArgs e)
        {
            showGoodQtyInput(sender);
        }

        private void SuspectQty_TouchUp(object sender, TouchEventArgs e)
        {
            showSuspectQtyInput(sender);
        }

        private void SuspectQty_MouseUp(object sender, MouseButtonEventArgs e)
        {
            showSuspectQtyInput(sender);
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
                        data.SuspectQty = value;
                        textBox.Content = value.ToString();
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
            kZReportHelper.ShowViewTaskList(modelScan.Workstation.ResourceId, modelScan.Workstation.ProcessId);
        }

        /// <summary>
        /// 安灯管理a
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAndon_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowViewAndon(modelScan.Workstation);
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTaskList();
        }
    }
}
