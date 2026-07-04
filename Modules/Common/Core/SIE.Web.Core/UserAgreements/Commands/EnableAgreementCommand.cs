using SIE.Core.UserAgreements;
using SIE.Domain.Validation;
using SIE.Security;
using SIE.Web.Command;
using System;

namespace SIE.Web.Core.UserAgreements.Commands
{
    /// <summary>
    /// 启用命令
    /// </summary>
    [AllowAnonymous]
    internal class EnableAgreementCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var recordId = args.Data.ToJsonObject<double>();
            if (recordId == 0)
            {
                throw new ValidationException("请先选择未启用的协议".L10N());
            }

            RT.Service.Resolve<UserAgreementController>().EnableAgreement(recordId);

            return null;
        }
    }
}
