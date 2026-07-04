using SIE.Common.Configs;
using SIE.ESop.Configs;
using SIE.ESop.Documents;
using SIE.Wpf.Command;
using SIE.Wpf.ESop.Documents.Commands;
using SIE.Wpf.ESop.Editors;

namespace SIE.Wpf.ESop.Documents
{
    /// <summary>
    /// 文档集视图配置
    /// </summary>
    internal class DocumentCollectionViewConfig : WPFViewConfig<DocumentCollection>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseDefaultCommands().UseDefaultBehaviors();
            View.ReplaceCommands(typeof(ListAddCommand), typeof(AddDocumentCollectionCommand));
            View.ReplaceCommands(typeof(ListEditCommand), typeof(EditDocumentCollectionCommand));
            View.UseCommands(typeof(DownloadCommand));
            View.RemoveCommands(typeof(ListCopyCommand));
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.FilePath).Readonly();
            View.Property(p => p.IsProcessed).Readonly();
            View.ChildrenProperty(p => p.DocumentList);
            View.ChildrenProperty(p => p.ItemList);
            View.ChildrenProperty(p => p.WorkOrderList);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(3);
            var config = ConfigService.GetConfig(new AttachmentConfig(), typeof(DocumentCollection));
            View.UseCommands(typeof(SaveDocumentCollectionCommand));
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.FilePath).UseDocumentSelectEditor(p =>
            {
                p.Filter = "Excel|*.xls;*.xlsx";
                if (config != null)
                {
                    p.SetExtendedProperty(DocumentSelectEditor.MaxSize, config.MaxSize);
                }
            });
            View.ChildrenProperty(p => p.DocumentList).Show(ChildShowInWhere.Detail).UseViewGroup(ViewConfig.DetailsView);
            View.ChildrenProperty(p => p.ItemList).Show(ChildShowInWhere.Detail).UseViewGroup(ViewConfig.DetailsView);
            View.ChildrenProperty(p => p.WorkOrderList);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.FilePath);
            View.Property(p => p.IsProcessed);
        }
    }
}