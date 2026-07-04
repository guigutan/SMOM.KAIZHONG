using SIE.MetaModel.View;

namespace SIE.Web.MES.RoutingSettings
{
    /// <summary>
    /// 产品工艺路线设置模板
    /// </summary>
    public class RoutingSettingsTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Web.MES.RoutingSettings.RoutingSettingsLayout");
            return result;
        }
    }
}