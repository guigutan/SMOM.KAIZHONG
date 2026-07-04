using SIE.Wpf.Command;

namespace SIE.Wpf.MES.WIP.Repairs.Commands
{
    /// <summary>
    /// 维修提交
    /// </summary>
    [Command(ImageName = "SettingFinish", Label = "提交", ToolTip = "提交", GroupType = CommandGroupType.Edit)]
    public class RepairSubmitCommand : DetailViewCommand
    {
        /// <summary>
        /// 判断命令是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>命令能执行返回true，否则返回false</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var repairViewModel = view.Current as RepairViewModel;
            if (repairViewModel == null)
            {
                return false;
            }
            if (!repairViewModel.CanSubmit())
            {
                return false;
            }

            return repairViewModel.HasOptionalPath;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var currentModel = view.Current as RepairViewModel;
            if (currentModel != null)
            {
                currentModel.SaveRepairRecord();
                currentModel.RepairSubmit();
            }
        }
    }
}