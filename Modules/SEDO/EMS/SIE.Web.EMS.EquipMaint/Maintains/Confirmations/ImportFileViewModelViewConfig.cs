using SIE.EMS.Maintains.Confirmations;
using SIE.Web.EMS.EquipMaint.Maintains.Confirmations.ViewModels;

namespace SIE.Web.EMS.EquipMaint.Maintains.Confirmations
{
    internal class ImportFileViewModelViewConfig : WebViewConfig<ImportFileViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainConfirmation));
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.FileName).UseImageComponentEditor(p =>
            {
                p.XType = "maitainConfirmFileUploadContextEditor";
            }).HasLabel("").Readonly();
        }
    }
}
