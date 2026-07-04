using SIE.MES.WorkOrders;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// ERP工单视图配置
    /// </summary>
    internal class ErpWorkOrderViewConfig : WebViewConfig<ErpWorkOrder>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.Property(p => p.No);
            View.Property(p => p.ErpSaleOrder).UsePagingLookUpEditor(p => p.BindDisplayField = ErpSaleOrder.NoProperty.Name).HasLabel("ERP订单");
            View.Property(p => p.ProductCode);
            View.Property(p => p.PlanBeginDate);
            View.Property(p => p.PlanEndDate);
            View.Property(p => p.PlanQty);
            View.Property(p => p.ActualFinishDate);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.ErpSaleOrder);
            View.Property(p => p.PlanBeginDate).UseDateRangeEditor();
            View.Property(p => p.PlanEndDate).UseDateRangeEditor();
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.No);
            View.Property(p => p.ErpSaleOrder);
            View.Property(p => p.ProductCode);
        }
    }
}
