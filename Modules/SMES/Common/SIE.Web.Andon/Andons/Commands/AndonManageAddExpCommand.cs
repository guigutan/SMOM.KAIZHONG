using SIE.Andon.Andons;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 加入经验库
    /// </summary>
    public class AndonManageAddExpCommand : ViewCommand
    {
        /// <summary>
        /// 加入经验库
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var andonManage = args.Data.ToJsonObject<AndonManage>();
            RT.Service.Resolve<AndonManageController>().AndonManageAddExp(andonManage);
            return andonManage;
        }
    }
}
