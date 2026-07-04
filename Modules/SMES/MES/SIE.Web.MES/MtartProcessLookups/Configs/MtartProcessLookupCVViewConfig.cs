using SIE.MES.MtartProcessLookups.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.MtartProcessLookups.Configs
{
    public class MtartProcessLookupCVViewConfig : WebViewConfig<MtartProcessLookupConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.GenrateStartProcessBom).Show();
            }
        }
    }
}
