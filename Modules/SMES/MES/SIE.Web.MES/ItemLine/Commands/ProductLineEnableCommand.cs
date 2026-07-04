using SIE.MES.ItemLine;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemLine.Commands
{
    /// <summary>
    /// 启用产品与产线关系
    /// </summary>
    public class ProductLineEnableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<ProductLineController>().EnableProductLine(args.ToList());
            return true;
        }
    }
}
