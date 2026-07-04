using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件库存查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备件库存查询实体")]
    public partial class StoreSummaryCriteria : Criteria
    {
        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty = P<StoreSummaryCriteria>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<StoreSummaryCriteria>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 库位 SparePartSite
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> SparePartSiteProperty = P<StoreSummaryCriteria>.Register(e => e.SparePartSite);

        /// <summary>
        /// 库位
        /// </summary>
        public string SparePartSite
        {
            get { return GetProperty(SparePartSiteProperty); }
            set { SetProperty(SparePartSiteProperty, value); }
        }
        #endregion

        #region 序列号 OrderNumber
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> OrderNumberProperty = P<StoreSummaryCriteria>.Register(e => e.OrderNumber);

        /// <summary>
        /// 序列号
        /// </summary>
        public string OrderNumber
        {
            get { return GetProperty(OrderNumberProperty); }
            set { SetProperty(OrderNumberProperty, value); }
        }
        #endregion

        #region 分类层级 ItemCategory
        /// <summary>
        /// 分类层级Id
        /// </summary>
        [Label("分类层级")]
        public static readonly IRefIdProperty ItemCategoryIdProperty =
            P<StoreSummaryCriteria>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 分类层级Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)this.GetRefNullableId(ItemCategoryIdProperty); }
            set { this.SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 分类层级
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty =
            P<StoreSummaryCriteria>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类层级
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return this.GetRefEntity(ItemCategoryProperty); }
            set { this.SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<StoreSummaryCriteria>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return GetProperty(SparePartCodeProperty); }
            set { SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<StoreSummaryCriteria>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return GetProperty(SparePartNameProperty); }
            set { SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
		{
            return RT.Service.Resolve<SparePartController>().GetStoreSummary(this);
        }
    }
}
