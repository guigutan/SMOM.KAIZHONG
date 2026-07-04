using SIE.Web.Command;

namespace SIE.Web.LES.StockOrders.Commands
{
    public class StockOrderMergeTimesDeleteCommand : DeleteCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
