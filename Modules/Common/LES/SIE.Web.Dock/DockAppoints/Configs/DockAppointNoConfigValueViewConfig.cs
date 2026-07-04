using SIE.Common.Prints;
using SIE.Dock.DockAppoints.Configs;
using SIE.Dock.Printables;
using SIE.Domain;
using SIE.Warehouses;
using System;

namespace SIE.Web.Dock.DockAppoints.Configs
{
    /// <summary>
    /// 月台预约配置项视图配置
    /// </summary>
    internal class DockAppointNoConfigValueViewConfig : WebViewConfig<DockAppointNoConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberRule).Show();
                View.Property(p => p.PrintTemplate).UseDataSource((o, c, r) =>
                {
                    var configValue = o as DockAppointNoConfigValue;
                    if (configValue == null)
                    {
                        return new EntityList<PrintTemplate>();
                    }

                    string qualifiedName = typeof(DockAppointPrintable).GetQualifiedName();
                    return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, r, c);
                }).Show();
                View.Property(p => p.MaxAppointTime).Show();
            }
        }
    }
}
