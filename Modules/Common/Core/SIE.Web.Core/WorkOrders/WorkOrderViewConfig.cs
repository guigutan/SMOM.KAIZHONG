using SIE.Core.WorkOrders;

namespace SIE.Web.Core.WorkOrders
{
    internal class WorkOrderViewConfig : WebViewConfig<WorkOrder>
    {
        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.UseClientOrder();
                View.Property(p => p.No).Show();
                View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Show();
                View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").Show();
                View.Property(p => p.PlanQty).Show();
                View.Property(p => p.PlanBeginDate).Show();
                View.Property(p => p.PlanEndDate).Show();
            }
        }
    }
}
