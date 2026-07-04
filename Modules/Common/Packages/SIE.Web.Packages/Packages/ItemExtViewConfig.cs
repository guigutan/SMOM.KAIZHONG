using SIE.Items;
using SIE.Packages;

namespace SIE.Web.Packages.Packages
{
    /// <summary>
    /// Item扩展视图配置
    /// </summary>
    public class ItemExtViewConfig : WebViewConfig<Item>
    {
        /// <summary>
        /// 历史经验库物料视图
        /// </summary>
        public const string ExperienceItemView = "ExperienceItemView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup != ExperienceItemView)
                View.AssociateChildrenProperty(ItemPackageRuleDetailProperty.ItemPackageRuleListProperty, (e) =>
                {
                    var w = e.Parent as Item;
                    return RT.Service.Resolve<PackageController>().GetItemPackageRuleByItemId(w.Id);
                }, ItemPackageRuleViewConfig.ItemPackageRuleDetailView).HasLabel("物料包装规则").OrderNo = 25;
        }
    }
}
