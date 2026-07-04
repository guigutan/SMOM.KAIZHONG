using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件用户组与用户关系
    /// </summary>
    [RootEntity, Serializable]
    //[DisplayMember(nameof())]
    [Label("文件用户组与用户关系")]
    public partial class UserInFileUserGroup : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<UserInFileUserGroup>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<UserInFileUserGroup>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 文件用户组 FileUserGroup
        /// <summary>
        /// 文件用户组Id
        /// </summary>
        [Label("文件用户组")]
        public static readonly IRefIdProperty FileUserGroupIdProperty = P<UserInFileUserGroup>.RegisterRefId(e => e.FileUserGroupId, ReferenceType.Normal);

        /// <summary>
        /// 文件用户组Id
        /// </summary>
        public double FileUserGroupId
        {
            get { return (double)GetRefId(FileUserGroupIdProperty); }
            set { SetRefId(FileUserGroupIdProperty, value); }
        }

        /// <summary>
        /// 文件用户组
        /// </summary>
        public static readonly RefEntityProperty<FileUserGroup> FileUserGroupProperty = P<UserInFileUserGroup>.RegisterRef(e => e.FileUserGroup, FileUserGroupIdProperty);

        /// <summary>
        /// 文件用户组
        /// </summary>
        public FileUserGroup FileUserGroup
        {
            get { return GetRefEntity(FileUserGroupProperty); }
            set { SetRefEntity(FileUserGroupProperty, value); }
        }
        #endregion

        #region 注册视图属性
        #region 员工编码 EmployeeCode
        /// <summary>
        /// 员工编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> EmployeeCodeProperty = P<UserInFileUserGroup>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 员工编码
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 员工姓名 EmployeeName
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Label("姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<UserInFileUserGroup>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 文件用户组与用户关系 实体配置
    /// </summary>
    internal class UserInFileUserGroupConfig : EntityConfig<UserInFileUserGroup>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("FMS_USER_FUG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}