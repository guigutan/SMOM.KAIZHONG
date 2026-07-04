using SIE.Web.Command;

namespace SIE.Web.Items.Items.Commands
{
    class AddChildCommand : ViewCommand<ViewArgs>
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
