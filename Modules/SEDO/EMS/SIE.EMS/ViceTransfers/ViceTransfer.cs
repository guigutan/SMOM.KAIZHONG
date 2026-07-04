using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.ViceTransfers
{
    /// <summary>
    /// 副资产调拨
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("副资产调拨")]
	[ConditionQueryType(typeof(ViceTransferCriteria))]
	[DisplayMember(nameof(TransferNo))]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityWithConfig(typeof(NoConfig), "副资产调拨单号配置项", "副资产调拨单号生成规则")]
	public partial class ViceTransfer : DataEntity
	{
		#region 调拨单号 TransferNo
		/// <summary>
		/// 调拨单号
		/// </summary>
		[Label("调拨单号")]
		public static readonly Property<string> TransferNoProperty = P<ViceTransfer>.Register(e => e.TransferNo);

		/// <summary>
		/// 调拨单号
		/// </summary>
		public string TransferNo
		{
			get { return GetProperty(TransferNoProperty); }
			set { SetProperty(TransferNoProperty, value); }
		}
		#endregion

		#region 申请日期 ApplyDate
		/// <summary>
		/// 申请日期
		/// </summary>
		[Label("申请日期")]
		public static readonly Property<DateTime> ApplyDateProperty = P<ViceTransfer>.Register(e => e.ApplyDate);

		/// <summary>
		/// 申请日期
		/// </summary>
		public DateTime ApplyDate
		{
			get { return GetProperty(ApplyDateProperty); }
			set { SetProperty(ApplyDateProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<ViceTransfer>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 关闭原因 CloseRemark
		/// <summary>
		/// 关闭原因
		/// </summary>
		[MaxLength(1000)]
		[Label("关闭原因")]
		public static readonly Property<string> CloseRemarkProperty = P<ViceTransfer>.Register(e => e.CloseRemark);

		/// <summary>
		/// 关闭原因
		/// </summary>
		public string CloseRemark
		{
			get { return GetProperty(CloseRemarkProperty); }
			set { SetProperty(CloseRemarkProperty, value); }
		}
		#endregion

		#region 调拨状态 TransferStatus
		/// <summary>
		/// 调拨状态
		/// </summary>
		[Label("副资产调拨状态")]
		public static readonly Property<TransferStatus> TransferStatusProperty = P<ViceTransfer>.Register(e => e.TransferStatus);

		/// <summary>
		/// 调拨状态
		/// </summary>
		public TransferStatus TransferStatus
		{
			get { return GetProperty(TransferStatusProperty); }
			set { SetProperty(TransferStatusProperty, value); }
		}
		#endregion

		#region 资产对象 ViceAssetObject
		/// <summary>
		/// 资产对象
		/// </summary>
		[Label("副资产对象")]
		public static readonly Property<ViceAssetObject> ViceAssetObjectProperty = P<ViceTransfer>.Register(e => e.ViceAssetObject);

		/// <summary>
		/// 资产对象
		/// </summary>
		public ViceAssetObject ViceAssetObject
		{
			get { return GetProperty(ViceAssetObjectProperty); }
			set { SetProperty(ViceAssetObjectProperty, value); }
		}
		#endregion

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<ViceTransfer>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<ViceTransfer>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<ViceTransfer>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 来源仓库 Warehouse
		/// <summary>
		/// 来源仓库Id
		/// </summary>
		[Label("来源仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<ViceTransfer>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 来源仓库Id
		/// </summary>
		public double WarehouseId
		{
			get { return (double)GetRefId(WarehouseIdProperty); }
			set { SetRefId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 来源仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<ViceTransfer>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 来源仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 目标仓库 TargetWarehouse
		/// <summary>
		/// 目标仓库Id
		/// </summary>
		[Label("目标仓库")]
		public static readonly IRefIdProperty TargetWarehouseIdProperty = P<ViceTransfer>.RegisterRefId(e => e.TargetWarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 目标仓库Id
		/// </summary>
		public double TargetWarehouseId
		{
			get { return (double)GetRefId(TargetWarehouseIdProperty); }
			set { SetRefId(TargetWarehouseIdProperty, value); }
		}

		/// <summary>
		/// 目标仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> TargetWarehouseProperty = P<ViceTransfer>.RegisterRef(e => e.TargetWarehouse, TargetWarehouseIdProperty);

		/// <summary>
		/// 目标仓库
		/// </summary>
		public Warehouse TargetWarehouse
		{
			get { return GetRefEntity(TargetWarehouseProperty); }
			set { SetRefEntity(TargetWarehouseProperty, value); }
		}
		#endregion

		#region 申请人 applicant
		/// <summary>
		/// 申请人Id
		/// </summary>
		[Label("申请人")]
		public static readonly IRefIdProperty ApplicantIdProperty = P<ViceTransfer>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

		/// <summary>
		/// 申请人Id
		/// </summary>
		public double ApplicantId
		{
			get { return (double)GetRefId(ApplicantIdProperty); }
			set { SetRefId(ApplicantIdProperty, value); }
		}

		/// <summary>
		/// 申请人
		/// </summary>
		public static readonly RefEntityProperty<Employee> ApplicantProperty = P<ViceTransfer>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

		/// <summary>
		/// 申请人
		/// </summary>
		public Employee Applicant
		{
			get { return GetRefEntity(ApplicantProperty); }
			set { SetRefEntity(ApplicantProperty, value); }
		}
		#endregion

		#region 工治具需求清单 ViceTransferFixtureList
		/// <summary>
		/// 工治具需求清单
		/// </summary>
		public static readonly ListProperty<EntityList<ViceTransferFixture>> ViceTransferFixtureListProperty = P<ViceTransfer>.RegisterList(e => e.ViceTransferFixtureList);
		/// <summary>
		/// 工治具需求清单
		/// </summary>
		public EntityList<ViceTransferFixture> ViceTransferFixtureList
		{
			get { return this.GetLazyList(ViceTransferFixtureListProperty); }
		}
		#endregion

		#region 备件需求清单 ViceTransferSparePartList
		/// <summary>
		/// 备件需求清单
		/// </summary>
		public static readonly ListProperty<EntityList<ViceTransferSparePart>> ViceTransferSparePartListProperty = P<ViceTransfer>.RegisterList(e => e.ViceTransferSparePartList);
		/// <summary>
		/// 备件需求清单
		/// </summary>
		public EntityList<ViceTransferSparePart> ViceTransferSparePartList
		{
			get { return this.GetLazyList(ViceTransferSparePartListProperty); }
		}
		#endregion

		#region 工治具调拨明细 ViceTransferFixtureDetailList
		/// <summary>
		/// 工治具调拨明细
		/// </summary>
		public static readonly ListProperty<EntityList<ViceTransferFixtureDetail>> ViceTransferFixtureDetailListProperty = P<ViceTransfer>.RegisterList(e => e.ViceTransferFixtureDetailList);
		/// <summary>
		/// 工治具调拨明细
		/// </summary>
		public EntityList<ViceTransferFixtureDetail> ViceTransferFixtureDetailList
		{
			get { return this.GetLazyList(ViceTransferFixtureDetailListProperty); }
		}
		#endregion

		#region 备件调拨明细 ViceTransferSparePartDetailList
		/// <summary>
		/// 备件调拨明细
		/// </summary>
		public static readonly ListProperty<EntityList<ViceTransferSparePartDetail>> ViceTransferSparePartDetailListProperty = P<ViceTransfer>.RegisterList(e => e.ViceTransferSparePartDetailList);
		/// <summary>
		/// 备件调拨明细
		/// </summary>
		public EntityList<ViceTransferSparePartDetail> ViceTransferSparePartDetailList
		{
			get { return this.GetLazyList(ViceTransferSparePartDetailListProperty); }
		}
		#endregion

		#region 附件 ViceTransferAttachmentList
		/// <summary>
		/// 附件
		/// </summary>
		public static readonly ListProperty<EntityList<ViceTransferAttachment>> ViceTransferAttachmentListProperty = P<ViceTransfer>.RegisterList(e => e.ViceTransferAttachmentList);
		/// <summary>
		/// 附件
		/// </summary>
		public EntityList<ViceTransferAttachment> ViceTransferAttachmentList
		{
			get { return this.GetLazyList(ViceTransferAttachmentListProperty); }
		}
		#endregion


		#region 申请人 ApplicantName	
		/// <summary>
		/// 申请人
		/// </summary>
		[Label("申请人")]
        public static readonly Property<string> ApplicantNameProperty = P<ViceTransfer>.RegisterView(e => e.ApplicantName, p => p.Applicant.Name);

        /// <summary>
        /// 注释
        /// </summary>
        public string ApplicantName
		{
            get { return this.GetProperty(ApplicantNameProperty); }
			set { SetProperty(ApplicantNameProperty, value); }
		}
		#endregion

		#region 来源仓库编码 WarehouseCode
		/// <summary>
		/// 来源仓库编码
		/// </summary>
		[Label("来源仓库编码")]
		public static readonly Property<string> WarehouseCodeProperty = P<ViceTransfer>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

		/// <summary>
		/// 来源仓库编码
		/// </summary>
		public string WarehouseCode
		{
			get { return this.GetProperty(WarehouseCodeProperty); }
		}
		#endregion

	}

	/// <summary>
	/// 副资产调拨 实体配置
	/// </summary>
	internal class ViceTransferConfig : EntityConfig<ViceTransfer>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_VICE_TRAN").MapAllProperties();
			Meta.Property(ViceTransfer.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.Property(ViceTransfer.CloseRemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}