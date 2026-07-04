using SIE.MES.PackingPrints.ViewModels;


namespace SIE.Wpf.MES.PackingPrints.ViewModels
{
    /// <summary>
    /// 工单包装规则视图配置
    /// </summary>
    internal class PackageRuleDetailViewModelViewConfig : WPFViewConfig<PackageRuleDetailViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            //
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.PackageUnitName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.LevelQty).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Qty).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.NumberRuleName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.IsPrint).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.TemplateName).ShowInList().Readonly();
        }
    }
}
