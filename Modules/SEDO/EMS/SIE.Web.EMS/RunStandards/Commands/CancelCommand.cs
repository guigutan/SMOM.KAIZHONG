using SIE.Domain.Validation;
using SIE.EMS.RunStandards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.RunStandards.Commands
{
    /// <summary>
    /// 撤回调拨
    /// </summary>
    [JsCommand("SIE.Web.EMS.RunStandards.Commands.CancelCommand")]
    public class CancelCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }

            List<double> selectedIds = new List<double>(args.SelectedIds);
            if (!selectedIds.Any())
            {
                throw new ValidationException("请先选择数据".L10N());
            }
            RT.Service.Resolve<RunStandardsController>().Cancel(selectedIds);
            return true;
        }
    }
}
