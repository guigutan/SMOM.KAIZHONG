using SIE.MES.Workbench.StationChecks;

namespace SIE.Wpf.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位点检设备项视图配置
    /// </summary>
    internal class CheckEquipmentDetailViewConfig : WPFViewConfig<CheckEquipmentDetail>
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
            View.Property(p => p.Project).UsePagingLookUpEditor(e => e.DisplayMember = "Code").HasLabel("项目编码");
            View.Property(p => p.Project.Name).HasLabel("项目名称");
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
