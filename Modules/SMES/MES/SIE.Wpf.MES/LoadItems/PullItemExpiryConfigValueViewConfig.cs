using SIE.MES.LoadItems;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 拉式物料扣料有效期配置值视图配置
    /// </summary>
    class PullItemExpiryConfigValueViewConfig : WPFViewConfig<PullItemExpiryConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Expiry).Show();
            }
        }
    }
}
