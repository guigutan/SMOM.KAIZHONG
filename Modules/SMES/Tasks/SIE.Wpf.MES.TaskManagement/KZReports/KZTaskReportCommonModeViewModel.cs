using DevExpress.Xpf.Editors;
using Newtonsoft.Json;
using SIE.Core.RedisUtil;
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
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SIE.Wpf.MES.TaskManagement.KZReports
{
    /// <summary>
    /// 生产报工(共模) 视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产报工(共模)")]
    public class KZTaskReportCommonModeViewModel : KZTaskReportViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KZTaskReportCommonModeViewModel()
        {
            IotMode = IotMode.CommonMode;
        }

        #region IOT产出数 IotOuptQty
        /// <summary>
        /// IOT产出数
        /// </summary>
        [Label("IOT产出数")]
        public static readonly Property<string> IotOuptQtyProperty = P<KZTaskReportCommonModeViewModel>.Register(e => e.IotOuptQty);

        /// <summary>
        /// IOT产出数
        /// </summary>
        public string IotOuptQty
        {
            get { return this.GetProperty(IotOuptQtyProperty); }
            set { this.SetProperty(IotOuptQtyProperty, value); }
        }
        #endregion

        /// <summary>
        /// 报工可疑品数量集合
        /// </summary>
        public Dictionary<double, decimal> dicIotSuspectQtys = new Dictionary<double, decimal>();

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
            Reset(ResetType.None);

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
        /// 加载首组共模任务
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public override void LoadFirstQueueTask()
        {
            dicIotSuspectQtys.Clear();
            CRT.MainThread.InvokeIfRequired(() =>
            {
                ReportTaskQueueList.Clear();
                ReportTaskQueueList.MarkSaved();
            });

            if (DispatchTaskQueueList.Count == 0)
            {
                throw new ValidationException("生产队列中没有生产的任务,请确认");
            }

            DispatchTaskQueue = DispatchTaskQueueList.OrderBy(p => p.Seq).FirstOrDefault(p => !p.IsFinished && p.TaskStatus != DispatchTaskStatus.Finished);
            if (DispatchTaskQueue == null)
                throw new ValidationException("生产队列中没有可生产的任务,请确认");

            DispatchTaskId = DispatchTaskQueue.DispatchTaskId;
            //DispatchTask = LoadTask(DispatchTaskQueue.DispatchTaskId);

            var ids = DispatchTaskQueueList.Where(p => p.Seq == DispatchTaskQueue.Seq).Select(p => p.Id).ToList();
            var queueList = RT.Service.Resolve<DispatchController>().GetDispatchTaskQueueList(ids, false, new EagerLoadOptions().LoadWithViewProperty());
            queueList = RT.Service.Resolve<DispatchController>().SetCavityCount(ModelCavityCount, queueList);
            //加载共模组任务
            CRT.MainThread.InvokeIfRequired(() =>
            {
                ReportTaskQueueList.Clear();
                ReportTaskQueueList.AddRange(queueList);
                ReportTaskQueueList.MarkSaved();

                IotOuptQty = ReportTaskQueueList.Select(p => p.IotQty.ToString()).Concat(",");
            });

        }

        /// <summary>
        /// 刷新生产任务数据
        /// </summary>
        public void RefreshReportList()
        {
            var ids = ReportTaskQueueList.Select(x => x.Id).ToList();
            var queueList = RT.Service.Resolve<DispatchController>().GetDispatchTaskQueueList(ids, null, new EagerLoadOptions().LoadWithViewProperty());
            //queueList = RT.Service.Resolve<DispatchController>().SetCavityCount(ModelCavityCount, queueList);
            //加载共模组任务
            CRT.MainThread.InvokeIfRequired(() =>
            {
                ReportTaskQueueList.Clear();
                ReportTaskQueueList.AddRange(queueList);
                ReportTaskQueueList.MarkSaved();

                IotOuptQty = ReportTaskQueueList.Select(p => p.IotQty.ToString()).Concat(",");
            });
            GetIotSuspectQty(queueList);
        }

        /// <summary>
        /// 更新设置可疑品数量
        /// </summary>
        /// <param name="queues"></param>
        public void GetIotSuspectQty(EntityList<DispatchTaskQueue> queues)
        {
            queues.ForEach(p =>
            {
                var queueId = p.Id;
                if (dicIotSuspectQtys.ContainsKey(queueId))
                {
                    p.IotSuspectQty = dicIotSuspectQtys[queueId];
                }
            });
        }
        /// <summary>
        /// 设置可疑品数量
        /// </summary>
        /// <param name="queueId"></param>
        /// <param name="suspectQty"></param>
        public void SetIotSuspectQty(double queueId, decimal suspectQty)
        {
            if (dicIotSuspectQtys.ContainsKey(queueId))
            {
                dicIotSuspectQtys[queueId] = suspectQty;
            }
            else
            {
                dicIotSuspectQtys.Add(queueId, suspectQty);
            }
        }

        /// <summary>
        /// 开工
        /// </summary>
        /// <returns></returns>
        public override async Task StartWork()
        {
            try
            {
                #region 开工前校验

                isReporting = false;

                var taskIds = ReportTaskQueueList.Select(p => p.DispatchTaskId).ToList();
                var taskNos = ReportTaskQueueList.Select(p => p.DispatchTaskNo).ToList();
                if (ReportTimer?.Enabled == true && taskIds.Count > 0)
                {
                    if (CRT.MessageService.AskQuestion("当前任务已开工,是否需要暂停!".L10nFormat(), "提示"))
                    {
                        if (PauseDispatchTasks(taskIds, Resource))
                        {
                            StopIOTReportTimer();
                        }
                    }
                    return;
                }

                LoadModel();

                if (Model.IsNullOrEmpty())
                    throw new ValidationException("模具不为空".L10nFormat());
                if (ModelCavityCount == 0)
                    throw new ValidationException("模具未维护穴位".L10nFormat());

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

                LoadFirstQueueTask();

                if (DispatchTask == null)
                {
                    throw new ValidationException("任务单数据异常,请确认".L10N());
                }
                if (ReportTaskQueueList.Any(p => p.CavityCount == 0))
                {
                    throw new ValidationException("任务单未维护产品穴位".L10nFormat());
                }
                if (ReportTaskQueueList.Any(p => p.TaskStatus == DispatchTaskStatus.Finished))
                {
                    //任何一个任务单完工,就切换下一组任务
                    StartNextTask();
                    return;
                }

                var states = new List<DispatchTaskStatus>() { DispatchTaskStatus.Dispatched, DispatchTaskStatus.Executing, DispatchTaskStatus.Pause };
                if (!states.Contains(DispatchTask.TaskStatus))
                    throw new ValidationException("任务单状态为[{0}]才能进行开工,请确认".L10nFormat(states.Select(p => p.ToLabel()).Concat("、")));

                LoadTaskList();

                if (DispatchTaskList.Any(p => !taskIds.Contains(p.Id) && p.TaskStatus == DispatchTaskStatus.Executing))
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

                taskIds = ReportTaskQueueList.Select(p => p.DispatchTaskId).ToList();
                taskNos = ReportTaskQueueList.Select(p => p.DispatchTaskNo).ToList();
                var tasks = RT.Service.Resolve<ReportController>().StartIOTWorkTask(taskIds, Resource);

                if (tasks.Any(p => p.IotStatus != IotStatus.Executing))
                {
                    ShowTips("任务单【{0}】IOT状态异常,无法开工!".L10nFormat(taskNos.Concat(",")));
                    return;
                }

                CRT.MessageService.ShowInstantMessage("任务单【{0}】开工成功!".L10nFormat(taskNos.Concat(",")), "提示", 3);

                await Task.Delay(3000);

                //开启定时器
                StartIOTReportTimer();

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 完工后自动切换下一组任务开工
        /// </summary>
        public async void StartNextTask()
        {
            var taskIds = ReportTaskQueueList.Select(p => p.DispatchTaskId).ToList();
            var queueIds = ReportTaskQueueList.Select(p => p.Id).ToList();
            var queueList = RT.Service.Resolve<DispatchController>().GetDispatchTaskQueueList(queueIds, false, new EagerLoadOptions().LoadWithViewProperty());

            //只要有一个任务单完工,即切换下一组任务
            if (queueList.Any(p => p.TaskStatus == DispatchTaskStatus.Finished))
            {
                StopIOTReportTimer();

                RT.Service.Resolve<ReportController>().FinishIotStatus(taskIds, Resource);  //IOT完工下发
                RT.Service.Resolve<ReportController>().PauseIOTWorkTask(taskIds, Resource); //暂停其他任务
                //完工任务队列
                var ret = RT.Service.Resolve<DispatchController>().FinishQueueTask(queueIds);

                var queues = RT.Service.Resolve<DispatchController>().GetDispatchTaskQueueList(Resource.Id, false, new EagerLoadOptions().LoadWithViewProperty());

                LoadTaskQueueList(queues);

                if (queues.Count == 0)
                    CRT.MessageService.ShowMessage("生产队列中所有任务单已经完工，请确认");
                else
                {
                    CRT.MessageService.ShowInstantMessage("当前任务组已完工,将自动切换下个生产任务组".L10N(), "提示".L10N(), 3);

                    await Task.Delay(3000);

                    await StartWork();    //队列中还有任务,则继续生产
                }
            }
        }

        /// <summary>
        /// 对象锁
        /// </summary>
        private readonly object lockObj = new object();
        /// <summary>
        /// 报工标识
        /// </summary>
        private bool isReporting = false;
        /// <summary>
        /// 定时报工事件
        /// </summary>
        public override void ReportTimerElapsed()
        {
            lock (lockObj)
            {
                try
                {
                    if (isReporting)
                        return;

                    var taskIds = ReportTaskQueueList.Select(p => p.DispatchTaskId).ToList();
                    RT.Service.Resolve<ReportController>().GetIOTWorkTask(taskIds, Resource);
                    var queueIds = ReportTaskQueueList.Select(p => p.Id).ToList();
                    var queueList = RT.Service.Resolve<DispatchController>().GetDispatchTaskQueueList(queueIds, false, new EagerLoadOptions().LoadWithViewProperty());

                    if (queueList.Any(p => p.TaskStatus == DispatchTaskStatus.Finished))
                    {
                        StartNextTask();
                        return;
                    }
                    GetIotSuspectQty(queueList);

                    var reportDatas = new List<SubmitPdaReportInfo>();
                    foreach (var queue in queueList)
                    {
                        if (queue.TaskStatus == DispatchTaskStatus.Finished)
                            continue;
                        var dispatchTask = LoadTask(queue.DispatchTaskId);
                        var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(dispatchTask);
                        var maxReportQty = tuple.Item1;
                        var maxRemainQty = tuple.Item2;

                        if (maxRemainQty == 0 || queue.Zcode == 0)
                            continue;

                        if (queue.IotSuspectQty > maxRemainQty)
                            throw new ValidationException("任务单[{0}]可疑品数量[{1}]已超过剩余可报工数[{2}]".FormatArgs(queue.DispatchTaskNo, queue.IotSuspectQty, maxRemainQty));

                        decimal zcode = queue.Zcode > 0 ? queue.Zcode : 1;

                        var iotQty = queue.IotQty * queue.CavityCount + queue.ManualReportQty + queue.IotSuspectQty;

                        bool isFinish = iotQty > dispatchTask.DispatchQty;
                        if (isFinish)
                        {
                            if (iotQty >= maxReportQty)
                            {
                                //超出最大报工数,强制报工
                                var suspectQty = queue.IotSuspectQty;
                                var okQty = maxRemainQty - suspectQty;
                                reportDatas.Add(NewReportInfo(queue, okQty, suspectQty, true));
                            }
                        }
                        else
                        {
                            var qty = iotQty - dispatchTask.ActualReportQty;
                            int count = (int)(qty / Zcode);
                            if (count <= 0)
                                continue;

                            var suspectQty = queue.IotSuspectQty;
                            var okQty = Zcode * count - suspectQty;
                            //ShowInstantMessage($"良品[{okQty}],可疑品[{suspectQty}]", "提示", 3);
                            reportDatas.Add(NewReportInfo(queue, okQty, suspectQty, true));
                        }
                    }

                    if (reportDatas.Count > 0)
                    {
                        CRT.MainThread.InvokeIfRequired(() =>
                        {
                            var redisKey = JsonConvert.SerializeObject(reportDatas) + RT.InvOrg;
                            //增加redis锁
                            var tuple = RT.Service.Resolve<RedisUtilController>().Lock(redisKey);
                            if (tuple.Item1)
                            {
                                AutoTaskReport(reportDatas);
                                //解锁
                                RT.Service.Resolve<RedisUtilController>().UnLock(redisKey, tuple.Item2);
                            }
                        });
                    }

                    RefreshReportList();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }

        /// <summary>
        /// 自动报工
        /// </summary>
        /// <param name="reportDatas"></param>
        public virtual async Task AutoTaskReport(List<SubmitPdaReportInfo> reportDatas)
        {
            isReporting = true;
            List<PdaPrintInfo> printInfos = new List<PdaPrintInfo>();
            var errors = new List<string>();
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
                StopIOTReportTimer();
                foreach (var reportInfo in reportDatas)
                {
                    try
                    {
                        var printInfo = RT.Service.Resolve<ReportController>().SubmitPdaReportInfo(reportInfo);
                        printInfos.AddRange(printInfo);
                        if (reportInfo.QueueId > 0)
                            SetIotSuspectQty(reportInfo.QueueId, 0); //重置可疑品数量
                    }
                    catch (Exception exc)
                    {
                        errors.Add(exc.GetBaseException().Message);
                        exception = exc;
                    }
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
            isReporting = false;

            if (errors.Count > 0)
            {
                ShowError(errors.Concat("; \r\n"));
                StartIOTReportTimer();
            }
            else
            {
                CRT.MessageService.ShowInstantMessage("报工成功".L10N(), "提示".L10N(), 3);

                StartNextTask();
            }

        }

        /// <summary>
        /// 生成报工参数
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="okQty"></param>
        /// <param name="suspectQty"></param>
        /// <param name="isIotReport"></param>
        /// <returns></returns>
        SubmitPdaReportInfo NewReportInfo(DispatchTaskQueue queue, decimal okQty, decimal suspectQty, bool isIotReport)
        {
            SubmitPdaReportInfo reportInfo = new SubmitPdaReportInfo()
            {
                QueueId = queue.Id,
                DispatchTaskId = queue.DispatchTaskId,
                ReportQty = okQty,
                GoodQty = okQty,
                SuspectQty = suspectQty,
                ReportEmployeeId = ReportEmployee.Id,
                IsValidatePrepare = ((DispatchTaskQueue?.Seq == 10 && isIotReport == true) || isIotReport != true) ? true : false, //isIotReport=false的时候(即手动报工)，还要判断是当前队列任务的序号是不是10，是的就要做开机准备，其他情况不需要
                IsTaskFinish = true,
                ResourceId = ResourceId ?? 0,
                IsCommonMode = true,
                SourceType = isIotReport == true ? SIE.MES.TaskManagement.Reports.Enums.SourceType.Report_IOT : SIE.MES.TaskManagement.Reports.Enums.SourceType.Report_Manual,
            };
            return reportInfo;
        }
    }
}
