using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Resources.Skills
{
    /// <summary>
	/// 员工技能
	/// </summary>
	[RootEntity, Serializable]
    [DisplayMember(nameof(Id))]
    [Label("员工技能")]
    public partial class EmployeeSkill : DataEntity
    {
        #region 认证时间 AuthDate
        /// <summary>
        /// 认证时间
        /// </summary>
        [Label("认证时间")]
        public static readonly Property<DateTime> AuthDateProperty = P<EmployeeSkill>.Register(e => e.AuthDate);

        /// <summary>
        /// 认证时间
        /// </summary>
        public DateTime AuthDate
        {
            get { return GetProperty(AuthDateProperty); }
            set { SetProperty(AuthDateProperty, value); }
        }
        #endregion

        #region 到期时间 ExpireDate
        /// <summary>
        /// 到期时间
        /// </summary>
        [Label("到期时间")]
        public static readonly Property<DateTime?> ExpireDateProperty = P<EmployeeSkill>.Register(e => e.ExpireDate);

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? ExpireDate
        {
            get { return GetProperty(ExpireDateProperty); }
            set { SetProperty(ExpireDateProperty, value); }
        }
        #endregion

        #region 考核意见 Opinion
        /// <summary>
        /// 考核意见
        /// </summary>
        [Label("考核意见")]
        public static readonly Property<string> OpinionProperty = P<EmployeeSkill>.Register(e => e.Opinion);

        /// <summary>
        /// 考核意见
        /// </summary>
        public string Opinion
        {
            get { return GetProperty(OpinionProperty); }
            set { SetProperty(OpinionProperty, value); }
        }
        #endregion

        #region 有效期（天） Validity
        /// <summary>
        /// 有效期（天）
        /// </summary>
        [Label("有效期(天)")]
        public static readonly Property<int?> ValidityProperty = P<EmployeeSkill>.Register(e => e.Validity);

        /// <summary>
        /// 有效期（天）
        /// </summary>
        public int? Validity
        {
            get { return GetProperty(ValidityProperty); }
            set { SetProperty(ValidityProperty, value); }
        }
        #endregion

        #region 考试结果 ExamRequired
        /// <summary>
        /// 考试结果
        /// </summary>
        [Label("考试结果")]
        public static readonly Property<ExamRequired> ExamRequiredProperty = P<EmployeeSkill>.Register(e => e.ExamRequired);

        /// <summary>
        /// 考试结果
        /// </summary>
        public ExamRequired ExamRequired
        {
            get { return GetProperty(ExamRequiredProperty); }
            set { SetProperty(ExamRequiredProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        public static readonly IRefIdProperty EmployeeIdProperty = P<EmployeeSkill>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employees.Employee> EmployeeProperty = P<EmployeeSkill>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employees.Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 当前状态 AuthStatus
        /// <summary>
        /// 当前状态
        /// </summary>
        [Label("认证状态")]
        public static readonly Property<AuthStatus> AuthStatusProperty = P<EmployeeSkill>.Register(e => e.AuthStatus);

        /// <summary>
        /// 当前状态
        /// </summary>
        public AuthStatus AuthStatus
        {
            get { return GetProperty(AuthStatusProperty); }
            set { SetProperty(AuthStatusProperty, value); }
        }
        #endregion

        #region 技能清单 Skill
        /// <summary>
        /// 技能清单Id
        /// </summary>
        public static readonly IRefIdProperty SkillIdProperty = P<EmployeeSkill>.RegisterRefId(e => e.SkillId, ReferenceType.Normal);

        /// <summary>
        /// 技能清单Id
        /// </summary>
        public double SkillId
        {
            get { return (double)GetRefId(SkillIdProperty); }
            set { SetRefId(SkillIdProperty, value); }
        }

        /// <summary>
        /// 技能清单
        /// </summary>
        public static readonly RefEntityProperty<Skill> SkillProperty = P<EmployeeSkill>.RegisterRef(e => e.Skill, SkillIdProperty);

        /// <summary>
        /// 技能清单
        /// </summary>
        public Skill Skill
        {
            get { return GetRefEntity(SkillProperty); }
            set { SetRefEntity(SkillProperty, value); }
        }
        #endregion

        #region 实操结果 OperationRequired
        /// <summary>
        /// 实操结果
        /// </summary>
        [Label("实操结果")]
        public static readonly Property<OperationRequired> OperationRequiredProperty = P<EmployeeSkill>.Register(e => e.OperationRequired);

        /// <summary>
        /// 实操结果
        /// </summary>
        public OperationRequired OperationRequired
        {
            get { return GetProperty(OperationRequiredProperty); }
            set { SetProperty(OperationRequiredProperty, value); }
        }
        #endregion

        #region 考核人 Verifier
        /// <summary>
        /// 考核人Id
        /// </summary>
        public static readonly IRefIdProperty VerifierIdProperty = P<EmployeeSkill>.RegisterRefId(e => e.VerifierId, ReferenceType.Normal);

        /// <summary>
        /// 考核人Id
        /// </summary>
        public double? VerifierId
        {
            get { return (double?)GetRefNullableId(VerifierIdProperty); }
            set { SetRefNullableId(VerifierIdProperty, value); }
        }

        /// <summary>
        /// 考核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> VerifierProperty = P<EmployeeSkill>.RegisterRef(e => e.Verifier, VerifierIdProperty);

        /// <summary>
        /// 考核人
        /// </summary>
        public Employee Verifier
        {
            get { return GetRefEntity(VerifierProperty); }
            set { SetRefEntity(VerifierProperty, value); }
        }
        #endregion

        #region 培训结果 TrainingRequired
        /// <summary>
        /// 培训结果
        /// </summary>
        [Label("培训结果")]
        public static readonly Property<TrainingRequired> TrainingRequiredProperty = P<EmployeeSkill>.Register(e => e.TrainingRequired);

        /// <summary>
        /// 培训结果
        /// </summary>
        public TrainingRequired TrainingRequired
        {
            get { return GetProperty(TrainingRequiredProperty); }
            set { SetProperty(TrainingRequiredProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web) 
        #region 技能编码 SkillCode
        /// <summary>
        /// 技能编码
        /// </summary>
        [Label("技能编码")]
        public static readonly Property<string> SkillCodeProperty = P<EmployeeSkill>.RegisterView(e => e.SkillCode, p => p.Skill.Code);

        /// <summary>
        /// 技能编码
        /// </summary>
        public string SkillCode
        {
            get { return this.GetProperty(SkillCodeProperty); }
        }
        #endregion

        #region 技能名称 SkillName
        /// <summary>
        /// 技能名称
        /// </summary>
        [Label("技能名称")]
        public static readonly Property<string> SkillNameProperty = P<EmployeeSkill>.RegisterView(e => e.SkillName, p => p.Skill.Name);

        /// <summary>
        /// 技能名称
        /// </summary>
        public string SkillName
        {
            get { return this.GetProperty(SkillNameProperty); }
        }
        #endregion

        #region 技能分类名称 SkillCategoryName
        /// <summary>
        /// 技能名称
        /// </summary>
        [Label("技能分类")]
        public static readonly Property<string> SkillCategoryNameProperty = P<EmployeeSkill>.RegisterView(e => e.SkillCategoryName, p => p.Skill.Category.Name);

        /// <summary>
        /// 技能名称
        /// </summary>
        public string SkillCategoryName
        {
            get { return this.GetProperty(SkillCategoryNameProperty); }
        }
        #endregion

        #region 技能描述 SkillRemark
        /// <summary>
        /// 技能描述
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> SkillRemarkProperty = P<EmployeeSkill>.RegisterView(e => e.SkillRemark, p => p.Skill.Remark);

        /// <summary>
        /// 技能描述
        /// </summary>
        public string SkillRemark
        {
            get { return this.GetProperty(SkillRemarkProperty); }
        }
        #endregion

        #region 工号 EmployeeCode
        /// <summary>
        /// 员工工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<EmployeeSkill>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 员工工号
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
        public static readonly Property<string> EmployeeNameProperty = P<EmployeeSkill>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 性别 EmployeeSex
        /// <summary>
        /// 性别
        /// </summary>
        [Label("性别")]
        public static readonly Property<Sex> EmployeeSexProperty = P<EmployeeSkill>.RegisterView(e => e.EmployeeSex, p => p.Employee.Sex);

        /// <summary>
        /// 性别
        /// </summary>
        public Sex EmployeeSex
        {
            get { return this.GetProperty(EmployeeSexProperty); }
        }
        #endregion

        #region 员工状态 EmployeeStatus
        /// <summary>
        /// 员工状态
        /// </summary>
        [Label("员工状态")]
        public static readonly Property<EmployeeStatus> EmployeeStatusProperty = P<EmployeeSkill>.RegisterView(e => e.EmployeeStatus, p => p.Employee.EmployeeStatus);

        /// <summary>
        /// 员工状态
        /// </summary>
        public EmployeeStatus EmployeeStatus
        {
            get { return this.GetProperty(EmployeeStatusProperty); }
        }
        #endregion

        #region 考核人 VerifierName
        /// <summary>
        /// 考核人
        /// </summary>
        [Label("考核人")]
        public static readonly Property<string> VerifierNameProperty = P<EmployeeSkill>.RegisterView(e => e.VerifierName, p => p.Verifier.Name);

        /// <summary>
        /// 考核人
        /// </summary>
        public string VerifierName
        {
            get { return this.GetProperty(VerifierNameProperty); }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// 员工技能 实体配置
    /// </summary>
    internal class EmployeeSkillConfig : EntityConfig<EmployeeSkill>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_EMP_SKILL").MapAllProperties();
            Meta.Property(EmployeeSkill.EmployeeIdProperty).ColumnMeta.HasIndex();
            Meta.Property(EmployeeSkill.SkillIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}