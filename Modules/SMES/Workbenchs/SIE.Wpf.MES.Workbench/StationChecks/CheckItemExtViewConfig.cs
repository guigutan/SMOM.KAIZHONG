using SIE.Tech.Stations;

namespace SIE.Wpf.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位物料点检
    /// </summary>
    public class CheckItemExtViewConfig : WPFViewConfig<Station>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.AssociateChildrenProperty(StationExtCheckItemListProperty.StationCheckItemListProperty, (o) =>
            //{
            //    var station = o.Parent as Station;
            //    if (station == null) return new EntityList<CheckItem>();
            //    return RT.Service.Resolve<StationCheckController>().GetCheckItemList(station.Id, (o as ChildPagingDataArgs)?.PagingInfo);
            //}).HasLabel("物料点检").OrderNo = 35;
        }
    }
}
