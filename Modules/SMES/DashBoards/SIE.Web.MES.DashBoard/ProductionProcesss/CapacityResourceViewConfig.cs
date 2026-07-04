using DevExpress.XtraRichEdit.Layout;
using SIE.MES.DashBoard.KzReport.ProductionProcesss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DashBoard.ProductionProcesss
{
    public class CapacityResourceViewConfig : WebViewConfig<CapacityResource>
    {

        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseDefaultCommands().UseImportCommands();
            View.Property(p=>p.ResourceId);
            View.Property(p=>p.ProcessId);
            View.Property(p => p.ResourceType);
            View.Property(p => p.Uph);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.ResourceId);
            View.Property(p => p.ProcessId);
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.Resource.Code).HasLabel("资源编码");
                View.PropertyRef(p => p.Process.Code).HasLabel("工序编码");
                View.Property(p => p.ResourceType);
                View.Property(p => p.Uph);
            }
        }
    }
}
