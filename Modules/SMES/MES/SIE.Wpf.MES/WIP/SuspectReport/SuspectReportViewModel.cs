using DevExpress.Xpf.Editors;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.PackingQC;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.Controls.WaitProgress;
using SIE.Wpf.MES.WIP.Pressure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace SIE.Wpf.MES.WIP.SuspectReport
{
    /// <summary>
    /// 可疑品报工
    /// </summary>
    [RootEntity]
    [Label("可疑品报工")]
    public class SuspectReportViewModel : KZDataCollectionViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public SuspectReportViewModel(KZWorkstation _KZWorkstation = null, string _process = null)
        {
            KZWorkstation.Employee = _KZWorkstation?.Employee;
            KZWorkstation.Resource = _KZWorkstation?.Resource;
            KZWorkstation.Process = _KZWorkstation?.Process;
            KZWorkstation.Station = _KZWorkstation?.Station;

            process = _process;
            ProcessList = _process.Split(',').ToList();

            Printer = SIE.Common.Properties.Settings.Default.PrinterName;
        }
        /// <summary>
        /// 工序
        /// </summary>
        string process;
        /// <summary>
        /// 工序
        /// </summary>
        public List<string> ProcessList { get; set; }

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public SuspectProductLabel SuspectLabel { get; set; }

        #region 任务单 DispatchTask
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<SuspectReportViewModel>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

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
            P<SuspectReportViewModel>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> SuspectQtyProperty = P<SuspectReportViewModel>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 是否SN码 IsSn
        /// <summary>
        /// 是否SN码
        /// </summary>
        [Label("是否SN码")]
        public static readonly Property<bool> IsSnProperty = P<SuspectReportViewModel>.Register(e => e.IsSn);

        /// <summary>
        /// 是否SN码
        /// </summary>
        public bool IsSn
        {
            get { return this.GetProperty(IsSnProperty); }
            set { this.SetProperty(IsSnProperty, value); }
        }
        #endregion

        #region SN WipPressureSn
        /// <summary>
        /// SNId
        /// </summary>
        [Label("SN")]
        public static readonly IRefIdProperty WipPressureSnIdProperty =
            P<SuspectReportViewModel>.RegisterRefId(e => e.WipPressureSnId, ReferenceType.Normal);

        /// <summary>
        /// SNId
        /// </summary>
        public double? WipPressureSnId
        {
            get { return (double?)this.GetRefNullableId(WipPressureSnIdProperty); }
            set { this.SetRefNullableId(WipPressureSnIdProperty, value); }
        }

        /// <summary>
        /// SN
        /// </summary>
        public static readonly RefEntityProperty<WipPressureSn> WipPressureSnProperty =
            P<SuspectReportViewModel>.RegisterRef(e => e.WipPressureSn, WipPressureSnIdProperty);

        /// <summary>
        /// SN
        /// </summary>
        public WipPressureSn WipPressureSn
        {
            get { return this.GetRefEntity(WipPressureSnProperty); }
            set { this.SetRefEntity(WipPressureSnProperty, value); }
        }
        #endregion

        #region 批次标签 WipBatch
        /// <summary>
        /// 批次标签Id
        /// </summary>
        [Label("批次标签")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<SuspectReportViewModel>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

        /// <summary>
        /// 批次标签Id
        /// </summary>
        public double? WipBatchId
        {
            get { return (double?)this.GetRefNullableId(WipBatchIdProperty); }
            set { this.SetRefNullableId(WipBatchIdProperty, value); }
        }

        /// <summary>
        /// 批次标签
        /// </summary>
        public static readonly RefEntityProperty<WipBatch> WipBatchProperty =
            P<SuspectReportViewModel>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 批次标签
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region 模板 Template 
        /// <summary>
        /// 模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty = P<SuspectReportViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<SuspectReportViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 打印机 Printer 
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<SuspectReportViewModel>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }
        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == SuspectQtyProperty && e.Source == ManagedPropertyChangedSource.FromUIOperating)
            {
                if (IsSn)
                    SuspectQty = 1;
                else
                {
                    if (SuspectQty > (WipBatch?.Qty ?? 0))
                        SuspectQty = (WipBatch?.Qty ?? 0);
                }
            }
        }

        /// <summary>
        /// 条码变更
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty()) return;
            ClearInfos();
            try
            {
                var packingDetail = RT.Service.Resolve<PackingQcController>().IsExistPackingDetail(Barcode);
                if (packingDetail != null)
                    throw new ValidationException("标签[{0}]已包装确认不允许报可疑品".L10nFormat(Barcode));

                WipPressureSn = RT.Service.Resolve<WipPressureController>().GetWipPressureSn(Barcode);
                if (WipPressureSn == null)
                {
                    WipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(Barcode);
                    if (WipBatch == null)
                        throw new ValidationException("耐压标签/工序标签[{0}]不存在".L10nFormat(Barcode));

                    if (WipBatch.IsSuspectProduct == YesNo.Yes)
                        throw new ValidationException("工序标签[{0}]已经是可疑品标签,请勿重复扫描".L10nFormat(Barcode));
                }
                else
                {
                    WipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(WipPressureSn.WipPressure?.BatchNo);
                    if (WipBatch == null)
                        throw new ValidationException("耐压标签[{0}]对应的工序标签不存在".L10nFormat(Barcode));

                    if (WipPressureSn.IsSuspectProduct)
                        throw new ValidationException("耐压标签[{0}]已经是可疑品标签,请勿重复扫描".L10nFormat(Barcode));
                }

                IsSn = WipPressureSn != null;
                var woId = WipPressureSn != null ? WipPressureSn.WipPressure.WorkOrderId ?? 0 : WipBatch.WorkOrderId;
                WorkOrder = RF.GetById<WorkOrder>(woId, new EagerLoadOptions().LoadWithViewProperty());
                if (WorkOrder == null)
                    throw new ValidationException("标签[{0}]对应的工单不存在".L10nFormat(Barcode));

                var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { woId });

                var taskList = new List<DispatchTask>();
                if (process.Contains("包装"))
                {
                    taskList = tasks.Where(p => p.WorkOrderId == woId && (p.ProcessCode.Contains("包装") || p.ProcessName.Contains("包装"))).OrderBy(p => p.PlanBeginTime).ToList();
                }
                else
                {
                    taskList = tasks.Where(p => p.WorkOrderId == woId && (ProcessList.Contains(p.ProcessCode) || ProcessList.Contains(p.ProcessName)) /*&& (p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched)*/).OrderBy(p => p.PlanBeginTime).ToList();
                }
                if (KZWorkstation.ResourceId > 0)
                    taskList = taskList.Where(p => p.ResourceId == KZWorkstation.ResourceId).ToList();
                if (taskList.Count == 0)
                    throw new ValidationException("当前资源未匹配到对应的[{0}]任务单".L10nFormat(process));
                DispatchTask = taskList.OrderByDescending(p => p.TaskStatus).FirstOrDefault();
                //校验前置工序报工
                RT.Service.Resolve<ReportController>().ValidatePrepareProcessHasReport(WipBatch, DispatchTask, false);


                SuspectQty = IsSn ? 1 : WipBatch?.Qty ?? 1;

                ShowTips("标签[{0}]扫描成功,请确认可疑品数量后提交".L10nFormat(Barcode));
            }
            catch (Exception ex)
            {
                Reset();
                ShowError(ex.GetBaseException().Message);
            }
            finally
            {
                Barcode = null;
                //FocuseBarcode();
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            WorkOrder = null;
            DispatchTask = null;
            WipBatch = null;
            WipPressureSn = null;
            SuspectLabel = null;
            SuspectQty = 0;
            FocuseBarcode();
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Submit()
        {
            try
            {
                if (WipBatch == null && WipPressureSn == null)
                    throw new ValidationException("请先扫描耐压标签/工序标签");
                if (SuspectQty == 0)
                    throw new ValidationException("请输入可疑品数量");
                if (!IsSn && SuspectQty > WipBatch.Qty)
                    throw new ValidationException("可疑品数量不能超过标签数量[{0}]".L10nFormat(WipBatch.Qty));

                var submit = CRT.MessageService.AskQuestion("确认要提交可疑品[{0}]吗".L10nFormat(SuspectQty), "确认");
                if (!submit)
                    return;

                var reportInfo = new SubmitPdaReportInfo()
                {
                    SnInfos = new List<ReportSnInfo>() { new ReportSnInfo() { Sn = IsSn ? WipPressureSn.Sn : WipBatch.BatchNo, SuspectQty = SuspectQty } },
                    DispatchTaskId = DispatchTaskId ?? 0,
                    ReportQty = SuspectQty,
                    GoodQty = 0,
                    SuspectQty = SuspectQty,
                    ReportEmployeeId = KZWorkstation.EmployeeId ?? 0,
                    IsValidatePrepare = false,
                    IsTaskFinish = true
                };
                //提交报工
                SuspectLabel = RT.Service.Resolve<ReportController>().SubmitSuspectReportInfo(reportInfo, IsSn);

                ShowTips("提交成功");

                if (!IsSn && SuspectLabel != null)
                    ShowTips("提交成功,已生成可疑品标签[{0}],请打印".L10nFormat(SuspectLabel.BatchNo));

                WorkOrder = null;
                DispatchTask = null;
                WipBatch = null;
                WipPressureSn = null;
                SuspectQty = 0;
            }
            catch (Exception ex)
            {
                ShowError(ex.GetBaseException().Message);
            }
            finally
            {
                FocuseBarcode();
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public void Print()
        {
            var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            Template = dispatchConfig.SuspectLabelTemplate;
            if (Template == null)
            {
                ShowError("请先维护可疑品标签打印模板"); return;
            }
            if (Printer.IsNullOrEmpty())
            {
                ShowError("请选择打印机"); return;
            }
            if (SuspectLabel == null || IsSn)
                return;

            ShowTips("正在打印[{0}]...".L10nFormat(SuspectLabel.BatchNo));


            Exception exception = null;
            var win = new WaitDialog();
            win.Width = 500;
            win.WindowStyle = System.Windows.WindowStyle.None;
            win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            win.Topmost = true;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "正在打印...".L10N();
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(SuspectLabel.BatchNo);
                    //标品模板打印
                    var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(TemplateId.Value);
                    IPrintable printable = new WipBatchPrintable();
                    var report = ReportFactory.Current.GetReportByExtension(Template.Type);
                    report.Print(printable, filePath, Printer, () =>
                    {
                        return new List<WipBatch>() { wipBatch };
                    }, () => { });

                }
                catch (Exception exc)
                {
                    exception = exc;
                }

                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();
            if (exception != null)
            {
                ShowError(exception.GetExceptionMessage());
                exception.Alert();
            }
            else
                ShowTips("打印[{0}]成功".L10nFormat(SuspectLabel.BatchNo));
        }

        /// <summary>
        /// 播放报警
        /// </summary>
        public void SoundPlayer()
        {
            // 播放指定路径的 WAV 文件
            string soundPath = "warning01.wav"; // 替换为你的音频路径
            if (File.Exists(soundPath))
            {
                using (SoundPlayer player = new SoundPlayer(soundPath))
                {
                    player.Play(); // 异步播放（不阻塞程序）
                }
            }
        }
    }
}
