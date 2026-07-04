using SIE.MetaModel.View;
using SIE.Resources.CalendarSchemes;
using SIE.Web.Resources.CalendarSchemes.Commands;

namespace SIE.Web.Resources.CalendarSchemes
{
    /// <summary>
    /// 日历方案视图配置
    /// </summary>
    public class CalendarSchemeViewConfig : WebViewConfig<CalendarScheme>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseChildrenAsHorizontal();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.Copy);
            View.UseCommands(typeof(CalendarSchemeEnableCommand).FullName, typeof(CalendarSchemeDisableCommand).FullName, typeof(CalendarSchemeSetDefaultCommand).FullName, "SIE.Web.Resources.CalendarSchemes.Commands.ShowShiftTypeCommand", "SIE.Web.Resources.CalendarSchemes.Commands.ShowHolidayCommand");
            View.ReplaceCommands(WebCommandNames.Delete, "SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeDeleteCommand");
            View.Property(p => p.Name);
            View.Property(p => p.IsEnable).Readonly(true).DefaultValue(0);
            View.Property(p => p.IsDefault).Readonly(true).DefaultValue(0);
            //View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
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
