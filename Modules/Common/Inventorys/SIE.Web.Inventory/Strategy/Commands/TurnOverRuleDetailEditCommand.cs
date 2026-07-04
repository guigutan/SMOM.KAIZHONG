using SIE.Web.Command;

namespace SIE.Web.Inventory.Strategy.Commands
{
    public class TurnOverRuleDetailEditCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
