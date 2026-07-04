using SIE.Core.ProjectMaintains;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 过站工单
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单")]
    public partial class WorkOrderMove : Entity<double>
    {
        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<WorkOrderMove>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WorkOrderMove>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 已打印数量 PrintedQty
        /// <summary>
        /// 已打印数量
        /// </summary>
        [Label("已打印数量")]
        public static readonly Property<int> PrintedQtyProperty = P<WorkOrderMove>.Register(e => e.PrintedQty);

        /// <summary>
        /// 已打印数量
        /// </summary>
        public int PrintedQty
        {
            get { return this.GetProperty(PrintedQtyProperty); }
            set { this.SetProperty(PrintedQtyProperty, value); }
        }
        #endregion 

        #region 产品 Product 
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<WorkOrderMove>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品编码")]
        public static readonly RefEntityProperty<Item> ProductProperty = P<WorkOrderMove>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 是否暂停 IsPause
        /// <summary>
        /// 是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo> IsPauseProperty = P<WorkOrderMove>.Register(e => e.IsPause);

        /// <summary>
        /// 是否暂停
        /// </summary>
        public YesNo IsPause
        {
            get { return GetProperty(IsPauseProperty); }
            set { SetProperty(IsPauseProperty, value); }
        }
        #endregion

        #region 上线数量 OnlineQty
        /// <summary>
        /// 上线数量
        /// </summary>
        [Label("上线数量")]
        public static readonly Property<decimal> OnlineQtyProperty = P<WorkOrderMove>.Register(e => e.OnlineQty);

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
        public static readonly Property<decimal> ScrapQtyProperty = P<WorkOrderMove>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> StateProperty = P<WorkOrderMove>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工单类型 WorkOrderType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> WorkOrderTypeProperty = P<WorkOrderMove>.Register(e => e.WorkOrderType);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType WorkOrderType
        {
            get { return GetProperty(WorkOrderTypeProperty); }
            set { SetProperty(WorkOrderTypeProperty, value); }
        }
        #endregion

        #region APS工艺单编号 ProcessTechOrderCode
        /// <summary>
        /// APS工艺单编号
        /// </summary>
        [Label("工艺单编号")]
        public static readonly Property<string> ProcessTechOrderCodeProperty = P<WorkOrderMove>.Register(e => e.ProcessTechOrderCode);

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
        public static readonly Property<string> BeforeTechOrderCodeProperty = P<WorkOrderMove>.Register(e => e.BeforeTechOrderCode);

        /// <summary>
        /// APS前工艺单编号
        /// </summary>
        public string BeforeTechOrderCode
        {
            get { return GetProperty(BeforeTechOrderCodeProperty); }
            set { SetProperty(BeforeTechOrderCodeProperty, value); }
        }
        #endregion

        #region 生产订单号 ProductionOrderCode
        /// <summary>
        /// 生产订单号
        /// </summary>
        [Label("生产订单号")]
        public static readonly Property<string> ProductionOrderCodeProperty = P<WorkOrderMove>.Register(e => e.ProductionOrderCode);

        /// <summary>
        /// 生产订单号
        /// </summary>
        public string ProductionOrderCode
        {
            get { return GetProperty(ProductionOrderCodeProperty); }
            set { SetProperty(ProductionOrderCodeProperty, value); }
        }
        #endregion

        #region 是否组合板工单 IsPanelWorkOrder
        /// <summary>
        /// 是否组合板工单
        /// </summary>
        [Label("是否组合板工单")]
        public static readonly Property<bool> IsPanelWorkOrderProperty = P<WorkOrderMove>.Register(e => e.IsPanelWorkOrder);

        /// <summary>
        /// 是否组合板工单
        /// </summary>
        public bool IsPanelWorkOrder
        {
            get { return GetProperty(IsPanelWorkOrderProperty); }
            set { SetProperty(IsPanelWorkOrderProperty, value); }
        }
        #endregion

        #region 组合板工单 PanelWorkOrder
        /// <summary>
        /// 组合板工单Id
        /// </summary>
        [Label("组合板工单")]
        public static readonly IRefIdProperty PanelWorkOrderIdProperty = P<WorkOrderMove>.RegisterRefId(e => e.PanelWorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 组合板工单Id
        /// </summary>
        public double? PanelWorkOrderId
        {
            get { return (double?)GetRefNullableId(PanelWorkOrderIdProperty); }
            set { SetRefNullableId(PanelWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 组合板工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderMove> PanelWorkOrderProperty = P<WorkOrderMove>.RegisterRef(e => e.PanelWorkOrder, PanelWorkOrderIdProperty);

        /// <summary>
        /// 组合板工单
        /// </summary>
        public WorkOrderMove PanelWorkOrder
        {
            get { return GetRefEntity(PanelWorkOrderProperty); }
            set { SetRefEntity(PanelWorkOrderProperty, value); }
        }
        #endregion

        #region 拼板数 PanelQty
        /// <summary>
        /// 拼板数
        /// </summary>
        [Label("拼板数")]
        [MinValue(0)]
        public static readonly Property<int> PanelQtyProperty = P<WorkOrderMove>.Register(e => e.PanelQty);

        /// <summary>
        /// 拼板数
        /// </summary>
        public int PanelQty
        {
            get { return this.GetProperty(PanelQtyProperty); }
            set { this.SetProperty(PanelQtyProperty, value); }
        }
        #endregion 

        #region 车间 WorkShop
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<WorkOrderMove>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<WorkOrderMove>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<WorkOrderMove>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<WorkOrderMove>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<WorkOrderMove>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WorkOrderMove>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtPropName
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrderMove>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<WorkOrderMove>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<WorkOrderMove>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<WorkOrderMove>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectMaintainId
        {
            get { return (double?)this.GetRefNullableId(ProjectMaintainIdProperty); }
            set { this.SetRefNullableId(ProjectMaintainIdProperty, value); }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        public static readonly RefEntityProperty<ProjectMaintain> ProjectMaintainProperty =
            P<WorkOrderMove>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

        /// <summary>
        /// 项目号
        /// </summary>
        public ProjectMaintain ProjectMaintain
        {
            get { return this.GetRefEntity(ProjectMaintainProperty); }
            set { this.SetRefEntity(ProjectMaintainProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 过站工单 实体配置
    /// </summary>
    internal class WorkOrderMoveConfig : EntityConfig<WorkOrderMove>
    {
        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<WorkOrder>().Select((a) => new
            {
                a.Id,
                a.No,
                PLAN_QTY = a.PlanQty,
                PRINTED_QTY = a.PrintedQty,
                PRODUCT_ID = a.ProductId,
                IS_PAUSE = a.IsPause,
                ONLINE_QTY = a.OnlineQty,
                SCRAP_QTY = a.ScrapQty,
                a.State,
                WORK_ORDER_TYPE = a.Type,
                PROCESS_TECH_ORDER_CODE = a.ProcessTechOrderCode,
                BEFORE_TECH_ORDER_CODE = a.BeforeTechOrderCode,
                PRODUCTION_ORDER_CODE = a.ProductionOrderCode,
                IS_PANEL_WORK_ORDER = a.IsPanelWorkOrder,
                PANEL_WORK_ORDER_ID = a.PanelWorkOrderId,
                PANEL_QTY = a.PanelQty,
                WORK_SHOP_ID = a.WorkShopId,
                RESOURCE_Id = a.ResourceId,
                FINISH_QTY = a.FinishQty,
                ITEM_EXT_PROP = a.ItemExtProp,
                ITEM_EXT_PROP_NAME = a.ItemExtPropName,
                PROJECT_MAINTAIN_ID = a.ProjectMaintainId,
                FACTORY_ID = a.FactoryId,
            }).ToQuery();
            Meta.MapView(view).MapAllProperties();
        }
    }
}
