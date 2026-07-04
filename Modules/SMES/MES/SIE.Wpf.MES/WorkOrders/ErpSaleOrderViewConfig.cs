using SIE.MES.WorkOrders;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// ERP订单视图配置
    /// </summary>
    internal class ErpSaleOrderViewConfig : WPFViewConfig<ErpSaleOrder>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            View.InlineEdit().UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.No);
            View.Property(p => p.CustomerName).HasLabel("客户名称");
            View.Property(p => p.OrderDate);
            View.Property(p => p.DeliveryDate);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.CustomerName).HasLabel("客户名称");
            View.Property(p => p.OrderDate).UseDateRangeEditor();
            View.Property(p => p.DeliveryDate).UseDateRangeEditor();
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.No);
            View.Property(p => p.CustomerName).HasLabel("客户名称");
            View.Property(p => p.OrderDate);
            View.Property(p => p.DeliveryDate);
        }
    }
}
