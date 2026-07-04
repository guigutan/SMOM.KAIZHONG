using SIE.Tech.Stations;
using SIE.Wpf.Tech.Stations.Commands;

namespace SIE.Wpf.Tech.Stations
{
    /// <summary>
    /// 工位物料视图配置
    /// </summary>
    internal class StationItemViewConfig : WPFViewConfig<StationItem>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands();
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete,
                typeof(ImportStationItemCommand), WPFCommandNames.Export);
            View.Property(p => p.Item).HasLabel("物料编码");
            View.Property(p => p.ItemName).HasLabel("物料名称").Readonly();
            View.Property(p => p.Warning).UseSpinEditor(e => { e.MinValue = 1; });
            View.Property(p => p.Capacity).UseSpinEditor(e => { e.MinValue = 1; });
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置视图
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置视图
        }
    }
}