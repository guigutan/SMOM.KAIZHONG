using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Skills;
using System;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
	/// 实操记录
	/// </summary>
	[ChildEntity, Serializable]
    [Label("实操记录")]
    public partial class OperationRecord : DataEntity
    {
        #region 考核意见 AuditOpinion
        /// <summary>
        /// 考核意见
        /// </summary>
        [Label("考核意见")]
        public static readonly Property<string> AuditOpinionProperty = P<OperationRecord>.Register(e => e.AuditOpinion);

        /// <summary>
        /// 考核意见
        /// </summary>
        public string AuditOpinion
        {
            get { return GetProperty(AuditOpinionProperty); }
            set { SetProperty(AuditOpinionProperty, value); }
        }
        #endregion

        #region 考核时间 AuditTime
        /// <summary>
        /// 考核时间
        /// </summary>
        [Label("考核时间")]
        [Required]
        public static readonly Property<DateTime?> AuditTimeProperty = P<OperationRecord>.Register(e => e.AuditTime);

        /// <summary>
        /// 考核时间
        /// </summary>
        public DateTime? AuditTime
        {
            get { return GetProperty(AuditTimeProperty); }
            set { SetProperty(AuditTimeProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<OperationRecord>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<OperationRecord>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 考核人 Verifier
        /// <summary>
        /// 考核人Id
        /// </summary>
        [Label("考核人")]
        public static readonly IRefIdProperty VerifierIdProperty = P<OperationRecord>.RegisterRefId(e => e.VerifierId, ReferenceType.Normal);

        /// <summary>
        /// 考核人Id
        /// </summary>
        public double VerifierId
        {
            get { return (double)GetRefId(VerifierIdProperty); }
            set { SetRefId(VerifierIdProperty, value); }
        }

        /// <summary>
        /// 考核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> VerifierProperty = P<OperationRecord>.RegisterRef(e => e.Verifier, VerifierIdProperty);

        /// <summary>
        /// 考核人
        /// </summary>
        public Employee Verifier
        {
            get { return GetRefEntity(VerifierProperty); }
            set { SetRefEntity(VerifierProperty, value); }
        }
        #endregion

        #region 实操结果 Result
        /// <summary>
        /// 实操结果
        /// </summary>
        [Label("实操结果")]
        public static readonly Property<OperationRequired> ResultProperty = P<OperationRecord>.Register(e => e.Result);

        /// <summary>
        /// 实操结果
        /// </summary>
        public OperationRequired Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 是否历史数据 IsHistory
        /// <summary>
        /// 是否历史数据
        /// </summary>
        [Label("是否历史数据")]
        public static readonly Property<bool> IsHistoryProperty = P<OperationRecord>.Register(e => e.IsHistory);

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
        public static readonly IRefIdProperty SkillAuthIdProperty = P<OperationRecord>.RegisterRefId(e => e.SkillAuthId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<SkillAuthentication> SkillAuthProperty = P<OperationRecord>.RegisterRef(e => e.SkillAuth, SkillAuthIdProperty);

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
        public static readonly Property<string> EmployeeCodeProperty = P<OperationRecord>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<OperationRecord>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

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
        public static readonly Property<Sex?> EmployeeSexProperty = P<OperationRecord>.RegisterView(e => e.EmployeeSex, p => p.Employee.Sex);

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
        public static readonly Property<EmployeeStatus?> EmployeeStatusProperty = P<OperationRecord>.RegisterView(e => e.EmployeeStatus, p => p.Employee.EmployeeStatus);

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
    /// 实操记录 实体配置
    /// </summary>
    internal class OperationRecordConfig : EntityConfig<OperationRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_OPERAT_RECORD").MapAllProperties();
            Meta.Property(OperationRecord.EmployeeIdProperty).ColumnMeta.HasIndex();
            Meta.IndexGroupOnProperties(OperationRecord.SkillAuthIdProperty, OperationRecord.EmployeeIdProperty);
            Meta.EnablePhantoms();
        }
    }
}