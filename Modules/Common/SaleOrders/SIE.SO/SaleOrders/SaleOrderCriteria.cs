using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单查询实体
    /// </summary>
    [QueryEntity, Serializable]
	[Label("销售订单查询")]
	public class SaleOrderCriteria : Criteria
	{
		#region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		public SaleOrderCriteria()
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
		public static readonly Property<string> CodeProperty = P<SaleOrderCriteria>.Register(e => e.Code);

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
		[Label("生产订单编号")]
		public static readonly Property<string> TargetOrderCodeProperty = P<SaleOrderCriteria>.Register(e => e.TargetOrderCode);

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
		[Label("行状态")]
		public static readonly Property<LineState?> LineStateProperty = P<SaleOrderCriteria>.Register(e => e.LineState);

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
		public static readonly IRefIdProperty ItemIdProperty = P<SaleOrderCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Item> ItemProperty = P<SaleOrderCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

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
		public static readonly Property<string> ItemCodeProperty = P<SaleOrderCriteria>.Register(e => e.ItemCode);

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
		public static readonly Property<string> ItemNameProperty = P<SaleOrderCriteria>.Register(e => e.ItemName);

		/// <summary>
		/// 物料名称
		/// </summary>
		public string ItemName
		{
			get { return this.GetProperty(ItemNameProperty); }
			set { SetProperty(ItemNameProperty, value); }
		}
		#endregion

		#region 库存组织名称 ItemName
		/// <summary>
		/// 库存组织名称
		/// </summary>
		[MaxLength(240)]
		[Label("库存组织名称")]
		public static readonly Property<string> EnterpriseNameProperty = P<SaleOrderCriteria>.Register(e => e.EnterpriseName);

		/// <summary>
		/// 库存组织名称
		/// </summary>
		public string EnterpriseName
		{
			get { return this.GetProperty(EnterpriseNameProperty); }
			set { SetProperty(EnterpriseNameProperty, value); }
		}
		#endregion

		#region RegisterDateTime
		/// <summary>
		/// 登记时间
		/// </summary>
		[Label("登记时间")]
		public static readonly Property<DateRange> RegisterDateTimeProperty = P<SaleOrderCriteria>.Register(e => e.RegisterDateTime);

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
		[Label("客户交期")]
		public static readonly Property<DateRange> RequireDeliveryProperty = P<SaleOrderCriteria>.Register(e => e.RequireDelivery);

		/// <summary>
		/// 客户交期
		/// </summary>
		public DateRange RequireDelivery
		{
			get { return GetProperty(RequireDeliveryProperty); }
			set { SetProperty(RequireDeliveryProperty, value); }
		}
		#endregion

		#region Source
		/// <summary>
		/// 来源
		/// </summary>
		[Label("来源")]
		public static readonly Property<string> SourceProperty = P<SaleOrderCriteria>.Register(e => e.Source);

		/// <summary>
		/// 来源
		/// </summary>
		public string Source
		{
			get { return GetProperty(SourceProperty); }
			set { SetProperty(SourceProperty, value); }
		}
		#endregion

		#region 员工 Employee
		/// <summary>
		/// 员工Id
		/// </summary>
		public static readonly IRefIdProperty EmployeeIdProperty = P<SaleOrderCriteria>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

		/// <summary>
		/// 员工Id
		/// </summary>
		public double? EmployeeId
		{
			get { return (double)GetRefId(EmployeeIdProperty); }
			set { SetRefId(EmployeeIdProperty, value); }
		}

		/// <summary>
		/// 员工
		/// </summary>
		public static readonly RefEntityProperty<Employee> EmployeeProperty = P<SaleOrderCriteria>.RegisterRef(e => e.Employee, EmployeeIdProperty);

		/// <summary>
		/// 员工
		/// </summary>
		public Employee Employee
		{
			get { return GetRefEntity(EmployeeProperty); }
			set { SetRefEntity(EmployeeProperty, value); }
		}
		#endregion

		#region 客户 Customer
		/// <summary>
		/// 客户Id
		/// </summary>
		public static readonly IRefIdProperty CustomerIdProperty = P<SaleOrderCriteria>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Customer> CustomerProperty = P<SaleOrderCriteria>.RegisterRef(e => e.Customer, CustomerIdProperty);

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
		public static readonly Property<string> CustomerNameProperty = P<SaleOrderCriteria>.Register(e => e.CustomerName);

		/// <summary>
		/// 客户名称
		/// </summary>
		public string CustomerName
		{
			get { return this.GetProperty(CustomerNameProperty); }
			set { SetProperty(CustomerNameProperty, value); }
		}
		#endregion

		#region 库存组织 Enterprise
		/// <summary>
		/// 企业Id
		/// </summary>
		public static readonly IRefIdProperty EnterpriseIdProperty = P<SaleOrderCriteria>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

		/// <summary>
		/// 企业Id
		/// </summary>
		public double? EnterpriseId
		{
			get { return (double)GetRefId(EnterpriseIdProperty); }
			set { SetRefId(EnterpriseIdProperty, value); }
		}

		/// <summary>
		/// 企业
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<SaleOrderCriteria>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

		/// <summary>
		/// 企业
		/// </summary>
		public Enterprise Enterprise
		{
			get { return GetRefEntity(EnterpriseProperty); }
			set { SetRefEntity(EnterpriseProperty, value); }
		}


		#region 是否导出 IsExport
		/// <summary>
		/// 是否导出
		/// </summary>
		[Label("是否导出")]
		public static readonly Property<bool> IsExportProperty = P<SaleOrderCriteria>.Register(e => e.IsExport);

		/// <summary>
		/// 是否导出
		/// </summary>
		public bool IsExport
		{
			get { return this.GetProperty(IsExportProperty); }
			set { SetProperty(IsExportProperty, value); }
		}
		#endregion


		#endregion

		/// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<SaleOrderController>().GetSalesOrderList(this);
		}
	}
}
