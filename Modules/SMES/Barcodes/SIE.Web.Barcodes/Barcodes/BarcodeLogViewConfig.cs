using SIE.Barcodes;
using SIE.MetaModel.View;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 条码打印日志视图配置
    /// </summary>
    internal class BarcodeLogViewConfig : WebViewConfig<BarcodeLog>
    {
        /// <summary>
        /// 列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.BarcodeLogWONo).ShowInList(150).HasLabel("工单").Readonly();
            View.Property(p => p.BarcodeLogBarcodeSn).ShowInList(150).HasLabel("条码").Readonly();
            View.Property(p => p.Type).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.Reason).Readonly();
            View.Property(p => p.BarcodeLogOperatorName).HasLabel("操作人").Readonly();
            View.Property(p => p.OperatDate).ShowInList(150).Readonly();
        }
    }
}