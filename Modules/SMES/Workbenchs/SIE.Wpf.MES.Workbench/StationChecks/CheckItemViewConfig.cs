using SIE.MES.Workbench.StationChecks;

namespace SIE.Wpf.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位物料点检视图配置
    /// </summary>
    internal class CheckItemViewConfig : WPFViewConfig<CheckItem>
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
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            View.Property(p => p.Item).HasLabel("物料编码");
            View.Property(p => p.Item.Name).HasLabel("物料名称");
            View.Property(p => p.DemandQty);
            View.Property(p => p.ArriveQty);
            View.Property(p => p.LackQty);
            View.Property(p => p.WarnQty);
            View.Property(p => p.State);
            View.Property(p => p.InRouteQty);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
