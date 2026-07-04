using SIE.MetaModel.View;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
    /// 工治具台账自定义UI模版
    /// </summary>
    public class FixtureAccountModelUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块方法
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.Fixtures.Accounts.Scripts.FixtureAccountModelLayout");
            return rst;
        }
    }
}
