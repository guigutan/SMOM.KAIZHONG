using SIE.Common.Configs;
using SIE.ESop.Configs;
using SIE.ESop.Documents;
using SIE.Web.ClientMetaModel;
using SIE.Web.ESop.Documents.Commands;

namespace SIE.Web.ESop.Documents
{
    /// <summary>
    /// 文档集视图配置
    /// </summary>
    internal class DocumentCollectionViewConfig : WebViewConfig<DocumentCollection>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseDefaultCommands();
            View.UseCommand(typeof(DocumentSaveCommand).FullName);
            View.UseCommands("SIE.Web.ESop.Documents.Commands.DownloadCommand");
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.FilePath).Readonly();
            View.Property(p => p.IsProcessed).Readonly();
            View.ChildrenProperty(p => p.DocumentList);
            View.ChildrenProperty(p => p.ItemList);
            View.ChildrenProperty(p => p.WorkOrderList);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(4);
            View.UseCommands("SIE.Web.ESop.Documents.Commands.DocumentSaveCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.FilePath).Readonly();
            View.Property(p => p.UploadFilePath).UseCustomEditor(p => { p.XType = "HeaderFileUploadContextEditor"; p.AllowAsterisk =AllowAsterisks.close;}).HasLabel("");

            View.ChildrenProperty(p => p.DocumentList).Show(ChildShowInWhere.Detail).UseViewGroup(ViewConfig.ListView);
            View.ChildrenProperty(p => p.ItemList).Show(ChildShowInWhere.Detail).UseViewGroup(ViewConfig.ListView);
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