using SIE.MES.Outsourcing;
using SIE.Web.Command;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 强制完成
    /// </summary>
    [JsCommand("SIE.Web.MES.Outsourcing.Commands.ForceCompleteCommand")]
    public class ForceCompleteCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null || args.SelectedIds.Length == 0)
            {
                return false;
            }
            
            RT.Service.Resolve<OutsourcingController>().ForceComplete(args.SelectedIds);

            return true;
        }
    }
}
