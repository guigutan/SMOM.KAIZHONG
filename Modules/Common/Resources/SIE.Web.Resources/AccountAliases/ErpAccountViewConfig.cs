using SIE.Resources;

namespace SIE.Web.Resources
{
    internal class ErpAccountViewConfig : WebViewConfig<ErpAccount>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ErpAccount.NameProperty);
            SelectListView();
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected void SelectListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.EffectiveDate).Show();
                View.Property(p => p.DisableDate).Show();
            }
        }
    }
}
