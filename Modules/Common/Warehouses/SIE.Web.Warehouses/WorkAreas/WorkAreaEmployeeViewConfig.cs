using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 工作区与员工关系视图配置
    /// </summary>
    internal class WorkAreaEmployeeViewConfig : WebViewConfig<WorkAreaEmployee>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(WorkAreaSelEmployeeCommand).FullName, WebCommandNames.Edit, typeof(WorkAreaEmployeeDeleteCommand).FullName);
            View.UseCommands(typeof(WorkAreaEmployeeOnDutyCommand).FullName, typeof(WorkAreaEmployeeOffDutyCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeCode).Readonly();
                View.Property(p => p.EmployeeName).Readonly();
                View.Property(p => p.EmployeeStatus).Readonly();
                View.Property(p => p.EmployeeGroupName).Readonly();
                View.Property(p => p.WorkGroupName).Readonly();
                View.Property(p => p.UserCode).Readonly();
                View.Property(p => p.UserState).Readonly();
                View.Property(p => p.WorkSituation).Readonly();
                View.Property(p => p.ShiftTypeId);
            }
        }
    }
}