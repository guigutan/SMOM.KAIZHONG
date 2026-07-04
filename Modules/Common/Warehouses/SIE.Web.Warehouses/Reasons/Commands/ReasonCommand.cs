using SIE.Warehouses;
using SIE.Web.Command;

namespace SIE.Web.Warehouses.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    [JsCommand("SIE.Web.Warehouses.Commands.AddReasonCommand")]
    public class AddReasonCommand : ViewCommand
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<Reason>();
            data.Code = RT.Service.Resolve<ReasonController>().GetReasonCode();
            return data;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    [JsCommand("SIE.Web.Warehouses.Commands.DeleteReasonCommand")]
    public class DeleteReasonCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 初始化命令
    /// </summary>
    [JsCommand("SIE.Web.Warehouses.Commands.InitReasonCommand")]
    public class InitReasonCommand : ViewCommand
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<ReasonController>().InitReason();
            return true;
        }
    }
}
