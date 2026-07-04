using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.PanelBindings
{
    /// <summary>
    /// 拼板码与条码关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("拼板码与条码关系")]
    public partial class PanelAndBarcode : DataEntity
    {
        #region 拼板码 PanelCode
        /// <summary>
        /// 拼板码
        /// </summary>
        [Label("拼板码")]
        public static readonly Property<string> PanelCodeProperty = P<PanelAndBarcode>.Register(e => e.PanelCode);

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode
        {
            get { return GetProperty(PanelCodeProperty); }
            set { SetProperty(PanelCodeProperty, value); }
        }
        #endregion

        #region SN条码号 SN
        /// <summary>
        /// SN条码号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("SN条码号")]
        public static readonly Property<string> SNProperty = P<PanelAndBarcode>.Register(e => e.SN);

        /// <summary>
        /// SN条码号
        /// </summary>
        public string SN
        {
            get { return GetProperty(SNProperty); }
            set { SetProperty(SNProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<PanelAndBarcode>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 拼板数 PanelQty
        /// <summary>
        /// 拼板数
        /// </summary>
        [Label("拼板数")]
        public static readonly Property<decimal> PanelQtyProperty = P<PanelAndBarcode>.Register(e => e.PanelQty);

        /// <summary>
        /// 拼板数
        /// </summary>
        public decimal PanelQty
        {
            get { return GetProperty(PanelQtyProperty); }
            set { SetProperty(PanelQtyProperty, value); }
        }
        #endregion

        #region 叉板数 ForkPlateQty
        /// <summary>
        /// 叉板数
        /// </summary>
        [MinValue(0)]
        [Label("叉板数")]
        public static readonly Property<int> ForkPlateQtyProperty = P<PanelAndBarcode>.Register(e => e.ForkPlateQty);

        /// <summary>
        /// 叉板数
        /// </summary>
        public int ForkPlateQty
        {
            get { return GetProperty(ForkPlateQtyProperty); }
            set { SetProperty(ForkPlateQtyProperty, value); }
        }
        #endregion

        #region 绑定数量 BindingQty
        /// <summary>
        /// 绑定数量
        /// </summary>
        [Label("绑定数量")]
        public static readonly Property<decimal> BindingQtyProperty = P<PanelAndBarcode>.Register(e => e.BindingQty);

        /// <summary>
        /// 绑定数量
        /// </summary>
        public decimal BindingQty
        {
            get { return GetProperty(BindingQtyProperty); }
            set { SetProperty(BindingQtyProperty, value); }
        }
        #endregion

        #region 绑定时间 BindingDate
        /// <summary>
        /// 绑定时间
        /// </summary>
        [Label("绑定时间")]
        public static readonly Property<DateTime> BindingDateProperty = P<PanelAndBarcode>.Register(e => e.BindingDate);

        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime BindingDate
        {
            get { return GetProperty(BindingDateProperty); }
            set { SetProperty(BindingDateProperty, value); }
        }
        #endregion

        #region 是否已绑定 IsBinding
        /// <summary>
        /// 是否已绑定
        /// </summary>
        [Label("是否已绑定")]
        public static readonly Property<bool> IsBindingProperty = P<PanelAndBarcode>.Register(e => e.IsBinding);

        /// <summary>
        /// 是否已绑定
        /// </summary>
        public bool IsBinding
        {
            get { return GetProperty(IsBindingProperty); }
            set { SetProperty(IsBindingProperty, value); }
        }
        #endregion

        #region 拼板码 Panel
        /// <summary>
        /// 拼板码Id
        /// </summary>
        public static readonly IRefIdProperty PanelIdProperty = P<PanelAndBarcode>.RegisterRefId(e => e.PanelId, ReferenceType.Normal);

        /// <summary>
        /// 拼板码Id
        /// </summary>
        public double? PanelId
        {
            get { return (double?)GetRefNullableId(PanelIdProperty); }
            set { SetRefNullableId(PanelIdProperty, value); }
        }

        /// <summary>
        /// 拼板码
        /// </summary>
        public static readonly RefEntityProperty<Panel> PanelProperty = P<PanelAndBarcode>.RegisterRef(e => e.Panel, PanelIdProperty);

        /// <summary>
        /// 拼板码
        /// </summary>
        public Panel Panel
        {
            get { return GetRefEntity(PanelProperty); }
            set { SetRefEntity(PanelProperty, value); }
        }
        #endregion

        #region 操作人 Operator
        /// <summary>
        /// 操作人Id
        /// </summary>
        public static readonly IRefIdProperty OperatorIdProperty = P<PanelAndBarcode>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double? OperatorId
        {
            get { return (double?)GetRefNullableId(OperatorIdProperty); }
            set { SetRefNullableId(OperatorIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperatorProperty = P<PanelAndBarcode>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operator
        {
            get { return GetRefEntity(OperatorProperty); }
            set { SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<PanelAndBarcode>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<PanelAndBarcode>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码Id
        /// </summary>
        public static readonly IRefIdProperty BarcodeIdProperty = P<PanelAndBarcode>.RegisterRefId(e => e.BarcodeId, ReferenceType.Normal);

        /// <summary>
        /// 条码Id
        /// </summary>
        public double BarcodeId
        {
            get { return (double)GetRefId(BarcodeIdProperty); }
            set { SetRefId(BarcodeIdProperty, value); }
        }

        /// <summary>
        /// 条码
        /// </summary>
        public static readonly RefEntityProperty<Barcode> BarcodeProperty = P<PanelAndBarcode>.RegisterRef(e => e.Barcode, BarcodeIdProperty);

        /// <summary>
        /// 条码
        /// </summary>
        public Barcode Barcode
        {
            get { return GetRefEntity(BarcodeProperty); }
            set { SetRefEntity(BarcodeProperty, value); }
        }
        #endregion

        #region 板号 BoardNo
        /// <summary>
        /// 板号
        /// </summary>
        [Label("板号")]
        public static readonly Property<int> BoardNoProperty = P<PanelAndBarcode>.Register(e => e.BoardNo);

        /// <summary>
        /// 板号
        /// </summary>
        public int BoardNo
        {
            get { return this.GetProperty(BoardNoProperty); }
            set { this.SetProperty(BoardNoProperty, value); }
        }
        #endregion

        //#region 分板状态 SplitPanelStatus
        ///// <summary>
        ///// 分板状态
        ///// </summary>
        //[Label("分板状态")]
        //public static readonly Property<SplitPanelStatus?> SplitPanelStatusProperty = P<PanelAndBarcode>.Register(e => e.SplitPanelStatus);

        ///// <summary>
        ///// 分板状态
        ///// </summary>
        //public SplitPanelStatus? SplitPanelStatus
        //{
        //    get { return this.GetProperty(SplitPanelStatusProperty); }
        //    set { this.SetProperty(SplitPanelStatusProperty, value); }
        //}
        //#endregion

        #region 是否已分板 IsSplitPanel
        /// <summary>
        /// 是否已分板
        /// </summary>
        [Label("是否已分板")]
        public static readonly Property<bool> IsSplitPanelProperty = P<PanelAndBarcode>.RegisterReadOnly(
            e => e.IsSplitPanel, e => e.GetIsSplitPanel(), PanelAndBarcode.IsBindingProperty);

        /// <summary>
        /// 是否已分板
        /// </summary>

        public bool IsSplitPanel
        {
            get { return this.GetProperty(IsSplitPanelProperty); }
        }

        private bool GetIsSplitPanel()
        {
            if (IsBinding)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion


        #region 子产品工单 ChildWorkOrder
        /// <summary>
        /// 子产品工单Id
        /// </summary>
        [Label("子产品工单")]
        public static readonly IRefIdProperty ChildWorkOrderIdProperty =
            P<PanelAndBarcode>.RegisterRefId(e => e.ChildWorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 子产品工单Id
        /// </summary>
        public double? ChildWorkOrderId
        {
            get { return (double?)this.GetRefNullableId(ChildWorkOrderIdProperty); }
            set { this.SetRefNullableId(ChildWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 子产品工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> ChildWorkOrderProperty =
            P<PanelAndBarcode>.RegisterRef(e => e.ChildWorkOrder, ChildWorkOrderIdProperty);

        /// <summary>
        /// 子产品工单
        /// </summary>
        public WorkOrder ChildWorkOrder
        {
            get { return this.GetRefEntity(ChildWorkOrderProperty); }
            set { this.SetRefEntity(ChildWorkOrderProperty, value); }
        }
        #endregion

        #region 完成绑定 IsBindComplete
        /// <summary>
        /// 完成绑定
        /// </summary>
        [Label("完成绑定")]
        public static readonly Property<bool> IsBindCompleteProperty = P<PanelAndBarcode>.Register(e => e.IsBindComplete);

        /// <summary>
        /// 完成绑定
        /// </summary>
        public bool IsBindComplete
        {
            get { return this.GetProperty(IsBindCompleteProperty); }
            set { this.SetProperty(IsBindCompleteProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 子产品工单号 ChildWorkOrderNo
        /// <summary>
        /// 子产品工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> ChildWorkOrderNoProperty = P<PanelAndBarcode>.RegisterView(e => e.ChildWorkOrderNo, p => p.ChildWorkOrder.No);

        /// <summary>
        /// 子产品工单号
        /// </summary>
        public string ChildWorkOrderNo
        {
            get { return this.GetProperty(ChildWorkOrderNoProperty); }
        }
        #endregion

        #region 子产品编码 ChildProductCode
        /// <summary>
        /// 子产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ChildProductCodeProperty = P<PanelAndBarcode>.RegisterView(e => e.ChildProductCode, p => p.ChildWorkOrder.Product.Code);

        /// <summary>
        /// 子产品编码
        /// </summary>
        public string ChildProductCode
        {
            get { return this.GetProperty(ChildProductCodeProperty); }
        }
        #endregion

        #region 子产品名称 ChildProducName
        /// <summary>
        /// 子产品名称
        /// </summary>
        [Label("子产品名称")]
        public static readonly Property<string> ChildProducNameProperty = P<PanelAndBarcode>.RegisterView(e => e.ChildProducName, p => p.ChildWorkOrder.Product.Name);

        /// <summary>
        /// 子产品名称
        /// </summary>
        public string ChildProducName
        {
            get { return this.GetProperty(ChildProducNameProperty); }
        }
        #endregion

        #region 子产品ID ChildProductId
        /// <summary>
        /// 子产品ID
        /// </summary>
        [Label("子产品ID")]
        public static readonly Property<double> ChildProductIdProperty = P<PanelAndBarcode>.RegisterView(e => e.ChildProductId, p => p.ChildWorkOrder.ProductId);

        /// <summary>
        /// 子产品ID
        /// </summary>
        public double ChildProductId
        {
            get { return this.GetProperty(ChildProductIdProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 拼板码与条码关系 实体配置
    /// </summary>
    internal class PanelAndBarcodeConfig : EntityConfig<PanelAndBarcode>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_PANEL_BARCODE").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(PanelAndBarcode.PanelCodeProperty).ColumnMeta.HasIndex();
            Meta.Property(PanelAndBarcode.SNProperty).ColumnMeta.HasIndex();
            Meta.Property(PanelAndBarcode.WorkOrderIdProperty).ColumnMeta.HasIndex();
        }
    }
}