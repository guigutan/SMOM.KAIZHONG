using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 产品缺陷责任视图配置
    /// </summary>
    internal class WipDefectResponsibilityViewConfig : WebViewConfig<WipDefectResponsibility>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.ResponseCode);
            View.Property(p => p.ResponseDesc);
        }
    }
}