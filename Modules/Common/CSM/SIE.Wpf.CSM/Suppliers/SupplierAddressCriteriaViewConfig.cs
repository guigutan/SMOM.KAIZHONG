using SIE.CSM.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.WPF.CSM.Suppliers
{
    internal class SupplierAddressCriteriaViewConfig : WPFViewConfig<SupplierAddressCriteria>
    {
        protected override void ConfigView()
        {
            View.DomainName("SupplierAddress");
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Name).HasLabel("简称").Show(ShowInWhere.All);
            }
           
        }
    }
}
