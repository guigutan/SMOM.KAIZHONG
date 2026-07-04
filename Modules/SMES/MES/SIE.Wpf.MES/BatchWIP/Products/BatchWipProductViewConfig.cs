using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次生产产品视图配置
    /// </summary>
    internal class BatchWipProductViewConfig : WPFViewConfig<BatchWipProduct>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Buid);
            View.Property(p => p.Model);
            View.Property(p => p.Qty);
            View.Property(p => p.ItemId);
        }
    }
}