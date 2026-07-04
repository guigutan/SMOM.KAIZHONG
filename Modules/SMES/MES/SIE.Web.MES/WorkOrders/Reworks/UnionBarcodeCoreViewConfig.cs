using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;

namespace SIE.Web.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码视图配置
    /// </summary>
    internal class UnionBarcodeCoreViewConfig : WebViewConfig<UnionBarcodeCore>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
        }

        /// <summary>
        /// 关联条码列表ViewGroup
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.WorkOrderNo).ShowInList(150);
            View.Property(p => p.ItemNo).ShowInList(150);
            View.Property(p => p.Barcode).ShowInList(150);
            View.Property(p => p.InspetNo).ShowInList(150);
            View.Property(p => p.Result);
        }
    }
}