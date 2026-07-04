using SIE.Web.Command;

namespace SIE.Web.Items.Items.Commands
{
    class InsertCommand : ViewCommand<ViewArgs>
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
