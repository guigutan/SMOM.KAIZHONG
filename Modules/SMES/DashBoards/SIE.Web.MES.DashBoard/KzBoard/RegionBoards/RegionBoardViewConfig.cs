using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DashBoard.KzBoard.RegionBoards
{
    public class RegionBoardViewConfig : WebViewConfig<RegionBoard>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(RegionBoard));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll).UseImportCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Region).Show();
                View.Property(p => p.RegionBoardType).Show();
                //View.Property(p => p.DeviceStatus).Show();
                //View.Property(p => p.HeatTreatment).Show();
                //View.Property(p => p.ProductionOutput).Show();
                View.ChildrenProperty(p => p.RegionBoardDetailList).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.RegionBoardMRBList).Show(ChildShowInWhere.All);
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Region).Show();
                View.Property(p => p.RegionBoardType).Show();
                //View.Property(p => p.DeviceStatus).Show();
                //View.Property(p => p.HeatTreatment).Show();
                //View.Property(p => p.ProductionOutput).Show();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Region).Show();
                View.Property(p => p.RegionBoardType).Show();
                //View.Property(p => p.DeviceStatus).Show();
                //View.Property(p => p.HeatTreatment).Show();
                //View.Property(p => p.ProductionOutput).Show();
            }
        }
    }
}
