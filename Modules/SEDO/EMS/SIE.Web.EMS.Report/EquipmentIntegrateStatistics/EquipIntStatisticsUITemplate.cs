using SIE.MetaModel.View;

namespace SIE.Web.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// ESD数据分析报表UITemplate
    /// </summary>
    public class EquipIntStatisticsUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块方法
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.EMS.Report.EquipmentIntegrateStatistics.Scripts.EquipIntStatisticsLayout");
            return rst;
        }
    }
}
