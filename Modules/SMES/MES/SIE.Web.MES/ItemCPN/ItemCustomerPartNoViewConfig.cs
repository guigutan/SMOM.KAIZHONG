using DevExpress.DataProcessing;
using SIE.MES.ItemCPN;
using SIE.MetaModel.View;
using SIE.Web.MES.LineAndon.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemCPN
{
    public class ItemCustomerPartNoViewConfig : WebViewConfig<ItemCustomerPartNo>
    {
        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                View.Property(p => p.Customer).ShowInList(width: 150);
                View.Property(p => p.CodeAlias).ShowInList(width: 150);
                View.Property(p => p.SupplierCode).ShowInList(width: 150);
                View.Property(p => p.VersionNo).ShowInList(width: 150);
                View.Property(p => p.ProjectName).ShowInList(width: 150);
                View.Property(p => p.Drawing).ShowInList(width: 150);
                View.Property(p => p.BatchNo).ShowInList(width: 150);
                View.Property(p => p.LineCode).ShowInList(width: 150);
                View.Property(p => p.WorkOrderQty).ShowInList(width: 150);
                View.Property(p => p.Attribute1).ShowInList(width: 150);
                View.Property(p => p.Attribute2).ShowInList(width: 150);
                View.Property(p => p.Attribute3).ShowInList(width: 150);
                View.Property(p => p.Attribute4).ShowInList(width: 150);
                View.Property(p => p.Attribute5).ShowInList(width: 150);
                View.Property(p => p.Attribute6).ShowInList(width: 150);
                View.Property(p => p.Attribute7).ShowInList(width: 150);
                View.Property(p => p.Attribute8).ShowInList(width: 150);
                View.Property(p => p.Attribute9).ShowInList(width: 150);
                View.Property(p => p.Attribute10).ShowInList(width: 150);
            }
        }
    }
}