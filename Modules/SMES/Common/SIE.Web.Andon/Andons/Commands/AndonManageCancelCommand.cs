using SIE.Andon.Andons;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯管理取消命令
    /// </summary>
    public class AndonManageCancelCommand : ViewCommand
    {
        /// <summary>
        /// 取消命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var info = args.Data.ToJsonObject<AndonManageOperate>();
            if (info == null)
            {
                throw new ValidationException("请求数据为空，信息异常".L10N());
            }
            if (info.Reason.IsNullOrEmpty())
            {
                throw new ValidationException("取消原因不能为空".L10N());
            }
            RT.Service.Resolve<AndonManageController>().AndonManageCancel(info.AndonManageId, info.OperateType, info.Reason);
            return true;
        }
    }
}
