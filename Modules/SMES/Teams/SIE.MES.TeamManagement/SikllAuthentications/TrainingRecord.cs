using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Skills;
using System;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
	/// 培训记录
	/// </summary>
	[ChildEntity, Serializable]
    [Label("培训记录")]
    public partial class TrainingRecord : DataEntity
    {
        #region 培训开始时间 BeginDate
        /// <summary>
        /// 培训开始时间
        /// </summary>
        [Label("培训开始时间")]
        [Required]
        public static readonly Property<DateTime?> BeginDateProperty = P<TrainingRecord>.Register(e => e.BeginDate);

        /// <summary>
        /// 培训开始时间
        /// </summary>
        public DateTime? BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 培训结束时间 EndDate
        /// <summary>
        /// 培训结束时间
        /// </summary>
        [Label("培训结束时间")]
        [Required]
        public static readonly Property<DateTime?> EndDateProperty = P<TrainingRecord>.Register(e => e.EndDate);

        /// <summary>
        /// 培训结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 培训时长 Duration
        /// <summary>
        /// 培训时长
        /// </summary>
        [Label("培训时长(h)")]
        public static readonly Property<decimal> DurationProperty = P<TrainingRecord>.Register(e => e.Duration);

        /// <summary>
        /// 培训时长
        /// </summary>
        public decimal Duration
        {
            get { return GetProperty(DurationProperty); }
            set { SetProperty(DurationProperty, value); }
        }
        #endregion

        #region 培训结果 Result
        /// <summary>
        /// 培训结果
        /// </summary>
        [Label("培训结果")]
        public static readonly Property<TrainingRequired> ResultProperty = P<TrainingRecord>.Register(e => e.Result);

        /// <summary>
        /// 培训结果
        /// </summary>
        public TrainingRequired Result
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
        public static readonly IRefIdProperty EmployeeIdProperty = P<TrainingRecord>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<TrainingRecord>.RegisterRef(e => e.Employee, EmployeeIdProperty);

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
        public static readonly Property<bool> IsHistoryProperty = P<TrainingRecord>.Register(e => e.IsHistory);

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
        public static readonly IRefIdProperty SkillAuthIdProperty = P<TrainingRecord>.RegisterRefId(e => e.SkillAuthId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<SkillAuthentication> SkillAuthProperty = P<TrainingRecord>.RegisterRef(e => e.SkillAuth, SkillAuthIdProperty);

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
        public static readonly Property<string> EmployeeCodeProperty = P<TrainingRecord>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<TrainingRecord>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

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
        public static readonly Property<Sex?> EmployeeSexProperty = P<TrainingRecord>.RegisterView(e => e.EmployeeSex, p => p.Employee.Sex);

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
        public static readonly Property<EmployeeStatus?> EmployeeStatusProperty = P<TrainingRecord>.RegisterView(e => e.EmployeeStatus, p => p.Employee.EmployeeStatus);

        /// <summary>
        /// 员工状态
        /// </summary>
        public EmployeeStatus? EmployeeStatus
        {
            get { return this.GetProperty(EmployeeStatusProperty); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == BeginDateProperty || e.Property == EndDateProperty)
            {
                if (BeginDate == null || EndDate == null)
                {
                    Duration = 0;
                    return;
                }

                var sysDiff = RT.Service.Resolve<SkillAuthController>().GetHourDiff(BeginDate.Value, EndDate.Value);
                Duration = sysDiff;
            }
        }
    }

    /// <summary>
    /// 培训记录 实体配置
    /// </summary>
    internal class TrainingRecordConfig : EntityConfig<TrainingRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_TRAIN_RECORD").MapAllProperties();
            Meta.Property(TrainingRecord.EmployeeIdProperty).ColumnMeta.HasIndex();
            Meta.IndexGroupOnProperties(TrainingRecord.SkillAuthIdProperty, TrainingRecord.EmployeeIdProperty);
            Meta.EnablePhantoms();
        }
    }
}