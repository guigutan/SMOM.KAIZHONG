using DevExpress.Xpf.Editors;
using DocumentFormat.OpenXml.EMMA;
using Newtonsoft.Json;
using SIE.Core.RedisUtil;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.IOT;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
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
    /// 生产报工(过程数采) 视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产报工(过程数采)")]
    public class KZTaskReportProcessViewModel : KZTaskReportViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KZTaskReportProcessViewModel()
        {
            IotMode = IotMode.Process;
            Workstation.PropertyChanged += OnWorkstationPropertyChanged;
        }

        /// <summary>
        /// 对象锁
        /// </summary>
        private readonly object lockObj = new object();
        /// <summary>
        /// 报工标识
        /// </summary>
        private bool isReporting = false;

        #region 工序标签队列 WipBatchQueueList
        /// <summary>
        /// 工序标签队列
        /// </summary>
        [Label("工序标签队列")]
        public static readonly ListProperty<EntityList<WipBatchQueue>> WipBatchQueueListProperty = P<KZTaskReportProcessViewModel>.RegisterList(e => e.WipBatchQueueList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (WipBatchQueue) => { return new EntityList<WipBatchQueue>(); }
        });

        /// <summary>
        /// 工序标签队列
        /// </summary>
        public EntityList<WipBatchQueue> WipBatchQueueList
        {
            get { return this.GetLazyList(WipBatchQueueListProperty); }
        }
        #endregion

        #region 当前标签队列 WipBatchQueue
        /// <summary>
        /// 当前标签队列Id
        /// </summary>
        [Label("当前标签队列")]
        public static readonly IRefIdProperty WipBatchQueueIdProperty =
            P<KZTaskReportProcessViewModel>.RegisterRefId(e => e.WipBatchQueueId, ReferenceType.Normal);

        /// <summary>
        /// 当前标签队列Id
        /// </summary>
        public double? WipBatchQueueId
        {
            get { return (double?)this.GetRefNullableId(WipBatchQueueIdProperty); }
            set { this.SetRefNullableId(WipBatchQueueIdProperty, value); }
        }

        /// <summary>
        /// 当前标签队列
        /// </summary>
        public static readonly RefEntityProperty<WipBatchQueue> WipBatchQueueProperty =
            P<KZTaskReportProcessViewModel>.RegisterRef(e => e.WipBatchQueue, WipBatchQueueIdProperty);

        /// <summary>
        /// 当前标签队列
        /// </summary>
        public WipBatchQueue WipBatchQueue
        {
            get { return this.GetRefEntity(WipBatchQueueProperty); }
            set { this.SetRefEntity(WipBatchQueueProperty, value); }
        }
        #endregion

        #region 获取生产队列

        /// <summary>
        /// 获取生产队列
        /// </summary>
        public virtual void LoadTaskQueueList(EntityList<WipBatchQueue> queues = null)
        {
            if (Resource == null || Process == null)
                return;

            if (queues == null)
                queues = RT.Service.Resolve<DispatchController>().GetWipBatchQueueList(Resource.Id, Process.Id, false, new EagerLoadOptions().LoadWithViewProperty());

            CRT.MainThread.InvokeIfRequired(() =>
            {
                WipBatchQueueList.Clear();
                WipBatchQueueList.AddRange(queues);
                WipBatchQueueList.MarkSaved();
                SetSelected(WipBatchQueue?.Id);
            });
        }
        /// <summary>
        /// 加载首个队列任务
        /// </summary>
        public override void LoadFirstQueueTask()
        {
            WipBatchQueue = null;
            if (WipBatchQueueList.Count == 0)
            {
                return;
            }

            var queue = WipBatchQueueList.OrderBy(p => p.Seq).FirstOrDefault(p => !p.IsFinished);
            if (queue == null)
                throw new ValidationException("队列中没有报工标签数据,请确认");

            WipBatchQueue = RF.GetById<WipBatchQueue>(queue.Id, new EagerLoadOptions().LoadWithViewProperty());
            //获取工序分单数量
            var wo = RF.GetById<WorkOrder>(WipBatchQueue.WorkOrderId);
            if (wo != null && Process != null)
            {
                var layoutInfo = wo.LayoutInfoList.Where(p => p.ProcessCode == Process?.Code).FirstOrDefault();
                Zcode = layoutInfo?.Zcode ?? 0;
            }
            SetSelected(WipBatchQueue?.Id);
        }

        /// <summary>
        /// 选中当前队列数据
        /// </summary>
        /// <param name="queueId"></param>
        void SetSelected(double? queueId)
        {
            WipBatchQueueList.ForEach(p => p.IsSelected = p.Id == queueId);
            DispatchTask = LoadDispatchTask(WipBatchQueue);
        }

        DispatchTask LoadDispatchTask(WipBatchQueue queue)
        {
            if (queue == null || Process == null)
                return null;
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { queue.WorkOrderId });

            var taskList = tasks.Where(p => p.WorkOrderId == queue.WorkOrderId && p.ProcessId == Process.Id && (p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched)).OrderBy(p => p.PlanBeginTime).ToList();

            var task = taskList.FirstOrDefault();
            return task;
        }
        #endregion

        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="batchNo"></param>
        public virtual void AddWipBatchQueue(string batchNo)
        {
            if (Resource == null || Process == null)
                return;
            if (WipBatchQueueList.Any(p => p.BatchNo == batchNo))
            {
                ShowError("队列已存在该标签数据,请勿重复加入");
                return;
            }

            var queue = RT.Service.Resolve<DispatchController>().AddQueueWipBatch(Resource.Id, Process.Id, batchNo);

            WipBatchQueueList.Add(queue);
            WipBatchQueueList.MarkSaved();
        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void Onload()
        {
            if (kZReportHelper == null)
                kZReportHelper = new KZReportHelper(this);
            kZReportHelper.ShowReportProcessControl();
            Reset(ResetType.None);

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            Workstation.PropertyChanged -= OnWorkstationPropertyChanged;
            StopIOTReportTimer();
            ReportTimer?.Dispose();
            ReportTimer = null;
        }
        /// <summary>
        /// 工作单元属性变更
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void OnWorkstationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Workstation.Process) || e.PropertyName == nameof(Workstation.Resource) || e.PropertyName == nameof(Workstation.Station))
            {
                var workstation = sender as Workstation;
                if (workstation.StationId > 0 && workstation.ProcessId > 0 && workstation.ResourceId > 0)
                {
                    //找出当前上次缓存的数据
                    string id_key = "KZTaskReportProcess_" + IotMode + RT.IdentityId;
                    //redis缓存将当前选择的给记录下来
                    //只保存七天
                    RT.Service.Resolve<RedisUtilController>().RedisUtilSet(id_key, (Workstation.ResourceId ?? 0) + "_" + (Workstation.ProcessId ?? 0) + "_" + (Workstation.StationId ?? 0), 10080);
                }
            }
            if (e.PropertyName == nameof(Workstation.Process))
            {
                Process = Workstation.Process;
            }
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ResourceIdProperty)
            {
                //限定资源列表
                if (Workstation != null)
                {
                    Workstation.Resource = Resource;
                    Workstation.Process = null;
                    Workstation.Station = null;

                    if (Resource != null)
                    {
                        Workstation.Resources.Clear();
                        Workstation.Resources.Add(Resource);

                        string id_key = "KZTaskReportProcess_" + IotMode + RT.IdentityId;
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
                                var processes = RT.Service.Resolve<ProcessController>().GetProcesssByUserId(ReportEmployeeId ?? 0, null, workstation.ProcessTypes);
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
                        if (workstation != null && workstation.ResourceId != null && workstation.ProcessId != null && workstation.StationId != null && workstation.ResourceId == Workstation.ResourceId)
                        {
                            //赋值顺序不能更换，工位信息选择触发相应的事件
                            Workstation.Resource = workstation.Resource;
                            Workstation.Process = workstation.Process;
                            Workstation.Station = workstation.Station;
                        }
                        else
                        {
                            if (Process == null && !WorkstationSelector.SelectOperation(Workstation, ReportEmployeeId))
                            {
                                ShowError("请先选择工作单元");
                            }
                        }
                    }
                }

            }
            if (e.Property == ProcessProperty)
            {
                CRT.MainThread.InvokeIfRequired(() =>
                {
                    WipBatchQueueList.Clear();
                });

                WipBatchQueue = null;
                OkQty = 0;
                SuspectQty = 0;
                Zcode = 0;
                StopIOTReportTimer();
                if (Process != null)
                {
                    LoadTaskQueueList();
                    LoadFirstQueueTask();
                }
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

                if (ReportTimer?.Enabled == true)
                {
                    if (CRT.MessageService.AskQuestion("【{0}】已开工,是否需要暂停!".L10nFormat(WipBatchQueue?.BatchNo), "提示"))
                    {
                        PauseWork();
                    }
                    return;
                }

                if (IsLocalPrint && Printer.IsNullOrEmpty())
                    throw new ValidationException("当前资源为本地打印模式,请先选择打印机".L10nFormat());

                if (IsRemotePrint && Printer.IsNullOrEmpty())
                    throw new ValidationException("当前资源为远程打印模式,但未配置远程打印机,请联系管理员处理".L10nFormat());

                LoadFirstQueueTask();

                if (WipBatchQueue == null || WipBatchQueue.WipBatch == null)
                {
                    throw new ValidationException("标签明细没有队列数据,请确认".L10N());
                }

                var isReported = RT.Service.Resolve<DispatchController>().CheckIsReport(WipBatchQueue);
                if (isReported)
                {
                    StartNextTask();
                    return;
                }

                #endregion

                RT.Service.Resolve<ReportController>().StartIOTWorkTask(WipBatchQueue, Resource);

                ShowInstantMessage("【{0}】开工成功!".L10nFormat(WipBatchQueue?.BatchNo), "提示", 3);

                await Task.Delay(3000);

                //开启定时器
                StartIOTReportTimer();
                ClearInfos();
                isReporting = false;

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public override void PauseWork()
        {
            WipBatchQueue = RT.Service.Resolve<ReportController>().PauseIOTWorkTask(WipBatchQueue, Resource);
            StopIOTReportTimer();
        }

        /// <summary>
        /// 进行下一个任务
        /// </summary>
        public override async void StartNextTask()
        {
            //var isReported = RT.Service.Resolve<DispatchController>().CheckIsReport(WipBatchQueue);
            //if (isReported)
            {
                StopIOTReportTimer();

                //完工任务队列
                WipBatchQueue = null;
                DispatchTask = null;

                var queues = RT.Service.Resolve<DispatchController>().GetWipBatchQueueList(Resource.Id, Process.Id, false, new EagerLoadOptions().LoadWithViewProperty());

                LoadTaskQueueList(queues);

                if (queues.Count == 0)
                {
                    ShowMessage("队列中所有标签都已经完工，请确认");
                }
                else
                {
                    ShowInstantMessage("当前标签已完工,将自动切换下个标签任务", "提示", 3);

                    await System.Threading.Tasks.Task.Delay(3000);

                    await StartWork();    //队列中还有任务,则继续生产
                }
            }
        }

        /// <summary>
        /// 定时报工事件
        /// </summary>
        public override void ReportTimerElapsed()
        {
            if (isReporting)
                return;
            isReporting = true;

            try
            {
                var isReported = RT.Service.Resolve<DispatchController>().CheckIsReport(WipBatchQueue);
                if (isReported)
                {
                    StartNextTask();
                    return;
                }

                //WipBatchQueue = RF.GetById<WipBatchQueue>(WipBatchQueue.Id, new EagerLoadOptions().LoadWithViewProperty());
                WipBatchQueue = RT.Service.Resolve<ReportController>().GetIOTWorkTask(WipBatchQueue, Resource);
                OkQty = WipBatchQueue.IotQty - SuspectQty;

                if (WipBatchQueue.IotQty < WipBatchQueue.BatchQty)
                    return;

                if (Zcode <= 0)
                    return;
                if (SuspectQty > WipBatchQueue.BatchQty)
                    throw new ValidationException("可疑品数量不能超过标签批次数量");

                var scanInfo = new PdaScanInfo()
                {
                    Sn = WipBatchQueue.BatchNo,
                    ScanType = 1,
                    IsFirstSn = true,
                    //DispatchTaskId = task?.Id,
                    WorkOrderId = WipBatchQueue.WorkOrderId,
                    ResourceId = Workstation.ResourceId ?? 0,
                    ProcessId = Workstation.ProcessId ?? 0,
                    StationId = Workstation.StationId ?? 0,
                };
                var ret = RT.Service.Resolve<ReportController>().CheckScanInfo(scanInfo);
                DispatchTaskId = ret.DispatchTaskId;

                OkQty = WipBatchQueue.BatchQty - SuspectQty;
                var snInfo = new ScanDetailInfo()
                {
                    Sn = WipBatchQueue.BatchNo,
                    Qty = WipBatchQueue.BatchQty,
                    GoodQty = OkQty,
                    SuspectQty = SuspectQty,
                };
                //ShowInstantMessage($"良品[{snInfo.GoodQty}],可疑品[{snInfo.SuspectQty}]", "提示", 3);
                CRT.MainThread.InvokeIfRequired(() =>
                {
                    var redisKey = JsonConvert.SerializeObject(snInfo) + RT.InvOrg;
                    //增加redis锁
                    var tuple = RT.Service.Resolve<RedisUtilController>().Lock(redisKey);
                    if (tuple.Item1)
                    {
                        AutoTaskReport(new List<ScanDetailInfo>() { snInfo });
                        //解锁
                        RT.Service.Resolve<RedisUtilController>().UnLock(redisKey, tuple.Item2);
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                isReporting = false;
            }
        }

        /// <summary>
        /// 自动报工
        /// </summary>
        /// <param name="detailInfos"></param>
        public virtual void AutoTaskReport(List<ScanDetailInfo> detailInfos)
        {
            lock (lockObj)
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
                        PdaScanSubmitInfo submitInfo = new PdaScanSubmitInfo()
                        {
                            ScanType = 1,
                            ResourceId = ResourceId ?? 0,
                            ProcessId = ProcessId ?? 0,
                            DispatchTaskId = DispatchTask.Id,
                            WorkOrderId = DispatchTask?.WorkOrderId ?? 0,
                            DetailInfos = detailInfos,
                            ReportEmployeeId = ReportEmployeeId ?? 0,
                            IsTaskFinish = true
                        };
                        //RT.Service.Resolve<DispatchController>().FinishWipBatchQueue(new List<double>() { WipBatchQueueId ?? 0 });

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
                        if (printInfos.Count > 1)
                            printInfos.Clear(); //没有拆分标签时,不打印

                        OkQty = 0;
                        SuspectQty = 0;
                        Zcode = 0;
                    }
                    catch (Exception exc)
                    {
                        exception = exc;
                    }
                    finally
                    {
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
                    CRT.MessageService.ShowInstantMessage("报工成功".L10N(), "提示".L10N(), 3);

                    var isReported = RT.Service.Resolve<DispatchController>().CheckIsReport(WipBatchQueue);
                    if (isReported)
                        StartNextTask();

                }
            }
        }

    }
}
