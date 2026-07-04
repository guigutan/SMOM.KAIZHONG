using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.ProductBoms
{
    /// <summary>
    /// 产品BOM明细中间表视图配置
    /// </summary>
    internal class ProductBomDetailInfViewConfig : WebViewConfig<ProductBomDetailInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.LossRate);
            View.Property(p => p.Remark);
            View.Property(p => p.UnitQty);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ProductBomCode);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ItemCode);
            View.Property(p => p.ProductBomCode);
        }
    }
}