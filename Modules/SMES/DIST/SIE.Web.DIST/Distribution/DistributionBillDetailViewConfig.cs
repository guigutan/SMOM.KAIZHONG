using SIE.DIST;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 箱号视图配置
    /// </summary>
    internal class DistributionBillDetailViewConfig : WebViewConfig<DistributionBillDetail>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemLabelNo).Show(ShowInWhere.All);
                View.Property(p => p.Qty).Show(ShowInWhere.All);
                View.Property(p => p.OkQty).Show(ShowInWhere.All);
                View.Property(p => p.NgQty).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.PropertyList).Show(ChildShowInWhere.List);
            }
        }
    }
}
