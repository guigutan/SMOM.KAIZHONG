using SIE.EMS.Purchases.PurchaseOrders;

namespace SIE.Web.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 历史订单界面
    /// </summary>
    internal class HistoryOrderViewModelViewConfig : WebViewConfig<HistoryOrderViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.Factory).ShowInList(120);
            View.Property(p => p.Department).ShowInList(120);
            View.Property(p => p.OrderNo).ShowInList(130);
            View.Property(p => p.SupplierCode).ShowInList(120);
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.ObjectCode).ShowInList(130);
            View.Property(p => p.Description).ShowInList(200);
            View.Property(p => p.Specification).ShowInList(120);
            View.Property(p => p.Qty).ShowInList(80);
            View.Property(p => p.UnitName).ShowInList(80);
            View.Property(p => p.Price).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130);
            View.Property(p => p.TaxRate).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(70);
            View.Property(p => p.PriceNoTax).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130);
            View.Property(p => p.Amount).ShowInList(130);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.PurchaseDate).ShowInList(150);
        }
    }
}
