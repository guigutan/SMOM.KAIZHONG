using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.EventMessages.MES.Dispatchs;
using SIE.ManagedProperty;
using SIE.MES.BlueLable;
using SIE.MES.PackingQC;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PackingQcs;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Wpf.MES.PackingQC;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIE.Wpf.MES.NewPackingQC
{
    /// <summary>
    /// B包装采集
    /// </summary>
    [RootEntity]
    [Label("B包装采集")]
    public class NewPackingQcViewModel : KZDataCollectionViewModel
    {
        public NewPackingQcViewModel()
        {
            InitWorkstation();
        }

        #region 包装明细
        /// <summary>
        /// 包装明细
        /// </summary>
        public static readonly ListProperty<EntityList<NewPackingQcModel>> PackageSnRecordListProperty = P<NewPackingQcViewModel>.RegisterList(e => e.PackageSnRecordList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as NewPackingQcViewModel).LoadPackageSnRecordList()
        });

        /// <summary>
        /// 包装明细
        /// </summary>
        public EntityList<NewPackingQcModel> PackageSnRecordList
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
        public static readonly Property<int> blueIntProperty = P<NewPackingQcViewModel>.Register(e => e.blueInt);

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
        public static readonly Property<int> blueZIntProperty = P<NewPackingQcViewModel>.Register(e => e.blueZInt);

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
        public static readonly Property<string> XtBlueProperty = P<NewPackingQcViewModel>.Register(e => e.XtBlue);

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
        public static readonly Property<string> YXtBlueProperty = P<NewPackingQcViewModel>.Register(e => e.YXtBlue);

        /// <summary>
        /// 原蓝标
        /// </summary>
        public string YXtBlue
        {
            get { return this.GetProperty(YXtBlueProperty); }
            set { this.SetProperty(YXtBlueProperty, value); }
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
                //蓝标层级
                blueLableLevel = RT.Service.Resolve<PackingQcController>().GetBlueLableLevel(blueBable.ItemId);

                if (blueLableLevel == null)
                {
                    Error = "该蓝标没有维护层级!";
                    Barcode = "";
                    return;
                }
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
                        NewPackingQcModel pqc = new NewPackingQcModel();
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
                            Error = "当前工序标签不存在!";
                            Barcode = "";
                            return;
                        }
                        if (wipSn.IsSuspectProduct == true)
                        {
                            Error = "SN二维码是可疑品,不允许使用!";
                            Barcode = "";
                            return;
                        }
                        if (wipSn.IsRework)
                        {
                            Error = "SN二维码已返工,不允许使用!";
                            Barcode = "";
                            return;
                        }
                        if (wipSn.IsScraped)
                        {
                            Error = "SN二维码是报废品,不允许使用!";
                            Barcode = "";
                            return;
                        }

                        SnIdent = 3;
                    }
                    else
                    {
                        if (wipBatch.IsSuspectProduct == YesNo.Yes)
                        {
                            Error = "当前工序标签是可疑品,不允许使用!";
                            Barcode = "";
                            return;
                        }
                        if (wipBatch.IsRework)
                        {
                            Error = "当前工序标签已返工,不允许使用!";
                            Barcode = "";
                            return;
                        }
                        if (wipBatch.IsScraped)
                        {
                            Error = "当前工序标签是报废品,不允许使用!";
                            Barcode = "";
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
                        Error = "批次标签不存在!";
                        Barcode = "";
                        return;
                    }
                    if (wipBatch.Isuse)
                    {
                        Error = "批次标签已经使用过!";
                        Barcode = "";
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
                        Error = "SN标签不存在!";
                        Barcode = "";
                        return;
                    }
                    if (wipSn.WipPressure == null)
                    {
                        Error = "SN标签不存在!";
                        Barcode = "";
                        return;
                    }
                    pcSn = wipSn.WipPressure.BatchNo;

                    if (wipSn.Isuse)
                    {
                        Error = "SN标签已经使用过!";
                        Barcode = "";
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
                    Error = "工序标签没在一个工单下面!当前标签批次[" + pcSn + "],工单[" + wipBatch.WorkOrder.No + "]";
                    Barcode = "";
                    SnIdent = 1;
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
                #region 存入数据库

                //界面显示明细
                PackingDetail packingDetail = new PackingDetail();
                NewPackingQcModel pqc = new NewPackingQcModel();
                pqc.BlueLabel = XtBlue;
                pqc.Confirm = ConfirmEnum.NO;
                pqc.PackIdent = PackIdentEnum.NotFullTank;
                pqc.ProductLabel = Barcode;

                pqc.ItemId = itemData.Id;
                pqc.ItemName = itemData.Name;
                pqc.ResourceId = (double)KZWorkstation.ResourceId;


                //获取层级
                LevelString(blueLableLevel.LevelName);
                if (blueLableLevel.CalcMethod == CalcMethodEnum.BatchLable)
                {
                    blueInt += (int)wipBatch.Qty;
                    currentCount = blueInt;
                    packingDetail.PackingNum = (int)wipBatch.Qty;
                    pqc.PackingNum = (int)wipBatch.Qty;

                    if (currentCount > levelTargets[currentLevelIndex])
                    {
                        Error = "当前批次数量大于蓝标第一层的数量,当前批次数量：【" + (int)wipBatch.Qty + "】,层级数量：【" + levelTargets[currentLevelIndex] + "】";
                        Barcode = "";
                        blueInt -= (int)wipBatch.Qty;
                        currentCount = blueInt;
                        return;
                    }
                    if (currentCount > blueZInt)
                    {
                        Error = "当前批次数量大于蓝标装箱数,当前批次数量：【" + (int)wipBatch.Qty + "】,蓝标装箱数量：【" + blueZInt + "】";
                        Barcode = "";
                        blueInt -= (int)wipBatch.Qty;
                        currentCount = blueInt;
                        return;
                    }
                }
                else
                {
                    blueInt += 1;
                    // 增加计数
                    currentCount++;
                    packingDetail.PackingNum = 1;
                    pqc.PackingNum = 1;
                }
                PackageSnRecordList.Add(pqc);

                if (packingQc == null)
                {
                    packingQc = new PackingQc();
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
                //存入包装QC确认表
                RF.Save(packingQc);
                RF.Save(blueBable);
                //存入包装QC确认明细表

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
                    Error = "当前标签类型错误,请选中耐压标签,或者批次标签!";
                    return;
                }
                packingDetail.ReportsType = ReportsTypeEnum.NO;

                RF.Save(packingDetail);

                //修改批次生成 工序标签是否已经使用
                if (SnIdent == 2)
                {
                    wipBatch.Isuse = true;
                    RF.Save(wipBatch);
                }
                else if (SnIdent == 3)
                {
                    wipSn.Isuse = true;
                    RF.Save(wipSn);
                }
                #endregion






                if (currentCount >= levelTargets[currentLevelIndex])
                {
                    currentLevelIndex++;
                    MessageBox.Show("蓝标的装箱数量：" + blueInt + ",已到达第【" + currentLevelIndex + "】层,请QC确认", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        RF.Save(packingQc);
                        return;
                    }

                }
                if (blueInt == blueZInt)
                {
                    MessageBox.Show("蓝标的装箱数量：" + blueInt + ",已经是尾箱,请QC确认", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    packingQc.BoxState = BoxStateEnum.NO;
                    RF.Save(packingQc);
                    return;
                }
                Tips = "请输入工序标签!";
                Barcode = "";
            }
            base.OnBarcodeChanged(e);
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
        /// 移除批次标签
        /// </summary>
        /// <param name="barcode"></param>
        public string DeleteLabel(string barcode)
        {
            //主表
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
            //从表
            var packingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatchPc(Barcode);
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
                        //RF.Save(wipPressureSn);

                        if (packingDetailList.Count == 1)
                        {
                            packingQc.PersistenceStatus = PersistenceStatus.Deleted;
                            RT.Service.Resolve<PackingQcController>().DeleteCSave(packDetail, wipBatch, packingQc);
                            Reset();

                        }
                        else
                        {
                            RT.Service.Resolve<PackingQcController>().DeleteCSave(packDetail, wipBatch, packingQc);

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
