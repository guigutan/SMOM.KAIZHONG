using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using SIE.Web.Command;
using SIE.Web.Equipments.Abnormal.ViewModels;
using System;

namespace SIE.Web.Equipments.Abnormal.Commands
{
    /// <summary>
    /// 恢复停线
    /// </summary>
    class AbnormalCauseRestoreCommand : ViewCommand
    {
        /// <summary>
        /// 恢复停线
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RestoreReasonViewModel model = args.Data.ToJsonObject<RestoreReasonViewModel>();
            if (model == null)
                throw new ValidationException("停线管理格式不正确。".L10N());
            var abnormalId = model.AbnormalCauseId;

            AppRuntime.Service.Resolve<AbnormalCauseController>().RestoreAbnormalCauseManual(abnormalId, model.Reason);
            return true;
        }
    }
}
