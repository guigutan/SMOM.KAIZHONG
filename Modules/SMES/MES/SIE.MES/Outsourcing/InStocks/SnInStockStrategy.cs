using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Outsourcing.InStocks
{
    /// <summary>
    /// 单体条码委外入库策略
    /// </summary>
    public class SnInStockStrategy : InStockStrategy
    {
        /// <summary>
        /// 物料管控类型
        /// </summary>
        public RetrospectType RetrospectType
        {
            get
            {
                return RetrospectType.Single;
            }
        }
        /// <summary>
        /// 恢复生产
        /// </summary>
        /// <param name="outsourcingRequest">委外需求单</param>
        /// <param name="inStocks">a</param>
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
                    return DB.Query<WipProductVersion>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
                });

            if (wipProductVersions == null || !wipProductVersions.Any())
            {
                throw new ValidationException("提交委外入库失败，找不到在制品信息，可能原因是工单的产品【{0}】修改了【追溯方式】"
                    .L10nFormat(outsourcingRequest.WorkOrder.Product.Code));
            }

            OutsourcingWipExcutor outsourcingWipExcutor = new OutsourcingWipExcutor(wipProductVersions);
            outsourcingWipExcutor.ResumeProduction(outsourcingRequest.EndProcessId, outsourcingRequest.WorkOrderId);
        }

        /// <summary>
        /// 获取工序委外入库
        /// </summary>
        /// <param name="SnOrLot"></param>
        /// <param name="outsourcingRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<ProcessingInStock> GetProcessingInbounds(string SnOrLot, OutsourcingRequest outsourcingRequest)
        {
            if (outsourcingRequest is null)
            {
                throw new ArgumentNullException(nameof(outsourcingRequest));
            }
            var wipProductVersions = RT.Service.Resolve<OutsourcingController>().GetWipProductVersionsByKeyWord(outsourcingRequest.WorkOrderId, outsourcingRequest.BeginProcess, new List<string>() { SnOrLot });
            if (!wipProductVersions.Any())
            {
                throw new ValidationException("条码下一工序不是当前委外需求单的开始工序或在制品中不存在该条码，请检查".L10N());
            }
            if (wipProductVersions.Any(m => !m.IsOutsourcing))
            {
                throw new ValidationException("当前在制条码【{0}】不是委外中，请检查".L10nFormat(SnOrLot));
            }
            var inStocks = DB.Query<ProcessingInStock>().Where(x => SnOrLot == x.SN).ToList();
            var outStocks = DB.Query<ProcessingOutbound>().Where(x => SnOrLot == x.SN).ToList();
            if (!outStocks.Any())
            {
                throw new ValidationException("当前在制条码【{0}】不存在出库记录，无法入库".L10nFormat(SnOrLot));
            }
            var result = new EntityList<ProcessingInStock>();

            foreach (var productVersion in wipProductVersions)
            {
                var remainQty = productVersion.BatchQty - inStocks.Where(m => m.SourceId == productVersion.Id).Sum(m => m.Qty);//剩余数量
                if (remainQty < 0)
                {
                    remainQty = productVersion.BatchQty;
                }
                if (remainQty == 0)
                {
                    throw new ValidationException("当前在制条码【{0}】已入库完成，无需再次入库".L10nFormat(SnOrLot));
                }
                string sn = productVersion.Sn;
                if (sn.IsNullOrEmpty())
                {
                    sn = productVersion.KeyLabel;
                }
                var outStock = outStocks.FirstOrDefault(m => m.SN == sn);
                result.Add(new ProcessingInStock
                {
                    SourceId = productVersion.Id,
                    SN = sn,
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
