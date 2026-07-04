using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.OnOffDutyA
{
    /// <summary>
    /// 上岗、下岗记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("在岗信息")]
    [ConditionQueryType(typeof(OnOffDutyRecrodsCriteria))]
    public partial class OnOffDutyRecrodsA : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工
        /// </summary>
        [Required]
        [Label("员工Id")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<OnOffDutyRecrodsA>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<OnOffDutyRecrodsA>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 在岗时长   OnDutyDuration
        /// <summary>
        /// 在岗时长
        /// </summary>
        [Label("在岗时长(分钟)")]
        public static readonly Property<double> OnDutyDurationProperty = P<OnOffDutyRecrodsA>.Register(e => e.OnDutyDuration);

        /// <summary>
        /// 在岗时长
        /// </summary>
        public double OnDutyDuration
        {
            get { return this.GetProperty(OnDutyDurationProperty); }
            set
            {
                this.SetProperty(OnDutyDurationProperty, value);                
            }
        }
        #endregion

        #region 下岗时间   OffDutyTime
        /// <summary>
        /// 下岗时间
        /// </summary>
        [Label("下岗时间")]
        public static readonly Property<DateTime?> OffDutyTimeProperty = P<OnOffDutyRecrodsA>.Register(e => e.OffDutyTime );

        /// <summary>
        /// 下岗时间
        /// </summary>
        public DateTime? OffDutyTime
        {
            get { return this.GetProperty(OffDutyTimeProperty); }
            set { this.SetProperty(OffDutyTimeProperty, value); }
        }
        #endregion

        #region 上岗时间   OnDutyTime
        /// <summary>
        /// 上岗时间
        /// </summary>
        [Label("上岗时间")]
        public static readonly Property<DateTime?> OnDutyTimeProperty = P<OnOffDutyRecrodsA>.Register(e => e.OnDutyTime);

        /// <summary>
        /// 上岗时间
        /// </summary>
        public DateTime? OnDutyTime
        {
            get { return this.GetProperty(OnDutyTimeProperty); }
            set { this.SetProperty(OnDutyTimeProperty, value); }
        }
        #endregion

        #region 状态   OnOffDutyType
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OnOffDutyType> OnOffDutyTypeProperty = P<OnOffDutyRecrodsA>.Register(e => e.OnOffDutyType);

        /// <summary>
        /// 状态
        /// </summary>
        public OnOffDutyType OnOffDutyType
        {
            get { return this.GetProperty(OnOffDutyTypeProperty); }
            set { this.SetProperty(OnOffDutyTypeProperty, value); }
        }
        #endregion

        #region 是否补录 IsAdditionalRecording
        /// <summary>
        /// 是否补录
        /// </summary>
        [Label("是否补录")]
        public static readonly Property<bool> IsAdditionalRecordingProperty = P<OnOffDutyRecrodsA>.Register(e => e.IsAdditionalRecording);

        /// <summary>
        /// 是否补录
        /// </summary>
        public bool IsAdditionalRecording
        {
            get { return GetProperty(IsAdditionalRecordingProperty); }
            set { SetProperty(IsAdditionalRecordingProperty, value); }
        }
        #endregion

        #region 员工号 EmployeeCode
        /// <summary>
        /// 员工号
        /// </summary>
        [Label("员工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<OnOffDutyRecrodsA>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
            set { SetProperty(EmployeeCodeProperty, value); }
        }
        #endregion

        #region 员工名 EmployeeName
        /// <summary>
        /// 员工名
        /// </summary>
        [Label("员工名")]
        public static readonly Property<string> EmployeeNameProperty = P<OnOffDutyRecrodsA>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
            set { SetProperty(EmployeeNameProperty, value); }
        }
        #endregion

        #region 员工组 EmployeeGroupName
        /// <summary>
        /// 员工组
        /// </summary>
        [Label("员工组")]
        public static readonly Property<string> EmployeeGroupNameProperty = P<OnOffDutyRecrodsA>.RegisterView(e => e.EmployeeGroupName, p => p.Employee.EmployeeGroup.Name);

        /// <summary>
        /// 员工组
        /// </summary>
        public string EmployeeGroupName
        {
            get { return this.GetProperty(EmployeeGroupNameProperty); }
            set { SetProperty(EmployeeGroupNameProperty, value); }
        }
        #endregion

        #region 用户 UserCode
        /// <summary>
        /// 用户
        /// </summary>
        [Label("用户")]
        public static readonly Property<string> UserCodeProperty = P<OnOffDutyRecrodsA>.RegisterView(e => e.UserCode, p => p.Employee.User.Code);

        /// <summary>
        /// 用户
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
            set { SetProperty(UserCodeProperty, value); }
        }
        #endregion      


    }
    internal class OnOffDutyRecrodsConfig : EntityConfig<OnOffDutyRecrodsA>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ON_OFF_DUTY_A").MapAllProperties();           
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }


    }
}