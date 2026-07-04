using SIE.Security;
using SIE.Web.Command;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 提交命令
    /// </summary>
    [JsCommand("SIE.Web.DIST.SubmitCommand")]
    [AllowAnonymous]
    public class SubmitCommand : ViewCommand
    {
        /// <summary>
        /// 提交保存
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scop</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var goodIssueEntity = args.Data.ToJsonObject<GoodsIssueViewModel>();
            GoodsIssueViewModelContrller ctl = new GoodsIssueViewModelContrller();
            ctl.Submit(goodIssueEntity);
            return goodIssueEntity;
        }
    }
}
