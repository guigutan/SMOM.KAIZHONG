using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    public class InfNcDataLogCriteriaViewConfig : WebViewConfig<InfNcDataLogCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InfType).ShowInList(width: 150);
                View.Property(p => p.InfCode).ShowInList(width: 150);
                View.Property(p => p.OperationType).ShowInList(width: 150);
                View.Property(p => p.CallResult);
                View.Property(p => p.DataJsons);
                View.Property(p => p.ErrorMsg);
                View.Property(p => p.GroupGuid);
            }
        }
    }
}
