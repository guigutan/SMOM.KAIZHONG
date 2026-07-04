using SIE.Packages.ItemLabels.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Packages.ItemLabels
{
    public class ItemLabelConfigValueViewConfig : WebViewConfig<ItemLabelConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsValidFactory).Show();
            }
        }
    }
}
