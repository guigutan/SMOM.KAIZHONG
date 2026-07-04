using SIE.MES.ItemChecker;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemChecker.Commands
{
    /// <summary>
    /// 工装与产品关系启用命令
    /// </summary>
    public class CheckerItemEnableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<CheckerItemController>().EnableCheckerItem(args.ToList());
            return true;
        }
    }
}
