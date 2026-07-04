using SIE.MetaModel.View;

namespace SIE.Web.MES.DashBoard.Reports.LineFPY
{
    public class LineReportTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Web.MES.DashBoard.Reports.LineFPY.Scripts.LineReportLayout");
            return result;
        }
    }
}
