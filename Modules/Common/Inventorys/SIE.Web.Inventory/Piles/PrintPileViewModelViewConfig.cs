using SIE.Inventory.Piles;
using SIE.Warehouses;
using System;

namespace SIE.Web.Inventory.Piles
{
    /// <summary>
    /// 垛表打印ViewModel视图配置
    /// </summary>
    internal class PrintPileViewModelViewConfig : WebViewConfig<PrintPileViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.LabelTemplateId).UseDataSource((o, c, r) =>
            {
                string qualifiedName = typeof(PilePrintable).GetQualifiedName();
                return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, r, c, SIE.Common.Prints.PrintType.Label);
            });
        }
    }
}