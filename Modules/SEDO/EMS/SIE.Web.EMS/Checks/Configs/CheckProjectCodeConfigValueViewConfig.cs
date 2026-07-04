using SIE.EMS.Checks.Configs;

namespace SIE.Web.EMS.Configs
{
    /// <summary>
    /// 点检保养项目编码配置值视图
    /// </summary>
    public class CheckProjectCodeConfigValueViewConfig : WebViewConfig<CheckProjectCodeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRule);
        }
    }
}
