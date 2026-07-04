using SIE.DIST;
using SIE.Items;
using SIE.Items.ViewModels;
using SIE.MetaModel;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", GroupType = 10)]
    public class GoodsIssuePropertyValueCommand : ListAddCommand
    {
        /// <summary>
        /// 能否执行修改
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>True:可以执行 False:不可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var parent = view.Parent?.Current as GoodsIssue;
            return view.CanAddItem() && parent != null && parent.Item != null;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var newEntity = view.CreateNewItem() as PropertyValueViewModel;
            if (view.Parent != null)
            {
                var parent = view.Parent.Current as GoodsIssue;
                newEntity.Type = parent.GetType();
                newEntity.ParentId = parent.Id;
                if (parent.Item != null)
                    newEntity.ItemId = parent.ItemId;
                var result = RT.Service.Resolve<ItemController>().GetItemPropertys(parent.ItemId);
                if (result == null || result.Count == 0)
                {
                    CRT.MessageService.ShowMessage("当前物料没有配置物料属性，请先在物料中配置".L10N());
                    return;
                }
            }

            view.Data.Add(newEntity);
            view.Current = newEntity;
            view.IsReadOnly = ReadOnlyStatus.None;
        }
    }
}
