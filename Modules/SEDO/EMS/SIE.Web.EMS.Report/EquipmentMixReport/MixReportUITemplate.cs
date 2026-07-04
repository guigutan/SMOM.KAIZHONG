using SIE.MetaModel.View;

namespace SIE.Web.EMS.Report.EquipmentMixReport
{
    /// <summary>
    /// 模板基类
    /// </summary>
    public class MixReportUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块方法
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.EMS.Report.EquipmentMixReport.Scripts.EmsMixReportLayout");
            return rst;
        }
    }
}
