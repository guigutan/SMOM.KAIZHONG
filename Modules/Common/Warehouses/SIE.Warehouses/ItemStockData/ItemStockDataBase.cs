using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
	/// 库存资料
	/// </summary>
	[RootEntity, Serializable]
    [Label("库存资料-公共")]
    public partial class ItemStockDataBase : DataEntity
    { 
        #region 序列号管理 IsSerialNumber
        /// <summary>
        /// 序列号管理
        /// </summary>
        [Label("序列号管理")]
        public static readonly Property<bool?> IsSerialNumberProperty = P<ItemStockDataBase>.Register(e => e.IsSerialNumber);

        /// <summary>
        /// 序列号管理
        /// </summary>
        public bool? IsSerialNumber
        {
            get { return GetProperty(IsSerialNumberProperty); }
            set { SetProperty(IsSerialNumberProperty, value); }
        }
        #endregion
        
        #region 批次管理 IsBatch
        /// <summary>
        /// 批次管理
        /// </summary>
        [Label("批次管理")]
        public static readonly Property<bool?> IsBatchProperty = P<ItemStockDataBase>.Register(e => e.IsBatch);

        /// <summary>
        /// 批次管理
        /// </summary>
        public bool? IsBatch
        {
            get { return GetProperty(IsBatchProperty); }
            set { SetProperty(IsBatchProperty, value); }
        }
        #endregion                

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [NotDuplicate]
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemStockDataBase>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemStockDataBase>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 库存资料 实体配置
    /// </summary>
    internal class ItemStockDataBaseConfig : EntityConfig<ItemStockDataBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_STOCK_DATA").MapAllProperties();
            Meta.Property(ItemStockDataBase.ItemIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();        
        }
    }    
}