using SIE.MetaModel.View;

namespace SIE.Web.Resources.WipResources
{
    /// <summary>
    /// 生产资源模板
    /// </summary>
    public class WipResourceTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Web.Resource.WipResource.WipResourceLayout");
            return result;
        }
    }
}