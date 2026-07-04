using SIE.Inventory.Strategy;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Strategy.Commands;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 任务分配规则员工视图配置
    /// </summary>
    public class TaskAllotRuleEmployeeViewConfig : WebViewConfig<TaskAllotRuleEmployee>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(SelectTaskAllotRuleEmployeeCommand).FullName, typeof(DeleteTaskAllotRuleEmployeeCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeCode);
                View.Property(p => p.EmployeeName);
                View.Property(p => p.EmployeeStatus);
                View.Property(p => p.EmployeeGroupName);
                View.Property(p => p.EmployeeWrokGroupName);
                View.Property(p => p.UserCode);
            }
        }
    }
}