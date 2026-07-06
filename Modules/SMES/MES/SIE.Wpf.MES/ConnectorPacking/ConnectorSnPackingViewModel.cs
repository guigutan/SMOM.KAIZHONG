using DevExpress.CodeParser;
using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.EventMessages.MES.Dispatchs;
using SIE.ManagedProperty;
using SIE.MES.BlueLable;
using SIE.MES.Engrave;
using SIE.MES.PackingQC;
using SIE.MES.PackRule;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PackingQcs;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SIE.Wpf.MES.ConnectorPacking
{
    /// <summary>
    /// 连接器器包装采集
    /// </summary>
    [RootEntity]
    [Label("连接器单个包装采集")]
    public class ConnectorSnPackingViewModel : KZDataCollectionViewModel
    {
        public ConnectorSnPackingViewModel()
        {
            InitWorkstation();
        }

        #region 包装明细
        /// <summary>
        /// 包装明细
        /// </summary>
        public static readonly ListProperty<EntityList<ConnectorSnPackingModel>> PackageSnRecordListProperty = P<ConnectorSnPackingViewModel>.RegisterList(e => e.PackageSnRecordList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as ConnectorSnPackingViewModel).LoadPackageSnRecordList()
        });

        /// <summary>
        /// 包装明细
        /// </summary>
        public EntityList<ConnectorSnPackingModel> PackageSnRecordList
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
        public static readonly Property<int> blueIntProperty = P<ConnectorSnPackingViewModel>.Register(e => e.blueInt);

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
        public static readonly Property<int> blueZIntProperty = P<ConnectorSnPackingViewModel>.Register(e => e.blueZInt);

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
        public static readonly Property<string> XtBlueProperty = P<ConnectorSnPackingViewModel>.Register(e => e.XtBlue);

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
        public static readonly Property<string> YXtBlueProperty = P<ConnectorSnPackingViewModel>.Register(e => e.YXtBlue);

        /// <summary>
        /// 原蓝标
        /// </summary>
        public string YXtBlue
        {
            get { return this.GetProperty(YXtBlueProperty); }
            set { this.SetProperty(YXtBlueProperty, value); }
        }
        #endregion

        #region 批次标签 BatchLable
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        public static readonly Property<string> BatchLableProperty = P<ConnectorSnPackingViewModel>.Register(e => e.BatchLable);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchLable
        {
            get { return this.GetProperty(BatchLableProperty); }
            set { this.SetProperty(BatchLableProperty, value); }
        }
        #endregion

        #region 批次总数量 BatchZInt
        /// <summary>
        /// 批次总数量
        /// </summary>
        [Label("批次总数量")]
        public static readonly Property<int> BatchZIntProperty = P<ConnectorSnPackingViewModel>.Register(e => e.BatchZInt);

        /// <summary>
        /// 批次总数量
        /// </summary>
        public int BatchZInt
        {
            get { return this.GetProperty(BatchZIntProperty); }
            set { this.SetProperty(BatchZIntProperty, value); }
        }
        #endregion

        #region 批次已装数量 BatchInt
        /// <summary>
        /// 批次已装数量
        /// </summary>
        [Label("批次已装数量")]
        public static readonly Property<int> BatchIntProperty = P<ConnectorSnPackingViewModel>.Register(e => e.BatchInt);

        /// <summary>
        /// 批次已装数量
        /// </summary>
        public int BatchInt
        {
            get { return this.GetProperty(BatchIntProperty); }
            set { this.SetProperty(BatchIntProperty, value); }
        }
        #endregion

        #region 状态 DeleteState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> DeleteStateProperty = P<ConnectorSnPackingViewModel>.Register(e => e.DeleteState);

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
        public static readonly Property<string> VerifyCodeProperty = P<ConnectorSnPackingViewModel>.Register(e => e.VerifyCode);

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
        public int BoolBlue { get; set; } = 1;


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
            if (BoolBlue == 1)
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
                    BatchInt = 0;
                    BatchZInt = 0;
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
                    BatchInt = 0;
                    BatchZInt = 0;

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
                            var detailSum = packdetails.Sum(p => p.PackingNum);
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
                                Error = "换箱失败,新蓝标数量【" + xtBlue.PackageNum + "】,原蓝标数量【" + yXtBlue.PackageNum + "】已装箱数【" + detailSum + "】";
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
                var msg = RT.Service.Resolve<ConnectorSnPackingController>().ValidBlueLable(blueBable);
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
                if (blueLableLevel != null && !string.IsNullOrEmpty(blueLableLevel.LevelName))
                {
                    LevelString(blueLableLevel.LevelName);
                }
                else if (blueLableLevel != null && string.IsNullOrEmpty(blueLableLevel.LevelName))
                {
                    // 蓝标层级名称为空，视为未维护层级
                    blueLableLevel = null;
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
                                BoolBlue = 1;
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
                        ConnectorSnPackingModel pqc = new ConnectorSnPackingModel();
                        pqc.BlueLabel = XtBlue;
                        pqc.Confirm = item.Confirm;
                        pqc.PackIdent = packingQc.PackIdent;
                        pqc.ProductLabel = item.ProductLabel;
                        pqc.PackingNum = item.PackingNum;
                        pqc.ItemId = packingQc.ItemId;
                        pqc.ItemName = packingQc.Item.Name;
                        pqc.ResourceId = PackresourceId;
                        pqc.BatchLabel = item.BatchLabel;
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
                        //开箱后恢复蓝标层级状态
                        if (blueLableLevel != null && levelTargets != null && levelTargets.Length > 0)
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
                    }
                    else
                    {
                        Error = "该蓝标已经封箱,请先点击开箱按钮!";
                        Barcode = "";
                        return;
                    }
                }


                XtBlue = Barcode;
                BoolBlue = 2;
                Barcode = "";
                Tips = "请输入批次标签!";
            }
            else if (BoolBlue == 2)
            {
                //批次标签
                WipBatch wipBatch = new WipBatch();
                //刻码标签
                EngraveLabel engrave = new EngraveLabel();
                EntityList<EngraveSn> labelSn = new EntityList<EngraveSn>();
                //第一步 批次标签是否存在
                wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatchPc(Barcode);
                if (wipBatch == null)
                {
                    Error = "批次标签不存在!";
                    Barcode = "";
                    BoolBlue = 2;
                    return;
                }

                if (WorkOrder.Id != wipBatch.WorkOrderId)
                {
                    Error = "批次标签的工单,和蓝标的工单不一致!";
                    Barcode = "";
                    BoolBlue = 2;
                    return;
                }
                BatchLable = Barcode;
                BatchZInt = (int)wipBatch.Qty;
                engrave = RT.Service.Resolve<EngraveLabelController>().BoolEngraveLabel(Barcode);
                if (engrave == null)
                {
                    engrave = new EngraveLabel();
                    engrave.BatchNo = Barcode;
                    engrave.WorkOrderId = wipBatch.WorkOrderId;
                    engrave.ResourceId = KZWorkstation.ResourceId;
                    engrave.ProductId = wipBatch.WorkOrder.ProductId;
                    engrave.Qty = wipBatch.Qty;
                    bool saveEngrave = RT.Service.Resolve<EngraveLabelController>().SaveEngraveLabel(engrave);
                    if (saveEngrave == false)
                    {
                        Error = "刻码标签保存失败!";
                        Barcode = "";
                        BoolBlue = 2;
                        return;
                    }
                }
                else
                {
                    BatchZInt = (int)engrave.Qty;
                    labelSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSns(engrave.Id);
                    if (labelSn.Count > 0)
                        BatchInt = labelSn.Count;
                    else
                        BatchInt = 0;
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

                BoolBlue = 3;
                Barcode = "";
                Tips = "请输入SN标签!";
                return;
            }
            else if (BoolBlue == 3)
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
                //蓝标层级QC确认检查
                if (blueLableLevel != null && currentCount == 0 && currentLevelIndex != 0)
                {
                    var checkPackingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
                    if (checkPackingQc != null && checkPackingQc.Confirm == ConfirmEnum.NO)
                    {
                        Error = "请QC确认完成后,继续扫SN标签!";
                        Barcode = "";
                        BoolBlue = 3;
                        return;
                    }
                    else if (checkPackingQc != null)
                    {
                        foreach (var item in PackageSnRecordList)
                        {
                            item.Confirm = ConfirmEnum.YES;
                        }
                    }
                }
                //包装采集主表
                PackingQc packingQc = new PackingQc();
                //获取工单
                var workOrderData = RT.Service.Resolve<WipBatchController>().GetWorkOrder(WorkOrderId);
                //获取产品
                var itemData = RT.Service.Resolve<WipBatchController>().GetItem(workOrderData.ProductId);
                if (RT.Service.Resolve<EngraveLabelController>().BoolEngraveSn(Barcode))
                {
                    Error = "刻码SN已经存在!";
                    BoolBlue = 3;
                    Barcode = "";
                    TcTs(Error);
                    return;
                }

                #region 包装规则验证
                var itemQrRule = RT.Service.Resolve<PackRuleController>().GetItemQRCodeRule(itemData.Id);
                if (itemQrRule == null)
                {
                    Error = "当前产品,没有维护包装物料二维码规则关系!";
                    BoolBlue = 3;
                    Barcode = "";
                    return;
                }

                var qRCodeRule = RT.Service.Resolve<PackRuleController>().GetQRCodeRule(itemQrRule.QRCodeRuleId);
                if (qRCodeRule == null)
                {
                    Error = "包装二维码规则维护没有!";
                    BoolBlue = 3;
                    Barcode = "";
                    return;
                }
                if (itemQrRule.IsQualityStatus == true)
                {
                    //验证质量状态，如果产品勾选了这个，则一定要在“工序参数数采”里面存在记录，并且最新的那条是OK的
                    var collect = RT.Service.Resolve<ProcessParamCollectController>().GetProcessParamCollect(Barcode);
                    if (collect == null)
                    {
                        Error = "SN不存在工序参数数采记录，请检查!".L10N();
                        BoolBlue = 3;
                        Barcode = "";
                        TcTs(Error);
                        return;
                    }
                    if (collect.QualityStatus == Core.Common.InspResult.NG)
                    {
                        Error = "产品质量不合格，请检查!".L10N();
                        BoolBlue = 3;
                        Barcode = "";
                        TcTs(Error);
                        return;
                    }
                }
                //总位数验证
                if (qRCodeRule.TotalDigit != null)
                {
                    if (qRCodeRule.TotalDigit != "")
                    {
                        int total = int.Parse(qRCodeRule.TotalDigit);
                        if (Barcode.Length != total)
                        {
                            Error = "当前二维码的长度跟规则不匹配,当前总位数是：【" + total + "】,规则总位数【" + Barcode.Length + "】!";
                            BoolBlue = 3;
                            Barcode = "";
                            return;
                        }
                    }
                }

                //客户零件号验证 开始，结束
                int khljStart = -1;
                int khljEnd = -1;
                if (qRCodeRule.CustomPnStartDigit != null)
                {
                    if (qRCodeRule.CustomPnStartDigit != "")
                    {
                        khljStart = int.Parse(qRCodeRule.CustomPnStartDigit);
                    }
                }
                if (qRCodeRule.CustomPnEndDigit != null)
                {
                    if (qRCodeRule.CustomPnEndDigit != "")
                    {
                        khljEnd = int.Parse(qRCodeRule.CustomPnEndDigit);
                    }
                }
                if (khljStart != -1 && khljEnd != -1)
                {
                    string khlj = Barcode.Substring(khljStart - 1, khljEnd - khljStart + 1);
                    if (khlj != itemQrRule.CustomPn)
                    {
                        Error = "二维码中的客户零件号不正确,当前客户零件号【" + khlj + "】,规则客户零件号【" + itemQrRule.CustomPn + "】!";
                        BoolBlue = 3;
                        Barcode = "";
                        return;
                    }
                }
                //客户版本号验证 开始，结束
                int khbbStart = -1;
                int khbbEnd = -1;
                if (qRCodeRule.VersionNumberStartDigit != null)
                {
                    if (qRCodeRule.VersionNumberStartDigit != "")
                    {
                        khbbStart = int.Parse(qRCodeRule.VersionNumberStartDigit);
                    }
                }
                if (qRCodeRule.VersionNumberEndDigit != null)
                {
                    if (qRCodeRule.VersionNumberEndDigit != "")
                    {
                        khbbEnd = int.Parse(qRCodeRule.VersionNumberEndDigit);
                    }
                }
                if (khbbStart != -1 && khbbEnd != -1)
                {
                    string khbb = Barcode.Substring(khbbStart - 1, khbbEnd - khbbStart + 1);
                    if (khbb != itemQrRule.VersionNumber)
                    {
                        Error = "二维码中的客户版本号不正确,当前客户版本号【" + khbb + "】,规则客户零件号【" + itemQrRule.VersionNumber + "】!";
                        BoolBlue = 3;
                        Barcode = "";
                        return;
                    }
                }


                //序号验证 开始，结束
                int xhStart = -1;
                int xhEnd = -1;
                if (qRCodeRule.SerialNumberStartDigit != null)
                {
                    if (qRCodeRule.SerialNumberEndDigit != "")
                    {
                        xhStart = int.Parse(qRCodeRule.SerialNumberStartDigit);
                    }
                }
                if (qRCodeRule.SerialNumberEndDigit != null)
                {
                    if (qRCodeRule.SerialNumberEndDigit != "")
                    {
                        xhEnd = int.Parse(qRCodeRule.SerialNumberEndDigit);
                    }
                }
                if (xhStart != -1 && xhEnd != -1)
                {
                    string xlh = Barcode.Substring(xhStart - 1, xhEnd - xhStart + 1);
                    var packingDetails = RT.Service.Resolve<PackingQcController>().GetPackingDetails(xlh);
                    if (packingDetails.Count > 0)
                    {
                        foreach (var item in packingDetails)
                        {
                            string label = item.ProductLabel.Substring(xhStart - 1, xhEnd - xhStart + 1);
                            if (xlh == label)
                            {
                                Error = "有一样的序列号，当前序列号二维码：【" + Barcode + "】,以扫过的序列号二维码：【" + item.ProductLabel + "】!";
                                BoolBlue = 3;
                                Barcode = "";
                                return;
                            }
                        }
                    }
                }


                #endregion

                //try
                //{
                //    //SN和批次标签校验 前置是否报工
                //    RT.Service.Resolve<ITaskReportKZ>().ValidatePrepareProcessHasReport(BatchLable, "成品包装");
                //}
                //catch (Exception ex)
                //{
                //    Error = ex.GetBaseException().Message;
                //    Barcode = "";
                //    return;
                //}




                blueInt += 1;
                BatchInt += 1;
                //蓝标层级计数
                if (blueLableLevel != null)
                {
                    currentCount++;
                }
                if (BatchInt > BatchZInt)
                {
                    if (blueInt != blueZInt)
                    {
                        MessageBox.Show("批次包装完成,请输入新的批次标签!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Error = "";
                        Tips = "该批次已经装箱完成,请输入新的批次标签!";
                        BoolBlue = 2;
                        Barcode = "";
                        BatchZInt = 0;
                        BatchInt = 0;
                        BatchLable = "";
                        blueInt -= 1;

                        return;
                    }
                }
                var wipResource = RT.Service.Resolve<WipResourceController>().GetWipResource(KZWorkstation.ResourceId.Value);
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
                decimal NeedQty = 0;
                foreach (var dispatchTask in dispatchTasks)
                {
                    var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(dispatchTask);

                    var packingTasks = RT.Service.Resolve<WipBatchController>().GetWipBatchesByPackingTaskIds(new List<double>() { dispatchTask.Id });
                    //剩余最大可包装数
                    var RemainQty = tuple.Item2;
                    var maxReportQty = tuple.Item1;

                    //需要装箱数
                    NeedQty += maxReportQty;

                    //if (packingTasks.Count > 0)
                    //{
                    //    RemainQty = (int)(maxReportQty - packingTasks.Sum(p => p.Qty) - dispatchTask.SuspectQty);//(int)(RemainQty - (dispatchTask.ReportQty - packingTasks.Sum(p => p.Qty)));
                    //}
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

                packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
                var details = RT.Service.Resolve<PackingQcController>().GetPackingDetails(KZWorkstation.ResourceId.Value, WorkOrder.No);
                if (details.Sum(p => p.PackingNum) + 1 > taskRemainQty)
                {
                    Error = "数量超过了剩余可报工数".L10N();
                    BoolBlue = 3;
                    Barcode = "";
                    return;
                }
                #region 存入数据库


                ConnectorSnPackingModel pqc = new ConnectorSnPackingModel();
                pqc.BlueLabel = XtBlue;
                pqc.Confirm = blueLableLevel != null ? ConfirmEnum.NO : ConfirmEnum.YES;
                pqc.PackIdent = PackIdentEnum.NotFullTank;
                pqc.ProductLabel = Barcode;

                pqc.ItemId = itemData.Id;
                pqc.ItemName = itemData.Name;
                pqc.ResourceId = (double)KZWorkstation.ResourceId;
                //pqc.BlueLableNum = 1;
                pqc.PackingNum = 1;
                pqc.BatchLabel = BatchLable;
                PackageSnRecordList.Add(pqc);

                if (packingQc == null)
                {
                    packingQc = new PackingQc();
                    packingQc.BlueLabel = XtBlue;
                    packingQc.Confirm = blueLableLevel != null ? ConfirmEnum.NO : ConfirmEnum.YES;
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

                    packingQc.Confirm = ConfirmEnum.YES;
                    packingQc.PackingNum = blueInt;
                    blueBable.IsPack = true;
                }
                //蓝标层级QC确认
                if (blueLableLevel != null && levelTargets != null && levelTargets.Length > 0)
                {
                    if (levelTargets.Length > currentLevelIndex && currentCount >= levelTargets[currentLevelIndex])
                    {
                        currentLevelIndex++;
                        currentCount = 0;
                        packingQc.Confirm = ConfirmEnum.NO;
                        MessageBox.Show("蓝标的装箱数量：" + blueInt + ",已到达第【" + currentLevelIndex + "】层,请QC确认", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (blueInt >= blueZInt)
                    {
                        packingQc.Confirm = ConfirmEnum.NO;
                        MessageBox.Show("蓝标的装箱数量：" + blueInt + ",已经是尾箱,请QC确认", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                var isReport = false;
                if (blueInt == blueZInt)
                {
                    if (blueLableLevel != null)
                    {
                        // 有蓝标层级，需要QC确认，不自动报工
                        isReport = false;
                    }
                    else
                    {
                        isReport = true;
                    }
                }

                //存入包装QC确认表并报工
                string submitMessage = RT.Service.Resolve<PackingQcController>().SavePackingQcAndReport(packingQc, blueBable, WorkOrder.No, Barcode, BatchLable, isReport);
                //报工
                if (isReport)
                {
                    PackageSnRecordList.Clear();
                    BoolBlue = 1;
                    XtBlue = "";
                    blueInt = 0;
                    XtBlue = "";
                    blueZInt = 0;
                    blueBable = null;
                    Tips = "请输入蓝标标签!";
                    Barcode = "";
                    SnIdent = 0;
                    BatchLable = "";
                    currentLevelIndex = 0;
                    currentCount = 0;
                    blueLableLevel = null;
                    levelTargets = null;
                    if (!submitMessage.IsNullOrWhiteSpace())
                    {
                        Tips = "";
                        Reset();
                        Error = submitMessage;
                        MessageBox.Show("包装失败!{0}".L10nFormat(submitMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        PackageSnRecordList.Clear();
                        MessageBox.Show("包装成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }
                #endregion

                Tips = "请输入Sn标签!";
                Barcode = "";
            }
            base.OnBarcodeChanged(e);
        }

        public void AllReset()
        {
            WorkOrderId = null;
            PackageSnRecordList.Clear();
            blueInt = 0;
            blueZInt = 0;
            XtBlue = "";
            BoolBlue = 1;
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
            BatchZInt = 0;
            BatchInt = 0;
            BatchLable = "";
            DeleteState = "扫码中";
            DeleteIdent = 0;
            //Reset(resetType: ResetType.CollectRestart);
            //FocuseBarcode();
        }

        /// <summary>
        /// 弹出提示
        /// </summary>
        public void TcTs(string name)
        {
            VerifyCode = null;
            var template = new DetailsUITemplate(typeof(ConnectorSnPackingViewModel), ConnectorSnPakcingModelViewConfig.VerifyCodeView);
            var ui = template.CreateUI();
            ui.MainView.Data = this;
            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
            {
                w.Title = name.L10N();
                w.Commands.Remove("取消");
                w.Height = 200;
                w.Width = 400;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (RT.Service.Resolve<PackingQcController>().VerifyCode(VerifyCode))
                        {
                            Tips = "请输入SN标签!";
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
            BatchInt = 0;
            BatchZInt = 0;
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
        public virtual bool SaveWipBatch(double workOrderId, int qty, string wipName)
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
            return RT.Service.Resolve<WipBatchController>().SaveWipBatch(wipBatch);
        }

        /// <summary>
        /// 移除批次标签
        /// </summary>
        /// <param name="barcode"></param>
        public string DeleteLabel(string barcode)
        {
            var engrave = RT.Service.Resolve<EngraveLabelController>().BoolEngraveLabel(BatchLable);
            if (engrave == null)
            {
                return "批次标签不存在!";
            }
            var enSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSn(barcode);
            if (enSn == null)
            {
                return "刻码标签不存在";
            }
            //主表
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);
            //从表
            var packingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
            var packingBatch = packingDetailList.Where(p => p.BatchLabel == BatchLable).ToList();
            if (packingDetailList.Count > 0)
            {
                if (packingBatch.Count > 0)
                {
                    var packDetail = packingDetailList.Where(p => p.ProductLabel == barcode && p.BatchLabel == BatchLable).FirstOrDefault();
                    if (packDetail != null)
                    {
                        if (packDetail.ReportsType == ReportsTypeEnum.NO)
                        {
                            packDetail.PersistenceStatus = PersistenceStatus.Deleted;
                            packingQc.PackingNum -= packDetail.PackingNum;
                            blueInt -= packDetail.PackingNum;
                            //RF.Save(packDetail);
                            enSn.PersistenceStatus = PersistenceStatus.Deleted;
                            //RF.Save(wipPressureSn);

                            if (packingDetailList.Count == 1)
                            {
                                packingQc.PersistenceStatus = PersistenceStatus.Deleted;
                                RT.Service.Resolve<PackingQcController>().DeleteConnectorSnSave(packDetail, packingQc, engrave, enSn);
                                Reset();
                                AllReset();
                            }
                            else
                            {
                                if (packingBatch.Count == 1)
                                {
                                    engrave.PersistenceStatus = PersistenceStatus.Deleted;
                                }
                                RT.Service.Resolve<PackingQcController>().DeleteConnectorSnSave(packDetail, packingQc, engrave, enSn);

                                PackageSnRecordList.Clear();
                                var NewPackingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                                foreach (var item in NewPackingDetailList)
                                {
                                    ConnectorSnPackingModel pqc = new ConnectorSnPackingModel();
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
                        return "该刻码标签不存在,刻码标签【" + barcode + "】!";
                    }
                }
                else
                {
                    return "该批次标签下,已经没有刻码标签!";
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
