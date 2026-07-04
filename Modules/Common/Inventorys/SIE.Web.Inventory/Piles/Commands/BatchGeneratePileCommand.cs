using SIE.Web.Command;

namespace SIE.Web.Inventory.Piles.Commands
{
    /// <summary>
    /// 批量生成命令
    /// </summary>
    public class BatchGeneratePileCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}