using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    public class InfNcDataLogFactoryCriteriaViewConfig : WebViewConfig<InfNcDataLogFactoryCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InvOrg);
                View.Property(p => p.InfType).ShowInList(width: 150);
                View.Property(p => p.FactoryName).ShowInList(width: 150);
                View.Property(p => p.SendState);
                View.Property(p => p.GroupGuid);
                View.Property(p => p.BatchNo).HasLabel("唯一码");
                View.Property(p => p.DataJsons);
                View.Property(p => p.ErrorMsg);
            }
        }
    }
}
