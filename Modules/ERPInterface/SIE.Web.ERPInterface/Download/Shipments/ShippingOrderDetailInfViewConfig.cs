using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Shipments
{
    /// <summary>
    /// 发运单明细中间表视图配置
    /// </summary>
    internal class ShippingOrderDetailInfViewConfig : WebViewConfig<ShippingOrderDetailInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.ShippingOrderNo);
            View.Property(p => p.ExpectQty);
            View.Property(p => p.OrderNo);
            View.Property(p => p.Remark);
            View.Property(p => p.AppointStorageLocation);
            View.Property(p => p.ItemCode);
            View.Property(p => p.BillDate);
            View.Property(p => p.LineNo);
            View.Property(p => p.RequestDate);
            View.Property(p => p.ItemUnit);
            View.Property(p => p.PoNo);
            View.Property(p => p.PoDetailLineNo);
            View.Property(p => p.State);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ShippingOrderNo);
            View.Property(p => p.ItemCode);
        }
    }
}