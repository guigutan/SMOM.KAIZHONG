using SIE.Domain.Validation;
using SIE.EMS.Lubrications;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Lubrications.Commands
{
    /// <summary>
    /// 更换备件 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Lubrications.Commands.ExeChangeSparePartCommand")]
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
            var datas = args.Data.ToJsonObject<List<LubricationSparePart>>();
            if (datas.Count > 0)
            {
                var stateIsChange = RT.Service.Resolve<LubricationController>().CheckParentState(datas.Select(p => p.LubricationId).ToList());
                if (stateIsChange)
                {
                    throw new ValidationException("润滑记录状态已提交，禁止继续备件更换及备件申请相关操作！".L10N());
                }
                RT.Service.Resolve<LubricationController>().UIChangeLubricationSparePart(datas);
            }
            else
            {
                throw new ValidationException("未添加更换单明细".L10N());
            }
            return true;
        }
    }
}
