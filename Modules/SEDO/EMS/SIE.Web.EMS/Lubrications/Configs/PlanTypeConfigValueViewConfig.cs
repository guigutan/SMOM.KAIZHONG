using SIE.EMS.Lubrications.Configs;

namespace SIE.Web.EMS.Lubrications.Configs
{
    /// <summary>
    /// 计划规则配置视图
    /// </summary>
    public class PlanTypeConfigValueViewConfig : WebViewConfig<PlanTypeConfigValue>
    {
        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.PlanType);
        }
    }
}
