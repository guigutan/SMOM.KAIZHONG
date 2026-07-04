using SIE.Web.Command;

namespace SIE.Web.LES.StockOrders.Commands
{
    public class StockOrderMergeIssuedAddCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
