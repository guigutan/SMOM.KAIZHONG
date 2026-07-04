using SIE.MES.MtartProcessLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.MtartProcessLookups
{
    public class MtartProcessLookupCriteriaViewConfig : WebViewConfig<MtartProcessLookupCriteria>
    {
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ProcessId).Show();
            //View.Property(p => p.Mtart).Show();
            //View.Property(p => p.Dispo).Show();
            View.Property(p => p.ItemCategoryId).Show();
        }
    }
}
