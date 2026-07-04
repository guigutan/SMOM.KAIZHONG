using SIE.MES.ReworkLayoutVersions;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ReworkLayoutVersions
{
    public class ReworkInfoRecordDtlViewConfig : WebViewConfig<ReworkInfoRecordDtl>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ReworkInfoRecordDtl));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn).ShowInList(width: 150).Readonly();
                View.Property(p => p.WipBatchQty).Show().Readonly();
                View.Property(p => p.WorkOrderNo).ShowInList(width: 150).Readonly();
            }
        }
    }
}
