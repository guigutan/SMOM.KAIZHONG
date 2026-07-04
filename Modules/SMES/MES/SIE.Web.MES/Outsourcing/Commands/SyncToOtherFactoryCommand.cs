using SIE.MES.Outsourcing;
using SIE.Web.Command;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 手动同步到其他工厂
    /// </summary>
    [JsCommand("SIE.Web.MES.Outsourcing.Commands.SyncToOtherFactoryCommand")]
    public class SyncToOtherFactoryCommand : ViewCommand
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

            RT.Service.Resolve<OutsourcingApiController>().ManualSyncOutboundsToOtherFactory(args.SelectedIds);

            return true;
        }
    }
}
