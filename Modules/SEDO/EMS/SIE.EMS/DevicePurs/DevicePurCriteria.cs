using SIE.Domain;
using SIE.ObjectModel;
using SIE.Rbac.Users;
using System;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 设备与人员权限查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备与人员权限查询实体")]
    public partial class DevicePurCriteria : Criteria
    {
        #region 设备与人员权限与用户的关系 Employee
        /// <summary>
        /// 设备与人员权限与用户的关系Id
        /// </summary>
        public static readonly IRefIdProperty EmployeeIdProperty = P<DevicePurCriteria>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);
        /// <summary>
        /// 设备与人员权限与用户的关系
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 设备与人员权限与用户的关系
        /// </summary>
        public static readonly RefEntityProperty<User> EmployeeProperty = P<DevicePurCriteria>.RegisterRef(e => e.Employee, EmployeeIdProperty);
        /// <summary>
        /// 设备与人员权限与用户的关系
        /// </summary>
        public User Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 设备与人员权限与用户组的关系 UserGroup
        /// <summary>
        /// 设备与人员权限与用户组的关系Id
        /// </summary>
        public static readonly IRefIdProperty UserGroupIdProperty = P<DevicePurCriteria>.RegisterRefId(e => e.UserGroupId, ReferenceType.Normal);
        /// <summary>
        /// 设备与人员权限与用户组的关系
        /// </summary>
        public double? UserGroupId
        {
            get { return (double?)GetRefId(UserGroupIdProperty); }
            set { SetRefId(UserGroupIdProperty, value); }
        }

        /// <summary>
        /// 设备与人员权限与用户组的关系
        /// </summary>
        public static readonly RefEntityProperty<UserGroup> UserGroupProperty = P<DevicePurCriteria>.RegisterRef(e => e.UserGroup, UserGroupIdProperty);
        /// <summary>
        /// 设备与人员权限与用户组的关系
        /// </summary>
        public UserGroup UserGroup
        {
            get { return GetRefEntity(UserGroupProperty); }
            set { SetRefEntity(UserGroupProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 用户编码 EmployeeCode
        /// <summary>
        /// 用户编码
        /// </summary>
        [Label("用户编码")]
        public static readonly Property<string> EmployeeCodeProperty = P<DevicePurCriteria>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 用户编码
        /// </summary>
        public string EmployeeCode
        {
            get { return GetProperty(EmployeeCodeProperty); }
            set { SetProperty(EmployeeCodeProperty, value); }
        }
        #endregion

        #region 用户名称 EmployeeName
        /// <summary>
        /// 用户名称
        /// </summary>
        [Label("用户名称")]
        public static readonly Property<string> EmployeeNameProperty = P<DevicePurCriteria>.RegisterView(e => e.EmployeeName, p => p.Employee.Employee.Name);

        /// <summary>
        /// 用户名称
        /// </summary>
        public string EmployeeName
        {
            get { return GetProperty(EmployeeNameProperty); }
            set { SetProperty(EmployeeNameProperty, value); }
        }
        #endregion

        #region 用户组编码 UserGroupCode
        /// <summary>
        /// 用户组编码
        /// </summary>
        [Label("用户组编码")]
        public static readonly Property<string> UserGroupCodeProperty = P<DevicePurCriteria>.RegisterView(e => e.UserGroupCode, p => p.UserGroup.Code);

        /// <summary>
        /// 用户组编码
        /// </summary>
        public string UserGroupCode
        {
            get { return GetProperty(UserGroupCodeProperty); }
            set { SetProperty(UserGroupCodeProperty, value); }
        }
        #endregion

        #region 用户组名称 UserGroupName
        /// <summary>
        ///  用户组名称
        /// </summary>
        [Label("用户组名称")]
        public static readonly Property<string> UserGroupNameProperty = P<DevicePurCriteria>.RegisterView(e => e.UserGroupName, p => p.UserGroup.Name);

        /// <summary>
        /// 用户组名称
        /// </summary>
        public string UserGroupName
        {
            get { return GetProperty(UserGroupNameProperty); }
            set { SetProperty(UserGroupNameProperty, value); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DevicePurController>().GetDevicePurs(this);
        }
    }
}
