using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 关键件属性值
    /// </summary>
    internal class KeyItemPropertyValueViewConfig : WPFViewConfig<KeyItemPropertyValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.DefinitionName).HasLabel("名称").Show(ShowInWhere.All);
                View.Property(p => p.Value).HasLabel("值").Show(ShowInWhere.All);
            }
        }
    }
}