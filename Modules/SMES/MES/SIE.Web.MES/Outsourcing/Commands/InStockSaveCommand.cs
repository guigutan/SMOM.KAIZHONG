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
    [JsCommand("SIE.Web.MES.Outsourcing.Commands.InStockSaveCommand")]
    public class InStockSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前操作
        /// </summary>
        /// <param name="data">员工出勤统计集合</param>
        protected override void DoSave(EntityList data)
        {
            var processingInStocks = data as EntityList<ProcessingInStock>;
            if (processingInStocks == null)
            {
                throw new ValidationException("保存失败，不存在待保存的数据，请检查".L10N());
            }
            if (processingInStocks.Any(m => m.Qty <= 0))
            {
                throw new ValidationException("保存失败，存在入库数量小于等于0的记录，请检查".L10N());
            }
            if (processingInStocks.Any(m => m.NgQty < 0))
            {
                throw new ValidationException("保存失败，存在不合格数量小于0的记录，请检查".L10N());
            }
            if (processingInStocks.Any(m => m.PassQty < 0))
            {
                throw new ValidationException("保存失败，存在合格数量小于0的记录，请检查".L10N());
            }
            if (processingInStocks.Any(m => m.Qty != (m.NgQty + m.PassQty)))
            {
                throw new ValidationException("保存失败，存在不合格数量+合格数量不等于入库数量的记录，请检查".L10N());
            }

            RT.Service.Resolve<OutsourcingController>().SaveProcessingInStocks(processingInStocks);
        }
    }
}