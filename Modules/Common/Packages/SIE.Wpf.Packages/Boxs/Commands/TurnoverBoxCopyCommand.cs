using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Packages.Boxs;
using SIE.Wpf.Command;

namespace SIE.Wpf.Packages.Boxs.Commands
{
    /// <summary>
    /// 复制并添加数据
    /// </summary>
    [Command(ImageName = "ContentCopy", Label = "复制添加", ToolTip = "复制并添加数据", Gestures = "Ctrl+Shift+C", Location = CommandLocation.All, GroupType = 10)]
    public class TurnoverBoxCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 复制周转箱对象
        /// </summary>
        /// <param name="entity">周转箱</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var box = entity as TurnoverBox;
            box.State = BoxState.Unused;
        }
    }
}