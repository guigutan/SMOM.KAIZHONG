using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.PurchaseOrders
{
    /// <summary>
    /// 采购订单明细中间表视图配置
    /// </summary>
    internal class PurchaseOrderDetailInfViewConfig : WebViewConfig<PurchaseOrderDetailInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.PoNo);
            View.Property(p => p.LineNo);
            View.Property(p => p.Quantity);
            View.Property(p => p.UnitPrice);
            View.Property(p => p.DeliveryDate);
            View.Property(p => p.ItemCode);
            View.Property(p => p.PurchaseUnit);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.TaskNo);
            View.Property(p => p.ClosedDate);
            View.Property(p => p.ClosedFlag);
            View.Property(p => p.CancelDate);
            View.Property(p => p.CancelFlag);
            View.Property(p => p.CancelReason);
            View.Property(p => p.UpdateByCode);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.PoNo);
            View.Property(p => p.LineNo);
        }
    }
}