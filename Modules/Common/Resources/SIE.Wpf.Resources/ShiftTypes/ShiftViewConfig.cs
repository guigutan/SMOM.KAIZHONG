using SIE.Domain;
using SIE.Resources.ShiftTypes;
using SIE.Wpf.Resources.ShiftTypes.Commands;

namespace SIE.Wpf.Resources.ShiftTypes
{
    /// <summary>
    /// 班次视图配置
    /// </summary>
    class ShiftViewConfig : WPFViewConfig<Shift>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors().InlineEdit().UseDefaultCommands()
                .ReplaceCommands(WPFCommandNames.ListAdd, typeof(ShiftAddCommand))
                .ReplaceCommands(WPFCommandNames.ListDelete, typeof(ShiftDeleteCommand))
                .RemoveCommands(WPFCommandNames.ListSave, WPFCommandNames.ListCopy);
            View.UseChildrenAsHorizontal(true);
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.BeginTime).UseTimeEditor();
            View.Property(p => p.EndTime).UseTimeEditor();
            View.Property(p => p.IsOverDay).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.ShiftRestList);
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.BeginTime).UseTimeEditor();
            View.Property(p => p.EndTime).UseTimeEditor();
        }
    }
}
