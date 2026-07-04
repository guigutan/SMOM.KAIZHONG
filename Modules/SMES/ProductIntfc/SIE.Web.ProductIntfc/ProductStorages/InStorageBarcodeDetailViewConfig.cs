using SIE.ProductIntfc.ProductStorages;

namespace SIE.Web.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 入库明细
    /// </summary>
    public class InStorageBarcodeDetailViewConfig : WebViewConfig<InStorageBarcodeDetail>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Qty).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Level).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.BatchBarcode).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.State).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
