using SIE.MetaModel.View;
using SIE.Packages.Packages;

namespace SIE.WPF.Packages.Packages
{
    /// <summary>
    /// 复核包装规则明细视图配置
    /// </summary>
    internal class RePackageRuleDetailViewConfig : WebViewConfig<RePackageRuleDetail>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.Copy, WebCommandNames.Save);
            View.Property(p => p.BoxType);
            View.Property(p => p.ItemQty).DefaultValue(1);
            View.Property(p => p.LotQty).DefaultValue(1);
            View.Property(p => p.MixedType);
            View.Property(p => p.ItemCategoryLevelId).Readonly(p => p.MixedType != MixedType.NoAllow);
        }
    }
}