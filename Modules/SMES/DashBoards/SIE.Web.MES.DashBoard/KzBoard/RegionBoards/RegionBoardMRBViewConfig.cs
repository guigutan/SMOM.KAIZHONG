using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DashBoard.KzBoard.RegionBoards
{
    public class RegionBoardMRBViewConfig : WebViewConfig<RegionBoardMRB>
    {

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(RegionBoard));
        }

        protected override void ConfigListView()
        {
            View.UseCommands( WebCommandNames.Add,WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.Code).Show();
        }
    }
}
