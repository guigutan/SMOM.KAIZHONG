using SIE.MES.DashBoard.KzReport.OrganizeCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DashBoard.OrganizeCodes
{
    public class OrganizeCodeViewConfig:WebViewConfig<OrganizeCode>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseImportCommands();
            View.Property(p => p.ParkCode);
            View.Property(p => p.ParkName);
            View.Property(p => p.ProductLine);
            View.Property(p => p.FactoryCode);
            View.Property(p => p.FactoryName);
            View.Property(p => p.PlantCode);
            View.Property(p => p.PlantName);
            View.Property(p => p.WorkshopCode);
            View.Property(p => p.WorkshopName);
            View.Property(p => p.WorkshopDescription);
            View.Property(p => p.MrpController);
            View.Property(p => p.SapScheduler);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.ParkCode);
            View.Property(p => p.ParkName);
            View.Property(p => p.ProductLine);
            View.Property(p => p.FactoryCode);
            View.Property(p => p.FactoryName);
            View.Property(p => p.PlantCode);
            View.Property(p => p.PlantName);
            View.Property(p => p.WorkshopCode);
            View.Property(p => p.WorkshopName);
            View.Property(p => p.MrpController);
            View.Property(p => p.SapScheduler);
        }

        protected override void ConfigImportView()
        {
            View.Property(p => p.ParkCode);
            View.Property(p => p.ParkName);
            View.Property(p => p.ProductLine);
            View.Property(p => p.FactoryCode);
            View.Property(p => p.FactoryName);
            View.Property(p => p.PlantCode);
            View.Property(p => p.PlantName);
            View.Property(p => p.WorkshopCode);
            View.Property(p => p.WorkshopName);
            View.Property(p => p.WorkshopDescription);
            View.Property(p => p.MrpController);
            View.Property(p => p.SapScheduler);
        }
    }
}
