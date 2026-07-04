using SIE.Web.Command;

namespace SIE.Web.Items.Items.Commands
{
    class AddTreeCommand : ViewCommand<ViewArgs>
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
