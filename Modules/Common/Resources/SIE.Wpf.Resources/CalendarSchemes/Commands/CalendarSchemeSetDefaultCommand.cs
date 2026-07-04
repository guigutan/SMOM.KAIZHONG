using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 设为缺省命令
    /// </summary>
    [Command(ImageName = "SetDefault", Label = "设为缺省", GroupType = 60)]
    class CalendarSchemeSetDefaultCommand : ListAddCommand
    {
        /// <summary>
        /// 是否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var calendarScheme = view.Current as CalendarScheme;
            return calendarScheme != null && calendarScheme.IsEnable == YesNo.Yes && calendarScheme.IsDefault == YesNo.No;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var calendarScheme = view.Current as CalendarScheme;
            if (CRT.MessageService.AskQuestion("是否将日历方案[{0}]设置为缺省?".L10nFormat(calendarScheme.Name)))
            {
                RT.Service.Resolve<CalendarSchemeController>().SetDefault(calendarScheme);
                view.QueryView.TryExecuteQuery();
            }
        }

        ///// <summary>
        ///// 创建新实体
        ///// </summary>
        ///// <returns>日历方案</returns>
        //protected override Entity CreateNewItem()
        //{
        //    var list = RT.Service.Resolve<CalendarSchemeController>().GetDefaultCalendar();
        //    if (list != null)
        //    {
        //        throw new ValidationException("系统已存在缺省的日历方案，不能重复新增".L10N());
        //    }
        //    else
        //    {
        //        var calendarScheme = base.CreateNewItem() as CalendarScheme;
        //        calendarScheme.IsDefault = YesNo.Yes;
        //        return calendarScheme;
        //    }
        //}
    }
}
