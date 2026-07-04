using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.Wpf.Common.Sort;

namespace SIE.Wpf.MES.LoadItems.Commands
{
    /// <summary>
    /// 上移命令
    /// </summary>
    [Command(ImageName = "ArrowLongUp", Label = "上移", ToolTip = "当前行数据上移", GroupType = CommandGroupType.Business)]
    public class CusMoveUpCommand : MoveUpCommand
    {
        /// <summary>
        /// 移动并执行保存
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
            var itemList = view.Data as EntityList<LoadItem>;
            RF.Save(itemList);
        }
    }

    /// <summary>
    /// 下移命令
    /// </summary>
    [Command(ImageName = "ArrowLongDown", Label = "下移", ToolTip = "当前行数据下移", GroupType = CommandGroupType.Business)]
    public class CusMoveDownCommand : MoveDownCommand
    {
        /// <summary>
        /// 移动并执行保存
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
            var itemList = view.Data as EntityList<LoadItem>;
            RF.Save(itemList);
        }
    }

    /// <summary>
    /// 置顶命令
    /// </summary>
    [Command(ImageName = "AlignTop", Label = "置顶", ToolTip = "当前行数据移到顶部", GroupType = CommandGroupType.Business)]
    public class CusMoveTopCommand : MoveTopCommand
    {
        /// <summary>
        /// 移动并执行保存
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
            var itemList = view.Data as EntityList<LoadItem>;
            RF.Save(itemList);
        }
    }

    /// <summary>
    /// 置底命令
    /// </summary>
    [Command(ImageName = "AlignBottom", Label = "置底", ToolTip = "当前行数据移到底部", GroupType = CommandGroupType.Business)]
    public class CusMoveBottomCommand : MoveBottomCommand
    {
        /// <summary>
        /// 移动并执行保存
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
            var itemList = view.Data as EntityList<LoadItem>;
            RF.Save(itemList);
        }
    }
}
