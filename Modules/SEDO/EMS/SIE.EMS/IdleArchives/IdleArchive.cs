using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.Enums;
using SIE.EMS.IdleArchives.Configs;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.IdleArchives
{
    /// <summary>
    /// 闲置与封存
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("闲置与封存")]
	[ConditionQueryType(typeof(IdleArchiveCriteria))]
	[DisplayMember(nameof(No))]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	[BussinessDepartmentAuth(nameof(DepartmentId), true)]
	[BussinessDepartmentAuth(nameof(UseDepartmentId), true)]
	[EntityWithConfig(typeof(IdleArchivesMaximumTermConfig))]
	[EntityWithConfig(typeof(MaintainedEquipmentTypeConfig))]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityWithConfig(typeof(NoConfig), "闲置封存单号配置项", "闲置封存单号生成规则")]
	public partial class IdleArchive : DataEntity
	{
		#region 单号 No
		/// <summary>
		/// 单号
		/// </summary>
		[Label("单号")]
		[Required]
		public static readonly Property<string> NoProperty = P<IdleArchive>.Register(e => e.No);

		/// <summary>
		/// 单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 设备类别 TypeCategory
		/// <summary>
		/// 设备类别
		/// </summary>
		[Label("设备类别")]
		[Required]
		public static readonly Property<string> TypeCategoryProperty = P<IdleArchive>.Register(e => e.TypeCategory);

		/// <summary>
		/// 设备类别
		/// </summary>
		public string TypeCategory
		{
			get { return GetProperty(TypeCategoryProperty); }
			set { SetProperty(TypeCategoryProperty, value); }
		}
		#endregion

		#region 固定资产 IsAsset
		/// <summary>
		/// 固定资产
		/// </summary>
		[Label("固定资产")]
		public static readonly Property<bool> IsAssetProperty = P<IdleArchive>.Register(e => e.IsAsset);

		/// <summary>
		/// 固定资产
		/// </summary>
		public bool IsAsset
		{
			get { return GetProperty(IsAssetProperty); }
			set { SetProperty(IsAssetProperty, value); }
		}
		#endregion

		#region 申请日期 ApplyDate
		/// <summary>
		/// 申请日期
		/// </summary>
		[Label("申请日期")]
		[Required]
		public static readonly Property<DateTime> ApplyDateProperty = P<IdleArchive>.Register(e => e.ApplyDate);

		/// <summary>
		/// 申请日期
		/// </summary>
		public DateTime ApplyDate
		{
			get { return GetProperty(ApplyDateProperty); }
			set { SetProperty(ApplyDateProperty, value); }
		}
		#endregion

		#region 业务说明 Remark
		/// <summary>
		/// 业务说明
		/// </summary>
		[MaxLength(1000)]
		[Label("业务说明")]
		[Required]
		public static readonly Property<string> RemarkProperty = P<IdleArchive>.Register(e => e.Remark);

		/// <summary>
		/// 业务说明
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 闲置与封存附件列表 IdleArchiveAttachmentList
		/// <summary>
		/// 闲置与封存附件列表
		/// </summary>
		[Label("闲置与封存附件列表")]
		public static readonly ListProperty<EntityList<IdleArchiveAttachment>> IdleArchiveAttachmentListProperty = P<IdleArchive>.RegisterList(e => e.IdleArchiveAttachmentList);
		/// <summary>
		/// 闲置与封存附件列表
		/// </summary>
		public EntityList<IdleArchiveAttachment> IdleArchiveAttachmentList
		{
			get { return this.GetLazyList(IdleArchiveAttachmentListProperty); }
		}
		#endregion

		#region 申请人 Applicant
		/// <summary>
		/// 申请人Id
		/// </summary>
		[Label("申请人")]
		public static readonly IRefIdProperty ApplicantIdProperty = P<IdleArchive>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Resources.Employee> ApplicantProperty = P<IdleArchive>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

		/// <summary>
		/// 申请人
		/// </summary>
		public Resources.Employee Applicant
		{
			get { return GetRefEntity(ApplicantProperty); }
			set { SetRefEntity(ApplicantProperty, value); }
		}
		#endregion

		#region 业务类型 IdleArchiveType
		/// <summary>
		/// 业务类型
		/// </summary>
		[Label("业务类型")]
		public static readonly Property<IdleArchiveType> IdleArchiveTypeProperty = P<IdleArchive>.Register(e => e.IdleArchiveType);

		/// <summary>
		/// 业务类型
		/// </summary>
		public IdleArchiveType IdleArchiveType
		{
			get { return GetProperty(IdleArchiveTypeProperty); }
			set { SetProperty(IdleArchiveTypeProperty, value); }
		}
		#endregion

		#region 设备明细 IdleArchiveDetailList
		/// <summary>
		/// 设备明细
		/// </summary>
		[Label("设备明细")]
		public static readonly ListProperty<EntityList<IdleArchiveDetail>> IdleArchiveDetailListProperty = P<IdleArchive>.RegisterList(e => e.IdleArchiveDetailList);
		/// <summary>
		/// 设备明细
		/// </summary>
		public EntityList<IdleArchiveDetail> IdleArchiveDetailList
		{
			get { return this.GetLazyList(IdleArchiveDetailListProperty); }
		}
		#endregion

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<IdleArchive>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<IdleArchive>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<IdleArchive>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 管理部门 Department
		/// <summary>
		/// 管理部门Id
		/// </summary>
		[Label("管理部门")]
		public static readonly IRefIdProperty DepartmentIdProperty = P<IdleArchive>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

		/// <summary>
		/// 管理部门Id
		/// </summary>
		public double DepartmentId
		{
			get { return (double)GetRefId(DepartmentIdProperty); }
			set { SetRefId(DepartmentIdProperty, value); }
		}

		/// <summary>
		/// 管理部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<IdleArchive>.RegisterRef(e => e.Department, DepartmentIdProperty);

		/// <summary>
		/// 管理部门
		/// </summary>
		public Enterprise Department
		{
			get { return GetRefEntity(DepartmentProperty); }
			set { SetRefEntity(DepartmentProperty, value); }
		}
		#endregion

		#region 使用部门 UseDepartment
		/// <summary>
		/// 使用部门Id
		/// </summary>
		[Label("使用部门")]
		public static readonly IRefIdProperty UseDepartmentIdProperty = P<IdleArchive>.RegisterRefId(e => e.UseDepartmentId, ReferenceType.Normal);

		/// <summary>
		/// 使用部门Id
		/// </summary>
		public double UseDepartmentId
		{
			get { return (double)GetRefId(UseDepartmentIdProperty); }
			set { SetRefId(UseDepartmentIdProperty, value); }
		}

		/// <summary>
		/// 使用部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> UseDepartmentProperty = P<IdleArchive>.RegisterRef(e => e.UseDepartment, UseDepartmentIdProperty);

		/// <summary>
		/// 使用部门
		/// </summary>
		public Enterprise UseDepartment
		{
			get { return GetRefEntity(UseDepartmentProperty); }
			set { SetRefEntity(UseDepartmentProperty, value); }
		}
		#endregion


		#region 申请人 ApplicantName
		/// <summary>
		/// 申请人
		/// </summary>
		[Label("申请人")]
        public static readonly Property<string> ApplicantNameProperty = P<IdleArchive>.RegisterView(e => e.ApplicantName, p => p.Applicant.Name);

        /// <summary>
        /// 注释
        /// </summary>
        public string ApplicantName
		{
            get { return this.GetProperty(ApplicantNameProperty); }
			set { SetProperty(ApplicantNameProperty, value); }
		}
        #endregion

    }

	/// <summary>
	/// 闲置与封存 实体配置
	/// </summary>
	internal class IdleArchiveConfig : EntityConfig<IdleArchive>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_IDLE_ARCH").MapAllProperties();
			Meta.Property(IdleArchive.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}