using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Boxs
{
    /// <summary>
	/// 产品容量
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("产品容量")]
    public partial class ProductCapacity : DataEntity
    {
        #region 容量 Capacity
        /// <summary>
        /// 容量
        /// </summary>
        [Label("容量")]
        [MinValue(1)]
        [MaxValue(1000000)]
        public static readonly Property<decimal> CapacityProperty = P<ProductCapacity>.Register(e => e.Capacity);

        /// <summary>
        /// 容量
        /// </summary>
        public decimal Capacity
        {
            get { return GetProperty(CapacityProperty); }
            set { SetProperty(CapacityProperty, value); }
        }
        #endregion

        #region 满箱运转 IsOperate
        /// <summary>
        /// 满箱运转
        /// </summary>
        [Label("满箱运转")]
        public static readonly Property<bool> IsOperateProperty = P<ProductCapacity>.Register(e => e.IsOperate);

        /// <summary>
        /// 满箱运转
        /// </summary>
        public bool IsOperate
        {
            get { return GetProperty(IsOperateProperty); }
            set { SetProperty(IsOperateProperty, value); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认
        /// </summary>
        [Label("是否默认")]
        public static readonly Property<bool> IsDefaultProperty = P<ProductCapacity>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ProductCapacity>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<ProductCapacity>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 周转箱 TurnoverBox
        /// <summary>
        /// 周转箱Id
        /// </summary>
        [Label("周转箱")]
        public static readonly IRefIdProperty TurnoverBoxIdProperty = P<ProductCapacity>.RegisterRefId(e => e.TurnoverBoxId, ReferenceType.Parent);

        /// <summary>
        /// 周转箱Id
        /// </summary>
        public double TurnoverBoxId
        {
            get { return (double)GetRefId(TurnoverBoxIdProperty); }
            set { SetRefId(TurnoverBoxIdProperty, value); }
        }

        /// <summary>
        /// 周转箱
        /// </summary>
        public static readonly RefEntityProperty<TurnoverBox> TurnoverBoxProperty = P<ProductCapacity>.RegisterRef(e => e.TurnoverBox, TurnoverBoxIdProperty);

        /// <summary>
        /// 周转箱
        /// </summary>
        public TurnoverBox TurnoverBox
        {
            get { return GetRefEntity(TurnoverBoxProperty); }
            set { SetRefEntity(TurnoverBoxProperty, value); }
        }
        #endregion

        #region 视图属性 用于Web

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ProductCapacity>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<ProductCapacity>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 产品容量 实体配置
    /// </summary>
    internal class ProductCapacityConfig : EntityConfig<ProductCapacity>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CNT_PRODUCT_CAP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}