using SIE.Common.Configs;
using SIE.Core.Common;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Outsourcing.Configs;
using SIE.MES.Outsourcing.InStocks;
using SIE.MES.Outsourcing.Outbounds;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 委外控制器
    /// </summary>
    public class OutsourcingController : DomainController
    {
        /// <summary>
        /// 获取委外报工配置项
        /// </summary>
        /// <returns></returns>
        public virtual OutsourcingReportConfigValue GetOutsourcingReportConfig()
        {
            var config = ConfigService.GetConfig(new OutsourcingReportConfig(), typeof(OutsourcingRequest));
            if(config == null)
                throw new ValidationException("委外报工配置项不存在");
            return config;
        }

        #region 发货确认明细

        /// <summary>
        /// 取消委外发货明细是否确认
        /// </summary>
        public virtual void DeleteOutboundConfirmSnDetail(List<double> ids)
        {
            var snDtls = GetOutboundConfirmSnDetailsByIds(ids);
            var processingOutboundIds = snDtls.Select(p => p.ProcessingOutboundId).Distinct().ToList();
            snDtls.ForEach(item => item.PersistenceStatus = PersistenceStatus.Deleted);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(snDtls);
                DB.Update<ProcessingOutbound>().Set(p => p.IsConfirm, false).Where(p => processingOutboundIds.Contains(p.Id)).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据Id获取明细
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<OutboundConfirmSnDetail> GetOutboundConfirmSnDetailsByIds(List<double> ids)
        {
            var list = ids.SplitContains(i =>
            {
                return Query<OutboundConfirmSnDetail>().Where(p => i.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            return list;
        }

        /// <summary>
        /// 根据唯一码获取发货确认
        /// </summary>
        /// <param name="zuids"></param>
        /// <returns></returns>
        public virtual EntityList<OutboundConfirmDetail> GetOutboundConfirmDetailsByZuid(List<string> zuids)
        {
            var list = zuids.SplitContains(ids =>
            {
                return Query<OutboundConfirmDetail>().Where(p => ids.Contains(p.Zuid)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 发货确认明细查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<OutboundConfirmDetail> CriteriaOutboundConfirmDetail(OutboundConfirmDetailCriteria criteria)
        {
            var q = DB.Query<OutboundConfirmDetail>("con");

            if (!criteria.FlowNo.IsNullOrEmpty())
                q.Where(p => p.FlowNo.Contains(criteria.FlowNo));
            if (!criteria.InitiatorFactory.IsNullOrEmpty())
                q.Where(p => p.InitiatorFactory.Contains(criteria.InitiatorFactory));
            if (!criteria.OutFactory.IsNullOrEmpty())
                q.Where(p => p.OutFactory.Contains(criteria.OutFactory));
            if (criteria.State != null)
                q.Where(p => p.State == criteria.State);
            if (criteria.DeliveryDate.BeginValue != null)
                q.Where(p => p.CreateDate >= criteria.DeliveryDate.BeginValue.Value);
            if (criteria.DeliveryDate.EndValue != null)
                q.Where(p => p.CreateDate <= criteria.DeliveryDate.EndValue.Value);

            Expression<Func<OutboundConfirmSnDetail, ProcessingOutbound, OutsourcingRequest, WorkOrder, Item, bool>> snexp = null;

            string snDelSql = @"select *
                                from OUTBOUND_CONFIRM_SN_DTL 
                                inner join PROC_OUT_OUTBOUND on PROC_OUT_OUTBOUND.id = OUTBOUND_CONFIRM_SN_DTL.Processing_Outbound_Id and PROC_OUT_OUTBOUND.is_phantom = 0              --委外发货明细
                                inner join OUT_REQUEST on OUT_REQUEST.id = PROC_OUT_OUTBOUND.Outsourcing_Request_Id and OUT_REQUEST.is_phantom = 0  --关联委外需求单
                                inner join WO on WO.id = OUT_REQUEST.Work_Order_Id and WO.is_phantom = 0                --关联工单
                                inner join ITEM on ITEM.id = wo.Product_Id and ITEM.is_phantom = 0                      --关联物料
                                where OUTBOUND_CONFIRM_SN_DTL.is_phantom = 0 AND OUTBOUND_CONFIRM_SN_DTL.inv_org_id = " + RT.InvOrg + @" and con.id = OUTBOUND_CONFIRM_SN_DTL.Outbound_Confirm_Detail_Id";
            bool isCriteriaDtl = false;
            if (!criteria.Sn.IsNullOrEmpty())
            {
                isCriteriaDtl = true;
                var symbol = "=";
                if (criteria.Sn.Contains("%"))
                    symbol = "like";
                snDelSql += $" and PROC_OUT_OUTBOUND.sn {symbol} '{criteria.Sn}'";
            }
            if (!criteria.ItemCode.IsNullOrEmpty())
            {
                isCriteriaDtl = true;
                var symbol = "=";
                if (criteria.ItemCode.Contains("%"))
                    symbol = "like";
                snDelSql += $" and ITEM.code {symbol} '{criteria.ItemCode}'";
            }
            if (!criteria.ItemName.IsNullOrEmpty())
            {
                isCriteriaDtl = true;
                var symbol = "=";
                if (criteria.ItemName.Contains("%"))
                    symbol = "like";
                snDelSql += $" and ITEM.name {symbol} '{criteria.ItemName}'";
            }
            if (isCriteriaDtl == true)
                q.Where(p => p.SQL<bool>($"exists({snDelSql})"));

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 选择添加工序标签明细
        /// </summary>
        /// <param name="snDetails"></param>
        public virtual void SelectAddOutboundConfirmSnDetail(List<OutboundConfirmSnDetail> snDetails)
        {
            EntityList<OutboundConfirmSnDetail> details = new EntityList<OutboundConfirmSnDetail>();
            var PprocessingOutboundIds = snDetails.Select(p => p.ProcessingOutboundId).Distinct().ToList();
            foreach (var snDetail in snDetails)
            {
                OutboundConfirmSnDetail detail = new OutboundConfirmSnDetail();
                detail.OutboundConfirmDetailId = snDetail.OutboundConfirmDetailId;
                detail.ProcessingOutboundId = snDetail.ProcessingOutboundId;
                detail.PersistenceStatus = PersistenceStatus.New;
                details.Add(detail);
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(details);
                DB.Update<ProcessingOutbound>().Set(p => p.IsConfirm, true).Where(p => PprocessingOutboundIds.Contains(p.Id)).Execute();
                tran.Complete();
            }
        }

        #endregion

        public virtual EntityList<ProcessingOutboundSelect> CriteriaProcessingOutboundSelect(ProcessingOutboundSelectCriteria criteria)
        {
            var q = Query<ProcessingOutboundSelect>();
            if (!criteria.Sn.IsNullOrEmpty())
                q.Where(p => p.SN.Contains(criteria.Sn));
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据Id获取委外需求单报工记录
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual EntityList<OutsourcingReportLog> GetOutsourcingReportLogsByIds(List<double> Ids)
        {
            var list = Ids.SplitContains(items =>
            {
                return Query<OutsourcingReportLog>().Where(p => items.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据标签号获取委外收货明细
        /// </summary>
        /// <param name="sns"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessingInStock> GetProcessingInStocksBySns(List<string> sns)
        {
            var list = sns.SplitContains(temp =>
            {
                return Query<ProcessingInStock>().Where(p => temp.Contains(p.SN)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据Id获取委外需求单收货列表
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessingInStock> GetProcessingInStocksById(List<double> Ids)
        {
            var list = Ids.SplitContains(items =>
            {
                return Query<ProcessingInStock>().Where(p => items.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据标签获取委外发货明细
        /// </summary>
        /// <param name="sns"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessingOutbound> GetProcessingOutboundsBySns(List<string> sns)
        {
            var list = sns.SplitContains(temp =>
            {
                return Query<ProcessingOutbound>().Where(p => temp.Contains(p.SN)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据Id获取委外需求单发货列表
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessingOutbound> GetProcessingOutboundsByIds(List<double> Ids)
        {
            var list = Ids.SplitContains(items =>
            {
                return Query<ProcessingOutbound>().Where(p => items.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据需求单号获取委外需求单
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public virtual OutsourcingRequest GetOutsourcingRequestByNo(string no)
        {
            var first = Query<OutsourcingRequest>().Where(p => p.NO == no).FirstOrDefault(new EagerLoadOptions().LoadWith(OutsourcingRequest.BeginProcessProperty).LoadWithViewProperty());
            return first;
        }

        /// <summary>
        /// 添加可入库委外在制品
        /// </summary>
        /// <param name="outsourcingId">委外需求单Id</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public virtual string AddInStockWips(double outsourcingId)
        {
            OutsourcingRequest outsourcingRequest
               = RF.GetById<OutsourcingRequest>(outsourcingId, new EagerLoadOptions().LoadWithViewProperty());
            if (outsourcingRequest == null)
            {
                throw new EntityNotFoundException(typeof(OutsourcingRequest), outsourcingId);
            }

            var outbounds = Query<ProcessingOutbound>()
                .Where(x => x.OutsourcingRequestId == outsourcingId && x.State == OutsourcingDetailState.Submitted).ToList();

            //别的单的委外入库            
            var outboundIds = outbounds.Select(x => (double?)x.Id).Distinct().ToList();
            var otherInStocks = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingInStock>()
                    .Where(x => tempIds.Contains(x.OutboundId))
                    .ToList();
            });

            EntityList<ProcessingInStock> processingInStocks
               = new EntityList<ProcessingInStock>();
            decimal totalQty = 0;
            foreach (var outbound in outbounds)
            {
                //已入库数量
                decimal inStockQty = 0;
                if (otherInStocks.Any(x => x.OutboundId == outbound.Id))
                {
                    inStockQty = otherInStocks.Where(x => x.OutboundId == outbound.Id).Sum(x => x.Qty);
                }

                //已经全部入库了
                if (inStockQty >= outbound.Qty)
                {
                    continue;
                }

                //本次入库数量
                decimal qty = outbound.Qty - inStockQty;

                //总入库数量
                totalQty += qty;

                processingInStocks.Add(new ProcessingInStock
                {
                    SourceId = outbound.SourceId,
                    OutboundId = outbound.Id,
                    SN = outbound.SN,
                    LotNo = outbound.LotNo,
                    OutsourcingRequestId = outsourcingRequest.Id,
                    Qty = qty,
                    PassQty = qty,
                    State = OutsourcingDetailState.Created
                });
            }

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(processingInStocks);
                //RF.BatchInsert(processingInStocks);
                trans.Complete();
            }

            return "委外需求单【{0}】成功【添加可入库委外在制品】，数量为【{1}】"
                    .L10nFormat(outsourcingRequest.NO, decimal.Parse(totalQty.ToString("0.###########")));
        }

        /// <summary>
        /// 添加可出库委外在制品
        /// </summary>
        /// <param name="outsourcingId">委外需求单Id</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public virtual string AddOutboundWips(double outsourcingId)
        {
            OutsourcingRequest outsourcingRequest
                = RF.GetById<OutsourcingRequest>(outsourcingId, new EagerLoadOptions().LoadWithViewProperty());
            if (outsourcingRequest == null)
            {
                throw new EntityNotFoundException(typeof(OutsourcingRequest), outsourcingId);
            }

            OutboundStrategy outboundStrategy = GetOutboundStrategy(outsourcingRequest);

            if (outsourcingRequest.BeginProcess == null)
            {
                throw new ValidationException("委外需求单【{0}】的【起始工序】为空"
                        .L10nFormat(outsourcingRequest.NO));
            }

            var count = outboundStrategy.AddOutboundProducts(outsourcingRequest);

            if (count <= 0)
            {
                return "委外需求单【{0}】新增可出库在制品失败，工单【{1}】在工序【{2}】无待生产的在制品!"
                    .L10nFormat(outsourcingRequest.NO, outsourcingRequest.WorkOrderNo, outsourcingRequest.BeginProcessName);
            }
            else
            {
                return "委外需求单【{0}】新增可出库在制品成功，添加在制品记录【{1}】笔"
                    .L10nFormat(outsourcingRequest.NO, count);
            }
        }
        /// <summary>
        /// 获取对应实现
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual OutboundStrategy GetOutboundStrategy(OutsourcingRequest outsourcingRequest)
        {
            var itemBatchRule = Query<ItemBatchRule>()
                .Join<WorkOrder>((x, y) => x.ItemId == y.ProductId)
                .Where<WorkOrder>((x, y) => y.Id == outsourcingRequest.WorkOrderId)
                .FirstOrDefault();

            if (itemBatchRule == null)
            {
                var workOrder = outsourcingRequest.WorkOrder;

                if (workOrder == null)
                {
                    throw new ValidationException("委外需求单【{0}】的工单为空"
                        .L10nFormat(outsourcingRequest.NO));
                }

                var item = workOrder.Product;
                if (item == null)
                {
                    throw new ValidationException("工单【{0}】的产品为空"
                        .L10nFormat(workOrder.No));
                }

                throw new ValidationException("物料【{0}】的【物料批次规则】没有维护，请在【物料】功能进行维护！"
                       .L10nFormat(item.Code));
            }

            OutboundStrategy outboundStrategy;
            if (itemBatchRule.RetrospectType == RetrospectType.Single)
            {
                outboundStrategy = new SnOutboundStrategy();
            }
            else
            {
                outboundStrategy = new BatchOutboundStrategy();
            }

            return outboundStrategy;
        }

        /// <summary>
        /// 获取对应入库对应实现
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual InStockStrategy GetInStockStrategy(OutsourcingRequest outsourcingRequest)
        {
            var itemBatchRule = Query<ItemBatchRule>()
                .Join<WorkOrder>((x, y) => x.ItemId == y.ProductId)
                .Where<WorkOrder>((x, y) => y.Id == outsourcingRequest.WorkOrderId)
                .FirstOrDefault();

            if (itemBatchRule == null)
            {
                var workOrder = outsourcingRequest.WorkOrder;

                if (workOrder == null)
                {
                    throw new ValidationException("委外需求单【{0}】的工单为空"
                        .L10nFormat(outsourcingRequest.NO));
                }

                var item = workOrder.Product;
                if (item == null)
                {
                    throw new ValidationException("工单【{0}】的产品为空"
                        .L10nFormat(workOrder.No));
                }

                throw new ValidationException("物料【{0}】的【物料批次规则】没有维护，请在【物料】功能进行维护！"
                       .L10nFormat(item.Code));
            }

            InStockStrategy inStockStrategy;
            if (itemBatchRule.RetrospectType == RetrospectType.Single)
            {
                inStockStrategy = new SnInStockStrategy();
            }
            else
            {
                inStockStrategy = new BatchInStockStrategy();
            }

            return inStockStrategy;
        }

        /// <summary>
        /// 删除委外出库
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void DeleteOutbounds(double[] selectedIds)
        {
            var outbounds = selectedIds.SplitContains(tempIds =>
            {
                return Query<ProcessingOutbound>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            if (!outbounds.Any())
            {
                return;
            }

            if (outbounds.Any(x => x.State != OutsourcingDetailState.Created))
            {
                throw new ValidationException("存在【在制品委外出库】的状态不是【{0}】，删除失败！"
                    .L10nFormat(OutsourcingDetailState.Created.ToLabel()));
            }

            var outbound = outbounds.FirstOrDefault();
            if (outbound == null)
            {
                return;
            }

            OutsourcingRequest outsourcingRequest
                = RF.GetById<OutsourcingRequest>(outbound.OutsourcingRequestId, new EagerLoadOptions().LoadWithViewProperty());
            if (outsourcingRequest == null)
            {
                throw new EntityNotFoundException(typeof(OutsourcingRequest), outbound.OutsourcingRequestId);
            }

            OutboundStrategy outboundStrategy = GetOutboundStrategy(outsourcingRequest);

            outboundStrategy.DeleteOutbounds(outsourcingRequest, outbounds);
        }


        /// <summary>
        /// 提交委外出库
        /// </summary>
        /// <param name="outboundIds"></param>
        /// <returns></returns>
        public virtual void SubmitOutbounds(IList<double> outboundIds)
        {
            var outbounds = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingOutbound>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            if (outbounds.Any(x => x.State != OutsourcingDetailState.Created))
            {
                throw new ValidationException("存在【在制品委外出库】的状态不是【{0}】，提交失败！"
                    .L10nFormat(OutsourcingDetailState.Created.ToLabel()));
            }
            if (!outbounds.Any())
            {
                throw new ValidationException("请至少选择一条委外出库数据进行提交，请检查".L10N());
            }
            OutsourcingRequest outsourcingRequest = RF.GetById<OutsourcingRequest>(outbounds[0].OutsourcingRequestId);
            if (outsourcingRequest == null)
            {
                throw new ValidationException("无法找到委外需求单，请检查单据是否丢失".L10N());
            }
            if (outsourcingRequest.OutsourcingState == OutsourcingState.Completed)
            {
                throw new ValidationException("提交失败,委外需求单状态为【已完成】".L10N());
            }

            OutboundStrategy outboundStrategy = GetOutboundStrategy(outsourcingRequest);
            outboundStrategy.SubmitOutbounds(outsourcingRequest, outbounds);
        }

        /// <summary>
        /// 提交委外入库
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void SubmitInstocks(double[] selectedIds)
        {
            var inStocks = selectedIds.SplitContains(tempIds =>
            {
                return Query<ProcessingInStock>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            if (!inStocks.Any())
            {
                return;
            }

            if (inStocks.Any(x => x.State != OutsourcingDetailState.Created))
            {
                throw new ValidationException("存在【在制品委外入库】的状态不是【{0}】，提交失败！"
                    .L10nFormat(OutsourcingDetailState.Created.ToLabel()));
            }

            var inStockFirst = inStocks.FirstOrDefault();
            if (inStockFirst == null)
            {
                return;
            }

            OutsourcingRequest outsourcingRequest
                = RF.GetById<OutsourcingRequest>(inStockFirst.OutsourcingRequestId, new EagerLoadOptions().LoadWithViewProperty());
            if (outsourcingRequest == null)
            {
                throw new EntityNotFoundException(typeof(OutsourcingRequest), inStockFirst.OutsourcingRequestId);
            }
            InStockStrategy inStockStrategy = GetInStockStrategy(outsourcingRequest);
            SubmitInstocks(inStockStrategy, outsourcingRequest, inStocks, selectedIds);
        }

        /// <summary>
        /// 提交入库
        /// </summary>
        /// <param name="inStockStrategy"></param>
        /// <param name="outsourcingRequest"></param>
        /// <param name="inStocks"></param>
        /// <param name="selectedIds"></param>
        public virtual void SubmitInstocks(InStockStrategy inStockStrategy, OutsourcingRequest outsourcingRequest, EntityList<ProcessingInStock> inStocks, double[] selectedIds = null)
        {

            //委外出库ID不为空的Ids
            var outboundIds = inStocks.Select(x => x.OutboundId).Distinct().ToList();

            //委外出库
            var outbounds = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingOutbound>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            //别的单的委外入库            
            var otherInStocks = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingInStock>()
                    .Where(x => tempIds.Contains(x.OutboundId) && x.State == OutsourcingDetailState.Submitted)
                    .ToList();
            });
            //提交时候增加校验
            foreach (var processingInStock in inStocks)
            {
                //SN和批次号为空的则不校验入库数和出库数的关系
                if (processingInStock.SN.IsNullOrEmpty() && processingInStock.LotNo.IsNullOrEmpty())
                {
                    continue;
                }

                var otherInstockQty = otherInStocks
                    .Where(x => x.OutboundId == processingInStock.OutboundId && x.Id != processingInStock.Id)
                    .Sum(x => x.Qty);
                var outQty = outbounds.Where(x => x.Id == processingInStock.OutboundId).Sum(x => x.Qty);

                var sn = processingInStock.SN;
                if (sn.IsNullOrEmpty())
                {
                    sn = processingInStock.LotNo;
                }

                if (otherInstockQty + processingInStock.Qty > outQty)
                {
                    throw new ValidationException("【{0}】入库数【{1}】不能大于 同条码或同批次的委外出库数【{2}】"
                        .L10nFormat(sn, otherInstockQty + processingInStock.Qty, outQty));
                }
            }


            List<ProcessingInStock> inStocksToResumeProduction = new List<ProcessingInStock>();

            Dictionary<double?, decimal> inStockQtyDictionary = new Dictionary<double?, decimal>();

            foreach (var processingInStock in inStocks)
            {
                if (!processingInStock.OutboundId.HasValue)
                {
                    continue;
                }
                decimal instockQty = processingInStock.Qty;
                if (inStockQtyDictionary.ContainsKey(processingInStock.OutboundId))
                {
                    instockQty = instockQty + inStockQtyDictionary[processingInStock.OutboundId];
                    inStockQtyDictionary[processingInStock.OutboundId] = instockQty;
                }
                else
                {
                    inStockQtyDictionary.Add(processingInStock.OutboundId, instockQty);
                }

                var otherInstockQty = otherInStocks
                    .Where(x => x.OutboundId == processingInStock.OutboundId)
                    .Sum(x => x.Qty);

                var outQty = outbounds.Where(x => x.Id == processingInStock.OutboundId).Sum(x => x.Qty);

                //委外入库数量 等于 出库数量，才能恢复生产，重新上线
                if (processingInStock.SourceId != 0 && otherInstockQty + instockQty == outQty)
                {
                    inStocksToResumeProduction.Add(processingInStock);
                }
            }

            var qty = inStocks.Sum(x => x.Qty);
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (inStocksToResumeProduction.Any())
                {
                    //委外完成回产线处理
                    inStockStrategy.ResumeProduction(outsourcingRequest, inStocksToResumeProduction);
                }
                if (selectedIds != null && selectedIds.Any())//存在数据的时候是WEB端传入 WEB存在保存按钮 此时数据为待提交
                {
                    //更新委外
                    selectedIds.SplitDataExecute(tempIds =>
                    {
                        DB.Update<ProcessingInStock>()
                        .Set(x => x.State, OutsourcingDetailState.Submitted)
                        .Where(x => tempIds.Contains(x.Id))
                        .Execute();
                    });
                }
                else//PDA 的数据全部为已提交 直接插入
                {
                    RF.Save(inStocks);
                }

                if (outsourcingRequest.RequestQty == outsourcingRequest.WarehousingQty + qty)
                {
                    //更新委外入库数量和委外需求单的状态
                    DB.Update<OutsourcingRequest>()
                        .Set(x => x.WarehousingQty, x => x.WarehousingQty + qty)
                        .Set(x => x.OutsourcingState, OutsourcingState.Completed)
                        .Where(x => x.Id == outsourcingRequest.Id)
                        .Execute();
                }
                else
                {
                    //更新委外入库数量
                    DB.Update<OutsourcingRequest>()
                        .Set(x => x.WarehousingQty, x => x.WarehousingQty + qty)
                        .Where(x => x.Id == outsourcingRequest.Id)
                        .Execute();
                }

                trans.Complete();
            }

            //调用接口同步
            //切记：此处不能做保存,只为了做为接口参数
            OutsourcingRequest request = RF.GetById<OutsourcingRequest>(outsourcingRequest.Id, new EagerLoadOptions().LoadWithViewProperty());
            request.ProcessingOutsourcingInStockList.Clear();
            request.ProcessingOutsourcingOutboundList.Clear();
            request.ProcessingOutsourcingInStockList.AddRange(inStocks);
            //创建事务、调用同步接口
            RT.Service.Resolve<OutsourcingApiController>().SyncOutsourcingRequestToOtherFactory(request, 2, request.OutFactory);
        }
        /// <summary>
        /// 保存无SN和批次的出库记录
        /// </summary>
        /// <param name="processingOutbounds"></param>
        public virtual void SaveProcessingInStocksReporWork(EntityList<ProcessingOutbound> processingOutbounds)
        {
            //只更新数量 状态交给提交
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(processingOutbounds);
                var sumQty = Query<ProcessingOutbound>().Where(x => x.OutsourcingRequestId == processingOutbounds[0].OutsourcingRequestId).Select(m => m.Qty).ToList().Sum(m => m.Qty);

                DB.Update<OutsourcingRequest>()
                            .Set(x => x.OutboundQty, sumQty)
                            .Where(x => x.Id == processingOutbounds[0].OutsourcingRequestId)
                            .Execute();
                tran.Complete();
            }

        }



        /// <summary>
        /// 保存委外入库
        /// </summary>
        /// <param name="processingInStocks"></param>
        public virtual void SaveProcessingInStocks(EntityList<ProcessingInStock> processingInStocks)
        {
            if (processingInStocks == null || !processingInStocks.Any())
            {
                throw new ValidationException("无法找到入库数据的委外需求单，请检查".L10N());
            }
            var outsourcingRequest = Query<OutsourcingRequest>().Where(m => m.Id == processingInStocks[0].OutsourcingRequestId).FirstOrDefault();
            if (outsourcingRequest == null)
            {
                throw new ValidationException("无法找到入库数据的委外需求单，请检查".L10N());
            }
            if (outsourcingRequest.OutsourcingState != OutsourcingState.Outsourcing)
            {
                throw new ValidationException("工序委外需求单状态不为【委外中】，无法添加、修改入库记录，请检查".L10N());
            }
            var outboundIds = processingInStocks.Select(x => x.OutboundId).Distinct().ToList();

            //委外出库
            var outbounds = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingOutbound>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            //别的单的委外入库                
            var otherInStocks = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingInStock>()
                    .Where(x => tempIds.Contains(x.OutboundId))
                    .ToList();
            });

            foreach (var processingInStock in processingInStocks)
            {
                //SN和批次号为空的则不校验入库数和出库数的关系
                if (processingInStock.SN.IsNullOrEmpty() && processingInStock.LotNo.IsNullOrEmpty())
                {
                    continue;
                }

                var otherInstockQty = otherInStocks
                    .Where(x => x.OutboundId == processingInStock.OutboundId && x.Id != processingInStock.Id)
                    .Sum(x => x.Qty);
                var outQty = outbounds.Where(x => x.Id == processingInStock.OutboundId).Sum(x => x.Qty);

                var sn = processingInStock.SN;
                if (sn.IsNullOrEmpty())
                {
                    sn = processingInStock.LotNo;
                }

                if (otherInstockQty + processingInStock.Qty > outQty)
                {
                    throw new ValidationException("【{0}】入库数【{1}】不能大于 同条码或同批次的委外出库数【{2}】"
                        .L10nFormat(sn, otherInstockQty + processingInStock.Qty, outQty));
                }
            }
            RF.Save(processingInStocks);
        }

        /// <summary>
        /// 强制完成
        /// </summary>
        /// <param name="outboundIds"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void ForceComplete(double[] outboundIds)
        {
            var outbounds = outboundIds.SplitContains(tempIds =>
            {
                return Query<OutsourcingRequest>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            if (outbounds.Any(x => x.OutsourcingState != OutsourcingState.Outsourcing))
            {
                throw new ValidationException("存在【委外需求单】的状态不是【{0}】，强制完成失败！"
                    .L10nFormat(OutsourcingState.Outsourcing.ToLabel()));
            }
            var processingInStocks = outboundIds.SplitContains(tempIds =>
               {
                   return Query<ProcessingInStock>()
                       .Where(x => tempIds.Contains(x.OutsourcingRequestId) && x.State == OutsourcingDetailState.Created)
                       .ToList();
               });
            if (processingInStocks.Any())
            {
                throw new ValidationException("强制完成失败，存在【{0}】的委外入库数据，请检查！".L10nFormat(OutsourcingDetailState.Created.ToLabel()));
            }

            var processingOutbounds = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingOutbound>()
                    .Where(x => tempIds.Contains(x.OutsourcingRequestId))
                    .ToList();
            });
            if (processingOutbounds.Any(x => x.State == OutsourcingDetailState.Created))
            {
                throw new ValidationException("强制完成失败，存在【{0}】的委外出库数据，请检查！".L10nFormat(OutsourcingDetailState.Created.ToLabel()));
            }
            //检查单体生产条码是否存在委外中
            if (processingOutbounds.Any(m => !m.SN.IsNullOrEmpty()))
            {
                var wipSnProductVersionIds = processingOutbounds.Where(m => m.SourceId != 0 && !m.SN.IsNullOrEmpty()).Select(x => x.SourceId).Distinct().ToList();

                var wipSnProductVersions = wipSnProductVersionIds.SplitContains(tempIds =>
                    {
                        return DB.Query<WipProductVersion>()
                        .Where(x => tempIds.Contains(x.Id) && x.IsOutsourcing)
                        .ToList();
                    });
                if (wipSnProductVersions.Any())
                {
                    var requestId = processingOutbounds.Where(m => wipSnProductVersions.Select(m => m.Sn).Contains(m.SN)).Select(m => m.OutsourcingRequestId).ToList();
                    var requestNos = outbounds.Where(m => requestId.Contains(m.Id)).Select(m => m.NO).ToList();
                    throw new ValidationException("需求单【{0}】存在生产条码处于委外中,强制完成失败！".L10nFormat(requestNos.Concat(",")));
                }
            }
            //检查批次生产条码是否存在委外中
            if (processingOutbounds.Any(m => !m.LotNo.IsNullOrEmpty()))
            {
                var wipProductVersionIds = processingOutbounds.Where(m => m.SourceId != 0 && !m.LotNo.IsNullOrEmpty()).Select(x => x.SourceId).Distinct().ToList();

                var wipLotProductVersions = wipProductVersionIds.SplitContains(tempIds =>
                    {
                        return DB.Query<BatchWipProductVersion>()
                        .Where(x => tempIds.Contains(x.Id) && x.IsOutsourcing)
                        .ToList();
                    });
                if (wipLotProductVersions.Any())
                {
                    var requestId = processingOutbounds.Where(m => wipLotProductVersions.Select(m => m.BatchNo).Contains(m.LotNo)).Select(m => m.OutsourcingRequestId).ToList();
                    var requestNos = outbounds.Where(m => requestId.Contains(m.Id)).Select(m => m.NO).ToList();
                    throw new ValidationException("需求单【{0}】存在批次条码处于委外中,强制完成失败！".L10nFormat(requestNos.Concat(",")));
                }
            }
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //更新委外需求单状态为完成
                outboundIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<OutsourcingRequest>()
                        .Set(x => x.OutsourcingState, OutsourcingState.Completed)
                        .Set(x => x.OutboundState, OutboundState.Finish)
                        .Where(x => tempIds.Contains(x.Id))
                        .Execute();
                });

                trans.Complete();
            }
        }


        /// <summary>
        /// 根据关键字获取在制品
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="workOrderRoutingProcess"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipProductVersion> GetWipProductVersionsByLotNos(double workOrderId,
            WorkOrderRoutingProcess workOrderRoutingProcess, List<string> keywords)
        {
            return DB.Query<BatchWipProductVersion>()
                .Where(x => x.WorkOrderId == workOrderId
                    && x.NextProcessId == workOrderRoutingProcess.ProcessId
                    && x.NextProcessIndex == workOrderRoutingProcess.Index
                    && keywords.Contains(x.BatchNo))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据关键字获取生产版本
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="workOrderRoutingProcess"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public virtual EntityList<WipProductVersion> GetWipProductVersionsByKeyWord(double workOrderId,
           WorkOrderRoutingProcess workOrderRoutingProcess, List<string> keyWords)
        {
            return DB.Query<WipProductVersion>()
                .Where(x => x.WorkOrderId == workOrderId
                    && x.NextProcessId == workOrderRoutingProcess.ProcessId
                    && x.NextProcessIndex == workOrderRoutingProcess.Index
                    && (keyWords.Contains(x.Sn) || keyWords.Contains(x.KeyLabel)))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 删除入库数据
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void DeleteInStock(double[] selectedIds)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                throw new ValidationException("请至少选择一条数据！".L10N());
            }
            selectedIds.SplitDataExecute(tempIds =>
            {
                DB.Delete<ProcessingInStock>()
                    .Where(x => tempIds.Contains(x.Id))
                    .Execute();
            });
        }
    }
}
