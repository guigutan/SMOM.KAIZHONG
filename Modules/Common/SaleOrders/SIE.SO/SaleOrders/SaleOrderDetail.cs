using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("订单明细")]
    [DisplayMember(nameof(LineNo))]
    public class SaleOrderDetail : DataEntity
    {
        /// <summary>
        /// 行业类型快码
        /// </summary>
        public const string INDUSTRYTYPE = "INDUSTRY_TYPE";
        /// <summary>
        ///订单类型快码
        /// </summary>
        public const string ORDERTYPE = "ORDER_TYPE";
        /// <summary>
        /// 产品类型快码
        /// </summary>
        public const string PRODUCTTYPE = "PRODUCT_TYPE";
        /// <summary>
        /// 产品等级快码
        /// </summary>
        public const string PRODUCTLEVEL = "PRODUCT_LEVEL";

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Required]
        [MaxLength(80)]
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<SaleOrderDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 版本号 Version
        /// <summary>
        /// 版本号
        /// </summary>
        [Label("版本号")]
        public static readonly Property<string> VersionProperty = P<SaleOrderDetail>.Register(e => e.Version);

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get { return GetProperty(VersionProperty); }
            set { SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 行业类型 IndustryType
        /// <summary>
        /// 行业类型
        /// </summary>
        [Required]
        [Label("行业类型")]
        public static readonly Property<string> IndustryTypeProperty = P<SaleOrderDetail>.Register(e => e.IndustryType);

        /// <summary>
        /// 行业类型
        /// </summary>
        public string IndustryType
        {
            get { return GetProperty(IndustryTypeProperty); }
            set { SetProperty(IndustryTypeProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Required]
        [Label("订单类型")]
        public static readonly Property<string> OrderTypeProperty = P<SaleOrderDetail>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 产品类型 ProductType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Required]
        [Label("产品类型")]
        public static readonly Property<string> ProductTypeProperty = P<SaleOrderDetail>.Register(e => e.ProductType);

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType
        {
            get { return GetProperty(ProductTypeProperty); }
            set { SetProperty(ProductTypeProperty, value); }
        }
        #endregion

        #region 产品等级 ProductLevel
        /// <summary>
        /// 产品等级
        /// </summary>
        //[Required]
        [Label("产品等级")]
        public static readonly Property<string> ProductLevelProperty = P<SaleOrderDetail>.Register(e => e.ProductLevel);

        /// <summary>
        /// 产品等级
        /// </summary>
        public string ProductLevel
        {
            get { return GetProperty(ProductLevelProperty); }
            set { SetProperty(ProductLevelProperty, value); }
        }
        #endregion

        #region 是否新单 IsNew
        /// <summary>
        /// 是否新单
        /// </summary>
        [Label("是否新单")]
        public static readonly Property<bool> IsNewProperty = P<SaleOrderDetail>.Register(e => e.IsNew);

        /// <summary>
        /// 是否新单
        /// </summary>
        public bool IsNew
        {
            get { return GetProperty(IsNewProperty); }
            set { SetProperty(IsNewProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<SaleOrderDetail>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 目标订单ID TargetOrderId
        /// <summary>
        /// 目标订单ID
        /// </summary>
        [Label("目标订单ID")]
        public static readonly Property<double> TargetOrderIdProperty = P<SaleOrderDetail>.Register(e => e.TargetOrderId);

        /// <summary>
        /// 目标订单ID
        /// </summary>
        public double TargetOrderId
        {
            get { return GetProperty(TargetOrderIdProperty); }
            set { SetProperty(TargetOrderIdProperty, value); }
        }
        #endregion

        #region 生产订单编号 TargetOrderCode
        /// <summary>
        /// 生产订单编号
        /// </summary>
        [Label("生产订单编号")]
        public static readonly Property<string> TargetOrderCodeProperty = P<SaleOrderDetail>.Register(e => e.TargetOrderCode);

        /// <summary>
        /// 生产订单编号
        /// </summary>
        public string TargetOrderCode
        {
            get { return GetProperty(TargetOrderCodeProperty); }
            set { SetProperty(TargetOrderCodeProperty, value); }
        }
        #endregion

        #region MI完成时间 MiDateTime
        /// <summary>
        /// MI完成时间
        /// </summary>
        [Label("MI完成时间")]
        public static readonly Property<DateTime> MiDateTimeProperty = P<SaleOrderDetail>.Register(e => e.MiDateTime);

        /// <summary>
        /// MI完成时间
        /// </summary>
        public DateTime MiDateTime
        {
            get { return GetProperty(MiDateTimeProperty); }
            set { SetProperty(MiDateTimeProperty, value); }
        }
        #endregion

        #region 总面积M2 Area
        /// <summary>
        /// 面积M2
        /// </summary>
        [Label("总面积M2")]
        public static readonly Property<decimal> AreaProperty = P<SaleOrderDetail>.Register(e => e.Area);

        /// <summary>
        /// 总面积M2
        /// </summary>
        public decimal Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 单个面积 SingleArea
        /// <summary>
        /// 单个面积
        /// </summary>
        [Label("单个面积")]
        public static readonly Property<decimal> SingleAreaProperty = P<SaleOrderDetail>.Register(e => e.SingleArea);

        /// <summary>
        /// 单个面积
        /// </summary>
        public decimal SingleArea
        {
            get { return GetProperty(SingleAreaProperty); }
            set { SetProperty(SingleAreaProperty, value); }
        }
        #endregion

        #region 大板尺寸 PlateSize
        /// <summary>
        /// 大板尺寸
        /// </summary>
        [Label("大板尺寸")]
        public static readonly Property<string> PlateSizeProperty = P<SaleOrderDetail>.Register(e => e.PlateSize);

        /// <summary>
        /// 大板尺寸
        /// </summary>
        public string PlateSize
        {
            get { return GetProperty(PlateSizeProperty); }
            set { SetProperty(PlateSizeProperty, value); }
        }
        #endregion

        #region 开料PNL数 MaterialPnl
        /// <summary>
        /// 开料PNL数
        /// </summary>
        [Label("开料PNL数")]
        public static readonly Property<decimal> MaterialPnlProperty = P<SaleOrderDetail>.Register(e => e.MaterialPnl);

        /// <summary>
        /// 开料PNL数
        /// </summary>
        public decimal MaterialPnl
        {
            get { return GetProperty(MaterialPnlProperty); }
            set { SetProperty(MaterialPnlProperty, value); }
        }
        #endregion

        #region SET/PNL数 SetPnl
        /// <summary>
        /// SET/PNL数
        /// </summary>
        [Label("SET/PNL数")]
        public static readonly Property<decimal> SetPnlProperty = P<SaleOrderDetail>.Register(e => e.SetPnl);

        /// <summary>
        /// SET/PNL数
        /// </summary>
        public decimal SetPnl
        {
            get { return GetProperty(SetPnlProperty); }
            set { SetProperty(SetPnlProperty, value); }
        }
        #endregion

        #region PCS/PNL数 PcsPnl
        /// <summary>
        /// PCS/PNL数
        /// </summary>
        [Label("PCS/PNL数")]
        public static readonly Property<decimal> PcsPnlProperty = P<SaleOrderDetail>.Register(e => e.PcsPnl);

        /// <summary>
        /// PCS/PNL数
        /// </summary>
        public decimal PcsPnl
        {
            get { return GetProperty(PcsPnlProperty); }
            set { SetProperty(PcsPnlProperty, value); }
        }
        #endregion

        #region 客户交期 RequireDelivery
        /// <summary>
        /// 客户交期
        /// </summary>
        [Label("客户交期")]
        public static readonly Property<DateTime> RequireDeliveryProperty = P<SaleOrderDetail>.Register(e => e.RequireDelivery);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery
        {
            get { return GetProperty(RequireDeliveryProperty); }
            set { SetProperty(RequireDeliveryProperty, value); }
        }
        #endregion

        #region 承诺交期 PromiseDelivery
        /// <summary>
        /// 承诺交期
        /// </summary>
        [Label("承诺交期")]
        public static readonly Property<DateTime?> PromiseDeliveryProperty = P<SaleOrderDetail>.Register(e => e.PromiseDelivery);

        /// <summary>
        /// 承诺交期
        /// </summary>
        public DateTime? PromiseDelivery
        {
            get { return GetProperty(PromiseDeliveryProperty); }
            set { SetProperty(PromiseDeliveryProperty, value); }
        }
        #endregion

        #region 订单行挂起 IsHangUp
        /// <summary>
        /// 订单行挂起
        /// </summary>
        [Label("订单行挂起")]
        public static readonly Property<bool> IsHangUpProperty = P<SaleOrderDetail>.Register(e => e.IsHangUp);

        /// <summary>
        /// 订单行挂起
        /// </summary>
        public bool IsHangUp
        {
            get { return GetProperty(IsHangUpProperty); }
            set { SetProperty(IsHangUpProperty, value); }
        }
        #endregion

        #region 企业 Enterprise
        /// <summary>
        /// 企业Id
        /// </summary>
        public static readonly IRefIdProperty EnterpriseIdProperty = P<SaleOrderDetail>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 企业Id
        /// </summary>
        public double? EnterpriseId
        {
            get { return (double?)GetRefNullableId(EnterpriseIdProperty); }
            set { SetRefNullableId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 企业
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<SaleOrderDetail>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 企业
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 库存组织编码 EnterpriseCode
        /// <summary>
        /// 库存组织编码
        /// </summary>
        [MaxLength(240)]
        [Label("库存组织代码")]
        public static readonly Property<string> EnterpriseCodeProperty = P<SaleOrderDetail>.RegisterView(e => e.EnterpriseCode, p => p.Enterprise.Code);

        /// <summary>
        /// 库存组织代码
        /// </summary>
        public string EnterpriseCode
        {
            get { return this.GetProperty(EnterpriseCodeProperty); }
            set { SetProperty(EnterpriseCodeProperty, value); }
        }
        #endregion

        #region 库存组织名称 EnterpriseName
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [MaxLength(240)]
        [Label("库存组织名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<SaleOrderDetail>.RegisterView(e => e.EnterpriseName, p => p.Enterprise.Name);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
            set { SetProperty(EnterpriseNameProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty = P<SaleOrderDetail>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId
        {
            get { return (double)GetRefId(UnitIdProperty); }
            set { SetRefId(UnitIdProperty, value); }
        }
        /// <summary>
        /// 单位Id
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<SaleOrderDetail>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public Unit Unit
        {
            get { return GetRefEntity(UnitProperty); }
            set { SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 行状态 LineState
        /// <summary>
        /// 行状态
        /// </summary>
        [Label("行状态")]
        public static readonly Property<LineState> LineStateProperty = P<SaleOrderDetail>.Register(e => e.LineState);

        /// <summary>
        /// 行状态
        /// </summary>
        public LineState LineState
        {
            get { return GetProperty(LineStateProperty); }
            set { SetProperty(LineStateProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<SaleOrderDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<SaleOrderDetail>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SaleOrderDetail>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [MaxLength(240)]
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<SaleOrderDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 销售订单 SaleOrder
        /// <summary>
        /// 销售订单Id
        /// </summary>
        public static readonly IRefIdProperty SaleOrderIdProperty = P<SaleOrderDetail>.RegisterRefId(e => e.SaleOrderId, ReferenceType.Parent);

        /// <summary>
        /// 销售订单Id
        /// </summary>
        public double SaleOrderId
        {
            get { return (double)GetRefId(SaleOrderIdProperty); }
            set { SetRefId(SaleOrderIdProperty, value); }
        }

        /// <summary>
        /// 销售订单
        /// </summary>
        public static readonly RefEntityProperty<SaleOrder> SaleOrderProperty = P<SaleOrderDetail>.RegisterRef(e => e.SaleOrder, SaleOrderIdProperty);

        /// <summary>
        /// 销售订单
        /// </summary>
        public SaleOrder SaleOrder
        {
            get { return GetRefEntity(SaleOrderProperty); }
            set { SetRefEntity(SaleOrderProperty, value); }
        }
        #endregion

        #region 生成工程计划状态 EngineeringPlanState
        /// <summary>
        /// 生成工程计划状态
        /// </summary>
        [Label("生成工程计划状态")]
        public static readonly Property<bool> EngineeringPlanStateProperty = P<SaleOrderDetail>.Register(e => e.EngineeringPlanState);

        /// <summary>
        /// 生成工程计划状态
        /// </summary>
        public bool EngineeringPlanState
        {
            get { return GetProperty(EngineeringPlanStateProperty); }
            set { SetProperty(EngineeringPlanStateProperty, value); }
        }
        #endregion

        #region 生成工程计划原因 EngineeringPlanRemark
        /// <summary>
        /// 生成工程计划原因
        /// </summary>
        [Label("生成工程计划原因")]
        public static readonly Property<string> EngineeringPlanRemarkProperty = P<SaleOrderDetail>.Register(e => e.EngineeringPlanRemark);

        /// <summary>
        /// 生成工程计划原因
        /// </summary>
        public string EngineeringPlanRemark
        {
            get { return GetProperty(EngineeringPlanRemarkProperty); }
            set { SetProperty(EngineeringPlanRemarkProperty, value); }
        }
        #endregion

        #region 特殊工艺列表 SpecialProcessList
        /// <summary>
        /// 特殊工艺列表
        /// </summary>
        public static readonly ListProperty<EntityList<SpecialProcess>> SpecialProcessListProperty = P<SaleOrderDetail>.RegisterList(e => e.SpecialProcessList);
        /// <summary>
        /// 特殊工艺列表
        /// </summary>
        public EntityList<SpecialProcess> SpecialProcessList
        {
            get { return this.GetLazyList(SpecialProcessListProperty); }
        }
        #endregion

        #region 特殊工艺 SpecialProcessStr
        /// <summary>
        /// 特殊工艺
        /// </summary>
        [Label("特殊工艺")]
        public static readonly Property<string> SpecialProcessStrProperty = P<SaleOrderDetail>.Register(e => e.SpecialProcessStr);

        /// <summary>
        /// 特殊工艺
        /// </summary>
        public string SpecialProcessStr
        {
            get { return GetProperty(SpecialProcessStrProperty); }
            set { SetProperty(SpecialProcessStrProperty, value); }
        }
        #endregion

        #region SGC

        #region 是否NPI IsNpi
        /// <summary>
        /// 是否NPI
        /// </summary>
        [Label("是否NPI")]
        public static readonly Property<bool> IsNpiProperty = P<SaleOrderDetail>.Register(e => e.IsNpi);

        /// <summary>
        /// 是否NPI
        /// </summary>
        public bool IsNpi
        {
            get { return this.GetProperty(IsNpiProperty); }
            set { this.SetProperty(IsNpiProperty, value); }
        }
        #endregion

        #region 是否认证样板 IsSamplePlate
        /// <summary>
        /// 是否认证样板
        /// </summary>
        [Label("是否认证样板")]
        public static readonly Property<bool> IsSamplePlateProperty = P<SaleOrderDetail>.Register(e => e.IsSamplePlate);

        /// <summary>
        /// 是否认证样板
        /// </summary>
        public bool IsSamplePlate
        {
            get { return this.GetProperty(IsSamplePlateProperty); }
            set { this.SetProperty(IsSamplePlateProperty, value); }
        }
        #endregion

        #region 是否研发样板 IsRdPlate
        /// <summary>
        /// 是否研发样板
        /// </summary>
        [Label("是否研发样板")]
        public static readonly Property<bool> IsRdPlateProperty = P<SaleOrderDetail>.Register(e => e.IsRdPlate);

        /// <summary>
        /// 是否研发样板
        /// </summary>
        public bool IsRdPlate
        {
            get { return this.GetProperty(IsRdPlateProperty); }
            set { this.SetProperty(IsRdPlateProperty, value); }
        }
        #endregion

        #region 快板类型 AllegroType
        /// <summary>
        /// 快板类型
        /// </summary>
        [Label("快板类型")]
        public static readonly Property<string> AllegroTypeProperty = P<SaleOrderDetail>.Register(e => e.AllegroType);

        /// <summary>
        /// 快板类型
        /// </summary>
        public string AllegroType
        {
            get { return this.GetProperty(AllegroTypeProperty); }
            set { this.SetProperty(AllegroTypeProperty, value); }
        }
        #endregion

        #region VM结算上限天数 VmSettlementLimitDays
        /// <summary>
        /// VM结算上限天数
        /// </summary>
        [Label("VM结算上限天数")]
        public static readonly Property<decimal> VmSettlementLimitDaysProperty = P<SaleOrderDetail>.Register(e => e.VmSettlementLimitDays);

        /// <summary>
        /// VM结算上限天数
        /// </summary>
        public decimal VmSettlementLimitDays
        {
            get { return this.GetProperty(VmSettlementLimitDaysProperty); }
            set { this.SetProperty(VmSettlementLimitDaysProperty, value); }
        }
        #endregion

        #region 要求的交货日期 RequirementDeliveryDate
        /// <summary>
        /// 要求的交货日期
        /// </summary>
        [Label("要求的交货日期")]
        public static readonly Property<DateTime> RequirementDeliveryDateProperty = P<SaleOrderDetail>.Register(e => e.RequirementDeliveryDate);

        /// <summary>
        /// 要求的交货日期
        /// </summary>
        public DateTime RequirementDeliveryDate
        {
            get { return this.GetProperty(RequirementDeliveryDateProperty); }
            set { this.SetProperty(RequirementDeliveryDateProperty, value); }
        }
        #endregion

        #region PO日期类型 PoDateType
        /// <summary>
        /// PO日期类型
        /// </summary>
        [Label("PO日期类型")]
        public static readonly Property<DateTime> PoDateTypeProperty = P<SaleOrderDetail>.Register(e => e.PoDateType);

        /// <summary>
        /// PO日期类型
        /// </summary>
        public DateTime PoDateType
        {
            get { return this.GetProperty(PoDateTypeProperty); }
            set { this.SetProperty(PoDateTypeProperty, value); }
        }
        #endregion

        #region ETD Etd
        /// <summary>
        /// ETD
        /// </summary>
        [Label("ETD")]
        public static readonly Property<string> EtdProperty = P<SaleOrderDetail>.Register(e => e.Etd);

        /// <summary>
        /// ETD
        /// </summary>
        public string Etd
        {
            get { return this.GetProperty(EtdProperty); }
            set { this.SetProperty(EtdProperty, value); }
        }
        #endregion

        #region 销售员 SalesPerson
        /// <summary>
        /// 销售员
        /// </summary>
        [Label("销售员")]
        public static readonly Property<string> SalesPersonProperty = P<SaleOrderDetail>.Register(e => e.SalesPerson);

        /// <summary>
        /// 销售员
        /// </summary>
        public string SalesPerson
        {
            get { return this.GetProperty(SalesPersonProperty); }
            set { this.SetProperty(SalesPersonProperty, value); }
        }
        #endregion

        #region 销售部门 SalesDepartment
        /// <summary>
        /// 销售部门
        /// </summary>
        [Label("销售部门")]
        public static readonly Property<string> SalesDepartmentProperty = P<SaleOrderDetail>.Register(e => e.SalesDepartment);

        /// <summary>
        /// 销售部门
        /// </summary>
        public string SalesDepartment
        {
            get { return this.GetProperty(SalesDepartmentProperty); }
            set { this.SetProperty(SalesDepartmentProperty, value); }
        }
        #endregion

        #region 销售组 SalesTeam
        /// <summary>
        /// 销售组
        /// </summary>
        [Label("销售组")]
        public static readonly Property<string> SalesTeamProperty = P<SaleOrderDetail>.Register(e => e.SalesTeam);

        /// <summary>
        /// 销售组
        /// </summary>
        public string SalesTeam
        {
            get { return this.GetProperty(SalesTeamProperty); }
            set { this.SetProperty(SalesTeamProperty, value); }
        }
        #endregion

        #region 二维码 QrCode
        /// <summary>
        /// 二维码
        /// </summary>
        [Label("二维码")]
        public static readonly Property<string> QrCodeProperty = P<SaleOrderDetail>.Register(e => e.QrCode);

        /// <summary>
        /// 二维码
        /// </summary>
        public string QrCode
        {
            get { return this.GetProperty(QrCodeProperty); }
            set { this.SetProperty(QrCodeProperty, value); }
        }
        #endregion

        #region 客户生产批号 CustomerProductionLot
        /// <summary>
        /// 客户生产批号
        /// </summary>
        [Label("客户生产批号")]
        public static readonly Property<string> CustomerProductionLotProperty = P<SaleOrderDetail>.Register(e => e.CustomerProductionLot);

        /// <summary>
        /// 客户生产批号
        /// </summary>
        public string CustomerProductionLot
        {
            get { return this.GetProperty(CustomerProductionLotProperty); }
            set { this.SetProperty(CustomerProductionLotProperty, value); }
        }
        #endregion

        #region P核算状态 PAccountingStatus
        /// <summary>
        /// P核算状态
        /// </summary>
        [Label("P核算状态")]
        public static readonly Property<string> PAccountingStatusProperty = P<SaleOrderDetail>.Register(e => e.PAccountingStatus);

        /// <summary>
        /// P核算状态
        /// </summary>
        public string PAccountingStatus
        {
            get { return this.GetProperty(PAccountingStatusProperty); }
            set { this.SetProperty(PAccountingStatusProperty, value); }
        }
        #endregion

        #region 扣款倍数 CutPaymentMultiple
        /// <summary>
        /// 扣款倍数
        /// </summary>
        [Label("扣款倍数")]
        public static readonly Property<decimal> CutPaymentMultipleProperty = P<SaleOrderDetail>.Register(e => e.CutPaymentMultiple);

        /// <summary>
        /// 扣款倍数
        /// </summary>
        public decimal CutPaymentMultiple
        {
            get { return this.GetProperty(CutPaymentMultipleProperty); }
            set { this.SetProperty(CutPaymentMultipleProperty, value); }
        }
        #endregion

        #region 扣款原因 CutPaymentReason
        /// <summary>
        /// 扣款原因
        /// </summary>
        [Label("扣款原因")]
        public static readonly Property<string> CutPaymentReasonProperty = P<SaleOrderDetail>.Register(e => e.CutPaymentReason);

        /// <summary>
        /// 扣款原因
        /// </summary>
        public string CutPaymentReason
        {
            get { return this.GetProperty(CutPaymentReasonProperty); }
            set { this.SetProperty(CutPaymentReasonProperty, value); }
        }
        #endregion

        #region D/C Dc
        /// <summary>
        /// D/C
        /// </summary>
        [Label("D/C")]
        public static readonly Property<string> DcProperty = P<SaleOrderDetail>.Register(e => e.Dc);

        /// <summary>
        /// D/C
        /// </summary>
        public string Dc
        {
            get { return this.GetProperty(DcProperty); }
            set { this.SetProperty(DcProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SaleOrderDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region MOQ Moq
        /// <summary>
        /// MOQ
        /// </summary>
        [Label("MOQ")]
        public static readonly Property<decimal> MoqProperty = P<SaleOrderDetail>.Register(e => e.Moq);

        /// <summary>
        /// MOQ
        /// </summary>
        public decimal Moq
        {
            get { return this.GetProperty(MoqProperty); }
            set { this.SetProperty(MoqProperty, value); }
        }
        #endregion

        #region 提前天数 AdvanceDays
        /// <summary>
        /// 提前天数
        /// </summary>
        [Label("提前天数")]
        public static readonly Property<decimal> AdvanceDaysProperty = P<SaleOrderDetail>.Register(e => e.AdvanceDays);

        /// <summary>
        /// 提前天数
        /// </summary>
        public decimal AdvanceDays
        {
            get { return this.GetProperty(AdvanceDaysProperty); }
            set { this.SetProperty(AdvanceDaysProperty, value); }
        }
        #endregion

        #region 推后天数 RetreatDays
        /// <summary>
        /// 推后天数
        /// </summary>
        [Label("推后天数")]
        public static readonly Property<decimal> RetreatDaysProperty = P<SaleOrderDetail>.Register(e => e.RetreatDays);

        /// <summary>
        /// 推后天数
        /// </summary>
        public decimal RetreatDays
        {
            get { return this.GetProperty(RetreatDaysProperty); }
            set { this.SetProperty(RetreatDaysProperty, value); }
        }
        #endregion

        #region 外发标识 OutgoIden
        /// <summary>
        /// 外发标识
        /// </summary>
        [Label("外发标识")]
        public static readonly Property<string> OutgoIdenProperty = P<SaleOrderDetail>.Register(e => e.OutgoIden);

        /// <summary>
        /// 外发标识
        /// </summary>
        public string OutgoIden
        {
            get { return this.GetProperty(OutgoIdenProperty); }
            set { this.SetProperty(OutgoIdenProperty, value); }
        }
        #endregion

        #region 外部ECN ExternalEcn
        /// <summary>
        /// 外部ECN
        /// </summary>
        [Label("外部ECN")]
        public static readonly Property<bool> ExternalEcnProperty = P<SaleOrderDetail>.Register(e => e.ExternalEcn);

        /// <summary>
        /// 外部ECN
        /// </summary>
        public bool ExternalEcn
        {
            get { return this.GetProperty(ExternalEcnProperty); }
            set { this.SetProperty(ExternalEcnProperty, value); }
        }
        #endregion

        #region 待通知开料 WaitNotifiedOpenMaterial
        /// <summary>
        /// 待通知开料
        /// </summary>
        [Label("待通知开料")]
        public static readonly Property<string> WaitNotifiedOpenMaterialProperty = P<SaleOrderDetail>.Register(e => e.WaitNotifiedOpenMaterial);

        /// <summary>
        /// 待通知开料
        /// </summary>
        public string WaitNotifiedOpenMaterial
        {
            get { return this.GetProperty(WaitNotifiedOpenMaterialProperty); }
            set { this.SetProperty(WaitNotifiedOpenMaterialProperty, value); }
        }
        #endregion

        #region 销售员2 SalePerson2
        /// <summary>
        /// 销售员2
        /// </summary>
        [Label("销售员2")]
        public static readonly Property<string> SalePerson2Property = P<SaleOrderDetail>.Register(e => e.SalePerson2);

        /// <summary>
        /// 销售员2
        /// </summary>
        public string SalePerson2
        {
            get { return this.GetProperty(SalePerson2Property); }
            set { this.SetProperty(SalePerson2Property, value); }
        }
        #endregion

        #region 外部单号 ExternalNo
        /// <summary>
        /// 外部单号
        /// </summary>
        [Label("外部单号")]
        public static readonly Property<string> ExternalNoProperty = P<SaleOrderDetail>.Register(e => e.ExternalNo);

        /// <summary>
        /// 外部单号
        /// </summary>
        public string ExternalNo
        {
            get { return this.GetProperty(ExternalNoProperty); }
            set { this.SetProperty(ExternalNoProperty, value); }
        }
        #endregion

        #region 预留标识 ReserveIden
        /// <summary>
        /// 预留标识
        /// </summary>
        [Label("预留标识")]
        public static readonly Property<string> ReserveIdenProperty = P<SaleOrderDetail>.Register(e => e.ReserveIden);

        /// <summary>
        /// 预留标识
        /// </summary>
        public string ReserveIden
        {
            get { return this.GetProperty(ReserveIdenProperty); }
            set { this.SetProperty(ReserveIdenProperty, value); }
        }
        #endregion

        #region 应用领域 AppArea
        /// <summary>
        /// 应用领域
        /// </summary>
        [Label("应用领域")]
        public static readonly Property<string> AppAreaProperty = P<SaleOrderDetail>.Register(e => e.AppArea);

        /// <summary>
        /// 应用领域
        /// </summary>
        public string AppArea
        {
            get { return this.GetProperty(AppAreaProperty); }
            set { this.SetProperty(AppAreaProperty, value); }
        }
        #endregion

        #region 备品数量 SparesQuantity
        /// <summary>
        /// 备品数量
        /// </summary>
        [Label("备品数量")]
        public static readonly Property<decimal> SparesQuantityProperty = P<SaleOrderDetail>.Register(e => e.SparesQuantity);

        /// <summary>
        /// 备品数量
        /// </summary>
        public decimal SparesQuantity
        {
            get { return this.GetProperty(SparesQuantityProperty); }
            set { this.SetProperty(SparesQuantityProperty, value); }
        }
        #endregion

        //#region 审核日期 ApproveDate
        ///// <summary>
        ///// 审核日期
        ///// </summary>
        //[Label("审核日期")]
        //public static readonly Property<DateTime> ApproveDateProperty = P<SaleOrderDetail>.Register(e => e.ApproveDate);

        ///// <summary>
        ///// 审核日期
        ///// </summary>
        //public DateTime ApproveDate
        //{
        //    get { return this.GetProperty(ApproveDateProperty); }
        //    set { this.SetProperty(ApproveDateProperty, value); }
        //}
        //#endregion

        #region 货运方式 FreightMode
        /// <summary>
        /// 货运方式
        /// </summary>
        [Label("发运方式")]
        public static readonly Property<string> FreightModeProperty = P<SaleOrderDetail>.Register(e => e.FreightMode);

        /// <summary>
        /// 货运方式
        /// </summary>
        public string FreightMode
        {
            get { return this.GetProperty(FreightModeProperty); }
            set { this.SetProperty(FreightModeProperty, value); }
        }
        #endregion

        #region 未交货数 UndeliveredQuantity
        /// <summary>
        /// 未交货数
        /// </summary>
        [Label("未交货数")]
        public static readonly Property<decimal> UndeliveredQuantityProperty = P<SaleOrderDetail>.Register(e => e.UndeliveredQuantity);

        /// <summary>
        /// 未交货数
        /// </summary>
        public decimal UndeliveredQuantity
        {
            get { return this.GetProperty(UndeliveredQuantityProperty); }
            set { this.SetProperty(UndeliveredQuantityProperty, value); }
        }
        #endregion

        #region 拒绝状态 RejectStatus
        /// <summary>
        /// 拒绝状态
        /// </summary>
        [Label("拒绝状态")]
        public static readonly Property<string> RejectStatusProperty = P<SaleOrderDetail>.Register(e => e.RejectStatus);

        /// <summary>
        /// 拒绝状态
        /// </summary>
        public string RejectStatus
        {
            get { return this.GetProperty(RejectStatusProperty); }
            set { this.SetProperty(RejectStatusProperty, value); }
        }
        #endregion

        #region 库存地点 InventoryLocation
        /// <summary>
        /// 库存地点
        /// </summary>
        [Label("库存地点")]
        public static readonly Property<string> InventoryLocationProperty = P<SaleOrderDetail>.Register(e => e.InventoryLocation);

        /// <summary>
        /// 库存地点
        /// </summary>
        public string InventoryLocation
        {
            get { return this.GetProperty(InventoryLocationProperty); }
            set { this.SetProperty(InventoryLocationProperty, value); }
        }
        #endregion

        #region 装运点 ShippingPoint
        /// <summary>
        /// 装运点
        /// </summary>
        [Label("装运点")]
        public static readonly Property<string> ShippingPointProperty = P<SaleOrderDetail>.Register(e => e.ShippingPoint);

        /// <summary>
        /// 装运点
        /// </summary>
        public string ShippingPoint
        {
            get { return this.GetProperty(ShippingPointProperty); }
            set { this.SetProperty(ShippingPointProperty, value); }
        }
        #endregion

        #region 已交货数量 IssuedQuantity
        /// <summary>
        /// 已交货数量
        /// </summary>
        [Label("已交货数量")]
        public static readonly Property<decimal> IssuedQuantityProperty = P<SaleOrderDetail>.Register(e => e.IssuedQuantity);

        /// <summary>
        /// 已交货数量
        /// </summary>
        public decimal IssuedQuantity
        {
            get { return this.GetProperty(IssuedQuantityProperty); }
            set { this.SetProperty(IssuedQuantityProperty, value); }
        }
        #endregion

        #region 订单面积 OrderArea
        /// <summary>
        /// 订单面积
        /// </summary>
        [Label("订单面积")]
        public static readonly Property<decimal> OrderAreaProperty = P<SaleOrderDetail>.Register(e => e.OrderArea);

        /// <summary>
        /// 订单面积
        /// </summary>
        public decimal OrderArea
        {
            get { return this.GetProperty(OrderAreaProperty); }
            set { this.SetProperty(OrderAreaProperty, value); }
        }
        #endregion

        #region 订单类别 OrderCategory
        /// <summary>
        /// 订单类别
        /// </summary>
        [Label("订单类别")]
        public static readonly Property<string> OrderCategoryProperty = P<SaleOrderDetail>.Register(e => e.OrderCategory);

        /// <summary>
        /// 订单类别
        /// </summary>
        public string OrderCategory
        {
            get { return this.GetProperty(OrderCategoryProperty); }
            set { this.SetProperty(OrderCategoryProperty, value); }
        }
        #endregion

        #region 下单次数 OrderNumber
        /// <summary>
        /// 下单次数
        /// </summary>
        [Label("下单次数")]
        public static readonly Property<int> OrderNumberProperty = P<SaleOrderDetail>.Register(e => e.OrderNumber);

        /// <summary>
        /// 下单次数
        /// </summary>
        public int OrderNumber
        {
            get { return this.GetProperty(OrderNumberProperty); }
            set { this.SetProperty(OrderNumberProperty, value); }
        }
        #endregion

        #region 物料齐套时间 ItemSetTime
        /// <summary>
        /// 物料齐套时间
        /// </summary>
        [Label("物料齐套时间")]
        public static readonly Property<DateTime?> ItemSetTimeProperty = P<SaleOrderDetail>.Register(e => e.ItemSetTime);

        /// <summary>
        /// 物料齐套时间
        /// </summary>
        public DateTime? ItemSetTime
        {
            get { return this.GetProperty(ItemSetTimeProperty); }
            set { this.SetProperty(ItemSetTimeProperty, value); }
        }
        #endregion

        #endregion

        #region 表面处理 Coating
        /// <summary>
        /// 表面处理
        /// </summary>
        [Label("表面处理")]
        public static readonly Property<string> CoatingProperty = P<SaleOrderDetail>.Register(e => e.Coating);

        /// <summary>
        /// 表面处理
        /// </summary>
        public string Coating
        {
            get { return this.GetProperty(CoatingProperty); }
            set { this.SetProperty(CoatingProperty, value); }
        }
        #endregion

        #region 大板数 BigPlateQty
        /// <summary>
        /// 大板数
        /// </summary>
        [Label("大板数")]
        public static readonly Property<decimal> BigPlateQtyProperty = P<SaleOrderDetail>.Register(e => e.BigPlateQty);

        /// <summary>
        /// 大板数
        /// </summary>
        public decimal BigPlateQty
        {
            get { return GetProperty(BigPlateQtyProperty); }
            set { SetProperty(BigPlateQtyProperty, value); }
        }
        #endregion

        #region 压合次数 LaminationQty
        /// <summary>
        /// 压合次数
        /// </summary>
        [Label("压合次数")]
        public static readonly Property<int> LaminationQtyProperty = P<SaleOrderDetail>.Register(e => e.LaminationQty);

        /// <summary>
        /// 压合次数
        /// </summary>
        public int LaminationQty
        {
            get { return GetProperty(LaminationQtyProperty); }
            set { SetProperty(LaminationQtyProperty, value); }
        }
        #endregion

        #region 合同评审MI状态 MendMIState
        /// <summary>
        /// 合同评审MI状态
        /// </summary>
        [Label("合同评审MI状态")]
        public static readonly Property<MiState> MendMIStateProperty = P<SaleOrderDetail>.Register(e => e.MendMIState);

        /// <summary>
        /// 合同评审MI状态
        /// </summary>
        public MiState MendMIState
        {
            get { return GetProperty(MendMIStateProperty); }
            set { SetProperty(MendMIStateProperty, value); }
        }
        #endregion

        #region 终版MI状态 FinishMIState
        /// <summary>
        /// 终版MI状态
        /// </summary>
        [Label("终版MI状态")]
        public static readonly Property<MiState> FinishMIStateProperty = P<SaleOrderDetail>.Register(e => e.FinishMIState);

        /// <summary>
        /// 终版MI状态
        /// </summary>
        public MiState FinishMIState
        {
            get { return GetProperty(FinishMIStateProperty); }
            set { SetProperty(FinishMIStateProperty, value); }
        }
        #endregion

        #region 投料状态 FeedState
        /// <summary>
        /// 投料状态
        /// </summary>
        [Label("投料状态")]
        public static readonly Property<string> FeedStateProperty = P<SaleOrderDetail>.Register(e => e.FeedState);

        /// <summary>
        /// 投料状态
        /// </summary>
        public string FeedState
        {
            get { return GetProperty(FeedStateProperty); }
            set { SetProperty(FeedStateProperty, value); }
        }
        #endregion

        #region 版本号 ItemRevision
        /// <summary>
        /// 版本号 （物料扩展属性id:版本号内容）
        /// </summary>
        [Label("版本号")]
        public static readonly Property<string> ItemRevisionProperty = P<SaleOrderDetail>.Register(e => e.ItemRevision);

        /// <summary>
        /// 版本号
        /// </summary>
        public string ItemRevision
        {
            get { return GetProperty(ItemRevisionProperty); }
            set { SetProperty(ItemRevisionProperty, value); }
        }
        #endregion

        #region 版本号显示 ItemExtPropName
        /// <summary>
        ///  版本号显示  （物料扩展属性名称:版本号内容）
        /// </summary>
        [Label(" 版本号")]
        public static readonly Property<string> ItemExtPropNameProperty = P<SaleOrderDetail>.Register(e => e.ItemExtPropName);

        /// <summary>
        ///  版本号显示
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料是否扩展 ItemEnableExtendProperty
        /// <summary>
        /// 物料是否扩展
        /// </summary>
        public static readonly Property<bool> ItemEnableExtendPropertyProperty = P<SaleOrderDetail>
            .RegisterView(e => e.ItemEnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 物料是否扩展
        /// </summary>
        public bool ItemEnableExtendProperty
        {
            get { return this.GetProperty(ItemEnableExtendPropertyProperty); }
        }
        #endregion

        #region 已打包数量 PackagedQty
        /// <summary>
        /// 已打包数量
        /// </summary>
        [Label("已打包数量")]
        public static readonly Property<decimal> PackagedQtyProperty = P<SaleOrderDetail>.Register(e => e.PackagedQty);

        /// <summary>
        /// 已打包数量
        /// </summary>
        public decimal PackagedQty
        {
            get { return GetProperty(PackagedQtyProperty); }
            set { SetProperty(PackagedQtyProperty, value); }
        }
        #endregion

        #region 只读属性
        #region 未打包Unit数 NotPackageQty
        /// <summary>
        /// 未打包Unit数
        /// </summary>
        [Label("未打包Unit数")]
        public static readonly Property<decimal> NotPackageQtyProperty
            = P<SaleOrderDetail>.RegisterReadOnly(e => e.NotPackageQty,
                e => e.GetNotPackageQty(), PackagedQtyProperty, QtyProperty);

        /// <summary>
        /// 未打包Unit数
        /// </summary>

        public decimal NotPackageQty
        {
            get { return this.GetProperty(NotPackageQtyProperty); }
        }

        private decimal GetNotPackageQty()
        {
            if (PackagedQty <= Qty)
            {
                return Qty - PackagedQty;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 销售订单行号 SaleOrderLineNo
        /// <summary>
        /// 销售订单行号
        /// </summary>
        [Label("销售订单行号")]
        public static readonly Property<string> SaleOrderLineNoProperty = P<SaleOrderDetail>.RegisterReadOnly(
            e => e.SaleOrderLineNo, e => e.GetSaleOrderLineNo(), SaleOrderProperty, LineNoProperty);
        /// <summary>
        /// 销售订单行号
        /// </summary>

        public string SaleOrderLineNo
        {
            get { return this.GetProperty(SaleOrderLineNoProperty); }
        }
        private string GetSaleOrderLineNo()
        {
            return string.Format("{0}-{1}", SaleOrder?.Code, LineNo);
        }
        #endregion
        #endregion

    }

    /// <summary>
    /// 订单明细 实体配置
    /// </summary>
    internal class SalesOrderDetailConfig : EntityConfig<SaleOrderDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("SALE_ORDER_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
        }

    }
}
