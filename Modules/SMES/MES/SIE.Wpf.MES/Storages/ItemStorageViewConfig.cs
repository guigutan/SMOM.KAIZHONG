using SIE.MES.Storages;
using SIE.Wpf.MES.Storages.Commands;

namespace SIE.Wpf.MES.Storages
{
    /// <summary>
    /// 产线物料货位视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ItemStorageViewConfig : WPFViewConfig<ItemStorage>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(DeleteItemStorageCommand));
            View.Property(p => p.ItemId);
            View.Property(p => p.ItemName).HasLabel("物料名称");
            View.Property(p => p.Qty).Readonly();
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.ItemCode).HasLabel("物料");
            View.Property(p => p.ItemName).HasLabel("物料名称");
            View.Property(p => p.Qty).Readonly();
        }
    }
}