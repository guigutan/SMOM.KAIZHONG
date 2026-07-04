using SIE.Items;

namespace SIE.Wpf.Items.Items
{
    /// <summary>
    /// 物料更新日记 配置视图
    /// </summary>
    internal class ItemLogViewConfig : WPFViewConfig<ItemLog>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // 默认视图配置
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.OperatDate).Show(ShowInWhere.All);
                //View.Property(DataEntityExtension.UpdateByNameProperty).HasLabel("用户").Show(ShowInWhere.All);
                View.Property(p => p.OperatType).Show(ShowInWhere.All);
                View.Property(p => p.OperatDescription).Show(ShowInWhere.All);
            }
        }
    }
}
