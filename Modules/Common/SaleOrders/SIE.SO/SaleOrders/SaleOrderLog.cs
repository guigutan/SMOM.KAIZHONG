using SIE.Core.Items;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 日志
    /// </summary>
    [ChildEntity, Serializable]
	[Label("日志")]
	public class SaleOrderLog : DataEntity
    {
		#region 行号 LineNo
		/// <summary>
		/// 行号
		/// </summary>
		[Label("行号")]
		public static readonly Property<string> LineNoProperty = P<SaleOrderLog>.Register(e => e.LineNo);

		/// <summary>
		/// 行号
		/// </summary>
		public string LineNo
		{
			get { return GetProperty(LineNoProperty); }
			set { SetProperty(LineNoProperty, value); }
		}
		#endregion

		#region 修改项 UpdateItem
		/// <summary>
		/// 修改项
		/// </summary>
		[Label("修改项")]
		public static readonly Property<string> UpdateItemProperty = P<SaleOrderLog>.Register(e => e.UpdateItem);

		/// <summary>
		/// 修改项
		/// </summary>
		public string UpdateItem
		{
			get { return GetProperty(UpdateItemProperty); }
			set { SetProperty(UpdateItemProperty, value); }
		}
		#endregion

		#region 修改前 ModifyBefore
		/// <summary>
		/// 修改前
		/// </summary>
		[Label("修改前")]
		public static readonly Property<string> ModifyBeforeProperty = P<SaleOrderLog>.Register(e => e.ModifyBefore);

		/// <summary>
		/// 修改前
		/// </summary>
		public string ModifyBefore
		{
			get { return GetProperty(ModifyBeforeProperty); }
			set { SetProperty(ModifyBeforeProperty, value); }
		}
		#endregion

		#region 修改后 ModifyAfter
		/// <summary>
		/// 修改后
		/// </summary>
		[Label("修改后")]
		public static readonly Property<string> ModifyAfterProperty = P<SaleOrderLog>.Register(e => e.ModifyAfter);

		/// <summary>
		/// 修改后
		/// </summary>
		public string ModifyAfter
		{
			get { return GetProperty(ModifyAfterProperty); }
			set { SetProperty(ModifyAfterProperty, value); }
		}
		#endregion

		#region 物料 Item
		/// <summary>
		/// 物料Id
		/// </summary>
		public static readonly IRefIdProperty ItemIdProperty = P<SaleOrderLog>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Item> ItemProperty = P<SaleOrderLog>.RegisterRef(e => e.Item, ItemIdProperty);

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
		public static readonly Property<string> ItemCodeProperty = P<SaleOrderLog>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
		[MaxLength(240)]
		[Label("物料名称")]
		public static readonly Property<string> ItemNameProperty = P<SaleOrderLog>.RegisterView(e => e.ItemName, p => p.Item.Name);

		/// <summary>
		/// 物料名称
		/// </summary>
		public string ItemName
		{
			get { return this.GetProperty(ItemNameProperty); }
			set { SetProperty(ItemNameProperty, value); }
		}
		#endregion

		#region 销售订单 SaleOrder
		/// <summary>
		/// 销售订单Id
		/// </summary>
		public static readonly IRefIdProperty SaleOrderIdProperty = P<SaleOrderLog>.RegisterRefId(e => e.SaleOrderId, ReferenceType.Parent);

		/// <summary>
		/// 销售订单Id
		/// </summary>
		public double SaleOrderId
		{
			get { return (double)GetRefId(SaleOrderIdProperty); }
			set { SetRefId(SaleOrderIdProperty, value); }
		}

		/// <summary>
		/// 销售订单
		/// </summary>
		public static readonly RefEntityProperty<SaleOrder> SaleOrderProperty = P<SaleOrderLog>.RegisterRef(e => e.SaleOrder, SaleOrderIdProperty);

		/// <summary>
		/// 销售订单
		/// </summary>
		public SaleOrder SaleOrder
		{
			get { return GetRefEntity(SaleOrderProperty); }
			set { SetRefEntity(SaleOrderProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 日志 实体配置
	/// </summary>
	internal class SaleOrderLogConfig : EntityConfig<SaleOrderLog>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("SALE_ORDER_LOG").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}
