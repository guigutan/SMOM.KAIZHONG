using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using SIE.Resources.Employees;
using System;

namespace SIE.Web.MES.TeamManagement.Vacancies
{
    /// <summary>
    /// 班组缺编查询块视图
    /// </summary>
    public class WorkGroupVacancyCriteriaViewConfig : WebViewConfig<WorkGroupVacancyCriteria>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkGroup).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WorkGroupController>().GetWorkGroups(pagingInfo, keyword);
                }).HasLabel("班组".L10N() + "*").ShowInDetail();
                View.Property(p => p.ClockInDate).UseDateEditor(p =>
                {
                    p.Format = "Y/m/d";
                    p.Value = RF.Find<EmployeeClockIn>().GetDbTime().ToString("yyyy/MM/dd");
                }).HasLabel("日期".L10N()+"*").ShowInDetail();
            }
        }
    }
}
