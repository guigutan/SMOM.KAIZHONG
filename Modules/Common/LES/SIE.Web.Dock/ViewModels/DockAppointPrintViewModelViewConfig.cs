using SIE.Common.Prints;
using SIE.Dock.Printables;
using SIE.Dock.ViewModels;
using SIE.Domain;
using SIE.Warehouses;
using System;

namespace SIE.Web.Dock.ViewModels
{
    /// <summary>
    /// 月台预约打印
    /// </summary>
    internal class DockAppointPrintViewModelViewConfig : WebViewConfig<DockAppointPrintViewModel>
    {
        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.PrintTemplate).UseDataSource((o, c, r) =>
            {
                var viewModel = o as DockAppointPrintViewModel;
                if (viewModel == null)
                {
                    return new EntityList<PrintTemplate>();
                }

                string qualifiedName = typeof(DockAppointPrintable).GetQualifiedName();
                return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, r, c);
            }).Show();
        }
    }
}
