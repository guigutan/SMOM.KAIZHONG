using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.InboundOrders
{
	/// <summary>
	/// 工治具入库采购信息
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("工治具入库采购信息")]
	public partial class InboundOrderPurchase : DataEntity
	{
		#region 采购订单号 PoNo
		/// <summary>
		/// 采购订单号
		/// </summary>
		[Label("采购订单号")]
		public static readonly Property<string> PoNoProperty = P<InboundOrderPurchase>.Register(e => e.PoNo);

		/// <summary>
		/// 采购订单号
		/// </summary>
		public string PoNo
		{
			get { return GetProperty(PoNoProperty); }
			set { SetProperty(PoNoProperty, value); }
		}
		#endregion

		#region 行号 PoLine
		/// <summary>
		/// 行号
		/// </summary>
		[Label("行号")]
		public static readonly Property<string> PoLineProperty = P<InboundOrderPurchase>.Register(e => e.PoLine);

		/// <summary>
		/// 行号
		/// </summary>
		public string PoLine
		{
			get { return GetProperty(PoLineProperty); }
			set { SetProperty(PoLineProperty, value); }
		}
		#endregion

		#region 数量 Qty
		/// <summary>
		/// 数量
		/// </summary>
		[Label("数量")]
		public static readonly Property<decimal> QtyProperty = P<InboundOrderPurchase>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 单价 Price
		/// <summary>
		/// 单价
		/// </summary>
		[Label("单价")]
		public static readonly Property<decimal> PriceProperty = P<InboundOrderPurchase>.Register(e => e.Price);

		/// <summary>
		/// 单价
		/// </summary>
		public decimal Price
		{
			get { return GetProperty(PriceProperty); }
			set { SetProperty(PriceProperty, value); }
		}
		#endregion

		#region  InboundOrder
		/// <summary>
		/// Id
		/// </summary>
		public static readonly IRefIdProperty InboundOrderIdProperty = P<InboundOrderPurchase>.RegisterRefId(e => e.InboundOrderId, ReferenceType.Parent);

		/// <summary>
		/// Id
		/// </summary>
		public double InboundOrderId
		{
			get { return (double)GetRefId(InboundOrderIdProperty); }
			set { SetRefId(InboundOrderIdProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly RefEntityProperty<InboundOrder> InboundOrderProperty = P<InboundOrderPurchase>.RegisterRef(e => e.InboundOrder, InboundOrderIdProperty);

		/// <summary>
		/// 
		/// </summary>
		public InboundOrder InboundOrder
		{
			get { return GetRefEntity(InboundOrderProperty); }
			set { SetRefEntity(InboundOrderProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 工治具入库采购信息 实体配置
	/// </summary>
	internal class InboundOrderPurchaseConfig : EntityConfig<InboundOrderPurchase>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_FIXTURE_IN_PO").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}