using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EquipRepair
{
    /// <summary>
    /// 设备维修单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备维修单查询实体")]
    public partial class PlanRepairCriteria : Criteria
    {

        #region 计划单号 No
        /// <summary>
        /// 计划单号
        /// </summary>
        [Label("计划单号")]
        public static readonly Property<string> NoProperty = P<PlanRepairCriteria>.Register(e => e.No);

        /// <summary>
        /// 计划单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<PlanRepairCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<PlanRepairCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<PlanRepairCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 定标类型 StandardType
        /// <summary>
        /// 定标类型
        /// </summary>
        [Label("定标类型")]
        public static readonly Property<StandardType?> StandardTypeProperty = P<PlanRepairCriteria>.Register(e => e.StandardType);

        /// <summary>
        /// 定标类型
        /// </summary>
        public StandardType? StandardType
        {
            get { return GetProperty(StandardTypeProperty); }
            set { SetProperty(StandardTypeProperty, value); }
        }
        #endregion

        #region 创建人 Create
        /// <summary>
        /// 创建人Id
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty CreateIdProperty =
            P<PlanRepairCriteria>.RegisterRefId(e => e.CreateId, ReferenceType.Normal);

        /// <summary>
        /// 创建人Id
        /// </summary>
        public double? CreateId
        {
            get { return (double?)this.GetRefNullableId(CreateIdProperty); }
            set { this.SetRefNullableId(CreateIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreateProperty =
            P<PlanRepairCriteria>.RegisterRef(e => e.Create, CreateIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee Create
        {
            get { return this.GetRefEntity(CreateProperty); }
            set { this.SetRefEntity(CreateProperty, value); }
        }
        #endregion

        #region 计划日期 PlanDate
        /// <summary>
        /// 计划日期
        /// </summary>
        [Label("计划日期")]
        public static readonly Property<DateRange> PlanDateProperty = P<PlanRepairCriteria>.Register(e => e.PlanDate);

        /// <summary>
        /// 计划日期
        /// </summary>
        public DateRange PlanDate
        {
            get { return GetProperty(PlanDateProperty); }
            set { SetProperty(PlanDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PlanRepairsController>().CriteriaPlanRepairss(this);
        }
    }
}
