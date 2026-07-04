using SIE.MES.DispoLookups;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DispoLookups
{
    public class DispoLookupViewConfig : WebViewConfig<DispoLookup>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls).UseImportCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Dispo).Show();
                View.Property(p => p.Fevor).Show();
                View.Property(p => p.MaterialDispo).Show();
                View.Property(p => p.Mtart).Show();
                View.Property(p => p.Remark).Show();
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Dispo).Show();
                View.Property(p => p.Fevor).Show();
                View.Property(p => p.MaterialDispo).Show();
                View.Property(p => p.Mtart).Show().UseEnumEditor();
                View.Property(p => p.Remark).Show();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Dispo).Show();
                View.Property(p => p.Fevor).Show();
                View.Property(p => p.MaterialDispo).Show();
                View.Property(p => p.Mtart).Show();
            }
        }
    }
}
