using SIE.Common.Users;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Employees
{

    /// <summary>
    /// 生产资源组选择生产资源查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("员工查询实体")]
    public class EmployeeSelectCriteria : Criteria
    {
        //View.Property(p => p.Code);
        //    View.Property(p => p.Name);
        //    View.Property(p => p.HireDate).UseDateEditor();
        //View.Property(p => p.User);
        //    View.Property(p => p.WorkGroup);
        //    View.Property(p => p.EmployeeStatus);
        //    View.Property(p => p.Sex);
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EmployeeSelectCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>        
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EmployeeSelectCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 入职时间 HireDate
        /// <summary>
        /// 入职时间
        /// </summary>
        [Label("入职时间")]
        public static readonly Property<DateTime?> HireDateProperty = P<EmployeeSelectCriteria>.Register(e => e.HireDate);

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? HireDate
        {
            get { return GetProperty(HireDateProperty); }
            set { SetProperty(HireDateProperty, value); }
        }
        #endregion

        #region 用户 User
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("用户")]
        public static readonly IRefIdProperty UserIdProperty =
            P<EmployeeSelectCriteria>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 用户Id
        /// </summary>
        public double? UserId
        {
            get { return (double?)this.GetRefNullableId(UserIdProperty); }
            set { this.SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static readonly RefEntityProperty<User> UserProperty =
            P<EmployeeSelectCriteria>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty = P<EmployeeSelectCriteria>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double?)GetRefNullableId(WorkGroupIdProperty); }
            set { SetRefNullableId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<EmployeeSelectCriteria>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return GetRefEntity(WorkGroupProperty); }
            set { SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 员工状态 EmployeeStatus
        /// <summary>
        /// 员工状态
        /// </summary>
        [Label("员工状态")]
        public static readonly Property<EmployeeStatus?> EmployeeStatusProperty = P<EmployeeSelectCriteria>.Register(e => e.EmployeeStatus);

        /// <summary>
        /// 员工状态
        /// </summary>
        public EmployeeStatus? EmployeeStatus
        {
            get { return GetProperty(EmployeeStatusProperty); }
            set { SetProperty(EmployeeStatusProperty, value); }
        }
        #endregion

        #region 性别 Sex
        /// <summary>
        /// 性别
        /// </summary>
        [Label("性别")]
        public static readonly Property<Sex?> SexProperty = P<EmployeeSelectCriteria>.Register(e => e.Sex);

        /// <summary>
        /// 性别
        /// </summary>
        public Sex? Sex
        {
            get { return GetProperty(SexProperty); }
            set { SetProperty(SexProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>资源组列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EmployeeController>().GetEmployees(this);
        }
    }
}
