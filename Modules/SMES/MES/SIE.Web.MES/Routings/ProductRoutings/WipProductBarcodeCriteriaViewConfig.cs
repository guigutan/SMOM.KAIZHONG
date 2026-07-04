using SIE.Barcodes;
using SIE.MES.WIP.Products;

namespace SIE.Web.MES.ProductRoutings
{
    /// <summary>
    /// 产品工艺路线条码查询视图
    /// </summary>
    public class WipProductBarcodeCriteriaViewConfig : WebViewConfig<WipProductBarcodeCriteria>
    {
        /// <summary>
        /// 产品工艺路线视图
        /// </summary>
        public const string ProductRoutingView = "ProductRoutingView";

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProductRoutingView);
            if (ViewGroup == ProductRoutingView)
                ConfigProductRoutingView();
        }

        /// <summary>
        /// 产品工艺路线 视图配置方法
        /// </summary>
        private void ConfigProductRoutingView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.Sn).Show(ShowInWhere.All);
            }
        }
    }
}