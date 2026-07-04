using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.Storages.Commands
{
    /// <summary>
    /// 工位货区从表修改按钮
    /// </summary>
    [Command(Label = "修改", GroupType = CommandGroupType.Edit, ImageName = "EditEntity", Location = CommandLocation.All, Gestures = "Ctrl+Shift+E")]
    public class EditStorageAreaExtCommand : ListEditCommand
    {
        /// <summary>
        /// 工位货区从表修改按钮执行方法
        /// </summary>
        /// <param name="view">实体</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
            if (view.Parent.Current.PersistenceStatus == PersistenceStatus.Unchanged)
                view.Parent.Current.PersistenceStatus = PersistenceStatus.Modified;
        }
    }
}