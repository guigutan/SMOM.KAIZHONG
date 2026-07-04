using SIE.MES.PackingQC.Configs;
using SIE.MES.WIP.Pressure.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.PackingQC.Configs
{
    class PackingQCVerifyCodeConfigValueViewConfig : WebViewConfig<PackingQCVerifyCodeConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.VerifyCode).Show(ShowInWhere.All);
            }
        }
    }
}
