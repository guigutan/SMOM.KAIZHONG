using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.OnOffDutyA
{
    /// <summary>
    /// 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("查询实体")]
    public partial class OnOffDutyRecrodsCriteria : Criteria
    {
        #region 状态   OnOffDutyType
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OnOffDutyType?> OnOffDutyTypeProperty = P<OnOffDutyRecrodsCriteria>.Register(e => e.OnOffDutyType);

        /// <summary>
        /// 状态
        /// </summary>
        public OnOffDutyType? OnOffDutyType
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
        public static readonly Property<YesNo?> IsAdditionalRecordingProperty = P<OnOffDutyRecrodsCriteria>.Register(e => e.IsAdditionalRecording);

        /// <summary>
        /// 是否补录
        /// </summary>
        public YesNo? IsAdditionalRecording
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
        public static readonly IRefIdProperty ProcessIdProperty = P<OnOffDutyRecrodsCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty = P<OnOffDutyRecrodsCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly IRefIdProperty ResourceIdProperty = P<OnOffDutyRecrodsCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<OnOffDutyRecrodsCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
            P<OnOffDutyRecrodsCriteria>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly RefEntityProperty<Station> StationProperty =
            P<OnOffDutyRecrodsCriteria>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 创建时间   CreateTime
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateTimeProperty = P<OnOffDutyRecrodsCriteria>.Register(e => e.CreateTime);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateTime
        {
            get { return this.GetProperty(CreateTimeProperty); }
            set { this.SetProperty(CreateTimeProperty, value); }
        }
        #endregion

        #region 下岗时间   OffDutyTime
        /// <summary>
        /// 下岗时间
        /// </summary>
        [Label("下岗时间")]
        public static readonly Property<DateRange> OffDutyTimeProperty = P<OnOffDutyRecrodsCriteria>.Register(e => e.OffDutyTime);

        /// <summary>
        /// 下岗时间
        /// </summary>
        public DateRange OffDutyTime
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
        public static readonly Property<DateRange> OnDutyTimeProperty = P<OnOffDutyRecrodsCriteria>.Register(e => e.OnDutyTime);

        /// <summary>
        /// 上岗时间
        /// </summary>
        public DateRange OnDutyTime
        {
            get { return this.GetProperty(OnDutyTimeProperty); }
            set { this.SetProperty(OnDutyTimeProperty, value); }
        }
        #endregion

        #region 员工  Staff
        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        public static readonly Property<string> StaffProperty = P<OnOffDutyRecrodsCriteria>.Register(e => e.Staff);

        /// <summary>
        /// 员工
        /// </summary>
        public string Staff
        {
            get { return this.GetProperty(StaffProperty); }
            set { this.SetProperty(StaffProperty, value); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OnOffDutyController>().Fetch(this);
        }

    }
}