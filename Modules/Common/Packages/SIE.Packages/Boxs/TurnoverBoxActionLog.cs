using SIE;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Boxs
{
	/// <summary>
	/// 操作日志
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("操作日志")]
	public partial class TurnoverBoxActionLog : DataEntity
	{
		#region 条码 Sn
		/// <summary>
		/// 条码
		/// </summary>
		[Label("条码")]
		public static readonly Property<string> SnProperty = P<TurnoverBoxActionLog>.Register(e => e.Sn);

		/// <summary>
		/// 条码
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
		#endregion

		#region 数量 Qty
		/// <summary>
		/// 数量
		/// </summary>
		[Label("数量")]
		public static readonly Property<decimal?> QtyProperty = P<TurnoverBoxActionLog>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public decimal? Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 周转箱 TurnoverBox
		/// <summary>
		/// 周转箱Id
		/// </summary>
		[Label("周转箱")]
		public static readonly IRefIdProperty TurnoverBoxIdProperty = P<TurnoverBoxActionLog>.RegisterRefId(e => e.TurnoverBoxId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<TurnoverBox> TurnoverBoxProperty = P<TurnoverBoxActionLog>.RegisterRef(e => e.TurnoverBox, TurnoverBoxIdProperty);

		/// <summary>
		/// 周转箱
		/// </summary>
		public TurnoverBox TurnoverBox
		{
			get { return GetRefEntity(TurnoverBoxProperty); }
			set { SetRefEntity(TurnoverBoxProperty, value); }
		}
		#endregion

		#region 周转操作类型 TurnoverType
		/// <summary>
		/// 周转操作类型
		/// </summary>
		[Label("周转操作类型")]
		public static readonly Property<TurnoverType> TurnoverTypeProperty = P<TurnoverBoxActionLog>.Register(e => e.TurnoverType);

		/// <summary>
		/// 周转操作类型
		/// </summary>
		public TurnoverType TurnoverType
		{
			get { return GetProperty(TurnoverTypeProperty); }
			set { SetProperty(TurnoverTypeProperty, value); }
		}
		#endregion

		#region 物料 Item
		/// <summary>
		/// 物料Id
		/// </summary>
		[Label("物料")]
		public static readonly IRefIdProperty ItemIdProperty = P<TurnoverBoxActionLog>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Item> ItemProperty = P<TurnoverBoxActionLog>.RegisterRef(e => e.Item, ItemIdProperty);

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
	/// 操作日志 实体配置
	/// </summary>
	internal class TurnoverBoxActionLogConfig : EntityConfig<TurnoverBoxActionLog>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("TurnoverBoxActionLog").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}