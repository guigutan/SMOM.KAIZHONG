using SIE.Core.Algorithms.KZ;

namespace SIE.Web.Core.Algorithms
{
    /// <summary>
    /// 配置界面
    /// </summary>
    internal class VolkswagenSnConfigViewConfig : WebViewConfig<VolkswagenSnConfig>
    {
        /// <summary>
        /// 配置界面
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.VwCode);
            View.Property(p => p.CompanyCode);
            View.Property(p => p.LineCode);
            View.Property(p => p.StartValue).Show(ShowInWhere.Hide);
            View.Property(p => p.Step).Show(ShowInWhere.Hide);
        }
    }
}
