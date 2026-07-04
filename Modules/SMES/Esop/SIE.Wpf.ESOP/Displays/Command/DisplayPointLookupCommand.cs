using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.ESOP.Displays.Command
{
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = 10, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class DisplayPointLookupCommand : LookupCommand
    {
        protected override void ShowDialog(ControlResult ui)
        {
            if (ClientRuntime.Workbench.ShowDialog(ui, delegate (SIE.View.Workbench.IDialogContent w)
            {
                w.Title = "选择{0}".L10nFormat(ui.MainView.Meta.Label.L10N());
            }) == 0)
            {
                OnAccept();
            }
            else
            {
                OnCancel();
            }
        }
    }
}
