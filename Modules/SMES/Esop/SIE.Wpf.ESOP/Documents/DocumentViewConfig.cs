using SIE.Common.Configs;
using SIE.ESop.Configs;
using SIE.ESop.Documents;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using SIE.Wpf.ESop.Documents.Commands;
using SIE.Wpf.ESop.Editors;

namespace SIE.Wpf.ESop.Documents
{
    /// <summary>
    /// 文档视图配置
    /// </summary>
    internal class DocumentViewConfig : WPFViewConfig<Document>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(DownloadCommand));
            View.Property(p => p.Code).Show(ShowInWhere.All);
            View.Property(p => p.Name).Show(ShowInWhere.All);
            View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.FileExtension).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.FileSize).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Md5).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Process).Show(ShowInWhere.All);
            View.Property(p => p.DocumentType).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.PdfPlayBeginPage).UseListSetting(p => p.HelpInfo = "填起始页码、结束页码（结束页码必须≥开始页码），播放起始页到结束页；其他则全部播放").Readonly();
            View.Property(p => p.PdfPlayEndPage).UseListSetting(p => p.HelpInfo = "填起始页码、结束页码（结束页码必须≥开始页码），播放起始页到结束页；其他则全部播放").Readonly();
            View.Property(p => p.Source).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.FilePath).Show(ShowInWhere.All);
            View.Property(p => p.IsProcessed).Show(ShowInWhere.List).Readonly();
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            var config = ConfigService.GetConfig(new AttachmentConfig(), typeof(DocumentCollection));
            View.InlineEdit();
            View.UseCommands(typeof(AddDocumentCommand), typeof(EditDocumentCommand), typeof(ListDeleteCommand));
            View.UseCommands(typeof(DownloadCommand));
            View.UseDefaultBehaviors();
            View.Property(p => p.Code).Show(ShowInWhere.All);
            View.Property(p => p.Name).Show(ShowInWhere.All);
            View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.FileExtension).UseListSetting(p => p.ListGridWidth = 80).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.FileSize).UseListSetting(p => p.ListGridWidth = 70).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Md5).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Process).Show(ShowInWhere.All);
            View.Property(p => p.DocumentType).Show(ShowInWhere.All).Readonly().UseListSetting(p => p.ListGridWidth = 70);
            View.Property(p => p.PdfPlayBeginPage).UseListSetting(p => p.HelpInfo = "填起始页码、结束页码（结束页码必须≥开始页码），播放起始页到结束页；其他则全部播放").UseSpinEditor(p => { p.MinValue = 0;p.Decimals = 0;p.Increment = 1; });
            View.Property(p => p.PdfPlayEndPage).UseListSetting(p => p.HelpInfo = "填起始页码、结束页码（结束页码必须≥开始页码），播放起始页到结束页；其他则全部播放").UseSpinEditor(p => { p.MinValue = 0; p.Decimals = 0; p.Increment = 1; });
            View.Property(p => p.Source).Show(ShowInWhere.All).Readonly().UseListSetting(p => p.ListGridWidth = 60);
            View.Property(p => p.FilePath).Show(ShowInWhere.All).UseDocumentSelectEditor(p =>
            {
                //p.Filter = "Excel|*.xls;*.xlsx|PDF|*.pdf|Video|*.avi;*.wmv;*.mp4|Image|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
                p.Filter = "全部|*.xls;*.xlsx;*.pdf;*.docx;*.avi;*.wmv;*.mp4;*.png;*.jpg;*.jpeg;*.gif;*.bmp|Excel|*.xls;*.xlsx|PDF|*.pdf|Word|*.docx|Video|*.avi;*.wmv;*.mp4|Image|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
                if (config != null)
                {
                    p.SetExtendedProperty(DocumentSelectEditor.MaxSize, config.MaxSize);
                }
            }).UseListSetting(p => p.ListGridWidth = 210);
            View.Property(p => p.IsProcessed).Show(ShowInWhere.List).UseListSetting(p => p.ListGridWidth = 60).Readonly();

            View.Property(p => p.FileName).UseEditor(WPFEditorNames.EntityDropDown).UseSelectionViewMeta(new SelectionViewMeta
            {
                ViewGroup = ViewConfig.SelectionView,
                DataSourceProvider = (o, p, c) =>
                {
                    var doc = o as Document;
                    return RT.Service.Resolve<DocumentPropertyChanged>().GetSheetNames(doc.DocumentCollection);
                },
                SelectionEntityType = typeof(Document),
                SelectedValuePath = Document.FileNameProperty
            }).UseListSetting(p => p.ListGridWidth = 180).Readonly(false);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name).HasLabel("工作表名");
        }
    }
}