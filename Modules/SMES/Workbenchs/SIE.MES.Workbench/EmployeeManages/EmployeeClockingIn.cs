using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.Workbench.EmployeeManages
{
    /// <summary>
    /// 员工出勤
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("员工出勤")]
    public class EmployeeClockingIn : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeClockingIn()
        {
            ClockingInDate = DateTime.Today;
        }

        #region 出勤日期 ClockingInDate
        /// <summary>
        /// 出勤日期
        /// </summary>
        [Label("出勤日期")]
        public static readonly Property<DateTime> ClockingInDateProperty = P<EmployeeClockingIn>.Register(e => e.ClockingInDate);

        /// <summary>
        /// 出勤日期
        /// </summary>
        public DateTime ClockingInDate
        {
            get { return this.GetProperty(ClockingInDateProperty); }
            set { this.SetProperty(ClockingInDateProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<EmployeeClockingIn>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)this.GetRefId(EmployeeIdProperty); }
            set { this.SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty =
            P<EmployeeClockingIn>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion 
    }

    internal class EmployeeCheckingInEntityConfig : EntityConfig<EmployeeClockingIn>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMP_CLOCKING_IN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}