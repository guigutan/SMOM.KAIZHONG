using SIE.Andon.AndonMonthReports;
using SIE.Andon.AndonStatisticsReports;
using SIE.Web.Data;

namespace SIE.Web.Andon.AndonMonthReports
{
    /// <summary>
    /// 查询器
    /// </summary>
    public class AndonMonthReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public AndonMonthReportInfos GetReportData(AndonMonthViewModelCriteria criteria)
        {
             return RT.Service.Resolve<AndonMonthReportController>().GetReportData(criteria);
        }
    }
}
