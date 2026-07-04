using SIE.Packages.ItemLabels;

namespace SIE.Wpf.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签视图配置
    /// </summary>
    internal class ItemLabelViewConfig : WPFViewConfig<ItemLabel>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.Label);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.Specification);
            View.Property(p => p.ItemType);
           
            View.Property(p => p.SourceType);
            View.ChildrenProperty(p => p.PropertyValueList);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Label);
            View.Property(p => p.Item);
            View.Property(p => p.ItemType);
            View.Property(p => p.SourceType);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Label);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.ItemType);
            View.Property(p => p.Qty);
            View.Property(p => p.SourceType);
        }
    }
}
