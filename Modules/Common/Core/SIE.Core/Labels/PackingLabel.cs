using SIE.Core.Items;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Labels
{
    /// <summary>
	/// 标签条码
	/// </summary>
	[RootEntity, Serializable]
    [Label("标签条码")]
    public partial class PackingLabel : DataEntity
    {
        #region 条码 No
        /// <summary>
        /// 条码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("条码")]
        public static readonly Property<string> NoProperty = P<PackingLabel>.Register(e => e.No);

        /// <summary>
        /// 条码
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 数量(主) Qty
        /// <summary>
        /// 数量(主)
        /// </summary>
        [Label("数量(主)")]
        public static readonly Property<decimal> QtyProperty = P<PackingLabel>.Register(e => e.Qty);

        /// <summary>
        /// 数量(主)
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotProperty = P<PackingLabel>.Register(e => e.Lot);

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot
        {
            get { return GetProperty(LotProperty); }
            set { SetProperty(LotProperty, value); }
        }
        #endregion

        #region 生产日期 ProductionDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<PackingLabel>.Register(e => e.ProductionDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return GetProperty(ProductionDateProperty); }
            set { SetProperty(ProductionDateProperty, value); }
        }
        #endregion

        #region 失效日期 InvalidDate
        /// <summary>
        /// 失效日期
        /// </summary>
        [Label("失效日期")]
        public static readonly Property<DateTime?> InvalidDateProperty = P<PackingLabel>.Register(e => e.InvalidDate);

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? InvalidDate
        {
            get { return GetProperty(InvalidDateProperty); }
            set { SetProperty(InvalidDateProperty, value); }
        }
        #endregion

        #region 生产批次 ProductBatch
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> ProductBatchProperty = P<PackingLabel>.Register(e => e.ProductBatch);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductBatch
        {
            get { return GetProperty(ProductBatchProperty); }
            set { SetProperty(ProductBatchProperty, value); }
        }
        #endregion

        #region 上级条码 PackageNo
        /// <summary>
        /// 上级条码
        /// </summary>
        [Label("上级条码")]
        public static readonly Property<string> PackageNoProperty = P<PackingLabel>.Register(e => e.PackageNo);

        /// <summary>
        /// 上级条码
        /// </summary>
        public string PackageNo
        {
            get { return GetProperty(PackageNoProperty); }
            set { SetProperty(PackageNoProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<PackingLabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<PackingLabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 标签条码 实体配置
    /// </summary>
    internal class PackingLabelConfig : EntityConfig<PackingLabel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PACK_LABEL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }
}