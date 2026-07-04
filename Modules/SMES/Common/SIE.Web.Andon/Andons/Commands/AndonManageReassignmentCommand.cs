using SIE.Andon.Andons;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯管理转派命令
    /// </summary>
    public class AndonManageReassignmentCommand : ViewCommand
    {
        /// <summary>
        /// 转派
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var info = args.Data.ToJsonObject<AndonManageOperate>();
            if (info == null)
            {
                throw new ValidationException("请求数据为空，信息异常".L10N());
            }
            if (info.ReassignAndonId == 0)
            {
                throw new ValidationException("未选择转派安灯!".L10N());

            }
            RT.Service.Resolve<AndonManageController>().AndonManageReassignment(info.AndonManageId, info.OperateType, info.Reason, info.ReassignEmployeeId, info.ReassignAndonId);
            return true;
        }
    }
}
