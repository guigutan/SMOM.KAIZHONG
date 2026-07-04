using SIE.Barcodes;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WoBarcodes
{
    /// <summary>
    /// 条码领用
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WoBarcodeRangeCriteria))]
    [Label("条码领用")]
    public partial class WoBarcodeRange : BarcodeRange
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static new readonly IRefIdProperty WorkOrderIdProperty =
            P<WoBarcodeRange>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public new double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static new readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<WoBarcodeRange>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public new WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion
    }
}
