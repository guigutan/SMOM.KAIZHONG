using DevExpress.XtraPrinting.BarCode;
using SIE.Items.Items;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Items.Items
{
    public class ItemCusotmerRelationViewConfig : WebViewConfig<ItemCusotmerRelation>
    {

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ItemCusotmerRelation));
            base.ConfigView();
        }
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.Customer).Show();
                View.Property(p => p.CodeAlias).Show();
                View.Property(p => p.SupplierCode).Show();
                View.Property(p => p.VersionNo).Show();
                View.Property(p => p.ProjectName).Show();
                View.Property(p => p.Drawing).Show();
                View.Property(p => p.Attribute1).Show();
                View.Property(p => p.Attribute2).Show();
                View.Property(p => p.Attribute3).Show();
                View.Property(p => p.Attribute4).Show();
                View.Property(p => p.Attribute5).Show();
                View.Property(p => p.Attribute6).Show();
                View.Property(p => p.Attribute7).Show();
                View.Property(p => p.Attribute8).Show();
                View.Property(p => p.Attribute9).Show();
                View.Property(p => p.Attribute10).Show();
            }
        }
    }
}
