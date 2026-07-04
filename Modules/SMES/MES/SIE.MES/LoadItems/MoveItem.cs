using SIE.Domain;
using SIE.Items;
using SIE.MES.SingleLabels;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 工位挪料
    /// </summary>
    [RootEntity, Serializable]
    ////[ConditionQueryType(typeof(MoveItemCriteria))]
    [Label("工位挪料")]
    public partial class MoveItem : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MoveItem()
        {
            this.Qty = 0;
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<MoveItem>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Required]
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<MoveItem>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<MoveItem>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Required]
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<MoveItem>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly RefEntityProperty<Station> StationProperty = P<MoveItem>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 配送工位 ToStation
        /// <summary>
        /// 配送工位Id
        /// </summary>
        [Required]
        [Label("配送工位")]
        public static readonly IRefIdProperty ToStationIdProperty = P<MoveItem>.RegisterRefId(e => e.ToStationId, ReferenceType.Normal);

        /// <summary>
        /// 配送工位Id
        /// </summary>
        public double ToStationId
        {
            get { return (double)GetRefId(ToStationIdProperty); }
            set { SetRefId(ToStationIdProperty, value); }
        }

        /// <summary>
        /// 配送工位
        /// </summary>
        [Label("配送工位")]
        public static readonly RefEntityProperty<Station> ToStationProperty = P<MoveItem>.RegisterRef(e => e.ToStation, ToStationIdProperty);

        /// <summary>
        /// 配送工位
        /// </summary>
        public Station ToStation
        {
            get { return GetRefEntity(ToStationProperty); }
            set { SetRefEntity(ToStationProperty, value); }
        }
        #endregion

        #region 配送线别 ToResource
        /// <summary>
        /// 配送资源Id
        /// </summary>
        [Required]
        [Label("配送资源")]
        public static readonly IRefIdProperty ToResourceIdProperty = P<MoveItem>.RegisterRefId(e => e.ToResourceId, ReferenceType.Normal);

        /// <summary>
        /// 配送资源Id
        /// </summary>
        public double ToResourceId
        {
            get { return (double)GetRefId(ToResourceIdProperty); }
            set { SetRefId(ToResourceIdProperty, value); }
        }

        /// <summary>
        /// 配送线别
        /// </summary>
        [Label("配送资源")]
        public static readonly RefEntityProperty<WipResource> ToResourceProperty = P<MoveItem>.RegisterRef(e => e.ToResource, ToResourceIdProperty);

        /// <summary>
        /// 配送资源
        /// </summary>
        public WipResource ToResource
        {
            get { return GetRefEntity(ToResourceProperty); }
            set { SetRefEntity(ToResourceProperty, value); }
        }
        #endregion

        #region 配送工序 ToProcess
        /// <summary>
        /// 配送工序Id
        /// </summary>
        [Required]
        [Label("配送工序")]
        public static readonly IRefIdProperty ToProcessIdProperty = P<MoveItem>.RegisterRefId(e => e.ToProcessId, ReferenceType.Normal);

        /// <summary>
        /// 配送工序Id
        /// </summary>
        public double ToProcessId
        {
            get { return (double)GetRefId(ToProcessIdProperty); }
            set { SetRefId(ToProcessIdProperty, value); }
        }

        /// <summary>
        /// 配送工序
        /// </summary>
        [Label("配送工序")]
        public static readonly RefEntityProperty<Process> ToProcessProperty = P<MoveItem>.RegisterRef(e => e.ToProcess, ToProcessIdProperty);

        /// <summary>
        /// 配送工序
        /// </summary>
        public Process ToProcess
        {
            get { return GetRefEntity(ToProcessProperty); }
            set { SetRefEntity(ToProcessProperty, value); }
        }
        #endregion

        #region 来源条码 SourceCode
        /// <summary>
        /// 来源条码
        /// </summary>
        [Label("来源条码")]
        public static readonly Property<string> SourceCodeProperty = P<MoveItem>.Register(e => e.SourceCode);

        /// <summary>
        /// 来源条码
        /// </summary>
        public string SourceCode
        {
            get { return this.GetProperty(SourceCodeProperty); }
            set { this.SetProperty(SourceCodeProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源")]
        public static readonly Property<double> SourceIdProperty = P<MoveItem>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<LoadItemSourceType> SourceTypeProperty = P<MoveItem>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public LoadItemSourceType SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<MoveItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<MoveItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion 

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<MoveItem>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<MoveItem>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 挪料属性值 PropertyValueList 
        /// <summary>
        /// 挪料属性值
        /// </summary>
        [Label("挪料属性值")]
        public static readonly ListProperty<EntityList<MoveItemPropertyValue>> PropertyValueListProperty = P<MoveItem>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 挪料属性值 
        /// </summary>
        public EntityList<MoveItemPropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> ItemCodeProperty = P<MoveItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<MoveItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工位挪料 实体配置
    /// </summary>
    internal class MoveItemConfig : EntityConfig<MoveItem>
    {
        /// <summary>
        /// 实体数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_MOVE_ITEM").MapAllProperties();
            Meta.Property(MoveItem.SourceIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}