using SIE.MES.ReworkLayoutVersions;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ReworkLayoutVersions
{
    public class ReworkLayoutViewConfig : WebViewConfig<ReworkLayout>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Vornr).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.WorkCenterCode).Show().Readonly();
                View.Property(p => p.Steus).Show().Readonly();
                View.Property(p => p.Factory).Show().Readonly();
                View.Property(p => p.ProcessQty).Show().Readonly();
                View.Property(p => p.Zcode).Show().Readonly();
            }
        }
    }
}
