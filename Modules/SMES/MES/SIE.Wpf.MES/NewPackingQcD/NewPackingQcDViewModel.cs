using SIE.Andon.Andons.Configs;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Configs;
using SIE.Core.Labels;
using SIE.Domain;
using SIE.EventMessages.MES.Dispatchs;
using SIE.ManagedProperty;
using SIE.MES.BlueLable;
using SIE.MES.PackingQC;
using SIE.MES.PackingQC.Configs;
using SIE.MES.PackRule;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PackingQcs;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Wpf.MES.NewPackingQC;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIE.Wpf.MES.NewPackingQcD
{
    /// <summary>
    /// D包装采集
    /// </summary>
    [RootEntity]
    [EntityWithConfig(typeof(PackingQcDVerifyCodeConfig))]
    [Label("D包装采集")]
    public class NewPackingQcDViewModel : KZDataCollectionViewModel
    {
        private readonly object _scanLock = new object();

        public NewPackingQcDViewModel()
        {
            InitWorkstation();
        }
        #region 包装明细
        /// <summary>
        /// 包装明细
        /// </summary>
        public static readonly ListProperty<EntityList<NewPackingQcDModel>> PackageSnRecordListProperty = P<NewPackingQcDViewModel>.RegisterList(e => e.PackageSnRecordList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as NewPackingQcDViewModel).LoadPackageSnRecordList()
        });

        /// <summary>
        /// 包装明细
        /// </summary>
        public EntityList<NewPackingQcDModel> PackageSnRecordList
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
        public static readonly Property<int> blueIntProperty = P<NewPackingQcDViewModel>.Register(e => e.blueInt);

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
        public static readonly Property<int> blueZIntProperty = P<NewPackingQcDViewModel>.Register(e => e.blueZInt);

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
        public static readonly Property<string> XtBlueProperty = P<NewPackingQcDViewModel>.Register(e => e.XtBlue);

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
        public static readonly Property<string> YXtBlueProperty = P<NewPackingQcDViewModel>.Register(e => e.YXtBlue);

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
            if (Barcode.IsNullOrEmpty()) return;

            // 添加锁确保同一时间只有一个扫描在处理
            lock (_scanLock)
            {
                ProcessBarcode();
            }

            base.OnBarcodeChanged(e);
        }

        private void ProcessBarcode()
        {
            Error = "";
            Tips = "";
            PackresourceId = (double)KZWorkstation.ResourceId;

            if (BoolBlue)
            {
                ProcessBlueLabel();
            }
            else
            {
                ProcessProductLabel();
            }
        }

        private void ProcessBlueLabel()
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
            // PackresourceId = 0;
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
            }
            else
            {
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
                        XtBlue = Barcode;
                        //原蓝标查询所有蓝标
                        var yXtBlue = RT.Service.Resolve<PackingQcController>().AllBlueLable(YXtBlue);
                        //原蓝标查询装箱数量
                        var yXpackingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(YXtBlue);
                        //现有蓝标
                        var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(yXpackingQc.Id);
                        var detailSum = packdetails.Count();
                        var xtBlue = RT.Service.Resolve<PackingQcController>().GetBlueLable(XtBlue);
                        YXtBlue = XtBlue;
                        if (xtBlue == null)
                        {
                            Error = "新蓝标在系统中不存在,新蓝标【" + Barcode + "】!!!";
                            Barcode = "";
                            XtBlue = "";
                            YXtBlue = "";
                            return;
                        }
                        //if (xtBlue.PackageNum <= yXtBlue.PackageNum && xtBlue.PackageNum >= detailSum)
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

            if (BoxExChange == 0)
            {
                blueBable = RT.Service.Resolve<PackingQcController>().GetBlueLable(Barcode);
            }
            else
            {
                blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(Barcode);
            }

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



            if (packingQc == null)
            {


                //获取蓝标QC确认
                var packingQcData = RT.Service.Resolve<PackingQcController>().GetPackingQcDByState((double)KZWorkstation.ResourceId);
                foreach (var item in packingQcData)
                {
                    if (item.BlueLabel != Barcode)
                    {
                        if (item.Confirm == ConfirmEnum.NO)
                        {
                            Error = "蓝标标签[" + item.BlueLabel + "]没有封箱,不允许使用其它蓝标标签!";
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
                    NewPackingQcDModel pqc = new NewPackingQcDModel();
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
                    // 正常状态
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

        private void ProcessProductLabel()
        {
            //var config = ConfigService.GetConfig(new PackingQcDVerifyCodeConfig(), typeof(NewPackingQcDViewModel));
            //if (config.CodeLength != Barcode.Length)
            //{
            //    Barcode = "";
            //    Error = "请输入正确的条码!";
            //    return;
            //}

            // 提前检查数量限制
            if (blueInt >= blueZInt)
            {
                Barcode = "";
                Error = $"该蓝标箱已装满，最多只能装 {blueZInt} 个!";
                MessageBox.Show($"蓝标已经装箱完成,蓝标数量：{blueInt}", "提示",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ResetAfterFull();
                return;
            }

            PackingQc packingQc = new PackingQc();
            string pcSn = "";

            // 获取蓝标QC确认
            packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(XtBlue);

            if (RT.Service.Resolve<PackingQcController>().GetPackingDetail(Barcode))
            {
                Barcode = "";
                Error = "条码已经在系统中存在!";
                return;
            }

            // 再次检查数量（双重检查）
            if (blueInt >= blueZInt)
            {
                Barcode = "";
                Error = $"该蓝标箱已装满!";
                return;
            }

            // 获取工单
            var workOrderData = RT.Service.Resolve<WipBatchController>().GetWorkOrder(WorkOrderId);
            // 获取产品
            var itemData = RT.Service.Resolve<WipBatchController>().GetItem(workOrderData.ProductId);

            #region 包装规则验证
            var itemQrRule = RT.Service.Resolve<PackRuleController>().GetItemQRCodeRule(itemData.Id);
            if (itemQrRule == null)
            {
                Error = "当前产品,没有维护包装物料二维码规则关系!";
                BoolBlue = false;
                Barcode = "";
                return;
            }

            var qRCodeRule = RT.Service.Resolve<PackRuleController>().GetQRCodeRule(itemQrRule.QRCodeRuleId);
            if (qRCodeRule == null)
            {
                Error = "包装二维码规则维护没有!";
                BoolBlue = false;
                Barcode = "";
                return;
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
                    BoolBlue = false;
                    Barcode = "";
                    return;
                }
            }
            //客户版本号验证 开始，结束
            int khbbStart = -1;
            int khbbEnd = -1;
            if (qRCodeRule.VersionNumberStartDigit != null)
                if (qRCodeRule.VersionNumberStartDigit != "")
                {
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
                    BoolBlue = false;
                    Barcode = "";
                    return;
                }
            }
            //总位数验证
            if (qRCodeRule.TotalDigit != null || qRCodeRule.TotalDigit != "")
            {
                int total = int.Parse(qRCodeRule.TotalDigit);
                if (Barcode.Length != total)
                {
                    Error = "当前二维码的长度跟规则不匹配,当前总位数是：【" + total + "】,规则总位数【" + Barcode.Length + "】!";
                    BoolBlue = false;
                    Barcode = "";
                    return;
                }
            }

            //序号验证 开始，结束
            int xhStart = -1;
            int xhEnd = -1;
            if (qRCodeRule.SerialNumberStartDigit != null || qRCodeRule.SerialNumberEndDigit != "")
            {
                xhStart = int.Parse(qRCodeRule.SerialNumberStartDigit);
            }
            if (qRCodeRule.SerialNumberEndDigit != null || qRCodeRule.SerialNumberEndDigit != "")
            {
                xhEnd = int.Parse(qRCodeRule.SerialNumberEndDigit);
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
                            BoolBlue = false;
                            Barcode = "";
                            return;
                        }
                    }
                }
            }


            #endregion

            #region 存入数据库

            // 界面显示明细
            PackingDetail packingDetail = new PackingDetail();
            NewPackingQcDModel pqc = new NewPackingQcDModel();
            pqc.BlueLabel = XtBlue;
            pqc.Confirm = ConfirmEnum.NO;
            pqc.PackIdent = PackIdentEnum.NotFullTank;
            pqc.ProductLabel = Barcode;
            pqc.PackingNum = 1;
            pqc.ItemId = itemData.Id;
            pqc.ItemName = itemData.Name;
            pqc.ResourceId = (double)KZWorkstation.ResourceId;

            // 更新数量
            blueInt += 1;
            currentCount++;
            PackageSnRecordList.Add(pqc);

            bool isFullTank = (blueInt >= blueZInt);

            if (packingQc == null)
            {
                packingQc = new PackingQc();
                packingQc.BlueLabel = XtBlue;
                packingQc.Confirm = isFullTank ? ConfirmEnum.YES : ConfirmEnum.NO;
                packingQc.PackIdent = isFullTank ? PackIdentEnum.FullTank : PackIdentEnum.NotFullTank;
                packingQc.ProductLabel = Barcode;
                packingQc.ItemId = itemData.Id;
                packingQc.ItemName = itemData.Name;
                packingQc.BlueLableNum = blueZInt;
                packingQc.PackingNum = blueInt;
                packingQc.ResourceId = (double)KZWorkstation.ResourceId;
                packingQc.BoxState = isFullTank ? BoxStateEnum.NO : BoxStateEnum.YES;
                packingQc.ReportsType = ReportsTypeEnum.YES;
                blueBable.IsPack = true;
            }
            else
            {
                packingQc.PackIdent = isFullTank ? PackIdentEnum.FullTank : PackIdentEnum.NotFullTank;
                packingQc.Confirm = isFullTank ? ConfirmEnum.YES : ConfirmEnum.NO;
                packingQc.PackingNum = blueInt;
                packingQc.BoxState = isFullTank ? BoxStateEnum.NO : BoxStateEnum.YES;
                blueBable.IsPack = true;
            }

            // 存入包装QC确认表
            RF.Save(packingQc);
            RF.Save(blueBable);

            // 存入包装QC确认明细表
            packingDetail.ProductLabel = Barcode;
            packingDetail.PackingQcId = packingQc.Id;
            packingDetail.Confirm = isFullTank ? ConfirmEnum.YES : ConfirmEnum.NO;
            packingDetail.BatchLabel = pcSn;
            packingDetail.WorkOrderNo = WorkOrder.No;
            packingDetail.LabelType = LabelTypeEnum.SnLabel;
            packingDetail.ReportsType = ReportsTypeEnum.NO;
            packingDetail.PackingNum = 1;

            RF.Save(packingDetail);

            #endregion

            // 检查是否已满
            if (isFullTank)
            {
                MessageBox.Show($"蓝标已经装箱完成,蓝标数量：{blueInt}，已自动关箱", "提示",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ResetAfterFull();
                return;
            }

            Tips = "请输入工序标签!";
            Barcode = "";
        }

        private void ResetAfterFull()
        {
            // 先保存 XtBlue 到临时变量
            string currentBlueLabel = XtBlue;

            // 确保数据库状态为封箱
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(currentBlueLabel);
            if (packingQc != null)
            {
                packingQc.BoxState = BoxStateEnum.NO;
                packingQc.PackIdent = PackIdentEnum.FullTank;
                packingQc.Confirm = ConfirmEnum.YES;
                RF.Save(packingQc);

                // 更新所有相关明细的确认状态
                var packDetails = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
                foreach (var detail in packDetails)
                {
                    detail.Confirm = ConfirmEnum.YES;
                    RF.Save(detail);
                }

                // 记录日志
                RT.Logger.Info($"蓝标 {currentBlueLabel} 已自动关箱，装箱数量：{blueInt}");
            }

            // 然后清空界面状态
            PackageSnRecordList.Clear();
            BoolBlue = true;
            XtBlue = "";
            blueInt = 0;
            blueZInt = 0;
            blueBable = null;
            Tips = "请输入蓝标标签!";
            Barcode = "";
            SnIdent = 0;
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
    }
}