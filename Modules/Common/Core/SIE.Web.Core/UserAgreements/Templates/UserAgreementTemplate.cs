using SIE.MetaModel.View;

namespace SIE.Web.Core.UserAgreements.Templates
{
    class UserAgreementTemplate: CodeBlocksTemplate
    {
        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var result = base.DefineBlocks();
            result.Layout = new LayoutMeta("SIE.Core.layouts.UserAgreementsLayout");
            return result;
        }
    }
}
