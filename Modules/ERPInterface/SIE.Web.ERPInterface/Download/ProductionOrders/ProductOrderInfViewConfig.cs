using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.ProductionOrders
{
    /// <summary>
    /// 生产订单BOM中间表视图配置
    /// </summary>
    public class ProductOrderInfViewConfig : WebViewConfig<ProductOrderInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code);
            View.Property(p => p.ItemCode);
            View.Property(p => p.Qty);
            View.Property(p => p.Priority);
            View.Property(p => p.RouteCode);
            //View.Property(p => p.OrderType);
            View.Property(p => p.FactoryCode);
            View.Property(p => p.SaleNo);
            View.Property(p => p.CustomerCode);
            View.Property(p => p.RequireDelivery);
            View.Property(p => p.PromiseDelivery);
            View.Property(p => p.RawMaterialDate);
            View.Property(p => p.SuggestStart);
            View.Property(p => p.SuggestEnd);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.ItemCode);
        }
    }
}
