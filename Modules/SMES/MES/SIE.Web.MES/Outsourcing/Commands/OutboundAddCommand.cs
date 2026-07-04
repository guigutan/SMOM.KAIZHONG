using SIE.MES.Outsourcing;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 添加可出库在制品
    /// </summary>
    [JsCommand("SIE.Web.MES.Outsourcing.Commands.OutboundAddCommand")]
    public class OutboundAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var outsourcingRequest = args.Data.ToJsonObject<OutsourcingRequest>();

            var returnMessage = RT.Service.Resolve<OutsourcingController>()
                .AddOutboundWips(outsourcingRequest.Id);

            return new
            {
                ReturnMessage = returnMessage,
            };
        }
    }
}
