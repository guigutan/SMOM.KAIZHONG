using SIE.CSM.Customers;

namespace SIE.WPF.CSM.Customers
{
    internal class CustomerCriteriaViewConfig : WPFViewConfig<CustomerCriteria>
    {
        protected override void ConfigView()
        {
            View.DomainName("客户查询");

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
                View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
            }
        }
    }
}
