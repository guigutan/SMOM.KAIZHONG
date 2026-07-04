using SIE.MetaModel.View;

namespace SIE.Web.MES.DashBoard.TeamManagement
{
    public class ScoreRecordTemplate : CodeBlocksTemplate
    {
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.MES.DashBoard.TeamManagement.ScoreRecordLayout");
            return rst;
        }
    }
}
