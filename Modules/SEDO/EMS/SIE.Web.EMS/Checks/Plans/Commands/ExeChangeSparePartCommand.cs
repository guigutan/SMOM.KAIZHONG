using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 更换备件 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.ExeChangeSparePartCommand")]
    public class ExeChangeSparePartCommand : ViewCommand
    {
        /// <summary>
        /// 执行更换备件命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var datas = args.Data.ToJsonObject<List<CheckPlanSparePart>>();
            if (datas.Count > 0)
                RT.Service.Resolve<CheckPlanController>().UIChangeCheckPlanSparePart(datas);
            else
                throw new ValidationException("未添加更换单明细".L10N());

            return true;
        }
    }
}
