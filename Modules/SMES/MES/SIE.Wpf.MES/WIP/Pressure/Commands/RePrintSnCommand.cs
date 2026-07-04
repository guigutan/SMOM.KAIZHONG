using SIE.Common.Prints;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WIP.Pressure;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.MES.WIP.Pressure
{
    /// <summary>
    /// SN补打 命令
    /// </summary>
    [Command(ImageName = "PrintData", Label = "打印", ToolTip = "打印", GroupType = CommandGroupType.Edit)]
    public class RePrintSnCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var sn = view.Current as WipPressureSn;
            var vm = view.Relations.Find("mainView")?.Current as PressureViewModel;
            vm?.PrintSn(new Domain.EntityList<WipPressureSn>() { sn });
        }
    }

}
