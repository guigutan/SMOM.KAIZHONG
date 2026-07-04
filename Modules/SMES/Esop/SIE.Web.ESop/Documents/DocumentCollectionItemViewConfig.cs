using SIE.ESop.Documents;
using SIE.MetaModel.View;

namespace SIE.Web.ESop.Documents
{
    /// <summary>
    /// 适用产品视图配置
    /// </summary>
    internal class DocumentCollectionItemViewConfig : WebViewConfig<DocumentCollectionItem>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.FormEdit();
            View.UseCommand("SIE.Web.ESop.Documents.Commands.SelDocumentsItem");
            View.UseCommand(WebCommandNames.Delete);
           View.Property(p => p.ItemCode).HasLabel("产品编码");
            View.Property(p => p.ItemName).HasLabel("产品名称");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.ClearCommands(true);
            View.UseCommand("SIE.Web.ESop.Documents.Commands.SelDocumentsItem");
            View.UseCommand(WebCommandNames.Delete);
            View.Property(p => p.ItemCode).Show(ShowInWhere.All).HasLabel("产品编码");
            View.Property(p => p.ItemName).Show(ShowInWhere.All).HasLabel("产品名称");
        }
    }
}
