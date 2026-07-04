using SIE.EMS.Report.EquipmentMixReport;
using SIE.Web.Data;

namespace SIE.Web.EMS.Report.EquipmentMixReport
{
    /// <summary>
    /// 综合报表查询器
    /// </summary>
    public class EmsMixReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EmsMixReportInfo GetReportData(EquipmentMixReportMonViewModelCriteria criteria)
        {
           return RT.Service.Resolve<EquipmentMixReportController>().GetReportData(criteria);
        }
    }
}
