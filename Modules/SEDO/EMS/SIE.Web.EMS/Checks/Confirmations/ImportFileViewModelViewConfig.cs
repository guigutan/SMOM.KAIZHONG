using SIE.EMS.Checks.Confirmations;
using SIE.Web.EMS.Checks.Confirmations.ViewModels;

namespace SIE.Web.EMS.Checks.Confirmations
{
    internal class ImportFileViewModelViewConfig : WebViewConfig<ImportFileViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckConfirmation));
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.FileName).UseImageComponentEditor(p => { p.XType = "checkFileUploadContextEditor"; }).HasLabel("").Readonly();
        }
    }
}
