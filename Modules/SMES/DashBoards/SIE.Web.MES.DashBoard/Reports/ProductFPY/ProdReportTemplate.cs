using SIE.MetaModel.View;

namespace SIE.Web.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 产品直通率报表模板
    /// </summary>
    public class ProdReportTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Web.MES.DashBoard.Reports.ProductFPY.Scripts.ProductReportLayout");
            return result;
        }
    }
}
