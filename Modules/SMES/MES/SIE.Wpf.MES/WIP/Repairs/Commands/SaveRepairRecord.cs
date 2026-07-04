using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.WIP.Repairs.Commands
{
    /// <summary>
    /// 暂存维修信息命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "暂存维修信息", Gestures = "Ctrl+S", GroupType = 10)]
    public class SaveRepairRecord : DetailViewCommand
    {
        /// <summary>
        /// 判断保存命令能否执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        /// <returns>true可点击，false不可点击</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var model = view.Current as RepairViewModel;
            return model != null && model.CanSubmit() && model.RepairDefectList.IsDirty;
        }

        /// <summary>
        /// 暂存维修信息命令执行方法
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = view.Current.CastTo<RepairViewModel>();
            model.SaveRepairRecord();
            model.RepairDefectList.MarkSaved();
            CRT.MessageService.ShowInstantMessage("保存成功！".L10N(), "提示".L10N(), 3);
        }
    }
}
