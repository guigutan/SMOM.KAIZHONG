using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.Barcodes.ViewModels
{
    /// <summary>
    /// 条码归属 ViewModel
    /// </summary>
    [Serializable, RootEntity]
    [Label("条码归属")]
    public class BarcodeBelongViewModel : ViewModel
    {
        #region 条码号 Sn
        /// <summary>
        /// 条码号
        /// </summary>
        [Label("条码号")]
        public static readonly Property<string> SnProperty = P<BarcodeBelongViewModel>.Register(e => e.Sn);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<BarcodeBelongViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<BarcodeBelongViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 原工单 OrgWorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty OrgWorkOrderIdProperty = P<BarcodeBelongViewModel>.RegisterRefId(e => e.OrgWorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double OrgWorkOrderId
        {
            get { return (double)GetRefId(OrgWorkOrderIdProperty); }
            set { SetRefId(OrgWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> OrgWorkOrderProperty = P<BarcodeBelongViewModel>.RegisterRef(e => e.OrgWorkOrder, OrgWorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder OrgWorkOrder
        {
            get { return GetRefEntity(OrgWorkOrderProperty); }
            set { SetRefEntity(OrgWorkOrderProperty, value); }
        }
        #endregion

        #region 条码明细 Barcode
        /// <summary>
        /// 条码明细Id
        /// </summary>
        [Label("条码明细")]
        public static readonly IRefIdProperty BarcodeIdProperty = P<BarcodeBelongViewModel>.RegisterRefId(e => e.BarcodeId, ReferenceType.Normal);

        /// <summary>
        /// 条码明细Id
        /// </summary>
        public double BarcodeId
        {
            get { return (double)GetRefId(BarcodeIdProperty); }
            set { SetRefId(BarcodeIdProperty, value); }
        }

        /// <summary>
        /// 条码明细
        /// </summary>
        public static readonly RefEntityProperty<Barcode> BarcodeProperty = P<BarcodeBelongViewModel>.RegisterRef(e => e.Barcode, BarcodeIdProperty);

        /// <summary>
        /// 条码明细
        /// </summary>
        public Barcode Barcode
        {
            get { return GetRefEntity(BarcodeProperty); }
            set { SetRefEntity(BarcodeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 原工单号 OrgWorkOrderNo
        /// <summary>
        /// 原工单号
        /// </summary>
        [Label("原工单号")]
        public static readonly Property<string> OrgWorkOrderNoProperty = P<BarcodeBelongViewModel>.RegisterView(e => e.OrgWorkOrderNo, p => p.OrgWorkOrder.No);

        /// <summary>
        /// 原工单号
        /// </summary>
        public string OrgWorkOrderNo
        {
            get { return this.GetProperty(OrgWorkOrderNoProperty); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<BarcodeBelongViewModel>.RegisterView(e => e.PlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
        }
        #endregion

        #region 已打印数量 PrintedQty
        /// <summary>
        /// 已打印数量
        /// </summary>
        [Label("已打印数量")]
        public static readonly Property<int> PrintedQtyProperty = P<BarcodeBelongViewModel>.RegisterView(e => e.PrintedQty, p => p.WorkOrder.PrintedQty);

        /// <summary>
        /// 已打印数量
        /// </summary>
        public int PrintedQty
        {
            get { return this.GetProperty(PrintedQtyProperty); }
        }
        #endregion
        #endregion
    }
}
