using SIE.ProductIntfc.ProductStorages;

namespace SIE.Wpf.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 入库明细
    /// </summary>
    public class InStorageBarcodeDetailViewConfig : WPFViewConfig<InStorageBarcodeDetail>
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
                View.Property(p => p.Qty).Show(ShowInWhere.All);
                View.Property(p => p.Level).Show(ShowInWhere.All);
                View.Property(p => p.BatchBarcode).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
