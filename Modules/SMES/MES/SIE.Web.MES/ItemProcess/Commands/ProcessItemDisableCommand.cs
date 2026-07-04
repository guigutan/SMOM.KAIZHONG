using SIE.MES.ItemLine;
using SIE.MES.ItemProcess;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemProcess.Commands
{
    /// <summary>
    /// 禁用产品与产线
    /// </summary>
    public class ProcessItemDisableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<ProcessItemController>().DisableProcessItem(args.ToList());
            return true;
        }
    }
}
