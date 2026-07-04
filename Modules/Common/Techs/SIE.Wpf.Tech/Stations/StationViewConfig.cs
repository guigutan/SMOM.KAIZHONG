using SIE.Tech.Stations;
using SIE.Wpf.Resources;

namespace SIE.Wpf.Tech.Stations
{
    /// <summary>
    /// 工位视图配置
    /// </summary>
    internal class StationViewConfig : WPFViewConfig<Station>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置默认视图
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseDefaultBehaviors();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ResourceId).UseEnterpriseEquipmentResourceEditor(); ////UseWipResourceEditor();
            View.ChildrenProperty(p => p.StationProcessList).Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Resource).UseEnterpriseEquipmentResourceEditor(); ////UseWipResourceEditor();
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Resource).UseEnterpriseEquipmentResourceEditor(); ////UseWipResourceEditor();
        }
    }
}