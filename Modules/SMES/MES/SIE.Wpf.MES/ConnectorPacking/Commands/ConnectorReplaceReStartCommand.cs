using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ConnectorPacking.Commands
{
    /// <summary>
    /// 重新开始
    /// </summary>
    [Command(ImageName = "", Label = "重新开始", ToolTip = "重新开始", GroupType = CommandGroupType.Edit)]
    public class ConnectorReplaceReStartCommand : DetailViewCommand
    {
        public override bool CanExecute(DetailLogicalView view)
        {
            return true;
        }

        public override void Execute(DetailLogicalView view)
        {
            var viewModel = view.Current as ConnectorSnPackingReplaceViewModel;
            viewModel.Reset(WIP.ResetType.CollectRestart);
        }
    }
}
