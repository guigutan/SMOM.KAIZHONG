using SIE.Domain;
using SIE.Resources.ShiftTypes;
using SIE.Wpf.Resources.ShiftTypes.Commands;

namespace SIE.Wpf.Resources.ShiftTypes
{
    /// <summary>
    /// 班制视图配置
    /// </summary>
    class ShiftTypeViewConfig : WPFViewConfig<ShiftType>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors().InlineEdit().UseDefaultCommands();
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(SetDefaultCommand));
            View.ReplaceCommands(WPFCommandNames.ListSave, typeof(ShiftTypeSaveCommand))
                .UseCommands(typeof(ShowCalendarCommand));
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.IsDefault).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.ShiftList);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 下拉视图配
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
