using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.AssetTransfers
{
    /// <summary>
    /// 资产调拨查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("资产调拨查询实体")]
    public partial class AssetTransferCriteria : Criteria
    {
        #region 调拨单号 TransferNo
        /// <summary>
        /// 调拨单号
        /// </summary>
        [Label("调拨单号")]
        public static readonly Property<string> TransferNoProperty = P<AssetTransferCriteria>.Register(e => e.TransferNo);

        /// <summary>
        /// 调拨单号
        /// </summary>
        public string TransferNo
        {
            get { return this.GetProperty(TransferNoProperty); }
            set { this.SetProperty(TransferNoProperty, value); }
        }
		#endregion

		#region 工厂 QureyFactory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty QureyFactoryIdProperty = P<AssetTransferCriteria>.RegisterRefId(e => e.QureyFactoryId, ReferenceType.Normal);

		/// <summary>
		/// 工厂Id
		/// </summary>
		public double? QureyFactoryId
		{
			get { return (double?)GetRefNullableId(QureyFactoryIdProperty); }
			set { SetRefNullableId(QureyFactoryIdProperty, value); }
		}

		/// <summary>
		/// 工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> QureyFactoryProperty = P<AssetTransferCriteria>.RegisterRef(e => e.QureyFactory, QureyFactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise QureyFactory
		{
			get { return GetRefEntity(QureyFactoryProperty); }
			set { SetRefEntity(QureyFactoryProperty, value); }
		}
		#endregion


		#region 业务类型 TransferType
		/// <summary>
		/// 业务类型
		/// </summary>
		[Label("业务类型")]
		public static readonly Property<TransferType?> TransferTypeProperty = P<AssetTransferCriteria>.Register(e => e.TransferType);

		/// <summary>
		/// 业务类型
		/// </summary>
		public TransferType? TransferType
		{
			get { return GetProperty(TransferTypeProperty); }
			set { SetProperty(TransferTypeProperty, value); }
		}
		#endregion



		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<AssetTransferCriteria>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus? ApprovalStatus
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
		public static readonly IRefIdProperty TargetManageDeptIdProperty = P<AssetTransferCriteria>.RegisterRefId(e => e.TargetManageDeptId, ReferenceType.Normal);

		/// <summary>
		/// 目标管理部门Id
		/// </summary>
		public double? TargetManageDeptId
		{
			get { return (double?)GetRefNullableId(TargetManageDeptIdProperty); }
			set { SetRefNullableId(TargetManageDeptIdProperty, value); }
		}

		/// <summary>
		/// 目标管理部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> TargetManageDeptProperty = P<AssetTransferCriteria>.RegisterRef(e => e.TargetManageDept, TargetManageDeptIdProperty);

		/// <summary>
		/// 目标管理部门
		/// </summary>
		public Enterprise TargetManageDept
		{
			get { return GetRefEntity(TargetManageDeptProperty); }
			set { SetRefEntity(TargetManageDeptProperty, value); }
		}
		#endregion



		#region 原管理部门 ManageDept
		/// <summary>
		/// 原管理部门Id
		/// </summary>
		[Label("原管理部门")]
		public static readonly IRefIdProperty ManageDeptIdProperty = P<AssetTransferCriteria>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

		/// <summary>
		/// 原管理部门Id
		/// </summary>
		public double? ManageDeptId
		{
			get { return (double?)GetRefNullableId(ManageDeptIdProperty); }
			set { SetRefNullableId(ManageDeptIdProperty, value); }
		}

		/// <summary>
		/// 原管理部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<AssetTransferCriteria>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

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
		public static readonly IRefIdProperty ApplicantIdProperty = P<AssetTransferCriteria>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

		/// <summary>
		/// 申请人Id
		/// </summary>
		public double? ApplicantId
		{
			get { return (double?)GetRefNullableId(ApplicantIdProperty); }
			set { SetRefNullableId(ApplicantIdProperty, value); }
		}

		/// <summary>
		/// 申请人
		/// </summary>
		public static readonly RefEntityProperty<Employee> ApplicantProperty = P<AssetTransferCriteria>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

		/// <summary>
		/// 申请人
		/// </summary>
		public Employee Applicant
		{
			get { return GetRefEntity(ApplicantProperty); }
			set { SetRefEntity(ApplicantProperty, value); }
		}
		#endregion


		#region 创建日期 CreateDate
		/// <summary>
		/// 创建日期
		/// </summary>
		[Label("创建日期")]
		public static readonly Property<DateRange> CreateDateProperty = P<AssetTransferCriteria>.Register(e => e.CreateDate);

		/// <summary>
		/// 创建日期
		/// </summary>
		public DateRange CreateDate
		{
			get { return GetProperty(CreateDateProperty); }
			set { SetProperty(CreateDateProperty, value); }
		}
        #endregion

		/// <summary>
		/// 重写实体查询
		/// </summary>
		/// <returns></returns>
        protected override EntityList Fetch()
        {
			return RT.Service.Resolve<AssetTransfersController>().Fetch(this);

		}
    }
}
