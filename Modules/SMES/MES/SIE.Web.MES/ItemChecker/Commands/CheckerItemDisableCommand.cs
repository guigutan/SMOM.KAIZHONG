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
    /// 禁用检具与产品
    /// </summary>
    public class CheckerItemDisableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<CheckerItemController>().DisableCheckerItem(args.ToList());
            return true;
        }
    }
}
