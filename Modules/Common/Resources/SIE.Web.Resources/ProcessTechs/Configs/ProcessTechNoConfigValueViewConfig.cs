using SIE.Resources.ProcessTechs.Configs;

namespace SIE.Web.Resources.ProcessTechs.Configs
{
    /// <summary>
    /// 制程工艺编码视图配置
    /// </summary>
    internal class ProcessTechNoConfigValueViewConfig : WebViewConfig<ProcessTechNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.NumberRule).Show(ShowInWhere.All);
        }
    }
}