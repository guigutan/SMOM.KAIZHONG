using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.LES.MaterialPreparations.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaterialReceiveRecordCriteria))]
    [Label("物料接收记录")]
    public class MaterialReceiveRecord : DataEntity
    {
        #region 来源单号 SourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoProperty = P<MaterialReceiveRecord>.Register(e => e.SourceNo);

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
        public static readonly Property<string> SoNoProperty = P<MaterialReceiveRecord>.Register(e => e.SoNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo
        {
            get { return this.GetProperty(SoNoProperty); }
            set { this.SetProperty(SoNoProperty, value); }
        }
        #endregion

        #region 发运单行号 SoLineNo
        /// <summary>
        /// 发运单行号
        /// </summary>
        [Label("发运单行号")]
        public static readonly Property<string> SoLineNoProperty = P<MaterialReceiveRecord>.Register(e => e.SoLineNo);

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo
        {
            get { return this.GetProperty(SoLineNoProperty); }
            set { this.SetProperty(SoLineNoProperty, value); }
        }
        #endregion

        #region 接收方式 ReceiveType
        /// <summary>
        /// 接收方式
        /// </summary>
        [Label("接收方式")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<MaterialReceiveRecord>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收方式
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
            set { this.SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        #region 本次备料数 PreparedQty
        /// <summary>
        /// 本次备料数
        /// </summary>
        [Label("本次备料数")]
        public static readonly Property<decimal> PreparedQtyProperty = P<MaterialReceiveRecord>.Register(e => e.PreparedQty);

        /// <summary>
        /// 本次备料数
        /// </summary>
        public decimal PreparedQty
        {
            get { return this.GetProperty(PreparedQtyProperty); }
            set { this.SetProperty(PreparedQtyProperty, value); }
        }
        #endregion

        #region 发料数 IssuedQty
        /// <summary>
        /// 发料数
        /// </summary>
        [Label("发料数")]
        public static readonly Property<decimal> IssuedQtyProperty = P<MaterialReceiveRecord>.Register(e => e.IssuedQty);

        /// <summary>
        /// 发料数
        /// </summary>
        public decimal IssuedQty
        {
            get { return this.GetProperty(IssuedQtyProperty); }
            set { this.SetProperty(IssuedQtyProperty, value); }
        }
        #endregion

        #region 接收数 ReceivedQty
        /// <summary>
        /// 接收数
        /// </summary>
        [Label("接收数")]
        public static readonly Property<decimal> ReceivedQtyProperty = P<MaterialReceiveRecord>.Register(e => e.ReceivedQty);

        /// <summary>
        /// 接收数
        /// </summary>
        public decimal ReceivedQty
        {
            get { return this.GetProperty(ReceivedQtyProperty); }
            set { this.SetProperty(ReceivedQtyProperty, value); }
        }
        #endregion

        #region 超收数 OverReceivedQty
        /// <summary>
        /// 超收数
        /// </summary>
        [Label("超收数")]
        public static readonly Property<decimal> OverReceivedQtyProperty = P<MaterialReceiveRecord>.Register(e => e.OverReceivedQty);

        /// <summary>
        /// 超收数
        /// </summary>
        public decimal OverReceivedQty
        {
            get { return this.GetProperty(OverReceivedQtyProperty); }
            set { this.SetProperty(OverReceivedQtyProperty, value); }
        }
        #endregion

        #region 拒收数 RejectedQty
        /// <summary>
        /// 拒收数
        /// </summary>
        [Label("拒收数")]
        public static readonly Property<decimal> RejectedQtyProperty = P<MaterialReceiveRecord>.Register(e => e.RejectedQty);

        /// <summary>
        /// 拒收数
        /// </summary>
        public decimal RejectedQty
        {
            get { return this.GetProperty(RejectedQtyProperty); }
            set { this.SetProperty(RejectedQtyProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<MaterialReceiveRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<MaterialReceiveRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料扩展属性名称 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MaterialReceiveRecord>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<MaterialReceiveRecord>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 标签号 LabelNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<MaterialReceiveRecord>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<MaterialReceiveRecord>.Register(e => e.LotCode);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<MaterialReceiveRecord>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<MaterialReceiveRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<MaterialReceiveRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

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
            P<MaterialReceiveRecord>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<MaterialReceiveRecord>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
            P<MaterialReceiveRecord>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<MaterialReceiveRecord>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

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
            P<MaterialReceiveRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<MaterialReceiveRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 接收人 ReceiveBy
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiveByIdProperty =
            P<MaterialReceiveRecord>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double? ReceiveById
        {
            get { return (double?)this.GetRefId(ReceiveByIdProperty); }
            set { this.SetRefId(ReceiveByIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiveByProperty =
            P<MaterialReceiveRecord>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee ReceiveBy
        {
            get { return this.GetRefEntity(ReceiveByProperty); }
            set { this.SetRefEntity(ReceiveByProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveTime
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiveTimeProperty = P<MaterialReceiveRecord>.Register(e => e.ReceiveTime);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveTime
        {
            get { return this.GetProperty(ReceiveTimeProperty); }
            set { this.SetProperty(ReceiveTimeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveState?> StateProperty = P<MaterialReceiveRecord>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReceiveState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 发料仓库 ShippingWarehouse
        /// <summary>
        /// 发料仓库Id
        /// </summary>
        [Label("发料仓库")]
        public static readonly IRefIdProperty ShippingWarehouseIdProperty =
            P<MaterialReceiveRecord>.RegisterRefId(e => e.ShippingWarehouseId, ReferenceType.Normal);

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
            P<MaterialReceiveRecord>.RegisterRef(e => e.ShippingWarehouse, ShippingWarehouseIdProperty);

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
            P<MaterialReceiveRecord>.RegisterRefId(e => e.ReceiveWarehouseId, ReferenceType.Normal);

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
            P<MaterialReceiveRecord>.RegisterRef(e => e.ReceiveWarehouse, ReceiveWarehouseIdProperty);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public Warehouse ReceiveWarehouse
        {
            get { return this.GetRefEntity(ReceiveWarehouseProperty); }
            set { this.SetRefEntity(ReceiveWarehouseProperty, value); }
        }
        #endregion

        #region 备料类型 PrepareType
        /// <summary>
        /// 备料类型
        /// </summary>
        [Label("备料类型")]
        public static readonly Property<PrepareType?> PrepareTypeProperty = P<MaterialReceiveRecord>.Register(e => e.PrepareType);

        /// <summary>
        /// 备料类型
        /// </summary>
        public PrepareType? PrepareType
        {
            get { return this.GetProperty(PrepareTypeProperty); }
            set { this.SetProperty(PrepareTypeProperty, value); }
        }
        #endregion

        #region 视图属性        

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialReceiveRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialReceiveRecord>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 工单 WoNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoNoProperty = P<MaterialReceiveRecord>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

        #region 接收仓库 WarehouseCode
        /// <summary>
        /// 接收仓库
        /// </summary>
        [Label("接收仓库")]
        public static readonly Property<string> WarehouseCodeProperty = P<MaterialReceiveRecord>.RegisterView(e => e.WarehouseCode, p => p.ReceiveWarehouse.Code);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 接收人 ReceiveByName
        /// <summary>
        /// 接收人
        /// </summary>
        [Label("接收人")]
        public static readonly Property<string> ReceiveByNameProperty = P<MaterialReceiveRecord>.RegisterView(e => e.ReceiveByName, p => p.ReceiveBy.Name);

        /// <summary>
        /// 接收人
        /// </summary>
        public string ReceiveByName
        {
            get { return this.GetProperty(ReceiveByNameProperty); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<MaterialReceiveRecord>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

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
        public static readonly Property<string> ResourceNameProperty = P<MaterialReceiveRecord>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

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
        public static readonly Property<string> FactoryNameProperty = P<MaterialReceiveRecord>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

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
    internal class MaterialReceiveRecordConfig : EntityConfig<MaterialReceiveRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_REC_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
