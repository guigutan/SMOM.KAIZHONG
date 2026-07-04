using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品维修记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品维修记录")]
    public partial class WipProductRepair : DataEntity
    {
        #region 维修类型 RepairType
        /// <summary>
        /// 维修类型
        /// </summary>
        [Label("维修类型")]
        public static readonly Property<RepairType?> RepairTypeProperty = P<WipProductRepair>.Register(e => e.RepairType);

        /// <summary>
        /// 维修类型
        /// </summary>
        public RepairType? RepairType
        {
            get { return GetProperty(RepairTypeProperty); }
            set { SetProperty(RepairTypeProperty, value); }
        }
        #endregion

        #region 维修开始时间 RepairStart
        /// <summary>
        /// 维修开始时间
        /// </summary>
        [Label("维修开始时间")]
        public static readonly Property<DateTime> RepairStartProperty = P<WipProductRepair>.Register(e => e.RepairStart);

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime RepairStart
        {
            get { return GetProperty(RepairStartProperty); }
            set { SetProperty(RepairStartProperty, value); }
        }
        #endregion

        #region 维修完成时间 RepaireTime
        /// <summary>
        /// 维修完成时间
        /// </summary>
        [Label("维修完成时间")]
        public static readonly Property<DateTime?> RepaireTimeProperty = P<WipProductRepair>.Register(e => e.RepaireTime);

        /// <summary>
        /// 维修完成时间
        /// </summary>
        public DateTime? RepaireTime
        {
            get { return GetProperty(RepaireTimeProperty); }
            set { SetProperty(RepaireTimeProperty, value); }
        }
        #endregion

        #region 返修人 ReparieBy
        /// <summary>
        /// 返修人Id
        /// </summary>
        [Label("返修人")]
        public static readonly IRefIdProperty ReparieByIdProperty = P<WipProductRepair>.RegisterRefId(e => e.ReparieById, ReferenceType.Normal);

        /// <summary>
        /// 返修人Id
        /// </summary>
        public double ReparieById
        {
            get { return (double)GetRefId(ReparieByIdProperty); }
            set { SetRefId(ReparieByIdProperty, value); }
        }

        /// <summary>
        /// 返修人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReparieByProperty = P<WipProductRepair>.RegisterRef(e => e.ReparieBy, ReparieByIdProperty);

        /// <summary>
        /// 返修人
        /// </summary>
        public Employee ReparieBy
        {
            get { return GetRefEntity(ReparieByProperty); }
            set { SetRefEntity(ReparieByProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<WipProductRepair>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)GetRefId(ShiftIdProperty); }
            set { SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<WipProductRepair>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<WipProductRepair>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<WipProductRepair>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WipProductRepair>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<WipProductRepair>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty StationIdProperty = P<WipProductRepair>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty = P<WipProductRepair>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 维修记录 Version
        /// <summary>
        /// 维修记录Id
        /// </summary>
        [Label("维修记录")]
        public static readonly IRefIdProperty VersionIdProperty = P<WipProductRepair>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 维修记录Id
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 维修记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductVersion> VersionProperty = P<WipProductRepair>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 维修记录
        /// </summary>
        public WipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion


        #region 换料列表 WipProductRepairReplaceRecordList
        /// <summary>
        /// 换料列表
        /// </summary>
        public static readonly ListProperty<EntityList<WipProductRepairReplaceRecord>> WipProductRepairReplaceRecordListProperty = P<WipProductRepair>.RegisterList(e => e.WipProductRepairReplaceRecordList);
        /// <summary>
        /// 换料列表
        /// </summary>
        public EntityList<WipProductRepairReplaceRecord> WipProductRepairReplaceRecordList  
        {
            get { return this.GetLazyList(WipProductRepairReplaceRecordListProperty); }
        }
        #endregion



        #region 产品维修缺陷列表 WipProductRepairDefectList
        /// <summary>
        /// 产品维修缺陷列表
        /// </summary>
        public static readonly ListProperty<EntityList<WipProductRepairDefect>> WipProductRepairDefectListProperty = P<WipProductRepair>.RegisterList(e => e.WipProductRepairDefectList);
        /// <summary>
        /// 产品维修缺陷列表
        /// </summary>
        public EntityList<WipProductRepairDefect> WipProductRepairDefectList
        {
            get { return this.GetLazyList(WipProductRepairDefectListProperty); }
        }
        #endregion

        #region 图片 Attachments
        /// <summary>
        /// 图片
        /// </summary>
        public static readonly ListProperty<EntityList<WipProductRepairAttachment>> AttachmentsProperty = P<WipProductRepair>.RegisterList(e => e.Attachments);
        /// <summary>
        /// 图片
        /// </summary>
        public EntityList<WipProductRepairAttachment> Attachments
        {
            get { return this.GetLazyList(AttachmentsProperty); }
        }
        #endregion

        #region 视图属性

        #region 返修人 RepaireByName
        /// <summary>
        /// 返修人
        /// </summary>
        [Label("返修人")]
        public static readonly Property<string> RepaireByNameProperty = P<WipProductRepair>.RegisterView(e => e.RepaireByName, p => p.ReparieBy.Name);

        /// <summary>
        /// 返修人
        /// </summary>
        public string RepaireByName
        {
            get { return this.GetProperty(RepaireByNameProperty); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<WipProductRepair>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<WipProductRepair>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WipProductRepair>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次")]
        public static readonly Property<string> ShiftNameProperty = P<WipProductRepair>.RegisterView(e => e.ShiftName, p => p.Shift.Name);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return this.GetProperty(ShiftNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品维修记录 实体配置
    /// </summary>
    internal class WipProductRepaireConfig : EntityConfig<WipProductRepair>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_REP").MapAllProperties();
            Meta.Property(WipProductRepair.VersionIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}