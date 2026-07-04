using SIE.ESop.Documents;
using SIE.Wpf.Command;
using SIE.Wpf.ESop.Documents.Commands;

namespace SIE.Wpf.ESop.Documents
{
    /// <summary>
    /// 适用产品视图配置
    /// </summary>
    internal class DocumentCollectionItemViewConfig : WPFViewConfig<DocumentCollectionItem>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.FormEdit();
            View.UseCommands(typeof(LookupItemCommand), typeof(ListDeleteCommand));
            View.Property(p => p.Item.Code).HasLabel("产品编码");
            View.Property(p => p.Item.Name).HasLabel("产品名称");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.ClearCommands(true);
            View.UseCommands(typeof(DetailLookupItemCommand), typeof(ListDeleteCommand));
            View.Property(p => p.Item.Code).Show(ShowInWhere.All).HasLabel("产品编码");
            View.Property(p => p.Item.Name).Show(ShowInWhere.All).HasLabel("产品名称");
        }
    }
}
