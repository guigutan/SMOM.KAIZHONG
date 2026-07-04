using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.PurchaseOrders
{
    /// <summary>
    /// 采购订单中间表视图配置
    /// </summary>
    internal class PurchaseOrderInfViewConfig : WebViewConfig<PurchaseOrderInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.No);
            View.Property(p => p.SupplierCode);
            View.Property(p => p.SupplierAdrssErpKey);
            View.Property(p => p.BillerCode);
            View.Property(p => p.BillDate);
            View.Property(p => p.AuditDate);
            View.Property(p => p.AuditFlag);
            View.Property(p => p.AuditorCode);
            View.Property(p => p.ReceivingWhCode);
            //View.Property(p => p.PoState);
            View.Property(p => p.ClosedDate);
            View.Property(p => p.ClosedFlag);
            View.Property(p => p.CancelDate);
            View.Property(p => p.CancelFlag);
            View.Property(p => p.Remark);
            View.Property(p => p.UpdateByCode);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.SupplierCode);
        }
    }
}