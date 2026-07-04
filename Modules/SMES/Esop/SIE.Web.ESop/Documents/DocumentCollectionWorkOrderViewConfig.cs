using SIE.ESop.Documents;
using SIE.MetaModel.View;

namespace SIE.Web.ESOP.Documents
{
    /// <summary>
    /// 文档集与工单关系-界面
    /// </summary>
    internal class DocumentCollectionWorkOrderViewConfig : WebViewConfig<DocumentCollectionWorkOrder>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.FormEdit();
            View.UseCommands("SIE.Web.ESop.Common.Commands.SelWoCommand");
            View.UseCommand(WebCommandNames.Delete);
            View.Property(p => p.WorkOrder).HasLabel("工单号");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.ClearCommands();
            View.UseCommands("SIE.Web.ESop.Common.Commands.SelWoCommand");
            View.UseCommand(WebCommandNames.Delete);
            View.Property(p => p.WorkOrder).HasLabel("工单号");
        }
    }
}
