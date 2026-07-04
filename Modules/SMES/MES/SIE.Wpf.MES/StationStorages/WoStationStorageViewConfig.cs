using SIE.MES.StationStorages;

namespace SIE.Wpf.MES.StationStorages
{
    /// <summary>
    /// 工单工位库存视图配置
    /// </summary>
    internal class WoStationStorageViewConfig : WPFViewConfig<WoStationStorage>
    {
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-2, -8);
            View.Property(p => p.WorkOrderNo);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
