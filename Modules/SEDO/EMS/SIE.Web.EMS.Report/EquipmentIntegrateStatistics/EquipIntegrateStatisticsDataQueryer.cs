using SIE.EMS.Report.EquipmentIntegrateStatistics;
using SIE.Web.Data;

namespace SIE.Web.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// 设备综合统计查询器
    /// </summary>
    public class EquipIntegrateStatisticsDataQueryer : DataQueryer
    {
        /// <summary>
        /// 查询设备综合统计数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EquipStaticViewModel GetEquipmentIntegrateStatisticViewModels(
            EquipmentIntegrateStatisticCriteria criteria)
        {
            return RT.Service.Resolve<EquipmentIntegrateStatisticController>()
                .GetEquipmentIntegrateStatisticViewModels(criteria);
        }
    }
}
