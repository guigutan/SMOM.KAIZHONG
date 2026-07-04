using SIE.Domain;
using SIE.Items;
using SIE.MES.LoadItems;
using SIE.MES.LoadItemsRecords;
using SIE.MES.SingleLabels;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.LoadItemRecords
{
    /// <summary>
    /// 上料下料记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("上料下料记录")]
    [ConditionQueryType(typeof(LoadItemsRecordCriterial))]
    public class LoadItemsRecord : DataEntity
    {
        #region 操作类型 OpareteType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<OpareteType> OpareteTypeProperty = P<LoadItemsRecord>.Register(e => e.OpareteType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public OpareteType OpareteType
        {
            get { return this.GetProperty(OpareteTypeProperty); }
            set { this.SetProperty(OpareteTypeProperty, value); }
        }
        #endregion

        #region 操作时间 OpareteTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OpareteTimeProperty = P<LoadItemsRecord>.Register(e => e.OpareteTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OpareteTime
        {
            get { return this.GetProperty(OpareteTimeProperty); }
            set { this.SetProperty(OpareteTimeProperty, value); }
        }
        #endregion

        #region 操作人 OparetorName
        /// <summary>
        /// 操作人
        /// </summary>
        [Label("操作人")]
        public static readonly Property<string> OparetorNameProperty = P<LoadItemsRecord>.Register(e => e.OparetorName);

        /// <summary>
        /// 操作人
        /// </summary>
        public string OparetorName
        {
            get { return this.GetProperty(OparetorNameProperty); }
            set { this.SetProperty(OparetorNameProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtPropName
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<LoadItemsRecord>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LoadItemsRecord>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<LoadItemSourceType> SourceTypeProperty = P<LoadItemsRecord>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public LoadItemSourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 标签号  SourceCode
        /// <summary>
        /// 标签号 
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> SourceCodeProperty = P<LoadItemsRecord>.Register(e => e.SourceCode);

        /// <summary>
        /// 标签号 
        /// </summary>
        public string SourceCode
        {
            get { return GetProperty(SourceCodeProperty); }
            set { SetProperty(SourceCodeProperty, value); }
        }
        #endregion

        #region 上料工单 WorkOrder
        /// <summary>
        /// 上料工单
        /// </summary>
        [Label("上料工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<LoadItemsRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 上料工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<LoadItemsRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 上料工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Required]
        [Label("资源Id")]
        public static readonly IRefIdProperty ResourceIdProperty = P<LoadItemsRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<LoadItemsRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
        [Label("工位Id")]
        public static readonly IRefIdProperty StationIdProperty =
            P<LoadItemsRecord>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)this.GetRefId(StationIdProperty); }
            set { this.SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly RefEntityProperty<Station> StationProperty =
            P<LoadItemsRecord>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<LoadItemsRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<LoadItemsRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion 

        #region 数量 LoadDownQty
        /// <summary>
        /// 数量
        /// </summary>
        [MinValue(0)]
        [Label("数量")]
        public static readonly Property<decimal> LoadDownQtyProperty = P<LoadItemsRecord>.Register(e => e.LoadDownQty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal LoadDownQty
        {
            get { return GetProperty(LoadDownQtyProperty); }
            set { SetProperty(LoadDownQtyProperty, value); }
        }
        #endregion

        #region 剩余数量 Qty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [MinValue(0)]
        [Label("剩余数量")]
        public static readonly Property<string> QtyProperty = P<LoadItemsRecord>.Register(e => e.Qty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public string Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region  FactoryName
        /// <summary>
        ///工厂名称
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<LoadItemsRecord>.RegisterView(e => e.FactoryName, p => p.Resource.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            set { SetProperty(FactoryNameProperty, value); }
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<LoadItemsRecord>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<LoadItemsRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 下料类型 UnloadItemType
        /// <summary>
        /// 下料类型
        /// </summary>
        [Label("下料类型")]
        public static readonly Property<UnloadItemType?> UnloadItemTypeProperty = P<LoadItemsRecord>.Register(e => e.UnloadItemType);

        /// <summary>
        /// 下料类型
        /// </summary>
        public UnloadItemType? UnloadItemType
        {
            get { return this.GetProperty(UnloadItemTypeProperty); }
            set { this.SetProperty(UnloadItemTypeProperty, value); }
        }
        #endregion

        #region 上下料的Id SourceId
        /// <summary>
        /// 上下料的Id
        /// </summary>
        [Label("上下料的Id")]
        public static readonly Property<double> SourceIdProperty = P<LoadItemsRecord>.Register(e => e.SourceId);

        /// <summary>
        /// 上下料的Id
        /// </summary>
        public double SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 上料记录实体 实体配置
    /// </summary>
    internal class LoadItemsRecordConfig : EntityConfig<LoadItemsRecord>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LOAD_ITEMS_REC").MapAllProperties();
            Meta.IndexGroupOnProperties(LoadItemsRecord.ResourceIdProperty, LoadItemsRecord.StationIdProperty);
            Meta.IndexGroupOnProperties(LoadItemsRecord.WorkOrderIdProperty, LoadItemsRecord.ItemIdProperty);
            Meta.DisablePhantoms();
        }
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OpareteType
    {
        /// <summary>
        /// 上料
        /// </summary>
        [Label("上料")]
        LoadItem,

        /// <summary>
        /// 下料
        /// </summary>
        [Label("下料")]
        UnloadItem

    }

    /// <summary>
    /// 下料类型
    /// </summary>
    public enum UnloadItemType
    {
        /// <summary>
        /// 正常下料
        /// </summary>
        [Label("正常下料")]
        Pass,

        /// <summary>
        /// 不良下料
        /// </summary>
        [Label("不良下料")]
        NG

    }
}
