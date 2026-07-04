using SIE.Warehouses;
using SIE.Web.Command;

namespace SIE.Web.Warehouses.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    [JsCommand("SIE.Web.Warehouses.Commands.AddBaseItemIOLimitCommand")]
    public class AddBaseItemIOLimitCommand : ViewCommand
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args != null)
            {
                var data = args.Data.ToJsonObject<BaseItemIoLimit>();
                return data;
            }
            else
                return null;
        }
    }
}
