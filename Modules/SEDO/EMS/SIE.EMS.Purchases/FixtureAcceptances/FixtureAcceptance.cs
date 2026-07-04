using SIE;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.FixtureAcceptances
{
	/// <summary>
	/// 工治具验收
	/// </summary>
	[RootEntity, Serializable]
	[Label("工治具验收")]
	[ConditionQueryType(typeof(FixtureAcceptanceCriteria))]
	[DisplayMember(nameof(AcceptanceNo))]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityWithConfig(typeof(NoConfig), "工治具验收单号配置项", "工治具验收单号生成规则")]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	[BudgetDepartmentAuth(nameof(DepartmentId), true)]
	public partial class FixtureAcceptance : DataEntity
	{
		#region 验收单号 AcceptanceNo
		/// <summary>
		/// 验收单号
		/// </summary>
		[Label("验收单号")]
		public static readonly Property<string> AcceptanceNoProperty = P<FixtureAcceptance>.Register(e => e.AcceptanceNo);

		/// <summary>
		/// 验收单号
		/// </summary>
		public string AcceptanceNo
		{
			get { return GetProperty(AcceptanceNoProperty); }
			set { SetProperty(AcceptanceNoProperty, value); }
		}
		#endregion

		#region 接收数量 ReceiveQty
		/// <summary>
		/// 接收数量
		/// </summary>
		[Label("接收数量")]
		public static readonly Property<int> ReceiveQtyProperty = P<FixtureAcceptance>.Register(e => e.ReceiveQty);

		/// <summary>
		/// 接收数量
		/// </summary>
		public int ReceiveQty
		{
			get { return GetProperty(ReceiveQtyProperty); }
			set { SetProperty(ReceiveQtyProperty, value); }
		}
		#endregion

		#region 合格数量 PassQty
		/// <summary>
		/// 合格数量
		/// </summary>
		[Label("合格数量")]
		public static readonly Property<int> PassQtyProperty = P<FixtureAcceptance>.Register(e => e.PassQty);

		/// <summary>
		/// 合格数量
		/// </summary>
		public int PassQty
		{
			get { return GetProperty(PassQtyProperty); }
			set { SetProperty(PassQtyProperty, value); }
		}
		#endregion

		#region 不合格数量 UnqualifiedQty
		/// <summary>
		/// 不合格数量
		/// </summary>
		[Label("不合格数量")]
		public static readonly Property<int> UnqualifiedQtyProperty = P<FixtureAcceptance>.Register(e => e.UnqualifiedQty);

		/// <summary>
		/// 不合格数量
		/// </summary>
		public int UnqualifiedQty
		{
			get { return GetProperty(UnqualifiedQtyProperty); }
			set { SetProperty(UnqualifiedQtyProperty, value); }
		}
		#endregion

		#region 工治具编码 FixtureEncode
		/// <summary>
		/// 工治具编码Id
		/// </summary>
		[Label("工治具编码")]
		public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureAcceptance>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

		/// <summary>
		/// 工治具编码Id
		/// </summary>
		public double FixtureEncodeId
		{
			get { return (double)GetRefId(FixtureEncodeIdProperty); }
			set { SetRefId(FixtureEncodeIdProperty, value); }
		}

		/// <summary>
		/// 工治具编码
		/// </summary>
		public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureAcceptance>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

		/// <summary>
		/// 工治具编码
		/// </summary>
		public FixtureEncode FixtureEncode
		{
			get { return GetRefEntity(FixtureEncodeProperty); }
			set { SetRefEntity(FixtureEncodeProperty, value); }
		}
		#endregion

		#region 客户 Customer
		/// <summary>
		/// 客户Id
		/// </summary>
		[Label("客户")]
		public static readonly IRefIdProperty CustomerIdProperty = P<FixtureAcceptance>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Customer> CustomerProperty = P<FixtureAcceptance>.RegisterRef(e => e.Customer, CustomerIdProperty);

		/// <summary>
		/// 客户
		/// </summary>
		public Customer Customer
		{
			get { return GetRefEntity(CustomerProperty); }
			set { SetRefEntity(CustomerProperty, value); }
		}
		#endregion

		#region 供应商 Supplier
		/// <summary>
		/// 供应商Id
		/// </summary>
		[Label("供应商")]
		public static readonly IRefIdProperty SupplierIdProperty = P<FixtureAcceptance>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Supplier> SupplierProperty = P<FixtureAcceptance>.RegisterRef(e => e.Supplier, SupplierIdProperty);

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
		public static readonly IRefIdProperty FactoryIdProperty = P<FixtureAcceptance>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<FixtureAcceptance>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
		{
			get { return GetRefEntity(FactoryProperty); }
			set { SetRefEntity(FactoryProperty, value); }
		}
		#endregion

		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<FixtureAcceptance>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 附件 FixtureAcceptanceAttachmentList
		/// <summary>
		/// 附件
		/// </summary>
		public static readonly ListProperty<EntityList<FixtureAcceptanceAttachment>> FixtureAcceptanceAttachmentListProperty = P<FixtureAcceptance>.RegisterList(e => e.FixtureAcceptanceAttachmentList);
		/// <summary>
		/// 附件
		/// </summary>
		public EntityList<FixtureAcceptanceAttachment> FixtureAcceptanceAttachmentList
		{
			get { return this.GetLazyList(FixtureAcceptanceAttachmentListProperty); }
		}
		#endregion

		#region 部门 Department
		/// <summary>
		/// 部门Id
		/// </summary>
		[Label("部门")]
		public static readonly IRefIdProperty DepartmentIdProperty = P<FixtureAcceptance>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

		/// <summary>
		/// 部门Id
		/// </summary>
		public double DepartmentId
		{
			get { return (double)GetRefId(DepartmentIdProperty); }
			set { SetRefId(DepartmentIdProperty, value); }
		}

		/// <summary>
		/// 部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<FixtureAcceptance>.RegisterRef(e => e.Department, DepartmentIdProperty);

		/// <summary>
		/// 部门
		/// </summary>
		public Enterprise Department
		{
			get { return GetRefEntity(DepartmentProperty); }
			set { SetRefEntity(DepartmentProperty, value); }
		}
		#endregion

		#region 验收项目 FixtureAcceptanceItemList
		/// <summary>
		/// 验收项目
		/// </summary>
		public static readonly ListProperty<EntityList<FixtureAcceptanceItem>> FixtureAcceptanceItemListProperty = P<FixtureAcceptance>.RegisterList(e => e.FixtureAcceptanceItemList);
		/// <summary>
		/// 验收项目
		/// </summary>
		public EntityList<FixtureAcceptanceItem> FixtureAcceptanceItemList
		{
			get { return this.GetLazyList(FixtureAcceptanceItemListProperty); }
		}
		#endregion

		#region 验收明细 FixtureAcceptanceDetailList
		/// <summary>
		/// 验收明细
		/// </summary>
		public static readonly ListProperty<EntityList<FixtureAcceptanceDetail>> FixtureAcceptanceDetailListProperty = P<FixtureAcceptance>.RegisterList(e => e.FixtureAcceptanceDetailList);
		/// <summary>
		/// 验收明细
		/// </summary>
		public EntityList<FixtureAcceptanceDetail> FixtureAcceptanceDetailList
		{
			get { return this.GetLazyList(FixtureAcceptanceDetailListProperty); }
		}
		#endregion

		#region 接收单 FixtureReceive
		/// <summary>
		/// 接收单Id
		/// </summary>
		[Label("接收单")]
		public static readonly IRefIdProperty FixtureReceiveIdProperty = P<FixtureAcceptance>.RegisterRefId(e => e.FixtureReceiveId, ReferenceType.Normal);

		/// <summary>
		/// 接收单Id
		/// </summary>
		public double FixtureReceiveId
		{
			get { return (double)GetRefId(FixtureReceiveIdProperty); }
			set { SetRefId(FixtureReceiveIdProperty, value); }
		}

		/// <summary>
		/// 接收单
		/// </summary>
		public static readonly RefEntityProperty<FixtureReceive> FixtureReceiveProperty = P<FixtureAcceptance>.RegisterRef(e => e.FixtureReceive, FixtureReceiveIdProperty);

		/// <summary>
		/// 接收单
		/// </summary>
		public FixtureReceive FixtureReceive
		{
			get { return GetRefEntity(FixtureReceiveProperty); }
			set { SetRefEntity(FixtureReceiveProperty, value); }
		}
		#endregion


		#region 视图属性

		#region 供应商名称 SupplierName	
		/// <summary>
		/// 供应商名称
		/// </summary>
		[Label("供应商名称")]
		public static readonly Property<string> SupplierNameProperty = P<FixtureAcceptance>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

		/// <summary>
		/// 供应商名称
		/// </summary>
		public string SupplierName
		{
			get { return this.GetProperty(SupplierNameProperty); }
		}
		#endregion

		#region 客户名称 CustomerName	
		/// <summary>
		/// 客户名称
		/// </summary>
		[Label("客户名称")]
		public static readonly Property<string> CustomerNameProperty = P<FixtureAcceptance>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

		/// <summary>
		/// 客户名称
		/// </summary>
		public string CustomerName
		{
			get { return this.GetProperty(CustomerNameProperty); }
		}
		#endregion

		#region 接收单号 ReceiveNo
		/// <summary>
		/// 接收单号
		/// </summary>
		[Label("接收单号")]
		public static readonly Property<string> ReceiveNoProperty = P<FixtureAcceptance>.RegisterView(e => e.ReceiveNo, p => p.FixtureReceive.ReceiveNo);

		/// <summary>
		/// 接收单号
		/// </summary>
		public string ReceiveNo
		{
			get { return this.GetProperty(ReceiveNoProperty); }
		}
		#endregion


		#region 接收类型 ReceiveType
		/// <summary>
		/// 接收类型
		/// </summary>
		[Label("接收类型")]
		public static readonly Property<ReceiveType> ReceiveTypeProperty = P<FixtureAcceptance>.RegisterView(e => e.ReceiveType, p => p.FixtureReceive.ReceiveType);

		/// <summary>
		/// 接收类型
		/// </summary>
		public ReceiveType ReceiveType
		{
			get { return this.GetProperty(ReceiveTypeProperty); }
		}
		#endregion


		#region 型号编码 ModelCode
		/// <summary>
		/// 型号编码
		/// </summary>
		[Label("型号编码")]
		public static readonly Property<string> ModelCodeProperty = P<FixtureAcceptance>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

		/// <summary>
		/// 型号编码
		/// </summary>
		public string ModelCode
		{
			get { return this.GetProperty(ModelCodeProperty); }
		}
		#endregion


		#region 型号名称 ModelName
		/// <summary>
		/// 型号名称
		/// </summary>
		[Label("型号名称")]
		public static readonly Property<string> ModelNameProperty = P<FixtureAcceptance>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

		/// <summary>
		/// 型号编码
		/// </summary>
		public string ModelName
		{
			get { return this.GetProperty(ModelNameProperty); }
		}
		#endregion


		#region 管控模式 ManageMode
		/// <summary>
		/// 管理模式
		/// </summary>
		[Label("管控模式")]
		public static readonly Property<ManageMode> ManageModeProperty = P<FixtureAcceptance>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

		/// <summary>
		/// 
		/// </summary>
		public ManageMode ManageMode
		{
			get { return this.GetProperty(ManageModeProperty); }
		}
		#endregion

		#region 工治具编码 FixtureEncodeCode
		/// <summary>
		/// 工治具编码
		/// </summary>
		[Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<FixtureAcceptance>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureEncode.Code);

		/// <summary>
		/// 工治具编码
		/// </summary>
		public string FixtureEncodeCode
		{
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
        }
        #endregion


        #endregion

    }

	/// <summary>
	/// 工治具验收 实体配置
	/// </summary>
	internal class FixtureAcceptanceConfig : EntityConfig<FixtureAcceptance>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_FIXT_ACPT").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}