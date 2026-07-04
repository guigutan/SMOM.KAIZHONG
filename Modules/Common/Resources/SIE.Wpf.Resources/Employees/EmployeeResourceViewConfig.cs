using SIE.Resources.Employees;
using SIE.Wpf.Resources.Employees.Commands;

namespace SIE.Wpf.Resources.Employees
{
    /// <summary>
    /// 员工与资源视图配置
    /// </summary>
    internal class EmployeeResourceViewConfig : WPFViewConfig<EmployeeResource>
    {

        /// <summary>
        /// 表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseCommands(typeof(EmployeeResourceLookupCommand)).UseCommands(WPFCommandNames.ListDelete); ////LookupCommand
            View.Property(p => p.Resource.Code).HasLabel("编码");
            View.Property(p => p.Resource.Name).HasLabel("名称");
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Resource);
            View.Property(p => p.Resource.Name).HasLabel("资源名称");
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Resource);
            View.Property(p => p.Resource.Name).HasLabel("资源名称");
        }
    }
}
