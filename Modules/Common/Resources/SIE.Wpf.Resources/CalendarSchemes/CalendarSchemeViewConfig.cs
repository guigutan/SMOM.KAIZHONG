using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.CalendarSchemes.Commands;

namespace SIE.Wpf.Resources.CalendarSchemes
{
    /// <summary>
    /// 日历方案视图配置
    /// </summary>
    class CalendarSchemeViewConfig : WPFViewConfig<CalendarScheme>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(CalendarScheme.NameProperty);
            View.InlineEdit()
                .UseDefaultBehaviors()
                .UseDefaultCommands()
                .RemoveCommands(typeof(ListCopyCommand))
                .UseCommands(typeof(CalendarSchemeEnableCommand), typeof(CalendarSchemeDisableCommand),
                    typeof(CalendarSchemeSetDefaultCommand))
                .UseCommands(typeof(ListSaveCommand), typeof(ShowShiftTypeCommand), typeof(ShowHolidayCommand))
                .ReplaceCommands(typeof(ListDeleteCommand), typeof(CalendarSchemeDeleteCommand));
            View.UseLayoutSize(4, 6);
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.IsEnable).Readonly(true);
            View.Property(p => p.IsDefault).Readonly(true);
            View.ChildrenProperty(p => p.SchemeWeeks);
            View.ChildrenProperty(p => p.Excepts).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.IsEnable);
            View.Property(p => p.IsDefault);
        }
    }
}
