using SIE.MES.Storages;
using SIE.Tech.Stations;
using SIE.Wpf.Command;
using SIE.Wpf.MES.Storages.Commands;

namespace SIE.Wpf.MES.Storages
{
    /// <summary>
    /// 产线工位货区视图配置
    /// </summary>
    internal class StationStorageAreaViewConfig : WPFViewConfig<StationStorageArea>
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
            View.UseDefaultCommands();
            View.RemoveCommands(WPFCommandNames.ListSave);
            View.ReplaceCommands(typeof(ListAddCommand), typeof(AddStationStorageAreaCommand));
            View.ReplaceCommands(typeof(ListEditCommand), typeof(EditStorageAreaExtCommand));
            View.Property(p => p.Station).UsePagingLookUpEditor(p => { p.DisplayMember = Station.CodeProperty.Name; }).UseDataSource((e, p, s) =>
            {
                return RT.Service.Resolve<StationController>().GetLoadItemStations(s, p);
            });
            View.Property(p => p.StationName).HasLabel("工位名称");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.StationCode).HasLabel("工位");
            View.Property(p => p.StationName).HasLabel("工位名称");
        }
    }
}