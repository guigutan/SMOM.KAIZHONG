using SIE.Defects;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 缺陷分类视图配置
    /// </summary>
    internal class DefectCategoryViewConfig : WPFViewConfig<DefectCategory>
    {
        /// <summary>
        /// 配置选择列表视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
        }
    }
}
