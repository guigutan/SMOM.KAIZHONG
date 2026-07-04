using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Inspects.Commands
{
    /// <summary>
    /// 缺陷代码删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity",
        Label = "删除",
        ToolTip = "删除数据",
        Gestures = "Delete",
        Location = CommandLocation.All,
        GroupType = CommandGroupType.Edit)]
    public class DefectItemDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 缺陷代码执行删除逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            var list = view.SelectedEntities.ToList();
            foreach (var item in list)
            {
                var defectItem = item as DefectItemViewModel;
                View.Data.Remove(defectItem);
            }

            view.RefreshControl();
        }
    }
}
