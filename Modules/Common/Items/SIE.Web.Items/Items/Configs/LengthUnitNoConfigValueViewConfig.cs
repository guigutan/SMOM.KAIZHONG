using SIE.Items.Items.Configs;

namespace SIE.Web.Items.Items.Configs
{
    /// <summary>
    /// 长度单位 视图配置
    /// </summary>
    public class LengthUnitNoConfigValueViewConfig : WebViewConfig<LengthUnitNoConfigValue>
    {
        /// <summary>
        /// 默认视图 配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.LengthTypeCode);
        }
    }
}
