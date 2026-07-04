using SIE.Resources.Employees;
using SIE.Web.Resources.Employees.Commands;

namespace SIE.Web.Resources.Employees
{
    /// <summary>
    /// 员工与资源视图配置
    /// </summary>
    public class EmployeeResourceViewConfig : WebViewConfig<EmployeeResource>
    {

        /// <summary>
        /// 表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(EmployeeResourceDLTemplateCommand).FullName, typeof(EmployeeResourceImportCommand).FullName);
            View.UseCommands(typeof(SelectResourceCommand).FullName, "SIE.Web.Resources.Employees.Commands.EmployeeResourceDelCommand");
            View.Property(p => p.ResourceCode).HasLabel("编码");
            View.Property(p => p.ResourceName).HasLabel("名称");
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Resource);
            View.Property(p => p.ResourceName).HasLabel("资源名称");
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Resource);
            View.Property(p => p.ResourceName).HasLabel("资源名称");
        }
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Employee.Code).HasLabel("员工工号");
            View.PropertyRef(p => p.Resource.Code).HasLabel("资源编码");
        }
    }
}
