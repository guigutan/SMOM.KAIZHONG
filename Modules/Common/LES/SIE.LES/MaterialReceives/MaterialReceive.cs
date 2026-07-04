using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaterialReceiveCriteria))]
    [Label("物料接收")]
    public class MaterialReceive : DataEntity
    {
        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveState?> StateProperty = P<MaterialReceive>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReceiveState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 备料需求单 MaterialPreparation
        /// <summary>
        /// 备料需求单Id
        /// </summary>
        [Label("备料需求单")]
        public static readonly IRefIdProperty MaterialPreparationIdProperty =
            P<MaterialReceive>.RegisterRefId(e => e.MaterialPreparationId, ReferenceType.Normal);

        /// <summary>
        /// 备料需求单Id
        /// </summary>
        public double? MaterialPreparationId
        {
            get { return (double?)this.GetRefNullableId(MaterialPreparationIdProperty); }
            set { this.SetRefNullableId(MaterialPreparationIdProperty, value); }
        }

        /// <summary>
        /// 备料需求单
        /// </summary>
        public static readonly RefEntityProperty<MaterialPreparation> MaterialPreparationProperty =
            P<MaterialReceive>.RegisterRef(e => e.MaterialPreparation, MaterialPreparationIdProperty);

        /// <summary>
        /// 备料需求单
        /// </summary>
        public MaterialPreparation MaterialPreparation
        {
            get { return this.GetRefEntity(MaterialPreparationProperty); }
            set { this.SetRefEntity(MaterialPreparationProperty, value); }
        }
        #endregion

        #region 来源单号 SourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoProperty = P<MaterialReceive>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 发运单号 SoNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> SoNoProperty = P<MaterialReceive>.Register(e => e.SoNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo
        {
            get { return this.GetProperty(SoNoProperty); }
            set { this.SetProperty(SoNoProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<DateTime?> DeliveryDateProperty = P<MaterialReceive>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return GetProperty(DeliveryDateProperty); }
            set { SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 单据小类 TransactionName
        /// <summary>
        /// 单据小类
        /// </summary>
        [Label("单据小类")]
        public static readonly Property<string> TransactionNameProperty = P<MaterialReceive>.Register(e => e.TransactionName);

        /// <summary>
        /// 单据小类
        /// </summary>
        public string TransactionName
        {
            get { return this.GetProperty(TransactionNameProperty); }
            set { this.SetProperty(TransactionNameProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<MaterialReceive>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<MaterialReceive>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

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
            P<MaterialReceive>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<MaterialReceive>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
            P<MaterialReceive>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<MaterialReceive>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

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
            P<MaterialReceive>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<MaterialReceive>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 发料仓库 ShippingWarehouse
        /// <summary>
        /// 发料仓库Id
        /// </summary>
        [Label("发料仓库")]
        public static readonly IRefIdProperty ShippingWarehouseIdProperty =
            P<MaterialReceive>.RegisterRefId(e => e.ShippingWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发料仓库Id
        /// </summary>
        public double? ShippingWarehouseId
        {
            get { return (double?)this.GetRefNullableId(ShippingWarehouseIdProperty); }
            set { this.SetRefNullableId(ShippingWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发料仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> ShippingWarehouseProperty =
            P<MaterialReceive>.RegisterRef(e => e.ShippingWarehouse, ShippingWarehouseIdProperty);

        /// <summary>
        /// 发料仓库
        /// </summary>
        public Warehouse ShippingWarehouse
        {
            get { return this.GetRefEntity(ShippingWarehouseProperty); }
            set { this.SetRefEntity(ShippingWarehouseProperty, value); }
        }
        #endregion

        #region 接收仓库 ReceiveWarehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty ReceiveWarehouseIdProperty =
            P<MaterialReceive>.RegisterRefId(e => e.ReceiveWarehouseId, ReferenceType.Normal);

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
            P<MaterialReceive>.RegisterRef(e => e.ReceiveWarehouse, ReceiveWarehouseIdProperty);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public Warehouse ReceiveWarehouse
        {
            get { return this.GetRefEntity(ReceiveWarehouseProperty); }
            set { this.SetRefEntity(ReceiveWarehouseProperty, value); }
        }
        #endregion

        #region 接收库位 ReceiveLocation
        /// <summary>
        /// 接收库位Id
        /// </summary>
        [Label("接收库位")]
        public static readonly IRefIdProperty ReceiveLocationIdProperty = P<MaterialReceive>.RegisterRefId(e => e.ReceiveLocationId, ReferenceType.Normal);

        /// <summary>
        /// 接收库位Id
        /// </summary>
        public double? ReceiveLocationId
        {
            get { return (double?)GetRefNullableId(ReceiveLocationIdProperty); }
            set { SetRefNullableId(ReceiveLocationIdProperty, value); }
        }

        /// <summary>
        /// 接收库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> ReceiveLocationProperty = P<MaterialReceive>.RegisterRef(e => e.ReceiveLocation, ReceiveLocationIdProperty);

        /// <summary>
        /// 接收库位
        /// </summary>
        public StorageLocation ReceiveLocation
        {
            get { return GetRefEntity(ReceiveLocationProperty); }
            set { SetRefEntity(ReceiveLocationProperty, value); }
        }
        #endregion

        #region 明细列表 DetailList
        /// <summary>
        /// 明细列表
        /// </summary>
        [Label("明细列表")]
        public static readonly ListProperty<EntityList<MaterialReceiveDetail>> DetailListProperty = P<MaterialReceive>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 明细列表
        /// </summary>
        public EntityList<MaterialReceiveDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 标签列表 LabelList
        /// <summary>
        /// 标签列表
        /// </summary>
        [Label("标签列表")]
        public static readonly ListProperty<EntityList<MaterialReceiveLabel>> LabelListProperty = P<MaterialReceive>.RegisterList(e => e.LabelList);

        /// <summary>
        /// 标签列表
        /// </summary>
        public EntityList<MaterialReceiveLabel> LabelList
        {
            get { return this.GetLazyList(LabelListProperty); }
        }
        #endregion

        #region 视图属性

        #region 备料需求单号 MaterialPreparationNo
        /// <summary>
        /// 备料需求单号
        /// </summary>
        [Label("备料需求单号")]
        public static readonly Property<string> MaterialPreparationNoProperty = P<MaterialReceive>.RegisterView(e => e.MaterialPreparationNo, p => p.MaterialPreparation.No);

        /// <summary>
        /// 备料需求单号
        /// </summary>
        public string MaterialPreparationNo
        {
            get { return this.GetProperty(MaterialPreparationNoProperty); }
        }

        #endregion

        #region 工单 WoNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoNoProperty = P<MaterialReceive>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

        #region 发货仓库编码 ShippingWarehouseCode
        /// <summary>
        /// 发货仓库编码
        /// </summary>
        [Label("发货仓库编码")]
        public static readonly Property<string> ShippingWarehouseCodeProperty = P<MaterialReceive>.RegisterView(e => e.ShippingWarehouseCode, p => p.ShippingWarehouse.Code);

        /// <summary>
        /// 发货仓库编码
        /// </summary>
        public string ShippingWarehouseCode
        {
            get { return this.GetProperty(ShippingWarehouseCodeProperty); }
        }

        #endregion

        #region 发货仓库 ShippingWarehouseName
        /// <summary>
        /// 发货仓库
        /// </summary>
        [Label("发货仓库")]
        public static readonly Property<string> ShippingWarehouseNameProperty = P<MaterialReceive>.RegisterView(e => e.ShippingWarehouseName, p => p.ShippingWarehouse.Name);

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string ShippingWarehouseName
        {
            get { return this.GetProperty(ShippingWarehouseNameProperty); }
        }

        #endregion

        #region 接收仓库编码 ReceiveWarehouseCode
        /// <summary>
        /// 接收仓库编码
        /// </summary>
        [Label("接收仓库编码")]
        public static readonly Property<string> ReceiveWarehouseCodeProperty = P<MaterialReceive>.RegisterView(e => e.ReceiveWarehouseCode, p => p.ReceiveWarehouse.Code);

        /// <summary>
        /// 接收仓库编码
        /// </summary>
        public string ReceiveWarehouseCode
        {
            get { return this.GetProperty(ReceiveWarehouseCodeProperty); }
        }
        #endregion

        #region 接收仓库 ReceiveWarehouseName
        /// <summary>
        /// 接收仓库
        /// </summary>
        [Label("接收仓库")]
        public static readonly Property<string> ReceiveWarehouseNameProperty = P<MaterialReceive>.RegisterView(e => e.ReceiveWarehouseName, p => p.ReceiveWarehouse.Name);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public string ReceiveWarehouseName
        {
            get { return this.GetProperty(ReceiveWarehouseNameProperty); }
        }

        #endregion

        #region 需求时间 PrepareTime
        /// <summary>
        /// 需求时间
        /// </summary>
        [Label("需求时间")]
        public static readonly Property<DateTime?> PrepareTimeProperty = P<MaterialReceive>.RegisterView(e => e.PrepareTime, p => p.MaterialPreparation.PrepareTime);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime? PrepareTime
        {
            get { return this.GetProperty(PrepareTimeProperty); }
        }

        #endregion

        #region 备料类型 PrepareType
        /// <summary>
        /// 备料类型
        /// </summary>
        [Label("备料类型")]
        public static readonly Property<PrepareType> PrepareTypeProperty = P<MaterialReceive>.RegisterView(e => e.PrepareType, p => p.MaterialPreparation.PrepareType);

        /// <summary>
        /// 备料类型
        /// </summary>
        public PrepareType PrepareType
        {
            get { return this.GetProperty(PrepareTypeProperty); }
        }

        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<MaterialReceive>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 资源 ResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<MaterialReceive>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<MaterialReceive>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class MaterialReceiveConfig : EntityConfig<MaterialReceive>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_REC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
