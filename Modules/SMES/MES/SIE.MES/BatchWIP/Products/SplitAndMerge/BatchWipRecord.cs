using SIE.Common;
using SIE.Domain;
using SIE.MES.BatchWIP.Products.SplitAndMerge.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BatchWIP.Products.SplitAndMerge
{
    /// <summary>
    /// 批次采集记录(出站入站分离)
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次采集记录(出站入站分离)")]
    public class BatchWipRecord : DataEntity
    {
        #region 批次生产通用 BatchVersion
        /// <summary>
        /// 批次生产通用Id
        /// </summary>
        [Label("批次生产通用")]
        public static readonly IRefIdProperty BatchVersionIdProperty =
            P<BatchWipRecord>.RegisterRefId(e => e.BatchVersionId, ReferenceType.Parent);

        /// <summary>
        /// 批次生产通用Id
        /// </summary>
        public double BatchVersionId
        {
            get { return (double)this.GetRefId(BatchVersionIdProperty); }
            set { this.SetRefId(BatchVersionIdProperty, value); }
        }

        /// <summary>
        /// 批次生产通用
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductVersion> BatchVersionProperty =
            P<BatchWipRecord>.RegisterRef(e => e.BatchVersion, BatchVersionIdProperty);

        /// <summary>
        /// 批次生产通用
        /// </summary>
        public BatchWipProductVersion BatchVersion
        {
            get { return this.GetRefEntity(BatchVersionProperty); }
            set { this.SetRefEntity(BatchVersionProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipRecord>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 原批次号 SourceBatchNo
        /// <summary>
        /// 原批次号
        /// </summary>
        [MaxLength(2000)]
        [Label("原批次号")]
        public static readonly Property<string> SourceBatchNoProperty = P<BatchWipRecord>.Register(e => e.SourceBatchNo);

        /// <summary>
        /// 原批次号
        /// </summary>
        public string SourceBatchNo
        {
            get { return this.GetProperty(SourceBatchNoProperty); }
            set { this.SetProperty(SourceBatchNoProperty, value); }
        }
        #endregion

        #region 出入类型 InOutType
        /// <summary>
        /// 出入类型
        /// </summary>
        [Label("出入类型")]
        public static readonly Property<PlugType> InOutTypeProperty = P<BatchWipRecord>.Register(e => e.InOutType);

        /// <summary>
        /// 出入类型
        /// </summary>
        public PlugType InOutType
        {
            get { return this.GetProperty(InOutTypeProperty); }
            set { this.SetProperty(InOutTypeProperty, value); }
        }
        #endregion

        #region 载具号 ContainerNo
        /// <summary>
        /// 载具号
        /// </summary>
        [Label("载具号")]
        public static readonly Property<string> ContainerNoProperty = P<BatchWipRecord>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchWipRecord>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 拆分数量 SplitQty
        /// <summary>
        /// 拆分数量
        /// </summary>
        [Label("拆分数量")]
        public static readonly Property<decimal> SplitQtyProperty = P<BatchWipRecord>.Register(e => e.SplitQty);

        /// <summary>
        /// 拆分数量
        /// </summary>
        public decimal SplitQty
        {
            get { return this.GetProperty(SplitQtyProperty); }
            set { this.SetProperty(SplitQtyProperty, value); }
        }
        #endregion

        #region 不良数量 DefectQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> DefectQtyProperty = P<BatchWipRecord>.Register(e => e.DefectQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal DefectQty
        {
            get { return this.GetProperty(DefectQtyProperty); }
            set { this.SetProperty(DefectQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<BatchWipRecord>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty =
            P<BatchWipRecord>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)this.GetRefId(ShiftIdProperty); }
            set { this.SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty =
            P<BatchWipRecord>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return this.GetRefEntity(ShiftProperty); }
            set { this.SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<BatchWipRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<BatchWipRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<BatchWipRecord>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<BatchWipRecord>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<BatchWipRecord>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Station> StationProperty =
            P<BatchWipRecord>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 结果类型 ResultType
        /// <summary>
        /// 结果类型
        /// </summary>
        [Label("结果类型")]
        public static readonly Property<ResultType> ResultTypeProperty = P<BatchWipRecord>.Register(e => e.ResultType);

        /// <summary>
        /// 结果类型
        /// </summary>
        public ResultType ResultType
        {
            get { return this.GetProperty(ResultTypeProperty); }
            set { this.SetProperty(ResultTypeProperty, value); }
        }
        #endregion

        #region 关键件列表 KeyItemList
        /// <summary>
        /// 关键件列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductProcessKeyItem>> KeyItemListProperty = P<BatchWipRecord>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 关键件列表
        /// </summary>
        public EntityList<BatchWipProductProcessKeyItem> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class BatchWipRecordConfig : EntityConfig<BatchWipRecord>
    {
        /// <summary>
        /// 实体元
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_WIP_RCD").MapAllProperties();
            Meta.Property(BatchWipRecord.SourceBatchNoProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
