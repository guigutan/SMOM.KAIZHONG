using SIE.DIST;
using SIE.Domain;
using SIE.Web.Command;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 配送管理删除命令
    /// </summary>
    [JsCommand("SIE.Web.DIST.GoodsIssueDeleteCommand")]
    public class GoodsIssueDeleteCommand : ViewCommand<double>
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(double args, string scope)
        {
            var goodsIssue = RF.GetById<GoodsIssue>(args);
            if (goodsIssue == null) return false;
            goodsIssue.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(goodsIssue);
            return true;
        }
    }
}
