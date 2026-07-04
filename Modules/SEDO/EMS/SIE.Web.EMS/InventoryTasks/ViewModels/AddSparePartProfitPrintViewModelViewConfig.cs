using SIE.Common.Prints;
using SIE.Domain;
using SIE.EMS.InventoryTasks;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.Printables;
using System;

namespace SIE.Web.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 序列号打印界面
    /// </summary>
    public class AddSparePartProfitPrintViewModelViewConfig : WebViewConfig<AddSparePartProfitPrintViewModel>
    {

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(InventoryTask));

            View.Property(p => p.TemplateId).UseDataSource((e, p, r) =>
            {
                var sparePartProfitPrintViewModel = e as AddSparePartProfitPrintViewModel;

                if (sparePartProfitPrintViewModel == null)
                {
                    return new EntityList<PrintTemplate>();
                }

                if (sparePartProfitPrintViewModel.ControlMethod == ControlMethod.Sn)
                {
                    return RT.Service.Resolve<InventoryTaskSpartPartController>().GetPrintTemplatesByType(typeof(StoreSummaryDetailPrintable).GetQualifiedName(), p, r);

                }
                else if (sparePartProfitPrintViewModel.ControlMethod == ControlMethod.Batch)
                {
                    return RT.Service.Resolve<InventoryTaskSpartPartController>().GetPrintTemplatesByType(typeof(StoreSummaryLotPrintable).GetQualifiedName(), p, r);
                }
                else
                {
                    return new EntityList<PrintTemplate>();
                }
            });
        }
    }
}
