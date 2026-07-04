using SIE.Web.Command;

namespace SIE.Web.Inventory.Piles.Commands
{
    /// <summary>
    /// 垛表打印
    /// </summary>
    public class PilePrintCommand : ViewCommand
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
