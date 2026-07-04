using SIE.Items;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    [JsCommand("SIE.Web.Items.Items.Commands.ProductGradeDeleteCommand")]
    public class ProductGradeDeleteCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<ItemController>().RemoveProductGrade(args.ToList());
            return "删除成功";
        }
    }
}
