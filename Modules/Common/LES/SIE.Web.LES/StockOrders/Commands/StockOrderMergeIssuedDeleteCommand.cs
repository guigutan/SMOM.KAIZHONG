using SIE.LES.StockOrders;
using SIE.Web.Command;

namespace SIE.Web.LES.StockOrders.Commands
{
    public class StockOrderMergeIssuedDeleteCommand : DeleteCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
