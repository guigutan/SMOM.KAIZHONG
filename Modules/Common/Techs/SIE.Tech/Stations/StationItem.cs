using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Stations
{
    /// <summary>
    /// 工位物料
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工位物料")]
    public partial class StationItem : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationItem()
        {
            Capacity = 1;
            Warning = 1;
        }

        #region 容量值 Capacity
        /// <summary>
        /// 容量值
        /// </summary>
        [Label("容量值")]
        [MinValue(0)]
        public static readonly Property<decimal> CapacityProperty = P<StationItem>.Register(e => e.Capacity);

        /// <summary>
        /// 容量值
        /// </summary>
        public decimal Capacity
        {
            get { return GetProperty(CapacityProperty); }
            set { SetProperty(CapacityProperty, value); }
        }
        #endregion

        #region 预警值 Warning
        /// <summary>
        /// 预警值
        /// </summary>
        [Label("预警值")]
        [MinValue(0)]
        public static readonly Property<decimal> WarningProperty = P<StationItem>.Register(e => e.Warning);

        /// <summary>
        /// 预警值
        /// </summary>
        public decimal Warning
        {
            get { return GetProperty(WarningProperty); }
            set { SetProperty(WarningProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<StationItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<StationItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        public static readonly IRefIdProperty StationIdProperty = P<StationItem>.RegisterRefId(e => e.StationId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Station> StationProperty = P<StationItem>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StationItem>.RegisterView(e => e.ItemCode, e => e.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<StationItem>.RegisterView(e => e.ItemName, e => e.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 工位物料 实体配置
    /// </summary>
    internal class StationItemConfig : EntityConfig<StationItem>
    {
        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_STATION_ITEM").MapAllProperties();
            Meta.Property(StationItem.StationIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}