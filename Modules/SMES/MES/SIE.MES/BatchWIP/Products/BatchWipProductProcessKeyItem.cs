using SIE.Domain;
using SIE.Items;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.SingleLabels;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次产品生产关键件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品生产关键件")]
    public partial class BatchWipProductProcessKeyItem : DataEntity
    {
        #region 用料量 Qty
        /// <summary>
        /// 用料量
        /// </summary>
        [Label("用料量")]
        public static readonly Property<decimal> QtyProperty = P<BatchWipProductProcessKeyItem>.Register(e => e.Qty);

        /// <summary>
        /// 用料量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单位用料量 SingleQty
        /// <summary>
        /// 单位用料量
        /// </summary>
        [Label("单位用料量")]
        public static readonly Property<decimal> SingleQtyProperty = P<BatchWipProductProcessKeyItem>.Register(e => e.SingleQty);

        /// <summary>
        /// 单位用料量
        /// </summary>
        public decimal SingleQty
        {
            get { return GetProperty(SingleQtyProperty); }
            set { SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 来源条码 SourceCode
        /// <summary>
        /// 来源条码
        /// </summary>
        [Label("来源条码")]
        public static readonly Property<string> SourceCodeProperty = P<BatchWipProductProcessKeyItem>.Register(e => e.SourceCode);

        /// <summary>
        /// 来源条码
        /// </summary>
        public string SourceCode
        {
            get { return GetProperty(SourceCodeProperty); }
            set { SetProperty(SourceCodeProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源ID")]
        public static readonly Property<double> SourceIdProperty = P<BatchWipProductProcessKeyItem>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId
        {
            get { return GetProperty(SourceIdProperty); }
            set { SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<LoadItemSourceType> SourceTypeProperty = P<BatchWipProductProcessKeyItem>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public LoadItemSourceType SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<BatchWipProductProcessKeyItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<BatchWipProductProcessKeyItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<BatchWipProductProcessKeyItem>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Station> StationProperty = P<BatchWipProductProcessKeyItem>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<BatchWipProductProcessKeyItem>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<BatchWipProductProcessKeyItem>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<BatchWipProductProcessKeyItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<BatchWipProductProcessKeyItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 采集记录Id Detail
        /// <summary>
        /// 采集记录Id
        /// </summary>
        [Label("采集记录")]
        public static readonly IRefIdProperty DetailIdProperty = P<BatchWipProductProcessKeyItem>.RegisterRefId(e => e.DetailId, ReferenceType.Parent);

        /// <summary>
        /// 采集记录Id
        /// </summary>
        public double DetailId
        {
            get { return (double)GetRefId(DetailIdProperty); }
            set { SetRefId(DetailIdProperty, value); }
        }

        /// <summary>
        /// 采集记录
        /// </summary>
        public static readonly RefEntityProperty<BatchWipRecord> DetailProperty = P<BatchWipProductProcessKeyItem>.RegisterRef(e => e.Detail, DetailIdProperty);

        /// <summary>
        /// 采集记录
        /// </summary>
        public BatchWipRecord Detail
        {
            get { return GetRefEntity(DetailProperty); }
            set { SetRefEntity(DetailProperty, value); }
        }
        #endregion

        #region 属性值 ValueList
        /// <summary>
        /// 属性值
        /// </summary>
        public static readonly ListProperty<EntityList<BatchKeyItemPropertyValue>> ValueListProperty = P<BatchWipProductProcessKeyItem>.RegisterList(e => e.ValueList);

        /// <summary>
        /// 属性值
        /// </summary>
        public EntityList<BatchKeyItemPropertyValue> ValueList
        {
            get { return this.GetLazyList(ValueListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<BatchWipProductProcessKeyItem>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<BatchWipProductProcessKeyItem>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<BatchWipProductProcessKeyItem>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BatchWipProductProcessKeyItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<BatchWipProductProcessKeyItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料描述 ItemDesc
        /// <summary>
        /// 物料描述
        /// </summary>
        [Label("物料描述")]
        public static readonly Property<string> ItemDescProperty = P<BatchWipProductProcessKeyItem>.RegisterView(e => e.ItemDesc, p => p.Item.Description);

        /// <summary>
        /// 物料描述
        /// </summary>
        public string ItemDesc
        {
            get { return this.GetProperty(ItemDescProperty); }
        }
        #endregion

        #region 物料单位 ItemNnitName
        /// <summary>
        /// 物料单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemNnitNameProperty = P<BatchWipProductProcessKeyItem>.RegisterView(e => e.ItemNnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemNnitName
        {
            get { return this.GetProperty(ItemNnitNameProperty); }
        }
        #endregion
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<BatchWipProductProcessKeyItem>.Register(e => e.ItemExtProp);

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
        public static readonly Property<string> ItemExtPropNameProperty = P<BatchWipProductProcessKeyItem>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品生产关键件 实体配置
    /// </summary>
    internal class BatchWipProductProcessKeyItemConfig : EntityConfig<BatchWipProductProcessKeyItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_KEY_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}