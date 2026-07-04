using SIE.Domain;
using SIE.Items;
using SIE.Items.Events;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 物料 禁用命令
    /// </summary>
    [Command(ImageName = "Cancel", Label = "停用", ToolTip = "停用", GroupType = CommandGroupType.Business)]
    public class ItemDisableCommand : ListViewCommand
    {
        /// <summary>
        /// 子类重写此方法来返回是否可执行的逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            Item item = null;
            if (view != null && view.Current != null)
            {
                item = view.Current as Item;
            }

            return item != null && item.State != State.Disable;
        }

        /// <summary>
        /// 子类重写此方法来返回是否可执行的逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            Item item = null;
            if (view != null && view.Current != null)
            {
                item = view.Current as Item;

                if (CRT.MessageService.AskQuestion("确定禁用选中的资料?".L10N()))
                {
                    RT.EventBus.Publish(new HasItemStockEvent() { IetmId = item.Id });
                    item.State = State.Disable;
                    RF.Save(item);
                }
            }
        }
    }

    /// <summary>
    /// 物料 启用命令，把实体状态修改为可用
    /// </summary>
    [Command(ImageName = "Play", Label = "启用", ToolTip = "启用", GroupType = CommandGroupType.Business)]
    public class ItemEnableCommand : ListViewCommand
    {
        /// <summary>
        /// 子类重写此方法来返回是否可执行的逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            Item item = null;
            if (view != null && view.Current != null)
            {
                item = view.Current as Item;
            }

            return item != null && item.State != State.Enable;
        }

        /// <summary>
        /// 子类重写此方法来返回是否可执行的逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            Item item = null;
            if (view != null && view.Current != null)
            {
                item = view.Current as Item;

                if (CRT.MessageService.AskQuestion("确定启用选中的资料?".L10N()))
                {
                    item.State = State.Enable;
                    RF.Save(item);
                }
            }
        }
    }
}
