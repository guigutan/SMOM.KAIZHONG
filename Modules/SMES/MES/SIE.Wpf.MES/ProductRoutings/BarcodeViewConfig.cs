using SIE.Barcodes;
using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.ProductRoutings
{
    /// <summary>
    /// 产品工艺路线 条码视图配置
    /// </summary>
    internal class BarcodeViewConfig : WPFViewConfig<Barcode>
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
                ConfigBarcodeView();
        }

        /// <summary>
        /// 配置产品工艺路线视图
        /// </summary>
        void ConfigBarcodeView()
        {
            View.AssignAuthorize(typeof(WipProductRouting));
            using (View.OrderProperties())
            {
                View.Property(p => p.WONo).ShowInList();
                View.Property(p => p.Sn).ShowInList();
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}