using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 生产批次
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产批次")]
    [DisplayMember(nameof(BatchNo))]
    public partial class WipBatch : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipBatch()
        {
            PrintedState = BarcodeState.Notprint;
            PrintTimes = 0;
        }

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<WipBatch>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 是否报废 IsScraped
        /// <summary>
        /// 是否报废
        /// </summary>
        [Label("是否报废")]
        public static readonly Property<bool> IsScrapedProperty = P<WipBatch>.Register(e => e.IsScraped);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScraped
        {
            get { return GetProperty(IsScrapedProperty); }
            set { SetProperty(IsScrapedProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<WipBatch>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 满箱数量 BoxesQty
        /// <summary>
        /// 满箱数量
        /// </summary>
        [Label("满箱数量")]
        public static readonly Property<decimal> BoxesQtyProperty = P<WipBatch>.Register(e => e.BoxesQty);

        /// <summary>
        /// 满箱数量
        /// </summary>
        public decimal BoxesQty
        {
            get { return GetProperty(BoxesQtyProperty); }
            set { SetProperty(BoxesQtyProperty, value); }
        }
        #endregion

        #region 是否尾数 IsMantissa
        /// <summary>
        /// 是否尾数
        /// </summary>
        [Label("是否尾数")]
        public static readonly Property<bool> IsMantissaProperty = P<WipBatch>.Register(e => e.IsMantissa);

        /// <summary>
        /// 是否尾数
        /// </summary>
        public bool IsMantissa
        {
            get { return GetProperty(IsMantissaProperty); }
            set { SetProperty(IsMantissaProperty, value); }
        }
        #endregion

        #region 打印日期 PrintDate
        /// <summary>
        /// 打印日期
        /// </summary>
        [Label("打印日期")]
        public static readonly Property<DateTime?> PrintDateProperty = P<WipBatch>.Register(e => e.PrintDate);

        /// <summary>
        /// 打印日期
        /// </summary>
        public DateTime? PrintDate
        {
            get { return GetProperty(PrintDateProperty); }
            set { SetProperty(PrintDateProperty, value); }
        }
        #endregion

        #region 打印次数 PrintTimes
        /// <summary>
        /// 打印次数
        /// </summary>
        [MinValue(0)]
        [Label("打印次数")]
        public static readonly Property<int> PrintTimesProperty = P<WipBatch>.Register(e => e.PrintTimes);

        /// <summary>
        /// 打印次数
        /// </summary>
        public int PrintTimes
        {
            get { return GetProperty(PrintTimesProperty); }
            set { SetProperty(PrintTimesProperty, value); }
        }
        #endregion

        #region 打印状态 State
        /// <summary>
        /// 打印状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<BarcodeState> StateProperty = P<WipBatch>.Register(e => e.PrintedState);

        /// <summary>
        /// 打印状态
        /// </summary>
        public BarcodeState PrintedState
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 批次状态 State
        /// <summary>
        /// 批次状态
        /// </summary>
        [Label("批次状态")]
        public static readonly Property<BatchState> BatchStateProperty = P<WipBatch>.Register(e => e.BatchState);

        /// <summary>
        /// 批次状态
        /// </summary>
        public BatchState BatchState
        {
            get { return GetProperty(BatchStateProperty); }
            set { SetProperty(BatchStateProperty, value); }
        }
        #endregion

        #region 是否生成子批次号 IsGenerateChild 
        /// <summary>
        /// 是否生成子批次号
        /// 生成了子批次号后只能通过子批次入站
        /// </summary>
        [Label("是否生成子批次号")]
        public static readonly Property<bool> IsGenerateChildProperty = P<WipBatch>.Register(e => e.IsGenerateChild);

        /// <summary>
        /// 是否生成子批次号
        /// </summary>
        public bool IsGenerateChild
        {
            get { return this.GetProperty(IsGenerateChildProperty); }
            set { this.SetProperty(IsGenerateChildProperty, value); }
        }
        #endregion

        #region 是否批次生成,用于区分批次是批次生成创建的还是批次拆分创建 IsGenerate
        /// <summary>
        /// 是否批次生成,用于区分批次是批次生成创建的还是批次拆分创建
        /// </summary>
        [Label("是否批次生成")]
        public static readonly Property<bool> IsGenerateProperty = P<WipBatch>.Register(e => e.IsGenerate);

        /// <summary>
        /// 是否批次生成,用于区分批次是批次生成创建的还是批次拆分创建
        /// </summary>
        public bool IsGenerate
        {
            get { return this.GetProperty(IsGenerateProperty); }
            set { this.SetProperty(IsGenerateProperty, value); }
        }
        #endregion 

        #region  条码范围 Range
        /// <summary>
        ///  条码范围Id
        /// </summary>
        [Label("条码范围")]
        public static readonly IRefIdProperty RangeIdProperty = P<WipBatch>.RegisterRefId(e => e.RangeId, ReferenceType.Normal);

        /// <summary>
        ///  条码范围Id
        /// </summary>
        public double? RangeId
        {
            get { return (double?)GetRefNullableId(RangeIdProperty); }
            set { SetRefNullableId(RangeIdProperty, value); }
        }

        /// <summary>
        ///  条码范围
        /// </summary>
        public static readonly RefEntityProperty<BarcodeRange> RangeProperty = P<WipBatch>.RegisterRef(e => e.Range, RangeIdProperty);

        /// <summary>
        ///  条码范围
        /// </summary>
        public BarcodeRange Range
        {
            get { return GetRefEntity(RangeProperty); }
            set { SetRefEntity(RangeProperty, value); }
        }
        #endregion

        #region 打印员 PrintBy
        /// <summary>
        /// 打印员Id
        /// </summary>
        [Label("打印员")]
        public static readonly IRefIdProperty PrintByIdProperty = P<WipBatch>.RegisterRefId(e => e.PrintById, ReferenceType.Normal);

        /// <summary>
        /// 打印员Id
        /// </summary>
        public double? PrintById
        {
            get { return (double?)GetRefNullableId(PrintByIdProperty); }
            set { SetRefNullableId(PrintByIdProperty, value); }
        }

        /// <summary>
        /// 打印员
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrintByProperty = P<WipBatch>.RegisterRef(e => e.PrintBy, PrintByIdProperty);

        /// <summary>
        /// 打印员
        /// </summary>
        public Employee PrintBy
        {
            get { return GetRefEntity(PrintByProperty); }
            set { SetRefEntity(PrintByProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WipBatch>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WipBatch>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 是否子批 IsChild
        /// <summary>
        /// 是否子批
        /// </summary>
        [Label("是否子批")]
        public static readonly Property<bool> IsChildProperty = P<WipBatch>.Register(e => e.IsChild);

        /// <summary>
        /// 是否子批
        /// </summary>
        public bool IsChild
        {
            get { return this.GetProperty(IsChildProperty); }
            set { this.SetProperty(IsChildProperty, value); }
        }
        #endregion

        #region 子生产批次列表 BatchList
        /// <summary>
        /// 子生产批次列表
        /// </summary>
        public static readonly ListProperty<EntityList<SubWipBatch>> BatchListProperty = P<WipBatch>.RegisterList(e => e.BatchList);

        /// <summary>
        /// 子生产批次列表
        /// </summary>
        public EntityList<SubWipBatch> BatchList
        {
            get { return this.GetLazyList(BatchListProperty); }
        }
        #endregion

        #region 剩余数量 RemainQty---不映射数据库
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal?> RemainQtyProperty = P<WipBatch>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal? RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal?> ScrapQtyProperty = P<WipBatch>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal? ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 是否已使用 Isuse
        /// <summary>
        /// 是否已使用
        /// </summary>
        [Label("是否已使用")]
        public static readonly Property<bool> IsuseProperty = P<WipBatch>.Register(e => e.Isuse);

        /// <summary>
        /// 是否已使用
        /// </summary>
        public bool Isuse
        {
            get { return this.GetProperty(IsuseProperty); }
            set { this.SetProperty(IsuseProperty, value); }
        }
        #endregion

        #region 来源标签 SourceNo
        /// <summary>
        /// 来源标签
        /// </summary>
        [Label("来源标签")]
        public static readonly Property<string> SourceNoProperty = P<WipBatch>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源标签
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 报工记录Id ReportRecordIds
        /// <summary>
        /// 报工记录Id
        /// </summary>
        [Label("报工记录Id")]
        public static readonly Property<string> ReportRecordIdsProperty = P<WipBatch>.Register(e => e.ReportRecordIds);

        /// <summary>
        /// 报工记录Id
        /// </summary>
        public string ReportRecordIds
        {
            get { return this.GetProperty(ReportRecordIdsProperty); }
            set { this.SetProperty(ReportRecordIdsProperty, value); }
        }
        #endregion

        #region 凯中扩展
        #region 是否可疑品 IsSuspectProduct
        /// <summary>
        /// 是否可疑品
        /// </summary>
        [Label("是否可疑品")]
        public static readonly Property<YesNo?> IsSuspectProductProperty = P<WipBatch>.Register(e => e.IsSuspectProduct);

        /// <summary>
        /// 是否可疑品
        /// </summary>
        public YesNo? IsSuspectProduct
        {
            get { return this.GetProperty(IsSuspectProductProperty); }
            set { this.SetProperty(IsSuspectProductProperty, value); }
        }
        #endregion

        #region 是否返工 IsRework
        /// <summary>
        /// 是否返工
        /// </summary>
        [Label("是否返工")]
        public static readonly Property<bool> IsReworkProperty = P<WipBatch>.Register(e => e.IsRework);

        /// <summary>
        /// 是否返工
        /// </summary>
        public bool IsRework
        {
            get { return this.GetProperty(IsReworkProperty); }
            set { this.SetProperty(IsReworkProperty, value); }
        }
        #endregion

        #region 任务单Id DispatchTaskId
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单Id")]
        public static readonly Property<double?> DispatchTaskIdProperty = P<WipBatch>.Register(e => e.DispatchTaskId);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return this.GetProperty(DispatchTaskIdProperty); }
            set { this.SetProperty(DispatchTaskIdProperty, value); }
        }
        #endregion

        #region 包装任务单Id PackingTaskId
        /// <summary>
        /// 包装任务单Id
        /// </summary>
        [Label("包装任务单Id")]
        public static readonly Property<double?> PackingTaskIdProperty = P<WipBatch>.Register(e => e.PackingTaskId);

        /// <summary>
        /// 包装任务单Id
        /// </summary>
        public double? PackingTaskId
        {
            get { return this.GetProperty(PackingTaskIdProperty); }
            set { this.SetProperty(PackingTaskIdProperty, value); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<WipBatch>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 模板工序 ProcessCode
        /// <summary>
        /// 模板工序
        /// </summary>
        [Label("模板工序")]
        public static readonly Property<string> ProcessCodeProperty = P<WipBatch>.Register(e => e.ProcessCode);

        /// <summary>
        /// 模板工序
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 生成工序 GenerateProcessCode
        /// <summary>
        /// 生成工序
        /// </summary>
        [Label("生成工序")]
        public static readonly Property<string> GenerateProcessCodeProperty = P<WipBatch>.Register(e => e.GenerateProcessCode);

        /// <summary>
        /// 生成工序
        /// </summary>
        public string GenerateProcessCode
        {
            get { return this.GetProperty(GenerateProcessCodeProperty); }
            set { this.SetProperty(GenerateProcessCodeProperty, value); }
        }
        #endregion

        #region 是否已上传IOT IsUploadIot
        /// <summary>
        /// 是否已上传IOT
        /// </summary>
        [Label("是否已上传IOT")]
        public static readonly Property<bool?> IsUploadIotProperty = P<WipBatch>.Register(e => e.IsUploadIot);

        /// <summary>
        /// 是否已上传IOT
        /// </summary>
        public bool? IsUploadIot
        {
            get { return this.GetProperty(IsUploadIotProperty); }
            set { this.SetProperty(IsUploadIotProperty, value); }
        }
        #endregion

        #region 是否委外生成标签 IsOutsourcing
        /// <summary>
        /// 是否委外生成标签
        /// </summary>
        [Label("是否委外生成标签")]
        public static readonly Property<bool?> IsOutsourcingProperty = P<WipBatch>.Register(e => e.IsOutsourcing);

        /// <summary>
        /// 是否委外生成标签
        /// </summary>
        public bool? IsOutsourcing
        {
            get { return this.GetProperty(IsOutsourcingProperty); }
            set { this.SetProperty(IsOutsourcingProperty, value); }
        }
        #endregion

        #region 是否委外生成标签 IsUpload
        /// <summary>
        /// 是否上传事务
        /// </summary>
        [Label("是否上传事务")]
        public static readonly Property<bool?> IsUploadProperty = P<WipBatch>.Register(e => e.IsUpload);

        /// <summary>
        /// 是否上传事务
        /// </summary>
        public bool? IsUpload
        {
            get { return this.GetProperty(IsUploadProperty); }
            set { this.SetProperty(IsUploadProperty, value); }
        }
        #endregion

        #region 重传次数 ReLoadCount
        /// <summary>
        /// 重传次数
        /// </summary>
        [Label("重传次数")]
        public static readonly Property<int?> ReLoadCountProperty = P<WipBatch>.Register(e => e.ReLoadCount);

        /// <summary>
        /// 重传次数
        /// </summary>
        public int? ReLoadCount
        {
            get { return this.GetProperty(ReLoadCountProperty); }
            set { this.SetProperty(ReLoadCountProperty, value); }
        }
        #endregion

        #region CS打印生成 IsPressureSnPrint
        /// <summary>
        /// CS打印生成
        /// </summary>
        [Label("CS打印生成")]
        public static readonly Property<bool?> IsPressureSnPrintProperty = P<WipBatch>.Register(e => e.IsPressureSnPrint);

        /// <summary>
        /// CS打印生成
        /// </summary>
        public bool? IsPressureSnPrint
        {
            get { return this.GetProperty(IsPressureSnPrintProperty); }
            set { this.SetProperty(IsPressureSnPrintProperty, value); }
        }
        #endregion

        #region 修改数量工序 EditQtyProcessCode
        /// <summary>
        /// 修改数量工序
        /// </summary>
        [Label("修改数量工序")]
        public static readonly Property<string> EditQtyProcessCodeProperty = P<WipBatch>.Register(e => e.EditQtyProcessCode);

        /// <summary>
        /// 修改数量工序
        /// </summary>
        public string EditQtyProcessCode
        {
            get { return this.GetProperty(EditQtyProcessCodeProperty); }
            set { this.SetProperty(EditQtyProcessCodeProperty, value); }
        }
        #endregion

        #endregion

        #region 不映射数据库

        #region 批次完工数量 ReportQty
        /// <summary>
        /// 批次完工数量
        /// </summary>
        [Label("批次完工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<WipBatch>.Register(e => e.ReportQty);

        /// <summary>
        /// 批次完工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return this.GetProperty(ReportQtyProperty); }
            set { this.SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 前工序完工数量 PreReportQty
        /// <summary>
        /// 前工序完工数量
        /// </summary>
        [Label("前工序完工数量")]
        public static readonly Property<decimal> PreReportQtyProperty = P<WipBatch>.Register(e => e.PreReportQty);

        /// <summary>
        /// 前工序完工数量
        /// </summary>
        public decimal PreReportQty
        {
            get { return this.GetProperty(PreReportQtyProperty); }
            set { this.SetProperty(PreReportQtyProperty, value); }
        }
        #endregion

        #region 在制数量 InProcessQty
        /// <summary>
        /// 在制数量
        /// </summary>
        [Label("在制数量")]
        public static readonly Property<decimal> InProcessQtyProperty = P<WipBatch>.RegisterReadOnly(
            e => e.InProcessQty, e => e.GetInProcessQty(), ReportQtyProperty, PreReportQtyProperty);
        /// <summary>
        /// 在制数量
        /// </summary>

        public decimal InProcessQty
        {
            get { return this.GetProperty(InProcessQtyProperty); }
        }
        private decimal GetInProcessQty()
        {
            if (IsScraped) return 0;
            if (IsSuspectProduct == YesNo.Yes) return Qty;
            var qty = Qty == 0 ? 0 : (Qty - ReportQty);
            //if (qty < 0) qty = 0;

            return qty <= 0 ? 0 : qty;
        }
        #endregion

        #region 打印工序 PrintProcessCode
        /// <summary>
        /// 打印工序
        /// </summary>
        [Label("打印工序")]
        public static readonly Property<string> PrintProcessCodeProperty = P<WipBatch>.Register(e => e.PrintProcessCode);

        /// <summary>
        /// 打印工序
        /// </summary>
        public string PrintProcessCode
        {
            get { return this.GetProperty(PrintProcessCodeProperty); }
            set { this.SetProperty(PrintProcessCodeProperty, value); }
        }
        #endregion


        #endregion

        #region 视图属性
        #region 开始条码 StartSn
        /// <summary>
        /// 开始条码
        /// </summary>
        [Label("开始条码")]
        public static readonly Property<string> StartSnProperty = P<WipBatch>.RegisterView(e => e.StartSn, p => p.Range.StartSn);

        /// <summary>
        /// 开始条码
        /// </summary>
        public string StartSn
        {
            get { return this.GetProperty(StartSnProperty); }
        }
        #endregion

        #region 结束条码 EndSn
        /// <summary>
        /// 结束条码
        /// </summary>
        [Label("结束条码")]
        public static readonly Property<string> EndSnProperty = P<WipBatch>.RegisterView(e => e.EndSn, p => p.Range.EndSn);

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn
        {
            get { return this.GetProperty(EndSnProperty); }
        }
        #endregion

        #region 打印员 PrintByName
        /// <summary>
        /// 打印员
        /// </summary>
        [Label("打印员")]
        public static readonly Property<string> PrintByNameProperty = P<WipBatch>.RegisterView(e => e.PrintByName, p => p.PrintBy.Name);

        /// <summary>
        /// 批次编码
        /// </summary>
        public string PrintByName
        {
            get { return this.GetProperty(PrintByNameProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipBatch>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<WipBatch>.RegisterView(e => e.ProductId, p => p.WorkOrder.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
        }
        #endregion


        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WipBatch>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<WipBatch>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<WipBatch>.RegisterView(e => e.ShortDescription, p => p.WorkOrder.Product.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }

        #endregion
        #endregion
    }

    /// <summary>
    /// 生产批次 实体配置
    /// </summary>
    internal class WipBatchEntityConfig : EntityConfig<WipBatch>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_BATCH").MapAllProperties();
            Meta.Property(WipBatch.RemainQtyProperty).DontMapColumn();
            Meta.Property(WipBatch.ReportQtyProperty).DontMapColumn();
            Meta.Property(WipBatch.PreReportQtyProperty).DontMapColumn();
            Meta.Property(WipBatch.PrintProcessCodeProperty).DontMapColumn();

            Meta.Property(WipBatch.BatchNoProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}