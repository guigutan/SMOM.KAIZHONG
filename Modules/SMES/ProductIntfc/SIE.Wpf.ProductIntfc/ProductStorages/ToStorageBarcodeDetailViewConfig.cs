using SIE.ProductIntfc.ProductStorages;

namespace SIE.Wpf.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库条码明细视图配置
    /// </summary>
    public class ToStorageBarcodeDetailViewConfig : WPFViewConfig<ToStorageBarcodeDetail>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).Show(ShowInWhere.All);
                View.Property(p => p.Batch).Show(ShowInWhere.All);
                View.Property(p => p.FinishQty).Show(ShowInWhere.All);
                View.Property(p => p.CollectDate).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}