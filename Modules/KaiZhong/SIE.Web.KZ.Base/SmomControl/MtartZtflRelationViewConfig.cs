using SIE.KZ.Base.SmomControl;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.SmomControl
{
    public class MtartZtflRelationViewConfig : WebViewConfig<MtartZtflRelation>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MtartZtflRelation));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.UseImportCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                //View.Property(p => p.Factory).Show();
                View.Property(p => p.Mtart).Show();
                View.Property(p => p.IsZtfl).ShowInList(width: 180);
                View.Property(p => p.IsUebto).ShowInList(width: 180);
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.Factory).Show();
                View.Property(p => p.Mtart).Show();
                View.Property(p => p.IsZtfl).Show();
                View.Property(p => p.IsUebto).Show();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.Factory).Show();
                View.Property(p => p.Mtart).Show();
            }
        }


    }
}
