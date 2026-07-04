using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位物料点检
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位物料点检")]
    public class CheckItem : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<CheckItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<CheckItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 需求数量 DemandQty
        /// <summary>
        /// 需求数量
        /// </summary>
        [Label("需求数量")]
        public static readonly Property<decimal> DemandQtyProperty = P<CheckItem>.Register(e => e.DemandQty);

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal DemandQty
        {
            get { return this.GetProperty(DemandQtyProperty); }
            set { this.SetProperty(DemandQtyProperty, value); }
        }
        #endregion

        #region 到位数量 ArriveQty
        /// <summary>
        /// 到位数量
        /// </summary>
        [Label("到位数量")]
        public static readonly Property<decimal> ArriveQtyProperty = P<CheckItem>.Register(e => e.ArriveQty);

        /// <summary>
        /// 到位数量
        /// </summary>
        public decimal ArriveQty
        {
            get { return this.GetProperty(ArriveQtyProperty); }
            set { this.SetProperty(ArriveQtyProperty, value); }
        }
        #endregion

        #region 缺料数量 LackQty
        /// <summary>
        /// 缺料数量
        /// </summary>
        [Label("缺料数量")]
        public static readonly Property<decimal> LackQtyProperty = P<CheckItem>.Register(e => e.LackQty);

        /// <summary>
        /// 缺料数量
        /// </summary>
        public decimal LackQty
        {
            get { return this.GetProperty(LackQtyProperty); }
            set { this.SetProperty(LackQtyProperty, value); }
        }
        #endregion

        #region 预警值 WarnQty
        /// <summary>
        /// 预警值
        /// </summary>
        [Label("预警值")]
        public static readonly Property<decimal> WarnQtyProperty = P<CheckItem>.Register(e => e.WarnQty);

        /// <summary>
        /// 预警值
        /// </summary>
        public decimal WarnQty
        {
            get { return this.GetProperty(WarnQtyProperty); }
            set { this.SetProperty(WarnQtyProperty, value); }
        }
        #endregion

        #region 配送状态 State
        /// <summary>
        /// 配送状态
        /// </summary>
        [Label("配送状态")]
        public static readonly Property<DistState> StateProperty = P<CheckItem>.Register(e => e.State);

        /// <summary>
        /// 配送状态
        /// </summary>
        public DistState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 在途数量 InRouteQty
        /// <summary>
        /// 在途数量
        /// </summary>
        [Label("在途数量")]
        public static readonly Property<decimal> InRouteQtyProperty = P<CheckItem>.Register(e => e.InRouteQty);

        /// <summary>
        /// 在途数量
        /// </summary>
        public decimal InRouteQty
        {
            get { return this.GetProperty(InRouteQtyProperty); }
            set { this.SetProperty(InRouteQtyProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<CheckItem>.RegisterRefId(e => e.StationId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Station> StationProperty =
            P<CheckItem>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 工位物料点检实体配置
    /// </summary>
    internal class CheckItemEntityConfig : EntityConfig<CheckItem>
    {
        /// <summary>
        /// 配置实体元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CHK_STATION_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 配送状态
    /// </summary>
    public enum DistState
    {
        /// <summary>
        /// 配送完成
        /// </summary>
        [Label("配送完成")]
        Finish,

        /// <summary>
        /// 已发料
        /// </summary>
        [Label("已发料")]
        Issued,

        /// <summary>
        /// 未发料
        /// </summary>
        [Label("未发料")]
        NotIssue
    }

    /// <summary>
    /// 工位扩展物料点检列表
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StationExtCheckItemListProperty
    {
        /// <summary>
        /// 工位物料点检扩展列表属性
        /// </summary>
        public static readonly ListProperty<EntityList<CheckItem>> StationCheckItemListProperty =
            P<Station>.RegisterExtensionList<EntityList<CheckItem>>("StationCheckItemList", typeof(StationExtCheckItemListProperty));

        /// <summary>
        /// 工位物料点检扩展列表属性
        /// </summary>
        /// <param name="me">工位对象</param>
        /// <returns>工位物料点检列表</returns>
        public static EntityList<CheckItem> GetStationCheckItemList(Station me)
        {
            return me.GetProperty(StationCheckItemListProperty);
        }

        /// <summary>
        /// 工位物料点检扩展列表属性
        /// </summary>
        /// <param name="me">工位</param>
        /// <param name="value">工位物料点检列表</param>
        public static void SetStationCheckItemList(Station me, EntityList<CheckItem> value)
        {
            me.SetProperty(StationCheckItemListProperty, value);
        }

        /// <summary>
        /// 工位物料点检列表实体配置
        /// </summary>
        internal class StationExtCheckItemListPropertyConfig : EntityConfig<Station>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(StationCheckItemListProperty).DontMapColumn();
            }
        }
    }
}