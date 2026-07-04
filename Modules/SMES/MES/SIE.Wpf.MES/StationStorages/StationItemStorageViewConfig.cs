using SIE.MES.StationStorages;

namespace SIE.Wpf.MES.StationStorages
{
    /// <summary>
    /// 工位物料库存视图配置
    /// </summary>
    internal class StationItemStorageViewConfig : WPFViewConfig<StationItemStorage>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            using (View.DeclareBand(string.Empty))
            {
                View.Property(p => p.ItemCode);
                View.Property(p => p.ItemName);
                View.Property(p => p.SendingQty).UseListSetting(e => { e.HelpInfo = "已叫料未接收数量"; });
            }
            using (View.DeclareBand("库存", horizontalAlignment: 1))
            {
                View.Property(p => p.BudgetQty).UseListSetting(e => { e.HelpInfo = "工位已接收未上料数量"; });
                View.Property(p => p.ActStoreQty).UseListSetting(e => { e.HelpInfo = "工位已上料和已下料数量"; });
            }
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}