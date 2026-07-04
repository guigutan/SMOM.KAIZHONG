using SIE.Common.Prints;
using SIE.Dock.Printables;
using SIE.Dock.ViewModels;
using SIE.Domain;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.ViewModels
{
    /// <summary>
    /// 月台排队打印
    /// </summary>
    internal class DockQueuePrintViewModelViewConfig : WebViewConfig<DockQueuePrintViewModel>
    {
        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.PrintTemplate).UseDataSource((o, c, r) =>
            {
                var viewModel = o as DockQueuePrintViewModel;
                if (viewModel == null)
                {
                    return new EntityList<PrintTemplate>();
                }

                string qualifiedName = typeof(DockQueuePrintable).GetQualifiedName();
                return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, r, c);
            }).Show();
        }
    }
}
