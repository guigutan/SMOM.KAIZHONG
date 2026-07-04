using SIE.Kit.MES.StationStorages;

namespace SIE.Web.Kit.MES.StationStorages
{
    /// <summary>
    /// 工位库存视图配置
    /// </summary>
    internal class StationStorageViewConfig : WebViewConfig<StationStorage>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.StationCode).Readonly().ShowInList(150);
            View.Property(p => p.StationName).Readonly().ShowInList(150);
            View.ChildrenProperty(p => p.WoStorageList).HasLabel("工位工单库存");
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}