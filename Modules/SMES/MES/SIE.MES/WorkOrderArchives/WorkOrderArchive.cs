using SIE.Core.Items;
using SIE.MES.WorkOrders;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Text;
using Item = SIE.Items.Item;
using SIE.Core.WorkOrders;
using WorkOrder = SIE.MES.WorkOrders.WorkOrder;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案实体
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkOrderArchiveCriteria))]
    [Label("工单制造档案")]
    public class WorkOrderArchive : Entity<Double>
    {
        #region 工单编号 No
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> NoProperty = P<WorkOrderArchive>.Register(e => e.No);

        /// <summary>
        /// 工单编号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<WorkOrderArchive>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)this.GetRefId(ProductIdProperty); }
            set { this.SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<WorkOrderArchive>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工单状态 WoState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> WoStateProperty = P<WorkOrderArchive>.Register(e => e.WoState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState WoState
        {
            get { return this.GetProperty(WoStateProperty); }
            set { this.SetProperty(WoStateProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WorkOrderArchive>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<WorkOrderArchive>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<WorkOrderArchive>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 工单类型 WoType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> WoTypeProperty = P<WorkOrderArchive>.Register(e => e.WoType);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType WoType
        {
            get { return this.GetProperty(WoTypeProperty); }
            set { this.SetProperty(WoTypeProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<WorkOrderArchive>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
            set { this.SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndDate
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<WorkOrderArchive>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return this.GetProperty(PlanEndDateProperty); }
            set { this.SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 实际开始时间 ActuStartDate
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime?> ActuStartDateProperty = P<WorkOrderArchive>.Register(e => e.ActuStartDate);

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActuStartDate
        {
            get { return this.GetProperty(ActuStartDateProperty); }
            set { this.SetProperty(ActuStartDateProperty, value); }
        }
        #endregion

        #region 实际完成时间 ActuFinishDate
        /// <summary>
        /// 实际完成时间
        /// </summary>
        [Label("实际完成时间")]
        public static readonly Property<DateTime?> ActuFinishDateProperty = P<WorkOrderArchive>.Register(e => e.ActuFinishDate);

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActuFinishDate
        {
            get { return this.GetProperty(ActuFinishDateProperty); }
            set { this.SetProperty(ActuFinishDateProperty, value); }
        }
        #endregion

        #region 物料拓展属性 ItemExtProp
        /// <summary>
        /// 物料拓展属性
        /// </summary>
        [Label("物料拓展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WorkOrderArchive>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料拓展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料拓展属性值 ItemExtPropName
        /// <summary>
        /// 物料拓展属性值
        /// </summary>
        [Label("物料拓展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrderArchive>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料拓展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 追溯方式 RetrospectType
        /// <summary>
        /// 追溯方式
        /// </summary>
        [Label("追溯方式")]
        public static readonly Property<RetrospectType> RetrospectTypeProperty = P<WorkOrderArchive>.Register(e => e.RetrospectType);

        /// <summary>
        /// 追溯方式
        /// </summary>
        public RetrospectType RetrospectType
        {
            get { return this.GetProperty(RetrospectTypeProperty); }
            set { this.SetProperty(RetrospectTypeProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<WorkOrderArchive>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<WorkOrderArchive>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<WorkOrderArchive>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<WorkOrderArchive>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

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
        public static readonly IRefIdProperty ResourceIdProperty =
            P<WorkOrderArchive>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<WorkOrderArchive>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工艺路线版本 Version
        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        [Label("工艺路线版本")]
        public static readonly IRefIdProperty VersionIdProperty =
            P<WorkOrderArchive>.RegisterRefId(e => e.VersionId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        public double? VersionId
        {
            get { return (double?)this.GetRefNullableId(VersionIdProperty); }
            set { this.SetRefNullableId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public static readonly RefEntityProperty<RoutingVersion> VersionProperty =
            P<WorkOrderArchive>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public RoutingVersion Version
        {
            get { return this.GetRefEntity(VersionProperty); }
            set { this.SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 产品编码 ProCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProCodeProperty = P<WorkOrderArchive>.RegisterView(e => e.ProCode, p => p.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProCode
        {
            get { return this.GetProperty(ProCodeProperty); }
        }
        #endregion

        #region 产品名称 ProName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProNameProperty = P<WorkOrderArchive>.RegisterView(e => e.ProName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class WorkOrderArchiveConfig : EntityConfig<WorkOrderArchive>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<WorkOrder>("wo")
            .Join<ItemBatchRule>("ibr", (wo, ibr) => wo.ProductId == ibr.ItemId && ibr.SQL<int>("ibr.IS_PHANTOM") == 0 && ibr.SQL<int?>("ibr.INV_ORG_ID") == RT.InvOrg)
            .Select<ItemBatchRule>((wo, ibr) => new
            {
                wo.Id,
                NO = wo.No,
                PRODUCT_ID = wo.ProductId,
                WO_STATE = wo.State,
                PLAN_QTY = wo.PlanQty,
                FINISH_QTY = wo.FinishQty,
                SCRAP_QTY = wo.ScrapQty,
                WO_TYPE = wo.Type,
                PLAN_BEGIN_DATE = wo.PlanBeginDate,
                PLAN_END_DATE = wo.PlanEndDate,
                ACTU_START_DATE = wo.ActuStartDate,
                ACTU_FINISH_DATE = wo.ActuFinishDate,
                ITEM_EXT_PROP = wo.ItemExtProp,
                ITEM_EXT_PROP_NAME = wo.ItemExtPropName,
                RETROSPECT_TYPE = ibr.RetrospectType,
                FACTORY_ID = wo.FactoryId,
                WORK_SHOP_ID = wo.WorkShopId,
                RESOURCE_ID = wo.ResourceId,
                VERSION_ID = wo.VersionId
            })
            .Where(wo => wo.SQL<int>("wo.IS_PHANTOM") == 0)
            .Where(wo => wo.SQL<int?>("wo.INV_ORG_ID") == RT.InvOrg)
            .ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
            Meta.DisablePhantoms();
        }
    }
}
