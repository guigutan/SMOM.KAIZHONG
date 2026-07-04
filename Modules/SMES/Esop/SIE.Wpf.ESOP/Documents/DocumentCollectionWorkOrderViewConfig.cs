using SIE.ESop.Documents;
using SIE.Wpf.Command;
using SIE.Wpf.ESOP.Documents.Commands;

namespace SIE.Wpf.ESOP.Documents
{
    /// <summary>
    /// 文档集与工单关系-界面
    /// </summary>
    internal class DocumentCollectionWorkOrderViewConfig : WPFViewConfig<DocumentCollectionWorkOrder>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.FormEdit();
            View.UseCommands(typeof(LookupWorkOrderCommand), typeof(ListDeleteCommand));
            View.Property(p => p.WorkOrder.No).HasLabel("工单号");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.ClearCommands();
            View.UseCommands(typeof(DetailLookupWorkOrderCommand), typeof(ListDeleteCommand));
            View.Property(p => p.WorkOrder.No).HasLabel("工单号");
        }
    }
}
