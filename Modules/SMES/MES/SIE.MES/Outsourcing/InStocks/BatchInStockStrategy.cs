using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Outsourcing.Outbounds;
using SIE.MES.WIP.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Outsourcing.InStocks
{
    /// <summary>
    /// 单体条码委外入库策略
    /// </summary>
    public class BatchInStockStrategy : InStockStrategy
    {
        /// <summary>
        /// 物料管控类型
        /// </summary>
        public RetrospectType RetrospectType
        {
            get
            {
                return RetrospectType.Batch;
            }
        }
        /// <summary>
        /// 入库提交重新上线
        /// </summary>
        /// <param name="outsourcingRequest">工序委外需求单</param>
        /// <param name="inStocks">入库明细</param>
        public void ResumeProduction(OutsourcingRequest outsourcingRequest, List<ProcessingInStock> inStocks)
        {
            if (outsourcingRequest is null)
            {
                throw new ArgumentNullException(nameof(outsourcingRequest));
            }

            if (inStocks is null)
            {
                throw new ArgumentNullException(nameof(inStocks));
            }

            var wipProductVersionIds = inStocks.Select(x => x.SourceId).Distinct().ToList();

            var wipProductVersions =
                wipProductVersionIds.SplitContains(tempIds =>
                {
                    return DB.Query<BatchWipProductVersion>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
                });

            if (wipProductVersions == null || !wipProductVersions.Any())
            {
                throw new ValidationException("提交委外入库失败，找不到在制品信息，可能原因是工单的产品【{0}】修改了【追溯方式】"
                    .L10nFormat(outsourcingRequest.WorkOrder.Product.Code));
            }
            BatchOutsourcingWipExcutor outsourcingWipExcutor = new BatchOutsourcingWipExcutor(wipProductVersions);
            outsourcingWipExcutor.ResumeProduction(outsourcingRequest.EndProcessId, outsourcingRequest.WorkOrderId);
        }
        /// <summary>
        /// 获取在制品入库信息
        /// </summary>
        /// <param name="SnOrLot"></param>
        /// <param name="outsourcingRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<ProcessingInStock> GetProcessingInbounds(string SnOrLot, OutsourcingRequest outsourcingRequest)
        {
            if (outsourcingRequest is null)
            {
                throw new ArgumentNullException(nameof(outsourcingRequest));
            }
            EntityList<BatchWipProductVersion> wipProductVersions = RT.Service.Resolve<OutsourcingController>().GetWipProductVersionsByLotNos(outsourcingRequest.WorkOrderId, outsourcingRequest.BeginProcess, new List<string>() { SnOrLot });
            if (!wipProductVersions.Any())
            {
                throw new ValidationException("条码下一工序不是当前委外需求单的开始工序或在制品中不存在该条码，请检查".L10N());
            }
            if (wipProductVersions.Any(m => !m.IsOutsourcing))
            {
                throw new ValidationException("当前在制条码【{0}】不是委外中，请检查".L10nFormat(SnOrLot));
            }
            var inStocks = DB.Query<ProcessingInStock>().Where(x => SnOrLot == x.LotNo).ToList();
            var outStocks = DB.Query<ProcessingOutbound>().Where(x => SnOrLot == x.LotNo).ToList();
            if (!outStocks.Any())
            {
                throw new ValidationException("当前在制条码【{0}】不存在出库记录，无法入库".L10nFormat(SnOrLot));
            }
            var result = new EntityList<ProcessingInStock>();

            foreach (var productVersion in wipProductVersions)
            {
                var remainQty = productVersion.RemainQty - inStocks.Where(m => m.SourceId == productVersion.Id).Sum(m => m.Qty);//剩余数量
                if (remainQty < 0)
                {
                    remainQty = productVersion.RemainQty;
                }
                if (remainQty == 0)
                {
                    throw new ValidationException("当前在制条码【{0}】已入库完成，无需再次入库".L10nFormat(SnOrLot));
                }
                var outStock = outStocks.FirstOrDefault(m => m.LotNo == productVersion.BatchNo);
                result.Add(new ProcessingInStock
                {
                    SourceId = productVersion.Id,
                    SN = productVersion.BatchNo,
                    OutsourcingRequestId = outsourcingRequest.Id,
                    Qty = remainQty,
                    State = OutsourcingDetailState.Created,
                    OutboundId = outStock != null ? outStock.Id : 0,
                });
            }
            return result;
        }
    }
}
