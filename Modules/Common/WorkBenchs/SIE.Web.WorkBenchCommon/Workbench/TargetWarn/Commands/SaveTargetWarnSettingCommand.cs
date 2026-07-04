using SIE.Domain;
using SIE.Web.Command;
using System;
using SIE.Domain.Validation;
using SIE.WorkBenchCommon.Workbench.TargetWarn;

namespace SIE.Web.WorkBenchCommon.Workbench.TargetWarn.Commands
{
    /// <summary>
    /// 全部符合
    /// </summary>
    public class SaveTargetWarnSettingCommand : FormSaveCommand
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
            if (!(entity is TargetWarnSetting))
                throw new ValidationException("该数据不是预警设定数据格式。".L10N());

            var audit = entity as TargetWarnSetting;

            RT.Service.Resolve<TargetWarnSettingController>().Save(audit);
        }
    }

}
