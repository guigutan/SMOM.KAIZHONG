using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    public class InfNcDataLogGroupCriteriaViewConfig : WebViewConfig<InfNcDataLogGroupCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).HasLabel("GUID");
                View.Property(p => p.InfType).ShowInList(width: 150);
                View.Property(p => p.InvOrg).ShowInList(width: 150);
                View.Property(p => p.FactoryName).ShowInList(width: 150);
                View.Property(p => p.CallResult);
                View.Property(p => p.BeginDate).UseDateRangeEditor();
                View.Property(p => p.SendState);
                View.Property(p => p.DataJsons);
                View.Property(p => p.SuccessJson);
                View.Property(p => p.ResponseContent);
                View.Property(p => p.ErrorMsg);
                View.Property(p => p.FactoryErrorMsg);
            }
        }
    }
}
