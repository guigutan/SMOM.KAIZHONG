using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.ProductOrderBoms
{
    /// <summary>
    /// 生产订单BOM中间表视图配置
    /// </summary>
    public class ProductOrderBomInfViewConfig : WebViewConfig<ProductOrderBomInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.ItemCode);
                View.Property(p => p.SpecificationDesc);
                //View.Property(p => p.ReplateItemType);
                View.Property(p => p.MainMaterialCode);
                View.Property(p => p.ElementNo);
                View.Property(p => p.RequireQty);
                View.Property(p => p.ProcessTech);
                View.Property(p => p.Remark);
            }
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.ItemCode);
        }
    }
}
