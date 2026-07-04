using SIE.LES.StockOrders.Service;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.LES.StockOrders.Commands
{
    /// <summary>
    /// 下发命令
    /// </summary>
    public class IssuedStockOrderCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<StockOrderService>().IssuedStockOrders(args.ToList());
            return true;
        }
    }
}
