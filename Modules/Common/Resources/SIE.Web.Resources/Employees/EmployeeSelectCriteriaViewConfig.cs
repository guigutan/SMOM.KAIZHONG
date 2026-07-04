using SIE.Resources.Employees;

namespace SIE.Web.Resources.Employees
{
    /// <summary>
    /// 工厂扩展选择视图
    /// </summary>
    internal class EmployeeSelectCriteriaViewConfig : WebViewConfig<EmployeeSelectCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("账号").Show();
                //View.Property(p => p.Name).Show().Visibility();
                //View.Property(p => p.HireDate).UseDateEditor().Show();
                View.Property(p => p.User).Show();
               // View.Property(p => p.WorkGroup).Show();
                View.Property(p => p.EmployeeStatus).Show();
                //View.Property(p => p.Sex).Show();
            }
        }
    }
}