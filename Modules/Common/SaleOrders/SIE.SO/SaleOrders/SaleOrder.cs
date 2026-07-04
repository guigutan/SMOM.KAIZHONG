using SIE.Common.Configs;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.SO.SaleOrders.Configs;
using SIE.Resources.Employees;
using System;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SaleOrderCriteria))]
    [EntityWithConfig(typeof(SaleOrderNoConfig))]
    [Label("销售订单")]
	[DisplayMember(nameof(Code))]
    public class SaleOrder : DataEntity
    {
        /// <summary>
        /// 数据来源快码
        /// </summary>
        public const string ORDERSOURCE = "ORDER_SOURCE";

		#region 销售订单编号 Code
		/// <summary>
		/// 销售订单编号
		/// </summary>
		[Required]
		[NotDuplicate]
		[MaxLength(200)]
		[Label("销售订单编号")]
		public static readonly Property<string> CodeProperty = P<SaleOrder>.Register(e => e.Code);

		/// <summary>
		/// 销售订单编号
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 订单行数(不映射数据库) OrderRowsQty
		/// <summary>
		/// 订单行数(不映射数据库)
		/// </summary>
		[Label("订单行数")]
		public static readonly Property<int> OrderRowsQtyProperty = P<SaleOrder>.Register(e => e.OrderRowsQty);

		/// <summary>
		/// 订单行数(不映射数据库)
		/// </summary>
		public int OrderRowsQty
		{
			get { return GetProperty(OrderRowsQtyProperty); }
			set { SetProperty(OrderRowsQtyProperty, value); }
		}
		#endregion

		#region 合计数量(不映射数据库) TotalQty
		/// <summary>
		/// 合计数量(不映射数据库)
		/// </summary>
		[Label("合计数量")]
		public static readonly Property<decimal> TotalQtyProperty = P<SaleOrder>.Register(e => e.TotalQty);

		/// <summary>
		/// 合计数量(不映射数据库)
		/// </summary>
		public decimal TotalQty
		{
			get { return GetProperty(TotalQtyProperty); }
			set { SetProperty(TotalQtyProperty, value); }
		}
		#endregion

		#region 登记时间 RegisterDateTime
		/// <summary>
		/// 登记时间
		/// </summary>
		[Label("登记时间")]
		public static readonly Property<DateTime?> RegisterDateTimeProperty = P<SaleOrder>.Register(e => e.RegisterDateTime);

		/// <summary>
		/// 登记时间
		/// </summary>
		public DateTime? RegisterDateTime
		{
			get { return GetProperty(RegisterDateTimeProperty); }
			set { SetProperty(RegisterDateTimeProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(2000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<SaleOrder>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 列表 SaleOrderDetailList
		/// <summary>
		/// 列表
		/// </summary>
		public static readonly ListProperty<EntityList<SaleOrderDetail>> SaleOrderDetailListProperty = P<SaleOrder>.RegisterList(e => e.SaleOrderDetailList);
		/// <summary>
		/// 列表
		/// </summary>
		public EntityList<SaleOrderDetail> SaleOrderDetailList
		{
			get { return this.GetLazyList(SaleOrderDetailListProperty); }
		}
		#endregion

		#region 员工 Employee
		/// <summary>
		/// 员工Id
		/// </summary>
		public static readonly IRefIdProperty EmployeeIdProperty = P<SaleOrder>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

		/// <summary>
		/// 员工Id
		/// </summary>
		public double EmployeeId
		{
			get { return (double)GetRefId(EmployeeIdProperty); }
			set { SetRefId(EmployeeIdProperty, value); }
		}

		/// <summary>
		/// 员工
		/// </summary>
		public static readonly RefEntityProperty<Employee> EmployeeProperty = P<SaleOrder>.RegisterRef(e => e.Employee, EmployeeIdProperty);

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
		public static readonly IRefIdProperty CustomerIdProperty = P<SaleOrder>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

		/// <summary>
		/// 客户Id
		/// </summary>
		public double CustomerId
		{
			get { return (double)GetRefId(CustomerIdProperty); }
			set { SetRefId(CustomerIdProperty, value); }
		}

		/// <summary>
		/// 客户
		/// </summary>
		public static readonly RefEntityProperty<Customer> CustomerProperty = P<SaleOrder>.RegisterRef(e => e.Customer, CustomerIdProperty);

		/// <summary>
		/// 客户
		/// </summary>
		public Customer Customer
		{
			get { return GetRefEntity(CustomerProperty); }
			set { SetRefEntity(CustomerProperty, value); }
		}
		#endregion

		#region 客户名称  CustomerName
		/// <summary>
		/// 客户名称
		/// </summary>
		[Label("客户名称")]
		public static readonly Property<string> CustomerNameProperty = P<SaleOrder>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

		/// <summary>
		/// 客户名称
		/// </summary>
		public string CustomerName
		{
			get { return this.GetProperty(CustomerNameProperty); }
		}
		#endregion

		#region 订单来源 OrderSourceType
		/// <summary>
		/// 订单来源
		/// </summary>
		[Label("订单来源")]
		public static readonly Property<OrderSourceType> OrderSourceTypeProperty = P<SaleOrder>.Register(e => e.OrderSourceType);

		/// <summary>
		/// 订单来源
		/// </summary>
		public OrderSourceType OrderSourceType
		{
			get { return GetProperty(OrderSourceTypeProperty); }
			set { SetProperty(OrderSourceTypeProperty, value); }
		}
		#endregion

		#region 销售订单日志列表 SaleOrderLogList
		/// <summary>
		/// 销售订单日志列表
		/// </summary>
		public static readonly ListProperty<EntityList<SaleOrderLog>> SaleOrderLogListProperty = P<SaleOrder>.RegisterList(e => e.SaleOrderLogList);
		/// <summary>
		/// 销售订单日志列表
		/// </summary>
		public EntityList<SaleOrderLog> SaleOrderLogList
		{
			get { return this.GetLazyList(SaleOrderLogListProperty); }
		}
		#endregion

		#region 明细是否包含除新建以为的行数据条数 (不映射数据库) DetailSum
		/// <summary>
		/// 明细是否包含除新建以为的行数据条数(不映射数据库)
		/// </summary>
		[Label("明细非新建条数")]
		public static readonly Property<decimal> DetailSumProperty = P<SaleOrder>.Register(e => e.DetailSum);

		/// <summary>
		/// 明细是否包含除新建以为的行数据条数(不映射数据库)
		/// </summary>
		public decimal DetailSum
		{
			get { return GetProperty(DetailSumProperty); }
			set { SetProperty(DetailSumProperty, value); }
		}
        #endregion

        #region SGC
        #region 客户PO CustomerPo
        /// <summary>
        /// 客户PO
        /// </summary>
        [Label("客户PO")]
        public static readonly Property<string> CustomerPoProperty = P<SaleOrder>.Register(e => e.CustomerPo);

        /// <summary>
        /// 客户PO
        /// </summary>
        public string CustomerPo
        {
            get { return this.GetProperty(CustomerPoProperty); }
            set { this.SetProperty(CustomerPoProperty, value); }
        }
        #endregion

        #region 客户PO日期 CustomerPoDate
        /// <summary>
        /// 客户PO日期
        /// </summary>
        [Label("客户PO日期")]
        public static readonly Property<DateTime?> CustomerPoDateProperty = P<SaleOrder>.Register(e => e.CustomerPoDate);

        /// <summary>
        /// 客户PO日期
        /// </summary>
        public DateTime? CustomerPoDate
        {
            get { return this.GetProperty(CustomerPoDateProperty); }
            set { this.SetProperty(CustomerPoDateProperty, value); }
        }
        #endregion

        #region 销售凭证类型 SalesVoucherType
        /// <summary>
        /// 销售凭证类型
        /// </summary>
        [Label("销售凭证类型")]
        public static readonly Property<string> SalesVoucherTypeProperty = P<SaleOrder>.Register(e => e.SalesVoucherType);

        /// <summary>
        /// 销售凭证类型
        /// </summary>
        public string SalesVoucherType
        {
            get { return this.GetProperty(SalesVoucherTypeProperty); }
            set { this.SetProperty(SalesVoucherTypeProperty, value); }
        }
        #endregion

        #region 订货原因 OrderReason
        /// <summary>
        /// 订货原因
        /// </summary>
        [Label("订货原因")]
        public static readonly Property<string> OrderReasonProperty = P<SaleOrder>.Register(e => e.OrderReason);

        /// <summary>
        /// 订货原因
        /// </summary>
        public string OrderReason
        {
            get { return this.GetProperty(OrderReasonProperty); }
            set { this.SetProperty(OrderReasonProperty, value); }
        }
        #endregion

        #region 销售组织 SalesOrganization
        /// <summary>
        /// 销售组织
        /// </summary>
        [Label("销售组织")]
        public static readonly Property<string> SalesOrganizationProperty = P<SaleOrder>.Register(e => e.SalesOrganization);

        /// <summary>
        /// 销售组织
        /// </summary>
        public string SalesOrganization
        {
            get { return this.GetProperty(SalesOrganizationProperty); }
            set { this.SetProperty(SalesOrganizationProperty, value); }
        }
        #endregion

        #region 分销渠道 DistributionChannel
        /// <summary>
        /// 分销渠道
        /// </summary>
        [Label("分销渠道")]
        public static readonly Property<string> DistributionChannelProperty = P<SaleOrder>.Register(e => e.DistributionChannel);

        /// <summary>
        /// 分销渠道
        /// </summary>
        public string DistributionChannel
        {
            get { return this.GetProperty(DistributionChannelProperty); }
            set { this.SetProperty(DistributionChannelProperty, value); }
        }
        #endregion

        #region 分部 Branch
        /// <summary>
        /// 分部
        /// </summary>
        [Label("分部")]
        public static readonly Property<string> BranchProperty = P<SaleOrder>.Register(e => e.Branch);

        /// <summary>
        /// 分部
        /// </summary>
        public string Branch
        {
            get { return this.GetProperty(BranchProperty); }
            set { this.SetProperty(BranchProperty, value); }
        }
        #endregion

        #region 销售组 SalesTeam
        /// <summary>
        /// 销售组
        /// </summary>
        [Label("销售组")]
        public static readonly Property<string> SalesTeamProperty = P<SaleOrder>.Register(e => e.SalesTeam);

        /// <summary>
        /// 销售组
        /// </summary>
        public string SalesTeam
        {
            get { return this.GetProperty(SalesTeamProperty); }
            set { this.SetProperty(SalesTeamProperty, value); }
        }
        #endregion

        #region 售达方 SoldParty
        /// <summary>
        /// 售达方
        /// </summary>
        [Label("售达方")]
        public static readonly Property<string> SoldPartyProperty = P<SaleOrder>.Register(e => e.SoldParty);

        /// <summary>
        /// 售达方
        /// </summary>
        public string SoldParty
        {
            get { return this.GetProperty(SoldPartyProperty); }
            set { this.SetProperty(SoldPartyProperty, value); }
        }
        #endregion

        #region 售达方描述 SoldPartyDesc
        /// <summary>
        /// 售达方描述
        /// </summary>
        [Label("售达方描述")]
        public static readonly Property<string> SoldPartyDescProperty = P<SaleOrder>.Register(e => e.SoldPartyDesc);

        /// <summary>
        /// 售达方描述
        /// </summary>
        public string SoldPartyDesc
        {
            get { return this.GetProperty(SoldPartyDescProperty); }
            set { this.SetProperty(SoldPartyDescProperty, value); }
        }
        #endregion

        #region 送达方 DeliveryParty
        /// <summary>
        /// 送达方
        /// </summary>
        [Label("送达方")]
        public static readonly Property<string> DeliveryPartyProperty = P<SaleOrder>.Register(e => e.DeliveryParty);

        /// <summary>
        /// 送达方
        /// </summary>
        public string DeliveryParty
        {
            get { return this.GetProperty(DeliveryPartyProperty); }
            set { this.SetProperty(DeliveryPartyProperty, value); }
        }
        #endregion

        #region 送达方描述 DeliveryPartyDesc
        /// <summary>
        /// 送达方描述
        /// </summary>
        [Label("送达方描述")]
        public static readonly Property<string> DeliveryPartyDescProperty = P<SaleOrder>.Register(e => e.DeliveryPartyDesc);

        /// <summary>
        /// 送达方描述
        /// </summary>
        public string DeliveryPartyDesc
        {
            get { return this.GetProperty(DeliveryPartyDescProperty); }
            set { this.SetProperty(DeliveryPartyDescProperty, value); }
        }
        #endregion

        #region 审核日期 ApproveDate
        /// <summary>
        /// 审核日期
        /// </summary>
        [Label("审核日期")]
        public static readonly Property<DateTime> ApproveDateProperty = P<SaleOrder>.Register(e => e.ApproveDate);

        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime ApproveDate
        {
            get { return this.GetProperty(ApproveDateProperty); }
            set { this.SetProperty(ApproveDateProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 销售订单 实体配置
    /// </summary>
    internal class SalesOrderConfig : EntityConfig<SaleOrder>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("SALE_ORDER").MapAllProperties();
			Meta.Property(SaleOrder.OrderRowsQtyProperty).DontMapColumn();
			Meta.Property(SaleOrder.TotalQtyProperty).DontMapColumn();
			Meta.Property(SaleOrder.DetailSumProperty).DontMapColumn();
			Meta.Property(SaleOrder.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}

	}

}
