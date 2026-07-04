using SIE.MES.Outsourcing;
using SIE.MES.Outsourcing.Model;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing.Model
{
    public class UnProcessingSNViewMidelViewConfig:WebViewConfig<UnProcessingSNViewMidel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(OutsourcingRequest));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn).ShowInList(width: 180).Readonly();
                View.Property(p => p.Qty).Show().Readonly();
            }
        }
    }
}
