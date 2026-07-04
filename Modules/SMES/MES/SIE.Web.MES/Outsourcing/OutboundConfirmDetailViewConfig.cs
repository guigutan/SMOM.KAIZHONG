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
    public class OutboundConfirmDetailViewConfig : WebViewConfig<OutboundConfirmDetail>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(OutboundConfirmDetail));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(typeof(OutboundConfirmDetailSaveCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.FlowNo).Show().Readonly();
                View.Property(p => p.OuterId).Show().Readonly();
                View.Property(p => p.InitiatorFactory).Show().Readonly();
                View.Property(p => p.OutFactory).Show().Readonly();
                View.Property(p => p.State).Show().Readonly();
                View.Property(p => p.Qty).Show().Readonly(p => p.State != OutboundConfirmDetailState.Return);
                View.Property(p => p.IsUpload).Show().Readonly();
                View.Property(p => p.OaMsg).ShowInList(width: 200).Readonly();
                View.Property(p => p.Zuid).Show().Readonly();
                View.ChildrenProperty(p => p.OutboundConfirmSnDetailList).Show(ChildShowInWhere.All);
            }
        }
    }
}
