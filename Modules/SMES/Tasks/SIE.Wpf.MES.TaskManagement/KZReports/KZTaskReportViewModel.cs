using DevExpress.Xpf.Editors;
using Microsoft.Scripting.Utils;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PreStartupSetupRecords;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.ObjectModel;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SIE.Wpf.MES.TaskManagement.KZReports
{
    /// <summary>
    /// 生产报工 视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产报工")]
    public class KZTaskReportViewModel : KZTaskReportViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KZTaskReportViewModel()
        {
            IotMode = IotMode.Normal;
        }


        #region 换轴米数 AxisQty
        /// <summary>
        /// 换轴米数
        /// </summary>
        [Label("换轴米数")]
        public static readonly Property<decimal?> AxisQtyProperty = P<KZTaskReportViewModel>.Register(e => e.AxisQty);

        /// <summary>
        /// 换轴米数
        /// </summary>
        public decimal? AxisQty
        {
            get { return this.GetProperty(AxisQtyProperty); }
            set { this.SetProperty(AxisQtyProperty, value); }
        }
        #endregion

        #region 计米数量 MeterCount
        /// <summary>
        /// 计米数量
        /// </summary>
        [Label("计米数量")]
        public static readonly Property<decimal?> MeterCountProperty = P<KZTaskReportViewModel>.Register(e => e.MeterCount);

        /// <summary>
        /// 计米数量
        /// </summary>
        public decimal? MeterCount
        {
            get { return this.GetProperty(MeterCountProperty); }
            set { this.SetProperty(MeterCountProperty, value); }
        }
        #endregion

        #region 剩余报工数量 RemainQty
        /// <summary>
        /// 剩余报工数量
        /// </summary>
        [Label("剩余报工数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<KZTaskReportViewModel>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余报工数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 实际报工数 ActualReportQty
        /// <summary>
        /// 实际报工数
        /// </summary>
        [Label("实际报工数")]
        public static readonly Property<decimal> ActualReportQtyProperty = P<KZTaskReportViewModel>.RegisterReadOnly(
            e => e.ActualReportQty, e => e.GetActualReportQty(), DispatchTaskIdProperty);
        /// <summary>
        /// 实际报工数
        /// </summary>

        public decimal ActualReportQty
        {
            get { return this.GetProperty(ActualReportQtyProperty); }
        }
        private decimal GetActualReportQty()
        {
            if (DispatchTask == null)
                return 0;
            return DispatchTask.ReportQty + DispatchTask.SuspectQty;
        }
        #endregion

        #region 报工进度 ReportProgress
        /// <summary>
        /// 报工进度
        /// </summary>
        [Label("报工进度")]
        public static readonly Property<decimal> ReportProgressProperty = P<KZTaskReportViewModel>.RegisterReadOnly(
            e => e.ReportProgress, e => e.GetReportProgress(), DispatchTaskIdProperty);
        /// <summary>
        /// 报工进度
        /// </summary>

        public decimal ReportProgress
        {
            get { return this.GetProperty(ReportProgressProperty); }
        }
        private decimal GetReportProgress()
        {
            if (DispatchTask == null || DispatchTask.DispatchQty == 0)
                return 0;
            return Math.Round(ActualReportQty * 100 / DispatchTask.DispatchQty, 2);
        }
        #endregion

        #region IOT产出数 IotQty
        /// <summary>
        /// IOT产出数
        /// </summary>
        [Label("IOT产出数")]
        public static readonly Property<decimal> IotQtyProperty = P<KZTaskReportViewModel>.RegisterReadOnly(
            e => e.IotQty, e => e.GetIotQty(), DispatchTaskProperty);
        /// <summary>
        /// IOT产出数
        /// </summary>

        public decimal IotQty
        {
            get { return this.GetProperty(IotQtyProperty); }
        }
        private decimal GetIotQty()
        {
            if (DispatchTask == null)
                return 0;
            return DispatchTask.IotQty + DispatchTask.ManualReportQty;
        }
        #endregion

        #region IOT良品数(当批) IotOkQty
        /// <summary>
        /// IOT良品数(当批)
        /// </summary>
        [Label("IOT良品数")]
        public static readonly Property<decimal> IotOkQtyProperty = P<KZTaskReportViewModel>.RegisterReadOnly(
            e => e.IotOkQty, e => e.GetIotOkQty(), IotQtyProperty, SuspectQtyProperty);
        /// <summary>
        /// IOT良品数
        /// </summary>

        public decimal IotOkQty
        {
            get { return this.GetProperty(IotOkQtyProperty); }
        }
        private decimal GetIotOkQty()
        {
            if (DispatchTask == null)
                return 0;
            var MaxReportQty = RT.Service.Resolve<DispatchController>().MaxReportQty(DispatchTask);
            //（设备产量-已报工数量（良品+报废品数量+返工品数量）-可疑品数量）- 当批可疑品数 
            var outputQty = IotQty > MaxReportQty ? MaxReportQty : IotQty;//IotQty > DispatchTask.MaxReportQty ? DispatchTask.MaxReportQty : IotQty;  //不允许超最大容差
            var qty = outputQty - DispatchTask.ReportQty - DispatchTask.SuspectQty - SuspectQty;
            if (qty < 0) qty = 0;
            return qty;
        }
        #endregion

        #region 累计合格数 TotalOkQty
        /// <summary>
        /// 累计合格数
        /// </summary>
        [Label("累计合格数")]
        public static readonly Property<decimal> TotalOkQtyProperty = P<KZTaskReportViewModel>.Register(e => e.TotalOkQty);

        /// <summary>
        /// 累计合格数
        /// </summary>
        public decimal TotalOkQty
        {
            get { return this.GetProperty(TotalOkQtyProperty); }
            set { this.SetProperty(TotalOkQtyProperty, value); }
        }
        #endregion

        #region 累计不合格数 TotalNgQty
        /// <summary>
        /// 累计不合格数
        /// </summary>
        [Label("累计不合格数")]
        public static readonly Property<decimal> TotalNgQtyProperty = P<KZTaskReportViewModel>.Register(e => e.TotalNgQty);

        /// <summary>
        /// 累计不合格数
        /// </summary>
        public decimal TotalNgQty
        {
            get { return this.GetProperty(TotalNgQtyProperty); }
            set { this.SetProperty(TotalNgQtyProperty, value); }
        }
        #endregion


        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void Onload()
        {
            if (kZReportHelper == null)
                kZReportHelper = new KZReportHelper(this);
            kZReportHelper.ShowReportMain();
            Reset(true);

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            StopIOTReportTimer();
            ReportTimer?.Dispose();
            ReportTimer = null;
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="isClearEmployee"></param>
        public virtual void Reset(bool isClearEmployee)
        {
            Error = null;
            Tips = isClearEmployee ? "请扫描员工编码".L10N() : "请扫描任务单号".L10N();
            FocuseBarcode();

            //重置界面数据
            this.ResetInfo(isClearEmployee);

            if (Printer.IsNullOrEmpty() && IsLocalPrint)
                Printer = SIE.Common.Properties.Settings.Default.PrinterName;
        }

        /// <summary>
        /// 重置界面信息
        /// </summary>
        /// <param name="isClearEmployee"></param>
        public virtual void ResetInfo(bool isClearEmployee)
        {
            ReportEmployeeId = isClearEmployee ? null : ReportEmployeeId;
            ReportEmployee = isClearEmployee ? null : ReportEmployee;
            DispatchTaskId = null;
            DispatchTask = null;
            TotalOkQty = 0;
            TotalNgQty = 0;
            OkQty = 0;
            NgQty = 0;
            IotEntity = null;
        }

        #region 开工

        /// <summary>
        /// 开工
        /// </summary>
        public override async System.Threading.Tasks.Task StartWork()
        {
            try
            {
                #region 开工前校验

                if (ReportTimer?.Enabled == true)
                {
                    if (CRT.MessageService.AskQuestion("任务单【{0}】已开工,是否需要暂停!".L10nFormat(DispatchTask?.No), "提示"))
                    {
                        PauseWork();
                    }
                    return;
                }

                if (IsLocalPrint && Printer.IsNullOrEmpty())
                    throw new ValidationException("当前资源为本地打印模式,请先选择打印机".L10nFormat());

                if (IsRemotePrint && Printer.IsNullOrEmpty())
                    throw new ValidationException("当前资源为远程打印模式,但未配置远程打印机,请联系管理员处理".L10nFormat());

                if (IotMode != IotMode.CommonMode)
                {
                    var group = DispatchTaskQueueList.GroupBy(p => p.Seq);
                    if (group.Any(p => p.Count() > 1))
                        throw new ValidationException("生产队列中序号存在重复,请确认");
                }

                LoadModel();

                LoadFirstQueueTask();

                if (DispatchTask == null)
                {
                    throw new ValidationException("任务单数据异常,请确认".L10N());
                }
                if (DispatchTask.TaskStatus == DispatchTaskStatus.Finished)
                {
                    StartNextTask();
                    return;
                }

                var states = new List<DispatchTaskStatus>() { DispatchTaskStatus.Dispatched, DispatchTaskStatus.Executing, DispatchTaskStatus.Pause };
                if (!states.Contains(DispatchTask.TaskStatus))
                    throw new ValidationException("任务单状态为[{0}]才能进行开工,请确认".L10nFormat(states.Select(p => p.ToLabel()).Concat("、")));

                LoadTaskList();
                if (DispatchTaskList.Any(p => p.Id != DispatchTask.Id && p.TaskStatus == DispatchTaskStatus.Executing))
                {
                    throw new ValidationException("当前资源有其他任务单正在执行中,请先将其暂停后再操作!".L10nFormat());
                }

                if (DispatchTaskQueue.Seq == 10)    //首个生产任务需要做产前准备校验
                {
                    //校验产前准备
                    RT.Service.Resolve<ProcessPrepareRecordsController>().ValidateProcessPrepare(DispatchTask);
                    //校验开机准备
                    RT.Service.Resolve<PreStartupSetupRecordsController>().ValidateStartupSetupPrepare(DispatchTask);
                }
                #endregion

                var task = RT.Service.Resolve<ReportController>().StartIOTWorkTask(DispatchTask, Resource);

                if (task.IotStatus != IotStatus.Executing)
                {
                    ShowMessage("任务单【{0}】IOT状态异常,无法开工!".L10nFormat(DispatchTask.No));
                    return;
                }

                ClearInfos();

                ShowInstantMessage("任务单【{0}】开工成功!".L10nFormat(DispatchTask.No), "提示", 3);

                await System.Threading.Tasks.Task.Delay(3000);

                DispatchTask = task;
                SetParShortDescription(DispatchTask);

                //开启定时器
                StartIOTReportTimer();
                ClearInfos();

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void PauseWork()
        {
            if (PauseDispatchTask(DispatchTask, Resource))
                StopIOTReportTimer();
        }

        /// <summary>
        /// 检查是否达到报工条件, 如果达到报工条件, 执行报工
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public virtual void CheckReport()
        {
            var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(DispatchTask);
            var MaxReportQty = tuple.Item1;
            MaxRemainQty = tuple.Item2;

            if (DispatchTask.IotQty >= MaxReportQty/*DispatchTask.MaxReportQty*/ || MaxRemainQty/*DispatchTask.MaxRemainQty*/ == 0)
            {
                if (MaxRemainQty/*DispatchTask.MaxRemainQty*/ > 0)
                {

                    var qty = MaxRemainQty/*DispatchTask.MaxRemainQty*/ - SuspectQty;
                    AutoTaskReport(qty, SuspectQty, true);
                    return;
                }
                throw new ValidationException("任务单[{0}]最大剩余可报工数为0,请确认".L10nFormat(DispatchTask.No, DispatchTask.IotQty));
            }
        }

        /// <summary>
        /// 对象锁
        /// </summary>
        private readonly object lockObj = new object();
        /// <summary>
        /// 定时报工事件
        /// </summary>
        public override void ReportTimerElapsed()
        {
            lock (lockObj)
            {
                try
                {
                    ClearInfos();
                    DispatchTask = RT.Service.Resolve<ReportController>().GetIOTWorkTask(DispatchTask, Resource);
                    SetParShortDescription(DispatchTask);

                    if (DispatchTask.TaskStatus == DispatchTaskStatus.Finished)
                    {
                        StartNextTask();
                        return;
                    }

                    if (Zcode <= 0 || DispatchTask.RemainQty == 0)
                        return;
                    var qty = IotOkQty + SuspectQty;

                    var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(DispatchTask);
                    var MaxReportQty = tuple.Item1;
                    MaxRemainQty = tuple.Item2;

                    if (SuspectQty > MaxRemainQty)
                        throw new ValidationException("可疑品数量已超过剩余可报工数");

                    bool isFinish = DispatchTask.IotQty >= DispatchTask.DispatchQty;
                    if (isFinish && qty > 0)
                    {
                        if (DispatchTask.IotQty >= MaxReportQty/*DispatchTask.MaxReportQty*/)
                        {
                            //超出最大报工数,强制报工
                            CRT.MainThread.InvokeIfRequired(() =>
                            {
                                qty = MaxRemainQty - SuspectQty;
                                AutoTaskReport(qty, SuspectQty, true);
                            });
                            return;
                        }
                    }

                    int count = (int)(qty / Zcode);
                    if (count <= 0)
                        return;

                    var okQty = Zcode * count - SuspectQty;
                    CRT.MainThread.InvokeIfRequired(() =>
                    {
                        AutoTaskReport(okQty, SuspectQty, true);
                    });

                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }

        #endregion

        #region 报工/异常报工

        /// <summary>
        /// 自动报工
        /// </summary>
        /// <param name="okQty"></param>
        /// <param name="suspectQty"></param>
        /// <param name="isIotReport"></param>
        public virtual void AutoTaskReport(decimal okQty, decimal suspectQty, bool isIotReport = false)
        {
            List<PdaPrintInfo> printInfos = new List<PdaPrintInfo>();
            Exception exception = null;
            var win = new WaitDialog();
            win.Width = 300;
            win.WindowStyle = WindowStyle.None;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Topmost = true;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "正在报工...".L10N();
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    StopIOTReportTimer();
                    printInfos = TaskReport(okQty, suspectQty, isIotReport);
                }
                catch (Exception exc)
                {
                    exception = exc;
                }

                Action ac = () =>
                {
                    win.DialogResult = true;
                    //打印标签
                    if (printInfos.Count > 0)
                        PrintLabels(printInfos, true);
                };
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();
            if (exception != null)
            {
                exception.Alert();
                StartIOTReportTimer();
            }
            else
            {
                ShowInstantMessage("报工成功".L10N(), "提示", 3);

                StartNextTask();
            }
        }

        /// <summary>
        /// 完工后自动切换下一任务开工
        /// </summary>
        public virtual async void StartNextTask()
        {
            if (DispatchTask.TaskStatus == DispatchTaskStatus.Finished /*|| DispatchTask.IotQty >= DispatchTask.MaxReportQty*/)
            {
                StopIOTReportTimer();
                RT.Service.Resolve<ReportController>().FinishIotStatus(DispatchTask, Resource);

                //完工任务队列
                var queueIds = new List<double>() { DispatchTaskQueue.Id };
                var ret = RT.Service.Resolve<DispatchController>().FinishQueueTask(queueIds);
                DispatchTaskQueue = null;
                DispatchTask = null;

                var queues = RT.Service.Resolve<DispatchController>().GetDispatchTaskQueueList(Resource.Id, false, new EagerLoadOptions().LoadWithViewProperty());

                LoadTaskQueueList(queues);

                if (queues.Count == 0)
                {
                    ShowMessage("生产队列中所有任务单已经完工，请确认");
                }
                else
                {
                    ShowInstantMessage("当前任务已完工,将自动切换下个生产任务", "提示", 3);

                    await System.Threading.Tasks.Task.Delay(3000);

                    await StartWork();    //队列中还有任务,则继续生产
                }
            }
        }

        /// <summary>
        /// 异常报工
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public override void ExceptionReport()
        {
            ////调试
            //testPrint();
            //return;

            if (DispatchTask == null)
                throw new ValidationException("请先选择一个任务单");

            if (IotOkQty <= 0 && SuspectQty <= 0)
                throw new ValidationException("没有要提交报工的数量");

            if (CRT.MessageService.AskQuestion("确认要报工良品数[{0}]可疑品数[{1}]吗?".L10nFormat(IotOkQty, SuspectQty), "确认"))
            {
                AutoTaskReport(IotOkQty, SuspectQty);

                StopIOTReportTimer();
            }

        }

        #endregion

    }
}
