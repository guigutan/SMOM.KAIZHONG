using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Resources.CalendarSchemes.Commands;

namespace SIE.Wpf.Resources.CalendarSchemes
{
    /// <summary>
    /// 周方案视图配置
    /// </summary>
    class CalendarSchemeWeekViewConfig : WPFViewConfig<CalendarSchemeWeek>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit().UseDefaultBehaviors()
                   .UseCommands(typeof(CalendarSchemeWeekAddCommand), typeof(CalendarSchemeWeekEditCommand), typeof(CalendarSchemeWeekDeleteCommand));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.ShiftType).HasLabel("班制");
            View.Property(p => p.ActiveDate).UseDateEditor();
            View.Property(p => p.Mon).UseCheckEditor();
            View.Property(p => p.Tue).UseCheckEditor();
            View.Property(p => p.Wed).UseCheckEditor();
            View.Property(p => p.Thu).UseCheckEditor();
            View.Property(p => p.Fri).UseCheckEditor();
            View.Property(p => p.Sat).UseCheckEditor();
            View.Property(p => p.Sun).UseCheckEditor();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
