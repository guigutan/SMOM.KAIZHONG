using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.OnOffDuty
{
    /// <summary>
    /// 上岗、下岗记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("在岗信息")]
    [ConditionQueryType(typeof(OnOffDutyRecrodsCriteria))]
    public partial class OnOffDutyRecrods : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工
        /// </summary>
        [Required]
        [Label("员工Id")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<OnOffDutyRecrods>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<OnOffDutyRecrods>.RegisterRef(e => e.Employee, EmployeeIdProperty);

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
        public static readonly Property<double> OnDutyDurationProperty = P<OnOffDutyRecrods>.Register(e => e.OnDutyDuration);

        /// <summary>
        /// 在岗时长
        /// </summary>
        public double OnDutyDuration
        {
            get { return this.GetProperty(OnDutyDurationProperty); }
            set { this.SetProperty(OnDutyDurationProperty, value); }
        }
        #endregion

        #region 下岗时间   OffDutyTime
        /// <summary>
        /// 下岗时间
        /// </summary>
        [Label("下岗时间")]
        public static readonly Property<DateTime?> OffDutyTimeProperty = P<OnOffDutyRecrods>.Register(e => e.OffDutyTime);

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
        public static readonly Property<DateTime?> OnDutyTimeProperty = P<OnOffDutyRecrods>.Register(e => e.OnDutyTime);

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
        public static readonly Property<OnOffDutyType> OnOffDutyTypeProperty = P<OnOffDutyRecrods>.Register(e => e.OnOffDutyType);

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
        public static readonly Property<bool> IsAdditionalRecordingProperty = P<OnOffDutyRecrods>.Register(e => e.IsAdditionalRecording);

        /// <summary>
        /// 是否补录
        /// </summary>
        public bool IsAdditionalRecording
        {
            get { return GetProperty(IsAdditionalRecordingProperty); }
            set { SetProperty(IsAdditionalRecordingProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Required]
        [Label("工序Id")]
        public static readonly IRefIdProperty ProcessIdProperty = P<OnOffDutyRecrods>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty = P<OnOffDutyRecrods>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Required]
        [Label("资源Id")]
        public static readonly IRefIdProperty ResourceIdProperty = P<OnOffDutyRecrods>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<OnOffDutyRecrods>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位Id")]
        public static readonly IRefIdProperty StationIdProperty =
            P<OnOffDutyRecrods>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)this.GetRefId(StationIdProperty); }
            set { this.SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly RefEntityProperty<Station> StationProperty =
            P<OnOffDutyRecrods>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 员工号 EmployeeCode
        /// <summary>
        /// 员工号
        /// </summary>
        [Label("员工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<OnOffDutyRecrods>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<OnOffDutyRecrods>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

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
        public static readonly Property<string> EmployeeGroupNameProperty = P<OnOffDutyRecrods>.RegisterView(e => e.EmployeeGroupName, p => p.Employee.EmployeeGroup.Name);

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
        public static readonly Property<string> UserCodeProperty = P<OnOffDutyRecrods>.RegisterView(e => e.UserCode, p => p.Employee.User.Code);

        /// <summary>
        /// 用户
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
            set { SetProperty(UserCodeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<OnOffDutyRecrods>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<OnOffDutyRecrods>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位名称")]
        public static readonly Property<string> StationNameProperty = P<OnOffDutyRecrods>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
            set { SetProperty(StationNameProperty, value); }
        }
        #endregion
    }
    internal class OnOffDutyRecrodsConfig : EntityConfig<OnOffDutyRecrods>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ON_OFF_DUTY").MapAllProperties();
            Meta.IndexGroupOnProperties(OnOffDutyRecrods.ResourceIdProperty, OnOffDutyRecrods.StationIdProperty);
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}