using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 线边仓盘点范围
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("线边仓盘点范围")]
    public partial class LesStockCountRange : DataEntity
    {
        #region 仓库 Warehouses
        /// <summary>
        /// 仓库
        /// </summary>
        [MaxLength(2000)]
        [Label("仓库")]
        public static readonly Property<string> WarehousesProperty = P<LesStockCountRange>.Register(e => e.Warehouses);

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouses
        {
            get { return GetProperty(WarehousesProperty); }
            set { SetProperty(WarehousesProperty, value); }
        }
        #endregion

        #region 仓库ID WarehousesIds
        /// <summary>
        /// 仓库ID
        /// </summary>
        [Label("仓库ID")]
        [MaxLength(4000)]
        public static readonly Property<string> WarehousesIdsProperty = P<LesStockCountRange>.Register(e => e.WarehousesIds);

        /// <summary>
        /// 仓库ID
        /// </summary>
        public string WarehousesIds
        {
            get { return this.GetProperty(WarehousesIdsProperty); }
            set { this.SetProperty(WarehousesIdsProperty, value); }
        }
        #endregion

        #region 物料分类 ItemCategorys
        /// <summary>
        /// 物料分类
        /// </summary>
        [MaxLength(2000)]
        [Label("物料分类")]
        public static readonly Property<string> ItemCategorysProperty = P<LesStockCountRange>.Register(e => e.ItemCategorys);

        /// <summary>
        /// 物料分类
        /// </summary>
        public string ItemCategorys
        {
            get { return GetProperty(ItemCategorysProperty); }
            set { SetProperty(ItemCategorysProperty, value); }
        }
        #endregion

        #region 物料编码 Items
        /// <summary>
        /// 物料编码
        /// </summary>
        [MaxLength(2000)]
        [Label("物料编码")]
        public static readonly Property<string> ItemsProperty = P<LesStockCountRange>.Register(e => e.Items);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string Items
        {
            get { return GetProperty(ItemsProperty); }
            set { SetProperty(ItemsProperty, value); }
        }
        #endregion

        #region 盘点单 LesStockCount
        /// <summary>
        /// 盘点单Id
        /// </summary>
        [Label("盘点单")]
        public static readonly IRefIdProperty LesStockCountIdProperty = P<LesStockCountRange>.RegisterRefId(e => e.LesStockCountId, ReferenceType.Parent);

        /// <summary>
        /// 盘点单Id
        /// </summary>
        public double LesStockCountId
        {
            get { return (double)GetRefId(LesStockCountIdProperty); }
            set { SetRefId(LesStockCountIdProperty, value); }
        }

        /// <summary>
        /// 盘点单
        /// </summary>
        public static readonly RefEntityProperty<LesStockCount> StockCountProperty = P<LesStockCountRange>.RegisterRef(e => e.LesStockCount, LesStockCountIdProperty);

        /// <summary>
        /// 盘点单
        /// </summary>
        public LesStockCount LesStockCount
        {
            get { return GetRefEntity(StockCountProperty); }
            set { SetRefEntity(StockCountProperty, value); }
        }
        #endregion

        #region 盲盘 IsBlindCount
        /// <summary>
        /// 盲盘
        /// </summary>
        [Label("盲盘")]
        public static readonly Property<bool> IsBlindCountProperty = P<LesStockCountRange>.Register(e => e.IsBlindCount);

        /// <summary>
        /// 盲盘
        /// </summary>
        public bool IsBlindCount
        {
            get { return GetProperty(IsBlindCountProperty); }
            set { SetProperty(IsBlindCountProperty, value); }
        }
        #endregion

        #region 盘点细度 CountDimension
        /// <summary>
        /// 盘点细度
        /// </summary>
        [Label("盘点细度")]
        public static readonly Property<CountDimension> CountDimensionProperty = P<LesStockCountRange>.Register(e => e.CountDimension);

        /// <summary>
        /// 盘点细度
        /// </summary>
        public CountDimension CountDimension
        {
            get { return GetProperty(CountDimensionProperty); }
            set { SetProperty(CountDimensionProperty, value); }
        }
        #endregion

        #region 物料消耗类型 ConsumeMode
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        [Label("物料消耗类型")]
        public static readonly Property<ConsumeMode?> ConsumeModeProperty = P<LesStockCountRange>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode? ConsumeMode
        {
            get { return GetProperty(ConsumeModeProperty); }
            set { SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("盘点状态")]
        public static readonly Property<LesCountState> StateProperty = P<LesStockCountRange>.RegisterView(e => e.State, p => p.LesStockCount.State);

        /// <summary>
        /// 状态
        /// </summary>
        public LesCountState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 动态盘点 IsDynamicOnhand
        /// <summary>
        /// 动态盘点
        /// </summary>
        [Label("动态盘点")]
        public static readonly Property<bool> IsDynamicOnhandProperty = P<LesStockCountRange>.Register(e => e.IsDynamicOnhand);

        /// <summary>
        /// 动态盘点
        /// </summary>
        public bool IsDynamicOnhand
        {
            get { return GetProperty(IsDynamicOnhandProperty); }
            set { SetProperty(IsDynamicOnhandProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 盘点范围 实体配置
    /// </summary>
    internal class LesStockCountRangeConfig : EntityConfig<LesStockCountRange>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_STOCK_COUNT_RANGE").MapAllProperties();
            Meta.Property(LesStockCountRange.WarehousesProperty).ColumnMeta.HasLength(4000);           
            Meta.Property(LesStockCountRange.ItemCategorysProperty).ColumnMeta.HasLength(4000);
            Meta.Property(LesStockCountRange.ItemsProperty).ColumnMeta.HasLength(4000);          
            Meta.EnablePhantoms();
        }
    }
}