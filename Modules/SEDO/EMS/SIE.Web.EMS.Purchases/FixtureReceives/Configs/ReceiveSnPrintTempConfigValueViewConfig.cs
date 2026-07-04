using SIE.EMS.Purchases.FixtureReceives.Configs;

namespace SIE.Web.EMS.Purchases.FixtureReceives.Configs
{
    /// <summary>
    /// 序列号编码规则配置视图配置
    /// </summary>
    internal class ReceiveSnPrintTempConfigValueViewConfig : WebViewConfig<ReceiveSnPrintTempConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.PrintTempId).Show(ShowInWhere.All).HasLabel("打印模板");
        }
    }
}
