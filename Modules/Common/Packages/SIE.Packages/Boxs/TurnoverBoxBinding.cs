using SIE;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Packages.Boxs
{
	/// <summary>
	/// 周转工具绑定明细
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("周转工具绑定明细")]
	public partial class TurnoverBoxBinding : DataEntity
	{
		#region 条码 Sn
		/// <summary>
		/// 条码
		/// </summary>
		[Label("条码")]
		public static readonly Property<string> SnProperty = P<TurnoverBoxBinding>.Register(e => e.Sn);

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
		public static readonly Property<string> QtyProperty = P<TurnoverBoxBinding>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public string Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 是否已解绑 IsUnbinding
		/// <summary>
		/// 是否已解绑
		/// </summary>
		[Label("是否已解绑")]
		public static readonly Property<bool> IsUnbindingProperty = P<TurnoverBoxBinding>.Register(e => e.IsUnbinding);

		/// <summary>
		/// 是否已解绑
		/// </summary>
		public bool IsUnbinding
		{
			get { return GetProperty(IsUnbindingProperty); }
			set { SetProperty(IsUnbindingProperty, value); }
		}
		#endregion

		#region 绑定时间 BindingDate
		/// <summary>
		/// 绑定时间
		/// </summary>
		[Label("绑定时间")]
		public static readonly Property<DateTime> BindingDateProperty = P<TurnoverBoxBinding>.Register(e => e.BindingDate);

		/// <summary>
		/// 绑定时间
		/// </summary>
		public DateTime BindingDate
		{
			get { return GetProperty(BindingDateProperty); }
			set { SetProperty(BindingDateProperty, value); }
		}
		#endregion

		#region 解绑时间 UnBindingDate
		/// <summary>
		/// 解绑时间
		/// </summary>
		[Label("解绑时间")]
		public static readonly Property<DateTime?> UnBindingDateProperty = P<TurnoverBoxBinding>.Register(e => e.UnBindingDate);

		/// <summary>
		/// 解绑时间
		/// </summary>
		public DateTime? UnBindingDate
		{
			get { return GetProperty(UnBindingDateProperty); }
			set { SetProperty(UnBindingDateProperty, value); }
		}
		#endregion

		#region 是否绑定完成 IsBindFinish
		/// <summary>
		/// 是否绑定完成
		/// </summary>
		[Label("是否绑定完成")]
		public static readonly Property<bool> IsBindFinishProperty = P<TurnoverBoxBinding>.Register(e => e.IsBindFinish);

		/// <summary>
		/// 是否绑定完成
		/// </summary>
		public bool IsBindFinish
		{
			get { return GetProperty(IsBindFinishProperty); }
			set { SetProperty(IsBindFinishProperty, value); }
		}
		#endregion

		#region 物料 Item
		/// <summary>
		/// 物料Id
		/// </summary>
		[Label("物料")]
		public static readonly IRefIdProperty ItemIdProperty = P<TurnoverBoxBinding>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Item> ItemProperty = P<TurnoverBoxBinding>.RegisterRef(e => e.Item, ItemIdProperty);

		/// <summary>
		/// 物料
		/// </summary>
		public Item Item
		{
			get { return GetRefEntity(ItemProperty); }
			set { SetRefEntity(ItemProperty, value); }
		}
		#endregion

		#region 条码类型 BarcodeType
		/// <summary>
		/// 条码类型
		/// </summary>
		[Label("条码类型")]
		public static readonly Property<BarcodeType> BarcodeTypeProperty = P<TurnoverBoxBinding>.Register(e => e.BarcodeType);

		/// <summary>
		/// 条码类型
		/// </summary>
		public BarcodeType BarcodeType
		{
			get { return GetProperty(BarcodeTypeProperty); }
			set { SetProperty(BarcodeTypeProperty, value); }
		}
		#endregion

		#region 绑定操作员 BindingOperator
		/// <summary>
		/// 绑定操作员Id
		/// </summary>
		[Label("绑定操作员")]
		public static readonly IRefIdProperty BindingOperatorIdProperty = P<TurnoverBoxBinding>.RegisterRefId(e => e.BindingOperatorId, ReferenceType.Normal);

		/// <summary>
		/// 绑定操作员Id
		/// </summary>
		public double BindingOperatorId
		{
			get { return (double)GetRefId(BindingOperatorIdProperty); }
			set { SetRefId(BindingOperatorIdProperty, value); }
		}

		/// <summary>
		/// 绑定操作员
		/// </summary>
		public static readonly RefEntityProperty<Employee> BindingOperatorProperty = P<TurnoverBoxBinding>.RegisterRef(e => e.BindingOperator, BindingOperatorIdProperty);

		/// <summary>
		/// 绑定操作员
		/// </summary>
		public Employee BindingOperator
		{
			get { return GetRefEntity(BindingOperatorProperty); }
			set { SetRefEntity(BindingOperatorProperty, value); }
		}
		#endregion

		#region 周转箱 TurnoverBox
		/// <summary>
		/// 周转箱Id
		/// </summary>
		[Label("周转箱")]
		public static readonly IRefIdProperty TurnoverBoxIdProperty = P<TurnoverBoxBinding>.RegisterRefId(e => e.TurnoverBoxId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<TurnoverBox> TurnoverBoxProperty = P<TurnoverBoxBinding>.RegisterRef(e => e.TurnoverBox, TurnoverBoxIdProperty);

		/// <summary>
		/// 周转箱
		/// </summary>
		public TurnoverBox TurnoverBox
		{
			get { return GetRefEntity(TurnoverBoxProperty); }
			set { SetRefEntity(TurnoverBoxProperty, value); }
		}
		#endregion

		#region 解绑操作员 UnbindingOperator
		/// <summary>
		/// 解绑操作员Id
		/// </summary>
		[Label("解绑操作员")]
		public static readonly IRefIdProperty UnbindingOperatorIdProperty = P<TurnoverBoxBinding>.RegisterRefId(e => e.UnbindingOperatorId, ReferenceType.Normal);

		/// <summary>
		/// 解绑操作员Id
		/// </summary>
		public double? UnbindingOperatorId
		{
			get { return (double?)GetRefNullableId(UnbindingOperatorIdProperty); }
			set { SetRefNullableId(UnbindingOperatorIdProperty, value); }
		}

		/// <summary>
		/// 解绑操作员
		/// </summary>
		public static readonly RefEntityProperty<Employee> UnbindingOperatorProperty = P<TurnoverBoxBinding>.RegisterRef(e => e.UnbindingOperator, UnbindingOperatorIdProperty);

		/// <summary>
		/// 解绑操作员
		/// </summary>
		public Employee UnbindingOperator
		{
			get { return GetRefEntity(UnbindingOperatorProperty); }
			set { SetRefEntity(UnbindingOperatorProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 周转工具绑定明细 实体配置
	/// </summary>
	internal class TurnoverBoxBindingConfig : EntityConfig<TurnoverBoxBinding>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("TurnoverBoxBinding").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}