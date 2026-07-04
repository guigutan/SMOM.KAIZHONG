using SIE.Web.Command;

namespace SIE.Web.EMS.Tpms.Commands
{
    /// <summary>
    /// 查看图片命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Tpms.Commands.LookImageCommand")]
    public class LookImageCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 查看图片
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
