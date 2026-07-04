using SIE.Domain;
using SIE.Domain.Validation;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.Web.Command;
using System;

namespace SIE.Web.AbnormalInfo.AbnormalInfos.Commands
{
    /// <summary>
    /// 确认异常 提交命令
    /// </summary>
    public class SubmitAbnormalInfoCommand : FormSaveCommand
    {
        /// <summary>
        /// 确认异常
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is AbnormalInfor))
                throw new ValidationException("该数据不是异常信息数据格式。".L10N());

            var abnormal = entity as AbnormalInfor;

            RT.Service.Resolve<AbnormalInfoController>().SumbitAbnormalInfo(abnormal);
        }
    }
}
