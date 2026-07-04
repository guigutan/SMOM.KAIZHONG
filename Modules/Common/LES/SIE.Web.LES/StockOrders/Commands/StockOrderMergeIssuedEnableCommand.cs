using SIE.LES.StockOrders;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.LES.StockOrders.Commands
{
    public class StockOrderMergeIssuedEnableCommand : ViewCommand<double[]>
    {
        protected override object Excute(double[] args, string scope)
        {
            return RT.Service.Resolve<StockOrderMergeIssuedController>().StockOrderMergeIssuedEnable(args.ToList());
        }
    }
}
