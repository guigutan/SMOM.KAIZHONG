using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品维修记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品维修记录")]
    public partial class BatchWipProductRepaire : DataEntity
    {
        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipProductRepaire>.Register(e => e.BatchNo);

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
        public static readonly Property<string> SubBatchNoProperty = P<BatchWipProductRepaire>.Register(e => e.SubBatchNo);

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
        public static readonly Property<string> ContainerNoProperty = P<BatchWipProductRepaire>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion 

        #region 返修数量 Qty
        /// <summary>
        /// 返修数量
        /// </summary>
        [Label("返修数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchWipProductRepaire>.Register(e => e.Qty);

        /// <summary>
        /// 返修数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 修复数量 RepairQty
        /// <summary>
        /// 修复数量
        /// </summary>
        [Label("修复数量")]
        public static readonly Property<decimal> RepairQtyProperty = P<BatchWipProductRepaire>.Register(e => e.RepairQty);

        /// <summary>
        /// 修复数量
        /// </summary>
        public decimal RepairQty
        {
            get { return GetProperty(RepairQtyProperty); }
            set { SetProperty(RepairQtyProperty, value); }
        }
        #endregion

        #region 废弃数量 ScrapQty
        /// <summary>
        /// 废弃数量
        /// </summary>
        [Label("废弃数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<BatchWipProductRepaire>.Register(e => e.ScrapQty);

        /// <summary>
        /// 废弃数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return GetProperty(ScrapQtyProperty); }
            set { SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 报废原因 ScrapReason
        /// <summary>
        /// 报废原因
        /// </summary>
        [Label("报废原因")]
        public static readonly Property<string> ScrapReasonProperty = P<BatchWipProductRepaire>.Register(e => e.ScrapReason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason
        {
            get { return this.GetProperty(ScrapReasonProperty); }
            set { this.SetProperty(ScrapReasonProperty, value); }
        }
        #endregion  

        #region 返修时间 RepaireTime
        /// <summary>
        /// 返修时间
        /// </summary>
        [Label("返修时间")]
        public static readonly Property<DateTime> RepaireTimeProperty = P<BatchWipProductRepaire>.Register(e => e.RepaireTime);

        /// <summary>
        /// 返修时间
        /// </summary>
        public DateTime RepaireTime
        {
            get { return GetProperty(RepaireTimeProperty); }
            set { SetProperty(RepaireTimeProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<BatchWipProductRepaire>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<BatchWipProductRepaire>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty = P<BatchWipProductRepaire>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductDefect> DefectProperty = P<BatchWipProductRepaire>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public BatchWipProductDefect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<BatchWipProductRepaire>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<BatchWipProductRepaire>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<BatchWipProductRepaire>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<BatchWipProductRepaire>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 返修人 ReparieBy
        /// <summary>
        /// 返修人Id
        /// </summary>
        [Label("返修人")]
        public static readonly IRefIdProperty ReparieByIdProperty = P<BatchWipProductRepaire>.RegisterRefId(e => e.ReparieById, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> ReparieByProperty = P<BatchWipProductRepaire>.RegisterRef(e => e.ReparieBy, ReparieByIdProperty);

        /// <summary>
        /// 返修人
        /// </summary>
        public Employee ReparieBy
        {
            get { return GetRefEntity(ReparieByProperty); }
            set { SetRefEntity(ReparieByProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<BatchWipProductRepaire>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Station> StationProperty = P<BatchWipProductRepaire>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 版本 Version
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本")]
        public static readonly IRefIdProperty VersionIdProperty = P<BatchWipProductRepaire>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 版本Id
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductVersion> VersionProperty = P<BatchWipProductRepaire>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 版本
        /// </summary>
        public BatchWipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<BatchWipProductRepaire>.RegisterView(e => e.StationName, p => p.Station.Name);

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
        public static readonly Property<string> ResourceNameProperty = P<BatchWipProductRepaire>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

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
        public static readonly Property<string> ProcessNameProperty = P<BatchWipProductRepaire>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion
        #region 返修人名称 ReparieByName
        /// <summary>
        /// 返修人名称
        /// </summary>
        [Label("返修人")]
        public static readonly Property<string> ReparieByNameProperty = P<BatchWipProductRepaire>.RegisterView(e => e.ReparieByName, p => p.ReparieBy.Name);

        /// <summary>
        /// 返修人名称
        /// </summary>
        public string ReparieByName
        {
            get { return this.GetProperty(ReparieByNameProperty); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次")]
        public static readonly Property<string> ShiftNameProperty = P<BatchWipProductRepaire>.RegisterView(e => e.ShiftName, p => p.Shift.Name);

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
    internal class BatchWipProductRepaireConfig : EntityConfig<BatchWipProductRepaire>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_REP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}