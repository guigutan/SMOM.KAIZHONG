using SIE.MES.Outsourcing;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 添加入库记录
    /// </summary>
    public class InStockRecordAddCommand : ViewCommand
    {
        /// <summary>
        /// 添加入库记录
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var processingInStock = args.Data.ToJsonObject<ProcessingInStock>();
            processingInStock.SourceId = 0;
            processingInStock.State = OutsourcingDetailState.Created;
            return processingInStock;
        }
    }
}
