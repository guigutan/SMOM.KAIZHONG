using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.ProjectMaintains;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.LES.MaterialReturnApplys.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig), "退料申请单号编码配置项", "退料申请单号编码配置规则")]
    [ConditionQueryType(typeof(MaterialReturnApplyCriteria))]
    [Label("退料申请")]
    public class MaterialReturnApply : DataEntity
    {
        /// <summary>
        /// 退料申请-退料原因快码
        /// </summary>
        public const string MaterialReturnReasonStr = "LES_MATERIALRE_REASON";

        #region 退料单号 No
        /// <summary>
        /// 退料单号
        /// </summary>
        [Label("退料单号")]
        public static readonly Property<string> NoProperty = P<MaterialReturnApply>.Register(e => e.No);

        /// <summary>
        /// 退料单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 状态 ReStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReStatus> ReStatusProperty = P<MaterialReturnApply>.Register(e => e.ReStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public ReStatus ReStatus
        {
            get { return this.GetProperty(ReStatusProperty); }
            set { this.SetProperty(ReStatusProperty, value); }
        }
        #endregion

        #region 退料类型 ReType
        /// <summary>
        /// 退料类型
        /// </summary>
        [Label("退料类型")]
        public static readonly Property<ReType> ReTypeProperty = P<MaterialReturnApply>.Register(e => e.ReType);

        /// <summary>
        /// 退料类型
        /// </summary>
        public ReType ReType
        {
            get { return this.GetProperty(ReTypeProperty); }
            set { this.SetProperty(ReTypeProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<MaterialReturnApply>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<MaterialReturnApply>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<MaterialReturnApply>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<MaterialReturnApply>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<MaterialReturnApply>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<MaterialReturnApply>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 退料仓库 Warehouse
        /// <summary>
        /// 退料仓库Id
        /// </summary>
        [Label("退料仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<MaterialReturnApply>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 退料仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 退料仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<MaterialReturnApply>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 退料仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<MaterialReturnApply>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<MaterialReturnApply>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 接收仓库 ReceiveWarehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty ReceiveWarehouseIdProperty =
            P<MaterialReturnApply>.RegisterRefId(e => e.ReceiveWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? ReceiveWarehouseId
        {
            get { return (double?)this.GetRefNullableId(ReceiveWarehouseIdProperty); }
            set { this.SetRefNullableId(ReceiveWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> ReceiveWarehouseProperty =
            P<MaterialReturnApply>.RegisterRef(e => e.ReceiveWarehouse, ReceiveWarehouseIdProperty);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public Warehouse ReceiveWarehouse
        {
            get { return this.GetRefEntity(ReceiveWarehouseProperty); }
            set { this.SetRefEntity(ReceiveWarehouseProperty, value); }
        }
        #endregion

        #region 退料原因 Reason
        /// <summary>
        /// 退料原因
        /// </summary>
        [Label("退料原因")]
        public static readonly Property<string> ReasonProperty = P<MaterialReturnApply>.Register(e => e.Reason);

        /// <summary>
        /// 退料原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 项目号 Project
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectIdProperty =
            P<MaterialReturnApply>.RegisterRefId(e => e.ProjectId, ReferenceType.Normal);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectId
        {
            get { return (double?)this.GetRefNullableId(ProjectIdProperty); }
            set { this.SetRefNullableId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        public static readonly RefEntityProperty<ProjectMaintain> ProjectProperty =
            P<MaterialReturnApply>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目号
        /// </summary>
        public ProjectMaintain Project
        {
            get { return this.GetRefEntity(ProjectProperty); }
            set { this.SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 退料明细 DetailList
        /// <summary>
        /// 退料明细
        /// </summary>
        [Label("退料明细")]
        public static readonly ListProperty<EntityList<MaterialReturnApplyDetail>> DetailListProperty = P<MaterialReturnApply>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 退料明细
        /// </summary>
        public EntityList<MaterialReturnApplyDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<MaterialReturnApply>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<MaterialReturnApply>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
        }
        #endregion

        #region 资源编码 WipRourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> WipResourceCodeProperty = P<MaterialReturnApply>.RegisterView(e => e.WipResourceCode, p => p.WipResource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string WipResourceCode
        {
            get { return this.GetProperty(WipResourceCodeProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<MaterialReturnApply>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<MaterialReturnApply>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 接收仓库编码 ReceiveWareCode
        /// <summary>
        /// 接收仓库编码
        /// </summary>
        [Label("接收仓库编码")]
        public static readonly Property<string> ReceiveWareCodeProperty = P<MaterialReturnApply>.RegisterView(e => e.ReceiveWareCode, p => p.ReceiveWarehouse.Code);

        /// <summary>
        /// 接收仓库编码
        /// </summary>
        public string ReceiveWareCode
        {
            get { return this.GetProperty(ReceiveWareCodeProperty); }
        }
        #endregion

        #region 项目号编码 ProjectCode
        /// <summary>
        /// 项目号编码
        /// </summary>
        [Label("项目号编码")]
        public static readonly Property<string> ProjectCodeProperty = P<MaterialReturnApply>.RegisterView(e => e.ProjectCode, p => p.Project.Code);

        /// <summary>
        /// 项目号编码
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class MaterialReturnConfig : EntityConfig<MaterialReturnApply>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MRAPPLY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
