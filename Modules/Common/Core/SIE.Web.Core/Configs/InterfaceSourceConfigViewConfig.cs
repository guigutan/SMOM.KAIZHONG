using SIE.Core.Configs;

namespace SIE.Web.Core.Configs
{
    /// <summary>
    ///  启用接口配置
    /// </summary>
    public class InterfaceSourceConfigValueViewConfig : WebViewConfig<InterfaceSourceConfigValue>
    {
        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.InterfaceSourceType).Show();
        }
    }
}
