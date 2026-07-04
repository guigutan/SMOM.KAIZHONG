using SIE.CSM.Customers;

namespace SIE.Web.CSM.Customers
{
    /// <summary>
    /// 客户视图配置
    /// </summary>
    internal class CustomerCriteriaViewConfig : WebViewConfig<CustomerCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.CustomerType).Show();
            }
        }
    }
}
