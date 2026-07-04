using SIE.Domain;
using SIE.EMS.EarlierStage.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更成员
    /// </summary>
    [ChildEntity, Serializable]
    [Label("项目变更成员")]
    public partial class ProjectChangeMember : DataEntity
    {
        #region 项目变更 ProjectChange
        /// <summary>
        /// 项目变更Id
        /// </summary>
        [Label("项目变更")]
        public static readonly IRefIdProperty ProjectChangeIdProperty =
            P<ProjectChangeMember>.RegisterRefId(e => e.ProjectChangeId, ReferenceType.Parent);

        /// <summary>
        /// 项目变更Id
        /// </summary>
        public double ProjectChangeId
        {
            get { return (double)this.GetRefId(ProjectChangeIdProperty); }
            set { this.SetRefId(ProjectChangeIdProperty, value); }
        }

        /// <summary>
        /// 项目变更
        /// </summary>
        public static readonly RefEntityProperty<ProjectChange> ProjectChangeProperty =
            P<ProjectChangeMember>.RegisterRef(e => e.ProjectChange, ProjectChangeIdProperty);

        /// <summary>
        /// 项目变更
        /// </summary>
        public ProjectChange ProjectChange
        {
            get { return this.GetRefEntity(ProjectChangeProperty); }
            set { this.SetRefEntity(ProjectChangeProperty, value); }
        }
        #endregion

        #region 职位 Position
        /// <summary>
        /// 职位
        /// </summary>
        [Label("职位")]
        public static readonly Property<string> PositionProperty = P<ProjectChangeMember>.Register(e => e.Position);

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
        public static readonly Property<string> RemarkProperty = P<ProjectChangeMember>.Register(e => e.Remark);

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
        public static readonly IRefIdProperty EmployeeIdProperty = P<ProjectChangeMember>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<ProjectChangeMember>.RegisterRef(e => e.Employee, EmployeeIdProperty);

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
        public static readonly Property<MemberStatus> MemberStatusProperty = P<ProjectChangeMember>.Register(e => e.MemberStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public MemberStatus MemberStatus
        {
            get { return GetProperty(MemberStatusProperty); }
            set { SetProperty(MemberStatusProperty, value); }
        }
        #endregion

        #region 原项目成员id ProjectMemberId
        /// <summary>
        /// 原项目成员id
        /// </summary>
        [Label("原项目成员id")]
        public static readonly Property<double?> ProjectMemberIdProperty = P<ProjectChangeMember>.Register(e => e.ProjectMemberId);

        /// <summary>
        /// 原项目成员id
        /// </summary>
        public double? ProjectMemberId
        {
            get { return this.GetProperty(ProjectMemberIdProperty); }
            set { this.SetProperty(ProjectMemberIdProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<ProjectChangeMember>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<ProjectChangeMember>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
            set { SetProperty(EmployeeNameProperty, value); }
        }
        #endregion

        #region 联系方式 Phone
        /// <summary>
        /// 联系方式
        /// </summary>
        [Label("联系方式")]
        public static readonly Property<string> PhoneProperty = P<ProjectChangeMember>.RegisterView(e => e.Phone, p => p.Employee.Phone);

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone
        {
            get { return this.GetProperty(PhoneProperty); }
            set { SetProperty(PhoneProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 项目变更成员 实体配置
    /// </summary>
    internal class ProjectChangeMemberConfig : EntityConfig<ProjectChangeMember>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PRO_CHA_MEM").MapAllProperties();
            Meta.Property(ProjectChangeMember.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
