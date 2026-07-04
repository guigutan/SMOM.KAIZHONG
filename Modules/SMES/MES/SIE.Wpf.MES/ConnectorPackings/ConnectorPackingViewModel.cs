using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Gauges;
using DevExpress.XtraScheduler.Internal.Implementations;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Barcodes.WipBatchs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.ManagedProperty;
using SIE.MES.BlueLable;
using SIE.MES.PackingQC;
using SIE.MES.PackRule;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PackingQcs;
using SIE.MES.TaskManagement.PackingQcs.Data;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Wpf.MES.ConnectorPacking;
using SIE.Wpf.MES.PackingQC;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkOrderController = SIE.MES.WorkOrders.WorkOrderController;

namespace SIE.Wpf.MES.ConnectorPackings
{
    /// <summary>
    /// 连接器器包装采集
    /// </summary>
    [RootEntity]
    [Label("连接器包装采集")]
    public class ConnectorPackingViewModel : KZDataCollectionViewModel
    {
        public ConnectorPackingViewModel()
        {
            InitWorkstation();
        }

        #region 包装明细
        /// <summary>
        /// 包装明细
        /// </summary>
        public static readonly ListProperty<EntityList<ConnectorPackingModel>> PackageSnRecordListProperty = P<ConnectorPackingViewModel>.RegisterList(e => e.PackageSnRecordList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as ConnectorPackingViewModel).LoadPackageSnRecordList()
        });

        /// <summary>
        /// 包装明细
        /// </summary>
        public EntityList<ConnectorPackingModel> PackageSnRecordList
        {
            get { return this.GetLazyList(PackageSnRecordListProperty); }
        }

        /// <summary>
        /// 包装明细
        /// </summary>
        /// <returns>包装明细</returns>
        private EntityList<PackingQc> LoadPackageSnRecordList()
        {
            return new EntityList<PackingQc>();
        }
        #endregion

        #region 已装箱总数量 blueInt
        /// <summary>
        /// 已装箱总数量
        /// </summary>
        [Label("已装箱总数量")]
        public static readonly Property<int> blueIntProperty = P<ConnectorPackingViewModel>.Register(e => e.blueInt);

        /// <summary>
        /// 已装箱总数量
        /// </summary>
        public int blueInt
        {
            get { return this.GetProperty(blueIntProperty); }
            set { this.SetProperty(blueIntProperty, value); }
        }
        #endregion

        #region 蓝标装箱总数 blueZInt
        /// <summary>
        /// 蓝标装箱总数
        /// </summary>
        [Label("蓝标装箱总数")]
        public static readonly Property<int> blueZIntProperty = P<ConnectorPackingViewModel>.Register(e => e.blueZInt);

        /// <summary>
        /// 蓝标装箱总数
        /// </summary>
        public int blueZInt
        {
            get { return this.GetProperty(blueZIntProperty); }
            set { this.SetProperty(blueZIntProperty, value); }
        }
        #endregion

        #region 需装箱数 NeedQty
        /// <summary>
        /// 需装箱数
        /// </summary>
        [Label("需装箱数")]
        public static readonly Property<decimal> NeedQtyProperty = P<ConnectorPackingViewModel>.Register(e => e.NeedQty);

        /// <summary>
        /// 需装箱数
        /// </summary>
        public decimal NeedQty
        {
            get { return this.GetProperty(NeedQtyProperty); }
            set { this.SetProperty(NeedQtyProperty, value); }
        }
        #endregion


        #region 蓝标 XtBlue
        /// <summary>
        /// 蓝标
        /// </summary>
        [Label("蓝标")]
        public static readonly Property<string> XtBlueProperty = P<ConnectorPackingViewModel>.Register(e => e.XtBlue);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string XtBlue
        {
            get { return this.GetProperty(XtBlueProperty); }
            set { this.SetProperty(XtBlueProperty, value); }
        }
        #endregion

        #region 原蓝标 YXtBlue
        /// <summary>
        /// 原蓝标
        /// </summary>
        [Label("原蓝标")]
        public static readonly Property<string> YXtBlueProperty = P<ConnectorPackingViewModel>.Register(e => e.YXtBlue);

        /// <summary>
        /// 原蓝标
        /// </summary>
        public string YXtBlue
        {
            get { return this.GetProperty(YXtBlueProperty); }
            set { this.SetProperty(YXtBlueProperty, value); }
        }
        #endregion

        #region 状态 DeleteState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> DeleteStateProperty = P<ConnectorPackingViewModel>.Register(e => e.DeleteState);

        /// <summary>
        /// 状态
        /// </summary>
        public string DeleteState
        {
            get { return this.GetProperty(DeleteStateProperty); }
            set { this.SetProperty(DeleteStateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 走蓝标还是工序标
        /// </summary>
        public bool BoolBlue { get; set; } = true;


        // 当前层级累计数量
        public int currentCount = 0;

        // 当前层级索引
        public int currentLevelIndex = 0;

        /// <summary>
        /// 蓝标
        /// </summary>
        public BlueLable blueBable { get; set; }

        /// <summary>
        /// 蓝标层级
        /// </summary>
        public BlueLableLevel blueLableLevel { get; set; }

        /// <summary>
        /// 是否在当前产线下。
        /// </summary>
        public bool BoolBlueLine { get; set; } = false;

        /// <summary>
        /// 标签标识：0=系统默认第一次进去界面，1=进系统第一次扫标签，2=批次生产下的标签，3=SN下的标签
        /// </summary>
        public int SnIdent { get; set; } = 0;

        /// <summary>
        /// 层级目标值数组
        /// </summary>
        public int[] levelTargets;

        /// <summary>
        /// 资源
        /// </summary>
        public double PackresourceId { get; set; } = 0;

        /// <summary>
        /// 等于0是正常。等于1是换箱
        /// </summary>
        public int BoxExChange { get; set; } = 0;

        /// <summary>
        /// 移除标识：0等于的不移除，1等于移除
        /// </summary>
        public int DeleteIdent { get; set; } = 0;

        /// <summary>
        /// 条码变更时间
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            //InputMethod.SetIsInputMethodSuspended( false);
            if (Barcode.IsNullOrEmpty()) return;
            #region 层级拍照

            Error = "";
            Tips = "";
            //var workcell = GetWorkcell();
            PackresourceId = (double)KZWorkstation.ResourceId;
            #endregion
            //BoolBlue=true 扫码蓝标,否则批次或耐压标签
            if (BoolBlue)
            {
                if (DeleteIdent == 0)
                {
                    DeleteState = "扫码中";
                }

                var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(Barcode);

                //蓝标下明细为空，且有删除标识
                var Blue = RT.Service.Resolve<PackingQcController>().AllBlueLable(Barcode);
                if (packingQc == null && Blue.CreateDeleteident == "删除")
                {
                    Error = "该蓝标状态为删除，不允许包装";
                    Barcode = "";
                    return;
                }

                if (BoxExChange == 0)
                {
                    PackageSnRecordList.Clear();
                    blueInt = 0;
                    blueZInt = 0;
                    NeedQty = 0;
                    XtBlue = "";
                    currentCount = 0;
                    currentLevelIndex = 0;
                    blueBable = null;
                    blueLableLevel = null;
                    BoolBlueLine = false;
                    SnIdent = 0;
                }
                else
                {
                    PackageSnRecordList.Clear();
                    blueInt = 0;
                    blueZInt = 0;
                    NeedQty = 0;
                    //XtBlue = "";
                    currentCount = 0;
                    currentLevelIndex = 0;
                    blueBable = null;
                    blueLableLevel = null;
                    BoolBlueLine = false;
                    SnIdent = 0;

                    if (packingQc == null)
                    {
                        if (YXtBlue == "")
                        {
                            Barcode = "";
                            Tips = "新蓝标无需换箱!!!";
                            return;
                        }
                        if (BoxExChange == 1)
                        {
                            YXtBlue = XtBlue;
                            XtBlue = Barcode;
                            //原蓝标查询所有蓝标
                            var yXtBlue = RT.Service.Resolve<PackingQcController>().AllBlueLable(YXtBlue);
                            //原蓝标查询装箱数量
                            var yXpackingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(YXtBlue);
                            //现有蓝标
                            var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(yXpackingQc.Id);
                            var detailSum = packdetails.Count();
                            var xtBlue = RT.Service.Resolve<PackingQcController>().GetBlueLable(XtBlue);
                            if (xtBlue == null)
                            {
                                Error = "新蓝标在系统中不存在,新蓝标【" + Barcode + "】!!!";
                                Barcode = "";
                                XtBlue = "";
                                YXtBlue = "";
                                return;
                            }
                            //if (xtBlue.PackageNum >= detailSum)
                            if (xtBlue.PackageNum >= packdetails.Sum(p => p.PackingNum))
                            {
                                Barcode = "";
                                Tips = "请点提交按钮,确认换箱!!!";
                                return;
                            }
                            else
                            {
                                Barcode = "";
                                XtBlue = "";
                                YXtBlue = "";
                                Error = "换箱失败,新蓝标数量【" + xtBlue.PackageNum + "】,原蓝标数量【" + yXtBlue.PackageNum + "】,已装箱数【" + detailSum + "】";
                                Tips = "请输入已装箱的蓝标!!!";
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (YXtBlue != "")
                        {
                            var xtBluePackDtls = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByBlueLabel(packingQc.BlueLabel);
                            if (xtBluePackDtls.Count > 0)
                            {
                                Error = "该蓝标存在装箱明细，不允许换箱";
                                Barcode = "";
                                XtBlue = "";
                                YXtBlue = "";
                                return;
                            }
                        }
                    }
                }

                XtBlue = Barcode;
                //第一步 系统中是否有蓝标
                //if (BoxExChange == 0)
                //{
                //    blueBable = RT.Service.Resolve<PackingQcController>().GetBlueLable(Barcode);
                //}
                //else
                //{
                //    blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(Barcode);
                //}

                blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(Barcode);

                if (blueBable == null)
                {
                    Error = "系统中没有此蓝标!";
                    Barcode = "";
                    return;
                }
                //根据蓝标获取工单
                WorkOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo(blueBable.ProductionNo);
                if (WorkOrder == null)
                {
                    Error = "蓝标中的工单【" + blueBable.ProductionNo + "】不存在!";
                    Barcode = "";
                    return;
                }
                //验证蓝标应该单体包装还是批次包装
                var msg = RT.Service.Resolve<ConnectorPackingController>().ValidBlueLable(blueBable);
                if (!msg.IsNullOrEmpty())
                {
                    Error = msg;
                    Barcode = "";
                    return;
                }
                //获取蓝标的总数
                blueZInt = blueBable.PackageNum;

                //是否派工单有包装的任务单
                var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTaskByResourceId(PackresourceId, null, null);
                if (tasks.Count <= 0)
                {
                    Error = "派工单中没有当前资源的任务单!";
                    Reset();
                    return;
                }
                var task = tasks.FirstOrDefault(p => p.WorkOrderId == WorkOrder.Id && p.ProcessName.Contains("包装"));
                if (task == null)
                {
                    Error = "派工单中没有当前资源下包装的任务单!";
                    Reset();
                    return;
                }

                try
                {
                    RT.Service.Resolve<ProcessPrepareRecordsController>().ValidateProcessPrepare(task);
                }
                catch (Exception ex)
                {
                    Error = ex.GetBaseException().Message;
                    Barcode = "";
                    return;
                }



                if (packingQc == null)
                {
                    if (BoxExChange == 1)
                    {
                        XtBlue = Barcode;
                        Barcode = "";
                        Tips = "请点提交按钮,确认换箱!!!";
                        return;
                    }
                    //获取蓝标QC确认
                    var packingQcData = RT.Service.Resolve<PackingQcController>().GetPackingQcByState((double)KZWorkstation.ResourceId);
                    foreach (var item in packingQcData)
                    {
                        if (item.BlueLabel != Barcode)
                        {
                            if (item.Confirm == ConfirmEnum.NO)
                            {
                                Error = "蓝标标签[" + item.BlueLabel + "]QC未确认,不允许使用其它蓝标标签!";
                                Reset();
                                BoolBlue = true;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    //开箱
                    var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
                    //记录已经装了多少箱数量
                    PackageSnRecordList.Clear();
                    foreach (var item in packDetailList)
                    {
                        ConnectorPackingModel pqc = new ConnectorPackingModel();
                        pqc.BlueLabel = XtBlue;
                        pqc.Confirm = item.Confirm;
                        pqc.PackIdent = packingQc.PackIdent;
                        pqc.ProductLabel = item.ProductLabel;
                        pqc.PackingNum = item.PackingNum;
                        pqc.ItemId = packingQc.ItemId;
                        pqc.ItemName = packingQc.Item.Name;
                        pqc.ResourceId = PackresourceId;
                        blueInt += item.PackingNum;
                        PackageSnRecordList.Add(pqc);
                    }
                    if (BoxExChange == 1)
                    {
                        YXtBlue = Barcode;
                        Barcode = "";
                        Tips = "请扫码换箱的蓝标!!!";
                        return;
                    }

                    if (packingQc.BoxState != BoxStateEnum.NO)
                    {
                    }
                    else
                    {
                        Error = "该蓝标已经封箱,请先点击开箱按钮!";
                        Barcode = "";
                        return;
                    }
                }


                XtBlue = Barcode;
                BoolBlue = false;
                Barcode = "";
                Tips = "请输入工序标签!";
            }
            else
            {
                //批次标签
                WipBatch wipBatch = new WipBatch();
                wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatchPc(Barcode);
                //第一步 工序标签是否存在
                if (wipBatch == null)
                {
                    Error = "批次标签不存在!";
                    Barcode = "";
                    return;
                }

                if (DeleteIdent == 1)
                {
                    var message = DeleteLabel(Barcode);
                    if (message == "移除成功!")
                    {
                        Tips = message;
                        Barcode = "";
                        //TcTs("请QC确认完成后,继续扫工序标签");
                        return;
                    }
                    else
                    {
                        Error = message;
                        Barcode = "";
                        return;
                    }
                }
                else
                {
                    if (blueBable.CreateDeleteident == "删除")
                    {
                        Error = "删除的蓝标不允许其他操作，只能移除";
                        Barcode = "";
                        return;
                    }
                }

                var wipResource = RT.Service.Resolve<WipResourceController>().GetWipResource(PackresourceId);
                //包装采集主表
                PackingQc packingQc = new PackingQc();

                PackingDetail packingDetail = new PackingDetail();

                string pcSn = "";
                pcSn = wipBatch.BatchNo;
                if (wipBatch.Isuse)
                {
                    Error = "批次标签已经使用过!";
                    Barcode = "";
                    return;
                }

                //第二步 判断工单是否一致
                if (WorkOrderId != wipBatch.WorkOrderId)
                {
                    Error = "工序标签没在一个工单下面!当前标签批次[" + pcSn + "],工单[" + wipBatch.WorkOrder.No + "]";
                    Barcode = "";
                    return;
                }

                //获取工单
                var workOrderData = RT.Service.Resolve<WipBatchController>().GetWorkOrder(WorkOrderId);
                //获取产品
                var itemData = RT.Service.Resolve<WipBatchController>().GetItem(workOrderData.ProductId);

                try
                {
                    //SN和批次标签校验 前置是否报工
                    RT.Service.Resolve<ITaskReportKZ>().ValidatePrepareProcessHasReport(Barcode, "包装");
                }
                catch (Exception ex)
                {
                    Error = ex.GetBaseException().Message;
                    Barcode = "";
                    return;
                }

                var dispatchTasks = RT.Service.Resolve<DispatchController>().GetDispatchTaskByWoPacking(WorkOrder.Id, PackresourceId);
                if (dispatchTasks.Count == 0)
                {
                    Error = "工单【{0}】,资源【{1}】,工序【包装】对应的派工任务单不存在!".L10nFormat(WorkOrder.No, wipResource.Name);
                    Barcode = "";
                    return;
                }
                decimal taskRemainQty = 0;
                //任务单与剩余可报工数字典，用于后面拆分标签的时候使用
                Dictionary<double, decimal> taskDic = new Dictionary<double, decimal>();
                NeedQty = 0;
                foreach (var dispatchTask in dispatchTasks)
                {
                    var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(dispatchTask);

                    var packingTasks = RT.Service.Resolve<WipBatchController>().GetWipBatchesByPackingTaskIds(new List<double>() { dispatchTask.Id });
                    //剩余最大可包装数
                    var RemainQty = tuple.Item2;
                    var maxReportQty = tuple.Item1;

                    //需要装箱数
                    NeedQty += maxReportQty;

                    if (packingTasks.Count > 0)
                    {
                        RemainQty = (int)(maxReportQty - packingTasks.Sum(p => p.Qty) - dispatchTask.SuspectQty);//(int)(RemainQty - (dispatchTask.ReportQty - packingTasks.Sum(p => p.Qty)));
                    }
                    //如果剩余可包装数，已经超了，就直接跳过
                    if (RemainQty <= 0)
                        continue;

                    taskDic.Add(dispatchTask.Id, RemainQty);
                }
                if (taskDic.Count > 0)
                {
                    taskDic = taskDic.OrderBy(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
                    taskRemainQty = taskDic.Sum(p => p.Value);
                }

                if (taskRemainQty <= 0)
                {
                    Error = "当前资源对应的任务单没有剩余可报工数".L10N();
                    Barcode = "";
                    return;
                }
                var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                var totalPackingNum = packdetails.Sum(p => p.PackingNum);
                if (totalPackingNum > blueBable.PackageNum)
                {

                    Error = "蓝标装箱数已满,不允许继续装箱!".L10N();
                    Barcode = "";
                    return;
                }
                decimal packingQty = blueBable.PackageNum - totalPackingNum;    //剩余可装箱数
                //当蓝标数比任务单数小的时候，就以蓝标数为主
                if (NeedQty > blueBable.PackageNum)
                {
                    NeedQty = blueBable.PackageNum;
                }
                if (packingQty <= 0)
                {
                    Error = "当前蓝标没有剩余可装箱数,请确认!".L10N();
                    Barcode = "";
                    return;
                }

                //校验不允许超任务数
                //var qcToReportDetails = RT.Service.Resolve<PackingQcController>().GetPackingDetails(PackresourceId, WorkOrder.No);
                //var qcToResportQty = qcToReportDetails.Sum(p => p.PackingNum);    //已装箱待报工数
                //taskRemainQty = taskRemainQty - qcToResportQty; //任务单可报工数需要去除待报工数
                if (taskRemainQty <= 0)
                {
                    Error = "当前资源对应的任务单,剩余报工数已全部装箱,请确认!".L10N();
                    Barcode = "";
                    return;
                }
                if (packingQty > taskRemainQty)
                {
                    packingQty = taskRemainQty;
                }
                if (packingQty > taskRemainQty)
                {
                    Error = "装箱数[{0}]，已超过任务单剩余可报工数[{1}]!".L10nFormat(wipBatch.Qty, taskRemainQty);
                    Barcode = "";
                    return;
                }
                if (packingQty > blueZInt - blueInt)
                    packingQty = blueZInt - blueInt;

                //任务单与标签号，记录对应的标签拆分到哪个任务单上去
                Dictionary<string, double> SnTaskDic = null;
                //切记只能>不能>=，否则拆分会出问题
                if (wipBatch.Qty > packingQty/*(blueZInt - blueInt)*/)
                {
                    if (wipBatch.Qty == packingQty/*(blueZInt - blueInt)*/)
                    {
                        blueInt += (int)wipBatch.Qty;
                        //修改批次生成 工序标签是否已经使用
                        wipBatch.Isuse = true;
                        packingDetail.PackingNum = (int)wipBatch.Qty;
                        wipBatch.PackingTaskId = taskDic.Where(p => p.Value >= wipBatch.Qty).FirstOrDefault().Key;
                        RF.Save(wipBatch);
                    }
                    else
                    {
                        //剩余数量
                        decimal surplusNum = wipBatch.Qty - packingQty/*(blueZInt - blueInt)*/;
                        int splitNum = (int)packingQty/*(blueZInt - blueInt)*/;
                        DialogResult result = MessageBox.Show("批次数量大于蓝标数量,拆分数量:[" + splitNum + "],标签剩余数量:[" + surplusNum + "]？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            //先赋值，后面会对surplusNum进行重新计算
                            if (wipBatch.Qty > 0 && wipBatch.Qty != surplusNum)
                            {
                                wipBatch.EditQtyProcessCode = "包装";
                            }
                            wipBatch.Qty = surplusNum;
                            //packingDetail.PackingNum = splitNum;
                            blueInt += (int)packingQty;//(blueZInt - blueInt);

                            SnTaskDic = new Dictionary<string, double>();
                            var pq = packingQty;
                            foreach (var dic in taskDic)
                            {
                                var chaiQty = pq;
                                if (dic.Value < pq)
                                {
                                    chaiQty = dic.Value;
                                }


                                WipBatch wipBatch1 = new WipBatch();
                                int i = 1;
                                var newWipName = "";
                                while (true)
                                {

                                    //先查询是否存在
                                    wipBatch1 = RT.Service.Resolve<WipBatchController>().GetWipBatchReport(Barcode + "-" + i);
                                    if (wipBatch1 != null)
                                    {
                                        i++;
                                    }
                                    else
                                    {
                                        newWipName = Barcode + "-" + i;
                                        break;
                                    }
                                }
                                SaveWipBatch((double)WorkOrderId, (int)chaiQty/*packingQty*//*(blueZInt - blueInt)*/, newWipName, Barcode, "包装", dic.Key);
                                SnTaskDic.Add(newWipName, dic.Key);
                                pq -= chaiQty;
                                if (pq <= 0)
                                    break;
                            }

                            RF.Save(wipBatch);
                        }
                        else
                        {
                            Tips = "请输入工序标签!";
                            Barcode = "";
                            return;
                        }
                    }
                }
                else
                {
                    if (taskDic.Count > 1)
                    {
                        SnTaskDic = new Dictionary<string, double>();
                        var pq = packingQty;
                        if (pq > wipBatch.Qty)
                        {
                            pq = wipBatch.Qty;
                        }
                        var index = 0;
                        decimal num = 0;
                        //此处要记一个数量，如2个任务单，数量分别是10，10，但是如果标签数为15，那么就只能拆一个标签出来，因为需要保存原标签，否则原标签数量就变成了0,index永远要比任务单数量少,计算有多少个任务单可以参与计算
                        foreach (var dic in taskDic)
                        {
                            num += dic.Value;
                            if (num >= wipBatch.Qty)
                                break;
                            index += 1;
                        }
                        //当数量刚好满足一个任务单的时候，直接赋值原数量即可，index = 0，即满足一个任务单
                        if (index == 0)
                            pq = wipBatch.Qty;
                        for (int j = 0; j < index; j++)
                        {
                            var dic = taskDic.ElementAtOrDefault(j);
                            var chaiQty = pq;
                            if (dic.Value < pq)
                            {
                                chaiQty = dic.Value;
                            }

                            WipBatch wipBatch1 = new WipBatch();
                            int i = 1;
                            var newWipName = "";
                            while (true)
                            {

                                //先查询是否存在
                                wipBatch1 = RT.Service.Resolve<WipBatchController>().GetWipBatchReport(Barcode + "-" + i);
                                if (wipBatch1 != null)
                                {
                                    i++;
                                }
                                else
                                {
                                    newWipName = Barcode + "-" + i;
                                    break;
                                }
                            }
                            SaveWipBatch((double)WorkOrderId, (int)chaiQty/*packingQty*//*(blueZInt - blueInt)*/, newWipName, Barcode, "包装", dic.Key);
                            SnTaskDic.Add(newWipName, dic.Key);
                            blueInt += (int)chaiQty;
                            pq -= chaiQty;
                            if (pq <= 0)
                                break;
                        }
                        wipBatch.Qty = pq;
                        wipBatch.PackingTaskId = taskDic.LastOrDefault().Key;
                        SnTaskDic.Add(wipBatch.BatchNo, taskDic.LastOrDefault().Key);
                        blueInt += (int)wipBatch.Qty;
                    }
                    else
                    {
                        blueInt += (int)wipBatch.Qty;
                        //修改批次生成 工序标签是否已经使用
                        packingDetail.PackingNum = (int)wipBatch.Qty;
                        wipBatch.PackingTaskId = taskDic.FirstOrDefault().Key;
                    }
                    wipBatch.Isuse = true;
                    RF.Save(wipBatch);
                }



                packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
                #region 存入数据库
                var PackIdent = PackIdentEnum.NotFullTank;
                if (blueInt >= blueBable.PackageNum)
                    PackIdent = PackIdentEnum.FullTank;

                if (packingQc == null)
                {
                    packingQc = new PackingQc();
                    packingQc.BlueLabel = XtBlue;
                    packingQc.Confirm = ConfirmEnum.YES;
                    packingQc.PackIdent = PackIdent;//PackIdentEnum.NotFullTank;
                    if (SnTaskDic != null && SnTaskDic.Count > 0)
                        packingQc.ProductLabel = string.Join(',', SnTaskDic.Select(p => p.Key).Distinct().ToList());
                    else
                        packingQc.ProductLabel = Barcode;
                    packingQc.ItemId = itemData.Id;
                    packingQc.ItemName = itemData.Name;
                    packingQc.BlueLableNum = blueZInt;
                    packingQc.PackingNum = blueInt;
                    packingQc.ResourceId = (double)KZWorkstation.ResourceId;
                    packingQc.BoxState = BoxStateEnum.YES;
                    packingQc.ReportsType = ReportsTypeEnum.NO;
                    blueBable.IsPack = true;
                }
                else
                {
                    //if (blueInt == blueBable.PackageNum)
                    //{
                    //    packingQc.PackIdent = PackIdentEnum.FullTank;
                    //}
                    //else
                    //{
                    //    packingQc.PackIdent = PackIdentEnum.NotFullTank;
                    //}
                    packingQc.PackIdent = PackIdent;
                    packingQc.Confirm = ConfirmEnum.YES;
                    packingQc.PackingNum = blueInt;
                    blueBable.IsPack = true;
                }
                //存入包装QC确认表
                RF.Save(packingQc);
                RF.Save(blueBable);

                //此处为了满足按按任务单拆分
                if (SnTaskDic != null && SnTaskDic.Count > 0)
                {
                    foreach (var dic in SnTaskDic)
                    {
                        var newWipName = dic.Key;
                        var newWipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(newWipName);

                        //界面显示明细
                        ConnectorPackingModel pqc = new ConnectorPackingModel();
                        pqc.BlueLabel = XtBlue;
                        pqc.Confirm = ConfirmEnum.YES;
                        pqc.PackIdent = PackIdent;//PackIdentEnum.NotFullTank;
                        pqc.ProductLabel = newWipName;
                        pqc.PackingNum = (int)newWipBatch.Qty;
                        pqc.ItemId = itemData.Id;
                        pqc.ItemName = itemData.Name;
                        pqc.ResourceId = (double)KZWorkstation.ResourceId;
                        PackageSnRecordList.Add(pqc);

                        //存入包装QC确认明细表
                        PackingDetail newPackingDetail = new PackingDetail();
                        newPackingDetail.PackingNum = (int)newWipBatch.Qty;
                        newPackingDetail.ProductLabel = newWipName;

                        newPackingDetail.PackingQcId = packingQc.Id;
                        newPackingDetail.Confirm = ConfirmEnum.YES;
                        newPackingDetail.BatchLabel = newWipName;
                        newPackingDetail.WorkOrderNo = WorkOrder.No;

                        newPackingDetail.LabelType = LabelTypeEnum.BatchLabel;
                        newPackingDetail.ReportsType = ReportsTypeEnum.NO;

                        RF.Save(newPackingDetail);
                    }
                }
                else
                {
                    //界面显示明细
                    ConnectorPackingModel pqc = new ConnectorPackingModel();
                    pqc.BlueLabel = XtBlue;
                    pqc.Confirm = ConfirmEnum.YES;
                    pqc.PackIdent = PackIdent;//PackIdentEnum.NotFullTank;
                    pqc.ProductLabel = Barcode;
                    pqc.PackingNum = (int)wipBatch.Qty;

                    pqc.ItemId = itemData.Id;
                    pqc.ItemName = itemData.Name;
                    pqc.ResourceId = (double)KZWorkstation.ResourceId;

                    PackageSnRecordList.Add(pqc);
                    //存入包装QC确认明细表
                    packingDetail.ProductLabel = Barcode;

                    packingDetail.PackingQcId = packingQc.Id;
                    packingDetail.Confirm = ConfirmEnum.YES;
                    packingDetail.BatchLabel = pcSn;
                    packingDetail.WorkOrderNo = WorkOrder.No;

                    packingDetail.LabelType = LabelTypeEnum.BatchLabel;
                    packingDetail.ReportsType = ReportsTypeEnum.NO;

                    RF.Save(packingDetail);
                }



                #endregion

                if (PackIdent == PackIdentEnum.FullTank/*blueInt == NeedQty*//*packingQty*//*blueZInt*/)
                {
                    PackingReportRecord record = new PackingReportRecord();
                    record.BeginDate = DateTime.Now;
                    record.BlueLabel = XtBlue;
                    record.Report = ReportType.Connector;
                    PackageSnRecordList.Clear();
                    BoolBlue = true;
                    XtBlue = "";
                    blueInt = 0;
                    NeedQty = 0;
                    XtBlue = "";
                    blueZInt = 0;
                    blueBable = null;
                    Tips = "请输入蓝标标签!";
                    Barcode = "";
                    SnIdent = 0;
                    packingQc.BoxState = BoxStateEnum.NO;
                    RF.Save(packingQc);
                    string submitMessage = RT.Service.Resolve<PackingQcController>().SubmitData(packingQc, autoFeeding: true, IsTaskFinish: false);
                    record.ReturnMessage = submitMessage;
                    RF.Save(record);
                    if (submitMessage != "")
                    {
                        Tips = "";
                        Reset();
                        Error = submitMessage;
                    }
                    else
                    {
                        PackageSnRecordList.Clear();
                        packingQc.BoxState = BoxStateEnum.NO;
                        //packingQc.PackIdent = PackIdentEnum.FullTank;
                        packingQc.ReportsType = ReportsTypeEnum.YES;
                        RF.Save(packingQc);
                    }
                    MessageBox.Show("包装成功,包装数量:[" + packingQc.PackingNum + "]!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }
                Tips = "请输入工序标签!";
                Barcode = "";
            }
            base.OnBarcodeChanged(e);
        }

        public void CreatePd()
        {

        }

        public void AllReset()
        {
            WorkOrderId = null;
            PackageSnRecordList.Clear();
            blueInt = 0;
            blueZInt = 0;
            NeedQty = 0;
            XtBlue = "";
            BoolBlue = true;
            YXtBlue = "";
            BoxExChange = 0;
            currentCount = 0;
            currentLevelIndex = 0;

            blueBable = null;
            blueLableLevel = null;
            BoolBlueLine = false;
            SnIdent = 0;
            Tips = "请输入蓝标!";
            Barcode = "";
            DeleteState = "扫码中";
            DeleteIdent = 0;
            //Reset(resetType: ResetType.CollectRestart);
            //FocuseBarcode();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            PackageSnRecordList.Clear();
            blueInt = 0;
            blueZInt = 0;
            NeedQty = 0;
            XtBlue = "";
            currentCount = 0;
            currentLevelIndex = 0;
            blueBable = null;
            blueLableLevel = null;
            BoolBlueLine = false;
            SnIdent = 0;
            PackresourceId = 0;
            Barcode = "";
        }

        /// <summary>
        /// 初始化层级计数器
        /// </summary>
        /// <param name="levelString">层级字符串</param>
        public void LevelString(string levelString)
        {
            // 解析层级字符串为整数数组
            levelTargets = levelString.Split(',').Select(int.Parse).ToArray();
        }

        /// <summary>
        /// 保存批次标签
        /// </summary>
        /// <param name="itemLabel"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual bool SaveWipBatch(double workOrderId, int qty, string wipName, string barcode, string processCode = null, double? taskId = null)
        {
            WipBatch wipBatch = new WipBatch();
            wipBatch.BatchNo = wipName;
            wipBatch.IsChild = true;
            wipBatch.IsGenerate = true;
            wipBatch.IsGenerateChild = true;
            wipBatch.IsMantissa = true;
            wipBatch.IsScraped = false;
            wipBatch.PrintTimes = 0;
            wipBatch.PrintedState = SIE.Barcodes.BarcodeState.Notprint;
            wipBatch.Qty = qty;
            wipBatch.WorkOrderId = workOrderId;
            wipBatch.Isuse = true;
            wipBatch.IsRework = false;
            wipBatch.SourceNo = barcode;
            wipBatch.EditQtyProcessCode = processCode;
            wipBatch.PackingTaskId = taskId;
            return RT.Service.Resolve<WipBatchController>().SaveWipBatch(wipBatch);
        }

        /// <summary>
        /// 移除批次标签
        /// </summary>
        /// <param name="barcode"></param>
        public string DeleteLabel(string barcode)
        {
            //主表
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
            try
            {
                //校验移除
                RT.Service.Resolve<ConnectorPackingController>().ValidBatchDeleteLabel(barcode, WorkOrder.Id);
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
            //从表
            var packingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(barcode);
            if (packingDetailList.Count > 0)
            {
                var packDetail = packingDetailList.Where(p => p.ProductLabel == barcode).FirstOrDefault();
                if (packDetail != null)
                {
                    if (packDetail.ReportsType == ReportsTypeEnum.NO)
                    {
                        packDetail.PersistenceStatus = PersistenceStatus.Deleted;
                        packingQc.PackingNum -= packDetail.PackingNum;
                        blueInt -= packDetail.PackingNum;
                        //RF.Save(packDetail);
                        wipBatch.Isuse = false;
                        wipBatch.PackingTaskId = null;
                        //RF.Save(wipPressureSn);

                        if (packingDetailList.Count == 1)
                        {
                            packingQc.PersistenceStatus = PersistenceStatus.Deleted;
                            RT.Service.Resolve<PackingQcController>().DeleteCSave(packDetail, wipBatch, packingQc);
                            Reset();
                            AllReset();
                        }
                        else
                        {
                            RT.Service.Resolve<PackingQcController>().DeleteCSave(packDetail, wipBatch, packingQc);

                            PackageSnRecordList.Clear();
                            var NewPackingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                            foreach (var item in NewPackingDetailList)
                            {
                                ConnectorPackingModel pqc = new ConnectorPackingModel();
                                pqc.BlueLabel = XtBlue;
                                pqc.Confirm = item.Confirm;
                                pqc.PackIdent = packingQc.PackIdent;
                                pqc.ProductLabel = item.ProductLabel;
                                pqc.ItemId = packingQc.ItemId;
                                pqc.ItemName = packingQc.Item.Name;
                                pqc.ResourceId = PackresourceId;
                                pqc.PackingNum = item.PackingNum;
                                //blueInt += item.PackingNum;

                                PackageSnRecordList.Add(pqc);
                            }
                        }

                    }
                    else
                    {
                        return "该标签【" + barcode + "】已经报工,没法移除!";
                    }
                }
                else
                {
                    return "该标签不存在,标签【" + barcode + "】!";
                }
            }
            else
            {
                return "蓝标【" + XtBlue + "】没有装箱!";
            }
            return "移除成功!";

        }
    }
}
