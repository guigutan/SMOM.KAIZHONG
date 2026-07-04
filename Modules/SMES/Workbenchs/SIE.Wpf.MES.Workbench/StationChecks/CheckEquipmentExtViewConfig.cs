using SIE.Tech.Stations;

namespace SIE.Wpf.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位的点检设备
    /// </summary>
    public class CheckEquipmentExtViewConfig : WPFViewConfig<Station>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            //View.AssociateChildrenProperty(StationExtCheckEquipmentListProperty.StationCheckEquipmentListProperty, (o) =>
            //{
            //    var station = o.Parent as Station;
            //    if (station == null) return new EntityList<CheckEquipment>();
            //    return RT.Service.Resolve<StationCheckController>().GetCheckEquipmentList(station.Id, (o as ChildPagingDataArgs)?.PagingInfo);
            //}).HasLabel("设备工器具点检").OrderNo = 35;
        }
    }
}
