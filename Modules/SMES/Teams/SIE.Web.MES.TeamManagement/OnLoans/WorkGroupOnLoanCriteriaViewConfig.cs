using SIE.MES.TeamManagement.OnLoans;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TeamManagement.OnLoans
{
    public class WorkGroupOnLoanCriteriaViewConfig : WebViewConfig<WorkGroupOnLoanCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(x => x.No);
                View.Property(x => x.Initiator).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                });
                View.Property(x => x.GroupIn).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WorkGroupController>().GetWorkGroups(pagingInfo, keyword);
                });
                View.Property(x => x.GroupOut).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WorkGroupController>().GetWorkGroups(pagingInfo, keyword);
                });
                View.Property(x => x.Approver).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                });
                View.Property(x => x.State);
            }
        }
    }
}
