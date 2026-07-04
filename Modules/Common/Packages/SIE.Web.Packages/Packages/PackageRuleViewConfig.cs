using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Packages;
using SIE.Web.Packages.Packages.Commands;

namespace SIE.Web.Packages
{
    /// <summary>
    /// 包装规则视图配置
    /// </summary>
    internal class PackageRuleViewConfig : WebViewConfig<PackageRule>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands().ReplaceCommands(WebCommandNames.Add, "SIE.Web.Packages.Packages.Commands.PackageRuleAddCommand");
            View.ReplaceCommands(WebCommandNames.Save, typeof(PackageRuleSaveCommand).FullName);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New).ShowInList(150)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(150);
            View.Property(p => p.Description).ShowInList(150);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
            View.ChildrenProperty(p => p.PackageRuleDetailList).UseViewGroup(PackageRuleDetailViewConfig.DetailMainViewGroup).HasLabel("主信息");
            View.AssociateChildrenProperty(PackageRule.PackageRuleDetailListProperty, (e) =>
            {
                var arg = e as ChildPagingDataArgs;
                var packageRuleId = arg.Parent.GetId();
                return RT.Service.Resolve<PackageController>().GetPackageRuleDetails((double)packageRuleId, arg.SortInfo, arg.PagingInfo);
            }, PackageRuleDetailViewConfig.DetailSubViewGroup, true).Show(ChildShowInWhere.All).HasLabel("附加信息");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
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