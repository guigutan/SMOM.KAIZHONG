using SIE.Domain;
using SIE.Items;
using SIE.LES.Commons;
using SIE.ObjectModel;
using System;

namespace SIE.LES
{
    /// <summary>
    /// 备料模式查询实体
    /// </summary>
    [QueryEntity, Serializable]
	[Label("备料模式查询实体")]
	public partial class BasePrepareItemCriteria : Criteria
	{
		#region 固定量  FixedQuantity
		/// <summary>
		/// 固定量
		/// </summary>
		[Label("固定量")]
		public static readonly Property<decimal?> FixedQuantityProperty = P<BasePrepareItemCriteria>.Register(e => e.FixedQuantity);

		/// <summary>
		/// 固定量
		/// </summary>
		public decimal? FixedQuantity
		{
			get { return GetProperty(FixedQuantityProperty); }
			set { SetProperty(FixedQuantityProperty, value); }
		}
		#endregion

		#region 物料 Item
		/// <summary>
		/// 物料Id
		/// </summary>
		[Label("物料编码")]
		public static readonly IRefIdProperty ItemIdProperty = P<BasePrepareItemCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

		/// <summary>
		/// 物料Id
		/// </summary>
		public double? ItemId
		{
			get { return (double?)GetRefNullableId(ItemIdProperty); }
			set { SetRefNullableId(ItemIdProperty, value); }
		}

		/// <summary>
		/// 物料
		/// </summary>
		public static readonly RefEntityProperty<Item> ItemProperty = P<BasePrepareItemCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

		/// <summary>
		/// 物料
		/// </summary>
		public Item Item
		{
			get { return GetRefEntity(ItemProperty); }
			set { SetRefEntity(ItemProperty, value); }
		}
		#endregion

		#region 分类 ItemCategory
		/// <summary>
		/// 分类Id
		/// </summary>
		[Label("物料类型")]
		public static readonly IRefIdProperty ItemCategoryIdProperty = P<BasePrepareItemCriteria>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

		/// <summary>
		/// 分类Id
		/// </summary>
		public double? ItemCategoryId
		{
			get { return (double?)GetRefNullableId(ItemCategoryIdProperty); }
			set { SetRefNullableId(ItemCategoryIdProperty, value); }
		}

		/// <summary>
		/// 分类
		/// </summary>
		public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<BasePrepareItemCriteria>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

		/// <summary>
		/// 分类
		/// </summary>
		public ItemCategory ItemCategory
		{
			get { return GetRefEntity(ItemCategoryProperty); }
			set { SetRefEntity(ItemCategoryProperty, value); }
		}
		#endregion

		#region 物料名称 ItemName
		/// <summary>
		/// 物料名称
		/// </summary>
		[Label("物料名称")]
		public static readonly Property<string> ItemNameProperty = P<BasePrepareItemCriteria>.RegisterView(e => e.ItemName, p => p.Item.Name);

		/// <summary>
		/// 物料名称
		/// </summary>
		public string ItemName
		{
			get { return this.GetProperty(ItemNameProperty); }
		}
		#endregion

		#region 备料方式  PrepareItemType
		/// <summary>
		/// 备料方式
		/// </summary>
		[Label("备料方式")]
		public static readonly Property<PrepareItemType> PrepareItemTypeProperty = P<BasePrepareItemCriteria>.Register(e => e.PrepareItemType);

		/// <summary>
		/// 备料方式
		/// </summary>
		public PrepareItemType PrepareItemType
		{
			get { return GetProperty(PrepareItemTypeProperty); }
			set { SetProperty(PrepareItemTypeProperty, value); }
		}
		#endregion

		#region 触发方式 TriggerType
		/// <summary>
		/// 触发方式
		/// </summary>
		[Label("触发方式")]
		[Required]
		public static readonly Property<TriggerMode?> TriggerTypeProperty = P<BasePrepareItemCriteria>.Register(e => e.TriggerType);

		/// <summary>
		/// 触发方式
		/// </summary>
		public TriggerMode? TriggerType
		{
			get { return GetProperty(TriggerTypeProperty); }
			set { SetProperty(TriggerTypeProperty, value); }
		}
		#endregion

		#region 需求计算方式 DemandType
		/// <summary>
		/// 需求计算方式
		/// </summary>
		[Label("需求计算方式")]
		[Required]
		public static readonly Property<DemandMode?> DemandTypeProperty = P<BasePrepareItemCriteria>.Register(e => e.DemandType);

		/// <summary>
		/// 需求计算方式
		/// </summary>
		public DemandMode? DemandType
		{
			get { return GetProperty(DemandTypeProperty); }
			set { SetProperty(DemandTypeProperty, value); }
		}
		#endregion
	}
}