using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Wpf.Command;

namespace SIE.Wpf.Tech.Processs.Commands
{
    /// <summary>
    /// 工序复制命令
    /// </summary>
    [Command(ImageName = "ContentCopy", Label = "复制添加", ToolTip = "复制并添加数据", Gestures = "Ctrl+Shift+C", Location = CommandLocation.Toolbar, GroupType = CommandGroupType.Edit)]
    class CopyProcessCommand : ListCopyCommand
    {
        /// <summary>
        /// 实体创建后执行克隆操作
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var process = entity as Process;
            process.ReferenceTimes = 0;
        }
    }
}