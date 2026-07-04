using SIE.Kit.MES.CallMaterials.Configs;

namespace SIE.Web.Kit.MES.CallMaterials.Configs
{
    /// <summary>
    /// 叫料单号配置值视图配置
    /// </summary>
    internal class CallMaterialConfigValueViewConfig : WebViewConfig<CallMaterialConfigValue>
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