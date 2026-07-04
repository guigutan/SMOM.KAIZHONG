using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.PanelBindings
{
    /// <summary>
    /// 条码领用
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WoPanelRangeCriteria))]
    [Label("拼板码领用")]
    public partial class WoPanelRange : PanelRange
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static new readonly IRefIdProperty WorkOrderIdProperty =
            P<WoPanelRange>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<WoPanelRange>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

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
