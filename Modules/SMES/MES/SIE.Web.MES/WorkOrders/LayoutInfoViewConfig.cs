using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WorkOrders
{
    public class LayoutInfoViewConfig : WebViewConfig<LayoutInfo>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            base.ConfigView();
            if (ViewGroup == "EditListView")
            {
                ConfigEditListView();
            }
        }

        public void ConfigEditListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Vornr).Show();
                View.Property(p => p.ProcessCode).Show();
                View.Property(p => p.WorkCenterCode).Show();
                View.Property(p => p.Steus).Show();
                View.Property(p => p.ProcessQty).Show();
                View.Property(p => p.Zcode).Show();
                View.Property(p => p.Factory).Show();
                View.Property(p => p.Aufpl).Show();
                View.Property(p => p.Aplzl).Show();
                View.Property(p => p.Vgw01).Show();
                View.Property(p => p.Vgw02).Show();
                View.Property(p => p.Vgw03).Show();
            }
        }

        protected override void ConfigListView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Vornr).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.WorkCenterCode).Show().Readonly();
                View.Property(p => p.Steus).Show().Readonly();
                View.Property(p => p.ProcessQty).Show().Readonly();
                View.Property(p => p.Zcode).Show().Readonly();
                View.Property(p => p.Factory).Show().Readonly();
                View.Property(p => p.Aufpl).Show().Readonly();
                View.Property(p => p.Aplzl).Show().Readonly();
                View.Property(p => p.Vgw01).Show().Readonly();
                View.Property(p => p.Vgw02).Show().Readonly();
                View.Property(p => p.Vgw03).Show().Readonly();

            }
        }
    }
}
