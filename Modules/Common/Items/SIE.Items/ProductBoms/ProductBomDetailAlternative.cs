using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 替代料
    /// </summary>
    [ChildEntity, Serializable]
    [Label("替代料")]
    public partial class ProductBomDetailAlternative : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ProductBomDetailAlternative>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<ProductBomDetailAlternative>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 替代料 BomDetail
        /// <summary>
        /// 替代料Id
        /// </summary>
        [Label("替代料")]
        public static readonly IRefIdProperty BomDetailIdProperty = P<ProductBomDetailAlternative>.RegisterRefId(e => e.BomDetailId, ReferenceType.Parent);

        /// <summary>
        /// 替代料Id
        /// </summary>
        public double BomDetailId
        {
            get { return (double)GetRefId(BomDetailIdProperty); }
            set { SetRefId(BomDetailIdProperty, value); }
        }

        /// <summary>
        /// 替代料
        /// </summary>
        [Label("替代料")]
        public static readonly RefEntityProperty<ProductBomDetail> BomDetailProperty = P<ProductBomDetailAlternative>.RegisterRef(e => e.BomDetail, BomDetailIdProperty);

        /// <summary>
        /// 替代料
        /// </summary>
        public ProductBomDetail BomDetail
        {
            get { return GetRefEntity(BomDetailProperty); }
            set { SetRefEntity(BomDetailProperty, value); }
        }
        #endregion

        #region RegisterView注册视图属性(关联实体属性平铺显示) 

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ProductBomDetailAlternative>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<ProductBomDetailAlternative>.RegisterView(e => e.ItemName, p => p.Item.Name);

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
        public static readonly Property<string> ItemSpecificationModelProperty = P<ProductBomDetailAlternative>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 单位 ItemUnitCode
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitCodeProperty = P<ProductBomDetailAlternative>.RegisterView(e => e.ItemUnitCode, p => p.Item.Unit.Code);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitCode
        {
            get { return this.GetProperty(ItemUnitCodeProperty); }
        }
        #endregion

        #region 单位名称 ItemUnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> ItemUnitNameProperty = P<ProductBomDetailAlternative>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 替代料 实体配置
    /// </summary>
    internal class ProductBomDetailAlternativeConfig : EntityConfig<ProductBomDetailAlternative>
    {
        /// <summary>
        /// 对 Meta 属性的配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROD_BOM_DTL_ALT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}