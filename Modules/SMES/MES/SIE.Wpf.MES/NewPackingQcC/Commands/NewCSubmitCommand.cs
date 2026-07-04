using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.NewPackingQcC.Commands
{
    /// <summary>
    /// 正常
    /// </summary>
    [Command(ImageName = "", Label = "正  常", ToolTip = "正  常", GroupType = CommandGroupType.Edit)]
    public  class NewCSubmitCommand : DetailViewCommand
    {
        public override void Execute(DetailLogicalView view)
        {
        }
    }
}
