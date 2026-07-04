using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 借调员工
    /// </summary>
    [ChildEntity, Serializable]
    [Label("借调员工")]
    public partial class OnLoanEmployee : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<OnLoanEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<OnLoanEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 班组借调 OnLoan
        /// <summary>
        /// 班组借调Id
        /// </summary>
        [Label("班组借调")]
        public static readonly IRefIdProperty OnLoanIdProperty = P<OnLoanEmployee>.RegisterRefId(e => e.OnLoanId, ReferenceType.Parent);

        /// <summary>
        /// 班组借调Id
        /// </summary>
        public double OnLoanId
        {
            get { return (double)GetRefId(OnLoanIdProperty); }
            set { SetRefId(OnLoanIdProperty, value); }
        }

        /// <summary>
        /// 班组借调
        /// </summary>
        public static readonly RefEntityProperty<WorkGroupOnLoan> OnLoanProperty = P<OnLoanEmployee>.RegisterRef(e => e.OnLoan, OnLoanIdProperty);

        /// <summary>
        /// 班组借调
        /// </summary>
        public WorkGroupOnLoan OnLoan
        {
            get { return GetRefEntity(OnLoanProperty); }
            set { SetRefEntity(OnLoanProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 借调员工 实体配置
    /// </summary>
    internal class OnLoanEmployeeConfig : EntityConfig<OnLoanEmployee>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_ON_LOAN_EMP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}