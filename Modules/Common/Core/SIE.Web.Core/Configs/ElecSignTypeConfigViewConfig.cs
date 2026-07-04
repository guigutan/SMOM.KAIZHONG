using SIE.Core.Configs;

namespace SIE.Web.Core.Configs
{
    /// <summary>
    ///  签名方式配置项视图
    /// </summary>
    public class ElecSignTypeConfigViewConfig:WebViewConfig<ElecSignTypeConfigValue>
    {
        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.ElecSignTypeValue).Show();
        }
    }
}
