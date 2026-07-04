using SIE.Defects;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 缺陷分类视图配置
    /// </summary>
    internal class DefectGradeViewConfig : WPFViewConfig<DefectGrade>
    {
        /// <summary>
        /// 配置选择列表视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.DefectSeverity);
        }
    }
}
