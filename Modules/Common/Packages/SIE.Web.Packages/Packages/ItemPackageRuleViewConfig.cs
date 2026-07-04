using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Packages;

namespace SIE.Web.Packages.Packages
{
    /// <summary>
    /// 物料包装规则视图配置
    /// </summary>
    public class ItemPackageRuleViewConfig : WebViewConfig<ItemPackageRule>
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
            View.UseCommands("SIE.Web.Packages.Packages.Commands.SelectPackageRuleCommand", "SIE.Web.Packages.Packages.Commands.PackageRuleDefaultCommand", WebCommandNames.Edit, WebCommandNames.Delete);
            //using (View.OrderProperties())
            //{
                View.Property(p => p.IsDefault).UseCheckEditor().Readonly().ShowInList(width: 70);
                View.Property(p => p.Code).Readonly().ShowInList(width: 200);
                View.Property(p => p.Name).Show();
                View.Property(p => p.Description).Show();
            //}

            View.ChildrenProperty(p => p.ItemPackageRuleDetailList).IsVisible = false;

            //子视图 - 主信息
            View.AssociateChildrenProperty(ItemPackageRule.ItemPackageRuleDetailListProperty, (e =>
            {
                var child = e as ChildPagingDataArgs;
                var itemPackageRule = child.Parent as ItemPackageRule;
                if (itemPackageRule == null) return new EntityList<ItemPackageRuleDetail>();
                return RT.Service.Resolve<PackageController>().GetItemPackageRuleDetails(itemPackageRule.Id, child.PagingInfo);
            }), ItemPackageRuleDetailViewConfig.ItemPackRuleDtlMasterView, false).HasLabel("主信息");


            //子视图 - 附加信息
            View.AssociateChildrenProperty(ItemPackageRule.ItemPackageRuleDetailListProperty, e =>
            {
                var child = e as ChildPagingDataArgs;
                var itemPackageRule = child.Parent as ItemPackageRule;
                if (itemPackageRule == null) return new EntityList<ItemPackageRuleDetail>();
                return RT.Service.Resolve<PackageController>().GetItemPackageRuleDetails(itemPackageRule.Id, child.PagingInfo);
            }, ItemPackageRuleDetailViewConfig.ItemPackRuleDtlAttachView, false).HasLabel("附加信息");
        }

        /// <summary>
        /// 配置默认视图（为了LookUpCommand写的视图）
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.PackageRuleCode).HasLabel("编码");
            View.Property(p => p.PackageRuleName).HasLabel("名称");
            View.Property(p => p.PackageRuleDescription).HasLabel("描述");
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
