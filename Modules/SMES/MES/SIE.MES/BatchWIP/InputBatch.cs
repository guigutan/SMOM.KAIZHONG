using SIE.Barcodes.WipBatchs;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Linq;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 转入批次
    /// </summary>
    [RootEntity, Serializable]
    [Label("转入批次")]
    public partial class InputBatch : DataEntity
    {
        #region 生产批号 BatchNo
        /// <summary>
        /// 生产批号
        /// </summary>
        [Label("生产批号")]
        public static readonly Property<string> BatchNoProperty = P<InputBatch>.Register(e => e.BatchNo);

        /// <summary>
        /// 生产批号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 载具号 ContainerNo
        /// <summary>
        /// 载具号
        /// </summary>
        [Label("载具号")]
        public static readonly Property<string> ContainerNoProperty = P<InputBatch>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion

        #region 子批次号 SubBatchNo
        /// <summary>
        /// 子批次号
        /// </summary>
        [Label("子批次号")]
        public static readonly Property<string> SubBatchNoProperty = P<InputBatch>.Register(e => e.SubBatchNo);

        /// <summary>
        /// 子批次号
        /// </summary>
        public string SubBatchNo
        {
            get { return this.GetProperty(SubBatchNoProperty); }
            set { this.SetProperty(SubBatchNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        [MinValue(0)]
        public static readonly Property<decimal> QtyProperty = P<InputBatch>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        [MinValue(0)]
        public static readonly Property<decimal> RemainQtyProperty = P<InputBatch>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 拆分数量 SplitQty
        /// <summary>
        /// 拆分数量
        /// </summary>
        [Label("拆分数量")]
        [MinValue(0)]
        public static readonly Property<decimal> SplitQtyProperty = P<InputBatch>.Register(e => e.SplitQty);

        /// <summary>
        /// 拆分数量
        /// </summary>
        public decimal SplitQty
        {
            get { return this.GetProperty(SplitQtyProperty); }
            set { this.SetProperty(SplitQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<InputBatch>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 拆分数量只读 SplitReadOnly
        /// <summary>
        /// 拆分数量只读,同批不拆分转出时为true
        /// </summary>
        [Label("拆分数量只读")]
        public static readonly Property<bool> SplitReadOnlyProperty = P<InputBatch>.Register(e => e.SplitReadOnly);

        /// <summary>
        /// 拆分数量只读
        /// </summary>
        public bool SplitReadOnly
        {
            get { return this.GetProperty(SplitReadOnlyProperty); }
            set { this.SetProperty(SplitReadOnlyProperty, value); }
        }
        #endregion 

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        [MinValue(0)]
        public static readonly Property<decimal> NgQtyProperty = P<InputBatch>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion 

        #region 入站时间 InputDate
        /// <summary>
        /// 入站时间
        /// </summary>
        [Label("入站时间")]
        public static readonly Property<DateTime> InputDateProperty = P<InputBatch>.Register(e => e.InputDate);

        /// <summary>
        /// 入站时间
        /// </summary>
        public DateTime InputDate
        {
            get { return this.GetProperty(InputDateProperty); }
            set { this.SetProperty(InputDateProperty, value); }
        }
        #endregion

        #region 批次状态 BatchState
        /// <summary>
        /// 批次状态
        /// </summary>
        [Label("排次状态")]
        public static readonly Property<BatchState> BatchStateProperty = P<InputBatch>.Register(e => e.BatchState);

        /// <summary>
        /// 批次状态
        /// </summary>
        public BatchState BatchState
        {
            get { return this.GetProperty(BatchStateProperty); }
            set { this.SetProperty(BatchStateProperty, value); }
        }
        #endregion 

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<InputBatch>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<InputBatch>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<InputBatch>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<InputBatch>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
            P<InputBatch>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<InputBatch>.RegisterRef(e => e.Process, ProcessIdProperty);

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
            P<InputBatch>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
            P<InputBatch>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 是否子批次 IsChild
        /// <summary>
        /// 是否子批次,用于区分转入批次是否为生产批次转入
        /// </summary>
        [Label("是否子批次")]
        public static readonly Property<bool> IsChildProperty = P<InputBatch>.Register(e => e.IsChild);

        /// <summary>
        /// 是否子批次
        /// </summary>
        public bool IsChild
        {
            get { return this.GetProperty(IsChildProperty); }
            set { this.SetProperty(IsChildProperty, value); }
        }
        #endregion

        #region 生产批次 WipBatch
        /// <summary>
        /// 生产批次Id
        /// </summary>
        [Label("生产批次")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<InputBatch>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

        /// <summary>
        /// 生产批次Id
        /// </summary>
        public double WipBatchId
        {
            get { return (double)this.GetRefId(WipBatchIdProperty); }
            set { this.SetRefId(WipBatchIdProperty, value); }
        }

        /// <summary>
        /// 生产批次
        /// </summary>
        public static readonly RefEntityProperty<WipBatch> WipBatchProperty =
            P<InputBatch>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 生产批次
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region 转入批次类型 InputType
        /// <summary>
        /// 转入批次类型
        /// </summary>
        [Label("转入批次类型")]
        public static readonly Property<BarcodeType?> InputTypeProperty = P<InputBatch>.Register(e => e.InputType);

        /// <summary>
        /// 转入批次类型
        /// </summary>
        public BarcodeType? InputType
        {
            get { return this.GetProperty(InputTypeProperty); }
            set { this.SetProperty(InputTypeProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<InputBatch>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<InputBatch>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<InputBatch>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 缺陷列表 DefectList
        /// <summary>
        /// 缺陷列表
        /// </summary>
        [Label("缺陷列表")]
        public static readonly ListProperty<EntityList<InputBatchDefect>> DefectListProperty = P<InputBatch>.RegisterList(e => e.DefectList);

        /// <summary>
        /// 缺陷列表
        /// </summary>
        public EntityList<InputBatchDefect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region 缺陷 DefectDisplay
        /// <summary>
        /// 缺陷
        /// </summary>
        [Label("缺陷")]
        public static readonly Property<string> DefectDisplayProperty = P<InputBatch>.RegisterReadOnly(
            e => e.DefectDisplay, e => e.GetDefectDisplay(), DefectListProperty);
        /// <summary>
        /// 缺陷
        /// </summary>

        public string DefectDisplay
        {
            get { return this.GetProperty(DefectDisplayProperty); }
        }
        private string GetDefectDisplay()
        {
            if (DefectList.Count <= 0)
            {
                return string.Empty;
            }
            return string.Join(";",DefectList.Select(p => p.DefectDesc).ToList());
        }
        #endregion

    }

    /// <summary>
    /// 批次打印设置 实体配置
    /// </summary>
    internal class IntoBatchEntityConfig : EntityConfig<InputBatch>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INPUT_BATCH").MapAllPropertiesExcept(InputBatch.SplitQtyProperty, InputBatch.SplitReadOnlyProperty, InputBatch.DefectDisplayProperty);
            Meta.EnablePhantoms();
        }
    }
}