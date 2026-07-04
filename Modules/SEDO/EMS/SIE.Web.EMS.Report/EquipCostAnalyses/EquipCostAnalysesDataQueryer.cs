using SIE.EMS.Report.EquipCostAnalyses;
using SIE.EMS.Report.EquipmentMixReport;
using SIE.Web.Data;

namespace SIE.Web.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 综合报表查询器
    /// </summary>
    public class EquipCostAnalysesDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EquipCostAnalysesInfo GetReportData(EquipCostAnalyseCriteria criteria)
        {
           return RT.Service.Resolve<EquipCostAnalyseController>().CriteriaEquipCostAnalyse(criteria);
        }
    }
}
