using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Wpf.Command;
using SIE.Tech.Processs.Event;

namespace SIE.Wpf.Tech.Processs.Commands
{
    /// <summary>
    /// 工序修改命令
    /// </summary>
    [Command(Label = "修改",
    GroupType = CommandGroupType.Edit,
    ImageName = "EditEntity",
    Location = CommandLocation.All,
    Gestures = "Ctrl+Shift+E")]
    public class EditProcessCommand : ListEditCommand
    {
        /// <summary>
        /// 能否执行修改
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var ct = view.Current as Process;

            if (ct != null && ct.Id != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取实体（如果是行内编辑，则在当前页面编辑，否则克隆当前实体，然后弹出标签页）
        /// </summary>
        /// <returns>获取到的实体</returns>
        protected override Entity GetEditEntity()
        {
            Entity entity = base.GetEditEntity();
            var eventChange = new ProcessPropertyChanged();
            entity.PropertyChanged += eventChange.PropertyChanged;

            return entity;
        }
    }
}