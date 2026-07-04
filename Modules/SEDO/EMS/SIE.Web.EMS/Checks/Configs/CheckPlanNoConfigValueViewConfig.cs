using SIE.EMS.Checks.Configs;

namespace SIE.Web.EMS.Configs
{
    /// <summary>
    /// 点检单号配置值视图
    /// </summary>
    public class CheckPlanNoConfigValueViewConfig : WebViewConfig<CheckPlanNoConfigValue>
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
