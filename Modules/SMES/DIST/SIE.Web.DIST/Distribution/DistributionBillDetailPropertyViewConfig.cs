using SIE.DIST;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 配送单明细视图配置
    /// </summary>
    internal class DistributionBillDetailPropertyViewConfig : WebViewConfig<DistributionBillDetailProperty>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
		protected override void ConfigView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.DefinitionName).HasLabel("属性名称").Show(ShowInWhere.All);
                View.Property(p => p.Value).Show(ShowInWhere.All);
            }
        }
    }
}