using SIE.Items;
using SIE.Web.ERPInterface.DownloadManual.ProductBoms.Commands;

namespace SIE.Web.ERPInterface.DownloadManual.ProductBoms
{
    /// <summary>
    /// 产品BOM扩展视图
    /// </summary>
    public class ProductBomExtViewConfig : WebViewConfig<ProductBom>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(DlProductBomCommand).FullName);
        }
    }
}