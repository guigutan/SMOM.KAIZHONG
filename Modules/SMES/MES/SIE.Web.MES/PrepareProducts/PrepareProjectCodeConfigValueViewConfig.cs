using SIE.MES.PrepareProducts.Configs;

namespace SIE.Web.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备项目维护配置项视图配置
    /// </summary>
    public class PrepareProjectCodeConfigValueViewConfig : WebViewConfig<PrepareProjectCodeConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProCodeRule).Show();
            }
        }
    }
}
