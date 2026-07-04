using SIE.MES.Outsourcing;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 添加出库记录
    /// </summary>
    public class OutboundRecordAddCommand : ViewCommand
    {
        /// <summary>
        /// 添加出库记录
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var processingOutbound = args.Data.ToJsonObject<ProcessingOutbound>();
            processingOutbound.SourceId = 0;
            processingOutbound.State = OutsourcingDetailState.Created;
            return processingOutbound;
        }
    }
}
