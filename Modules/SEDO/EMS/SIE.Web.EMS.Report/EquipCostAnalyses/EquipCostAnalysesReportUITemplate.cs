using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipCostAnalysesReportUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块方法
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.EMS.Report.EquipCostAnalyses.EquipCostAnalysesLayout");
            return rst;
        }
    }
}
