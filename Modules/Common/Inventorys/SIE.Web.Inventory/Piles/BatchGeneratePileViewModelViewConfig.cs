using SIE.Inventory.Piles;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Inventory.Piles
{
    /// <summary>
    /// 批量生成ViewModel视图配置
    /// </summary>
    public class BatchGeneratePileViewModelViewConfig : WebViewConfig<BatchGeneratePileViewModel>
    {
        /// <summary>
        /// 通用视图配置
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
            View.HasDetailColumnsCount(1);
            using (View.OrderProperties())
            {
                View.Property(p => p.TurnoverBoxModelId);
                View.Property(p => p.GenerateQty);
                View.Property(p => p.TemplateId).UseDataSource((o, c, r) =>
                {
                    string qualifiedName = typeof(PilePrintable).GetQualifiedName();
                    return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, r, c, SIE.Common.Prints.PrintType.Label);
                });
            }
        }
    }
}
