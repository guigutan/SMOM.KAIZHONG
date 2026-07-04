using SIE.Andon.Andons;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯管理响应命令
    /// </summary>
    public class AndonManageResponseCommand : ViewCommand
    {
        /// <summary>
        /// 响应
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
            RT.Service.Resolve<AndonManageController>().AndonManageResponse(info.AndonManageId, info.OperateType, info.Reason);
            return true;
        }
    }
}
