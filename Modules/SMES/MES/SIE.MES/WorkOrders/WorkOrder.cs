using SIE.Common;
using SIE.Common.Configs;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MES.BatchGeneration;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.WorkOrders.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs;
using SIE.Resources.WipResources;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkOrderCriteria))]
    [EntityWithConfig(typeof(WorkOrderNoConfig))]
    [EntityWithConfig(typeof(PrintTemplateConfig))]    
    [EntityWithConfig(typeof(ReferenceWoBomConfig))]
    [EntityWithConfig(typeof(AutoStartWoDataTraceWorkFlowConfig))]
    [EntityWithConfig(typeof(ProcessBomWeightConfig))]
    [Label("工单")]
    [DisplayMember(nameof(No))]
    //[SIE.DataAuth.EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class WorkOrder : SIE.Core.WorkOrders.WorkOrder
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrder()
        {
            OrderQty = 0m;
            OnlineQty = 0m;
            FinishQty = 0m;
            MakeDate = DateTime.Now;
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static new readonly IRefIdProperty FactoryIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public new double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<WorkOrder>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)this.GetRefNullableId(CustomerIdProperty); }
            set { this.SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty =
            P<WorkOrder>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return this.GetRefEntity(CustomerProperty); }
            set { this.SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)this.GetRefNullableId(SupplierIdProperty); }
            set { this.SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty =
            P<WorkOrder>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return this.GetRefEntity(SupplierProperty); }
            set { this.SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 订单数量 OrderQty
        /// <summary>
        /// 订单数量
        /// </summary>
        [MinValue(0)]
        [Label("订单数量")]
        public static readonly Property<decimal> OrderQtyProperty = P<WorkOrder>.Register(e => e.OrderQty);

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal OrderQty
        {
            get { return GetProperty(OrderQtyProperty); }
            set { SetProperty(OrderQtyProperty, value); }
        }
        #endregion

        #region 工单层级 Level
        /// <summary>
        /// 工单层级
        /// </summary>
        [Label("工单层级")]
        public static readonly Property<int> LevelProperty = P<WorkOrder>.Register(e => e.Level);

        /// <summary>
        /// 工单层级
        /// </summary>
        public int Level
        {
            get { return GetProperty(LevelProperty); }
            set { SetProperty(LevelProperty, value); }
        }
        #endregion

        #region 上级工单 ParentId
        /// <summary>
        /// 上级工单
        /// </summary>
        [Label("上级工单")]
        public static readonly Property<double?> ParentIdProperty = P<WorkOrder>.Register(e => e.ParentId);

        /// <summary>
        /// 上级工单
        /// </summary>
        public double? ParentId
        {
            get { return GetProperty(ParentIdProperty); }
            set { SetProperty(ParentIdProperty, value); }
        }
        #endregion

        #region 工艺路线 Routing
        /// <summary>
        /// 工艺路线Id
        /// </summary>
        [Label("工艺路线")]
        public static readonly IRefIdProperty RoutingIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.RoutingId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double? RoutingId
        {
            get { return (double?)this.GetRefNullableId(RoutingIdProperty); }
            set { this.SetRefNullableId(RoutingIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线
        /// </summary>
        [Label("工艺路线")]
        public static readonly RefEntityProperty<Routing> RoutingProperty =
            P<WorkOrder>.RegisterRef(e => e.Routing, RoutingIdProperty);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public Routing Routing
        {
            get { return this.GetRefEntity(RoutingProperty); }
            set { this.SetRefEntity(RoutingProperty, value); }
        }
        #endregion

        #region ERP订单号 ErpOrderNo
        /// <summary>
        /// ERP订单号
        /// </summary>
        [Label("ERP订单号")]
        public static readonly Property<string> ErpOrderNoProperty = P<WorkOrder>.Register(e => e.ErpOrderNo);

        /// <summary>
        /// ERP订单号
        /// </summary>
        public string ErpOrderNo
        {
            get { return GetProperty(ErpOrderNoProperty); }
            set { SetProperty(ErpOrderNoProperty, value); }
        }
        #endregion

        #region ERP工单号 ErpWorkOrderNo
        /// <summary>
        /// ERP工单号
        /// </summary>
        [Label("ERP工单号")]
        public static readonly Property<string> ErpWorkOrderNoProperty = P<WorkOrder>.Register(e => e.ErpWorkOrderNo);

        /// <summary>
        /// ERP工单号
        /// </summary>
        public string ErpWorkOrderNo
        {
            get { return GetProperty(ErpWorkOrderNoProperty); }
            set { SetProperty(ErpWorkOrderNoProperty, value); }
        }
        #endregion

        #region 制单时间 MakeDate
        /// <summary>
        /// 制单时间
        /// </summary>
        [Label("制单时间")]
        public static readonly Property<DateTime> MakeDateProperty = P<WorkOrder>.Register(e => e.MakeDate);

        /// <summary>
        /// 制单时间
        /// </summary>
        public DateTime MakeDate
        {
            get { return GetProperty(MakeDateProperty); }
            set { SetProperty(MakeDateProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static new readonly IRefIdProperty ResourceIdProperty = P<WorkOrder>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public new double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<WorkOrder>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间")]
        public static new readonly IRefIdProperty WorkShopIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间ID
        /// </summary>
        public new double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<WorkOrder>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 制单人 Maker
        /// <summary>
        /// 制单人Id
        /// </summary>
        [Label("制单人")]
        public static readonly IRefIdProperty MakerIdProperty = P<WorkOrder>.RegisterRefId(e => e.MakerId, ReferenceType.Normal);

        /// <summary>
        /// 制单人Id
        /// </summary>
        public double? MakerId
        {
            get { return (double?)GetRefNullableId(MakerIdProperty); }
            set { SetRefNullableId(MakerIdProperty, value); }
        }

        /// <summary>
        /// 制单人
        /// </summary>
        [Label("制单人")]
        public static readonly RefEntityProperty<Employee> MakerProperty = P<WorkOrder>.RegisterRef(e => e.Maker, MakerIdProperty);

        /// <summary>
        /// 制单人
        /// </summary>
        public Employee Maker
        {
            get { return GetRefEntity(MakerProperty); }
            set { SetRefEntity(MakerProperty, value); }
        }
        #endregion

        #region 制单人名称 MakerName
        /// <summary>
        /// 制单人名称
        /// </summary>
        [Label("制单人")]
        public static readonly Property<string> MakerNameProperty = P<WorkOrder>.RegisterView(e => e.MakerName, p => p.Maker.Name);

        /// <summary>
        /// 制单人名称
        /// </summary>
        public string MakerName
        {
            get { return this.GetProperty(MakerNameProperty); }
        }
        #endregion

        #region 工艺路线版本 Version
        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        [Label("工艺路线版本")]
        public static readonly IRefIdProperty VersionIdProperty = P<WorkOrder>.RegisterRefId(e => e.VersionId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        public double? VersionId
        {
            get { return (double?)GetRefNullableId(VersionIdProperty); }
            set { SetRefNullableId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        [Label("工艺路线版本")]
        public static readonly RefEntityProperty<RoutingVersion> VersionProperty = P<WorkOrder>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public RoutingVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 来源 Source
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<SourceType> SourceProperty = P<WorkOrder>.Register(e => e.Source);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType Source
        {
            get { return GetProperty(SourceProperty); }
            set { SetProperty(SourceProperty, value); }
        }
        #endregion

        #region ERP订单 ErpOrder
        /// <summary>
        /// ERP订单Id
        /// </summary>
        [Label("ERP订单")]
        public static readonly IRefIdProperty ErpSaleOrderIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.ErpSaleOrderId, ReferenceType.Normal);

        /// <summary>
        /// ERP订单Id
        /// </summary>
        public double? ErpSaleOrderId
        {
            get { return (double?)this.GetRefNullableId(ErpSaleOrderIdProperty); }
            set { this.SetRefNullableId(ErpSaleOrderIdProperty, value); }
        }

        /// <summary>
        /// ERP订单
        /// </summary>
        [Label("ERP订单")]
        public static readonly RefEntityProperty<ErpSaleOrder> ErpSaleOrderProperty =
            P<WorkOrder>.RegisterRef(e => e.ErpSaleOrder, ErpSaleOrderIdProperty);

        /// <summary>
        /// ERP订单
        /// </summary>
        public ErpSaleOrder ErpSaleOrder
        {
            get { return this.GetRefEntity(ErpSaleOrderProperty); }
            set { this.SetRefEntity(ErpSaleOrderProperty, value); }
        }
        #endregion

        #region ERP工单 ErpWorkOrder
        /// <summary>
        /// ERP工单Id
        /// </summary>
        [Label("ERP工单")]
        public static readonly IRefIdProperty ErpWorkOrderIdProperty = P<WorkOrder>.RegisterRefId(e => e.ErpWorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// ERP工单Id
        /// </summary>
        public double? ErpWorkOrderId
        {
            get { return (double?)GetRefNullableId(ErpWorkOrderIdProperty); }
            set { SetRefNullableId(ErpWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// ERP工单
        /// </summary>
        [Label("ERP工单")]
        public static readonly RefEntityProperty<ErpWorkOrder> ErpWorkOrderProperty = P<WorkOrder>.RegisterRef(e => e.ErpWorkOrder, ErpWorkOrderIdProperty);

        /// <summary>
        /// ERP工单
        /// </summary>
        public ErpWorkOrder ErpWorkOrder
        {
            get { return GetRefEntity(ErpWorkOrderProperty); }
            set { SetRefEntity(ErpWorkOrderProperty, value); }
        }
        #endregion

        #region 工单与工单BOM关系 BomList
        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        [Label("工单与工单BOM关系")]
        public static new readonly ListProperty<EntityList<WorkOrderBom>> BomListProperty = P<WorkOrder>.RegisterList(e => e.BomList);

        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        public new EntityList<WorkOrderBom> BomList
        {
            get { return this.GetLazyList(BomListProperty); }
        }
        #endregion

        #region 工单与日志关系 WorkOrderLogList
        /// <summary>
        /// 工单与日志关系
        /// </summary>
        [Label("工单与日志关系")]
        public static readonly ListProperty<EntityList<WorkOrderLog>> WorkOrderLogListProperty = P<WorkOrder>.RegisterList(e => e.WorkOrderLogList);

        /// <summary>
        /// 工单与日志关系
        /// </summary>
        public EntityList<WorkOrderLog> WorkOrderLogList
        {
            get { return this.GetLazyList(WorkOrderLogListProperty); }
        }
        #endregion

        #region 工单与工序清单关系 RoutingProcessList
        /// <summary>
        /// 工单与工序清单关系
        /// </summary>
        [Label("工单与工序清单关系")]
        public static readonly ListProperty<EntityList<WorkOrderRoutingProcess>> RoutingProcessListProperty = P<WorkOrder>.RegisterList(e => e.RoutingProcessList);

        /// <summary>
        /// 工单与工序清单关系
        /// </summary>
        public EntityList<WorkOrderRoutingProcess> RoutingProcessList
        {
            get { return this.GetLazyList(RoutingProcessListProperty); }
        }
        #endregion

        #region 工单与工序BOM关系 ProcessBomList
        /// <summary>
        /// 工单与工序BOM关系
        /// </summary>
        [Label("工单与工序BOM关系")]
        public static readonly ListProperty<EntityList<WorkOrderProcessBom>> ProcessBomListProperty = P<WorkOrder>.RegisterList(e => e.ProcessBomList);

        /// <summary>
        /// 工单与工序BOM关系
        /// </summary>
        public EntityList<WorkOrderProcessBom> ProcessBomList
        {
            get { return this.GetLazyList(ProcessBomListProperty); }
        }
        #endregion

        #region 联/副产品 WorkOrderOutputList
        /// <summary>
        /// 联/副产品
        /// </summary>
        [Label("联/副产品")]
        public static readonly ListProperty<EntityList<WorkOrderOutputProduct>> WorkOrderOutputProductProperty = P<WorkOrder>.RegisterList(e => e.WorkOrderOutputProductList);

        /// <summary>
        /// 联/副产品
        /// </summary>
        public EntityList<WorkOrderOutputProduct> WorkOrderOutputProductList
        {
            get { return this.GetLazyList(WorkOrderOutputProductProperty); }
        }
        #endregion

        #region 工单与包装规则关系 PackageRuleDetailList
        /// <summary>
        /// 工单与包装规则关系
        /// </summary>
        public static readonly ListProperty<EntityList<WorkOrderPackageRuleDetail>> PackageRuleDetailListProperty = P<WorkOrder>.RegisterList(e => e.PackageRuleDetailList);

        /// <summary>
        /// 工单与包装规则关系
        /// </summary>
        public EntityList<WorkOrderPackageRuleDetail> PackageRuleDetailList
        {
            get { return this.GetLazyList(PackageRuleDetailListProperty); }
        }
        #endregion

        #region 工单与工艺路线布局关系 Layout
        /// <summary>
        /// 工单与工艺路线布局关系Id
        /// </summary>
        public static readonly IRefIdProperty LayoutIdProperty = P<WorkOrder>.RegisterRefId(e => e.LayoutId, ReferenceType.Normal);

        /// <summary>
        /// 工单与工艺路线布局关系Id
        /// </summary>
        public double? LayoutId
        {
            get { return (double?)GetRefNullableId(LayoutIdProperty); }
            set { SetRefNullableId(LayoutIdProperty, value); }
        }

        /// <summary>
        /// 工单与工艺路线布局关系
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderRoutingLayout> LayoutProperty = P<WorkOrder>.RegisterRef(e => e.Layout, LayoutIdProperty);

        /// <summary>
        /// 工单与工艺路线布局关系
        /// </summary>
        public WorkOrderRoutingLayout Layout
        {
            get { return GetRefEntity(LayoutProperty); }
            set { SetRefEntity(LayoutProperty, value); }
        }
        #endregion

        #region 工单与工艺路线信息 LayoutInfoList
        /// <summary>
        /// 工单与工艺路线信息
        /// </summary>
        [Label("工单与工艺路线信息")]
        public static readonly ListProperty<EntityList<LayoutInfo>> LayoutInfoListProperty = P<WorkOrder>.RegisterList(e => e.LayoutInfoList);

        /// <summary>
        /// 工单与工艺路线信息
        /// </summary>
        public EntityList<LayoutInfo> LayoutInfoList
        {
            get { return this.GetLazyList(LayoutInfoListProperty); }
        }
        #endregion

        #region 产品 Product //重写子类产品
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static new readonly IRefIdProperty ProductIdProperty = P<WorkOrder>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public new double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品编码")]
        public static new readonly RefEntityProperty<Item> ProductProperty = P<WorkOrder>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public new Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 上线数量 OnlineQty
        /// <summary>
        /// 上线数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("上线数量")]
        public static readonly Property<decimal> OnlineQtyProperty = P<WorkOrder>.Register(e => e.OnlineQty);

        /// <summary>
        /// 上线数量
        /// </summary>
        public decimal OnlineQty
        {
            get { return this.GetProperty(OnlineQtyProperty); }
            set { this.SetProperty(OnlineQtyProperty, value); }
        }
        #endregion        

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<WorkOrder>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 入库数量 StorageQty
        /// <summary>
        /// 入库数量
        /// </summary>
        [Label("入库数量")]
        public static readonly Property<decimal?> StorageQtyProperty = P<WorkOrder>.Register(e => e.StorageQty);

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal? StorageQty
        {
            get { return this.GetProperty(StorageQtyProperty); }
            set { this.SetProperty(StorageQtyProperty, value); }
        }
        #endregion

        #region 齐套状态 KitType
        /// <summary>
        /// 齐套状态
        /// </summary>
        [Label("齐套状态")]
        public static readonly Property<KitType?> KitTypeProperty = P<WorkOrder>.Register(e => e.KitType);

        /// <summary>
        /// 齐套状态
        /// </summary>
        public KitType? KitType
        {
            get { return GetProperty(KitTypeProperty); }
            set { SetProperty(KitTypeProperty, value); }
        }
        #endregion 

        #region APS工艺单编号 ProcessTechOrderCode
        /// <summary>
        /// APS工艺单编号
        /// </summary>
        [Label("工艺单编号")]
        [MaxLength(2000)]
        public static readonly Property<string> ProcessTechOrderCodeProperty = P<WorkOrder>.Register(e => e.ProcessTechOrderCode);

        /// <summary>
        /// APS工艺单编号
        /// </summary>
        public string ProcessTechOrderCode
        {
            get { return GetProperty(ProcessTechOrderCodeProperty); }
            set { SetProperty(ProcessTechOrderCodeProperty, value); }
        }
        #endregion

        #region APS前工艺单编号，逗号分隔 BeforeTechOrderCode
        /// <summary>
        /// APS前工艺单编号
        /// </summary>
        [Label("APS前工艺单编号")]
        [MaxLength(2000)]
        public static readonly Property<string> BeforeTechOrderCodeProperty = P<WorkOrder>.Register(e => e.BeforeTechOrderCode);

        /// <summary>
        /// APS前工艺单编号
        /// </summary>
        public string BeforeTechOrderCode
        {
            get { return GetProperty(BeforeTechOrderCodeProperty); }
            set { SetProperty(BeforeTechOrderCodeProperty, value); }
        }
        #endregion

        #region 是否共模 IsCommonMode
        /// <summary>
        /// 是否共模
        /// </summary>
        [Label("是否共模")]
        public static readonly Property<bool> IsCommonModeProperty = P<WorkOrder>.Register(e => e.IsCommonMode);

        /// <summary>
        /// 是否共模
        /// </summary>
        public bool IsCommonMode
        {
            get { return this.GetProperty(IsCommonModeProperty); }
            set { this.SetProperty(IsCommonModeProperty, value); }
        }
        #endregion

        #region 是否主料 IsMainMaterial
        /// <summary>
        /// 是否主料
        /// </summary>
        [Label("是否主料")]
        public static readonly Property<bool> IsMainMaterialProperty = P<WorkOrder>.Register(e => e.IsMainMaterial);

        /// <summary>
        /// 是否主料
        /// </summary>
        public bool IsMainMaterial
        {
            get { return this.GetProperty(IsMainMaterialProperty); }
            set { this.SetProperty(IsMainMaterialProperty, value); }
        }
        #endregion

        #region 计划单号 PlanNo
        /// <summary>
        /// 计划单号
        /// </summary>
        [Label("计划单号")]
        [MaxLength(2000)]
        public static readonly Property<string> PlanNoProperty = P<WorkOrder>.Register(e => e.PlanNo);

        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanNo
        {
            get { return this.GetProperty(PlanNoProperty); }
            set { this.SetProperty(PlanNoProperty, value); }
        }
        #endregion

        #region 共模模具数 Proportion
        /// <summary>   
        /// 模具数
        /// </summary>
        [Label("模具数")]
        public static readonly Property<double> ProportionProperty = P<WorkOrder>.Register(e => e.Proportion);

        /// <summary>
        /// 模具数
        /// </summary>
        public double Proportion
        {
            get { return this.GetProperty(ProportionProperty); }
            set { this.SetProperty(ProportionProperty, value); }
        }
        #endregion

        #region 制程工艺 ProcessTech
        /// <summary>
        /// 制程工艺Id
        /// </summary>
        [Label("制程工艺")]
        public static readonly IRefIdProperty ProcessTechIdProperty = P<WorkOrder>.RegisterRefId(e => e.ProcessTechId, ReferenceType.Normal);

        /// <summary>
        /// 制程工艺Id
        /// </summary>
        public double? ProcessTechId
        {
            get { return (double?)GetRefNullableId(ProcessTechIdProperty); }
            set { SetRefNullableId(ProcessTechIdProperty, value); }
        }

        /// <summary>
        /// 制程工艺
        /// </summary>
        public static readonly RefEntityProperty<ProcessTech> ProcessTechProperty = P<WorkOrder>.RegisterRef(e => e.ProcessTech, ProcessTechIdProperty);

        /// <summary>
        /// 制程工艺
        /// </summary>
        public ProcessTech ProcessTech
        {
            get { return GetRefEntity(ProcessTechProperty); }
            set { SetRefEntity(ProcessTechProperty, value); }
        }
        #endregion

        #region 工段 ProcessSegment
        /// <summary>
        /// 工段Id
        /// </summary>
        [Label("工段")]
        public static readonly IRefIdProperty ProcessSegmentIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return (double?)this.GetRefNullableId(ProcessSegmentIdProperty); }
            set { this.SetRefNullableId(ProcessSegmentIdProperty, value); }
        }

        /// <summary>
        /// 工段
        /// </summary>
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty =
            P<WorkOrder>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment ProcessSegment
        {
            get { return this.GetRefEntity(ProcessSegmentProperty); }
            set { this.SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 产前准备状态 PrepareState
        /// <summary>
        /// 产前准备状态
        /// </summary>
        [Label("产前准备状态")]
        public static readonly Property<PrepareRecordState> PrepareStateProperty = P<WorkOrder>.Register(e => e.PrepareState);

        /// <summary>
        /// 产前准备状态
        /// </summary>
        public PrepareRecordState PrepareState
        {
            get { return this.GetProperty(PrepareStateProperty); }
            set { this.SetProperty(PrepareStateProperty, value); }
        }
        #endregion

        #region BS注册视图  
        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<WorkOrder>.RegisterView(e => e.CustomerCode, p => p.Customer.Code);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<WorkOrder>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 产品机型编码 ProductModelCode
        /// <summary>
        /// 产品机型编码
        /// </summary>
        [Label("产品机型编码")]
        public static readonly Property<string> ProductModelCodeProperty = P<WorkOrder>.RegisterView(e => e.ProductModelCode, e => e.Product.Model.Code);

        /// <summary>
        /// 产品机型编码
        /// </summary>
        public string ProductModelCode
        {
            get { return GetProperty(ProductModelCodeProperty); }
        }
        #endregion

        #region 产品机型 ProductModelName
        /// <summary>
        /// 产品机型
        /// </summary>
        [Label("产品机型")]
        public static readonly Property<string> ProductModelNameProperty = P<WorkOrder>.RegisterView(e => e.ProductModelName, e => e.Product.Model.Name);

        /// <summary>
        /// 产品机型
        /// </summary>
        public string ProductModelName
        {
            get { return GetProperty(ProductModelNameProperty); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<WorkOrder>.RegisterView(e => e.WorkShopCode, e => e.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return GetProperty(WorkShopCodeProperty); }
        }
        #endregion     

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<WorkOrder>.RegisterView(e => e.WorkShopName, e => e.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<WorkOrder>.RegisterView(e => e.ResourceCode, e => e.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return GetProperty(ResourceCodeProperty); }
        }
        #endregion   

        #region 资源 ResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WorkOrder>.RegisterView(e => e.ResourceName, e => e.Resource.Name);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<WorkOrder>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 旧料号 ProductShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ProductShortDescriptionProperty = P<WorkOrder>.RegisterView(e => e.ProductShortDescription, e => e.Product.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ProductShortDescription
        {
            get { return GetProperty(ProductShortDescriptionProperty); }
        }
        #endregion

        #region 产品类型 ProductType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Label("产品类型")]
        public static readonly Property<ItemType> ProductTypeProperty = P<WorkOrder>.RegisterView(e => e.ProductType, e => e.Product.Type);

        /// <summary>
        /// 产品类型
        /// </summary>
        public ItemType ProductType
        {
            get { return GetProperty(ProductTypeProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WorkOrder>.RegisterView(e => e.ProductCode, e => e.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WorkOrder>.RegisterView(e => e.ProductName, e => e.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 物料类型 ProductMtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> ProductMtartProperty = P<WorkOrder>.RegisterView(e => e.ProductMtart, p => p.Product.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string ProductMtart
        {
            get { return this.GetProperty(ProductMtartProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<WorkOrder>.RegisterView(e => e.SupplierName, e => e.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 工艺路线版本 VersionName
        /// <summary>
        /// 工艺路线版本
        /// </summary>
        [Label("工艺路线版本")]
        public static readonly Property<string> VersionNameProperty = P<WorkOrder>.RegisterView(e => e.VersionName, p => p.Version.Name);

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string VersionName
        {
            get { return this.GetProperty(VersionNameProperty); }
        }
        #endregion

        #region 制程段编码 ProcessTechCode
        /// <summary>
        /// 制程段编码
        /// </summary>
        [Label("制程段编码")]
        public static readonly Property<string> ProcessTechCodeProperty = P<WorkOrder>.RegisterView(e => e.ProcessTechCode, e => e.ProcessTech.Code);

        /// <summary>
        /// 制程段编码
        /// </summary>
        public string ProcessTechCode
        {
            get { return GetProperty(ProcessTechCodeProperty); }
        }
        #endregion

        #region 工段编码 ProcessSegmentCode
        /// <summary>
        /// 工段编码
        /// </summary>
        [Label("工段编码")]
        public static readonly Property<string> ProcessSegmentCodeProperty = P<WorkOrder>.RegisterView(e => e.ProcessSegmentCode, e => e.ProcessSegment.Code);

        /// <summary>
        /// 工段编码
        /// </summary>
        public string ProcessSegmentCode
        {
            get { return GetProperty(ProcessSegmentCodeProperty); }
        }
        #endregion

        #region 工段 ProcessSegmentName
        /// <summary>
        /// 工段
        /// </summary>
        [Label("工段")]
        public static readonly Property<string> ProcessSegmentNameProperty = P<WorkOrder>.RegisterView(e => e.ProcessSegmentName, e => e.ProcessSegment.Name);

        /// <summary>
        /// 工段
        /// </summary>
        public string ProcessSegmentName
        {
            get { return GetProperty(ProcessSegmentNameProperty); }
        }
        #endregion

        #region 产品族Id ProductFamilyId
        /// <summary>
        /// 产品族Id
        /// </summary>
        [Label("产品族Id")]
        public static readonly Property<double?> ProductFamilyIdProperty = P<WorkOrder>.RegisterView(e => e.ProductFamilyId, p => p.Product.ProductFamilyId);

        /// <summary>
        /// 产品族Id
        /// </summary>
        public double? ProductFamilyId
        {
            get { return this.GetProperty(ProductFamilyIdProperty); }
        }
        #endregion

        #region 项目编码 ProjectMaintainCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectMaintainCodeProperty = P<WorkOrder>.RegisterView(e => e.ProjectMaintainCode, p => p.ProjectMaintain.Code);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectMaintainCode
        {
            get { return this.GetProperty(ProjectMaintainCodeProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库
        #region 是否重新生成任务单 IsReGenerateTask
        /// <summary>
        /// 是否重新生成任务单
        /// </summary>
        [Label("是否重新生成任务单")]
        public static readonly Property<bool> IsReGenerateTaskProperty = P<WorkOrder>.Register(e => e.IsReGenerateTask);

        /// <summary>
        /// 是否重新生成任务单
        /// </summary>
        public bool IsReGenerateTask
        {
            get { return this.GetProperty(IsReGenerateTaskProperty); }
            set { this.SetProperty(IsReGenerateTaskProperty, value); }
        }
        #endregion

        public string DAUAT { get; set; }
        #endregion

        #region 已生成数量 GeneratedQty
        /// <summary>
        /// 已生成数量
        /// </summary>
        [Label("已生成数量")]
        public static readonly Property<decimal?> GeneratedQtyProperty = P<WorkOrder>.Register(e => e.GeneratedQty);

        /// <summary>
        /// 已生成数量
        /// </summary>
        public decimal? GeneratedQty
        {
            get { return this.GetProperty(GeneratedQtyProperty); }
            set { this.SetProperty(GeneratedQtyProperty, value); }
        }
        #endregion

        #region 交货容差 Uebto
        /// <summary>
        /// 交货容差
        /// </summary>
        [Label("交货容差")]
        public static readonly Property<string> UebtoProperty = P<WorkOrder>.Register(e => e.Uebto);

        /// <summary>
        /// 交货容差
        /// </summary>
        public string Uebto
        {
            get { return this.GetProperty(UebtoProperty); }
            set { this.SetProperty(UebtoProperty, value); }
        }
        #endregion

        #region 生产管理者 Fevor
        /// <summary>
        /// 生产管理者
        /// </summary>
        [Label("生产管理者")]
        public static readonly Property<string> FevorProperty = P<WorkOrder>.Register(e => e.Fevor);

        /// <summary>
        /// 生产管理者
        /// </summary>
        public string Fevor
        {
            get { return this.GetProperty(FevorProperty); }
            set { this.SetProperty(FevorProperty, value); }
        }
        #endregion

        #region 库存地 Lgort
        /// <summary>
        /// 库存地
        /// </summary>
        [Label("库存地")]
        public static readonly Property<string> LgortProperty = P<WorkOrder>.Register(e => e.Lgort);

        /// <summary>
        /// 库存地
        /// </summary>
        public string Lgort
        {
            get { return this.GetProperty(LgortProperty); }
            set { this.SetProperty(LgortProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<WorkOrder>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 内部订单 OrderNo
        /// <summary>
        /// 内部订单
        /// </summary>
        [Label("内部订单")]
        public static readonly Property<string> OrderNoProperty = P<WorkOrder>.Register(e => e.OrderNo);

        /// <summary>
        /// 内部订单
        /// </summary>
        public string OrderNo
        {
            get { return this.GetProperty(OrderNoProperty); }
            set { this.SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 制卡数量 Ztfl
        /// <summary>
        /// 制卡数量
        /// </summary>
        [Label("制卡数量")]
        public static readonly Property<decimal?> ZtflProperty = P<WorkOrder>.Register(e => e.Ztfl);

        /// <summary>
        /// 制卡数量
        /// </summary>
        public decimal? Ztfl
        {
            get { return this.GetProperty(ZtflProperty); }
            set { this.SetProperty(ZtflProperty, value); }
        }
        #endregion

    }

    #region 工单 实体配置
    /// <summary>
    /// 工单 实体配置
    /// </summary>
    internal class WorkOrderConfig : EntityConfig<WorkOrder>
    {
        //protected override void AddValidations(IValidationDeclarer rules)
        //{
        //    rules.AddRule(WorkOrder.NoProperty, new NotDuplicateRule());
        //    base.AddValidations(rules);
        //}

        ///// <summary>
        ///// 重写实体验证方法，增加实体验证规则
        ///// </summary>
        ///// <param name="rules">规则集合</param>
        //protected override void AddValidations(IValidationDeclarer rules)
        //{
        //    rules.AddRule(WorkOrder.WorkShopIdProperty, new RequiredRule()
        //    {
        //        MessageBuilder = e =>
        //        {
        //            return "车间不能为空";
        //        }
        //    });

        //    rules.AddRule(WorkOrder.ResourceIdProperty, new RequiredRule()
        //    {
        //        MessageBuilder = e =>
        //        {
        //            return "资源不能为空";
        //        }
        //    });

        //    base.AddValidations(rules);
        //}

        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(WorkOrder.IsReGenerateTaskProperty).DontMapColumn();
            Meta.Property(WorkOrder.PlanNoProperty).ColumnMeta.HasLength(2000);
            Meta.Property(WorkOrder.ProcessTechOrderCodeProperty).ColumnMeta.HasLength(2000);
            Meta.Property(WorkOrder.BeforeTechOrderCodeProperty).ColumnMeta.HasLength(2000);
            Meta.Property(WorkOrder.ErpWorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.IndexGroupOnProperties(WorkOrder.ProductIdProperty, WorkOrder.ResourceIdProperty);
            Meta.IndexGroupOnProperties(WorkOrder.ResourceIdProperty, WorkOrder.PlanBeginDateProperty);
            Meta.IndexGroupOnProperties(WorkOrder.WorkShopIdProperty, WorkOrder.PlanBeginDateProperty);
            Meta.IndexGroupOnProperties(WorkOrder.ProductionOrderCodeProperty, WorkOrder.PlanBeginDateProperty);
        }
    }
    #endregion

    /// <summary>
    /// 工单打印设置扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class WOLabelPrintDetailProperty
    {
        /// <summary>
        /// 扩展打印设置属性
        /// </summary>
        public static readonly Property<Core.Items.LabelPrintTemplate> LabelPrintTemProperty =
            P<WorkOrder>.RegisterExtension<Core.Items.LabelPrintTemplate>("Template", typeof(WOLabelPrintDetailProperty));

        /// <summary>
        /// 获取打印设置对象
        /// </summary>
        /// <param name="me">工单对象</param>
        /// <returns>返回打印设置对象</returns>
        public static Core.Items.LabelPrintTemplate GetLabelPrintTem(WorkOrder me)
        {
            return me.GetProperty(LabelPrintTemProperty);
        }

        /// <summary>
        /// 设置打印设置对象
        /// </summary>
        /// <param name="me">工单对象</param>
        /// <param name="value">需要设置的打印设置对象</param>
        public static void SetLabelPrintTem(WorkOrder me, Core.Items.LabelPrintTemplate value)
        {
            me.SetProperty(LabelPrintTemProperty, value);
        }

        /// <summary>
        /// 工单打印设置 实体配置
        /// </summary>
        internal class WorkOrderLabelPrintDetailPropertyConfig : EntityConfig<WorkOrder>
        {

            protected override void AddValidations(IValidationDeclarer rules)
            {
                rules.AddRule(WorkOrder.NoProperty, new NotDuplicateRule());
                base.AddValidations(rules);
            }

            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(WOLabelPrintDetailProperty.LabelPrintTemProperty).DontMapColumn();
            }
        }
    }
}