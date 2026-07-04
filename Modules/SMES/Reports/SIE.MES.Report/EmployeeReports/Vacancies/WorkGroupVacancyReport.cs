using SIE.MES.TeamManagement.Vacancies;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.EmployeeReports.Vacancies
{
    /// <summary>
    /// 班组缺编统计表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkGroupVacancyReportCriteria))]
    [Label("班组缺编统计表")]
    public class WorkGroupVacancyReport : WorkGroupVacancy
    {
    }
}
