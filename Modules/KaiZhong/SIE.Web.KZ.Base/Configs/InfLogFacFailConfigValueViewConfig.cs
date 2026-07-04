using SIE.KZ.Base.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Configs
{
    internal class InfLogFacFailConfigValueViewConfig : WebViewConfig<InfLogFacFailConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FailCount).Show();
            }
        }
    }
}
