using SIE.Domain;
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
    /// 物料与工序关系保存命令
    /// </summary>
    public class ProcessItemEnableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<ProcessItemController>().EnableProcessItem(args.ToList());
            return true;
        }
    }
}
