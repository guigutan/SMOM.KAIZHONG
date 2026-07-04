
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("点检计划查询实体")]
    public partial class CheckPlanCriteria : Criteria
    {
        /// <summary>
        /// 设备类别快码组
        /// </summary>
        public const string EquipTypeCatalogType = "EQUIP_TYPE";

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<CheckPlanCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<CheckPlanCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<CheckPlanCriteria>.Register(e => e.EquipAccountName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { this.SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<CheckPlanCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<CheckPlanCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备类别 TypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> TypeCategoryProperty = P<CheckPlanCriteria>.Register(e => e.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory
        {
            get { return this.GetProperty(TypeCategoryProperty); }
            set { this.SetProperty(TypeCategoryProperty, value); }
        }
        #endregion


        #region 月份 Month
        /// <summary>
        /// 月份
        /// </summary>
        [Label("年月")]
        public static readonly Property<DateTime?> MonthProperty = P<CheckPlanCriteria>.Register(e => e.Month);

        /// <summary>
        /// 月份
        /// </summary>
        public DateTime? Month
        {
            get { return GetProperty(MonthProperty); }
            set { SetProperty(MonthProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        ///// <summary>
        ///// 产线Id
        ///// </summary>
        //[Label("产线")]
        //public static readonly IRefIdProperty WipResourceIdProperty =
        //    P<CheckPlanCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        ///// <summary>
        ///// 产线Id
        ///// </summary>
        //public double? WipResourceId
        //{
        //    get { return (double?)this.GetRefId(WipResourceIdProperty); }
        //    set { this.SetRefId(WipResourceIdProperty, value); }
        //}

        ///// <summary>
        ///// 产线
        ///// </summary>
        //public static readonly RefEntityProperty<WipResource> WipResourceProperty =
        //    P<CheckPlanCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        ///// <summary>
        ///// 产线
        ///// </summary>
        //public WipResource WipResource
        //{
        //    get { return this.GetRefEntity(WipResourceProperty); }
        //    set { this.SetRefEntity(WipResourceProperty, value); }
        //}
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<CheckPlanCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WipResourceProperty =
            P<CheckPlanCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<CheckPlanCriteria>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<CheckPlanCriteria>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 设备编码 EquipCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipCodeProperty = P<CheckPlanCriteria>.Register(e => e.EquipCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode
        {
            get { return GetProperty(EquipCodeProperty); }
            set { SetProperty(EquipCodeProperty, value); }
        }
        #endregion

        #region 机台号 MachineNo
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> MachineNoProperty = P<CheckPlanCriteria>.Register(e => e.MachineNo);

        /// <summary>
        /// 机台号
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion

        #region 周期类型 CheckCycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CheckCycleType?> CheckCycleTypeProperty = P<CheckPlanCriteria>.Register(e => e.CheckCycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CheckCycleType? CheckCycleType
        {
            get { return GetProperty(CheckCycleTypeProperty); }
            set { SetProperty(CheckCycleTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<CheckPlanCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<CheckPlanCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion


        #region 设备状态 State
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        public static readonly Property<AccountState?> StateProperty = P<CheckPlanCriteria>.Register(e => e.State);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 管理状态 UseState 
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState?> UseStateProperty = P<CheckPlanCriteria>.Register(e => e.UseState);

        /// <summary>
        /// 管理状态（原使用状态）
        /// </summary>
        public AccountUseState? UseState
        {
            get { return GetProperty(UseStateProperty); }
            set { SetProperty(UseStateProperty, value); }
        }
        #endregion

        #region 管理部门 ManageDepartment
        /// <summary>
        /// 管理部门Id
        /// </summary>
        [Label("管理部门")]
        public static readonly IRefIdProperty ManageDepartmentIdProperty =
            P<CheckPlanCriteria>.RegisterRefId(e => e.ManageDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 管理部门Id
        /// </summary>
        public double? ManageDepartmentId
        {
            get { return (double?)this.GetRefNullableId(ManageDepartmentIdProperty); }
            set { this.SetRefNullableId(ManageDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 管理部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ManageDepartmentProperty =
            P<CheckPlanCriteria>.RegisterRef(e => e.ManageDepartment, ManageDepartmentIdProperty);

        /// <summary>
        /// 管理部门
        /// </summary>
        public Enterprise ManageDepartment
        {
            get { return this.GetRefEntity(ManageDepartmentProperty); }
            set { this.SetRefEntity(ManageDepartmentProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            if (!this.Month.HasValue)
            {
                throw new ValidationException("查询条件【年月】不能为空，请检查！".L10N());
            }

            return RT.Service.Resolve<CheckPlanController>().GetEquipCheckPlans(this);
        }
    }
}
