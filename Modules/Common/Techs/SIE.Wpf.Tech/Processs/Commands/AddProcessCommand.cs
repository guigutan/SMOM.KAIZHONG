using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Wpf.Command;
using SIE.Tech.Processs.Event;

namespace SIE.Wpf.Tech.Processs.Commands
{
    /// <summary>
    /// 工序添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = CommandGroupType.Edit)]
    public class AddProcessCommand : ListAddCommand
    {
        /// <summary>
        /// 新实体创建后-提供扩展
        /// </summary>
        /// <param name="entity">新实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            Process process = entity as Process;
            entity.PropertyChanged += new ProcessPropertyChanged().PropertyChanged;
            process.Type = ProcessType.Pqc;
            base.OnItemCreated(entity);
        }
    }
}