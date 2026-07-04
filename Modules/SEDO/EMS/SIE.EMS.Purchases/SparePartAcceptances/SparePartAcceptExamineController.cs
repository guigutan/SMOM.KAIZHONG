using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.EMS.SpareParts;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收控制器
    /// </summary>
    public partial class SparePartAcceptExamineController : DomainController
    {
        /// <summary>
        /// 审核备件验收
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExamineSparePartAccept(List<double> acceptIds, ApprovalResult value, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineSparePartAcceptInner(acceptIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核备件验收
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        /// <param name="accepts">数据组</param>
        public virtual void ExamineSparePartAcceptInner(List<double> acceptIds, ApprovalResult value, string remark, EntityList<SparePartAcceptance> accepts = null)
        {
            var ct = RT.Service.Resolve<SparePartAcceptanceController>();
            if (accepts == null)
            {
                accepts = ct.GetSparePartAcceptsByIds(acceptIds);
                if (!accepts.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            var allDetails = ct.GetDetailsByAcceptIds(acceptIds);
            var allLots = ct.GetLotsByAcceptIds(acceptIds);
            var allSns = ct.GetSnsByAcceptIds(acceptIds);
            var orderItemIds = allDetails.Where(p => p.PurchaseOrderItemId != null).Select(p => (double)p.PurchaseOrderItemId).Distinct().ToList();
            var orderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByIds(orderItemIds);
            var orderIds = orderItems.Select(p => p.PurchaseOrderId).Distinct().ToList();
            var orders = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrdersByIds(orderIds);
            var allOrderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByPurIds(orderIds);
            var now = RF.Find<SparePartAcceptance>().GetDbTime();

            foreach (var accept in accepts)
            {
                accept.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
                if (value == ApprovalResult.Pass)
                {
                    var details = allDetails.Where(p => p.SparePartAcceptanceId == accept.Id).ToList();

                    //更新对应采购订单行
                    UpdateOrderItem(accept, details, orders, allOrderItems);

                    //更新删除备件数据
                    var detailIds = details.Select(p => p.Id).ToList();
                    var lots = allLots.Where(p => detailIds.Contains(p.AccepDtlId)).ToList();
                    var sns = allSns.Where(p => detailIds.Contains(p.AcceptDtlId)).ToList();
                    UpdateDeleteSparePart(lots, sns);

                    //保存入库单
                    SaveSparePartStore(accept, details, lots, sns);
                }
            }
            RF.Save(accepts);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(SparePartAcceptance).FullName, value, now, remark);
        }

        /// <summary>
        /// 更新对应采购订单行的【接收数量】【验收数量】
        /// </summary>
        /// <param name="accept">验收</param>
        /// <param name="details">验收明细</param>
        /// <param name="orders">采购订单</param>
        /// <param name="orderItems">订单明细</param>
        private void UpdateOrderItem(SparePartAcceptance accept, List<SparePartAcceptanceDetail> details, EntityList<PurchaseOrder> orders, EntityList<PurchaseOrderItem> orderItems)
        {
            if (accept.ReceiveType == ReceiveType.Purchase)
            {
                foreach (var group in details.GroupBy(p => p.PurchaseOrderItemId))
                {
                    //更新对应采购订单行的【接收数量】【验收数量】
                    var orderItem = orderItems.FirstOrDefault(p => p.Id == group.Key);
                    if (orderItem == null)
                    {
                        throw new ValidationException("找不到id为:{0}的采购订单行".L10nFormat(group.Key));
                    }
                    var order = orders.FirstOrDefault(p => p.Id == orderItem.PurchaseOrderId);
                    if (order == null)
                    {
                        throw new ValidationException("找不到id为:{0}的采购订单".L10nFormat(orderItem.PurchaseOrderId));
                    }
                    var list = group.ToList();
                    //更新对应采购订单行的【接收数量】为原来的值减去本行数据的【接收数量】
                    orderItem.ReciveQty -= list.Sum(p => p.ReceiveQty);
                    //【验收数量】更新为原来的值加上本行数据的【合格数量】
                    orderItem.AcceptanceQty += list.Sum(p => p.PassQty);
                    orderItem.TotalAcceptanceQty += list.Sum(p => p.PassQty);
                    //更新【拒收数量】为原来的值加上本组数据中【验收状态】为【不合格】的数量
                    orderItem.RejectQty += list.Sum(p => p.UnqualifiedQty);
                    //更新后的【接收数量+验收数量+入库数量】等于0时，更新采购订单行的状态为【待收货】
                    if (orderItem.ReciveQty + orderItem.AcceptanceQty + orderItem.InboundQty == 0)
                    {
                        orderItem.Status = PurchaseOrderStatus.TobeRecive;
                        //全部行状态为【待收货】时，更新主表订单状态为【待收货】；
                        var allOrderItems = orderItems.Where(p => p.PurchaseOrderId == orderItem.PurchaseOrderId).ToList();
                        if (allOrderItems.All(p => p.Status == PurchaseOrderStatus.TobeRecive))
                        {
                            order.PurchaseOrderStatus = PurchaseOrderStatus.TobeRecive;
                            RF.Save(order);
                        }
                    }
                    //更新后的【接收数量+验收数量+入库数量】小于【采购数量】时，更新采购订单行的状态为【部分收货】，同时更新主表的【订单状态】为【部分收货】
                    if (orderItem.ReciveQty + orderItem.AcceptanceQty + orderItem.InboundQty < orderItem.Qty)
                    {
                        orderItem.Status = PurchaseOrderStatus.PartRecive;
                        order.PurchaseOrderStatus = PurchaseOrderStatus.PartRecive;
                        RF.Save(order);
                    }
                    RF.Save(orderItem);
                }
            }
        }

        /// <summary>
        /// 更新删除备件数据
        /// </summary>
        /// <param name="lots">批次</param>
        /// <param name="sns">序列号</param>
        private void UpdateDeleteSparePart(List<SparePartAcceptanceLot> lots, List<SparePartAcceptanceSn> sns)
        {
            var passLots = lots.Where(p => p.AcceptanceResult == InspectionResult.Pass).Select(p => p.LotNo).Distinct().ToList();
            var failLots = lots.Where(p => p.AcceptanceResult == InspectionResult.Fail).Select(p => p.LotNo).Distinct().ToList();
            var failSns = sns.Where(p => p.AcceptanceResult == InspectionResult.Fail).Select(p => p.Sn).Distinct().ToList();
            var oldLots = Query<StoreSummaryLot>().Where(p => passLots.Contains(p.BatchNumber)).ToList();
            foreach (var lot in lots.Where(p => p.AcceptanceResult == InspectionResult.Pass))
            {
                var oldLot = oldLots.FirstOrDefault(p => p.BatchNumber == lot.LotNo);
                if (oldLot != null)
                {
                    oldLot.GoodNumber = lot.PassQty;
                    oldLot.RotNumber = lot.UnqualifiedQty;
                    oldLot.SumNumber = lot.Qty;
                    RF.Save(oldLot);
                }
            }
            DB.Delete<StoreSummaryLot>().Where(p => failLots.Contains(p.BatchNumber)).Execute();
            DB.Delete<StoreSummaryDetail>().Where(p => failSns.Contains(p.OrderNumberCode)).Execute();
        }

        /// <summary>
        /// 保存入库单
        /// </summary>
        /// <param name="accept">验收</param>
        /// <param name="details">验收明细</param>
        /// <param name="lots">批次</param>
        /// <param name="sns">序列号</param>
        private void SaveSparePartStore(SparePartAcceptance accept, List<SparePartAcceptanceDetail> details, List<SparePartAcceptanceLot> lots, List<SparePartAcceptanceSn> sns)
        {
            if (accept.PassQty <= 0)//只有合格的单据才需要入库
            {
                return;
            }
            foreach (var group in details.GroupBy(p => p.WarehouseId))
            {
                if (group.Key == null)
                {
                    continue;
                }
                var store = new SparePartStore();
                store.StoreCode = RT.Service.Resolve<SparePartController>().GetStoreCode();
                store.InboundType = RT.Service.Resolve<SparePartReceiveSnController>().GetInboundType(accept.ReceiveType);
                store.ReceiveNo = accept.SparePartReceive.ReceiveNo;
                store.AcceptanceNo = accept.AcceptanceNo;
                store.InboundStatus = InboundStatus.ToBe;
                store.SupplierId = accept.SupplierId;
                store.WarehouseId = group.Key.Value;
                RF.Save(store);
                var lineNo = 0;
                foreach (var item in group.ToList())
                {
                    var lotList = lots.Where(p => p.AccepDtlId == item.Id).ToList();
                    var snList = sns.Where(p => p.AcceptDtlId == item.Id).ToList();
                    if (!lotList.Any() && !snList.Any())
                    {
                        lineNo++;
                        var storeDetail = GenerateStoreDetail(lineNo, item, accept, store.Id);
                        storeDetail.Number = item.PassQty;
                        RF.Save(storeDetail);
                    }
                    else
                    {
                        foreach (var lot in lotList)
                        {
                            if (lot.PassQty <= 0)
                            {
                                continue;
                            }
                            lineNo++;
                            var storeDetail = GenerateStoreDetail(lineNo, item, accept, store.Id);
                            storeDetail.BatchNumber = lot.LotNo;
                            storeDetail.Number = lot.PassQty;
                            RF.Save(storeDetail);
                        }
                        foreach (var sn in snList)
                        {
                            if (sn.AcceptanceResult == InspectionResult.Fail)
                            {
                                continue;
                            }
                            lineNo++;
                            var storeDetail = GenerateStoreDetail(lineNo, item, accept, store.Id);
                            storeDetail.Sn = sn.Sn;
                            storeDetail.Number = 1;
                            RF.Save(storeDetail);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成入库明细
        /// </summary>
        /// <param name="lineNo">行号</param>
        /// <param name="item">备件验收明细</param>
        /// <param name="accept">备件验收</param>
        /// <param name="sparePartStoreId">备件入库id</param>
        /// <returns>入库明细</returns>
        private StoreDetail GenerateStoreDetail(int lineNo, SparePartAcceptanceDetail item, SparePartAcceptance accept, double sparePartStoreId)
        {
            var storeDetail = new StoreDetail();
            storeDetail.LineNo = lineNo.ToString();
            storeDetail.UnitPrice = item.Price;
            storeDetail.SparePartId = accept.SparePartId;
            storeDetail.SparePartStoreId = sparePartStoreId;
            storeDetail.QualityStatus = QualityStatus.Good;
            storeDetail.InboundStatus = InboundStatus.ToBe;
            storeDetail.PurchaseOrderNo = item.PurchaseOrderNo;
            storeDetail.PurchaseOrderLineNo = item.PurchaseOrderLine;
            return storeDetail;
        }
    }
}
