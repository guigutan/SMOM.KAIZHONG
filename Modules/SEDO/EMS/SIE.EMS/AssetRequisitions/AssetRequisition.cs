using SIE;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetRequisitions.Configs;
using SIE.EMS.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.EMS.AssetRequisitions
{
    /// <summary>
    /// 资产领用
    /// </summary>
    [RootEntity, Serializable]
	[ConditionQueryType(typeof(AssetRequisitionCriteria))]
	[Label("资产领用")]
	[EntityWithConfig(typeof(NoConfig), "资产领用单号生成规则配置项", "资产领用单号生成规则")]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	[DisplayMember("RequisitionNo")]
	public partial class AssetRequisition : DataEntity
	{
		#region 领用单号 RequisitionNo
		/// <summary>
		/// 领用单号
		/// </summary>		
		[Label("领用单号")]
		[Required]
		public static readonly Property<string> RequisitionNoProperty = P<AssetRequisition>.Register(e => e.RequisitionNo);

		/// <summary>
		/// 领用单号
		/// </summary>
		public string RequisitionNo
		{
			get { return GetProperty(RequisitionNoProperty); }
			set { SetProperty(RequisitionNoProperty, value); }
		}
		#endregion

		#region 外部领用 External
		/// <summary>
		/// 外部领用
		/// </summary>
		[Label("外部领用")]
		public static readonly Property<bool> ExternalProperty = P<AssetRequisition>.Register(e => e.External);

		/// <summary>
		/// 外部领用
		/// </summary>
		public bool External
		{
			get { return GetProperty(ExternalProperty); }
			set { SetProperty(ExternalProperty, value); }
		}
		#endregion

		#region 申请日期 ApplyDate
		/// <summary>
		/// 申请日期
		/// </summary>
		[Label("申请日期")]
		public static readonly Property<DateTime> ApplyDateProperty = P<AssetRequisition>.Register(e => e.ApplyDate);

		/// <summary>
		/// 申请日期
		/// </summary>
		public DateTime ApplyDate
		{
			get { return GetProperty(ApplyDateProperty); }
			set { SetProperty(ApplyDateProperty, value); }
		}
		#endregion

		#region 预计归还日期 RetureDate
		/// <summary>
		/// 预计归还日期
		/// </summary>
		[Label("预计归还日期")]
		public static readonly Property<DateTime?> RetureDateProperty = P<AssetRequisition>.Register(e => e.RetureDate);

		/// <summary>
		/// 预计归还日期
		/// </summary>
		public DateTime? RetureDate
		{
			get { return GetProperty(RetureDateProperty); }
			set { SetProperty(RetureDateProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[Label("备注")]
		[MaxLength(1000)]
		public static readonly Property<string> RemarkProperty = P<AssetRequisition>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 预估总金额 Amount
		/// <summary>
		/// 预估总金额
		/// </summary>
		[Label("预估总金额")]
		public static readonly Property<decimal> AmountProperty = P<AssetRequisition>.Register(e => e.Amount);

		/// <summary>
		/// 预估总金额
		/// </summary>
		public decimal Amount
		{
			get { return GetProperty(AmountProperty); }
			set { SetProperty(AmountProperty, value); }
		}
		#endregion

		#region 目的地描述 Destination
		/// <summary>
		/// 目的地描述
		/// </summary>
		[Label("目的地描述")]
		public static readonly Property<string> DestinationProperty = P<AssetRequisition>.Register(e => e.Destination);

		/// <summary>
		/// 目的地描述
		/// </summary>
		public string Destination
		{
			get { return GetProperty(DestinationProperty); }
			set { SetProperty(DestinationProperty, value); }
		}
		#endregion

		#region 联系人 ContactPerson
		/// <summary>
		/// 联系人
		/// </summary>
		[Label("联系人")]
		public static readonly Property<string> ContactPersonProperty = P<AssetRequisition>.Register(e => e.ContactPerson);

		/// <summary>
		/// 联系人
		/// </summary>
		public string ContactPerson
		{
			get { return GetProperty(ContactPersonProperty); }
			set { SetProperty(ContactPersonProperty, value); }
		}
		#endregion

		#region 联系方式 ContactInformation
		/// <summary>
		/// 联系方式
		/// </summary>
		[Label("联系方式")]
		public static readonly Property<string> ContactInformationProperty = P<AssetRequisition>.Register(e => e.ContactInformation);

		/// <summary>
		/// 联系方式
		/// </summary>
		public string ContactInformation
		{
			get { return GetProperty(ContactInformationProperty); }
			set { SetProperty(ContactInformationProperty, value); }
		}
		#endregion

		#region 联系地址 Address
		/// <summary>
		/// 联系地址
		/// </summary>
		[Label("联系地址")]
		public static readonly Property<string> AddressProperty = P<AssetRequisition>.Register(e => e.Address);

		/// <summary>
		/// 联系地址
		/// </summary>
		public string Address
		{
			get { return GetProperty(AddressProperty); }
			set { SetProperty(AddressProperty, value); }
		}
		#endregion

		#region 快递/出厂单号 TrackNo
		/// <summary>
		/// 快递/出厂单号
		/// </summary>
		[Label("快递/出厂单号")]
		public static readonly Property<string> TrackNoProperty = P<AssetRequisition>.Register(e => e.TrackNo);

		/// <summary>
		/// 快递/出厂单号
		/// </summary>
		public string TrackNo
		{
			get { return GetProperty(TrackNoProperty); }
			set { SetProperty(TrackNoProperty, value); }
		}
		#endregion

		#region 申请部门 ApplyDepartment
		/// <summary>
		/// 申请部门Id
		/// </summary>
		[Label("申请部门")]
		public static readonly IRefIdProperty ApplyDepartmentIdProperty = P<AssetRequisition>.RegisterRefId(e => e.ApplyDepartmentId, ReferenceType.Normal);

		/// <summary>
		/// 申请部门Id
		/// </summary>
		public double ApplyDepartmentId
		{
			get { return (double)GetRefId(ApplyDepartmentIdProperty); }
			set { SetRefId(ApplyDepartmentIdProperty, value); }
		}

		/// <summary>
		/// 申请部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> ApplyDepartmentProperty = P<AssetRequisition>.RegisterRef(e => e.ApplyDepartment, ApplyDepartmentIdProperty);

		/// <summary>
		/// 申请部门
		/// </summary>
		public Enterprise ApplyDepartment
		{
			get { return GetRefEntity(ApplyDepartmentProperty); }
			set { SetRefEntity(ApplyDepartmentProperty, value); }
		}
        #endregion

        #region 资产对象 AssetObject
        /// <summary>
        /// 资产对象
        /// </summary>
		[Required]
        [Label("资产对象")]
        public static readonly Property<AssetObject> AssetObjectProperty = P<AssetRequisition>.Register(e => e.AssetObject);

        /// <summary>
        /// 资产对象
        /// </summary>
        public AssetObject AssetObject
        {
            get { return this.GetProperty(AssetObjectProperty); }
            set { this.SetProperty(AssetObjectProperty, value); }
        }
		#endregion

		#region 业务类型 RequisitionType
		/// <summary>
		/// 业务类型
		/// </summary>
		[Required]
		[Label("业务类型")]
		public static readonly Property<RequisitionType> RequisitionTypeProperty = P<AssetRequisition>.Register(e => e.RequisitionType);

		/// <summary>
		/// 业务类型
		/// </summary>
		public RequisitionType RequisitionType
		{
			get { return GetProperty(RequisitionTypeProperty); }
			set { SetProperty(RequisitionTypeProperty, value); }
		}
		#endregion

		#region 发放仓库 Warehouse
		/// <summary>
		/// 发放仓库Id
		/// </summary>
		[Label("发放仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<AssetRequisition>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 发放仓库Id
		/// </summary>
		public double? WarehouseId
		{
			get { return (double?)GetRefNullableId(WarehouseIdProperty); }
			set { SetRefNullableId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 发放仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetRequisition>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 发放仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 归还状态 ReturnStatus
		/// <summary>
		/// 归还状态
		/// </summary>
		[Label("归还状态")]
		public static readonly Property<ReturnStatus> ReturnStatusProperty = P<AssetRequisition>.Register(e => e.ReturnStatus);

		/// <summary>
		/// 归还状态
		/// </summary>
		public ReturnStatus ReturnStatus
		{
			get { return GetProperty(ReturnStatusProperty); }
			set { SetProperty(ReturnStatusProperty, value); }
		}
		#endregion

		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<AssetRequisition>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 客户 Customer
		/// <summary>
		/// 客户Id
		/// </summary>
		[Label("客户")]
		public static readonly IRefIdProperty CustomerIdProperty = P<AssetRequisition>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

		/// <summary>
		/// 客户Id
		/// </summary>
		public double? CustomerId
		{
			get { return (double?)GetRefNullableId(CustomerIdProperty); }
			set { SetRefNullableId(CustomerIdProperty, value); }
		}

		/// <summary>
		/// 客户
		/// </summary>
		public static readonly RefEntityProperty<Customer> CustomerProperty = P<AssetRequisition>.RegisterRef(e => e.Customer, CustomerIdProperty);

		/// <summary>
		/// 客户
		/// </summary>
		public Customer Customer
		{
			get { return GetRefEntity(CustomerProperty); }
			set { SetRefEntity(CustomerProperty, value); }
		}
		#endregion

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<AssetRequisition>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

		/// <summary>
		/// 工厂Id
		/// </summary>
		public double FactoryId
		{
			get { return (double)GetRefId(FactoryIdProperty); }
			set { SetRefId(FactoryIdProperty, value); }
		}

		/// <summary>
		/// 工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<AssetRequisition>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
		{
			get { return GetRefEntity(FactoryProperty); }
			set { SetRefEntity(FactoryProperty, value); }
		}
		#endregion

        #region 借出部门 LendingDepartment
        /// <summary>
        /// 借出部门Id
        /// </summary>
        [Label("借出部门")]
        public static readonly IRefIdProperty LendingDepartmentIdProperty =
            P<AssetRequisition>.RegisterRefId(e => e.LendingDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 借出部门Id
        /// </summary>
        public double? LendingDepartmentId
        {
            get { return (double?)this.GetRefNullableId(LendingDepartmentIdProperty); }
            set { this.SetRefNullableId(LendingDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 借出部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> LendingDepartmentProperty =
            P<AssetRequisition>.RegisterRef(e => e.LendingDepartment, LendingDepartmentIdProperty);

        /// <summary>
        /// 借出部门
        /// </summary>
        public Enterprise LendingDepartment
        {
            get { return this.GetRefEntity(LendingDepartmentProperty); }
            set { this.SetRefEntity(LendingDepartmentProperty, value); }
        }
        #endregion

        #region 领用人 Employee
        /// <summary>
        /// 领用人Id
        /// </summary>
        [Label("领用人")]
		public static readonly IRefIdProperty EmployeeIdProperty = P<AssetRequisition>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

		/// <summary>
		/// 领用人Id
		/// </summary>
		public double EmployeeId
		{
			get { return (double)GetRefId(EmployeeIdProperty); }
			set { SetRefId(EmployeeIdProperty, value); }
		}

		/// <summary>
		/// 领用人
		/// </summary>
		public static readonly RefEntityProperty<Employee> EmployeeProperty = P<AssetRequisition>.RegisterRef(e => e.Employee, EmployeeIdProperty);

		/// <summary>
		/// 领用人
		/// </summary>
		public Employee Employee
		{
			get { return GetRefEntity(EmployeeProperty); }
			set { SetRefEntity(EmployeeProperty, value); }
		}
		#endregion

		#region 工治具清单 AssetRequisitionFixtureList
		/// <summary>
		/// 工治具清单
		/// </summary>
		[Label("工治具清单")]
		public static readonly ListProperty<EntityList<AssetRequisitionFixture>> AssetRequisitionFixtureListProperty = P<AssetRequisition>.RegisterList(e => e.AssetRequisitionFixtureList);
		/// <summary>
		/// 工治具清单
		/// </summary>
		public EntityList<AssetRequisitionFixture> AssetRequisitionFixtureList
		{
			get { return this.GetLazyList(AssetRequisitionFixtureListProperty); }
		}
		#endregion

		#region 设备清单 AssetRequisitionEquipmentList
		/// <summary>
		/// 设备清单
		/// </summary>
		[Label("设备清单")]
		public static readonly ListProperty<EntityList<AssetRequisitionEquipment>> AssetRequisitionEquipmentListProperty
			= P<AssetRequisition>.RegisterList(e => e.AssetRequisitionEquipmentList);
		/// <summary>
		/// 设备清单
		/// </summary>
		public EntityList<AssetRequisitionEquipment> AssetRequisitionEquipmentList
		{
			get { return this.GetLazyList(AssetRequisitionEquipmentListProperty); }
		}
		#endregion

		#region 供应商 Supplier
		/// <summary>
		/// 供应商Id
		/// </summary>
		[Label("供应商")]
		public static readonly IRefIdProperty SupplierIdProperty = P<AssetRequisition>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Supplier> SupplierProperty = P<AssetRequisition>.RegisterRef(e => e.Supplier, SupplierIdProperty);

		/// <summary>
		/// 供应商
		/// </summary>
		public Supplier Supplier
		{
			get { return GetRefEntity(SupplierProperty); }
			set { SetRefEntity(SupplierProperty, value); }
		}
		#endregion

		#region 发放状态 IssueStatus
		/// <summary>
		/// 发放状态
		/// </summary>
		[Label("发放状态")]
		public static readonly Property<IssueStatus> IssueStatusProperty = P<AssetRequisition>.Register(e => e.IssueStatus);

		/// <summary>
		/// 发放状态
		/// </summary>
		public IssueStatus IssueStatus
		{
			get { return GetProperty(IssueStatusProperty); }
			set { SetProperty(IssueStatusProperty, value); }
		}
		#endregion

		#region 外部领用类型 ExternalType
		/// <summary>
		/// 外部领用类型
		/// </summary>
		[Label("外部领用类型")]
		public static readonly Property<ExternalType?> ExternalTypeProperty = P<AssetRequisition>.Register(e => e.ExternalType);

		/// <summary>
		/// 外部领用类型
		/// </summary>
		public ExternalType? ExternalType
		{
			get { return GetProperty(ExternalTypeProperty); }
			set { SetProperty(ExternalTypeProperty, value); }
		}
		#endregion

		#region 发货方式 DeliveryWay
		/// <summary>
		/// 发货方式
		/// </summary>
		[Label("发货方式")]
		public static readonly Property<DeliveryWay?> DeliveryWayProperty = P<AssetRequisition>.Register(e => e.DeliveryWay);

		/// <summary>
		/// 发货方式
		/// </summary>
		public DeliveryWay? DeliveryWay
		{
			get { return GetProperty(DeliveryWayProperty); }
			set { SetProperty(DeliveryWayProperty, value); }
		}
		#endregion

		#region 附件 AssetRequisitionAttachmentList
		/// <summary>
		/// 附件
		/// </summary>
		[Label("附件")]
		public static readonly ListProperty<EntityList<AssetRequisitionAttachment>> AssetRequisitionAttachmentListProperty = P<AssetRequisition>.RegisterList(e => e.AssetRequisitionAttachmentList);
		/// <summary>
		/// 附件
		/// </summary>
		public EntityList<AssetRequisitionAttachment> AssetRequisitionAttachmentList
		{
			get { return this.GetLazyList(AssetRequisitionAttachmentListProperty); }
		}
        #endregion

        #region 视图属性

        #region 供应商代码 SupplierCode
        /// <summary>
        /// 供应商代码
        /// </summary>
        [Label("供应商代码")]
        public static readonly Property<string> SupplierCodeProperty = P<AssetRequisition>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商代码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<AssetRequisition>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 客户代码 CustomerCode
        /// <summary>
        /// 客户代码
        /// </summary>
        [Label("客户代码")]
        public static readonly Property<string> CustomerCodeProperty = P<AssetRequisition>.RegisterView(e => e.CustomerCode, p => p.Customer.Code);

        /// <summary>
        /// 客户代码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<AssetRequisition>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
		{
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<AssetRequisition>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<AssetRequisition>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
		#endregion

		#region 申请部门名称 ApplyDepartmentName
		/// <summary>
		/// 申请部门名称
		/// </summary>
		[Label("申请部门名称")]
        public static readonly Property<string> ApplyDepartmentNameProperty = P<AssetRequisition>.RegisterView(e => e.ApplyDepartmentName, p => p.ApplyDepartment.Name);

        /// <summary>
        /// 申请部门名称
        /// </summary>
        public string ApplyDepartmentName
		{
            get { return this.GetProperty(ApplyDepartmentNameProperty); }
        }
        #endregion

        #region 借出部门名称 LendingDepartmentName
        /// <summary>
        /// 借出部门名称
        /// </summary>
        [Label("借出部门名称")]
        public static readonly Property<string> LendingDepartmentNameProperty = P<AssetRequisition>.RegisterView(e => e.LendingDepartmentName, p => p.LendingDepartment.Name);

        /// <summary>
        /// 借出部门名称
        /// </summary>
        public string LendingDepartmentName
		{
            get { return this.GetProperty(LendingDepartmentNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 资产领用 实体配置
    /// </summary>
    internal class AssetRequisitionConfig : EntityConfig<AssetRequisition>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_REQ").MapAllProperties();
			Meta.Property(AssetRequisition.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}

		/// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
		{
			rules.AddRule(new HandlerRule()
			{
				Handler = (o, e) =>
				{
					var para = o.CastTo<AssetRequisition>();
					StringBuilder sb = new StringBuilder();

					if (para.RequisitionType == 0)
					{
						sb.AppendLine("【业务类型】不能为空！".L10N());
					}
					if (para.AssetObject == 0)
					{
						sb.AppendLine("【资产对象】不能为空！".L10N());
					}
					if ((para.WarehouseId == null || para.WarehouseId == 0) && para.AssetObject != AssetObject.Equipment)
					{
						sb.AppendLine("【发放仓库】不能为空！".L10N());
					}
					if (para.ApplyDate == new DateTime(2000,1,1))
					{
						sb.AppendLine("【申请日期】不能为空！".L10N());
					}
					if (para.RetureDate == null && para.RequisitionType == RequisitionType.Borrow)
					{
						sb.AppendLine("【预计归还日期】不能为空！".L10N());
					}
					if (para.External)
					{
						if (para.Remark.IsNullOrEmpty())
						{
							sb.AppendLine("外部领用时，【备注】不能为空！".L10N());
						}
						if (para.ExternalType == null)
						{
							sb.AppendLine("外部领用信息的【外部领用类型】不能为空！".L10N());
						}
						if (para.ExternalType == ExternalType.Supply && (para.SupplierId == null || para.SupplierId == 0))
						{
							sb.AppendLine("外部领用信息的【供应商编码】不能为空！".L10N());
						}
						if (para.ExternalType == ExternalType.Customer && (para.CustomerId == null || para.CustomerId == 0))
						{
							sb.AppendLine("外部领用信息的【客户编码】不能为空！".L10N());
						}
						if (para.ExternalType == ExternalType.Other && para.Destination.IsNullOrEmpty())
						{
							sb.AppendLine("外部领用信息的【目的地描述】不能为空！".L10N());
						}
						if (para.ContactPerson.IsNullOrEmpty())
						{
							sb.AppendLine("外部领用信息的【联系人】不能为空！".L10N());
						}
						if (para.ContactInformation.IsNullOrEmpty())
						{
							sb.AppendLine("外部领用信息的【联系方式】不能为空！".L10N());
						}
						if (para.Address.IsNullOrEmpty())
						{
							sb.AppendLine("外部领用信息的【联系地址】不能为空！".L10N());
						}
						if (para.DeliveryWay == null)
						{
							sb.AppendLine("外部领用信息的【发货方式】不能为空！".L10N());
						}
					}

					e.BrokenDescription = sb.ToString();
				}
			}, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
		}
	}
}