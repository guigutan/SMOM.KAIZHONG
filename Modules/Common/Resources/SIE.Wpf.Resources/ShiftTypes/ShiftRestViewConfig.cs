using SIE.Resources.ShiftTypes;
using SIE.Wpf.Resources.ShiftTypes.Commands;

namespace SIE.Wpf.Resources.ShiftTypes
{
    /// <summary>
    /// 班次休息
    /// </summary>
    class ShiftRestViewConfig : WPFViewConfig<ShiftRest>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors().InlineEdit().UseDefaultCommands()
                .ReplaceCommands(WPFCommandNames.ListAdd, typeof(ShiftRestAddCommand))
                .ReplaceCommands(WPFCommandNames.ListDelete, typeof(ShiftRestDeleteCommand))
                .RemoveCommands(WPFCommandNames.ListSave, WPFCommandNames.ListCopy);
            View.Property(p => p.Type);
            View.Property(p => p.BeginTime).UseTimeEditor();
            View.Property(p => p.EndTime).UseTimeEditor();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
