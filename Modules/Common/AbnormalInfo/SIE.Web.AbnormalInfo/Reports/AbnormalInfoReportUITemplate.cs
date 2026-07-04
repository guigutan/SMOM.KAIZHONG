using SIE.MetaModel.View;

namespace SIE.Web.AbnormalInfo.Reports
{
    /// <summary>
    /// 异常信息报表模板
    /// </summary>
    public class AbnormalInfoReportUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.AbnormalInfo.Reports.Scripts.AbnormalInfoReportLayout");
            return rst;
        }
    }
}
