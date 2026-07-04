using SIE.MetaModel.View;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 排班表模板
    /// </summary>
    public class ShiftScheduleTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleLayout");
            return result;
        }
    }
}