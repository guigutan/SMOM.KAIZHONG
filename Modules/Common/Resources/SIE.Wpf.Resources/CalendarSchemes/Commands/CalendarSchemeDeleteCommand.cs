using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 删除日历方案命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", GroupType = CommandGroupType.Edit)]
    class CalendarSchemeDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            bool isSelectedEntitiesAnyIsDefault =
                view.SelectedEntities.Cast<CalendarScheme>().Any(x => x.IsDefault == YesNo.Yes);

            return base.CanExecute(view) && (!isSelectedEntitiesAnyIsDefault);
        } 
    }
}
