using SIE.Dock.DockMaintains;
using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.Dock.DockMaintains.Commands;

namespace SIE.Web.Dock.DockMaintains
{
    /// <summary>
    /// 月台维护视图配置
    /// </summary>
    internal class DockMaintainViewConfig : WebViewConfig<DockMaintain>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, typeof(DeleteDockMaintainCommand).FullName, WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.IsReceive).DefaultValue(true);
                View.Property(p => p.IsShip).DefaultValue(true);
                View.Property(p => p.YardZoneId).UseDataSource((e, p, k) =>
                {
                    var yardZone = e as DockMaintain;
                    if (yardZone == null)
                    {
                        return new EntityList<YardZone>();
                    }
                    return RT.Service.Resolve<YardZoneService>().GetEnableYardZones(k, p);
                });
                View.Property(p => p.RecPriority).DefaultValue(1).ShowInList(120).UseListSetting(e => { e.HelpInfo = "数字越小，优先级越高"; });
                View.Property(p => p.ShipPriority).DefaultValue(1).ShowInList(120).UseListSetting(e => { e.HelpInfo = "数字越小，优先级越高"; });
                View.Property(p => p.State).Readonly().DefaultValue(Domain.State.Enable);
                View.Property(p => p.Remark);
                View.ChildrenProperty(p => p.DockMaintainWhList).HasLabel("适用仓库");
            }
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }
    }
}