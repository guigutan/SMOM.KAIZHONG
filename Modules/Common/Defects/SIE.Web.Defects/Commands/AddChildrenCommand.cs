using SIE.Web.Command;

namespace SIE.Web.Defects.Commands
{
    class AddChildrenCommand : ViewCommand<ViewArgs>
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
