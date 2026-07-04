using SIE.Kit.MES.CallMaterials.Configs;

namespace SIE.Web.Kit.MES.CallMaterials.Configs
{
    /// <summary>
    /// 叫料单号配置值视图配置
    /// </summary>
    internal class CallMaterialMatchConfigValueViewConfig : WebViewConfig<CallMaterialMatchConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDetailColumnsCount(2);
            View.Property(p => p.MatchItemCode).UseCheckEditor().Show(ShowInWhere.All);
            View.Property(p => p.ExtentAttr).UseCheckEditor().Show(ShowInWhere.All);
        }
    }
}
