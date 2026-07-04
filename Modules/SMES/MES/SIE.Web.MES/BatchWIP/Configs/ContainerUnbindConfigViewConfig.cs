using SIE.MES.BatchWIP.Configs;

namespace SIE.Web.MES.BatchWIP.Configs
{
    /// <summary>
    /// 批次采集载具解绑方式视图配置
    /// </summary>
    public class ContainerUnbindConfigViewConfig : WebViewConfig<ContainerUnbindConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.UnbindMode).UseEnumEditor().Show(ShowInWhere.All);
        }
    }
}