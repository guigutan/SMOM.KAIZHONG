using SIE.MetaModel.View;

namespace SIE.Web.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 模板
    /// </summary>
    public class WorkOrderReachTemplate : CodeBlocksTemplate
    {
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.MES.DashBoard.WorkOrderReachs.WoReachLayout");
            return rst;
        }
    }
}
