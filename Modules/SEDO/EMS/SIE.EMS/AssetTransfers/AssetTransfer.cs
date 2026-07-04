using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using Employee = SIE.Resources.Employee;

namespace SIE.EMS.AssetTransfers
{
    /// <summary>
    /// 资产调拨
    /// </summary>
    [RootEntity, Serializable]
	[Label("资产调拨")]
	[ConditionQueryType(typeof(AssetTransferCriteria))]
	[DisplayMember(nameof(TransferNo))]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(SourceFactoryId), true)]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(TargetFactoryId), true)]
	[BussinessDepartmentAuth(nameof(TargetManageDeptId), true)]
	[BussinessDepartmentAuth(nameof(ManageDeptId), true)]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityWithConfig(typeof(NoConfig), "资产调拨单号配置项", "资产调拨单号生成规则")]
	public partial class AssetTransfer : DataEntity
	{
		#region 调拨单号 TransferNo
		/// <summary>
		/// 调拨单号
		/// </summary>
		[Label("调拨单号")]
		public static readonly Property<string> TransferNoProperty = P<AssetTransfer>.Register(e => e.TransferNo);

		/// <summary>
		/// 调拨单号
		/// </summary>
		public string TransferNo
		{
			get { return GetProperty(TransferNoProperty); }
			set { SetProperty(TransferNoProperty, value); }
		}
		#endregion

		#region 固定资产 IsAsset
		/// <summary>
		/// 固定资产
		/// </summary>
		[Label("固定资产")]
		public static readonly Property<bool> IsAssetProperty = P<AssetTransfer>.Register(e => e.IsAsset);

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
		public static readonly Property<DateTime> ApplyDateProperty = P<AssetTransfer>.Register(e => e.ApplyDate);

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
		public static readonly Property<string> RemarkProperty = P<AssetTransfer>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 原工厂 SourceFactory
		/// <summary>
		/// 原工厂Id
		/// </summary>
		[Label("原工厂")]
		public static readonly IRefIdProperty SourceFactoryIdProperty = P<AssetTransfer>.RegisterRefId(e => e.SourceFactoryId, ReferenceType.Normal);

		/// <summary>
		/// 原工厂Id
		/// </summary>
		public double SourceFactoryId
		{
			get { return (double)GetRefId(SourceFactoryIdProperty); }
			set { SetRefId(SourceFactoryIdProperty, value); }
		}

		/// <summary>
		/// 原工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> SourceFactoryProperty = P<AssetTransfer>.RegisterRef(e => e.SourceFactory, SourceFactoryIdProperty);

		/// <summary>
		/// 原工厂
		/// </summary>
		public Enterprise SourceFactory
		{
			get { return GetRefEntity(SourceFactoryProperty); }
			set { SetRefEntity(SourceFactoryProperty, value); }
		}
		#endregion

		#region 目标工厂 TargetFactory
		/// <summary>
		/// 目标工厂Id
		/// </summary>
		[Label("目标工厂")]
		public static readonly IRefIdProperty TargetFactoryIdProperty = P<AssetTransfer>.RegisterRefId(e => e.TargetFactoryId, ReferenceType.Normal);

		/// <summary>
		/// 目标工厂Id
		/// </summary>
		public double TargetFactoryId
		{
			get { return (double)GetRefId(TargetFactoryIdProperty); }
			set { SetRefId(TargetFactoryIdProperty, value); }
		}

		/// <summary>
		/// 目标工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> TargetFactoryProperty = P<AssetTransfer>.RegisterRef(e => e.TargetFactory, TargetFactoryIdProperty);

		/// <summary>
		/// 目标工厂
		/// </summary>
		public Enterprise TargetFactory
		{
			get { return GetRefEntity(TargetFactoryProperty); }
			set { SetRefEntity(TargetFactoryProperty, value); }
		}
		#endregion

		#region 原管理部门 ManageDept
		/// <summary>
		/// 原管理部门Id
		/// </summary>
		[Label("原管理部门")]
		public static readonly IRefIdProperty ManageDeptIdProperty = P<AssetTransfer>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

		/// <summary>
		/// 原管理部门Id
		/// </summary>
		public double ManageDeptId
		{
			get { return (double)GetRefId(ManageDeptIdProperty); }
			set { SetRefId(ManageDeptIdProperty, value); }
		}

		/// <summary>
		/// 原管理部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<AssetTransfer>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

		/// <summary>
		/// 原管理部门
		/// </summary>
		public Enterprise ManageDept
		{
			get { return GetRefEntity(ManageDeptProperty); }
			set { SetRefEntity(ManageDeptProperty, value); }
		}
		#endregion

		#region 申请人 applicant
		/// <summary>
		/// 申请人Id
		/// </summary>
		[Label("申请人")]
		public static readonly IRefIdProperty ApplicantIdProperty = P<AssetTransfer>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Employee> applicantProperty = P<AssetTransfer>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

		/// <summary>
		/// 申请人
		/// </summary>
		public Employee Applicant
		{
			get { return GetRefEntity(applicantProperty); }
			set { SetRefEntity(applicantProperty, value); }
		}
		#endregion

		#region 调拨状态 TransferStatus
		/// <summary>
		/// 调拨状态
		/// </summary>
		[Label("调拨状态")]
		public static readonly Property<TransferStatus> TransferStatusProperty = P<AssetTransfer>.Register(e => e.TransferStatus);

		/// <summary>
		/// 调拨状态
		/// </summary>
		public TransferStatus TransferStatus
		{
			get { return GetProperty(TransferStatusProperty); }
			set { SetProperty(TransferStatusProperty, value); }
		}
		#endregion

		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<AssetTransfer>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 目标管理部门 TargetManageDept
		/// <summary>
		/// 目标管理部门Id
		/// </summary>
		[Label("目标管理部门")]
		public static readonly IRefIdProperty TargetManageDeptIdProperty = P<AssetTransfer>.RegisterRefId(e => e.TargetManageDeptId, ReferenceType.Normal);

		/// <summary>
		/// 目标管理部门Id
		/// </summary>
		public double TargetManageDeptId
		{
			get { return (double)GetRefId(TargetManageDeptIdProperty); }
			set { SetRefId(TargetManageDeptIdProperty, value); }
		}

		/// <summary>
		/// 目标管理部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> TargetManageDeptProperty = P<AssetTransfer>.RegisterRef(e => e.TargetManageDept, TargetManageDeptIdProperty);

		/// <summary>
		/// 目标管理部门
		/// </summary>
		public Enterprise TargetManageDept
		{
			get { return GetRefEntity(TargetManageDeptProperty); }
			set { SetRefEntity(TargetManageDeptProperty, value); }
		}
		#endregion

		#region 原使用部门 UseDept
		/// <summary>
		/// 使用部门Id
		/// </summary>
		[Label("原使用部门")]
		public static readonly IRefIdProperty UseDeptIdProperty = P<AssetTransfer>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

		/// <summary>
		/// 原使用部门Id
		/// </summary>
		public double? UseDeptId
		{
			get { return (double?)GetRefNullableId(UseDeptIdProperty); }
			set { SetRefNullableId(UseDeptIdProperty, value); }
		}

		/// <summary>
		/// 原使用部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<AssetTransfer>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

		/// <summary>
		/// 原使用部门
		/// </summary>
		public Enterprise UseDept
		{
			get { return GetRefEntity(UseDeptProperty); }
			set { SetRefEntity(UseDeptProperty, value); }
		}
		#endregion

		#region 目标使用部门 TargetUseDepart
		/// <summary>
		/// 目标使用部门Id
		/// </summary>
		[Label("目标使用部门")]
		public static readonly IRefIdProperty TargetUseDepartIdProperty = P<AssetTransfer>.RegisterRefId(e => e.TargetUseDepartId, ReferenceType.Normal);

		/// <summary>
		/// 目标使用部门Id
		/// </summary>
		public double? TargetUseDepartId
		{
			get { return (double?)GetRefNullableId(TargetUseDepartIdProperty); }
			set { SetRefNullableId(TargetUseDepartIdProperty, value); }
		}

		/// <summary>
		/// 目标使用部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> TargetUseDepartProperty = P<AssetTransfer>.RegisterRef(e => e.TargetUseDepart, TargetUseDepartIdProperty);

		/// <summary>
		/// 目标使用部门
		/// </summary>
		public Enterprise TargetUseDepart
		{
			get { return GetRefEntity(TargetUseDepartProperty); }
			set { SetRefEntity(TargetUseDepartProperty, value); }
		}
		#endregion

		#region 设备清单 AssetTransferDetailList
		/// <summary>
		/// 设备清单
		/// </summary>
		public static readonly ListProperty<EntityList<AssetTransferDetail>> AssetTransferDetailListProperty = P<AssetTransfer>.RegisterList(e => e.AssetTransferDetailList);
		/// <summary>
		/// 设备清单
		/// </summary>
		public EntityList<AssetTransferDetail> AssetTransferDetailList
		{
			get { return this.GetLazyList(AssetTransferDetailListProperty); }
		}
		#endregion

		#region 业务类型 TransferType
		/// <summary>
		/// 业务类型
		/// </summary>
		[Label("业务类型")]
		[Required]
		public static readonly Property<TransferType> TransferTypeProperty = P<AssetTransfer>.Register(e => e.TransferType);

		/// <summary>
		/// 业务类型
		/// </summary>
		public TransferType TransferType
		{
			get { return GetProperty(TransferTypeProperty); }
			set { SetProperty(TransferTypeProperty, value); }
		}
		#endregion

		#region 附件 AssetTransferAttachmentList
		/// <summary>
		/// 附件
		/// </summary>
		public static readonly ListProperty<EntityList<AssetTransferAttachment>> AssetTransferAttachmentListProperty = P<AssetTransfer>.RegisterList(e => e.AssetTransferAttachmentList);
		/// <summary>
		/// 附件
		/// </summary>
		public EntityList<AssetTransferAttachment> AssetTransferAttachmentList
		{
			get { return this.GetLazyList(AssetTransferAttachmentListProperty); }
		}
		#endregion


		#region 申请人 ApplicantName	
		/// <summary>
		/// 申请人
		/// </summary>
		[Label("申请人")]
        public static readonly Property<string> ApplicantNameProperty = P<AssetTransfer>.RegisterView(e => e.ApplicantName, p => p.Applicant.Name);

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
	/// 资产调拨 实体配置
	/// </summary>
	internal class AssetTransferConfig : EntityConfig<AssetTransfer>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_TRAN").MapAllProperties();
			Meta.Property(AssetTransfer.RemarkProperty).ColumnMeta.HasLength(2000);
			Meta.EnablePhantoms();
		}
	}
}