using SIE.Common.Configs;
using SIE.CSM.Customers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Items;
using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.Kit.APS.EngineerPlans.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.SO.SaleOrders;
using System;

namespace SIE.Kit.APS.EngineerPlans
{
    /// <summary>
    /// 工程计划
    /// </summary>
    [RootEntity, Serializable]
    [Label("工程计划")]
    [ConditionQueryType(typeof(EngineerPlanCriteria))]
    [EntityWithConfig(typeof(EngineerPlan_NewEcnPre_Config))]
    [EntityWithConfig(typeof(EngineerPlan_IsOverTimeTakeCapacity_Config))]
    [EntityWithConfig(typeof(EngineerPlan_SST_Config))]
    [EntityDataAuthAttribute(typeof(EmployeeEnterprise), nameof(FactoryId), false)]
    public partial class EngineerPlan : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<EngineerPlan>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)GetRefId(FactoryIdProperty); }
            set { SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<EngineerPlan>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 排单日期  ScheduleDay
        /// <summary>
        /// 排单日期
        /// </summary>
        [Label("排单日期")]
        public static readonly Property<DateTime?> ScheduleDayProperty = P<EngineerPlan>.Register(e => e.ScheduleDay);

        /// <summary>
        ///  排单日期
        /// </summary>
        public DateTime? ScheduleDay
        {
            get { return GetProperty(ScheduleDayProperty); }
            set { SetProperty(ScheduleDayProperty, value); }
        }
        #endregion

        #region 落单日期  SortDate
        /// <summary>
        /// 落单日期
        /// </summary>
        [Label("落单日期")]
        public static readonly Property<DateTime?> SortDateProperty = P<EngineerPlan>.Register(e => e.SortDate);

        /// <summary>
        ///  落单日期
        /// </summary>
        public DateTime? SortDate
        {
            get { return GetProperty(SortDateProperty); }
            set { SetProperty(SortDateProperty, value); }
        }
        #endregion

        #region 排序顺序  SortIndex
        /// <summary>
        /// 排序顺序
        /// </summary>
        [Label("排序顺序")]
        public static readonly Property<String> SortIndexProperty = P<EngineerPlan>.Register(e => e.SortIndex);

        /// <summary>
        ///  排序顺序
        /// </summary>
        public String SortIndex
        {
            get { return GetProperty(SortIndexProperty); }
            set { SetProperty(SortIndexProperty, value); }
        }
        #endregion

        #region 指定加急 IsUrgent
        /// <summary>
        /// 指定加急
        /// </summary>
        [Label("指定加急")]
        public static readonly Property<bool> IsUrgentProperty = P<EngineerPlan>.Register(e => e.IsUrgent);

        /// <summary>
        /// 指定加急
        /// </summary>
        public bool IsUrgent
        {
            get { return GetProperty(IsUrgentProperty); }
            set { SetProperty(IsUrgentProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EngineerPlan>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 工程计划状态 PlanState
        /// <summary>
        /// 工程计划状态
        /// </summary>
        [Label("工程计划状态")]
        public static readonly Property<SOMI_PlanState> PlanStateProperty = P<EngineerPlan>.Register(e => e.PlanState);

        /// <summary>
        /// 工程计划状态
        /// </summary>
        public SOMI_PlanState PlanState
        {
            get { return GetProperty(PlanStateProperty); }
            set { SetProperty(PlanStateProperty, value); }
        }
        #endregion

        #region 客户优先级 CustLevel
        /// <summary>
        /// 客户优先级Id
        /// </summary>
        public static readonly IRefIdProperty CustLevelIdProperty = P<EngineerPlan>.RegisterRefId(e => e.CustLevelId, ReferenceType.Normal);

        /// <summary>
        /// 客户优先级Id
        /// </summary>
        public double? CustLevelId
        {
            get { return (double?)GetRefNullableId(CustLevelIdProperty); }
            set { SetRefNullableId(CustLevelIdProperty, value); }
        }

        /// <summary>
        /// 客户优先级
        /// </summary>
        public static readonly RefEntityProperty<CustLevel> CustLevelProperty = P<EngineerPlan>.RegisterRef(e => e.CustLevel, CustLevelIdProperty);

        /// <summary>
        /// 客户优先级
        /// </summary>
        public CustLevel CustLevel
        {
            get { return GetRefEntity(CustLevelProperty); }
            set { SetRefEntity(CustLevelProperty, value); }
        }
        #endregion

        #region 时效性 Hour
        /// <summary>
        /// 时效性(H)
        /// </summary>
        [Label("时效性(H)")]
        public static readonly Property<int?> HourProperty = P<EngineerPlan>.Register(e => e.Hour);

        /// <summary>
        ///  时效性(H)
        /// </summary>
        public int? Hour
        {
            get { return GetProperty(HourProperty); }
            set { SetProperty(HourProperty, value); }
        }
        #endregion

        #region 分配行次(用于调试) DaySortIndex 
        /// <summary>
        /// 分配行次(用于调试)
        /// </summary>
        [Label("分配行次(用于调试)")]
        public static readonly Property<int?> DaySortIndexProperty = P<EngineerPlan>.Register(e => e.DaySortIndex);

        /// <summary>
        /// 分配行次(用于调试)
        /// </summary>
        public int? DaySortIndex
        {
            get { return GetProperty(DaySortIndexProperty); }
            set { SetProperty(DaySortIndexProperty, value); }
        }
        #endregion




        //合同评审MI状态
        //终版MI状态
        #region 订单分类 OrderClassify
        /// <summary>
        /// 订单分类
        /// </summary>
        [Label("订单分类")]
        public static readonly Property<string> OrderClassifyProperty = P<EngineerPlan>.Register(e => e.OrderClassify);

        /// <summary>
        /// 订单分类
        /// </summary>
        public string OrderClassify
        {
            get { return this.GetProperty(OrderClassifyProperty); }
            set { this.SetProperty(OrderClassifyProperty, value); }
        }
        #endregion

        #region 产品类型 ProductType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Label("产品类型")]
        public static readonly Property<string> ProductTypeProperty = P<EngineerPlan>.Register(e => e.ProductType);

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType
        {
            get { return GetProperty(ProductTypeProperty); }
            set { SetProperty(ProductTypeProperty, value); }
        }
        #endregion

        //表面处理?

        #region 销售订单  多个信息冗余 

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<EngineerPlan>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return GetProperty(SaleOrderNoProperty); }
            set { SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 订单行号 LineNo
        /// <summary>
        /// 订单行号
        /// </summary>
        [Label("订单行号")]
        public static readonly Property<string> LineNoProperty = P<EngineerPlan>.Register(e => e.LineNo);

        /// <summary>
        /// 订单行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 版本号 ItemRevision
        /// <summary>
        /// 版本号 （物料扩展属性id:版本号内容）
        /// </summary>
        [Label("版本号")]
        public static readonly Property<string> ItemRevisionProperty = P<EngineerPlan>.Register(e => e.ItemRevision);

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
        public static readonly Property<string> ItemExtPropNameProperty = P<EngineerPlan>.Register(e => e.ItemExtPropName);

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
        public static readonly Property<bool> ItemEnableExtendPropertyProperty = P<EngineerPlan>
            .RegisterView(e => e.ItemEnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 物料是否扩展
        /// </summary>
        public bool ItemEnableExtendProperty
        {
            get { return this.GetProperty(ItemEnableExtendPropertyProperty); }
            set { SetProperty(ItemEnableExtendPropertyProperty, value); }
        }
        #endregion

        #region 物料(生产型号) Item
        /// <summary>
        /// 物料(生产型号)Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<EngineerPlan>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料(生产型号)Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 生产型号
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<EngineerPlan>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 生产型号
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<EngineerPlan>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<EngineerPlan>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public double CustomerId
        {
            get { return (double)GetRefId(CustomerIdProperty); }
            set { SetRefId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<EngineerPlan>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion



        #region 销售订单行 SaleOrderDetail
        /// <summary>
        /// 销售订单行Id
        /// </summary>
        public static readonly IRefIdProperty SaleOrderDetailIdProperty = P<EngineerPlan>.RegisterRefId(e => e.SaleOrderDetailId, ReferenceType.Normal);

        /// <summary>
        /// 销售订单行Id
        /// </summary>
        public double? SaleOrderDetailId
        {
            get { return (double?)GetRefNullableId(SaleOrderDetailIdProperty); }
            set { SetRefNullableId(SaleOrderDetailIdProperty, value); }
        }

        /// <summary>
        /// 生产型号
        /// </summary>
        public static readonly RefEntityProperty<SaleOrderDetail> SaleOrderDetailProperty = P<EngineerPlan>.RegisterRef(e => e.SaleOrderDetail, SaleOrderDetailIdProperty);

        /// <summary>
        /// 生产型号
        /// </summary>
        public SaleOrderDetail SaleOrderDetail
        {
            get { return GetRefEntity(SaleOrderDetailProperty); }
            set { SetRefEntity(SaleOrderDetailProperty, value); }
        }
        #endregion

        #region 是否新单 IsNew
        /// <summary>
        /// 是否新单
        /// </summary>
        [Label("是否新单")]
        public static readonly Property<bool> IsNewProperty = P<EngineerPlan>.Register(e => e.IsNew);

        /// <summary>
        /// 是否新单
        /// </summary>
        public bool IsNew
        {
            get { return GetProperty(IsNewProperty); }
            set { SetProperty(IsNewProperty, value); }
        }
        #endregion

        #region 外部ECN ExternalEcn
        /// <summary>
        /// 外部ECN
        /// </summary>
        [Label("外部ECN")]
        public static readonly Property<bool> ExternalEcnProperty = P<EngineerPlan>.Register(e => e.ExternalEcn);

        /// <summary>
        /// 外部ECN
        /// </summary>
        public bool ExternalEcn
        {
            get { return this.GetProperty(ExternalEcnProperty); }
            set { this.SetProperty(ExternalEcnProperty, value); }
        }
        #endregion

        #region 订单数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("订单数量")]
        public static readonly Property<decimal> QtyProperty = P<EngineerPlan>.Register(e => e.Qty);

        /// <summary>
        /// 订单数量
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
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty = P<EngineerPlan>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Unit> UnitProperty = P<EngineerPlan>.RegisterRef(e => e.Unit, UnitIdProperty);

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

        #region 总面积M2 Area
        /// <summary>
        /// 面积M2
        /// </summary>
        [Label("总面积M2")]
        public static readonly Property<decimal> AreaProperty = P<EngineerPlan>.Register(e => e.Area);

        /// <summary>
        /// 总面积M2
        /// </summary>
        public decimal Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 下单日期 CustomerPoDate
        /// <summary>
        /// 下单日期
        /// </summary>
        [Label("下单日期")]
        public static readonly Property<DateTime?> CustomerPoDateProperty = P<EngineerPlan>.Register(e => e.CustomerPoDate);

        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime? CustomerPoDate
        {
            get { return this.GetProperty(CustomerPoDateProperty); }
            set { this.SetProperty(CustomerPoDateProperty, value); }
        }
        #endregion

        #region 登记时间 RegisterDateTime
        /// <summary>
        /// 登记时间
        /// </summary>
        [Label("登记时间")]
        public static readonly Property<DateTime?> RegisterDateTimeProperty = P<EngineerPlan>.Register(e => e.RegisterDateTime);

        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime? RegisterDateTime
        {
            get { return GetProperty(RegisterDateTimeProperty); }
            set { SetProperty(RegisterDateTimeProperty, value); }
        }
        #endregion

        #region 客户交期 RequireDelivery
        /// <summary>
        /// 客户交期
        /// </summary>
        [Label("客户交期")]
        public static readonly Property<DateTime> RequireDeliveryProperty = P<EngineerPlan>.Register(e => e.RequireDelivery);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery
        {
            get { return GetProperty(RequireDeliveryProperty); }
            set { SetProperty(RequireDeliveryProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<string> OrderTypeProperty = P<EngineerPlan>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 快板类型 AllegroType
        /// <summary>
        /// 快板类型
        /// </summary>
        [Label("快板类型")]
        public static readonly Property<string> AllegroTypeProperty = P<EngineerPlan>.Register(e => e.AllegroType);

        /// <summary>
        /// 快板类型
        /// </summary>
        public string AllegroType
        {
            get { return this.GetProperty(AllegroTypeProperty); }
            set { this.SetProperty(AllegroTypeProperty, value); }
        }
        #endregion

        #region 应用领域 AppArea
        /// <summary>
        /// 应用领域
        /// </summary>
        [Label("应用领域")]
        public static readonly Property<string> AppAreaProperty = P<EngineerPlan>.Register(e => e.AppArea);

        /// <summary>
        /// 应用领域
        /// </summary>
        public string AppArea
        {
            get { return this.GetProperty(AppAreaProperty); }
            set { this.SetProperty(AppAreaProperty, value); }
        }
        #endregion

        #endregion


        #region 视图属性 

        #region 订单行状态 LineState
        /// <summary>
        /// 订单行状态
        /// </summary>
        [Label("订单行状态")]
        public static readonly Property<LineState> LineStateProperty = P<EngineerPlan>.RegisterView(e => e.LineState, p => p.SaleOrderDetail.LineState);

        /// <summary>
        /// 订单行状态
        /// </summary>
        public LineState LineState
        {
            get { return GetProperty(LineStateProperty); }
            set { SetProperty(LineStateProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<EngineerPlan>.RegisterView(e => e.CustomerCode, p => p.Customer.Code);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<EngineerPlan>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
            set { SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #endregion
    }


    /// <summary>
    ///  实体配置
    /// </summary>
    internal class EngineerPlanEntityConfig : EntityConfig<EngineerPlan>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("APS_MSO_MI_PLAN").MapAllPropertiesExcept();
            Meta.EnablePhantoms();
        }
    }

}
