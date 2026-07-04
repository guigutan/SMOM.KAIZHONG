using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 接口下载异常报表模板
    /// </summary>
    public class DownloadExcReportTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Web.ERPInterface.Logs.DownloadExcReportLayout");
            return result;
        }
    }
}
