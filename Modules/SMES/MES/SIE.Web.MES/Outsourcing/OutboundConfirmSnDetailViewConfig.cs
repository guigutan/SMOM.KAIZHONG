using DocumentFormat.OpenXml.Drawing.Charts;
using SIE.MES.Outsourcing;
using SIE.MetaModel.View;
using SIE.Web.MES.Outsourcing.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing
{
    public class OutboundConfirmSnDetailViewConfig : WebViewConfig<OutboundConfirmSnDetail>
    {
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(OutboundConfirmSnDetailAddCommand).FullName, typeof(OutboundConfirmSnDetailDeleteCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn).Show();
                View.Property(p => p.Qty).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show().Readonly();
            }
        }
    }
}
