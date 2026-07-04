using SIE.Domain;
using SIE.MES.DashBoard.KzReport.ProductionProcesss;
using SIE.Tech.Processs;
using SIE.Web.MES.DashBoard.ProductionProcesss.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DashBoard.ProductionProcesss
{
    public class ProductionProcessViewConfig:WebViewConfig<ProductionProcess>
    {

        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(ProductionProcessImportCommand).FullName, typeof(ProductionProcessDLTemplateCommand).FullName);
            View.Property(p => p.ProductLine);
            View.Property(p => p.InventoryCode);
            View.Property(p => p.PlantCode);
            View.Property(p => p.PlantName);
            View.Property(p => p.ProcessCode);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.ProductLine);
            View.Property(p => p.PlantCode);
            View.Property(p => p.PlantName);
            //View.Property(p => p.ProcessId);
        }

        protected override void ConfigImportView()
        {
            View.Property(p => p.ProductLine).ImportIndexer();
            View.Property(p => p.InventoryCode).ImportIndexer();
            View.Property(p => p.PlantCode).ImportIndexer();
            View.Property(p => p.PlantName);
            View.Property(p => p.ProcessCode).HasLabel("工序编码").ImportIndexer();
        }
    }
}
