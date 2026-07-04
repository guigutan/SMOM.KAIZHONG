using SIE.MES.StationStorages;
using SIE.Tech.Stations;

namespace SIE.Wpf.MES.StationStorages
{
    /// <summary>
    /// 工位库存查询实体视图配置
    /// </summary>
    internal class StationStorageCriteriaViewConfig : WPFViewConfig<StationStorageCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Station).UseDataSource((e, p, s) =>
            {
                return RT.Service.Resolve<StationController>().GetLoadItemStations(s, p);
            }).ShowInDetail();
            View.Property(p => p.WorkOrder).ShowInDetail();
        }
    }
}