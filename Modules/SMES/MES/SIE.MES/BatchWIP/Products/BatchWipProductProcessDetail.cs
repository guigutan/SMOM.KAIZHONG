using SIE.Barcodes.WipBatchs;
using SIE.Common;
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
    /// 批次采集工序明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次采集工序明细")]
    public partial class BatchWipProductProcessDetail : DataEntity
    {
        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipProductProcessDetail>.Register(e => e.BatchNo);

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
        public static readonly Property<string> SubBatchNoProperty = P<BatchWipProductProcessDetail>.Register(e => e.SubBatchNo);

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
        public static readonly Property<string> ContainerNoProperty = P<BatchWipProductProcessDetail>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion 

        #region 出入类型 PlugType
        /// <summary>
        /// 出入类型
        /// </summary>
        [Label("出入类型")]
        public static readonly Property<PlugType> PlugTypeProperty = P<BatchWipProductProcessDetail>.Register(e => e.PlugType);

        /// <summary>
        /// 出入类型
        /// </summary>
        public PlugType PlugType
        {
            get { return this.GetProperty(PlugTypeProperty); }
            set { this.SetProperty(PlugTypeProperty, value); }
        }
        #endregion

        #region 批次状态 BatchState
        /// <summary>
        /// 批次状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<BatchState> BatchStateProperty = P<BatchWipProductProcessDetail>.Register(e => e.BatchState);

        /// <summary>
        /// 批次状态
        /// </summary>
        public BatchState BatchState
        {
            get { return this.GetProperty(BatchStateProperty); }
            set { this.SetProperty(BatchStateProperty, value); }
        }
        #endregion 

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchWipProductProcessDetail>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal?> RemainQtyProperty = P<BatchWipProductProcessDetail>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal? RemainQty
        {
            get { return GetProperty(RemainQtyProperty); }
            set { SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<BatchWipProductProcessDetail>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return GetProperty(ScrapQtyProperty); }
            set { SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> NgQtyProperty = P<BatchWipProductProcessDetail>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion 

        #region 采集结果 ResultType
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("结果类型")]
        public static readonly Property<ResultType> ResultTypeProperty = P<BatchWipProductProcessDetail>.Register(e => e.ResultType);

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType ResultType
        {
            get { return GetProperty(ResultTypeProperty); }
            set { SetProperty(ResultTypeProperty, value); }
        }
        #endregion

        #region 操作人 OperateBy
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperateByIdProperty = P<BatchWipProductProcessDetail>.RegisterRefId(e => e.OperateById, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperateById
        {
            get { return (double)GetRefId(OperateByIdProperty); }
            set { SetRefId(OperateByIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperateByProperty = P<BatchWipProductProcessDetail>.RegisterRef(e => e.OperateBy, OperateByIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee OperateBy
        {
            get { return GetRefEntity(OperateByProperty); }
            set { SetRefEntity(OperateByProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<BatchWipProductProcessDetail>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Station> StationProperty = P<BatchWipProductProcessDetail>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<BatchWipProductProcessDetail>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<BatchWipProductProcessDetail>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<BatchWipProductProcessDetail>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<BatchWipProductProcessDetail>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly IRefIdProperty ResourceIdProperty = P<BatchWipProductProcessDetail>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<BatchWipProductProcessDetail>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序记录 Process
        /// <summary>
        /// 工序记录Id
        /// </summary>
        [Label("工序记录")]
        public static readonly IRefIdProperty ProductProcessIdProperty =
            P<BatchWipProductProcessDetail>.RegisterRefId(e => e.ProductProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序记录Id
        /// </summary>
        public double ProductProcessId
        {
            get { return (double)this.GetRefId(ProductProcessIdProperty); }
            set { this.SetRefId(ProductProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序记录
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductProcess> ProductProcessProperty =
            P<BatchWipProductProcessDetail>.RegisterRef(e => e.ProductProcess, ProductProcessIdProperty);

        /// <summary>
        /// 工序记录
        /// </summary>
        public BatchWipProductProcess ProductProcess
        {
            get { return this.GetRefEntity(ProductProcessProperty); }
            set { this.SetRefEntity(ProductProcessProperty, value); }
        }
        #endregion

        #region 入站时间 InputDate
        /// <summary>
        /// 入站时间
        /// </summary>
        [Label("入站时间")]
        public static readonly Property<DateTime> InputDateProperty = P<BatchWipProductProcessDetail>.Register(e => e.InputDate);

        /// <summary>
        /// 入站时间
        /// </summary>
        public DateTime InputDate
        {
            get { return GetProperty(InputDateProperty); }
            set { SetProperty(InputDateProperty, value); }
        }
        #endregion

        #region 出站时间 OutputDate
        /// <summary>
        /// 出站时间
        /// </summary>
        [Label("出站时间")]
        public static readonly Property<DateTime?> OutputDateProperty = P<BatchWipProductProcessDetail>.Register(e => e.OutputDate);

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? OutputDate
        {
            get { return GetProperty(OutputDateProperty); }
            set { SetProperty(OutputDateProperty, value); }
        }
        #endregion

        #region 关键件列表 KeyItemList
        /// <summary>
        /// 关键件列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductProcessKeyItem>> KeyItemListProperty = P<BatchWipProductProcessDetail>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 关键件列表
        /// </summary>
        public EntityList<BatchWipProductProcessKeyItem> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion

        #region 入站明细ID InputDetailId
        /// <summary>
        /// 入站明细ID
        /// </summary>
        [Label("入站明细ID")]
        public static readonly Property<double?> InputDetailIdProperty = P<BatchWipProductProcessDetail>.Register(e => e.InputDetailId);

        /// <summary>
        /// 入站明细ID
        /// </summary>
        public double? InputDetailId
        {
            get { return this.GetProperty(InputDetailIdProperty); }
            set { this.SetProperty(InputDetailIdProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<BatchWipProductProcessDetail>.RegisterView(e => e.StationName, p => p.Station.Name);

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
        public static readonly Property<string> ResourceNameProperty = P<BatchWipProductProcessDetail>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

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
        public static readonly Property<string> ProcessNameProperty = P<BatchWipProductProcessDetail>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 操作人名称 OperateByName
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Label("操作人")]
        public static readonly Property<string> OperateByNameProperty = P<BatchWipProductProcessDetail>.RegisterView(e => e.OperateByName, p => p.OperateBy.Name);

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperateByName
        {
            get { return this.GetProperty(OperateByNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 批次采集工序明细 实体配置
    /// </summary>
    internal class BatchWipProductProcessDetailConfig : EntityConfig<BatchWipProductProcessDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_PROCESS_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}