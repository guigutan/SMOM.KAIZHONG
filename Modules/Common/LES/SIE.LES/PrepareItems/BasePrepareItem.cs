using SIE.Domain;
using SIE.Items;
using SIE.LES.Commons;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES
{
	/// <summary>
	/// 备料模式
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("备料模式")]
	public partial class BasePrepareItem : DataEntity
	{
		#region 固定量  FixedQuantity
		/// <summary>
		/// 固定量
		/// </summary>
		[Label("固定量")]
		[MinValue(0)]
		public static readonly Property<decimal?> FixedQuantityProperty = P<BasePrepareItem>.Register(e => e.FixedQuantity);

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
		public static readonly IRefIdProperty ItemIdProperty = P<BasePrepareItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Item> ItemProperty = P<BasePrepareItem>.RegisterRef(e => e.Item, ItemIdProperty);

		/// <summary>
		/// 物料
		/// </summary>
		public Item Item
		{
			get { return GetRefEntity(ItemProperty); }
			set { SetRefEntity(ItemProperty, value); }
		}
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BasePrepareItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 分类 ItemCategory
        /// <summary>
        /// 分类Id
        /// </summary>
        [Label("物料类型")]
		public static readonly IRefIdProperty ItemCategoryIdProperty = P<BasePrepareItem>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<BasePrepareItem>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

		/// <summary>
		/// 分类
		/// </summary>
		public ItemCategory ItemCategory
		{
			get { return GetRefEntity(ItemCategoryProperty); }
			set { SetRefEntity(ItemCategoryProperty, value); }
		}
		#endregion

		#region 物料类型 ItemCategoryCode
		/// <summary>
		/// 物料类型
		/// </summary>
		[Label("物料类型")]
        public static readonly Property<string> NameProperty = P<BasePrepareItem>.RegisterView(e => e.ItemCategoryCode, p => p.ItemCategory.Code);

        /// <summary>
        /// 注释
        /// </summary>
        public string ItemCategoryCode
		{
            get { return this.GetProperty(NameProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
		public static readonly Property<string> ItemNameProperty = P<BasePrepareItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

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
		public static readonly Property<PrepareItemType> PrepareItemTypeProperty = P<BasePrepareItem>.Register(e => e.PrepareItemType);

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
		public static readonly Property<TriggerMode?> TriggerTypeProperty = P<BasePrepareItem>.Register(e => e.TriggerType);

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
		public static readonly Property<DemandMode?> DemandTypeProperty = P<BasePrepareItem>.Register(e => e.DemandType);

		/// <summary>
		/// 需求计算方式
		/// </summary>
		public DemandMode? DemandType
		{
			get { return GetProperty(DemandTypeProperty); }
			set { SetProperty(DemandTypeProperty, value); }
		}
		#endregion

		#region 物料扩展属性 ItemExtProp
		/// <summary>
		/// 物料扩展属性
		/// </summary>
		[Label("物料扩展属性")]
		public static readonly Property<string> ItemExtPropProperty = P<BasePrepareItem>.Register(e => e.ItemExtProp);

		/// <summary>
		/// 物料扩展属性
		/// </summary>
		public string ItemExtProp
		{
			get { return this.GetProperty(ItemExtPropProperty); }
			set { this.SetProperty(ItemExtPropProperty, value); }
		}
		#endregion

		#region 物料扩展属性 ItemExtPropName
		/// <summary>
		/// 物料扩展属性
		/// </summary>
		[Label("物料扩展属性名称")]
		public static readonly Property<string> ItemExtPropNameProperty = P<BasePrepareItem>.Register(e => e.ItemExtPropName);

		/// <summary>
		/// 物料扩展属性
		/// </summary>
		public string ItemExtPropName
		{
			get { return this.GetProperty(ItemExtPropNameProperty); }
			set { this.SetProperty(ItemExtPropNameProperty, value); }
		}
		#endregion

		#region 是否允许编辑物料扩展属性 IsAllowEdit
		/// <summary>
		/// 是否允许编辑物料扩展属性
		/// </summary>
		[Label("是否允许编辑物料扩展属性")]
		public static readonly Property<bool> IsAllowEditProperty = P<BasePrepareItem>.Register(e => e.IsAllowEdit);

		/// <summary>
		/// 是否允许编辑物料扩展属性
		/// </summary>
		public bool IsAllowEdit
		{
			get { return this.GetProperty(IsAllowEditProperty); }
			set { this.SetProperty(IsAllowEditProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 备料模式 实体配置
	/// </summary>
	internal class BasePrepareItemConfig : EntityConfig<BasePrepareItem>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("PREPARE_ITEM").MapAllPropertiesExcept(BasePrepareItem.IsAllowEditProperty);
			Meta.EnablePhantoms();
		}
	}
}