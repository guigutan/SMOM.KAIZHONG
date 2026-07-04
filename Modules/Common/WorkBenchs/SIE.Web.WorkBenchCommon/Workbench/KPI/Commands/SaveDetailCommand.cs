using SIE.Domain;
using SIE.Web.Command;
using System;
using SIE.Domain.Validation;
using SIE.WorkBenchCommon.Workbench.KPI;

namespace SIE.Web.WorkBenchCommon.Workbench.KPI.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class SaveDetailCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is QuotaTargetSetting))
                throw new ValidationException("该数据格式不正确。".L10N());

            var audit = entity as QuotaTargetSetting;

            RT.Service.Resolve<QuotaTargetSettingController>().Save(audit);
        }
    }

}
