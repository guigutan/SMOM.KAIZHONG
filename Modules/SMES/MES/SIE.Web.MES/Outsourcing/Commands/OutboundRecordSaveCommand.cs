using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Outsourcing;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 委外入库保存命令
    /// </summary>
    [JsCommand("SIE.Web.MES.Outsourcing.Commands.OutboundRecordSaveCommand")]
    public class OutboundRecordSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前操作
        /// </summary>
        /// <param name="data">员工出勤统计集合</param>
        protected override void DoSave(EntityList data)
        {
            var processingInStocks = data as EntityList<ProcessingOutbound>;
            if (processingInStocks != null)
            {
                var processingInStockList = processingInStocks.Where(m => m.SN.IsNullOrEmpty() && m.LotNo.IsNullOrEmpty());
                if (processingInStockList.Any())
                {
                    if (processingInStockList.Any(m => m.Qty <= 0))
                    {
                        throw new ValidationException("请输入大于0的数量".L10N());
                    }
                    RT.Service.Resolve<OutsourcingController>().SaveProcessingInStocksReporWork(processingInStockList.AsEntityList());
                }
            }
        }
    }
}