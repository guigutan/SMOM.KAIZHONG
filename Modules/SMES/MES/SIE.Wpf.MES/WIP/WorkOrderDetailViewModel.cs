using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Utils;
using System;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 工单明细
    /// </summary>
    [RootEntity, Serializable]
    public class WorkOrderDetailViewModel : ViewModel
    {

        #region WorkOrder 工单
        /// <summary>
        /// 工单ID
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<WorkOrderDetailViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<WorkOrderDetailViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单明细 视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class WorkOrderDetailViewModelViewConfig : WPFViewConfig<WorkOrderDetailViewModel>
    {
        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary> 
        public static readonly Property<string> WorkOrderStateProperty = P<WorkOrderDetailViewModel>.RegisterExtensionReadOnly("WorkOrderState", typeof(WorkOrderDetailViewModelViewConfig),
            GetWorkOrderState, WorkOrderDetailViewModel.WorkOrderIdProperty);

        /// <summary>
        /// 工单状态
        /// </summary>
        /// <returns>工单状态</returns>
        public static string GetWorkOrderState(WorkOrderDetailViewModel me)
        {
            return me.WorkOrder?.State.ToLabel();
        }
        #endregion

        #region 显示状态 DisplayState
        /// <summary>
        /// 显示状态  用一个字段显示工单状态和排产状态，工单状态有值时显示工单状态，排产状态有值时显示排产状态
        /// </summary>
        public static readonly Property<string> DisplayStateProperty
            = P<WorkOrderDetailViewModel>.RegisterExtensionReadOnly("DisplayState", typeof(WorkOrderDetailViewModelViewConfig),
            GetDisplayState, WorkOrderDetailViewModel.WorkOrderIdProperty);

        /// <summary>
        /// 显示状态
        /// </summary>
        /// <param name="me">工单</param>
        /// <returns>工单状态/排产状态</returns>
        public static string GetDisplayState(WorkOrderDetailViewModel me)
        {
            if (me.WorkOrder == null)
                return string.Empty;
            if (me.WorkOrder.IsPause == YesNo.Yes
                && (me.WorkOrder.State == SIE.Core.WorkOrders.WorkOrderState.Release
                    || me.WorkOrder.State == SIE.Core.WorkOrders.WorkOrderState.Producing))
            {
                return EnumViewModel.EnumToLabel(me.WorkOrder.State).L10N() + "暂停";
            }
            else
            {
                return EnumViewModel.EnumToLabel(me.WorkOrder.State).L10N();
            }
        }
        #endregion

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("工单明细");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseDetail(dialogHeight: 400, dialogWidth: 600);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("工单信息", detailColumnCount: 2))
                {
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号").ShowInDetail(columnSpan: 2, height: 0);
                }

                using (View.DeclareGroup("工单详情", detailColumnCount: 2))
                {
                    View.Property(p => p.WorkOrder.PlanQty).HasLabel("数量").ShowInDetail();
                    View.Property(DisplayStateProperty).HasLabel("工单状态").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.SaleOrderNo).HasLabel("销售订单").ShowInDetail();
                    View.Property(p => p.WorkOrder.WorkShop.Name).HasLabel("分配车间").ShowInDetail();
                    View.Property(p => p.WorkOrder.PlanBeginDate).HasLabel("计划开始时间").ShowInDetail();
                    View.Property(p => p.WorkOrder.PlanEndDate).HasLabel("计划完成时间").ShowInDetail();
                }
            }
        }
    }
}
