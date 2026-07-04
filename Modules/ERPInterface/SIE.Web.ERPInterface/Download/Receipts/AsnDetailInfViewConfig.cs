using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Receipts
{
    /// <summary>
    /// ASN单明细中间表视图配置
    /// </summary>
    internal class AsnDetailInfViewConfig : WebViewConfig<AsnDetailInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.AsnNo);
            View.Property(p => p.ExpectQty);
            View.Property(p => p.OrderNo);
            View.Property(p => p.Remark);
            //View.Property(p => p.AsnState);
            View.Property(p => p.ReceiveStorageLocation);
            View.Property(p => p.ItemCode);
            View.Property(p => p.BillDate);
            View.Property(p => p.LineNo);
            View.Property(p => p.RequestDate);
            View.Property(p => p.ItemUnit);
            View.Property(p => p.PoNo);
            View.Property(p => p.PoLineNo);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.AsnNo);
            View.Property(p => p.OrderNo);
        }
    }
}