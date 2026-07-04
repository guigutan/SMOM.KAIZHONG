using SIE.Domain;
using SIE.Items;
using SIE.MES.SingleLabels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品生产关键件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品生产关键件")]
    public partial class WipProductProcessKeyItem : DataEntity
    {
        #region 用料数 Qty
        /// <summary>
        /// 用料数
        /// </summary>
        [Required]
        [Label("用料数")]
        public static readonly Property<decimal> QtyProperty = P<WipProductProcessKeyItem>.Register(e => e.Qty);

        /// <summary>
        /// 用料数
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 工序过站记录 Process
        /// <summary>
        /// 工序过站记录Id
        /// </summary>
        [Label("工序过站记录")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WipProductProcessKeyItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序过站记录Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序过站记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductProcess> ProcessProperty = P<WipProductProcessKeyItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序过站记录
        /// </summary>
        public WipProductProcess Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<WipProductProcessKeyItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<WipProductProcessKeyItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 来源条码 SourceCode
        /// <summary>
        /// 来源条码
        /// </summary>
        [Label("来源条码")]
        public static readonly Property<string> SourceCodeProperty = P<WipProductProcessKeyItem>.Register(e => e.SourceCode);

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
        [Label("来源ID")]
        public static readonly Property<double> SourceIdProperty = P<WipProductProcessKeyItem>.Register(e => e.SourceId);

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
        public static readonly Property<LoadItemSourceType> SourceTypeProperty = P<WipProductProcessKeyItem>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public LoadItemSourceType SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WipProductProcessKeyItem>.Register(e => e.ItemExtProp);

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
        public static readonly Property<string> ItemExtPropNameProperty = P<WipProductProcessKeyItem>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 单位用料数 UnitQty
        /// <summary>
        /// 用料数
        /// </summary>
        [Required]
        [Label("单位用料数")]
        public static readonly Property<decimal> UnitQtyProperty = P<WipProductProcessKeyItem>.Register(e => e.UnitQty);

        /// <summary>
        /// 单位用料数
        /// </summary>
        public decimal UnitQty
        {
            get { return GetProperty(UnitQtyProperty); }
            set { SetProperty(UnitQtyProperty, value); }
        }
        #endregion

        #region 是否解绑 IsUnbound
        /// <summary>
        /// 是否解绑
        /// </summary>
        [Label("是否解绑")]
        public static readonly Property<bool> IsUnboundProperty = P<WipProductProcessKeyItem>.Register(e => e.IsUnbound);

        /// <summary>
        /// 是否解绑
        /// </summary>
        public bool IsUnbound
        {
            get { return this.GetProperty(IsUnboundProperty); }
            set { this.SetProperty(IsUnboundProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.ProcessName, p => p.Process.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 工位 StationName
        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.StationName, p => p.Process.Station.Name);

        /// <summary>
        /// 工位
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料描述 ItemDescription
        /// <summary>
        /// 物料描述
        /// </summary>
        [Label("物料描述")]
        public static readonly Property<string> ItemDescriptionProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.ItemDescription, p => p.Item.Description);

        /// <summary>
        /// 物料描述
        /// </summary>
        public string ItemDescription
        {
            get { return this.GetProperty(ItemDescriptionProperty); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }
        #endregion

        #region 资源Id ResourceId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double> ResourceIdProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.ResourceId, p => p.Process.Resource.Id);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.ResourceName, p => p.Process.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 采集条码 Barcode
        /// <summary>
        /// 采集条码
        /// </summary>
        [Label("采集条码")]
        public static readonly Property<string> BarcodeProperty = P<WipProductProcessKeyItem>.RegisterView(e => e.Barcode, p => p.Process.Barcode);

        /// <summary>
        /// 采集条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 产品生产关键件 实体配置
    /// </summary>
    internal class WipProductProcessKeyItemConfig : EntityConfig<WipProductProcessKeyItem>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_KEY_ITEM").MapAllProperties();
            Meta.Property(WipProductProcessKeyItem.ProcessIdProperty).ColumnMeta.HasIndex();
            Meta.Property(WipProductProcessKeyItem.SourceCodeProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}