using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Items
{
    /// <summary>
    /// 物料批次规则
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料批次规则")]
    public partial class ItemBatchRule : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemBatchRule()
        {
            Qty = 1;
        }

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        [MinValue(1)]
        public static readonly Property<decimal> QtyProperty = P<ItemBatchRule>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 批次规则 BatchRule
        /// <summary>
        /// 批次规则
        /// </summary>
        [Label("批次规则")]
        public static readonly Property<BatchRule?> BatchRuleProperty = P<ItemBatchRule>.Register(e => e.BatchRule);

        /// <summary>
        /// 批次规则
        /// </summary>
        public BatchRule? BatchRule
        {
            get { return GetProperty(BatchRuleProperty); }
            set { SetProperty(BatchRuleProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<ItemBatchRule>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemBatchRule>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 追溯方式 RetrospectType
        /// <summary>
        /// 追溯方式
        /// </summary>
        [Label("追溯方式")]
        public static readonly Property<RetrospectType> RetrospectTypeProperty = P<ItemBatchRule>.Register(e => e.RetrospectType);

        /// <summary>
        /// 追溯方式
        /// </summary>
        public RetrospectType RetrospectType
        {
            get { return GetProperty(RetrospectTypeProperty); }
            set { SetProperty(RetrospectTypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料批次规则 实体配置
    /// </summary>
    internal class ItemBatchRuleConfig : EntityConfig<ItemBatchRule>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_BATCH_RULE").MapAllProperties();
            Meta.Property(ItemBatchRule.ItemIdProperty).ColumnMeta.IgnoreFK();
            Meta.Property(ItemBatchRule.ItemIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}