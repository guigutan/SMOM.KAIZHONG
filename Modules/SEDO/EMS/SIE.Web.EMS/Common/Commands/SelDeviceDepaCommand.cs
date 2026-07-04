using SIE.Web.Command;

namespace SIE.Web.EMS.Common.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    [JsCommand("SIE.Web.EMS.Common.Commands.SelDeviceDepaCommand")]
    public class SelDeviceDepaCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
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
