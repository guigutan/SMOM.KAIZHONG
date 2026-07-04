using SIE.Resources.Employees;

namespace SIE.Web.Resources.Employees
{
    /// <summary>
    /// 工厂扩展选择视图
    /// </summary>
    internal class EmployeeEnterpriseCriteriaViewConfig : WebViewConfig<EmployeeEnterpriseSelectCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
            }
        }
    }
}