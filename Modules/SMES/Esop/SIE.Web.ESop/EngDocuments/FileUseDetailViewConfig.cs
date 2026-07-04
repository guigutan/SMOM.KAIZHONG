using SIE.ESop.EngDocuments;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.ESop.EngDocuments.Commands;

namespace SIE.Web.ESop.EngDocuments
{
    /// <summary>
    /// 工程文件子表视图配置
    /// </summary>
    public class FileUseDetailViewConfig : WebViewConfig<FileUseDetail>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(EngDocument));
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(FileUseDetailSaveCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.UseType).UseListSetting(p => p.HelpInfo = "来源于快码DOC_TYPE").UseCatalogEditor(p => { p.CatalogType = EngDocumentDetail.DocTypeCatalogType; p.CatalogReloadData = true; }).ShowInList(width: 120);
                View.Property(p => p.File).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.ESop.EngDocuments.Scripts.FileUseTypeSelDocEditor"; p.Editable = false; }).ShowInList(width: 300);
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.UseType).UseListSetting(p => p.HelpInfo = "来源于快码DOC_TYPE").UseCatalogEditor(p => { p.CatalogType = EngDocumentDetail.DocTypeCatalogType; p.CatalogReloadData = true; }).ShowInList(width: 120);

        }
    }
}
