using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Commom;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Outsourcing.Outbounds
{
    /// <summary>
    /// 批次委外出库策略
    /// </summary>
    public class BatchOutboundStrategy : OutboundStrategy
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
        /// 提交API出库记录
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <param name="processingOutbounds"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>
        public virtual bool SaveOutboundProduct(OutsourcingRequest outsourcingRequest, EntityList<ProcessingOutbound> processingOutbounds)
        {
            if (outsourcingRequest is null)
            {
                throw new ArgumentNullException(nameof(outsourcingRequest));
            }
            var lotNos = processingOutbounds.Select(p => p.LotNo).ToList();

            var wipProductVersions = RT.Service.Resolve<OutsourcingController>().GetWipProductVersionsByLotNos(outsourcingRequest.WorkOrderId, outsourcingRequest.BeginProcess, lotNos);
            if (!wipProductVersions.Any())
            {
                throw new ValidationException("提交的条码存在下一工序不是当前委外需求单的开始工序或条码不存在，请检查".L10N());
            }
            if (wipProductVersions.Any(m => m.IsOutsourcing))
            {
                throw new ValidationException("存在至少一个条码处于委外中，无法提交，请检查".L10N());
            }
            var outQty = wipProductVersions.Sum(p => p.RemainQty);
            var outboundState = OutboundState.PartOutbound;

            if (outsourcingRequest.OutboundQty + outQty >= outsourcingRequest.RequestQty)
                outboundState = OutboundState.Finish;

            if (outsourcingRequest.OutsourcingState == OutsourcingState.NotStarted)
            {
                //更新出库量
                DB.Update<OutsourcingRequest>()
                    .Set(x => x.OutboundQty, x => x.OutboundQty + outQty)
                    .Set(x => x.OutsourcingState, OutsourcingState.Outsourcing)
                    .Set(x => x.OutboundState, outboundState)
                    .Where(x => x.Id == outsourcingRequest.Id)
                    .Execute();
            }
            else
            {
                //更新出库量
                DB.Update<OutsourcingRequest>()
                    .Set(x => x.OutboundQty, x => x.OutboundQty + outQty)
                    .Set(x => x.OutboundState, outboundState)
                    .Where(x => x.Id == outsourcingRequest.Id)
                    .Execute();
            }
            RF.Save(processingOutbounds);
            //RF.BatchInsert(processingOutbounds);

            //更新委外中为是，如果未提交之前删除，要把【委外中】更新回为否
            var wipProductVersionIds = processingOutbounds.Select(x => x.SourceId).ToList();

            wipProductVersionIds.SplitDataExecute(tempIds =>
            {
                DB.Update<BatchWipProductVersion>()
                  .Set(x => x.IsOutsourcing, true)
                  .Where(x => tempIds.Contains(x.Id))
                  .Execute();
            });
            outsourcingRequest.OutsourcingState = OutsourcingState.Outsourcing;
            return true;
        }

        /// <summary>
        /// 获取批次在制品
        /// </summary>
        /// <param name="SnOrLot"></param>
        /// <param name="outsourcingRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<ProcessingOutbound> GetProcessingOutbounds(string SnOrLot, OutsourcingRequest outsourcingRequest)
        {

            if (outsourcingRequest is null)
            {
                throw new ArgumentNullException(nameof(outsourcingRequest));
            }
            var wipProductVersions = RT.Service.Resolve<OutsourcingController>().GetWipProductVersionsByLotNos(outsourcingRequest.WorkOrderId,
                outsourcingRequest.BeginProcess, new List<string>() { SnOrLot });

            //没有符合出库条件的数据，则返回0
            if (!wipProductVersions.Any())
            {
                throw new ValidationException("条码【{0}】数据不存在或未开始生产或条码的下一工序不为委外需求单的开始工序，请检查".L10nFormat(SnOrLot));
            }
            if (wipProductVersions.Any(m => m.IsOutsourcing))
            {

                throw new ValidationException("条码【{0}】处于委外中，无法进行委外出库，请检查".L10nFormat(SnOrLot));
            }
            EntityList<ProcessingOutbound> processingOutbounds = new EntityList<ProcessingOutbound>();


            foreach (var productVersion in wipProductVersions)
            {
                processingOutbounds.Add(new ProcessingOutbound
                {
                    SourceId = productVersion.Id,
                    SN = productVersion.BatchNo,
                    OutsourcingRequestId = outsourcingRequest.Id,
                    Qty = productVersion.RemainQty,
                    State = OutsourcingDetailState.Created
                });
            }
            return processingOutbounds;
        }

        /// <summary>
        /// 添加委外出库的在制品
        /// </summary>
        /// <param name="outsourcingRequest">委外需求单</param>
        public int AddOutboundProducts(OutsourcingRequest outsourcingRequest)
        {
            if (outsourcingRequest is null)
            {
                throw new ArgumentNullException(nameof(outsourcingRequest));
            }

            var wipProductVersions = GetWipProductVersions(outsourcingRequest.WorkOrderId, outsourcingRequest.BeginProcess);

            //没有符合出库条件的数据，则返回0
            if (!wipProductVersions.Any())
            {
                return 0;
            }

            EntityList<ProcessingOutbound> outsourcingOutbounds
                = new EntityList<ProcessingOutbound>();

            //var requestQtyRemain = outsourcingRequest.RequestQty - outsourcingRequest.OutboundQty;
            var exsitedLotNoList = DB.Query<ProcessingOutbound>().Where(p => p.OutsourcingRequest.WorkOrderId == outsourcingRequest.WorkOrderId).Select(m => new { m.LotNo }).Distinct().ToList<string>();
            decimal outQty = 0;

            foreach (var productVersion in wipProductVersions)
            {
                if (exsitedLotNoList.Contains(productVersion.BatchNo))//已经存在的不在加
                {
                    continue;
                }
                //if (productVersion.Qty > requestQtyRemain)
                //{
                //    throw new ValidationException("委外需求单【{0}】在起始工序【{1}】的数量大于剩余需求量【{2}】"
                //        .L10nFormat(outsourcingRequest.NO, outsourcingRequest.BeginProcess.Name,
                //            requestQtyRemain));
                //}

                //requestQtyRemain -= productVersion.RemainQty;
                outQty += productVersion.RemainQty;

                outsourcingOutbounds.Add(new ProcessingOutbound
                {
                    SourceId = productVersion.Id,
                    LotNo = productVersion.BatchNo,
                    OutsourcingRequestId = outsourcingRequest.Id,
                    Qty = productVersion.RemainQty,// 240712 lyp 批次取值改为当前数量
                    State = OutsourcingDetailState.Created
                });

                //if (requestQtyRemain == 0)
                //{
                //    break;
                //}
            }

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(outsourcingOutbounds);
                //RF.BatchInsert(outsourcingOutbounds);
                DB.Update<OutsourcingRequest>()
                        .Set(x => x.OutboundQty, x => x.OutboundQty + outQty)
                        .Where(x => x.Id == outsourcingRequest.Id)
                        .Execute();
                trans.Complete();
            }

            return outsourcingOutbounds.Count;
        }

        /// <summary>
        /// 获取工单的下一工序为指定工单工序清单的生产产品版本
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="workOrderRoutingProcess"></param>
        /// <returns></returns>
        private EntityList<BatchWipProductVersion> GetWipProductVersions(double workOrderId,
            WorkOrderRoutingProcess workOrderRoutingProcess)
        {
            return DB.Query<BatchWipProductVersion>()
                .Where(x => x.WorkOrderId == workOrderId
                    && x.NextProcessId == workOrderRoutingProcess.ProcessId
                    && x.NextProcessIndex == workOrderRoutingProcess.Index
                    && !x.IsOutsourcing)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 删除委外出库
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <param name="outbounds"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void DeleteOutbounds(OutsourcingRequest outsourcingRequest, EntityList<ProcessingOutbound> outbounds)
        {
            if (outsourcingRequest is null)
            {
                throw new ArgumentNullException(nameof(outsourcingRequest));
            }

            if (outbounds is null)
            {
                throw new ArgumentNullException(nameof(outbounds));
            }

            var qty = outbounds.Sum(x => x.Qty);

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //更新出库量 不改变状态
                DB.Update<OutsourcingRequest>()
                    .Set(x => x.OutboundQty, x => x.OutboundQty - qty)
                    .Where(x => x.Id == outsourcingRequest.Id)
                    .Execute();

                outbounds.Select(x => x.Id).SplitDataExecute(tempIds =>
                {
                    DB.Delete<ProcessingOutbound>()
                        .Where(x => tempIds.Contains(x.Id))
                        .Execute();
                });

                trans.Complete();
            }
        }
        /// <summary>
        /// 提交出库
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <param name="processingOutbounds"></param>

        public void SubmitOutbounds(OutsourcingRequest outsourcingRequest, EntityList<ProcessingOutbound> processingOutbounds)
        {
            var outboundIds = processingOutbounds.Select(m => m.Id);
            var processingOutboundsLotNos = processingOutbounds.Select(m => m.LotNo);
            if (processingOutboundsLotNos.Any())
            {
                var exsitedSns = DB.Query<ProcessingOutbound>().Where(m => m.OutsourcingRequest.WorkOrderId == outsourcingRequest.WorkOrderId && m.State == OutsourcingDetailState.Submitted).Select(m =>new { m.LotNo }).Distinct().ToList<string>();//已提交的数据的SN
                foreach (var lotNo in processingOutboundsLotNos)
                {
                    if (!lotNo.IsNullOrEmpty()&&exsitedSns.Contains(lotNo))
                    {
                        throw new ValidationException("提交失败，在制品条码【{0}】已经是【已提交】的状态，请检查".L10nFormat(lotNo));
                    }
                }
            }

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var outboundState = OutboundState.PartOutbound;

                if (outsourcingRequest.OutboundQty >= outsourcingRequest.RequestQty)
                    outboundState = OutboundState.Finish;

                DB.Update<OutsourcingRequest>()
                        .Set(x => x.OutsourcingState, OutsourcingState.Outsourcing)
                        .Set(x => x.OutboundState, outboundState)
                        .Where(x => x.Id == outsourcingRequest.Id)
                        .Execute();

                var wipProductVersionIds = processingOutbounds.Select(x => x.SourceId).ToList();

                wipProductVersionIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<BatchWipProductVersion>()
                      .Set(x => x.IsOutsourcing, true)
                      .Where(x => tempIds.Contains(x.Id))
                      .Execute();
                });
                //更新出库提交的数据为已提交
                outboundIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<ProcessingOutbound>()
                        .Set(x => x.State, OutsourcingDetailState.Submitted)
                        .Where(x => tempIds.Contains(x.Id))
                        .Execute();
                });

                trans.Complete();
            }
        }
    }
}
