using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.IdleArchives
{
	/// <summary>
	/// 资产调拨查询实体
	/// </summary>
	[QueryEntity, Serializable]
    [Label("闲置封存查询实体")]
    public class IdleArchiveCriteria : Criteria
    {

		#region 单号 No
		/// <summary>
		/// 单号
		/// </summary>
		[Label("单号")]
		public static readonly Property<string> TransferNoProperty = P<IdleArchiveCriteria>.Register(e => e.No);

		/// <summary>
		/// 单号
		/// </summary>
		public string No
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
		public static readonly IRefIdProperty QureyFactoryIdProperty = P<IdleArchiveCriteria>.RegisterRefId(e => e.QureyFactoryId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> QureyFactoryProperty = P<IdleArchiveCriteria>.RegisterRef(e => e.QureyFactory, QureyFactoryIdProperty);

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
		public static readonly Property<IdleArchiveType?> IdleArchiveTypeProperty = P<IdleArchiveCriteria>.Register(e => e.IdleArchiveType);

		/// <summary>
		/// 业务类型
		/// </summary>
		public IdleArchiveType? IdleArchiveType
		{
			get { return GetProperty(IdleArchiveTypeProperty); }
			set { SetProperty(IdleArchiveTypeProperty, value); }
		}
		#endregion



		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<IdleArchiveCriteria>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus? ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion


		#region 管理部门 TargetManageDept
		/// <summary>
		/// 管理部门Id
		/// </summary>
		[Label("管理部门")]
		public static readonly IRefIdProperty ManageDeptIdProperty = P<IdleArchiveCriteria>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

		/// <summary>
		/// 目标管理部门Id
		/// </summary>
		public double? ManageDeptId
		{
			get { return (double?)GetRefNullableId(ManageDeptIdProperty); }
			set { SetRefNullableId(ManageDeptIdProperty, value); }
		}

		/// <summary>
		/// 目标管理部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<IdleArchiveCriteria>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

		/// <summary>
		/// 目标管理部门
		/// </summary>
		public Enterprise ManageDept
		{
			get { return GetRefEntity(ManageDeptProperty); }
			set { SetRefEntity(ManageDeptProperty, value); }
		}
		#endregion

		#region 使用部门 UseDept
		/// <summary>
		/// 使用部门Id
		/// </summary>
		[Label("使用部门")]
		public static readonly IRefIdProperty UseDeptIdProperty = P<IdleArchiveCriteria>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

		/// <summary>
		/// 使用部门Id
		/// </summary>
		public double? UseDeptId
		{
			get { return (double?)GetRefNullableId(UseDeptIdProperty); }
			set { SetRefNullableId(UseDeptIdProperty, value); }
		}

		/// <summary>
		/// 目标管理部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<IdleArchiveCriteria>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

		/// <summary>
		/// 目标管理部门
		/// </summary>
		public Enterprise UseDept
		{
			get { return GetRefEntity(UseDeptProperty); }
			set { SetRefEntity(UseDeptProperty, value); }
		}
		#endregion




		#region 申请人 Applicant
		/// <summary>
		/// 申请人Id
		/// </summary>
		[Label("申请人")]
		public static readonly IRefIdProperty ApplicantIdProperty = P<IdleArchiveCriteria>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Employee> ApplicantProperty = P<IdleArchiveCriteria>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

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
		public static readonly Property<DateRange> CreateDateProperty = P<IdleArchiveCriteria>.Register(e => e.CreateDate);

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
			return RT.Service.Resolve<IdleArchivesController>().Fetch(this);

		}
	}
}
