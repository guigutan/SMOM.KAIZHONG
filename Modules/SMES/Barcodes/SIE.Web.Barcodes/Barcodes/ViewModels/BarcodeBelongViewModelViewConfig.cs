using SIE.Barcodes;
using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Core.WorkOrders;
using SIE.Domain;
using System.Collections.Generic;

namespace SIE.Web.Barcodes.Barcodes.ViewModels
{
    /// <summary>
    /// 条码归属视图配置
    /// </summary>
    internal class BarcodeBelongViewModelViewConfig : WebViewConfig<BarcodeBelongViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(Barcode));
            View.Property(p => p.Sn).Readonly().Show(ShowInWhere.All);
            View.Property(p => p.OrgWorkOrderNo).Readonly().Show(ShowInWhere.All);
            View.Property(p => p.WorkOrderId).UseDataSource((source, pagingInfo, keyword) =>
              {
                  var barcodeBelongVM = source as BarcodeBelongViewModel;
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
