using SIE.Barcodes.Barcodes.Enums;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BarcodeCriteria))]
    [Label("条码")]
    [DisplayMember(nameof(Sn))]
    public partial class Barcode : Core.Barcodes.Barcode
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Barcode()
        {
            PrintedState = BarcodeState.Notprint;
            PrintTimes = 0;
        }

        #region 打印日期 PrintDate
        /// <summary>
        /// 打印日期
        /// </summary>
        [Label("打印日期")]
        public static readonly Property<DateTime?> PrintDateProperty = P<Barcode>.Register(e => e.PrintDate);

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
        public static readonly Property<int> PrintTimesProperty = P<Barcode>.Register(e => e.PrintTimes);

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
        public static readonly Property<BarcodeState> StateProperty = P<Barcode>.Register(e => e.PrintedState);

        /// <summary>
        /// 打印状态
        /// </summary>
        public BarcodeState PrintedState
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion  

        #region  条码范围 Range
        /// <summary>
        ///  条码范围Id
        /// </summary>
        [Label("条码范围")]
        public static readonly IRefIdProperty RangeIdProperty = P<Barcode>.RegisterRefId(e => e.RangeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<BarcodeRange> RangeProperty = P<Barcode>.RegisterRef(e => e.Range, RangeIdProperty);

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
        public static readonly IRefIdProperty PrintByIdProperty = P<Barcode>.RegisterRefId(e => e.PrintById, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> PrintByProperty = P<Barcode>.RegisterRef(e => e.PrintBy, PrintByIdProperty);

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
        public static readonly IRefIdProperty WorkOrderIdProperty = P<Barcode>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public override double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<Barcode>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 指派状态 AssignState
        /// <summary>
        /// 指派状态
        /// </summary>
        [Label("指派状态")]
        public static readonly Property<AssignState> AssignStateProperty = P<Barcode>.Register(e => e.AssignState);

        /// <summary>
        /// 指派状态
        /// </summary>
        public AssignState AssignState
        {
            get { return this.GetProperty(AssignStateProperty); }
            set { this.SetProperty(AssignStateProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 开始条码 StartSn
        /// <summary>
        /// 开始条码
        /// </summary>
        [Label("开始条码")]
        public static readonly Property<string> StartSnProperty = P<Barcode>.RegisterView(e => e.StartSn, p => p.Range.StartSn);

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
        public static readonly Property<string> EndSnProperty = P<Barcode>.RegisterView(e => e.EndSn, p => p.Range.EndSn);

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn
        {
            get { return this.GetProperty(EndSnProperty); }
        }
        #endregion 

        #region 工单号 WONo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WONoProperty = P<Barcode>.RegisterView(e => e.WONo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WONo
        {
            get { return this.GetProperty(WONoProperty); }
        }
        #endregion

        #region 打印人 PrinterName
        /// <summary>
        /// 打印人
        /// </summary>
        [Label("打印人")]
        public static readonly Property<string> PrinterNameProperty = P<Barcode>.RegisterView(e => e.PrinterName, p => p.PrintBy.Name);

        /// <summary>
        /// 打印人
        /// </summary>
        public string PrinterName
        {
            get { return this.GetProperty(PrinterNameProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<Barcode>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<Barcode>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 条码
    /// </summary>
    partial class Barcode
    {
        /// <summary>
        /// 完成标识，当扫码输入OK时，表示完成扫码
        /// </summary>
        public static readonly  string SubmitCode = "OK";
    }

    /// <summary>
    /// 条码 实体配置
    /// </summary>
    internal class BarcodeConfig : EntityConfig<Barcode>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BC_BARCODE").MapAllProperties();
            Meta.Property(Barcode.SnProperty).ColumnMeta.HasIndex();
            Meta.Property(Barcode.PrintDateProperty).ColumnMeta.HasIndex();
            Meta.Property(Barcode.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}