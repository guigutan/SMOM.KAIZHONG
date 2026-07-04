using SIE.MetaModel.View;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 排班表模板
    /// </summary>
    public class GoodsIssueTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Web.DIST.GoodsIssue.GoodsIssueLayout");
            return result;
        }
    }
}