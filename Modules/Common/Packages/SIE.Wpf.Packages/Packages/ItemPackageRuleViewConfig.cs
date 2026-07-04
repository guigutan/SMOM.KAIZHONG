using SIE.Packages;
using SIE.Wpf.Behaviors;
using SIE.Wpf.Packages.Commands;

namespace SIE.Wpf.Packages
{
    /// <summary>
    /// 物料包装规则视图配置
    /// </summary>
    internal class ItemPackageRuleViewConfig : WPFViewConfig<ItemPackageRule>
    {
        /// <summary>
        /// 物料包装规则视图ViewGroup
        /// </summary>
        public const string ItemPackageRuleDetailView = "ItemPackageRuleDetailView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ItemPackageRuleDetailView);

            if (ViewGroup == ItemPackageRuleDetailView)
            {
                ItemPackageRuleDetail();
            }
        }

        /// <summary>
        /// 列表显示的视图
        /// </summary>
        protected void ItemPackageRuleDetail()
        {
            View.UseChildrenAsHorizontal(true);
            View.InlineEdit();
            View.AddBehavior(typeof(GridRowDoubleClickViewBehavior));
            View.UseCommands(typeof(SelectPackageRuleCommand), typeof(PackageRuleDefaultCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.IsDefault).UseCheckEditor().Readonly().Show().UseListSetting(w => w.ListGridWidth = 70);
                View.Property(p => p.Code).Readonly().Show().UseListSetting(w => w.ListGridWidth = 200);
                View.Property(p => p.Name).Show();
                View.Property(p => p.Description).Show();
            }

            View.ChildrenProperty(p => p.ItemPackageRuleDetailList).IsVisible = false;
            ////子视图 - 主信息
            View.AssociateChildrenProperty(ItemPackageRule.ItemPackageRuleDetailListProperty, (e =>
            {
                var itemPackageRule = e.Parent as ItemPackageRule;
                return itemPackageRule.ItemPackageRuleDetailList;
            }), ItemPackageRuleDetailViewConfig.ItemPackRuleDtlMasterView, false).HasLabel("主信息");

            //子视图 - 附加信息
            View.AssociateChildrenProperty(ItemPackageRule.ItemPackageRuleDetailListProperty, e =>
            {
                var itemPackageRule = e.Parent as ItemPackageRule;
                return itemPackageRule.ItemPackageRuleDetailList;
            }, ItemPackageRuleDetailViewConfig.ItemPackRuleDtlAttachView, false).HasLabel("附加信息");
        }

        /// <summary>
        /// 配置默认视图（为了LookUpCommand写的视图）
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.PackageRule.Code).HasLabel("编码");
            View.Property(p => p.PackageRule.Name).HasLabel("名称");
            View.Property(p => p.PackageRule.Description).HasLabel("描述");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageRuleCode).Show();
                View.Property(p => p.PackageRuleName).HasLabel("名称").Show();
                View.Property(p => p.PackageRuleDescription).HasLabel("描述").Show();
            }
        }
    }
}
