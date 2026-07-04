using DevExpress.Xpf.Editors;
using SIE.Barcodes.WipBatchs;
using SIE.Common.NumberRules;
using SIE.Core.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Pressure;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Wpf.Controls.WaitProgress;
using SIE.Wpf.MES.WIP.Pressure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.TaskManagement.Pressure
{
    /// <summary>
    /// 条码打印(KZ)
    /// </summary>
    [RootEntity, Serializable]
    [Label("条码打印(KZ)")]
    public class PressureSnPrintViewModel : WIP.Pressure.PressureViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PressureSnPrintViewModel()
        {

        }

        #region 任务单 DispatchTask
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<PressureSnPrintViewModel>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return (double?)this.GetRefNullableId(DispatchTaskIdProperty); }
            set { this.SetRefNullableId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<PressureSnPrintViewModel>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 打印数量 PrintQty
        /// <summary>
        /// 打印数量
        /// </summary>
        [Label("打印数量")]
        public static readonly Property<int> PrintQtyProperty = P<PressureSnPrintViewModel>.Register(e => e.PrintQty);

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintQty
        {
            get { return this.GetProperty(PrintQtyProperty); }
            set { this.SetProperty(PrintQtyProperty, value); }
        }
        #endregion


        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="resetType"></param>
        public override void Reset(WIP.ResetType resetType)
        {
            base.Reset(resetType);

            Resource = Workstation.Resource;
            DispatchTask = null;
            WipPressure = null;
            WipBatch = null;
            WorkOrder = null;
            SnDetailList.Clear();
            //var config = ConfigService.GetConfig(new WipPressureSnConfig(), typeof(PressureViewModel));
            //NumberRule = config.BacodeRule;
            NumberRule = null;
            Template = null;
            PrinterSettingTpl = null;
            if (Printer.IsNullOrEmpty())
                Printer = SIE.Common.Properties.Settings.Default.PrinterName;
            if (Resource == null)
                ShowTips("请扫描资源编码");
            else if (DispatchTask == null)
                ShowTips("请扫描或选择一个派工任务单号");
            else if (WipBatch == null)
                ShowTips("请进行SN条码打印");

            //InitDevicePort();

        }

        /// <summary>
        /// 清空
        /// </summary>
        protected override void ClearInfos()
        {
            base.ClearInfos();

        }
        /// <summary>
        /// 属性变更后处理逻辑
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == DispatchTaskIdProperty)
            {
                ClearInfos();
                WipPressure = null;
                WipBatch = null;
                WorkOrder = null;
                NumberRule = null;
                Template = null;
                CRT.MainThread.InvokeIfRequired(() =>
                {
                    SnDetailList.Clear();
                    FocuseBarcode();
                });
                if (DispatchTask != null)
                {
                    if (DispatchTask.TaskStatus != DispatchTaskStatus.Dispatched && DispatchTask.TaskStatus != DispatchTaskStatus.Executing)
                    {
                        ShowError("当前任务单[{0}]状态为[{1}],请选择其他任务单".L10nFormat(DispatchTask.No, DispatchTask.TaskStatus.ToLabel()));
                        return;
                    }
                    WorkOrder = RF.GetById<WorkOrder>(DispatchTask.WorkOrderId);
                    var wipPressureTpl = RT.Service.Resolve<WipPressureController>().GetWipPressurePrintTemplate(WorkOrder.ProductId);
                    NumberRule = wipPressureTpl?.NumberRule;
                    Template = wipPressureTpl?.PrintTemplate;
                    PrinterSettingTpl = RT.Service.Resolve<PrinterSettingController>().GetPrinterSettingTpl(RT.IdentityId, WorkOrder?.Product.Code);
                    if (NumberRule == null || Template == null)
                    {
                        ShowError("产品[{0}]还未维护对应的SN编码规则与打印模板,请检查".L10nFormat(WorkOrder.Product?.Code));
                        return;
                    }
                    ShowTips("请进行SN条码打印");
                }
                else
                {
                    ShowTips("请扫描或选择一个派工任务单号");
                }
            }
            else if (e.Property == WipPressureProperty)
            {
                CRT.MainThread.InvokeIfRequired(() =>
                {
                    SnDetailList.Clear();
                    SnDetailList.MarkSaved();
                    if (WipPressure != null)
                    {
                        SnDetailList.AddRange(WipPressure.WipPressureSnList);
                    }
                });
            }
        }

        /// <summary>
        /// 条码扫描后处理逻辑
        /// </summary>
        /// <param name="e">1</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty())
            {
                return;
            }
            try
            {
                ClearInfos();

                if (!ValidateCollect())
                    return;
                //扫描资源
                if (Resource == null)
                {
                    Resource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(Barcode);
                    if (Resource == null)
                        ShowError("请扫描正确的资源编码".L10nFormat(Barcode));
                    else
                        ShowTips("请扫描或选择一个派工任务单号");
                    Workstation.Resource = Resource;
                    return;
                }
                //扫描任务单
                if (DispatchTask == null)
                {
                    ScanDispatchTask(Barcode);
                    return;
                }
                else
                {
                    if (DispatchTask.TaskStatus != DispatchTaskStatus.Dispatched && DispatchTask.TaskStatus != DispatchTaskStatus.Executing)
                    {
                        if (DispatchTask.No != Barcode)
                        {
                            ScanDispatchTask(Barcode);
                            return;
                        }
                        ShowError("当前任务单[{0}]状态为[{1}],请选择其他任务单".L10nFormat(DispatchTask.No, DispatchTask.TaskStatus.ToLabel()));

                        return;
                    }
                }

                ShowTips("请进行SN条码打印");

            }
            catch (Exception exc)
            {
                ShowError(exc.GetExceptionMessage());
            }
            finally
            {
                Barcode = null;
                FocuseBarcode();
            }
        }

        void ScanDispatchTask(string barcode)
        {
            DispatchTask = null;
            var task = RT.Service.Resolve<DispatchController>().GetDispatchTask(barcode);
            if (task == null)
            {
                ShowError("派工任务单不存在");
                return;
            }
            DispatchTask = task;
        }

        /// <summary>
        /// 显示打印SN窗口
        /// </summary>
        public void ShowPrintSnWin()
        {
            if (WipPressure != null)
            {
                if (!CRT.MessageService.AskQuestion("当前打印操作已完成, 是否需要继续打印".L10nFormat(), "确认"))
                {
                    return;
                }
                WipPressure = null;
                WipBatch = null;
                SnDetailList.Clear();
            }
            if (DispatchTask.TaskStatus != DispatchTaskStatus.Dispatched && DispatchTask.TaskStatus != DispatchTaskStatus.Executing)
            {
                ShowError("当前任务单[{0}]状态为[{1}],请选择其他任务单".L10nFormat(DispatchTask.No, DispatchTask.TaskStatus.ToLabel()));
                return;
            }

            if (!ValidateCollect())
                return;
            if (DispatchTask == null)
                return;
            DispatchTask = RT.Service.Resolve<DispatchController>().GetDispatchTask(DispatchTask.Id);
            var maxPrintCount = RT.Service.Resolve<WipPressureController>().GetMaxPrintCount(DispatchTask.DispatchQty);
            if (DispatchTask.PrintedQty >= maxPrintCount)
            {
                ShowError("当前任务单打印数量已超出最大允许打印数量");
                return;
            }
            var maxPrintQty = (int)(DispatchTask.DispatchQty - DispatchTask.PrintedQty);
            PrintQty = maxPrintQty;

            var template = new DetailsUITemplate(typeof(PressureSnPrintViewModel), PressureSnPrintViewModelViewConfig.PrintSnView);
            var ui = template.CreateUI();
            ui.MainView.Data = this;
            var textEdit = ui.MainView.LayoutControl.GetLogicalChild<TextEdit>();
            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
            {
                w.Title = "打印数量".L10N();
                w.Height = 200;
                w.Width = 400;
                w.Closing += async (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (PrintQty > maxPrintCount)
                        {
                            CRT.MessageService.ShowError("打印数量不能超过[{0}]!".L10nFormat(maxPrintCount));
                            PrintQty = (int)maxPrintCount;
                            e.Cancel = true;
                            return;
                        }

                        EntityList<WipPressure> wipPressures = RT.Service.Resolve<WipPressureController>().GetWipPressuresByWoNos(new System.Collections.Generic.List<string>() { DispatchTask.WorkOrderNo });
                        if (wipPressures.Sum(p => p.Qty) + PrintQty > maxPrintCount)
                        {
                            CRT.MessageService.ShowError("总打印数量不能超过[{0}]!".L10nFormat(maxPrintCount));
                            PrintQty = (int)maxPrintCount;
                            e.Cancel = true;
                            return;
                        }

                        bool isAllowOver = true;
                        if (PrintQty > maxPrintQty)
                        {
                            if (!CheckAllowOver())
                            {
                                e.Cancel = true;
                                return;
                            }
                        }

                        //生成SN数据
                        GenerateAndPrintSns(isAllowOver);
                    }
                };

                CRT.MainThread.InvokeIfRequired(() =>
                {
                    textEdit?.Focus();
                });
            });
        }

        /// <summary>
        /// 是否可以继续测试
        /// </summary>
        /// <returns></returns>
        protected bool CheckAllowOver()
        {
            bool IsVerifyCode = false;
            if (CRT.MessageService.AskQuestion("打印数量已超过任务单计划数量, 是否需要继续打印".L10nFormat(), "确认"))
            {
                VerifyCode = null;
                var template = new DetailsUITemplate(typeof(PressureViewModel), PressureViewModelViewConfig.VerifyCodeView);
                var ui = template.CreateUI();
                ui.MainView.Data = this;
                var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
                {
                    w.Title = "超打验证码".L10N();
                    w.Height = 200;
                    w.Width = 400;
                    w.Closing += (s, e) =>
                    {
                        if (w.Result == 0)
                        {
                            if (RT.Service.Resolve<WipPressureController>().VerifyCode(null, VerifyCode))
                            {
                                Error = null;
                                IsVerifyCode = true;
                            }
                            else
                            {
                                CRT.MessageService.ShowError("验证码不正确!");
                                e.Cancel = true;
                            }
                        }
                    };
                });

                if (result == 0 && IsVerifyCode == true)
                    return true;

            }

            return false;
        }


        void GenerateAndPrintSns(bool isAllowOver)
        {
            WipPressure wipPressure;
            //生成SN数据
            ShowTips("正在生成工序标签批次...");

            Exception exception = null;
            var win = new WaitDialog();
            win.Width = 300;
            win.WindowStyle = System.Windows.WindowStyle.None;
            win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            win.Topmost = true;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "正在生成工序标签批次...".L10N();
            Task.Run(() =>
            {
                try
                {
                    wipPressure = RT.Service.Resolve<PressureSnPrintController>().GenerateWipPressureSns(PrintQty, DispatchTask, isAllowOver, NumberRule.Id, Resource);
                    if (wipPressure == null)
                    {
                        throw new ValidationException("工序标签批次生成失败");
                    }
                    WipPressure = wipPressure;
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();
            if (exception != null)
            {
                ClearInfos();
                ShowError(exception.GetExceptionMessage());
                exception.Alert();
            }
            else
            {
                ShowTips("已生成工序标签[{0}], 开始打印SN...".L10nFormat(WipPressure.BatchNo));
                PrintSn(WipPressure.WipPressureSnList);
            }
        }
    }
}
