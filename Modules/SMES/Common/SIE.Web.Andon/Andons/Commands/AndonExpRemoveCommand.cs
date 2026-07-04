using SIE.Andon.Andons;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯经验库移除命令
    /// </summary>
    public class AndonExpRemoveCommand : ViewCommand
    {
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var andonExpIds = args.SelectedIds.ToList();
            RT.Service.Resolve<AndonManageController>().AndonExpRemove(andonExpIds);
            return true;
        }
    }
}
