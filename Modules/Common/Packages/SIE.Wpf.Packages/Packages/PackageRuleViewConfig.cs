using SIE.Packages;
using SIE.Wpf.Packages.Packages.Commands;

namespace SIE.Wpf.Packages
{
    /// <summary>
    /// 包装规则视图配置
    /// </summary>
    internal class PackageRuleViewConfig : WPFViewConfig<PackageRule>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            View.InlineEdit().UseDefaultCommands();
            View.ReplaceCommands(WPFCommandNames.ListAdd, typeof(PackageRuleAddCommand));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.ChildrenProperty(p => p.PackageRuleDetailList).Visible(false);

            View.AttachChildrenProperty(typeof(PackageRuleDetail), (e =>
            {
                var packageRule = e.Parent as PackageRule;
                return packageRule.PackageRuleDetailList;
            }), PackageRuleDetailViewConfig.DetailMainViewGroup, true).HasLabel("主信息");
            View.AttachChildrenProperty(typeof(PackageRuleDetail), (e) =>
            {
                return (e.Parent as PackageRule).PackageRuleDetailList;
            }, PackageRuleDetailViewConfig.DetailSubViewGroup, true).HasLabel("附加信息");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 默认选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }
    }
}