using SIE.Andon.AndonStatisticsReports;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.AndonStatisticsReports
{
    /// <summary>
    /// 查询器
    /// </summary>
    public class AndonStatisticsReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public AndonReportInfos GetReportData(AndonStatisticsViewModelCriteria criteria)
        {
             return RT.Service.Resolve<AndonStatisticsReportController>().GetReportData(criteria);
        }
    }
}
