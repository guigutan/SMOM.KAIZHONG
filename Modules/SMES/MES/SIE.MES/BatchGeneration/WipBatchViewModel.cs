using SIE.Barcodes;
using SIE.Barcodes.WipBatchs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.MES.BatchGeneration
{
    /// <summary>
    /// 生产批次
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产批次")]
    [DisplayMember(nameof(BatchNo))]
    public partial class WipBatchViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipBatchViewModel()
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
        public static readonly Property<string> BatchNoProperty = P<WipBatchViewModel>.Register(e => e.BatchNo);

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
        public static readonly Property<bool> IsScrapedProperty = P<WipBatchViewModel>.Register(e => e.IsScraped);

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
        public static readonly Property<decimal> QtyProperty = P<WipBatchViewModel>.Register(e => e.Qty);

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
        public static readonly Property<decimal> BoxesQtyProperty = P<WipBatchViewModel>.Register(e => e.BoxesQty);

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
        public static readonly Property<bool> IsMantissaProperty = P<WipBatchViewModel>.Register(e => e.IsMantissa);

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
        public static readonly Property<DateTime?> PrintDateProperty = P<WipBatchViewModel>.Register(e => e.PrintDate);

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
        public static readonly Property<int> PrintTimesProperty = P<WipBatchViewModel>.Register(e => e.PrintTimes);

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
        public static readonly Property<BarcodeState> StateProperty = P<WipBatchViewModel>.Register(e => e.PrintedState);

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
        public static readonly Property<BatchState> BatchStateProperty = P<WipBatchViewModel>.Register(e => e.BatchState);

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
        public static readonly Property<bool> IsGenerateChildProperty = P<WipBatchViewModel>.Register(e => e.IsGenerateChild);

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
        public static readonly Property<bool> IsGenerateProperty = P<WipBatchViewModel>.Register(e => e.IsGenerate);

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
        public static readonly IRefIdProperty RangeIdProperty = P<WipBatchViewModel>.RegisterRefId(e => e.RangeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<BarcodeRange> RangeProperty = P<WipBatchViewModel>.RegisterRef(e => e.Range, RangeIdProperty);

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
        public static readonly IRefIdProperty PrintByIdProperty = P<WipBatchViewModel>.RegisterRefId(e => e.PrintById, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> PrintByProperty = P<WipBatchViewModel>.RegisterRef(e => e.PrintBy, PrintByIdProperty);

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
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WipBatchViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WipBatchViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

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
        public static readonly Property<bool> IsChildProperty = P<WipBatchViewModel>.Register(e => e.IsChild);

        /// <summary>
        /// 是否子批
        /// </summary>
        public bool IsChild
        {
            get { return this.GetProperty(IsChildProperty); }
            set { this.SetProperty(IsChildProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty---不映射数据库
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal?> RemainQtyProperty = P<WipBatchViewModel>.Register(e => e.RemainQty);

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
        public static readonly Property<decimal?> ScrapQtyProperty = P<WipBatchViewModel>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal? ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateTime> CreateDateProperty = P<WipBatchViewModel>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 修改时间 UpdateDate
        /// <summary>
        /// 修改时间
        /// </summary>
        [Label("修改时间")]
        public static readonly Property<DateTime?> UpdateDateProperty = P<WipBatchViewModel>.Register(e => e.UpdateDate);

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateDate
        {
            get { return this.GetProperty(UpdateDateProperty); }
            set { this.SetProperty(UpdateDateProperty, value); }
        }
        #endregion

        #region 创建人 CreateBy
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建人")]
        public static readonly Property<string> CreateByProperty = P<WipBatchViewModel>.Register(e => e.CreateBy);

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy
        {
            get { return this.GetProperty(CreateByProperty); }
            set { this.SetProperty(CreateByProperty, value); }
        }
        #endregion


        #region 修改人 UpdateBy
        /// <summary>
        /// 修改人
        /// </summary>
        [Label("修改人")]
        public static readonly Property<string> UpdateByProperty = P<WipBatchViewModel>.Register(e => e.UpdateBy);

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy
        {
            get { return this.GetProperty(UpdateByProperty); }
            set { this.SetProperty(UpdateByProperty, value); }
        }
        #endregion

        #region 视图属性


        #region 打印员 PrintByName
        /// <summary>
        /// 打印员
        /// </summary>
        [Label("打印员")]
        public static readonly Property<string> PrintByNameProperty = P<WipBatchViewModel>.Register(e => e.PrintByName);

        /// <summary>
        /// 打印员
        /// </summary>
        public string PrintByName
        {
            get { return this.GetProperty(PrintByNameProperty); }
            set { this.SetProperty(PrintByNameProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipBatchViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion
        #endregion
    }
}