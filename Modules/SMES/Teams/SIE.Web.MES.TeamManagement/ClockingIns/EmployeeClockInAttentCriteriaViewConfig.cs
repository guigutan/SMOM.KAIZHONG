using SIE.MES.TeamManagement.ClockingIns;
using SIE.Resources.Employees;
using System;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 人员工时统计查询视图配置
    /// </summary>
    public class EmployeeClockInAttentCriteriaViewConfig : WebViewConfig<EmployeeClockInAttentCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeCode).ShowInDetail();
                View.Property(p => p.EmployeeName).ShowInDetail();
                View.Property(p => p.WorkGroup).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WorkGroupController>().GetWorkGroups(pagingInfo, keyword);
                }).ShowInDetail();
                View.Property(p => p.EmployeeDate).UseDateEditor(p =>
                {
                    p.Format = "Y/m/d";
                    p.Value = DateTime.Now.ToString("yyyy/MM/dd");
                }).ShowInDetail();
                View.Property(p => p.OnDutyState).ShowInDetail();
                View.Property(p => p.ShiftRange).Show(ShowInWhere.Hide);
            }
        }
    }
}
