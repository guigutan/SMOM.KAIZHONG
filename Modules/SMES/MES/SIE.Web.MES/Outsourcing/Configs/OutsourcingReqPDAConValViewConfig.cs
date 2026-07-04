using SIE.MES.Outsourcing.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing.Configs
{
    public class OutsourcingReqPDAConValViewConfig : WebViewConfig<OutsourcingRequestPDAConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ConfirmDay).Show();
            }
        }
    }
}
