using SIE.MES.LoadItems;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 上料物料属性值视图配置
    /// </summary>
    internal class LoadItemPropertyValueViewConfig : WPFViewConfig<LoadItemPropertyValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
		protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.PropertyName).HasLabel("属性名称").Show(ShowInWhere.All);
                View.Property(p => p.Value).HasLabel("属性值").Show(ShowInWhere.All);
            }
        }
    }
}
