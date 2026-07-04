using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.BatchWIP.Repairs.Commands
{
    /// <summary>
    /// 暂存维修信息命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "暂存维修信息", Gestures = "Ctrl+S", GroupType = CommandGroupType.Edit)]
    public class SaveBatchRepairRecord : ListViewCommand
    {
        /// <summary>
        /// 判断保存命令能否执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        /// <returns>true可点击，false不可点击</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var model = view.Relations.Find("mainView")?.Data as BatchRepairViewModel;
            return view.Data != null && view.Data.Count > 0 && view.Data.IsDirty && model != null && model.CanSubmit();
        }

        /// <summary>
        /// 暂存维修信息命令执行方法
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var model = view.Relations.Find("mainView")?.Data as BatchRepairViewModel;
            model?.SaveRepairRecord();
            model?.BatchRepairDefectList?.MarkSaved();
            CRT.MessageService.ShowInstantMessage("保存成功！".L10N(), "提示".L10N(), 3);
        }
    }
}
