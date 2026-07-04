using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Skills;
using System;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
	/// 考试结果
	/// </summary>
	[ChildEntity, Serializable]
    [Label("考试结果")]
    public partial class ExamResult : DataEntity
    {
        #region 考试得分 Score
        /// <summary>
        /// 考试得分
        /// </summary>
        [Label("考试得分(分)")]
        [Required]
        public static readonly Property<decimal?> ScoreProperty = P<ExamResult>.Register(e => e.Score);

        /// <summary>
        /// 考试得分
        /// </summary>
        public decimal? Score
        {
            get { return GetProperty(ScoreProperty); }
            set { SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 考试时间 ExamTime
        /// <summary>
        /// 考试时间
        /// </summary>
        [Label("考试时间")]
        [Required]
        public static readonly Property<DateTime?> ExamTimeProperty = P<ExamResult>.Register(e => e.ExamTime);

        /// <summary>
        /// 考试时间
        /// </summary>
        public DateTime? ExamTime
        {
            get { return GetProperty(ExamTimeProperty); }
            set { SetProperty(ExamTimeProperty, value); }
        }
        #endregion

        #region 考试结果 Result
        /// <summary>
        /// 考试结果
        /// </summary>
        [Label("考试结果")]
        [Required]
        public static readonly Property<ExamRequired> ResultProperty = P<ExamResult>.Register(e => e.Result);

        /// <summary>
        /// 考试结果
        /// </summary>
        public ExamRequired Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<ExamResult>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<ExamResult>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 是否历史数据 IsHistory
        /// <summary>
        /// 是否历史数据
        /// </summary>
        [Label("是否历史数据")]
        public static readonly Property<bool> IsHistoryProperty = P<ExamResult>.Register(e => e.IsHistory);

        /// <summary>
        /// 是否历史数据
        /// </summary>
        public bool IsHistory
        {
            get { return this.GetProperty(IsHistoryProperty); }
            set { this.SetProperty(IsHistoryProperty, value); }
        }
        #endregion

        #region 员工技能认证管理 SkillAuth
        /// <summary>
        /// 员工技能认证管理Id
        /// </summary>
        [Label("技能认证")]
        public static readonly IRefIdProperty SkillAuthIdProperty = P<ExamResult>.RegisterRefId(e => e.SkillAuthId, ReferenceType.Parent);

        /// <summary>
        /// 员工技能认证管理Id
        /// </summary>
        public double SkillAuthId
        {
            get { return (double)GetRefId(SkillAuthIdProperty); }
            set { SetRefId(SkillAuthIdProperty, value); }
        }

        /// <summary>
        /// 员工技能认证管理
        /// </summary>
        public static readonly RefEntityProperty<SkillAuthentication> SkillAuthProperty = P<ExamResult>.RegisterRef(e => e.SkillAuth, SkillAuthIdProperty);

        /// <summary>
        /// 员工技能认证管理
        /// </summary>
        public SkillAuthentication SkillAuth
        {
            get { return GetRefEntity(SkillAuthProperty); }
            set { SetRefEntity(SkillAuthProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<ExamResult>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<ExamResult>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

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
        public static readonly Property<Sex?> EmployeeSexProperty = P<ExamResult>.RegisterView(e => e.EmployeeSex, p => p.Employee.Sex);

        /// <summary>
        /// 性别
        /// </summary>
        public Sex? EmployeeSex
        {
            get { return this.GetProperty(EmployeeSexProperty); }
        }
        #endregion

        #region 员工状态 EmployeeStatus
        /// <summary>
        /// 员工状态
        /// </summary>
        [Label("员工状态")]
        public static readonly Property<EmployeeStatus?> EmployeeStatusProperty = P<ExamResult>.RegisterView(e => e.EmployeeStatus, p => p.Employee.EmployeeStatus);

        /// <summary>
        /// 员工状态
        /// </summary>
        public EmployeeStatus? EmployeeStatus
        {
            get { return this.GetProperty(EmployeeStatusProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 考试结果 实体配置
    /// </summary>
    internal class ExamResultConfig : EntityConfig<ExamResult>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_EXAM_RESULT").MapAllProperties();
            Meta.Property(ExamResult.EmployeeIdProperty).ColumnMeta.HasIndex();
            Meta.IndexGroupOnProperties(ExamResult.SkillAuthIdProperty, ExamResult.EmployeeIdProperty);
            Meta.EnablePhantoms();
        }
    }
}