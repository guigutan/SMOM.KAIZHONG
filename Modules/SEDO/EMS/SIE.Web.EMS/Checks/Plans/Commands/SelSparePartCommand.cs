using SIE.Domain;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Enums;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 选择备件
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.SelSparePartCommand")]
    public class SelSparePartCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var sparePartList = args.Data.ToJsonObject<List<CheckPlanSparePart>>();
            Check.NotNullOrEmpty(sparePartList, nameof(sparePartList));
            if (sparePartList == null || sparePartList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(sparePartList)));
            }
            //保存选择的备件数据
            RT.Service.Resolve<CheckPlanController>().SelSparePart(sparePartList);
            return true;
        }
    }
}
