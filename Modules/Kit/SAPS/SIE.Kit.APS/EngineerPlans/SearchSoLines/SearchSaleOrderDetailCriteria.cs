using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.SO.SaleOrders;
using System;

namespace SIE.Kit.APS.EngineerPlans.SearchSoLines
{
	/// <summary>
	/// 销售订单行查询
	/// </summary>
	[QueryEntity,Serializable]
	[Label("销售订单行查询")]
	public class SearchSaleOrderDetailCriteria : Criteria
    {
		#region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		public SearchSaleOrderDetailCriteria()
		{
			RegisterDateTime = new DateRange();
			RegisterDateTime.DateTimePart = DateTimePart.Date;  //选择日期格式为天
			RegisterDateTime.DateRangeType = DateRangeType.Month;  //默认日期为本月	


			RequireDelivery = new DateRange();
			RequireDelivery.DateTimePart = DateTimePart.Date;  //选择日期格式为天
			RequireDelivery.DateRangeType = DateRangeType.Month;  //默认日期为本月	
		}
		#endregion

		#region Code
		/// <summary>
		/// 销售订单编号
		/// </summary>
		[Label("销售订单编号")]
		public static readonly Property<string> CodeProperty = P<SearchSaleOrderDetailCriteria>.Register(e => e.Code);

		/// <summary>
		/// 销售订单编号
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 生产订单编号 ProductionOrderCode
		/// <summary>
		/// 生产订单编号
		/// </summary>
		[Required]
		[Label("生产订单编号")]
		public static readonly Property<string> TargetOrderCodeProperty = P<SearchSaleOrderDetailCriteria>.Register(e => e.TargetOrderCode);

		/// <summary>
		/// 生产订单编号
		/// </summary>
		public string TargetOrderCode
		{
			get { return GetProperty(TargetOrderCodeProperty); }
			set { SetProperty(TargetOrderCodeProperty, value); }
		}
		#endregion

		#region 行状态 LineState
		/// <summary>
		/// 行状态
		/// </summary>
		[Required]
		[Label("行状态")]
		public static readonly Property<LineState?> LineStateProperty = P<SearchSaleOrderDetailCriteria>.Register(e => e.LineState);

		/// <summary>
		/// 行状态
		/// </summary>
		public LineState? LineState
		{
			get { return GetProperty(LineStateProperty); }
			set { SetProperty(LineStateProperty, value); }
		}
		#endregion

		#region 物料 Item
		/// <summary>
		/// 物料Id
		/// </summary>
		public static readonly IRefIdProperty ItemIdProperty = P<SearchSaleOrderDetailCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

		/// <summary>
		/// 物料Id
		/// </summary>
		public double? ItemId
		{
			get { return (double)GetRefId(ItemIdProperty); }
			set { SetRefId(ItemIdProperty, value); }
		}

		/// <summary>
		/// 物料
		/// </summary>
		public static readonly RefEntityProperty<Item> ItemProperty = P<SearchSaleOrderDetailCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

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
		public static readonly Property<string> ItemCodeProperty = P<SearchSaleOrderDetailCriteria>.Register(e => e.ItemCode);

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
		public static readonly Property<string> ItemNameProperty = P<SearchSaleOrderDetailCriteria>.Register(e => e.ItemName);

		/// <summary>
		/// 物料名称
		/// </summary>
		public string ItemName
		{
			get { return this.GetProperty(ItemNameProperty); }
			set { SetProperty(ItemNameProperty, value); }
		}
		#endregion

		#region RegisterDateTime
		/// <summary>
		/// 登记时间
		/// </summary>
		[Label("登记时间")]
		public static readonly Property<DateRange> RegisterDateTimeProperty = P<SearchSaleOrderDetailCriteria>.Register(e => e.RegisterDateTime);

		/// <summary>
		/// 登记时间
		/// </summary>
		public DateRange RegisterDateTime
		{
			get { return GetProperty(RegisterDateTimeProperty); }
			set { SetProperty(RegisterDateTimeProperty, value); }
		}
		#endregion

		#region 客户交期 RequireDelivery
		/// <summary>
		/// 客户交期
		/// </summary>
		[Required]
		[Label("客户交期")]
		public static readonly Property<DateRange> RequireDeliveryProperty = P<SearchSaleOrderDetailCriteria>.Register(e => e.RequireDelivery);

		/// <summary>
		/// 客户交期
		/// </summary>
		public DateRange RequireDelivery
		{
			get { return GetProperty(RequireDeliveryProperty); }
			set { SetProperty(RequireDeliveryProperty, value); }
		}
		#endregion

		#region 客户 Customer
		/// <summary>
		/// 客户Id
		/// </summary>
		public static readonly IRefIdProperty CustomerIdProperty = P<SearchSaleOrderDetailCriteria>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

		/// <summary>
		/// 客户Id
		/// </summary>
		public double? CustomerId
		{
			get { return (double)GetRefId(CustomerIdProperty); }
			set { SetRefId(CustomerIdProperty, value); }
		}

		/// <summary>
		/// 客户
		/// </summary>
		public static readonly RefEntityProperty<Customer> CustomerProperty = P<SearchSaleOrderDetailCriteria>.RegisterRef(e => e.Customer, CustomerIdProperty);

		/// <summary>
		/// 客户
		/// </summary>
		public Customer Customer
		{
			get { return GetRefEntity(CustomerProperty); }
			set { SetRefEntity(CustomerProperty, value); }
		}
		#endregion

		#region 客户名称 CustomerName
		/// <summary>
		/// 客户名称
		/// </summary>
		[MaxLength(240)]
		[Label("客户名称")]
		public static readonly Property<string> CustomerNameProperty = P<SearchSaleOrderDetailCriteria>.Register(e => e.CustomerName);

		/// <summary>
		/// 客户名称
		/// </summary>
		public string CustomerName
		{
			get { return this.GetProperty(CustomerNameProperty); }
			set { SetProperty(CustomerNameProperty, value); }
		}
		#endregion

		/// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<EngineerPlanController>().DoSearchSoLine(this);
		}

	}
}
