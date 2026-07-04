using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Lubrications
{

    /// <summary>
    /// 润滑记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class LubricationCriteria : Criteria
    {
		#region 润滑单号 LubricationNo
		/// <summary>
		/// 润滑单号
		/// </summary>
		[Label("润滑单号")]
		public static readonly Property<string> LubricationNoProperty = P<LubricationCriteria>.Register(e => e.LubricationNo);

		/// <summary>
		/// 润滑单号
		/// </summary>
		public string LubricationNo
		{
			get { return GetProperty(LubricationNoProperty); }
			set { SetProperty(LubricationNoProperty, value); }
		}
		#endregion

		#region 设备台账 EquipAccount
		/// <summary>
		/// 设备台账Id
		/// </summary>
		public static readonly IRefIdProperty EquipAccountIdProperty = P<LubricationCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

		/// <summary>
		/// 设备台账Id
		/// </summary>
		public double? EquipAccountId
		{
			get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
			set { SetRefNullableId(EquipAccountIdProperty, value); }
		}

		/// <summary>
		/// 设备台账
		/// </summary>
		public static readonly RefEntityProperty<EquipAccountSelect> EquipmentAccountProperty = P<LubricationCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

		/// <summary>
		/// 设备台账
		/// </summary>
		public EquipAccountSelect EquipAccount
		{
			get { return GetRefEntity(EquipmentAccountProperty); }
			set { SetRefEntity(EquipmentAccountProperty, value); }
		}
		#endregion

		#region 设备名称 EquipAccountName
		/// <summary>
		/// 设备名称
		/// </summary>
		[Label("设备名称")]
		public static readonly Property<string> EquipAccountNameProperty = P<LubricationCriteria>.Register(e => e.EquipAccountName);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipAccountName
		{
			get { return this.GetProperty(EquipAccountNameProperty); }
			set { SetProperty(EquipAccountNameProperty, value); }
		}
		#endregion

		#region 设备类型 EquipType
		/// <summary>
		/// 设备类型Id
		/// </summary>
		public static readonly IRefIdProperty EquipTypeIdProperty = P<LubricationCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

		/// <summary>
		/// 设备类型Id
		/// </summary>
		public double? EquipTypeId
		{
			get { return (double?)GetRefNullableId(EquipTypeIdProperty); }
			set { SetRefNullableId(EquipTypeIdProperty, value); }
		}

		/// <summary>
		/// 设备类型
		/// </summary>
		public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<LubricationCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

		/// <summary>
		/// 设备类型
		/// </summary>
		public EquipType EquipType
		{
			get { return GetRefEntity(EquipTypeProperty); }
			set { SetRefEntity(EquipTypeProperty, value); }
		}
		#endregion

		#region 设备型号维护 EquipModel
		/// <summary>
		/// 设备型号维护Id
		/// </summary>
		public static  readonly IRefIdProperty EquipModelIdProperty = P<LubricationCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

		/// <summary>
		/// 设备型号维护Id
		/// </summary>
		public  double? EquipModelId
		{
			get { return (double?)GetRefNullableId(EquipModelIdProperty); }
			set { SetRefNullableId(EquipModelIdProperty, value); }
		}

		/// <summary>
		/// 设备型号维护
		/// </summary>
		public static  readonly RefEntityProperty<EquipModel> EquipModelProperty = P<LubricationCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

		/// <summary>
		/// 设备型号维护
		/// </summary>
		public  EquipModel EquipModel
		{
			get { return GetRefEntity(EquipModelProperty); }
			set { SetRefEntity(EquipModelProperty, value); }
		}
		#endregion

		#region 润滑执行状态 LubricationStatus
		/// <summary>
		/// 润滑执行状态
		/// </summary>
		[Label("润滑执行状态")]
		public static readonly Property<LubricationStatus?> LubricationStatusProperty = P<LubricationCriteria>.Register(e => e.LubricationStatus);

		/// <summary>
		/// 润滑执行状态
		/// </summary>
		public LubricationStatus? LubricationStatus
		{
			get { return GetProperty(LubricationStatusProperty); }
			set { SetProperty(LubricationStatusProperty, value); }
		}
		#endregion

		#region 车间 WorkShop
		/// <summary>
		/// 车间Id
		/// </summary>
		[Label("车间")]
		public static readonly IRefIdProperty WorkShopIdProperty = P<LubricationCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

		/// <summary>
		/// 车间Id
		/// </summary>
		public double? WorkShopId
		{
			get { return (double?)GetRefNullableId(WorkShopIdProperty); }
			set { SetRefNullableId(WorkShopIdProperty, value); }
		}

		/// <summary>
		/// 车间
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<LubricationCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

		/// <summary>
		/// 车间
		/// </summary>
		public Enterprise WorkShop
		{
			get { return GetRefEntity(WorkShopProperty); }
			set { SetRefEntity(WorkShopProperty, value); }
		}
		#endregion

		#region 使用部门 UseDepartment
		/// <summary>
		/// 使用部门Id
		/// </summary>
		[Label("部门")]
		public static readonly IRefIdProperty UseDepartmentIdProperty =P<LubricationCriteria>.RegisterRefId(e => e.UseDepartmentId, ReferenceType.Normal);

		/// <summary>
		/// 使用部门Id
		/// </summary>
		public double? UseDepartmentId
		{
			get { return (double?)this.GetRefNullableId(UseDepartmentIdProperty); }
			set { this.SetRefNullableId(UseDepartmentIdProperty, value); }
		}

		/// <summary>
		/// 使用部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> UseDepartmentProperty =P<LubricationCriteria>.RegisterRef(e => e.UseDepartment, UseDepartmentIdProperty);

		/// <summary>
		/// 使用部门
		/// </summary>
		public Enterprise UseDepartment
		{
			get { return this.GetRefEntity(UseDepartmentProperty); }
			set { this.SetRefEntity(UseDepartmentProperty, value); }
		}
		#endregion

		#region 是否逾期 IsOverdue
		/// <summary>
		/// 是否逾期
		/// </summary>
		[Label("是否逾期")]
		public static readonly Property<bool?> IsOverdueProperty = P<LubricationCriteria>.Register(e => e.IsOverdue);

		/// <summary>
		/// 是否逾期
		/// </summary>
		public bool? IsOverdue
		{
			get { return this.GetProperty(IsOverdueProperty); }
			set { SetProperty(IsOverdueProperty, value); }
		}
		#endregion

		#region 计划日期 PlanDate
		/// <summary>
		/// 计划日期
		/// </summary>
		[Label("计划日期")]
		public static readonly Property<DateRange> PlanDateProperty = P<LubricationCriteria>.Register(e => e.PlanDate);

		/// <summary>
		/// 计划日期
		/// </summary>
		public DateRange PlanDate
		{
			get { return GetProperty(PlanDateProperty); }
			set { SetProperty(PlanDateProperty, value); }
		}
		#endregion

		#region 润滑开始日期 StartDateTime
		/// <summary>
		/// 润滑开始日期
		/// </summary>
		[Label("润滑日期")]
		public static readonly Property<DateRange> StartDateTimeProperty = P<LubricationCriteria>.Register(e => e.StartDateTime);

		/// <summary>
		/// 润滑开始日期
		/// </summary>
		public DateRange StartDateTime
		{
			get { return GetProperty(StartDateTimeProperty); }
			set { SetProperty(StartDateTimeProperty, value); }
		}
		#endregion

		/// <summary>
		/// 查询方法
		/// </summary>
		/// <returns>检验规程列表</returns>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<LubricationController>().GetLubricationList(this);
		}

	}
}
