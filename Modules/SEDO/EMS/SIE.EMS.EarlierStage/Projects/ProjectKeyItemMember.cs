using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 事项成员
    /// </summary>
    [ChildEntity, Serializable]
    [Label("事项成员")]
    public partial class ProjectKeyItemMember : DataEntity
    {
        #region 职位 Position
        /// <summary>
        /// 职位
        /// </summary>
        [Label("职位")]
        public static readonly Property<string> PositionProperty = P<ProjectKeyItemMember>.Register(e => e.Position);

        /// <summary>
        /// 职位
        /// </summary>
        public string Position
        {
            get { return GetProperty(PositionProperty); }
            set { SetProperty(PositionProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProjectKeyItemMember>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("工号")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<ProjectKeyItemMember>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<ProjectKeyItemMember>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 项目关键事项 ProjectKeyItem
        /// <summary>
        /// 项目关键事项Id
        /// </summary>
        public static readonly IRefIdProperty ProjectKeyItemIdProperty = P<ProjectKeyItemMember>.RegisterRefId(e => e.ProjectKeyItemId, ReferenceType.Parent);

        /// <summary>
        /// 项目关键事项Id
        /// </summary>
        public double ProjectKeyItemId
        {
            get { return (double)GetRefId(ProjectKeyItemIdProperty); }
            set { SetRefId(ProjectKeyItemIdProperty, value); }
        }

        /// <summary>
        /// 项目关键事项
        /// </summary>
        public static readonly RefEntityProperty<ProjectKeyItem> ProjectKeyItemProperty = P<ProjectKeyItemMember>.RegisterRef(e => e.ProjectKeyItem, ProjectKeyItemIdProperty);

        /// <summary>
        /// 项目关键事项
        /// </summary>
        public ProjectKeyItem ProjectKeyItem
        {
            get { return GetRefEntity(ProjectKeyItemProperty); }
            set { SetRefEntity(ProjectKeyItemProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<ProjectKeyItemMember>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
            set { SetProperty(EmployeeCodeProperty, value); }
        }
        #endregion

        #region 姓名 EmployeeName
        /// <summary>
        /// 姓名
        /// </summary>
        [Label("姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<ProjectKeyItemMember>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 联系方式 Phone
        /// <summary>
        /// 联系方式
        /// </summary>
        [Label("联系方式")]
        public static readonly Property<string> PhoneProperty = P<ProjectKeyItemMember>.RegisterView(e => e.Phone, p => p.Employee.Phone);

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone
        {
            get { return this.GetProperty(PhoneProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 事项成员 实体配置
    /// </summary>
    internal class ProjectKeyItemMemberConfig : EntityConfig<ProjectKeyItemMember>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PROJ_KEY_MEMBER").MapAllProperties();
            Meta.Property(ProjectKeyItemMember.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}