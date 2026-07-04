using SIE.Items.Items;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Items.Items
{
    public class ParentItemViewConfig : WebViewConfig<ParentItem>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.ParentItemCode).Show();
                View.Property(p => p.Bismt).Show();
                View.Property(p => p.Mtart).Show();
                View.Property(p => p.Werks).Show();
            }
        }
    }
}
