using SIE.Inventory.Strategy;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 上架规则
    /// </summary>
    internal class OnShelvesRuleCriteriaViewConfig : WebViewConfig<OnShelvesRuleCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.Warehouse).HasLabel("仓库").Show();
            View.Property(p => p.State).Show();
        }
    }
}
