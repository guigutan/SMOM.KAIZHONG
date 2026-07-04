using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 强制关单
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.CompelCloseCommand")]
    public class CompelCloseCommand : ViewCommand
    {
        /// <summary>
        /// 强制关单
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //【维修人员】中是否存在和【维修责任人】相同的人员，是则提示：其他维修人员无需与主责任人重复维护！
            var compelCloseViewModel = args.Data.ToJsonObject<CompelCloseViewModel>();
            RT.Service.Resolve<RepairController>().CompelClose(compelCloseViewModel);
            return true;
        }
    }
}
