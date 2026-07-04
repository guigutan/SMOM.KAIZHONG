using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    internal class ImportFileViewModelViewConfig : WebViewConfig<ImportFileViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(HandoverConfirmDetail));
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
