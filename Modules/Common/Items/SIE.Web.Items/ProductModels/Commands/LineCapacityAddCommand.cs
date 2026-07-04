using SIE.Web.Command;

namespace SIE.Web.Items.ProductModels.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    public class LineCapacityAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
