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
    /// 禁用产品与产线
    /// </summary>
    public class ProductLineDisableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<ProductLineController>().DisableProductLine(args.ToList());
            return true;
        }
    }
}
