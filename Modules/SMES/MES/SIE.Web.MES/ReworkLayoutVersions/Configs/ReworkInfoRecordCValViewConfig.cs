using SIE.MES.ReworkLayoutVersions.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ReworkLayoutVersions.Configs
{
    public class ReworkInfoRecordCValViewConfig : WebViewConfig<ReworkInfoRecordConfigValue>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.NumberRuleId).Show();
        }
    }
}
