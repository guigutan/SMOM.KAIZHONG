using SIE.Dock.DockAppoints;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.ViewModels;
using SIE.Domain;

namespace SIE.Web.Dock.ViewModels
{
    /// <summary>
    /// 分配月台ViewModel视图配置
    /// </summary>
    public class AssignDockViewModelViewConfig : WebViewConfig<AssignDockViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.DockMaintainId).UseDataSource((o, c, r) =>
                {
                    var viewModel = o as AssignDockViewModel;
                    if (viewModel == null || viewModel.YardId <= 0)
                    {
                        return new EntityList<DockMaintain>();
                    }

                    bool? isRec = null;
                    bool? isShip = null;
                    if (viewModel.AppointType == AppointType.Delivery)
                    {
                        isRec = true;
                    }
                    else
                    {
                        isShip = true;
                    }

                    return RT.Service.Resolve<DockMaintainService>().GetDockMaintainByParkIds(viewModel.YardId, r, isRec, isShip, c);
                }).Show();
            }
        }
    }
}
