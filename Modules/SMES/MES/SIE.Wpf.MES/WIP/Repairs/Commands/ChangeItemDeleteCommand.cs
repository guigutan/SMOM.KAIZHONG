using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.WIP.Repairs.Commands
{
    /// <summary>
    /// 删除换料视图数据
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", Location = CommandLocation.All, GroupType = 10)]
    public class ChangeItemDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 执行删除逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            if (view.SelectedEntities.Count == 0)
            {
                CRT.MessageService.ShowError("请先选择删除行！！！".L10N());
                return;
            }
            foreach (var item in view.SelectedEntities)
            {
                ChangeItemViewModel changeItemViewModel = item as ChangeItemViewModel;

                if (changeItemViewModel != null)
                {
                    view.Data.Remove(changeItemViewModel);

                    //总换料数量减少
                    changeItemViewModel.ProductAssemblyDetailViewModel.ComputeTotalChangeQty();
                }
            }
        }
    }
}
