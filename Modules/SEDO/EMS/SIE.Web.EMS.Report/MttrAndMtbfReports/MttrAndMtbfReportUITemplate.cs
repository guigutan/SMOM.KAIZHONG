using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Report.MttrAndMtbfReports
{
    /// <summary>
    /// MTTR/MTBF统计报表UITemplate
    /// </summary>
    public class MttrAndMtbfReportUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块方法
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.EMS.Report.MttrAndMtbfReports.Scripts.MttrAndMtbfReportLayout");
            return rst;
        }
    }
}
