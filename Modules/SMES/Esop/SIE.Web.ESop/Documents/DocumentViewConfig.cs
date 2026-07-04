using SIE.Common.Configs;
using SIE.ESop.Configs;
using SIE.ESop.Documents;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;

namespace SIE.Web.ESop.Documents
{
    /// <summary>
    /// 文档视图配置
    /// </summary>
    internal class DocumentViewConfig : WebViewConfig<Document>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.UseCommands("SIE.Web.ESop.Documents.Commands.DownloadCommand");
            View.Property(p => p.Code).Show(ShowInWhere.All);
            View.Property(p => p.Name).Show(ShowInWhere.All);
            View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.FileExtension).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.FileSize).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Md5).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Process).Show(ShowInWhere.All);
            View.Property(p => p.DocumentType).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.PdfPlayBeginPage).UseListSetting(p => p.HelpInfo = "填起始页码、结束页码（结束页码必须≥开始页码），播放起始页到结束页；其他则全部播放").UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals=false; p.Step = 1; });
            View.Property(p => p.PdfPlayEndPage).UseListSetting(p => p.HelpInfo = "填起始页码、结束页码（结束页码必须≥开始页码），播放起始页到结束页；其他则全部播放").UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; p.Step = 1; });
            View.Property(p => p.Source).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.IsProcessed).Show(ShowInWhere.List).Readonly();
            View.Property(p => p.FilePath).UseConfigValueEditor(p => { p.XType = "docfileUploadContextEditor"; p.AllowAsterisk = AllowAsterisks.close; }).HasLabel("存储路径");
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