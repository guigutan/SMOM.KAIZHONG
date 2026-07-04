using SIE.Tech.Routings;
using SIE.Wpf.Tech.Routings.Commands;

namespace SIE.Wpf.Tech.Routings
{
    /// <summary>
    /// 关键物料视图配置
    /// </summary>
    internal class KeyItemViewConfig : WPFViewConfig<KeyItem>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.FormEdit();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors().UseCommands(typeof(AddKeyItemCommand), WPFCommandNames.ListDelete);
            View.Property(p => p.ItemCode).HasLabel("编码");
            View.Property(p => p.ItemName).HasLabel("名称");
            View.Property(p => p.ItemDescription).HasLabel("描述");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
