using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 生成备件申请单 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.GenerateSparePartAppCommand")]
    public class GenerateSparePartAppCommand : ViewCommand
    {
        /// <summary>
        /// 执行生成备件申请单命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var datas = args.Data.ToJsonObject<List<CheckPlanSparePartApl>>();
            if (datas.Count > 0)
                RT.Service.Resolve<SparePartAppController>().UIGenerateSparePartApp(datas);
            else
                throw new ValidationException("未添加申请单明细".L10N());

            return true;
        }
    }
}
