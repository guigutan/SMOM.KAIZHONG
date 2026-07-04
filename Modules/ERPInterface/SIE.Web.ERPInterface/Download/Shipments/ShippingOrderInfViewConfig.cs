using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Shipments
{
    /// <summary>
    /// 发运单中间表视图配置
    /// </summary>
    internal class ShippingOrderInfViewConfig : WebViewConfig<ShippingOrderInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.No);
            View.Property(p => p.BillerCode);
            View.Property(p => p.BillDate);
            View.Property(p => p.CustomerCode);
            View.Property(p => p.SupplierCode);
            View.Property(p => p.EnterpriseCode);
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.CancelFlag);
            View.Property(p => p.CancelDate);
            View.Property(p => p.Address);
            View.Property(p => p.DeliveryDate);
            View.Property(p => p.ShippingDate);
            View.Property(p => p.OrderType);
            View.Property(p => p.State);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.WarehouseCode);
        }
    }
}