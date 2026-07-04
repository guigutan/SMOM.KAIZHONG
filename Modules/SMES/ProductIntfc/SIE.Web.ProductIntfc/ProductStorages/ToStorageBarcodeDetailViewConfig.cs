using SIE.ProductIntfc.ProductStorages;

namespace SIE.Web.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 待入库条码明细视图配置
    /// </summary>
    public class ToStorageBarcodeDetailViewConfig : WebViewConfig<ToStorageBarcodeDetail>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).Readonly().ShowInList(150);
                View.Property(p => p.Batch).Readonly().ShowInList(150);
                View.Property(p => p.FinishQty).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CollectDate).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
