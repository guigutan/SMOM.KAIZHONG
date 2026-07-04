using SIE.Fixtures.InboundOrders;

namespace SIE.Web.Fixtures.InboundOrders
{
    /// <summary>
    /// 采购订单配置视图
    /// </summary>
    public class InboundOrderPurchaseViewConfig:WebViewConfig<InboundOrderPurchase>
    {
       /// <summary>
       /// 配置
       /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
        }

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            //View.UseDefaultCommands();
            View.Property(p => p.PoNo);
            View.Property(p => p.PoLine);
            View.Property(p => p.Qty);
            View.Property(p => p.Price);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }
    }
}
