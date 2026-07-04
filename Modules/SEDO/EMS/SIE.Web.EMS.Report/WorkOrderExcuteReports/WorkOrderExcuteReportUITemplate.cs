using SIE.MetaModel.View;

namespace SIE.Web.EMS.Report.WorkOrderExcuteReports
{
    /// <summary>
    /// 工单执行统计报表
    /// </summary>
    public class WorkOrderExcuteReportUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块方法
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.EMS.Report.WorkOrderExcuteReports.Scripts.WorkOrderExcuteReportLayout");
            return rst;
        }
    }
}
