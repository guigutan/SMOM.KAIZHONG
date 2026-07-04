using SIE.CSM.Suppliers.Configs;

namespace SIE.Web.CSM.Suppliers.Configs
{
    /// <summary>
    /// 是否自动创建供应商用户 视图配置
    /// </summary>
    public class SupplierIsAutoCreateConfigValueViewConfig : WebViewConfig<SupplierIsAutoCreateConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("是否自动创建供应商用户");
            using (View.OrderProperties())
            {
                View.Property(p => p.IsAutoCreate).Show();
            }
        }
    }
}
