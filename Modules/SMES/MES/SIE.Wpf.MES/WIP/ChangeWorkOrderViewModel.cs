using SIE.Domain;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 切换在制工单
    /// </summary>
    [RootEntity, Serializable]
    public class ChangeWorkOrderViewModel : ViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workcell"></param>
        public ChangeWorkOrderViewModel(Workcell workcell)
        {
            Workcell = workcell;
        }

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ChangeWorkOrderViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<ChangeWorkOrderViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        /// <summary>
        /// 工作单元信息
        /// </summary>
        public Workcell Workcell { get; }
    }
}
