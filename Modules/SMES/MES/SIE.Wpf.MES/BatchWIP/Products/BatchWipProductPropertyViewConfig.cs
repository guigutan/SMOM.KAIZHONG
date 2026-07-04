using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品属性视图配置
    /// </summary>
    internal class BatchWipProductPropertyViewConfig : WPFViewConfig<BatchWipProductProperty>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Value);
            View.Property(p => p.PropertyName);
        }
    }
}