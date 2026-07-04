using SIE.EMS.EquipLends;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.EquipLends.Commands
{
    /// <summary>
    /// 设备借还提交命令
    /// </summary>
    public class EquipLendSubmitCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var lendIds = args.SelectedIds.ToList();
            RT.Service.Resolve<EquipLendController>().EquipLendSubmit(lendIds);
            return true;
        }
    }
}
