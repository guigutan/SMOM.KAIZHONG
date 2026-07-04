using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.DataAuth;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Outsourcing.Configs;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.MES.Outsourcing
{
	/// <summary>
	/// 委外需求单
	/// </summary>
	[RootEntity, Serializable]
	[Label("工序委外需求单")]
    [ConditionQueryType(typeof(OutsourcingRequestCriteria))]
    [EntityWithConfig(typeof(NoConfig), "工序委外需求单需求单号生成规则", "用于配置工序委外需求单需求单号生成规则")]
	[EntityWithConfig(typeof(OutsourcingReportConfig))]
	[EntityWithConfig(typeof(OutsourcingRequestPDAConfig))]
    //[SIE.DataAuth.EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class OutsourcingRequest : DataEntity
	{
		#region 需求单号 NO
		/// <summary>
		/// 需求单号
		/// </summary>
		[Label("需求单号")]
		public static readonly Property<string> NOProperty = P<OutsourcingRequest>.Register(e => e.NO);

		/// <summary>
		/// 需求单号
		/// </summary>
		public string NO
		{
			get { return GetProperty(NOProperty); }
			set { SetProperty(NOProperty, value); }
		}
		#endregion

		#region 需求数量 RequestQty
		/// <summary>
		/// 需求数量
		/// </summary>
		[Label("需求数量")]
		public static readonly Property<decimal> RequestQtyProperty = P<OutsourcingRequest>.Register(e => e.RequestQty);

		/// <summary>
		/// 需求数量
		/// </summary>
		public decimal RequestQty
		{
			get { return GetProperty(RequestQtyProperty); }
			set { SetProperty(RequestQtyProperty, value); }
		}
        #endregion

        #region 发料数量 OutboundQty
        /// <summary>
        /// 发料数量
        /// </summary>
        [Label("发料数量")]
		public static readonly Property<decimal> OutboundQtyProperty = P<OutsourcingRequest>.Register(e => e.OutboundQty);

        /// <summary>
        /// 发料数量
        /// </summary>
        public decimal OutboundQty
		{
			get { return GetProperty(OutboundQtyProperty); }
			set { SetProperty(OutboundQtyProperty, value); }
		}
        #endregion

        #region 收货数量 WarehousingQty
        /// <summary>
        /// 收货数量
        /// </summary>
        [Label("收货数量")]
		public static readonly Property<decimal> WarehousingQtyProperty = P<OutsourcingRequest>.Register(e => e.WarehousingQty);

        /// <summary>
        /// 收货数量
        /// </summary>
        public decimal WarehousingQty
		{
			get { return GetProperty(WarehousingQtyProperty); }
			set { SetProperty(WarehousingQtyProperty, value); }
		}
        #endregion

        #region 发料状态 OutboundState
        /// <summary>
        /// 发料状态
        /// </summary>
        [Label("发料状态")]
		public static readonly Property<OutboundState> OutboundStateProperty = P<OutsourcingRequest>.Register(e => e.OutboundState);

		/// <summary>
		/// 发料状态
		/// </summary>
		public OutboundState OutboundState
        {
			get { return this.GetProperty(OutboundStateProperty); }
			set { this.SetProperty(OutboundStateProperty, value); }
		}
        #endregion

        #region 报工状态 ReportState
        /// <summary>
        /// 报工状态
        /// </summary>
        [Label("报工状态")]
		public static readonly Property<ReportState> ReportStateProperty = P<OutsourcingRequest>.Register(e => e.ReportState);

		/// <summary>
		/// 报工状态
		/// </summary>
		public ReportState ReportState
        {
			get { return this.GetProperty(ReportStateProperty); }
			set { this.SetProperty(ReportStateProperty, value); }
		}
		#endregion

		#region 委外状态 OutsourcingState
		/// <summary>
		/// 委外状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<OutsourcingState> OutsourcingStateProperty = P<OutsourcingRequest>.Register(e => e.OutsourcingState);

		/// <summary>
		/// 委外状态
		/// </summary>
		public OutsourcingState OutsourcingState
		{
			get { return GetProperty(OutsourcingStateProperty); }
			set { SetProperty(OutsourcingStateProperty, value); }
		}
		#endregion

		#region 工单号 WorkOrder
		/// <summary>
		/// 工单号Id
		/// </summary>
		[Label("工单号")]
		public static readonly IRefIdProperty WorkOrderIdProperty = P<OutsourcingRequest>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

		/// <summary>
		/// 工单号Id
		/// </summary>
		public double WorkOrderId
		{
			get { return (double)GetRefId(WorkOrderIdProperty); }
			set { SetRefId(WorkOrderIdProperty, value); }
		}

		/// <summary>
		/// 工单号
		/// </summary>
		public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<OutsourcingRequest>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

		/// <summary>
		/// 工单号
		/// </summary>
		public WorkOrder WorkOrder
		{
			get { return GetRefEntity(WorkOrderProperty); }
			set { SetRefEntity(WorkOrderProperty, value); }
		}
		#endregion

		#region 起始工序 BeginProcess
		/// <summary>
		/// 起始工序Id
		/// </summary>
		[Label("起始工序")]
		public static readonly IRefIdProperty BeginProcessIdProperty = P<OutsourcingRequest>.RegisterRefId(e => e.BeginProcessId, ReferenceType.Normal);

		/// <summary>
		/// 起始工序Id
		/// </summary>
		public double BeginProcessId
        {
			get { return (double)GetRefId(BeginProcessIdProperty); }
			set { SetRefId(BeginProcessIdProperty, value); }
		}

		/// <summary>
		/// 起始工序
		/// </summary>
		public static readonly RefEntityProperty<WorkOrderRoutingProcess> BeginProcessProperty = P<OutsourcingRequest>.RegisterRef(e => e.BeginProcess, BeginProcessIdProperty);

		/// <summary>
		/// 起始工序
		/// </summary>
		public WorkOrderRoutingProcess BeginProcess
        {
			get { return GetRefEntity(BeginProcessProperty); }
			set { SetRefEntity(BeginProcessProperty, value); }
		}
		#endregion

		#region 结束工序 EndProcess
		/// <summary>
		/// 结束工序Id
		/// </summary>
		[Label("结束工序")]
		public static readonly IRefIdProperty EndProcessIdProperty = P<OutsourcingRequest>.RegisterRefId(e => e.EndProcessId, ReferenceType.Normal);

		/// <summary>
		/// 结束工序Id
		/// </summary>
		public double EndProcessId
        {
			get { return (double)GetRefId(EndProcessIdProperty); }
			set { SetRefId(EndProcessIdProperty, value); }
		}

		/// <summary>
		/// 结束工序
		/// </summary>
		public static readonly RefEntityProperty<WorkOrderRoutingProcess> EndProcessProperty = P<OutsourcingRequest>.RegisterRef(e => e.EndProcess, EndProcessIdProperty);

		/// <summary>
		/// 结束工序
		/// </summary>
		public WorkOrderRoutingProcess EndProcess
        {
			get { return GetRefEntity(EndProcessProperty); }
			set { SetRefEntity(EndProcessProperty, value); }
		}
		#endregion

		#region 供应商 Supplier
		/// <summary>
		/// 供应商Id
		/// </summary>
		[Label("供应商")]
		public static readonly IRefIdProperty SupplierIdProperty = P<OutsourcingRequest>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

		/// <summary>
		/// 供应商Id
		/// </summary>
		public double? SupplierId
		{
			get { return (double?)GetRefNullableId(SupplierIdProperty); }
			set { SetRefNullableId(SupplierIdProperty, value); }
		}

		/// <summary>
		/// 供应商
		/// </summary>
		public static readonly RefEntityProperty<Supplier> SupplierProperty = P<OutsourcingRequest>.RegisterRef(e => e.Supplier, SupplierIdProperty);

		/// <summary>
		/// 供应商
		/// </summary>
		public Supplier Supplier
		{
			get { return GetRefEntity(SupplierProperty); }
			set { SetRefEntity(SupplierProperty, value); }
		}
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<OutsourcingRequest>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

		/// <summary>
		/// 工厂Id
		/// </summary>
		public double? FactoryId
        {
			get { return (double?)GetRefNullableId(FactoryIdProperty); }
			set { SetRefNullableId(FactoryIdProperty, value); }
		}

		/// <summary>
		/// 工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<OutsourcingRequest>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
        {
			get { return GetRefEntity(FactoryProperty); }
			set { SetRefEntity(FactoryProperty, value); }
		}
        #endregion

        #region 发起工厂 InitiatorFactory
        /// <summary>
        /// 发起工厂
        /// </summary>
        [Label("发起工厂")]
		public static readonly Property<string> InitiatorFactoryProperty = P<OutsourcingRequest>.Register(e => e.InitiatorFactory);

		/// <summary>
		/// 发起工厂
		/// </summary>
		public string InitiatorFactory
        {
			get { return this.GetProperty(InitiatorFactoryProperty); }
			set { this.SetProperty(InitiatorFactoryProperty, value); }
		}
		#endregion

		#region 委外工厂 OutFactory
		/// <summary>
		/// 委外工厂
		/// </summary>
		[Label("委外工厂")]
		public static readonly Property<string> OutFactoryProperty = P<OutsourcingRequest>.Register(e => e.OutFactory);

		/// <summary>
		/// 委外工厂
		/// </summary>
		public string OutFactory
        {
			get { return this.GetProperty(OutFactoryProperty); }
			set { this.SetProperty(OutFactoryProperty, value); }
		}
		#endregion

		#region 供应商名称 SupplierName
		/// <summary>
		/// 供应商名称
		/// </summary>
		[Label("供应商名称")]
		public static readonly Property<string> SupplierNameProperty = P<OutsourcingRequest>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

		/// <summary>
		/// 供应商名称
		/// </summary>
		public string SupplierName
		{
			get { return this.GetProperty(SupplierNameProperty); }
		}
        #endregion

        #region 发料明细 ProcessingOutsourcingOutbound
        /// <summary>
        /// 发料明细
        /// </summary>
        [Label("发料明细")]
        public static readonly ListProperty<EntityList<ProcessingOutbound>> ProcessingOutsourcingOutboundListProperty = P<OutsourcingRequest>.RegisterList(e => e.ProcessingOutsourcingOutboundList);
        /// <summary>
        /// 发料明细
        /// </summary>
        public EntityList<ProcessingOutbound> ProcessingOutsourcingOutboundList
        {
            get { return this.GetLazyList(ProcessingOutsourcingOutboundListProperty); }
        }
        #endregion

        #region 收货明细 ProcessingOutsourcingInStockList
        /// <summary>
        /// 收货明细
        /// </summary>
        [Label("收货明细")]
        public static readonly ListProperty<EntityList<ProcessingInStock>> ProcessingOutsourcingInStockListProperty = P<OutsourcingRequest>.RegisterList(e => e.ProcessingOutsourcingInStockList);
        /// <summary>
        /// 收货明细
        /// </summary>
        public EntityList<ProcessingInStock> ProcessingOutsourcingInStockList
        {
            get { return this.GetLazyList(ProcessingOutsourcingInStockListProperty); }
        }
        #endregion

        #region 报工记录 OutsourcingReportLogList
        /// <summary>
        /// 报工记录
        /// </summary>
        [Label("报工记录")]
		public static readonly ListProperty<EntityList<OutsourcingReportLog>> OutsourcingReportLogListeProperty = P<OutsourcingRequest>.RegisterList(e => e.OutsourcingReportLogList);

		/// <summary>
		/// 报工记录
		/// </summary>
		public EntityList<OutsourcingReportLog> OutsourcingReportLogList
        {
			get { return this.GetLazyList(OutsourcingReportLogListeProperty); }
		}
		#endregion

		#region 工序链 ProcessLinks
		/// <summary>
		/// 工序链
		/// </summary>
		[Label("工序链")]
		[MaxLength(2000)]
		public static readonly Property<string> ProcessLinksProperty = P<OutsourcingRequest>.Register(e => e.ProcessLinks);

		/// <summary>
		/// 工序链
		/// </summary>
		public string ProcessLinks
		{
			get { return this.GetProperty(ProcessLinksProperty); }
			set { this.SetProperty(ProcessLinksProperty, value); }
		}
		#endregion


		#region 视图属性

		#region 生产资源 WipResource
		/// <summary>
		/// 生产资源
		/// </summary>
		[Label("生产资源")]
		public static readonly Property<string> WipResourceProperty = P<OutsourcingRequest>.RegisterView(e => e.WipResource, p => p.WorkOrder.Resource.Code);

        /// <summary>
        /// 生产资源
        /// </summary>
        public string WipResource
        {
			get { return this.GetProperty(WipResourceProperty); }
		}
        #endregion

        #region 车间编码 WorkShop
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopProperty = P<OutsourcingRequest>.RegisterView(e => e.WorkShop, p => p.WorkOrder.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShop
        {
            get { return this.GetProperty(WorkShopProperty); }
        }
        #endregion

        #region 工单计划开始时间 PlanBeginDate
        /// <summary>
        /// 工单计划开始时间
        /// </summary>
        [Label("工单计划开始时间")]
		public static readonly Property<DateTime> PlanBeginDateProperty = P<OutsourcingRequest>.RegisterView(e => e.PlanBeginDate, p => p.WorkOrder.PlanBeginDate);

        /// <summary>
        /// 工单计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
			get { return this.GetProperty(PlanBeginDateProperty); }
		}
        #endregion

        #region 产品扩展 ItemExtProp
        /// <summary>
        /// 产品扩展
        /// </summary>
        [Label("产品扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<OutsourcingRequest>.RegisterView(e => e.ItemExtProp, p => p.WorkOrder.ItemExtProp);

        /// <summary>
        /// 产品扩展
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
        }
        #endregion

        #region 扩展属性名称 ItemExtPropName	
        /// <summary>
        /// 扩展属性名称
        /// </summary>
        [Label("扩展属性名称")]
        public static readonly Property<string> ItemExtProNameProperty = P<OutsourcingRequest>.RegisterView(e => e.ItemExtPropName, p => p.WorkOrder.ItemExtPropName);

        /// <summary>
        /// 扩展属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtProNameProperty); }
        }
		#endregion

		#region 工单号 WorkOrderNo
		/// <summary>
		/// 工单号
		/// </summary>
		[Label("工单号")]
		public static readonly Property<string> WorkOrderNoProperty
			= P<OutsourcingRequest>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

		/// <summary>
		/// 工单号
		/// </summary>
		public string WorkOrderNo
		{
			get { return this.GetProperty(WorkOrderNoProperty); }
		}
        #endregion

        #region 起始工序名称 BeginProcessName
        /// <summary>
        /// 起始工序名称
        /// </summary>
        [Label("起始工序名称")]
		public static readonly Property<string> BeginProcessNameProperty
			= P<OutsourcingRequest>.RegisterView(e => e.BeginProcessName, p => p.BeginProcess.Name);

        /// <summary>
        /// 起始工序名称
        /// </summary>
        public string BeginProcessName
        {
			get { return this.GetProperty(BeginProcessNameProperty); }
		}
		#endregion

		#region 产品编码 ProduceCode
		/// <summary>
		/// 产品编码
		/// </summary>
		[Label("产品编码")]
		public static readonly Property<string> ProduceCodeProperty = P<OutsourcingRequest>.RegisterView(e => e.ProduceCode, p => p.WorkOrder.Product.Code);

		/// <summary>
		/// 产品编码
		/// </summary>
		public string ProduceCode
		{
			get { return this.GetProperty(ProduceCodeProperty); }
		}
		#endregion

		#region 产品名称 ProduceName
		/// <summary>
		/// 产品名称
		/// </summary>
		[Label("产品名称")]
		public static readonly Property<string> ProduceNameProperty = P<OutsourcingRequest>.RegisterView(e => e.ProduceName, p => p.WorkOrder.Product.Name);

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProduceName
		{
			get { return this.GetProperty(ProduceNameProperty); }
		}
        #endregion

        #region 结束工序名称 EndProcessName
        /// <summary>
        /// 结束工序名称
        /// </summary>
        [Label("结束工序名称")]
        public static readonly Property<string> EndProcessNameProperty = P<OutsourcingRequest>.RegisterView(e => e.EndProcessName, p => p.EndProcess.Name);

        /// <summary>
        /// 结束工序名称
        /// </summary>
        public string EndProcessName
        {
            get { return this.GetProperty(EndProcessNameProperty); }
        }
		#endregion

		#region 项目号 ProjectMaintainCode
		/// <summary>
		/// 项目号
		/// </summary>
		[Label("项目号")]
		public static readonly Property<string> ProjectMaintainCodeProperty = P<OutsourcingRequest>.RegisterView(e => e.ProjectMaintainCode, p => p.WorkOrder.ProjectMaintain.Code);

		/// <summary>
		/// 项目号
		/// </summary>
		public string ProjectMaintainCode
		{
			get { return this.GetProperty(ProjectMaintainCodeProperty); }
		}
		#endregion


		#endregion

	}

    /// <summary>
    /// 委外需求单 实体配置
    /// </summary>
    internal class OutsourcingRequestConfig : EntityConfig<OutsourcingRequest>
	{

        protected override void AddValidations(IValidationDeclarer rules)
        {
			rules.AddRule(OutsourcingRequest.NOProperty, new RequiredRule());
			rules.AddRule(OutsourcingRequest.NOProperty, new NotDuplicateRule());
            base.AddValidations(rules);
        }
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("OUT_REQUEST").MapAllProperties();
            Meta.Property(OutsourcingRequest.ProcessLinksProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
		}
	}
}