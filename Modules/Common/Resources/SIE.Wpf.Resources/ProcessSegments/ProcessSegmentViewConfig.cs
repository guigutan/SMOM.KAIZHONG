using SIE.Resources.ProcessSegments;

namespace SIE.Wpf.Resources.ProcessSegments
{
    /// <summary>
    /// 工段视图配置
    /// </summary>
    internal class ProcessSegmentViewConfig : WPFViewConfig<ProcessSegment>
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
            View.InlineEdit();
            View.UseDefaultCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.IsSplit);
            View.Property(p => p.ReceiveMaterialSortIndex);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.IsSplit);
        }
    }
}
