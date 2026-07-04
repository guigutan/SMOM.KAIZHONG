using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 工时登记
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工时登记")]
    public partial class WorkHoursRegister : DataEntity
    {
        #region 保养计划 MaintainPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty MaintainPlanIdProperty =
            P<WorkHoursRegister>.RegisterRefId(e => e.MaintainPlanId, ReferenceType.Parent);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double MaintainPlanId
        {
            get { return (double)this.GetRefId(MaintainPlanIdProperty); }
            set { this.SetRefId(MaintainPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainPlan> MaintainPlanProperty =
            P<WorkHoursRegister>.RegisterRef(e => e.MaintainPlan, MaintainPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainPlan MaintainPlan
        {
            get { return this.GetRefEntity(MaintainPlanProperty); }
            set { this.SetRefEntity(MaintainPlanProperty, value); }
        }
        #endregion

        #region 执行人 Employee
        /// <summary>
        /// 执行人Id
        /// </summary>
        [Label("执行人")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<WorkHoursRegister>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 执行人Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 执行人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ExecuteByProperty = P<WorkHoursRegister>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 执行人
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(ExecuteByProperty); }
            set { SetRefEntity(ExecuteByProperty, value); }
        }
        #endregion

        #region 保养开始日期 BeginDay
        /// <summary>
        /// 保养开始日期
        /// </summary>
        [Label("保养开始日期")]
        public static readonly Property<DateTime> BeginDayProperty = P<WorkHoursRegister>.Register(e => e.BeginDay);

        /// <summary>
        /// 保养开始日期
        /// </summary>
        public DateTime BeginDay
        {
            get { return GetProperty(BeginDayProperty); }
            set { SetProperty(BeginDayProperty, value); }
        }
        #endregion

        #region 保养结束日期 EndDay
        /// <summary>
        /// 保养结束日期
        /// </summary>
        [Label("保养结束日期")]
        public static readonly Property<DateTime> EndDayProperty = P<WorkHoursRegister>.Register(e => e.EndDay);

        /// <summary>
        /// 保养结束日期
        /// </summary>
        public DateTime EndDay
        {
            get { return GetProperty(EndDayProperty); }
            set { SetProperty(EndDayProperty, value); }
        }
        #endregion

        #region 工时(H) WorkHours
        /// <summary>
        /// 工时(H)
        /// </summary>
        [Label("工时(H)")]
        public static readonly Property<double> WorkHoursProperty = P<WorkHoursRegister>.Register(e => e.WorkHours);

        /// <summary>
        /// 工时(H)
        /// </summary>
        public double WorkHours
        {
            get { return GetProperty(WorkHoursProperty); }
            set { SetProperty(WorkHoursProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工时登记 实体配置
    /// </summary>
    internal class WorkHoursRegisterConfig : EntityConfig<WorkHoursRegister>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_WORK_RGSTR").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}