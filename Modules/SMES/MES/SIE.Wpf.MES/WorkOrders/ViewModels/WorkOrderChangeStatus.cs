using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.WorkOrders.ViewModels
{
    /// <summary>
    /// 工单改变状态ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单状态改变")]
    [DisplayMember(nameof(Reason))]
    public class WorkOrderChangeStatus : ViewModel
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="workOrder">工单</param>
        public WorkOrderChangeStatus(WorkOrder workOrder)
        {
            WorkOrder = workOrder;
        }

        #region 工单 WorkOrder
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<WorkOrder> WorkOrderProperty = P<WorkOrderChangeStatus>.Register(e => e.WorkOrder);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetProperty(WorkOrderProperty); }
            set { this.SetProperty(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<string> StateProperty = P<WorkOrderChangeStatus>.RegisterReadOnly(
            e => e.State, e => e.GetState(), WorkOrderProperty);

        /// <summary>
        /// 工单状态
        /// </summary>
        public string State
        {
            get { return this.GetProperty(StateProperty); }
        }

        /// <summary>
        /// 获取工单状态
        /// </summary>
        /// <returns>工单状态</returns>
        private string GetState()
        {
            if (WorkOrder == null)
                return string.Empty;
            return WorkOrder.State.ToLabel();
        }
        #endregion

        #region 是否暂停 IsPause
        /// <summary>
        /// 是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<string> IsPauseProperty = P<WorkOrderChangeStatus>.RegisterReadOnly(
            e => e.IsPause, e => e.GetIsPause(), WorkOrderProperty);

        /// <summary>
        /// 是否暂停
        /// </summary>
        public string IsPause
        {
            get { return this.GetProperty(IsPauseProperty); }
        }

        /// <summary>
        /// 获取是否暂停
        /// </summary>
        /// <returns>是否暂停</returns>
        private string GetIsPause()
        {
            if (WorkOrder == null)
                return string.Empty;
            return WorkOrder.IsPause.ToLabel();
        }
        #endregion

        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        public static readonly Property<string> ReasonProperty = P<WorkOrderChangeStatus>.Register(e => e.Reason);

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单改变状态ViewModel视图配置
    /// </summary>
    internal class WorkOrderChangeStatusViewModelViewConfig : WPFViewConfig<WorkOrderChangeStatus>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrder.No).HasLabel("工单号").ShowInDetail();
                View.Property(p => p.IsPause).ShowInDetail();
                View.Property(p => p.State).ShowInDetail();
                View.Property(p => p.Reason).UseMemoEditor().ShowInDetail(rowSpan: 3);
            }
        }
    }
}
