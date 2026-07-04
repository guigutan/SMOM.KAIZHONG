using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 产品检验记录视图配置
    /// </summary>
    internal class WipProductInspectionItemViewConfig : WebViewConfig<WipProductInspectionItem>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion));
            View.ClearCommands();
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.LimitMax).Readonly();
            View.Property(p => p.LimitLow).Readonly();
            View.Property(p => p.InspectionValue).Readonly();
            View.Property(p => p.Result).Readonly();
            View.Property(p => p.Remarks).Readonly();
            View.Property(p => p.InspectByName).Readonly();
        }
    }
}
