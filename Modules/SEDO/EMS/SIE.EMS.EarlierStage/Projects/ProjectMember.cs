using SIE.Domain;
using SIE.EMS.EarlierStage.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目成员
    /// </summary>
    [ChildEntity, Serializable]
    [Label("项目成员")]
    public partial class ProjectMember : DataEntity
    {
        #region 职位 Position
        /// <summary>
        /// 职位
        /// </summary>
        [Label("职位")]
        public static readonly Property<string> PositionProperty = P<ProjectMember>.Register(e => e.Position);

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
        public static readonly Property<string> RemarkProperty = P<ProjectMember>.Register(e => e.Remark);

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
        public static readonly IRefIdProperty EmployeeIdProperty = P<ProjectMember>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<ProjectMember>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 状态 MemberStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<MemberStatus> MemberStatusProperty = P<ProjectMember>.Register(e => e.MemberStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public MemberStatus MemberStatus
        {
            get { return GetProperty(MemberStatusProperty); }
            set { SetProperty(MemberStatusProperty, value); }
        }
        #endregion

        #region 项目 Project
        /// <summary>
        /// 项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectIdProperty = P<ProjectMember>.RegisterRefId(e => e.ProjectId, ReferenceType.Parent);

        /// <summary>
        /// 项目Id
        /// </summary>
        public double ProjectId
        {
            get { return (double)GetRefId(ProjectIdProperty); }
            set { SetRefId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public static readonly RefEntityProperty<Project> ProjectProperty = P<ProjectMember>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project
        {
            get { return GetRefEntity(ProjectProperty); }
            set { SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<ProjectMember>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 姓名 EmployeeName
        /// <summary>
        /// 姓名
        /// </summary>
        [Label("姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<ProjectMember>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

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
        public static readonly Property<string> PhoneProperty = P<ProjectMember>.RegisterView(e => e.Phone, p => p.Employee.Phone);

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone
        {
            get { return this.GetProperty(PhoneProperty); }
        }
        #endregion

        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<ProjectMember>.RegisterView(e => e.ProjectCode, p => p.Project.Code);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 项目成员 实体配置
    /// </summary>
    internal class ProjectMemberConfig : EntityConfig<ProjectMember>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PROJ_MEMBER").MapAllProperties();
            Meta.Property(ProjectMember.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}