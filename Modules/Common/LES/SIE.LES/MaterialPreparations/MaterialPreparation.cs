using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.LES.MaterialPreparations.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.LES.MaterialPreparations.Configs;
using SIE.Core.ProjectMaintains;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig), "备料需求单号编码配置项", "备料需求单号编码配置规则")]
    [EntityWithConfig(typeof(LimitedPrepareMaxConfig))]
    [ConditionQueryType(typeof(MaterialPreparationCriteria))]
    [Label("备料需求单")]
    public class MaterialPreparation : DataEntity
    {
        /// <summary>
        /// 备料原因快码
        /// </summary>
        public const string MaterialPreReasonStr = "LES_MATERIALPRE_REASON";

        #region 备料单号 No
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> NoProperty = P<MaterialPreparation>.Register(e => e.No);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<MaterialPreparation>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<MaterialPreparation>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<MaterialPreparation>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<MaterialPreparation>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
            P<MaterialPreparation>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<MaterialPreparation>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

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
            P<MaterialPreparation>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<MaterialPreparation>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 状态 PrepareStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<PrepareStatus> PrepareStatusProperty = P<MaterialPreparation>.Register(e => e.PrepareStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public PrepareStatus PrepareStatus
        {
            get { return this.GetProperty(PrepareStatusProperty); }
            set { this.SetProperty(PrepareStatusProperty, value); }
        }
        #endregion

        #region 备料类型 PrepareType
        /// <summary>
        /// 备料类型
        /// </summary>
        [Label("备料类型")]
        public static readonly Property<PrepareType> PrepareTypeProperty = P<MaterialPreparation>.Register(e => e.PrepareType);

        /// <summary>
        /// 备料类型
        /// </summary>
        public PrepareType PrepareType
        {
            get { return this.GetProperty(PrepareTypeProperty); }
            set { this.SetProperty(PrepareTypeProperty, value); }
        }
        #endregion

        #region 备料原因 Reason
        /// <summary>
        /// 备料原因
        /// </summary>
        [Label("备料原因")]
        public static readonly Property<string> ReasonProperty = P<MaterialPreparation>.Register(e => e.Reason);

        /// <summary>
        /// 备料原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 需求时间 PrepareTime
        /// <summary>
        /// 需求时间
        /// </summary>
        [Label("需求时间")]
        public static readonly Property<DateTime?> PrepareTimeProperty = P<MaterialPreparation>.Register(e => e.PrepareTime);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime? PrepareTime
        {
            get { return this.GetProperty(PrepareTimeProperty); }
            set { this.SetProperty(PrepareTimeProperty, value); }
        }
        #endregion

        #region 发货仓库 Warehouse
        /// <summary>
        /// 发货仓库Id
        /// </summary>
        [Label("发货仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<MaterialPreparation>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发货仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<MaterialPreparation>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发货仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 产线线边仓 LineSideWarehouse
        /// <summary>
        /// 产线线边仓Id
        /// </summary>
        [Label("产线线边仓")]
        public static readonly IRefIdProperty LineSideWarehouseIdProperty =
            P<MaterialPreparation>.RegisterRefId(e => e.LineSideWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 产线线边仓Id
        /// </summary>
        public double? LineSideWarehouseId
        {
            get { return (double?)this.GetRefNullableId(LineSideWarehouseIdProperty); }
            set { this.SetRefNullableId(LineSideWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 产线线边仓
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> LineSideWarehouseProperty =
            P<MaterialPreparation>.RegisterRef(e => e.LineSideWarehouse, LineSideWarehouseIdProperty);

        /// <summary>
        /// 产线线边仓
        /// </summary>
        public Warehouse LineSideWarehouse
        {
            get { return this.GetRefEntity(LineSideWarehouseProperty); }
            set { this.SetRefEntity(LineSideWarehouseProperty, value); }
        }
        #endregion

        #region 工单视图字段
        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<MaterialPreparation>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

        #region 产品编码 WoProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> WoProductCodeProperty = P<MaterialPreparation>.RegisterView(e => e.WoProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string WoProductCode
        {
            get { return this.GetProperty(WoProductCodeProperty); }
        }
        #endregion

        #region 产品名称 WoProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> WoProductNameProperty = P<MaterialPreparation>.RegisterView(e => e.WoProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string WoProductName
        {
            get { return this.GetProperty(WoProductNameProperty); }
        }
        #endregion

        #region 工单状态 WoState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState?> WoStateProperty = P<MaterialPreparation>.RegisterView(e => e.WoState, p => p.WorkOrder.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState? WoState
        {
            get { return this.GetProperty(WoStateProperty); }
        }
        #endregion

        #region 工单类型 WoType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType?> WoTypeProperty = P<MaterialPreparation>.RegisterView(e => e.WoType, p => p.WorkOrder.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType? WoType
        {
            get { return this.GetProperty(WoTypeProperty); }
        }
        #endregion

        #region 计划数量 WoPlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal?> WoPlanQtyProperty = P<MaterialPreparation>.RegisterView(e => e.WoPlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal? WoPlanQty
        {
            get { return this.GetProperty(WoPlanQtyProperty); }
        }
        #endregion

        #region 完工数量 WoFinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal?> WoFinishQtyProperty = P<MaterialPreparation>.RegisterView(e => e.WoFinishQty, p => p.WorkOrder.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal? WoFinishQty
        {
            get { return this.GetProperty(WoFinishQtyProperty); }
        }
        #endregion

        #region 计划开始时间 WoPlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime?> WoPlanBeginDateProperty = P<MaterialPreparation>.RegisterView(e => e.WoPlanBeginDate, p => p.WorkOrder.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? WoPlanBeginDate
        {
            get { return this.GetProperty(WoPlanBeginDateProperty); }
        }
        #endregion

        #region 计划结束时间 WoPlanEndDate
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime?> WoPlanEndDateProperty = P<MaterialPreparation>.RegisterView(e => e.WoPlanEndDate, p => p.WorkOrder.PlanEndDate);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? WoPlanEndDate
        {
            get { return this.GetProperty(WoPlanEndDateProperty); }
        }
        #endregion

        #region 实际开始时间 WoActuStartDate
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime?> WoActuStartDateProperty = P<MaterialPreparation>.RegisterView(e => e.WoActuStartDate, p => p.WorkOrder.ActuStartDate);

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? WoActuStartDate
        {
            get { return this.GetProperty(WoActuStartDateProperty); }
        }
        #endregion

        #region 实际结束时间 WoActuFinishDate
        /// <summary>
        /// 实际结束时间
        /// </summary>
        [Label("实际结束时间")]
        public static readonly Property<DateTime?> WoActuFinishDateProperty = P<MaterialPreparation>.RegisterView(e => e.WoActuFinishDate, p => p.WorkOrder.ActuFinishDate);

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? WoActuFinishDate
        {
            get { return this.GetProperty(WoActuFinishDateProperty); }
        }
        #endregion

        #region 销售订单号 WoSaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> WoSaleOrderNoProperty = P<MaterialPreparation>.RegisterView(e => e.WoSaleOrderNo, p => p.WorkOrder.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string WoSaleOrderNo
        {
            get { return this.GetProperty(WoSaleOrderNoProperty); }
        }
        #endregion

        #region 客户订单号 WoCustomerOrderNo
        /// <summary>
        /// 客户订单号
        /// </summary>
        [Label("客户订单号")]
        public static readonly Property<string> WoCustomerOrderNoProperty = P<MaterialPreparation>.RegisterView(e => e.WoCustomerOrderNo, p => p.WorkOrder.CustomerOrderNo);

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string WoCustomerOrderNo
        {
            get { return this.GetProperty(WoCustomerOrderNoProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<MaterialPreparation>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion


        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<MaterialPreparation>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<MaterialPreparation>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 发货仓库名称 WarehouseName
        /// <summary>
        /// 发货仓库名称
        /// </summary>
        [Label("发货仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<MaterialPreparation>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 发货仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<MaterialPreparation>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

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
            P<MaterialPreparation>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

        /// <summary>
        /// 项目号
        /// </summary>
        public ProjectMaintain ProjectMaintain
        {
            get { return this.GetRefEntity(ProjectMaintainProperty); }
            set { this.SetRefEntity(ProjectMaintainProperty, value); }
        }
        #endregion

        #endregion

        #region 发运单号 ShippingOrderNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> ShippingOrderNoProperty = P<MaterialPreparation>.Register(e => e.ShippingOrderNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippingOrderNo
        {
            get { return this.GetProperty(ShippingOrderNoProperty); }
            set { this.SetProperty(ShippingOrderNoProperty, value); }
        }
        #endregion

        #region 备料需求明细 DetailList
        /// <summary>
        /// 备料需求明细
        /// </summary>
        [Label("备料需求明细")]
        public static readonly ListProperty<EntityList<MaterialPreparationDetail>> DetailListProperty = P<MaterialPreparation>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 备料需求明细
        /// </summary>
        public EntityList<MaterialPreparationDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 取消记录 CancelRecordList
        /// <summary>
        /// 取消记录
        /// </summary>
        [Label("取消记录")]
        public static readonly ListProperty<EntityList<MaterialPreCancelRecord>> CancelRecordListProperty = P<MaterialPreparation>.RegisterList(e => e.CancelRecordList);

        /// <summary>
        /// 取消记录
        /// </summary>
        public EntityList<MaterialPreCancelRecord> CancelRecordList
        {
            get { return this.GetLazyList(CancelRecordListProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 备料需求单实体配置
    /// </summary>
    public class MaterialPreparationConfig : EntityConfig<MaterialPreparation>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_PRE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
