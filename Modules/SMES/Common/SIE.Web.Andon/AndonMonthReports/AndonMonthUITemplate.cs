using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.AndonMonthReports
{
    /// <summary>
    /// 安灯统计报表模板
    /// </summary>
   public class AndonMonthUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块方法
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.Andon.AndonMonthReports.Scripts.AndonMonthReportLayout");
            return rst;
        }
    }
}
