using SIE.Web.Command;

namespace SIE.Web.LES.StockOrders.Commands
{
    public class StockOrderMergeIssuedEditCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
