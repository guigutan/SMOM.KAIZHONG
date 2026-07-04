using SIE.MES.WIP.Repairs;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP.TemporaryRepairs;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.MES.Wip.TemporaryRepairs.Commands
{
    /// <summary>
    /// 添加不良命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加不良", GroupType = CommandGroupType.Edit)]
    public class AddDefectCommand : ListViewCommand
    {
        /// <summary>
        /// 判断命令是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>命令能执行返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var parent = view.Relations["mainView"]?.Current as TemporaryRepairViewModel;
            return parent != null && parent.SubmitBarcode != null && !string.IsNullOrEmpty(parent.SubmitBarcode.Code);
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            (view.Relations["mainView"]?.Current as TemporaryRepairViewModel)?.AddDefect();
        }
    }

    /// <summary>
    /// 删除不良命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除不良", GroupType = CommandGroupType.Edit)]
    public class DeleteDefectCommand : ListDeleteCommand
    {
        /// <summary>
        /// 判断命令是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>命令能执行返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var parent = view.Relations["mainView"]?.Current as TemporaryRepairViewModel;
            return parent != null && view.Current != null && parent.SubmitBarcode != null && !string.IsNullOrEmpty(parent.SubmitBarcode.Code)
                && (view.Current as TemporaryRepairDefectViewModel)?.IsNewAdd == true;

        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (view.SelectedEntities.Count == 0)
            {
                CRT.MessageService.ShowError("请先选择删除行！！！".L10N());
                return;
            }
            foreach (var item in view.SelectedEntities)
            {
                TemporaryRepairDefectViewModel temporaryRepairDefectViewModel = item as TemporaryRepairDefectViewModel;

                if (temporaryRepairDefectViewModel != null)
                {
                    view.Data.Remove(temporaryRepairDefectViewModel);
                }
            }
        }
    }
}