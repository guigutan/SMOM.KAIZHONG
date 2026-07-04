using SIE.Inventory.Strategy;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 分配规则查询视图
    /// </summary>
    internal class AssignRuleCriteriaViewConfig : WebViewConfig<AssignRuleCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.State).Show();
        }
    }
}