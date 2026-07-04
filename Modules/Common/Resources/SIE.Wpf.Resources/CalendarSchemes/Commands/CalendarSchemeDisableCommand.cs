using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 停用日历方案命令
    /// </summary>
    [Command(ImageName = "MinusCircleOutline", Label = "停用", GroupType = 60)]
    class CalendarSchemeDisableCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Count > 0 && view.SelectedEntities.OfType<CalendarScheme>().All(p => p.IsEnable == YesNo.Yes && p.IsDefault == YesNo.No);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (CRT.MessageService.AskQuestion("是否把选择的{0}条日历方案设置为停用状态?".L10nFormat(View.SelectedEntities.Count)))
            {
                var calendarSchemes = view.SelectedEntities.OfType<CalendarScheme>().ToList();
                RT.Service.Resolve<CalendarSchemeController>().DisableCalendarScheme(calendarSchemes);

                calendarSchemes.ForEach(x => { x.IsEnable = YesNo.No; x.PersistenceStatus = PersistenceStatus.Unchanged; });
            }
        }
    }
}
