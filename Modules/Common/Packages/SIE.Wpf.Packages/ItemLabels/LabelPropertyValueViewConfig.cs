using SIE.Packages.ItemLabels;

namespace SIE.Wpf.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签视图配置
    /// </summary>
    internal class LabelPropertyValueViewConfig : WPFViewConfig<LabelPropertyValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.PropertyName).Show(ShowInWhere.All);
                View.Property(p => p.PropertyValue).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
