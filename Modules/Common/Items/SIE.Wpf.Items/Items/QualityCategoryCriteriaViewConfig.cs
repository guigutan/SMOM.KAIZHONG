using SIE.Items;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 质量分类查询实体视图配置
    /// </summary>
    class QualityCategoryCriteriaViewConfig : WPFViewConfig<QualityCategoryCriteria>
    {
        /// <summary>
        /// 质量分类查询视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
            }
        }
    }
}
