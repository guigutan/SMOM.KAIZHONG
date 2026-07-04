using SIE.MES.Outsourcing;
using SIE.Web.Command;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 委外出库提交
    /// </summary>
    [JsCommand("SIE.Web.MES.Outsourcing.Commands.OutboundSubmitCommand")]
    public class OutboundSubmitCommand : ViewCommand
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
            
            RT.Service.Resolve<OutsourcingController>().SubmitOutbounds(args.SelectedIds);

            return true;
        }
    }
}
