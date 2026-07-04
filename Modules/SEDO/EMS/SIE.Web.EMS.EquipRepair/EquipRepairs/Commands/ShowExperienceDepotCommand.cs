using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 查看经验库
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ShowExperienceDepotCommand")]
    public class ShowExperienceDepotCommand : ViewCommand
    { /// <summary>
      /// 执行选择
      /// </summary>
      /// <param name="args">args</param>
      /// <param name="scope">scope</param>
      /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
