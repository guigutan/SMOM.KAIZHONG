using SIE.Barcodes;
using SIE.MES.WIP.Products;

namespace SIE.Web.MES.ProductRoutings
{
    /// <summary>
    /// 产品工艺路线 条码视图配置
    /// </summary>
    public class WipProductBarcodeViewConfig : WebViewConfig<WipProductBarcode>
    {
        /// <summary>
        /// 产品工艺路线 视图
        /// </summary>
        public static readonly string BarcodeViewGroup = "BarcodeViewGroup";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BarcodeViewGroup);
            if (ViewGroup == BarcodeViewGroup)
            {
                ConfigBarcodeView();
            }
        }

        /// <summary>
        /// 配置产品工艺路线视图
        /// </summary>
        void ConfigBarcodeView()
        {
            View.AssignAuthorize(typeof(WipProductRouting));
            using (View.OrderProperties())
            {
                View.Property(p => p.WONo).Readonly().ShowInList(130);
                View.Property(p => p.Sn).Readonly().ShowInList(130);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}