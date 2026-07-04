using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.WIP.NewPackages.Commands
{
    /// <summary>
    /// 采集重新开始命令
    /// </summary>
    [Command(ImageName = "Refresh",
        Label = "重新开始",
        ToolTip = "重新开始",
        GroupType = CommandGroupType.Edit)]
    public class NewPackageRestartCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as NewPackageViewModel) != null;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as NewPackageViewModel;
            vm.Reset(ResetType.CollectRestart);
            vm.FocuseBarcode();
        }
    }
}
