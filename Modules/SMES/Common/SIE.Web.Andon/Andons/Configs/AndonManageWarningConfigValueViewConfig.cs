using SIE.Andon.Andons.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons.Configs
{
    public class AndonManageWarningConfigValueViewConfig : WebViewConfig<AndonManageWarningConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsWarning).Show();
            }
        }
    }
}
