using SIE.Items.Items;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Items.Items
{
    public class CustomFeatureRelViewConfig : WebViewConfig<CustomFeatureRel>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
                View.Property(p => p.Customer).Show();
                View.Property(p => p.Zqttx).Show();
            }
        }
    }
}
