using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.EventMessages.MES.Dispatchs;
using SIE.ManagedProperty;
using SIE.MES.BlueLable;
using SIE.MES.PackingQC;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Wpf.MES.WIP;
using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Forms;

namespace SIE.Wpf.MES.PackingQC
{
    /// <summary>
    /// A包装采集
    /// </summary>
    [RootEntity]
    [Label("A包装采集")]
    public class PackingQcViewModel : KZDataCollectionViewModel
    {
        public PackingQcViewModel()
        {
            InitWorkstation();
            PropertyChanged += PackingQcViewModel_PropertyChanged;
            //PropertyChanged -= PackingQcViewModel_PropertyChanged;

            KZWorkstation.PropertyChanged += KZWorkstation_PropertyChanged;
            //KZWorkstation.PropertyChanged -= KZWorkstation_PropertyChanged;

        }

        private void KZWorkstation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == KZWorkstation.ResourceProperty.Name)
            {
                WorkOrderId = null;
                PackageSnRecordList.Clear();
                blueInt = 0;
                blueZInt = 0;
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
                Reset(resetType: ResetType.CollectRestart);
                FocuseBarcode();
            }
        }

        private void PackingQcViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Reset();
        }

        #region 包装明细
        /// <summary>
        /// 包装明细
        /// </summary>
        public static readonly ListProperty<EntityList<PackingQcModel>> PackageSnRecordListProperty = P<PackingQcViewModel>.RegisterList(e => e.PackageSnRecordList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as PackingQcViewModel).LoadPackageSnRecordList()
        });

        /// <summary>
        /// 包装明细
        /// </summary>
        public EntityList<PackingQcModel> PackageSnRecordList
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
        public static readonly Property<int> blueIntProperty = P<PackingQcViewModel>.Register(e => e.blueInt);

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
        public static readonly Property<int> blueZIntProperty = P<PackingQcViewModel>.Register(e => e.blueZInt);

        /// <summary>
        /// 蓝标装箱总数
        /// </summary>
        public int blueZInt
        {
            get { return this.GetProperty(blueZIntProperty); }
            set { this.SetProperty(blueZIntProperty, value); }
        }
        #endregion

        #region 蓝标 XtBlue
        /// <summary>
        /// 蓝标
        /// </summary>
        [Label("蓝标")]
        public static readonly Property<string> XtBlueProperty = P<PackingQcViewModel>.Register(e => e.XtBlue);

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
        public static readonly Property<string> YXtBlueProperty = P<PackingQcViewModel>.Register(e => e.YXtBlue);

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
        public static readonly Property<string> DeleteStateProperty = P<PackingQcViewModel>.Register(e => e.DeleteState);

        /// <summary>
        /// 状态
        /// </summary>
        public string DeleteState
        {
            get { return this.GetProperty(DeleteStateProperty); }
            set { this.SetProperty(DeleteStateProperty, value); }
        }
        #endregion

        #region 验证码 VerifyCode
        /// <summary>
        /// 验证码
        /// </summary>
        [Label("验证码")]
        public static readonly Property<string> VerifyCodeProperty = P<PackingQcViewModel>.Register(e => e.VerifyCode);

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode
        {
            get { return this.GetProperty(VerifyCodeProperty); }
            set { this.SetProperty(VerifyCodeProperty, value); }
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
            //Console.Beep(1000, 300);

            #region 层级拍照
            //确认蓝标是否存在
            //是否有封箱记录 <有，必须点开箱按钮，去修改数据PackingQc开箱>
            //当前是扫蓝标还是工序标签
            //获取蓝标的总数，以及层数
            //每层都需要QC确认，在弹框点是的时候，保存数据，把QC确认字段改成否
            //按照QC确认字段卡控是否能继续下一步
            //满箱情况数据。

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
                if (packingQc == null && Blue?.CreateDeleteident == "删除")
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

                            //if (xtBlue.PackageNum <= yXtBlue.PackageNum && xtBlue.PackageNum >= detailSum)
                            if (xtBlue.PackageNum <= yXtBlue.PackageNum && xtBlue.PackageNum >= packdetails.Sum(p => p.PackingNum))
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
                    SoundPlayer();
                    Error = "系统中没有此蓝标!";
                    Barcode = "";
                    //TcTs("系统中没有此蓝标");
                    return;
                }
                //根据蓝标获取工单
                WorkOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo(blueBable.ProductionNo);

                if (WorkOrder == null)
                {
                    SoundPlayer();
                    Error = "系统中没有此工单!";
                    Barcode = "";
                    //TcTs("系统中没有此蓝标");
                    return;
                }

                //获取蓝标的总数
                blueZInt = blueBable.PackageNum;
                //蓝标层级
                blueLableLevel = RT.Service.Resolve<PackingQcController>().GetBlueLableLevel(blueBable.ItemId);

                if (blueLableLevel == null)
                {
                    SoundPlayer();
                    Error = "该蓝标没有维护层级!";
                    Barcode = "";
                    //TcTs("该蓝标没有维护层级");
                    return;
                }

                //旧蓝标包装时所在的线体
                string ResourceName = "";
                try
                {
                    PackingQcCriterial criterial = new PackingQcCriterial();
                    criterial.BlueLabel = blueBable.BlueLableBox;
                    DateRange dateRange = new DateRange();
                    dateRange.BeginValue = new DateTime(1900, 1, 1);
                    dateRange.EndValue = DateTime.Now.AddDays(100);
                    criterial.CreateDate = dateRange;
                    EntityList<PackingQc> PackingQcEntityList = RT.Service.Resolve<PackingQcController>().CriterialPackingQc(criterial);
                    ResourceName = PackingQcEntityList.FirstOrDefault()?.Resource?.Name;
                    ResourceName = string.IsNullOrEmpty(ResourceName) ? "" : @$"---旧蓝标在《{ResourceName}》";
                }
                catch { }

                //是否派工单有包装的任务单
                var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTaskByResourceId(PackresourceId, null, null);
                if (tasks.Count <= 0)
                {
                    SoundPlayer();
                    Error = $@"派工单中没有当前资源的任务单!{ResourceName}";
                    Reset();
                    return;
                }
                var task = tasks.FirstOrDefault(p => p.WorkOrderId == WorkOrder.Id && p.ProcessName.Contains("包装"));
                if (task == null)
                {


                    SoundPlayer();
                    Error = $@"派工单中没有当前资源下包装的任务单!{ResourceName}";
                    Reset();
                    return;
                }

                //decimal Uebto = 0;
                //decimal.TryParse(WorkOrder.Uebto, out Uebto);

                //容差数=计划数量*（1+交货容差%）
                //decimal TotalTolerance = WorkOrder.PlanQty * (1 + Uebto / 100);
                //未完成数=容差数-完工数
                //decimal UnfinishedNum = TotalTolerance - WorkOrder.FinishQty;
                //蓝标总数大于未完成数，提示报错
                var packSum = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);

                if (packSum == null)
                {
                    var MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task).Item2;

                    if (blueBable.PackageNum > MaxRemainQty/*task.MaxRemainQty*/)
                    {
                        SoundPlayer();
                        Error = "蓝标装箱数大于未报工数,目前蓝标数【" + blueBable.PackageNum + "】未完工数【" + MaxRemainQty/*task.MaxRemainQty */+ "】!";
                        Barcode = "";
                        //TcTs("系统中没有此蓝标");
                        return;
                    }
                }
                else
                {
                    //var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailAByids(packSum.Id);
                    var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packSum.Id);
                    //
                    var totalPackingNum = packdetails.Count;

                    var MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task).Item2;

                    if ((blueBable.PackageNum - totalPackingNum) > MaxRemainQty/*task.MaxRemainQty*/)
                    {
                        SoundPlayer();
                        Error = "蓝标装箱数大于未报工数,目前蓝标可装箱数【" + (blueBable.PackageNum - totalPackingNum) + "】未完工数【" + MaxRemainQty/*task.MaxRemainQty*/ + "】!";
                        Barcode = "";
                        //TcTs("系统中没有此蓝标");
                        return;
                    }
                }

                try
                {
                    RT.Service.Resolve<ProcessPrepareRecordsController>().ValidateProcessPrepare(task);
                }
                catch (Exception ex)
                {
                    SoundPlayer();
                    Error = ex.GetBaseException().Message;
                    Barcode = "";
                    return;
                }

                //第二步 蓝标是正常状态，还是开箱


                if (packingQc == null)
                {


                    //工单如果是已经关闭，不验证是否QC确认

                    //获取蓝标QC确认
                    var packingQcData = RT.Service.Resolve<PackingQcController>().GetPackingQcByState((double)KZWorkstation.ResourceId);
                    foreach (var item in packingQcData)
                    {
                        if (item.BlueLabel != Barcode)
                        {
                            if (item.Confirm == ConfirmEnum.NO)
                            {
                                var qcWo = RT.Service.Resolve<PackingQcController>().GetWoId(item.Id);
                                if (qcWo == null || qcWo.State == Core.WorkOrders.WorkOrderState.Close || qcWo.State == Core.WorkOrders.WorkOrderState.Finish)
                                    continue;
                                var qcWoid = qcWo.Id;
                                var dispatch = tasks.FirstOrDefault(p => p.WorkOrderId == qcWoid && p.ProcessName.Contains("包装"));
                                if (dispatch.TaskStatus != DispatchTaskStatus.Closed)
                                {
                                    SoundPlayer();
                                    Error = "蓝标标签[" + item.BlueLabel + "]QC未确认,不允许使用其它蓝标标签!";
                                    Reset();
                                    BoolBlue = true;
                                    return;
                                }
                            }
                        }
                    }

                }
                else
                {
                    //开箱
                    var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
                    //记录已经装了多少箱数量
                    blueInt = packDetailList.Count;
                    PackageSnRecordList.Clear();
                    foreach (var item in packDetailList)
                    {
                        PackingQcModel pqc = new PackingQcModel();
                        pqc.BlueLabel = XtBlue;
                        pqc.Confirm = item.Confirm;
                        pqc.PackIdent = packingQc.PackIdent;
                        pqc.ProductLabel = item.ProductLabel;
                        pqc.ItemId = packingQc.ItemId;
                        pqc.ItemName = packingQc.Item.Name;
                        pqc.ResourceId = PackresourceId;
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
                        LevelString(blueLableLevel.LevelName);
                        //临时计算当前行
                        var lsCount = blueInt;
                        //当前行数
                        var dqCount = 0;
                        for (int i = 0; i < levelTargets.Length; i++)
                        {
                            lsCount = lsCount - levelTargets[i];
                            if (i == 0)
                            {
                                if (lsCount < 0)
                                {
                                    dqCount = blueInt;
                                }
                            }
                            //如果等于0 就直接去下一层
                            if (lsCount == 0)
                            {
                                currentCount = 0;
                                currentLevelIndex = i + 1;
                                break;
                            }
                            //如果小于0 就去当前数，做当前层，和当前累计值
                            if (lsCount < 0)
                            {
                                currentCount = dqCount;
                                currentLevelIndex = i;
                                break;
                            }
                            if (lsCount > 0)
                                dqCount = lsCount;
                        }
                    }
                    else
                    {
                        SoundPlayer();
                        Error = "该蓝标已经封箱,请先点击开箱按钮!";
                        Barcode = "";
                        //TcTs("该蓝标已经封箱,请先点击开箱按钮");
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
                //批次标签
                WipBatch wipBatch = new WipBatch();
                //包装采集主表
                PackingQc packingQc = new PackingQc();
                //Sn标签
                WipPressureSn wipSn = new WipPressureSn();
                //耐压测试
                WipPressure wipPressure = new WipPressure();
                string pcSn = "";
                if (SnIdent == 0)
                {
                    //获取蓝标QC确认
                    packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
                    if (packingQc == null)
                    {
                        //第一次进入数据
                        SnIdent = 1;
                    }
                    else
                    {
                        //开箱
                        var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
                        //获取工序标签
                        var productLabel = packDetailList.FirstOrDefault().ProductLabel;
                        //查看标签在批次下SnIdent == 2，还SN下面SnIdent == 3
                        if (RT.Service.Resolve<WipBatchController>().GetWipBatchPc(productLabel) != null)
                        {
                            SnIdent = 2;
                        }
                        else
                        {
                            SnIdent = 3;
                        }
                    }
                }
                if (SnIdent == 1)
                {
                    //判断在批次标签中是否存在
                    wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatchPc(Barcode);
                    if (wipBatch == null)
                    {
                        wipSn = RT.Service.Resolve<WipPressureController>().GetWipPressureSn(Barcode);
                        if (wipSn == null)
                        {
                            SoundPlayer();
                            Error = "当前SN二维码不存在!";
                            Barcode = "";
                            TcTs(Error);
                            return;
                        }
                        SnIdent = 3;

                    }
                    else
                    {
                        if (wipBatch.IsSuspectProduct == YesNo.Yes)
                        {
                            SoundPlayer();
                            Error = "当前SN二维码是可疑品,不允许使用!";
                            Barcode = "";
                            TcTs(Error);
                            return;
                        }
                        if (wipBatch.IsRework)
                        {
                            SoundPlayer();
                            Error = "当前SN二维码已返工,不允许使用!";
                            Barcode = "";
                            TcTs(Error);
                            return;
                        }
                        if (wipBatch.IsScraped)
                        {
                            SoundPlayer();
                            Error = "当前SN二维码是报废品,不允许使用!";
                            Barcode = "";
                            TcTs(Error);
                            return;
                        }
                        SnIdent = 2;
                    }
                }
                if (SnIdent == 2)
                {
                    //获取蓝标QC确认
                    packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
                    //第一步 工序标签是否存在
                    wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatchPc(Barcode);
                    pcSn = wipBatch.BatchNo;
                    if (wipBatch == null)
                    {
                        SoundPlayer();
                        Error = "批次标签不存在!";
                        Barcode = "";
                        //TcTs("批次标签不存在");
                        return;
                    }
                    if (wipBatch.Isuse)
                    {
                        SoundPlayer();
                        Error = "批次标签已经使用过!";
                        Barcode = "";
                        //TcTs("批次标签已经使用过");
                        return;
                    }
                }
                if (SnIdent == 3)
                {
                    //获取蓝标QC确认
                    packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
                    //第一步查找SN是否存在，是否使用
                    wipSn = RT.Service.Resolve<WipPressureController>().GetWipPressureSn(Barcode);
                    if (wipSn == null)
                    {
                        SoundPlayer();
                        Error = "SN标签不存在!";
                        Barcode = "";
                        TcTs(Error);
                        return;
                    }
                    if (wipSn.WipPressure == null)
                    {
                        SoundPlayer();
                        Error = "SN二维码不存在!";
                        Barcode = "";
                        TcTs(Error);
                        return;
                    }
                    pcSn = wipSn.WipPressure.BatchNo;

                    if (wipSn.Isuse)
                    {
                        SoundPlayer();
                        Error = "SN二维码已经使用过!";
                        Barcode = "";
                        //System.Windows.MessageBox.Show("SN标签已经使用过,请先输入验证码!!!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        TcTs(Error);
                        return;
                    }

                    if (wipSn.IsSuspectProduct)
                    {
                        SoundPlayer();
                        Error = "SN二维码是可疑品,不允许使用!";
                        Barcode = "";
                        TcTs(Error);
                        return;
                    }
                    if (wipSn.IsScraped)
                    {
                        SoundPlayer();
                        Error = "SN二维码是报废品,不允许使用!";
                        Barcode = "";
                        TcTs(Error);
                        return;
                    }
                    if (wipSn.IsRework)
                    {
                        SoundPlayer();
                        Error = "SN二维码已返工,不允许使用!";
                        Barcode = "";
                        TcTs(Error);
                        return;
                    }

                    //第二步 去耐压测试 去找批次标签，找到工单
                    wipPressure = RT.Service.Resolve<WipPressureController>().GetWipPressureById(wipSn.WipPressureId);

                    //第三步 获取工单
                    wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatchPc(wipPressure.BatchNo);
                }



                //第二步 判断工单是否一致
                if (WorkOrderId != wipBatch.WorkOrderId)
                {
                    SoundPlayer();
                    Error = "SN二维码没在一个工单下面!当前标签批次[" + pcSn + "],工单[" + wipBatch.WorkOrder.No + "]";
                    Barcode = "";
                    SnIdent = 1;
                    TcTs("SN二维码没在一个工单下面!当前标签批次[" + pcSn + "],工单[" + wipBatch.WorkOrder.No + "]");
                    return;
                }

                //获取工单
                var workOrderData = RT.Service.Resolve<WipBatchController>().GetWorkOrder(WorkOrderId);
                //获取产品
                var itemData = RT.Service.Resolve<WipBatchController>().GetItem(workOrderData.ProductId);
                //如果当前层数是0的时候,当前层级也是0
                //必须要先判断是否还有其他蓝标在使用
                if (currentCount == 0 && currentLevelIndex == 0)
                {
                    //判断蓝标，是否可以继续
                }

                if (currentCount == 0 && currentLevelIndex != 0)
                {
                    //判断工序标签是否可以继续
                    if (packingQc.Confirm == ConfirmEnum.NO)
                    {
                        Error = "请QC确认完成后,继续扫工序标签!";
                        Barcode = "";
                        //TcTs("请QC确认完成后,继续扫工序标签");
                        return;
                    }
                    else
                    {
                        if (PackageSnRecordList != null)
                        {
                            foreach (var item in PackageSnRecordList)
                            {
                                item.Confirm = ConfirmEnum.YES;
                            }
                        }
                    }
                }
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

                packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);

                if (packingQc != null)
                {
                    if (packingQc.PackingDetailList.Count >= packingQc.BlueLableNum)
                    {
                        SoundPlayer();
                        Error = "蓝标的装箱数量：" + packingQc.BlueLableNum + ",请QC确认";
                        return;
                    }
                }

                var isErr = false;
                PackingDetail packingDetail = null;
                try
                {
                    //界面显示明细
                    PackingQcModel pqc = new PackingQcModel();
                    pqc.BlueLabel = XtBlue;
                    pqc.Confirm = ConfirmEnum.NO;
                    pqc.PackIdent = PackIdentEnum.NotFullTank;
                    pqc.ProductLabel = Barcode;
                    pqc.ItemId = itemData.Id;
                    pqc.ItemName = itemData.Name;
                    pqc.ResourceId = (double)KZWorkstation.ResourceId;
                    PackageSnRecordList.Add(pqc);
                    blueInt += 1;

                    if (packingQc == null)
                    {
                        packingQc = new PackingQc();
                        packingQc.GenerateId();
                        packingQc.BlueLabel = XtBlue;
                        packingQc.Confirm = ConfirmEnum.NO;
                        packingQc.PackIdent = PackIdentEnum.NotFullTank;
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
                        if (blueInt == blueBable.PackageNum)
                        {
                            packingQc.PackIdent = PackIdentEnum.FullTank;
                        }
                        else
                        {
                            packingQc.PackIdent = PackIdentEnum.NotFullTank;
                        }

                        packingQc.Confirm = ConfirmEnum.NO;
                        packingQc.PackingNum = blueInt;
                        blueBable.IsPack = true;
                    }
                    //存入包装QC确认表PackingNum
                    //RF.Save(packingQc);

                    //存入包装QC确认明细表
                    packingDetail = new PackingDetail();
                    packingDetail.ProductLabel = Barcode;
                    packingDetail.PackingQcId = packingQc.Id;
                    packingDetail.Confirm = ConfirmEnum.NO;
                    packingDetail.BatchLabel = pcSn;
                    packingDetail.WorkOrderNo = WorkOrder.No;
                    if (SnIdent == 2)
                        packingDetail.LabelType = LabelTypeEnum.BatchLabel;
                    else if (SnIdent == 3)
                        packingDetail.LabelType = LabelTypeEnum.SnLabel;
                    else
                    {
                        SoundPlayer();
                        Error = "当前标签类型错误,请选中耐压标签,或者批次标签!";
                        //TcTs("当前标签类型错误,请选中耐压标签,或者批次标签");
                        return;
                    }
                    packingDetail.ReportsType = ReportsTypeEnum.NO;
                    packingDetail.PackingNum = 1;
                    //RF.Save(packingDetail);

                    //修改批次生成 工序标签是否已经使用
                    if (SnIdent == 2)
                    {
                        wipBatch.Isuse = true;
                        //RF.Save(wipBatch);
                    }
                    else if (SnIdent == 3)
                    {
                        wipSn.Isuse = true;
                        //RF.Save(wipSn);
                    }

                    if (blueLableLevel.LevelName == "")
                    {
                        SoundPlayer();
                        Error = "请维护蓝标层级!";
                        //TcTs("当前标签类型错误,请选中耐压标签,或者批次标签");
                        return;
                    }
                    try
                    {
                        //获取层级
                        LevelString(blueLableLevel.LevelName);
                        // 增加计数
                        currentCount++;

                        if (currentCount >= levelTargets[currentLevelIndex])
                        {
                            currentLevelIndex++;
                            System.Windows.MessageBox.Show("蓝标的装箱数量：" + blueInt + ",已到达第【" + currentLevelIndex + "】层,请QC确认", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            currentCount = 0;

                            if (blueInt == blueBable.PackageNum)
                            {
                                PackageSnRecordList.Clear();
                                BoolBlue = true;
                                XtBlue = "";
                                blueInt = 0;
                                blueZInt = 0;
                                XtBlue = "";
                                blueBable = null;
                                Tips = "请输入蓝标标签!";
                                Barcode = "";
                                SnIdent = 0;
                                packingQc.BoxState = BoxStateEnum.NO;
                                //RF.Save(packingQc);
                                return;
                            }

                        }
                    }
                    catch (Exception)
                    {
                        isErr = true;
                        SoundPlayer();
                        PackageSnRecordList.Remove(pqc);
                        Error = "蓝标层级总数为" + blueLableLevel.LevelName + ",已超过总数，请检查蓝标层级";
                        //TcTs("当前标签类型错误,请选中耐压标签,或者批次标签");                      
                        return;
                    }

                    if (blueInt == blueZInt)
                    {
                        System.Windows.MessageBox.Show("蓝标的装箱数量：" + blueInt + ",已经是尾箱,请QC确认", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        PackageSnRecordList.Clear();
                        BoolBlue = true;
                        XtBlue = "";
                        blueInt = 0;
                        XtBlue = "";
                        blueZInt = 0;
                        blueBable = null;
                        Tips = "请输入蓝标标签!";
                        Barcode = "";
                        SnIdent = 0;
                        currentLevelIndex = 0;
                        currentCount = 0;
                        blueLableLevel = null;
                        levelTargets = null;
                        packingQc.BoxState = BoxStateEnum.NO;
                        //RF.Save(packingQc);
                        return;
                    }
                    Tips = "请输入工序标签!";
                    Barcode = "";
                }
                catch (Exception ex)
                {
                    isErr = true;
                    Error = ex.Message;
                    //CRT.MessageService.ShowError(ex.Message);
                    return;
                }
                finally
                {
                    //如果catch捕捉到报错，就不保存
                    if (isErr == false)
                    {
                        //保存数据
                        RT.Service.Resolve<PackingQcController>().PackingQcSave(ref packingQc, ref wipSn, ref wipBatch, ref packingDetail, blueBable);
                    }
                }
            }
            //base.OnBarcodeChanged(e);
        }

        /// <summary>
        /// 弹出提示
        /// </summary>
        public void TcTs(string name)
        {
            VerifyCode = null;
            var template = new DetailsUITemplate(typeof(PackingQcViewModel), PackingQcViewModelViewConfig.VerifyCodeView);
            var ui = template.CreateUI();
            ui.MainView.Data = this;
            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
            {
                w.Title = name.L10N();
                // w.Title = "<span style='color:red'>SN有重码风险,请确认排除</span>".L10N();
                //w.commamds.clear(),
                w.Commands.Remove("取消");
                w.Height = 200;
                w.Width = 400;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (RT.Service.Resolve<PackingQcController>().VerifyCode(VerifyCode))
                        {
                            //WipPressure.IsAllowOver = true;
                            Error = null;
                        }
                        else
                        {
                            CRT.MessageService.ShowError("验证码不正确!");
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                };
            });
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            PackageSnRecordList.Clear();
            blueInt = 0;
            blueZInt = 0;
            XtBlue = "";
            currentCount = 0;
            currentLevelIndex = 0;
            blueBable = null;
            blueLableLevel = null;
            BoolBlueLine = false;
            SnIdent = 0;
            PackresourceId = 0;
            Barcode = "";
            DeleteIdent = 0;
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


        /// <summary>
        /// 移除批次标签
        /// </summary>
        /// <param name="barcode"></param>
        public string DeleteLabel(string barcode)
        {
            //主表
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
            //从表
            var packingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
            var wipPressureSn = RT.Service.Resolve<WipPressureController>().GetWipPressureSn(Barcode);
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
                        wipPressureSn.Isuse = false;
                        //RF.Save(wipPressureSn);

                        if (packingDetailList.Count == 1)
                        {
                            packingQc.PersistenceStatus = PersistenceStatus.Deleted;
                            RT.Service.Resolve<PackingQcController>().DeleteSave(packDetail, wipPressureSn, packingQc);
                            Reset();

                        }
                        else
                        {
                            RT.Service.Resolve<PackingQcController>().DeleteSave(packDetail, wipPressureSn, packingQc);

                            PackageSnRecordList.Clear();
                            var NewPackingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                            foreach (var item in NewPackingDetailList)
                            {
                                PackingQcModel pqc = new PackingQcModel();
                                pqc.BlueLabel = XtBlue;
                                pqc.Confirm = item.Confirm;
                                pqc.PackIdent = packingQc.PackIdent;
                                pqc.ProductLabel = item.ProductLabel;
                                pqc.ItemId = packingQc.ItemId;
                                pqc.ItemName = packingQc.Item.Name;
                                pqc.ResourceId = PackresourceId;
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
