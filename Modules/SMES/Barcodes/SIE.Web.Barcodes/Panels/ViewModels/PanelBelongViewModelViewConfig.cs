using SIE.Barcodes.Panels;
using SIE.Barcodes.Panels.ViewModels;
using SIE.Core.WorkOrders;
using SIE.Domain;
using System.Collections.Generic;

namespace SIE.Web.Barcodes.Panels.ViewModels
{
    /// <summary>
    /// 拼板码归属视图配置
    /// </summary>
    internal class PanelBelongViewModelViewConfig : WebViewConfig<PanelBelongViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(Panel));
            View.Property(p => p.Sn).Readonly().Show(ShowInWhere.All);
            View.Property(p => p.OrgWorkOrderNo).Readonly().Show(ShowInWhere.All);
            View.Property(p => p.WorkOrderId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var barcodeBelongVM = source as PanelBelongViewModel;
                if (barcodeBelongVM == null)
                    return new EntityList<WorkOrder>();
                return RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(barcodeBelongVM.OrgWorkOrderId, keyword, pagingInfo);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.PlanQty), nameof(e.WorkOrder.PlanQty));
                keyValues.Add(nameof(e.PrintedQty), nameof(e.WorkOrder.PrintedQty));
                m.DicLinkField = keyValues;
            }).Show(ShowInWhere.All);
            View.Property(p => p.PlanQty).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.PrintedQty).Readonly().Show(ShowInWhere.Hide);
        }
    }
}
