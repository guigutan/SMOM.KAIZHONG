using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// SN接收控制器
    /// </summary>
    public class FixtureReceiveSnController : DomainController
    {
        /// <summary>
        /// 提交接收
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void SubmitReceive(List<double> selectedIds)
        {
            var ctr = RT.Service.Resolve<FixtureReceiveController>();

            var receives = ctr.GetReceivesByIds(selectedIds);
            EntityList<FixtureReceiveDetail> allDetails = CheckData(selectedIds, ctr, receives);

            var purchaseOrderItemIdListIds = allDetails.Select(m => m.PurchaseOrderItemId).ToList();

            //取回所有的订单行数据
            var purchaseOrderItemList = new EntityList<PurchaseOrderItem>();
            if (purchaseOrderItemIdListIds.Any())
            {
                purchaseOrderItemList = purchaseOrderItemIdListIds.SplitContains(items =>
                {
                    return Query<PurchaseOrderItem>().Where(n => items.Contains(n.Id)).ToList();
                });
            }
            //所有的采购订单
            var purchaseOrderListIds = allDetails.Select(m => m.PurchaseOrderId).ToList();
            var purchaseOrderList = new EntityList<PurchaseOrder>();
            if (purchaseOrderListIds.Any())
            {
                purchaseOrderList = purchaseOrderListIds.SplitContains(items =>
                {
                    return Query<PurchaseOrder>().Where(n => items.Contains(n.Id)).ToList();
                });
            }
            var now = RF.Find<PurchaseOrder>().GetDbTime();
            var resultFixtureAcceptance = new EntityList<FixtureAcceptance>();
            var resultFixtureInboundOrder = new EntityList<InboundOrder>();

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var receive in receives)
                {
                    receive.ReceiveBillStatus = ReceiveBillStatus.Completed;
                    var details = allDetails.Where(m => m.FixtureReceiveId == receive.Id).ToList();//明细
                    if (details.Any(m => m.RecivedQty != m.Qty || m.RecivedQty == 0))
                    {
                        throw new ValidationException("提交失败：存在未接收完成的工治具明细数据！".L10N());
                    }
                    if (receive.ReceiveType == ReceiveType.Purchase)
                    {
                        details.ForEach(item =>
                        {
                            var purOrItem = purchaseOrderItemList.FirstOrDefault(m => m.Id == item.PurchaseOrderItemId);
                            if (purOrItem != null)
                            {
                                var order = purchaseOrderList.FirstOrDefault(m => m.Id == purOrItem.PurchaseOrderId);
                                if (order == null)
                                {
                                    throw new ValidationException("采购订单数据无法找到".L10N());
                                }
                                purOrItem.ReciveQty += item.Qty;
                                purOrItem.TotalReciveQty += item.Qty;
                                var qty = purOrItem.ReciveQty + purOrItem.InboundQty + purOrItem.AcceptanceQty;
                                //更新后【接收数量+验收数量+入库数量】不能大于采购数量，否则报错：采购订单：XXX，行号：XXX，收货数量不能大于采购数量；
                                if (qty > purOrItem.Qty)
                                {
                                    throw new ValidationException("采购订单：{0}，行号：{1}，收货数量不能大于采购数量".L10nFormat(purOrItem.PurchaseOrder.OrderNo, purOrItem.LineNo));
                                }
                                //当【接收数量+验收数量+入库数量】大于0小于【采购数量】时，更新采购订单行的状态为【部分收货】，同时更新主表的订单状态为【部分收货】；
                                if (0 < qty && qty < purOrItem.Qty)
                                {
                                    purOrItem.Status = PurchaseOrderStatus.PartRecive;
                                    order.PurchaseOrderStatus = PurchaseOrderStatus.PartRecive;
                                    RF.Save(purOrItem);//为了后续判断是否全部已经接收需先保存
                                }
                                //当【接收数量+验收数量+入库数量】等于【采购数量】时，更新采购订单行的状态为【已收货】；全部行状态为【已收货】时，更新主表订单状态为【已收货】
                                if (qty == purOrItem.Qty)
                                {
                                    purOrItem.Status = PurchaseOrderStatus.Recived;
                                    RF.Save(purOrItem);//为了后续判断是否全部已经接收需先保存
                                    if (GetPurchaseOrderIsAllRecived(order.Id))
                                    {
                                        order.PurchaseOrderStatus = PurchaseOrderStatus.Recived;
                                    }
                                }
                            }
                        });
                    }
                    receive.ReceiveDateTime = now;
                    receive.ReceiverId = RT.IdentityId;
                    GetExemptionDetail(resultFixtureAcceptance, receive, details);
                    GetExemptionInspectInboundOrder(resultFixtureInboundOrder, receive, details);

                }
                if (resultFixtureAcceptance.Any())
                {
                    RF.Save(resultFixtureAcceptance);
                }

                if (resultFixtureInboundOrder.Any())
                {
                    RF.Save(resultFixtureInboundOrder);
                }
                if (purchaseOrderList.Any())
                {
                    RF.Save(purchaseOrderList);
                }
                if (purchaseOrderItemList.Any())
                {
                    RF.Save(purchaseOrderItemList);
                }
                RF.Save(receives);
                trans.Complete();
            }
        }


        /// <summary>
        /// 校验数据
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="ctr"></param>
        /// <param name="receives"></param>
        /// <returns></returns>
        private EntityList<FixtureReceiveDetail> CheckData(List<double> selectedIds, FixtureReceiveController ctr, EntityList<FixtureReceive> receives)
        {
            if (receives.Any(p => p.ReceiveBillStatus != ReceiveBillStatus.ToBeSubmitted))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能提交".L10N());
            }
            var allDetails = ctr.GetDetailsByReceiveIds(selectedIds);

            if (allDetails.Any(m => m.Qty == 0))
            {
                throw new ValidationException("接收明细存在接收数量为0的数据，请确认！".L10N());
            }

            return allDetails;
        }

        /// <summary>
        /// 获取免检的入库单
        /// </summary>
        /// <param name="resultFixtureInboundOrder"></param>
        /// <param name="receive"></param>
        /// <param name="details"></param>
        private void GetExemptionInspectInboundOrder(EntityList<InboundOrder> resultFixtureInboundOrder, FixtureReceive receive, List<FixtureReceiveDetail> details)
        {
            if (receive == null|| details==null)
            {
                return;
            }
            var exemptionDetail = details.Where(m => m.ExemptionInspect).ToList();
            //免检字段为【是】的工治具明细表数据按：工治具编码+客户编码+供应商编码+接收类型+接收仓库相同的数据分组
            if (exemptionDetail.Any())
            {
                var dicExemptionDetail = exemptionDetail.GroupBy(m => new { m.FixtureEncodeId, m.CustomerId, m.SupplierId, m.ReceiveType, m.WarehouseId })
                    .ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in dicExemptionDetail.Keys)
                {
                    var keyDetails = dicExemptionDetail[key].ToList();
                    var result = GenarateInboundOrder(receive, keyDetails);
                    if (result != null)
                    {
                        resultFixtureInboundOrder.Add(result);
                    }
                }
            }
        }

        /// <summary>
        /// 更新或新增管控为编码的工治具台账
        /// </summary>
        /// <param name="fixtureReceiveDetail"></param>
        /// <param name="ExemptionInspect">是否免检</param>
        private void UpdateCodeManageMode(FixtureReceiveDetail fixtureReceiveDetail, bool ExemptionInspect)
        {
            if (fixtureReceiveDetail == null)
            {
                return;
            }
            var coreFixtureController = RT.Service.Resolve<CoreFixtureController>();
            var exsiedEntity = coreFixtureController.GetFixtureAccountByCodeOrRFID(fixtureReceiveDetail.FixtureEncodeCode);//获取工治具台账是否存在该编码作为编码类台账的数据 如果不存在则生成

            //免检更新编码类台账的【总数和待入库】为原来的值加上本次接收的数量，台账没有这个工治具时，增加一条数据，除【总数和待入库】外其他数量都为0；
            if (ExemptionInspect)
            {
                if (exsiedEntity == null)
                {
                    var codeAccount = new FixtureCodeAccount()
                    {
                        Code = fixtureReceiveDetail.FixtureEncodeCode,
                        FixtureEncodeId = fixtureReceiveDetail.FixtureEncodeId,
                        FixtureTypeId = fixtureReceiveDetail.FixtureEncode.FixtureModel.FixtureTypeId,
                        TotalQty = fixtureReceiveDetail.Qty,
                        PassQty = fixtureReceiveDetail.Qty,
                        WaitShelfQty = fixtureReceiveDetail.Qty,//待入库
                        QualityState = FixtureQualityState.Pass
                    };
                    RF.Save(codeAccount);
                    return;
                }
                exsiedEntity.TotalQty += fixtureReceiveDetail.Qty;
                exsiedEntity.PassQty += fixtureReceiveDetail.Qty;
                exsiedEntity.WaitShelfQty += fixtureReceiveDetail.Qty;
                exsiedEntity.QualityState = FixtureQualityState.Pass;
                RF.Save(exsiedEntity);
            }
            else
            {
                //2.不免检更新编码类台账的【总数和待验收】为原来的值加上本次接收的数量，台账没有这个工治具时，增加一条数据
                if (exsiedEntity == null)
                {
                    var codeAccount = new FixtureCodeAccount()
                    {
                        Code = fixtureReceiveDetail.FixtureEncodeCode,
                        FixtureEncodeId = fixtureReceiveDetail.FixtureEncodeId,
                        FixtureTypeId = fixtureReceiveDetail.FixtureEncode.FixtureModel.FixtureTypeId,
                        ToAccepted = fixtureReceiveDetail.Qty,//待验收
                        TotalQty = fixtureReceiveDetail.Qty,
                        QualityState = FixtureQualityState.Pass
                    };
                    RF.Save(codeAccount);
                    return;
                }
                exsiedEntity.TotalQty += fixtureReceiveDetail.Qty;
                exsiedEntity.ToAccepted += fixtureReceiveDetail.Qty;
                exsiedEntity.QualityState = FixtureQualityState.Pass;
                RF.Save(exsiedEntity);
            }
        }

        /// <summary>
        /// 生成入库
        /// </summary>
        /// <param name="fixtureReceive"></param>
        /// <param name="fixtureReceiveDetails"></param>
        /// <returns></returns>
        public virtual InboundOrder GenarateInboundOrder(FixtureReceive fixtureReceive, List<FixtureReceiveDetail> fixtureReceiveDetails)
        {
            if (fixtureReceive == null || fixtureReceiveDetails == null)
            {
                throw new ValidationException("参数异常".L10N());
            }

            var firstItem = fixtureReceiveDetails.FirstOrDefault();
            var inboundType = GetInboundType(fixtureReceive.ReceiveType);
            InboundOrder inboundOrder = GetInboundOrder(fixtureReceive, fixtureReceiveDetails, firstItem, inboundType);
            var detailIds = fixtureReceiveDetails.Select(m => m.Id);
            var snList = Query<FixtureReceiveSn>().Where(m => detailIds.Contains(m.FixtureReceiveDetailId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (firstItem.ManageMode == Fixtures.ManageMode.Code)
            {
                foreach (var detail in fixtureReceiveDetails)
                {
                    //是免检工治具接收明细
                    UpdateCodeManageMode(detail,true);
                }

                //本组数据是的工治具是编码管理且采购订单不为空时，生成采购订单子表数据
                if (!firstItem.PurchaseOrderId.HasValue)
                {
                    return inboundOrder;
                }

                var dicOrder = fixtureReceiveDetails.GroupBy(p => new { p.PurchaseOrderId, p.PurchaseOrderItemId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in dicOrder.Keys)
                {
                    if (key.PurchaseOrderItemId.HasValue && dicOrder[key].Any())
                    {
                        inboundOrder.InboundOrderPurchaseList.Add(new InboundOrderPurchase()
                        {
                            InboundOrderId = inboundOrder.Id,
                            PoLine = dicOrder[key].First().LineNo,
                            PoNo = dicOrder[key].First().PurOrderNo,
                            Price = dicOrder[key].First().Price,
                            Qty = dicOrder[key].Sum(m => m.Qty)
                        });
                    }
                }
            }
            else//当本组数据是的工治具是ID管理时，生成ID明细子表数据
            {
                var snCodes = snList.Select(m => m.Sn).ToList();
                var idAccount = Query<FixtureAccount>().Where(m => snCodes.Contains(m.Code)).ToList();
                foreach (var sn in snList)
                {
                    var account = idAccount.FirstOrDefault(m => m.Code == sn.Sn);
                    if (account != null)
                    {
                        inboundOrder.InboundOrderFixtureIdAccountList.Add(new InboundOrderFixtureIdAccount()
                        {
                            FixtureIDAccountId = account.Id,
                            InboundOrderId = inboundOrder.Id,
                            LineNo = sn.LineNo,
                            OriginalSerialNumber = sn.OriginalSn,
                            PoNo = sn.PurOrderNo,
                            PoLineNo = sn.OrderLineNo,
                            Qty = 1,
                            Price = sn.FixtureReceiveDetail.Price,
                            
                        });
                        account.QualityState = FixtureQualityState.Pass;
                        RF.Save(account);
                    }
                }
            }
            return inboundOrder;

        }

        /// <summary>
        /// 获取一个工治具入库对象
        /// </summary>
        /// <param name="fixtureReceive"></param>
        /// <param name="fixtureReceiveDetails"></param>
        /// <param name="firstItem"></param>
        /// <param name="inboundType"></param>
        /// <returns></returns>
        private InboundOrder GetInboundOrder(FixtureReceive fixtureReceive, List<FixtureReceiveDetail> fixtureReceiveDetails, FixtureReceiveDetail firstItem, FixtureInboundType inboundType)
        {
            if (fixtureReceive == null || firstItem == null)
            {
                throw new ValidationException("方法参数异常".L10N());
            }
            InboundOrder inboundOrder = new InboundOrder()
            {
                No = RT.Service.Resolve<CommonController>().GetNo<InboundOrder>("工治具入库单号"),
                SupplierId = firstItem.SupplierId,
                CustomerId = firstItem.CustomerId,
                FixtureEncodeId = firstItem.FixtureEncodeId,
                InboundStatus = InboundStatus.ToBe,
                InboundType = inboundType,
                Qty = fixtureReceiveDetails.Sum(m => m.Qty),
                WarehouseId = firstItem.WarehouseId,
                Proprietorship = GetProprietorship(inboundType),
                ReceiptOrderNo = fixtureReceive.ReceiveNo,
                QualityState= FixtureQualityState.Pass,
            };
            inboundOrder.GenerateId();
            return inboundOrder;
        }

        /// <summary>
        ///  根据入库类型：租赁入库时取值【租赁】；客供入库时取值【客供】；其他类型取值【自有】
        /// </summary>
        /// <param name="inboundType"></param>
        /// <returns></returns>
        public virtual Proprietorship GetProprietorship(FixtureInboundType inboundType)
        {
            if (inboundType == FixtureInboundType.Lease)
                return Proprietorship.Lease;
            else if (inboundType == FixtureInboundType.Guest)
                return Proprietorship.ByCustomer;
            else
                return Proprietorship.Own;
        }

        /// <summary>
        /// 接收和入库类型转换
        /// </summary>
        /// <param name="receiveType"></param>
        /// <returns></returns>

        public virtual FixtureInboundType GetInboundType(ReceiveType receiveType)
        {
            switch (receiveType)
            {
                case ReceiveType.Purchase:
                    return FixtureInboundType.Po;
                case ReceiveType.Giveaway:
                    return FixtureInboundType.Gift;
                case ReceiveType.Lease:
                    return FixtureInboundType.Lease;
                case ReceiveType.Customer:
                    return FixtureInboundType.Guest;
                case ReceiveType.Outsourced:
                    return FixtureInboundType.Outsourced;
                case ReceiveType.Other:
                    return FixtureInboundType.Other;
                default:
                    return FixtureInboundType.Other;

            }

        }

        /// <summary>
        /// 获取免检为否的验收明细
        /// </summary>
        /// <param name="resultFixtureAcceptance"></param>
        /// <param name="receive"></param>
        /// <param name="details"></param>
        private void GetExemptionDetail(EntityList<FixtureAcceptance> resultFixtureAcceptance, FixtureReceive receive, List<FixtureReceiveDetail> details)
        {
            //不是免检的验收明细
            var exemptionDetail = details.Where(m => !m.ExemptionInspect).ToList();
            //免检字段为【否】的工治具明细表数据按：工治具编码+客户编码+供应商编码相同的数据分组，每一组生成一条验收数据
            if (exemptionDetail.Any())
            {
                var dicExemptionDetail = exemptionDetail.GroupBy(m => new { m.FixtureEncodeId, m.CustomerId, m.SupplierId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in dicExemptionDetail.Keys)
                {
                    var keyDetails = dicExemptionDetail[key].ToList();
                    var result = GenarateFixtureAcceptance(receive, keyDetails);
                    if (result != null)
                    {
                        resultFixtureAcceptance.Add(result);
                    }
                }

            }
        }


        /// <summary>
        /// 获取是否所有订单号明细都为已接收
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        private bool GetPurchaseOrderIsAllRecived(double purchaseOrderId)
        {
            return Query<PurchaseOrderItem>().Where(m => m.Status != PurchaseOrderStatus.Recived && m.PurchaseOrderId == purchaseOrderId).FirstOrDefault() == null;

        }

        /// <summary>
        /// 创建工治具验收
        /// </summary>
        /// <returns></returns>
        private FixtureAcceptance GenarateFixtureAcceptance(FixtureReceive fixtureReceive, List<FixtureReceiveDetail> fixtureReceiveDetails)
        {
            var no = RT.Service.Resolve<CommonController>().GetNo<FixtureAcceptance>("工治具验收单号配置项");
            var firstItem = fixtureReceiveDetails.FirstOrDefault();
            var fixtureAcceptance = new FixtureAcceptance()
            {
                AcceptanceNo = no,
                ApprovalStatus = ApprovalStatus.Draft,
                CustomerId = firstItem.CustomerId,
                DepartmentId = fixtureReceive.DepartmentId,
                FactoryId = fixtureReceive.FactoryId,
                FixtureEncodeId = firstItem.FixtureEncodeId,
                SupplierId = firstItem.SupplierId,
                ReceiveQty = fixtureReceiveDetails.Sum(m => m.Qty),
                FixtureReceiveId = fixtureReceive.Id,
            };
            fixtureAcceptance.GenerateId();
            var detailIds = fixtureReceiveDetails.Select(m => m.Id);
            var snList = Query<FixtureReceiveSn>().Where(m => detailIds.Contains(m.FixtureReceiveDetailId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            fixtureReceiveDetails.ForEach(item =>
            {
                var child = new FixtureAcceptanceDetail
                {
                    FixtureAcceptanceId = fixtureAcceptance.Id,
                    ReceiveQty = item.Qty,
                    FixtureReceiveDetailId = item.Id,
                    Price = item.Price,
                };
                child.GenerateId();
                fixtureAcceptance.FixtureAcceptanceDetailList.Add(child);

                var childSn = snList.Where(m => m.FixtureReceiveDetailId == item.Id);
                childSn.ForEach(snInfo =>
                {
                    child.FixtureAcceptanceSnList.Add(new FixtureAcceptanceSn()
                    {
                        Sn = snInfo.Sn,
                        Maker = snInfo.Maker,
                        OriginalSn = snInfo.OriginalSn,
                        ProductionDate = snInfo.ProductionDate,
                        FixtureAcceptanceDetailId = child.Id,
                    });
                });

            });


            if (firstItem.ManageMode == Fixtures.ManageMode.Code)
            {
                foreach (var detail in fixtureReceiveDetails)
                {
                    //不是免检工治具接收明细
                    UpdateCodeManageMode(detail, false);
                }
            }
            return fixtureAcceptance;

        }

        /// <summary>
        /// 根据接收ID获取接收SN列表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<FixtureReceiveSn> GetFixtureReceiveSnByReceiveId(double receiveId)
        {
            return Query<FixtureReceiveSn>().Join<FixtureReceiveDetail>((x, y) => x.FixtureReceiveDetailId == y.Id && y.FixtureReceiveId == receiveId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

        }
    }
}
