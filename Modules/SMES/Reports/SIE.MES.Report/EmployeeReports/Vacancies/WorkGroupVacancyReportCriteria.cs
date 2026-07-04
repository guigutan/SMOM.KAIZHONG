using SIE.Domain;
using SIE.MES.TeamManagement.Vacancies;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.EmployeeReports.Vacancies
{
    /// <summary>
    /// 班组缺编查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("班组缺编查询")]
    public class WorkGroupVacancyReportCriteria : WorkGroupVacancyCriteria
    {
        /// <summary>
        /// 获取班组缺编信息
        /// </summary>
        /// <returns>班组缺编信息列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EmployeeReportController>().GetWorkGroupVacancy(this, PagingInfo);
        }
    }
}
