using SIE.Items;
using SIE.MES.WorkOrders;
using System.ComponentModel;


namespace SIE.Wpf.MES.WorkOrders.ViewBehaviors
{
    /// <summary>
    /// 工单行为
    /// </summary>
    public class WorkOrderBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as DetailLogicalView;
            if (view != null)
            {
                view.CurrentChanged -= MainView_CurrentChanged;
                view.CurrentChanged += MainView_CurrentChanged;
            }
        }

        /// <summary>
        /// 当前工序BOM对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void MainView_CurrentChanged(object sender, System.EventArgs e)
        {
            DetailLogicalView logicalView = sender as DetailLogicalView;
            if (logicalView != null)
            {
                WorkOrder wo = logicalView.Current as WorkOrder;
                if (wo != null)
                {
                    wo.PropertyChanged -= WorkOrder_PropertyChanged;
                    wo.PropertyChanged += WorkOrder_PropertyChanged;
                    if (wo.ProductId > 0)
                    {
                        SetChildPanelVisible(wo.ProductId);
                    }
                }
            }
        }

        private void WorkOrder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == WorkOrder.ProductProperty.Name)
            {
                WorkOrder wo = sender as WorkOrder;
                if (wo != null && wo.ProductId > 0)
                {
                    SetChildPanelVisible(wo.ProductId);
                }
            }
        }

        /// <summary>
        /// 设置批次子列表只读
        /// </summary>
        /// <param name="productId">产品Id</param>
        private void SetChildPanelVisible(double productId)
        {
            var batch = RT.Service.Resolve<ItemController>().GetBatchRule(productId);
            if (batch == null) return;
            var child = View.GetChildView(typeof(Core.WorkOrders.WoWipBatch));
            if (child == null) return;
            if (batch.RetrospectType == Core.Items.RetrospectType.Batch)
            {
                child.IsReadOnly = MetaModel.ReadOnlyStatus.Dynamic;
            }
            else
            {
                child.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;
                var current = child.Current as Core.WorkOrders.WoWipBatch;
                if (current != null)
                    current.Qty = 1;
            }
        }
    }
}
