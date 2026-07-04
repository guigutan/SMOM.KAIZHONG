using SIE.Items.Items.Configs;

namespace SIE.Web.Items.Items.Configs
{
    /// <summary>
    /// 重量单位 视图配置
    /// </summary>
    public class WeightUnitNoConfigValueViewConfig : WebViewConfig<WeightUnitNoConfigValue>
    {
        /// <summary>
        /// 默认视图 配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.WeightTypeCode);
        }
    }
}
