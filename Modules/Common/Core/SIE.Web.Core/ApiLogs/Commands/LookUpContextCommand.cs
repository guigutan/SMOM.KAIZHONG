using SIE.Web.Command;

namespace SIE.Web.Core.ApiLogs.Commands
{
    /// <summary>
    /// 查看报文命令
    /// </summary>
    public class LookUpContextCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
