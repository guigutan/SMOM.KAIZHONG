using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 模板保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", GroupType = CommandGroupType.Edit)]
    public class TemplateSaveCommand : DetailViewCommand
    {
        /// <summary>
        /// 判断是否可执行
        /// </summary>
        /// <param name="view">详细逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return view.Parent != null && (view.Parent.Current as Item) != null && view.Current != null && view.Current.IsDirty;
        }

        /// <summary>
        /// 命令执行代码块
        /// </summary>
        /// <param name="view">详细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var item = view.Parent.Current as Item;
            RF.Save(item.Template);
            RF.Save(item);
        }
    }
}
