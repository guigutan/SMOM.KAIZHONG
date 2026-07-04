using SIE.MES.Workbench.StationChecks;

namespace SIE.Wpf.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位点检设备视图配置
    /// </summary>
    internal class CheckEquipmentViewConfig : WPFViewConfig<CheckEquipment>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.State);
            View.Property(p => p.Period);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
