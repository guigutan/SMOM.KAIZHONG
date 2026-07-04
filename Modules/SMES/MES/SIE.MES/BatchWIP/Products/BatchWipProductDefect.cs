using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷记录")]
    public partial class BatchWipProductDefect : DataEntity
    {
        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipProductDefect>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 子批次号 SubBatchNo
        /// <summary>
        /// 子批次号
        /// </summary>
        [Label("子批次号")]
        public static readonly Property<string> SubBatchNoProperty = P<BatchWipProductDefect>.Register(e => e.SubBatchNo);

        /// <summary>
        /// 子批次号
        /// </summary>
        public string SubBatchNo
        {
            get { return GetProperty(SubBatchNoProperty); }
            set { SetProperty(SubBatchNoProperty, value); }
        }
        #endregion

        #region 载具号 ContainerNo
        /// <summary>
        /// 载具号
        /// </summary>
        [Label("载具号")]
        public static readonly Property<string> ContainerNoProperty = P<BatchWipProductDefect>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchWipProductDefect>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<BatchWipProductDefect>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion 

        #region 缺陷位置 Location
        /// <summary>
        /// 缺陷位置
        /// </summary>
        [Label("缺陷位置")]
        public static readonly Property<string> LocationProperty = P<BatchWipProductDefect>.Register(e => e.Location);

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 维修时间 FixedDate
        /// <summary>
        /// 维修时间
        /// </summary>
        [Label("维修时间")]
        public static readonly Property<DateTime?> FixedDateProperty = P<BatchWipProductDefect>.Register(e => e.FixedDate);

        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? FixedDate
        {
            get { return GetProperty(FixedDateProperty); }
            set { SetProperty(FixedDateProperty, value); }
        }
        #endregion

        #region 维修人 FixedBy
        /// <summary>
        /// 维修人Id
        /// </summary>
        [Label("维修人")]
        public static readonly IRefIdProperty FixedByIdProperty = P<BatchWipProductDefect>.RegisterRefId(e => e.FixedById, ReferenceType.Normal);

        /// <summary>
        /// 维修人Id
        /// </summary>
        public double? FixedById
        {
            get { return (double?)GetRefNullableId(FixedByIdProperty); }
            set { SetRefNullableId(FixedByIdProperty, value); }
        }

        /// <summary>
        /// 维修人
        /// </summary>
        public static readonly RefEntityProperty<Employee> FixedByProperty = P<BatchWipProductDefect>.RegisterRef(e => e.FixedBy, FixedByIdProperty);

        /// <summary>
        /// 维修人
        /// </summary>
        public Employee FixedBy
        {
            get { return GetRefEntity(FixedByProperty); }
            set { SetRefEntity(FixedByProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<BatchWipProductDefect>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<BatchWipProductDefect>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<BatchWipProductDefect>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Station> StationProperty = P<BatchWipProductDefect>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<BatchWipProductDefect>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<BatchWipProductDefect>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 不良记录 Version
        /// <summary>
        /// 不良记录Id
        /// </summary>
        public static readonly IRefIdProperty VersionIdProperty = P<BatchWipProductDefect>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 不良记录Id
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 不良记录
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductVersion> VersionProperty = P<BatchWipProductDefect>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 不良记录
        /// </summary>
        public BatchWipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 责任列表 ResponsibilityList
        /// <summary>
        /// 责任列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipDefectResponsibility>> ResponsibilityListProperty = P<BatchWipProductDefect>.RegisterList(e => e.ResponsibilityList);

        /// <summary>
        /// 责任列表
        /// </summary>
        public EntityList<BatchWipDefectResponsibility> ResponsibilityList
        {
            get { return this.GetLazyList(ResponsibilityListProperty); }
        }
        #endregion

        #region 明细列表 DetailList
        /// <summary>
        /// 明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductDefectDetail>> DetailListProperty = P<BatchWipProductDefect>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 明细列表
        /// </summary>
        public EntityList<BatchWipProductDefectDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 维修措施列表 MeasureList
        /// <summary>
        /// 维修措施列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipDefectMeasure>> MeasureListProperty = P<BatchWipProductDefect>.RegisterList(e => e.MeasureList);

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public EntityList<BatchWipDefectMeasure> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<BatchWipProductDefect>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<BatchWipProductDefect>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<BatchWipProductDefect>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 维修人名称 FixedByName
        /// <summary>
        /// 维修人名称
        /// </summary>
        [Label("维修人")]
        public static readonly Property<string> FixedByNameProperty = P<BatchWipProductDefect>.RegisterView(e => e.FixedByName, p => p.FixedBy.Name);

        /// <summary>
        /// 维修人名称
        /// </summary>
        public string FixedByName
        {
            get { return this.GetProperty(FixedByNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品缺陷记录 实体配置
    /// </summary>
    internal class BatchWipProductDefectConfig : EntityConfig<BatchWipProductDefect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_DEFECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}