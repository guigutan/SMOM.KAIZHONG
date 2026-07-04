using SIE.Barcodes;

namespace SIE.Web.MES.PanelBindings
{
    /// <summary>
    /// 条码绑定 未绑定条码视图配置
    /// </summary>
    public class BarcodeViewConfig : WebViewConfig<Barcode>
    {
        /// <summary>
        /// MES工单条码绑定记录-未绑定记录
        /// </summary>
        public const string NoBindingView = "NoBindingView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(NoBindingView);
            if (ViewGroup == NoBindingView)
            {
                ConfigNoBindingView();
            }
        }

        /// <summary>
        /// MES工单条码绑定记录-未绑定记录
        /// </summary>
        void ConfigNoBindingView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn).ShowInList(width: 150).Readonly();
                View.Property(p => p.WONo).Readonly().ShowInList(width: 200);
                View.Property(p => p.ProductCode).Readonly().ShowInList(width: 150);
                View.Property(p => p.Qty).Readonly().ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}