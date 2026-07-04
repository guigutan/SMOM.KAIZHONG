using Amazon.Runtime.Internal.Auth;
using SIE.MES.WorkOrders.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WorkOrders.Configs
{
    public class ProcessBomWeightConValViewConfig : WebViewConfig<ProcessBomWeightConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Scope).Show();
            }
        }
    }
}
