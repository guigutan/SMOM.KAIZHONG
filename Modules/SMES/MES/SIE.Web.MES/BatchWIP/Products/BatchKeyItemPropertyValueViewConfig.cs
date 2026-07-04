using SIE.MES.BatchWIP.Products;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 关键件属性值视图配置
    /// </summary>
    internal class BatchKeyItemPropertyValueViewConfig : WebViewConfig<BatchKeyItemPropertyValue>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseDefaultCommands();
            View.Property(p => p.Value).Readonly();
            View.Property(p => p.PropertyName).Readonly();
        }
    }
}
