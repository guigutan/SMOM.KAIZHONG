using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("条码查询实体")]
    public partial class BarcodeCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodeCriteria()
        {
            PrintDate = new DateRange() { DateRangeType = DateRangeType.All };
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BarcodeCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 条码号 Sn
        /// <summary>
        /// 条码号
        /// </summary>
        [Label("条码号")]
        public static readonly Property<string> SnProperty = P<BarcodeCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 打印人 Printer
        /// <summary>
        /// 打印人Id
        /// </summary>
        [Label("打印人")]
        public static readonly IRefIdProperty PrinterIdProperty =
            P<BarcodeCriteria>.RegisterRefId(e => e.PrinterId, ReferenceType.Normal);

        /// <summary>
        /// 打印人Id
        /// </summary>
        public double? PrinterId
        {
            get { return (double?)this.GetRefNullableId(PrinterIdProperty); }
            set { this.SetRefNullableId(PrinterIdProperty, value); }
        }

        /// <summary>
        /// 打印人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrinterProperty =
            P<BarcodeCriteria>.RegisterRef(e => e.Printer, PrinterIdProperty);

        /// <summary>
        /// 打印人
        /// </summary>
        public Employee Printer
        {
            get { return this.GetRefEntity(PrinterProperty); }
            set { this.SetRefEntity(PrinterProperty, value); }
        }
        #endregion

        #region 打印时间
        /// <summary>
        /// 打印时间
        /// </summary>
        [Label("打印时间")]
        public static readonly Property<DateRange> PrintDateProperty = P<BarcodeCriteria>.Register(e => e.PrintDate);

        /// <summary>
        /// 打印时间
        /// </summary>
        public DateRange PrintDate
        {
            get { return this.GetProperty(PrintDateProperty); }
            set { this.SetProperty(PrintDateProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<BarcodeState?> StateProperty = P<BarcodeCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public BarcodeState? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 报废人 ScrapedBy
        /// <summary>
        /// 报废人Id
        /// </summary>
        public static readonly IRefIdProperty ScrapedByIdProperty = P<BarcodeCriteria>.RegisterRefId(e => e.ScrapedById, ReferenceType.Normal);

        /// <summary>
        /// 报废人Id
        /// </summary>
        public double? ScrapedById
        {
            get { return (double?)GetRefNullableId(ScrapedByIdProperty); }
            set { SetRefNullableId(ScrapedByIdProperty, value); }
        }

        /// <summary>
        /// 报废人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ScrapedByProperty = P<BarcodeCriteria>.RegisterRef(e => e.ScrapedBy, ScrapedByIdProperty);

        /// <summary>
        /// 报废人
        /// </summary>
        public Employee ScrapedBy
        {
            get { return GetRefEntity(ScrapedByProperty); }
            set { SetRefEntity(ScrapedByProperty, value); }
        }
        #endregion

        #region 报废日期 ScrapedDate
        /// <summary>
        /// 报废日期
        /// </summary>
        [Label("报废日期")]
        public static readonly Property<DateRange> ScrapedDateProperty = P<BarcodeCriteria>.Register(e => e.ScrapedDate);

        /// <summary>
        /// 报废日期
        /// </summary>
        public DateRange ScrapedDate
        {
            get { return GetProperty(ScrapedDateProperty); }
            set { SetProperty(ScrapedDateProperty, value); }
        }
        #endregion

        #region 是否报废 IsScraped
        /// <summary>
        /// 是否报废
        /// </summary>
        [Label("是否报废")]
        public static readonly Property<bool?> IsScrapedProperty = P<BarcodeCriteria>.Register(e => e.IsScraped);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool? IsScraped
        {
            get { return GetProperty(IsScrapedProperty); }
            set { SetProperty(IsScrapedProperty, value); }
        }
        #endregion

        #region 是否挂起 IsPending
        /// <summary>
        /// 是否挂起
        /// </summary>
        [Label("是否挂起")]
        public static readonly Property<bool?> IsPendingProperty = P<BarcodeCriteria>.Register(e => e.IsPending);

        /// <summary>
        /// 是否挂起
        /// </summary>
        public bool? IsPending
        {
            get { return GetProperty(IsPendingProperty); }
            set { SetProperty(IsPendingProperty, value); }
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>条码列表</returns>
        protected override EntityList Fetch()
        {
            var ctl = RT.Service.Resolve<BarcodeController>();
            return ctl.GetBarcodes(this);
        }
        #endregion
    }
}
