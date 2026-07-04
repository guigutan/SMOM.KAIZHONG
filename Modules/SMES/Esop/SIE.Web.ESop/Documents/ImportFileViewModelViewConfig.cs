using SIE.ESop.Documents;
using SIE.ESop.Documents.ViewModels;

namespace SIE.Wpf.ESop.Documents
{
    internal class ImportFileViewModelViewConfig : WebViewConfig<ImportFileViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(DocumentCollection));
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.FileName).UseConfigValueEditor(p => { p.XType = "listFileUpload_ContextEditor"; }).HasLabel("").Readonly();
        }
    }
}
