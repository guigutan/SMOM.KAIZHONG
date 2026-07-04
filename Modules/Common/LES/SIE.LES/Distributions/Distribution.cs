using SIE.Common.Configs;
using SIE.Domain;
using SIE.LES.Distributions.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.Distributions
{
    /// <summary>
    /// 配送单管理
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DistributionCriteria))]    
    [EntityWithConfig(typeof(DistributionNoConfig))]
    [Label("配送单管理")]
    public class Distribution : DataEntity
    {
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<Distribution>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 订单状态 OrderState
        /// <summary>
        /// 订单状态
        /// </summary>
        [Label("订单状态")]
        public static readonly Property<OrderState> OrderStateProperty = P<Distribution>.Register(e => e.OrderState);

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderState OrderState
        {
            get { return this.GetProperty(OrderStateProperty); }
            set { this.SetProperty(OrderStateProperty, value); }
        }
        #endregion

        #region 来源订单号 SourceNo
        /// <summary>
        /// 来源订单号
        /// </summary>
        [Label("来源订单号")]
        public static readonly Property<string> SourceNoProperty = P<Distribution>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源订单号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 容器编码 Lpn
        /// <summary>
        /// 容器编码
        /// </summary>
        [Label("容器编码")]
        public static readonly Property<string> LpnProperty = P<Distribution>.Register(e => e.Lpn);

        /// <summary>
        /// 容器编码
        /// </summary>
        public string Lpn
        {
            get { return this.GetProperty(LpnProperty); }
            set { this.SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<Distribution>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<Distribution>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 发货库位 StorageLocation
        /// <summary>
        /// 发货库位Id
        /// </summary>
        [Label("发货库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<Distribution>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 发货库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 发货库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<Distribution>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 发货库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 目标产线 ProductLine
        /// <summary>
        /// 目标产线Id
        /// </summary>
        [Label("目标产线")]
        public static readonly IRefIdProperty ProductLineIdProperty =
            P<Distribution>.RegisterRefId(e => e.ProductLineId, ReferenceType.Normal);

        /// <summary>
        /// 目标产线Id
        /// </summary>
        public double ProductLineId
        {
            get { return (double)this.GetRefId(ProductLineIdProperty); }
            set { this.SetRefId(ProductLineIdProperty, value); }
        }

        /// <summary>
        /// 目标产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ProductLineProperty =
            P<Distribution>.RegisterRef(e => e.ProductLine, ProductLineIdProperty);

        /// <summary>
        /// 目标产线
        /// </summary>
        public WipResource ProductLine
        {
            get { return this.GetRefEntity(ProductLineProperty); }
            set { this.SetRefEntity(ProductLineProperty, value); }
        }
        #endregion

        #region 是否呼叫AGV IsCallAgv
        /// <summary>
        /// 是否呼叫AGV
        /// </summary>
        [Label("是否呼叫AGV")]
        public static readonly Property<bool> IsCallAgvProperty = P<Distribution>.Register(e => e.IsCallAgv);

        /// <summary>
        /// 是否呼叫AGV
        /// </summary>
        public bool IsCallAgv
        {
            get { return this.GetProperty(IsCallAgvProperty); }
            set { this.SetProperty(IsCallAgvProperty, value); }
        }
        #endregion

        #region 配送发起人 Deliveryman
        /// <summary>
        /// 配送发起人Id
        /// </summary>
        [Label("配送发起人")]
        public static readonly IRefIdProperty DeliverymanIdProperty =
            P<Distribution>.RegisterRefId(e => e.DeliverymanId, ReferenceType.Normal);

        /// <summary>
        /// 配送发起人Id
        /// </summary>
        public double? DeliverymanId
        {
            get { return (double?)this.GetRefNullableId(DeliverymanIdProperty); }
            set { this.SetRefNullableId(DeliverymanIdProperty, value); }
        }

        /// <summary>
        /// 配送发起人
        /// </summary>
        public static readonly RefEntityProperty<Employee> DeliverymanProperty =
            P<Distribution>.RegisterRef(e => e.Deliveryman, DeliverymanIdProperty);

        /// <summary>
        /// 配送发起人
        /// </summary>
        public Employee Deliveryman
        {
            get { return this.GetRefEntity(DeliverymanProperty); }
            set { this.SetRefEntity(DeliverymanProperty, value); }
        }
        #endregion

        #region 配送发起时间 DeliveryDate
        /// <summary>
        /// 配送发起时间
        /// </summary>
        [Label("配送发起时间")]
        public static readonly Property<DateTime?> DeliveryDateProperty = P<Distribution>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 配送发起时间
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return this.GetProperty(DeliveryDateProperty); }
            set { this.SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 接收人 Receiver
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiverIdProperty =
            P<Distribution>.RegisterRefId(e => e.ReceiverId, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double? ReceiverId
        {
            get { return (double?)this.GetRefNullableId(ReceiverIdProperty); }
            set { this.SetRefNullableId(ReceiverIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiverProperty =
            P<Distribution>.RegisterRef(e => e.Receiver, ReceiverIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee Receiver
        {
            get { return this.GetRefEntity(ReceiverProperty); }
            set { this.SetRefEntity(ReceiverProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveDate
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiveDateProperty = P<Distribution>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveDate
        {
            get { return this.GetProperty(ReceiveDateProperty); }
            set { this.SetProperty(ReceiveDateProperty, value); }
        }
        #endregion

        #region 明细列表 DistributionDetailList
        /// <summary>
        /// 明细列表
        /// </summary>
        [Label("明细")]
        public static readonly ListProperty<EntityList<DistributionDetail>> DistributionDetailListProperty = P<Distribution>.RegisterList(e => e.DistributionDetailList);
        /// <summary>
        /// 明细列表
        /// </summary>
        public EntityList<DistributionDetail> DistributionDetailList
        {
            get { return this.GetLazyList(DistributionDetailListProperty); }
        }
        #endregion

        #region 扫描记录 DistributionLabelList
        /// <summary>
        /// 扫描记录
        /// </summary>
        [Label("扫描记录")]
        public static readonly ListProperty<EntityList<DistributionLabel>> DistributionLabelListProperty = P<Distribution>.RegisterList(e => e.DistributionLabelList);
        /// <summary>
        /// 扫描记录
        /// </summary>
        public EntityList<DistributionLabel> DistributionLabelList
        {
            get { return this.GetLazyList(DistributionLabelListProperty); }
        }
        #endregion

        #region 视图属性
        #region 目标产线 ProductLineName
        /// <summary>
        /// 目标产线
        /// </summary>
        [Label("目标产线")]
        public static readonly Property<string> ProductLineNameProperty = P<Distribution>.RegisterView(e => e.ProductLineName, p => p.ProductLine.Name);

        /// <summary>
        /// 目标产线
        /// </summary>
        public string ProductLineName
        {
            get { return this.GetProperty(ProductLineNameProperty); }
        }
        #endregion

        #region 目标产线编码 ProductLineCode
        /// <summary>
        /// 目标产线编码
        /// </summary>
        [Label("目标产线编码")]
        public static readonly Property<string> ProductLineCodeProperty = P<Distribution>.RegisterView(e => e.ProductLineCode, p => p.ProductLine.Code);

        /// <summary>
        /// 目标产线
        /// </summary>
        public string ProductLineCode
        {
            get { return this.GetProperty(ProductLineCodeProperty); }
        }
        #endregion

        #region 配送发起人 DeliveryManName
        /// <summary>
        /// 配送发起人
        /// </summary>
        [Label("配送发起人")]
        public static readonly Property<string> DeliveryManNameProperty = P<Distribution>.RegisterView(e => e.DeliveryManName, p => p.Deliveryman.Name);

        /// <summary>
        /// 配送发起人
        /// </summary>
        public string DeliveryManName
        {
            get { return this.GetProperty(DeliveryManNameProperty); }
        }
        #endregion

        #region 接收人 ReceiverName
        /// <summary>
        /// 接收人
        /// </summary>
        [Label("接收人")]
        public static readonly Property<string> ReceiverNameProperty = P<Distribution>.RegisterView(e => e.ReceiverName, p => p.Receiver.Name);

        /// <summary>
        /// 接收人
        /// </summary>
        public string ReceiverName
        {
            get { return this.GetProperty(ReceiverNameProperty); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<Distribution>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 配送管理 实体配置
    /// </summary>
    internal class DistributionConfig : EntityConfig<Distribution>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DISTRIBUTION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
