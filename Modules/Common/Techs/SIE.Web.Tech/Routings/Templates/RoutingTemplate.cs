using SIE.MetaModel.View;

namespace SIE.Web.Tech
{
    /// <summary>
    /// 工艺路线模板
    /// </summary>
    class RoutingTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Tech.layouts.RoutingLayout");
            return result;
        }
    }
}