namespace SIE.Web.EMS.Editors.ViewModels
{
    internal class ImportFileViewModelViewConfig : WebViewConfig<ImportFileViewModel>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.FileName)
                .UseImageComponentEditor(p => { p.XType = "checkFileUploadContextEditor"; })
                .HasLabel("")
                .Readonly();
        }
    }
}
