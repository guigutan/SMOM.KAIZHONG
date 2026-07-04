using SIE.MES.TaskManagement.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Configs
{
    public class ItemTypeConfigValueViewConfig : WebViewConfig<ItemTypeConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessNumberRuleId).Show();
                View.Property(p => p.ProcessPrintTemplateId).Show();
            }
        }
    }
}
