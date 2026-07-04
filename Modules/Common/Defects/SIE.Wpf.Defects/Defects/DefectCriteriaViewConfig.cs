using SIE.Defects.Defects;

namespace SIE.Wpf.Defects
{
    /// <summary>
    /// 缺陷代码视图配置
    /// </summary>
    internal class DefectCriteriaViewConfig : WPFViewConfig<DefectCriteria>
    {
        private const string TYPE_CODE = "分类编码";


        /// <summary>
        /// 查询面板视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Description);
            View.Property(p => p.DefectCategory).HasLabel(TYPE_CODE);
            View.Property(p => p.QualityType);
            View.Property(p => p.DefectGrade);
        }
    }
}