using SIE.Items.Items;

namespace SIE.Wpf.Items.Items
{
    /// <summary>
    /// 物料多选视图配置
    /// </summary>
    public class MultiQueryItemCriteriaViewConfig : WPFViewConfig<MultiQueryItemCriteria>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Code).ShowInDetail();
            View.Property(p => p.Name).ShowInDetail();
            View.Property(p => p.SpecificationModel).ShowInDetail();
            View.Property(p => p.Type).ShowInDetail();
            View.Property(p => p.ItemSourceType).ShowInDetail();
            View.Property(p => p.State).ShowInDetail();
            View.Property(p => p.SourceType).ShowInDetail();
        }
    }
}
