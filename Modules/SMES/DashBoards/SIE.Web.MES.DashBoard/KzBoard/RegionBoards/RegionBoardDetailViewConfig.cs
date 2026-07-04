using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.MetaModel.View;
using SIE.Web.MES.DashBoard.KzBoard.RegionBoards.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DashBoard.KzBoard.RegionBoards
{
    public class RegionBoardDetailViewConfig : WebViewConfig<RegionBoardDetail>
    {

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(RegionBoard));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelectResourceCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.ResourceCode).Show().Readonly();
            View.Property(p => p.ResourceName).Show().Readonly();
            View.Property(p => p.Sort).Show();
            View.Property(p => p.ReportHours).Show();
        }
    }
}
