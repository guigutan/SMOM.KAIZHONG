using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 专储物料清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("专储物料清单")]
    public partial class StorageLocationItemList : DataEntity
    {
        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<StorageLocationItemList>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<StorageLocationItemList>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<StorageLocationItemList>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<StorageLocationItemList>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Parent);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)GetRefId(StorageLocationIdProperty); }
            set { SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StorageLocationItemList>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        [MaxLength(80)]
        public static readonly Property<string> ItemNameProperty = P<StorageLocationItemList>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<StorageLocationItemList>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StorageLocationItemList>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 单位 ItemUnit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitProperty = P<StorageLocationItemList>.RegisterView(e => e.ItemUnit, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnit
        {
            get { return this.GetProperty(ItemUnitProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 库位专储物料清单 实体配置
    /// </summary>
    internal class StorageLocationItemListConfig : EntityConfig<StorageLocationItemList>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_LOCATION_ITEM").MapAllProperties();
            Meta.Property(StorageLocationItemList.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
