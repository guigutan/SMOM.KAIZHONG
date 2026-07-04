using SIE.Domain.Validation;
using SIE.EMS.Lubrications;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.Lubrications.Commands
{

    /// <summary>
    /// 生成备件申请单 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Lubrications.Commands.GenerateSparePartAppCommand")]
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
            var datas = args.Data.ToJsonObject<List<LubricationSparePartApply>>();
            if (datas.Count > 0)
            {
                RT.Service.Resolve<SparePartAppController>().UIGenerateSparePartApp(datas);
            }
            else
            {
                throw new ValidationException("未添加申请单明细".L10N());
            }
            var stateIsChange = RT.Service.Resolve<LubricationController>().CheckParentState(datas.Select(p => p.LubricationId).ToList());
            if (stateIsChange)
            {
                throw new ValidationException("润滑记录状态已提交，禁止继续备件更换及备件申请相关操作！".L10N());
            }
            return true;
        }
    }
  
}
