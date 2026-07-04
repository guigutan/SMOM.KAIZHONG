using SIE.Common.Prints;
using SIE.Dock.DockQueues.Configs;
using SIE.Dock.Printables;
using SIE.Domain;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.DockQueues.Configs
{
    /// <summary>
    /// 月台排队配置项视图配置
    /// </summary>
    internal class DockQueueNumberConfigValueViewConfig : WebViewConfig<DockQueueNumberConfigValue>
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
                    var configValue = o as DockQueueNumberConfigValue;
                    if (configValue == null)
                    {
                        return new EntityList<PrintTemplate>();
                    }
                    string qualifiedName = typeof(DockQueuePrintable).GetQualifiedName();
                    return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, r, c);
                }).Show();
                View.Property(p => p.MaxDelayNum).DefaultValue(2).Show();
                View.Property(p => p.CheckOutTimeOut).DefaultValue(4).Show();
                View.Property(p => p.CheckOutDelay).DefaultValue(10).Show();
                View.Property(p => p.AutoCheckIn).DefaultValue(true).Show();
            }
        }
    }
}
