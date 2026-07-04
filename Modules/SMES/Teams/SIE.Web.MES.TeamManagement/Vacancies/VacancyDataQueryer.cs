using SIE.Core.Common.Models;
using SIE.MES.TeamManagement.Vacancies;
using SIE.Web.Data;

namespace SIE.Web.MES.TeamManagement.Vacancies
{
    /// <summary>
    /// 班组缺编查询器
    /// </summary>
    public class VacancyDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取班组缺编导出数据
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="criter">查询实体</param>
        /// <returns>班组缺编记录信息</returns>
        public ExportDataTable GetVacancyData(int year, int month, int day, WorkGroupVacancyCriteria criter)
        {
            return RT.Service.Resolve<VacancyController>().GetExportVacancys(year, month, day, criter);
        }
    }
}
