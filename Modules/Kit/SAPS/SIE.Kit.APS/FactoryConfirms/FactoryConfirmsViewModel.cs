using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.SO.SaleOrders;
using System;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 厂别确认
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FactoryConfirmsViewModelCriteria))]
    [Label("厂别确认")]
    public class FactoryConfirmsViewModel : Entity<double>
    {
        #region 销售订单 SaleOrderId
        /// <summary>
        /// 销售订单SaleOrderId
        /// </summary>
        public static readonly IRefIdProperty SaleOrderIdProperty = P<FactoryConfirmsViewModel>.RegisterRefId(e => e.SaleOrderId, ReferenceType.Parent);
        /// <summary>
        /// 销售订单Id
        /// </summary>
        public double SaleOrderId
        {
            get { return (double)GetRefId(SaleOrderIdProperty); }
            set { SetRefId(SaleOrderIdProperty, value); }
        }
        #endregion

        #region 销售订单 SaleOrder
        /// <summary>
        /// 销售订单
        /// </summary>
        public static readonly RefEntityProperty<SaleOrder> SaleOrderProperty = P<FactoryConfirmsViewModel>.RegisterRef(e => e.SaleOrder, SaleOrderIdProperty);

        /// <summary>
        /// 销售订单
        /// </summary>
        public SaleOrder SaleOrder
        {
            get { return GetRefEntity(SaleOrderProperty); }
            set { SetRefEntity(SaleOrderProperty, value); }
        }
        #endregion

        #region 销售订单编码 SalesOrderCode
        /// <summary>
        /// 销售订单编码
        /// </summary>
        [Label("销售订单编码")]
        public static readonly Property<string> SalesOrderCodeProperty = P<FactoryConfirmsViewModel>.RegisterView(e => e.SalesOrderCode, p => p.SaleOrder.Code);

        /// <summary>
        /// 销售订单编码
        /// </summary>
        public string SalesOrderCode
        {
            get { return this.GetProperty(SalesOrderCodeProperty); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        public static readonly Property<string> LineNoProperty = P<FactoryConfirmsViewModel>.Register(e => e.LineNo);
        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }



        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<FactoryConfirmsViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<FactoryConfirmsViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> ItemCodeProperty = P<FactoryConfirmsViewModel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<FactoryConfirmsViewModel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 版本号 ItemRevision
        /// <summary>
        /// 版本号 （物料扩展属性id:版本号内容）
        /// </summary>
        [Label("版本号")]
        public static readonly Property<string> ItemRevisionProperty = P<FactoryConfirmsViewModel>.Register(e => e.ItemRevision);

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
        public static readonly Property<string> ItemExtPropNameProperty = P<FactoryConfirmsViewModel>.Register(e => e.ItemExtPropName);

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
        public static readonly Property<bool> ItemEnableExtendPropertyProperty = P<FactoryConfirmsViewModel>
            .RegisterView(e => e.ItemEnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 物料是否扩展
        /// </summary>
        public bool ItemEnableExtendProperty
        {
            get { return this.GetProperty(ItemEnableExtendPropertyProperty); }
        }
        #endregion

        #region 行业类型 IndustryType
        /// <summary>
        /// 行业类型
        /// </summary>
        [Required]
        [Label("行业类型")]
        public static readonly Property<string> IndustryTypeProperty = P<FactoryConfirmsViewModel>.Register(e => e.IndustryType);

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
        public static readonly Property<string> OrderTypeProperty = P<FactoryConfirmsViewModel>.Register(e => e.OrderType);

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
        public static readonly Property<string> ProductTypeProperty = P<FactoryConfirmsViewModel>.Register(e => e.ProductType);

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType
        {
            get { return GetProperty(ProductTypeProperty); }
            set { SetProperty(ProductTypeProperty, value); }
        }
        #endregion  

        #region 是否新单 IsNew
        /// <summary>
        /// 是否新单
        /// </summary>
        [Required]
        [Label("是否新单")]
        public static readonly Property<bool> IsNewProperty = P<FactoryConfirmsViewModel>.Register(e => e.IsNew);

        /// <summary>
        /// 是否新单
        /// </summary>
        public bool IsNew
        {
            get { return GetProperty(IsNewProperty); }
            set { SetProperty(IsNewProperty, value); }
        }
        #endregion

        #region 行状态 LineState
        /// <summary>
        /// 行状态
        /// </summary>
        [Label("行状态")]
        public static readonly Property<LineState> LineStateProperty = P<FactoryConfirmsViewModel>.Register(e => e.LineState);

        /// <summary>
        /// 行状态
        /// </summary>
        public LineState LineState
        {
            get { return GetProperty(LineStateProperty); }
            set { SetProperty(LineStateProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<FactoryConfirmsViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>

        public static readonly IRefIdProperty UnitIdProperty = P<FactoryConfirmsViewModel>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId
        {
            get { return (double)GetRefId(UnitIdProperty); }
            set { SetRefId(UnitIdProperty, value); }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<FactoryConfirmsViewModel>.RegisterRef(e => e.Unit, UnitIdProperty);

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

        #region 单位名称  UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<FactoryConfirmsViewModel>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
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

        #region 库存组织 Enterprise
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public static readonly IRefIdProperty EnterpriseIdProperty = P<FactoryConfirmsViewModel>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public double EnterpriseId
        {
            get { return (double)GetRefId(EnterpriseIdProperty); }
            set { SetRefId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 库存组织
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<FactoryConfirmsViewModel>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 库存组织
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 库存组织名称 EnterpriseName
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("库存组织名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<FactoryConfirmsViewModel>.RegisterView(e => e.EnterpriseName, p => p.Enterprise.Name);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
        }

        #endregion

        #region MI完成时间 MiDateTime
        /// <summary>
        /// MI完成时间
        /// </summary>
        [Required]
        [Label("MI完成时间")]
        public static readonly Property<DateTime> MiDateTimeProperty = P<FactoryConfirmsViewModel>.Register(e => e.MiDateTime);

        /// <summary>
        /// MI完成时间
        /// </summary>
        public DateTime MiDateTime
        {
            get { return GetProperty(MiDateTimeProperty); }
            set { SetProperty(MiDateTimeProperty, value); }
        }
        #endregion

        #region 面积M2 Area
        /// <summary>
        /// 面积M2
        /// </summary>
        [Required]
        [Label("面积M2")]
        public static readonly Property<decimal> AreaProperty = P<FactoryConfirmsViewModel>.Register(e => e.Area);

        /// <summary>
        /// 面积M2
        /// </summary>
        public decimal Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 大板尺寸 PlateSize
        /// <summary>
        /// 大板尺寸
        /// </summary>
        [Required]
        [Label("大板尺寸")]
        public static readonly Property<string> PlateSizeProperty = P<FactoryConfirmsViewModel>.Register(e => e.PlateSize);

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
        [Required]
        [Label("开料PNL数")]
        public static readonly Property<decimal> MaterialPnlProperty = P<FactoryConfirmsViewModel>.Register(e => e.MaterialPnl);

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
        [Required]
        [Label("SET/PNL数")]
        public static readonly Property<decimal> SetPnlProperty = P<FactoryConfirmsViewModel>.Register(e => e.SetPnl);

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
        [Required]
        [Label("PCS/PNL数")]
        public static readonly Property<decimal> PcsPnlProperty = P<FactoryConfirmsViewModel>.Register(e => e.PcsPnl);

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
        [Required]
        [Label("客户交期")]
        public static readonly Property<DateTime> RequireDeliveryProperty = P<FactoryConfirmsViewModel>.Register(e => e.RequireDelivery);

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
        public static readonly Property<DateTime?> PromiseDeliveryProperty = P<FactoryConfirmsViewModel>.Register(e => e.PromiseDelivery);

        /// <summary>
        /// 承诺交期
        /// </summary>
        public DateTime? PromiseDelivery
        {
            get { return GetProperty(PromiseDeliveryProperty); }
            set { SetProperty(PromiseDeliveryProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<FactoryConfirmsViewModel>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double CustomerId
        {
            get { return (double)GetRefId(CustomerIdProperty); }
            set { SetRefId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<FactoryConfirmsViewModel>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<FactoryConfirmsViewModel>.RegisterView(e => e.CustomerCode, p => p.Customer.Code);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
        }
        #endregion

        #region 客户名称  CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<FactoryConfirmsViewModel>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 订单行挂起 IsHangUp
        /// <summary>
        /// 订单行挂起
        /// </summary>
        [Label("订单行挂起")]
        public static readonly Property<bool> IsHangUpProperty = P<FactoryConfirmsViewModel>.Register(e => e.IsHangUp);

        /// <summary>
        /// 订单行挂起
        /// </summary>
        public bool IsHangUp
        {
            get { return GetProperty(IsHangUpProperty); }
            set { SetProperty(IsHangUpProperty, value); }
        }
        #endregion

        #region 产品等级 ProductLevel
        /// <summary>
        /// 产品等级
        /// </summary>
        [Required]
        [Label("产品等级")]
        public static readonly Property<string> ProductLevelProperty = P<FactoryConfirmsViewModel>.Register(e => e.ProductLevel);

        /// <summary>
        /// 产品等级
        /// </summary>
        public string ProductLevel
        {
            get { return GetProperty(ProductLevelProperty); }
            set { SetProperty(ProductLevelProperty, value); }
        }
        #endregion

        #region 生成工程计划状态 EngineeringPlanState
        /// <summary>
        /// 生成工程计划状态
        /// </summary>
        [Label("生成工程计划状态")]
        public static readonly Property<bool> EngineeringPlanStateProperty = P<FactoryConfirmsViewModel>.Register(e => e.EngineeringPlanState);

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
        public static readonly Property<string> EngineeringPlanRemarkProperty = P<FactoryConfirmsViewModel>.Register(e => e.EngineeringPlanRemark);

        /// <summary>
        /// 生成工程计划原因
        /// </summary>
        public string EngineeringPlanRemark
        {
            get { return GetProperty(EngineeringPlanRemarkProperty); }
            set { SetProperty(EngineeringPlanRemarkProperty, value); }
        }
        #endregion

        #region 特殊工艺 SpecialProcessStr
        /// <summary>
        /// 特殊工艺
        /// </summary>
        [Label("特殊工艺")]
        public static readonly Property<string> SpecialProcessStrProperty = P<FactoryConfirmsViewModel>.Register(e => e.SpecialProcessStr);

        /// <summary>
        /// 特殊工艺
        /// </summary>
        public string SpecialProcessStr
        {
            get { return GetProperty(SpecialProcessStrProperty); }
            set { SetProperty(SpecialProcessStrProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class SalesOrderDetailConfig : EntityConfig<FactoryConfirmsViewModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<SaleOrderDetail>("SaleOrderDetail")
                 .LeftJoin<SaleOrder>("SaleOrder", (x, y) => x.SaleOrderId == y.Id)
                 .LeftJoin<Enterprise>("Enterprise", (x, z) => x.EnterpriseId == z.Id)
                 .LeftJoin<Item>("Item", (x, i) => x.ItemId == i.Id)
                 .LeftJoin<SaleOrder, Customer>("Customer", (x, i) => x.CustomerId == i.Id)
                 .LeftJoin<Unit>("Unit", (x, i) => x.UnitId == i.Id)
                 .Select<SaleOrder, Enterprise, Item, Customer, Unit>(
                     (SalesOrderDetail, SalesOrder, Enterprise, Item, Customer, Unit) =>
                         new
                         {
                             SalesOrderDetail.Id,
                             Sale_Order_Id = SalesOrder.Id,
                             Sale_Order_Code = SalesOrder.Code,
                             Line_No = SalesOrderDetail.LineNo,
                             Item_Id = SalesOrderDetail.ItemId,
                             Item_Name = Item.Name,
                             Item_Code = Item.Code,
                             Item_Revision=SalesOrderDetail.ItemRevision,
                             Item_Ext_Prop_Name = SalesOrderDetail.ItemExtPropName,
                             Industry_Type = SalesOrderDetail.IndustryType,
                             Order_Type = SalesOrderDetail.OrderType,
                             Product_Type = SalesOrderDetail.ProductType,
                             Product_Level = SalesOrderDetail.ProductLevel,
                             Is_New = SalesOrderDetail.IsNew,
                             Line_State = SalesOrderDetail.LineState,
                             Qty = SalesOrderDetail.Qty,
                             Customer_Id = Customer.Id,
                             Customer_Code = Customer.Code,
                             Customer_Name = Customer.Name,
                             Unit_Id = Unit.Id,
                             Unit_Name = Unit.Name,
                             Enterprise_Id = Enterprise.Id,
                             Enterprise_Name = Enterprise.Name,
                             Mi_Date_Time = SalesOrderDetail.MiDateTime,
                             Area = SalesOrderDetail.Area,
                             Plate_Size = SalesOrderDetail.PlateSize,
                             Material_Pnl = SalesOrderDetail.MaterialPnl,
                             Set_Pnl = SalesOrderDetail.SetPnl,
                             Pcs_Pnl = SalesOrderDetail.PcsPnl,
                             Require_Delivery = SalesOrderDetail.RequireDelivery,
                             Promise_Delivery = SalesOrderDetail.PromiseDelivery,
                             Is_Hang_Up = SalesOrderDetail.IsHangUp,
                             Target_Order_Code = SalesOrderDetail.TargetOrderCode,
                             Engineering_Plan_State = SalesOrderDetail.EngineeringPlanState,
                             Engineering_Plan_Remark = SalesOrderDetail.EngineeringPlanRemark,
                             Special_Process_Str = SalesOrderDetail.SpecialProcessStr
                         })
                 .ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
        }
    }
}
