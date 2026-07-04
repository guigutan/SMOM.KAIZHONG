using SIE.Resources.Employees;
using SIE.Web.Resources.Employees.Commands;

namespace SIE.Web.Resources.Employees
{
    /// <summary>
    /// 员工与企业关系视图配置
    /// </summary>
    internal class EmployeeEnterpriseViewConfig : WebViewConfig<EmployeeEnterprise>
    {

        /// <summary>
        /// 表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(SelectEnterpriseCommand).FullName, typeof(EmployeeEnterpriseDelCommand).FullName);
            View.Property(p => p.EnterpriseCode).HasLabel("编码");
            View.Property(p => p.EnterpriseName).HasLabel("名称");
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Enterprise);
            View.Property(p => p.EnterpriseName).HasLabel("工厂名称");
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Enterprise);
            View.Property(p => p.EnterpriseName).HasLabel("工厂名称");
        }
    }
}
