using SIE.EMS.Common.Configs;

namespace SIE.Web.EMS.Common
{
    /// <summary>
    /// 配置值视图
    /// </summary>
    public class IsWmsControlConfigValueViewConfig : WebViewConfig<IsWmsControlConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsWmsControl).HasLabel("启用WMS管控");
        }
    }
}
