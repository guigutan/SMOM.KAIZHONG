using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.EMS.SpareParts;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件接收控制器
    /// </summary>
    public partial class SparePartReceiveSnController : DomainController
    {
        /// <summary>
        /// 提交备件接收
        /// </summary>
        /// <param name="receiveIds">接收id</param>
        public virtual void SubmitSparePartReceive(List<double> receiveIds)
        {
            var ct = RT.Service.Resolve<SparePartReceiveController>();
            var receives = ct.GetSparePartReceivesByIds(receiveIds);
            if (receives.Any(p => p.ReceiveBillStatus != ReceiveBillStatus.ToBeSubmitted))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能提交".L10N());
            }
            var allDetails = ct.GetDetailsByReceiveIds(receiveIds);
            var orderItemIds = allDetails.Where(p => p.PurchaseOrderItemId != null).Select(p => (double)p.PurchaseOrderItemId).Distinct().ToList();
            var orderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByIds(orderItemIds);
            var orderIds = orderItems.Select(p => p.PurchaseOrderId).Distinct().ToList();
            var orders = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrdersByIds(orderIds);
            var allOrderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByPurIds(orderIds);
            var now = RF.Find<SparePartReceive>().GetDbTime();
            var allLotList = ct.GetReceiveLotList(receiveIds);
            var allSnList = ct.GetReceiveSnList(receiveIds);
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var receive in receives)
                {
                    var details = allDetails.Where(p => p.SparePartReceiveId == receive.Id).ToList();
                    if (!details.Any())
                    {
                        throw new ValidationException("接收明细必须有数据".L10N());
                    }
                    if (details.Any(p => p.Qty == 0))
                    {
                        throw new ValidationException("接收明细的接收数量不能为0".L10N());
                    }
                    if (details.Any(p => p.RecivedQty != p.Qty))
                    {
                        throw new ValidationException("接收明细的已接收数量必须等于接收数量".L10N());
                    }
                    receive.ReceiveBillStatus = ReceiveBillStatus.Completed;
                    receive.ReceiverId = RT.IdentityId;
                    receive.ReceiveDateTime = now;
                    if (receive.ReceiveType == ReceiveType.Purchase)
                    {
                        //更新采购订单行
                        UpdateOrderItem(orders, allOrderItems, details);
                    }
                    //生成验收数据
                    GenerateAcceptance(receive, details, allLotList, allSnList);

                    //生成入库数据
                    GenerateStore(receive, details, allLotList, allSnList);
                }
                RF.Save(receives);
                trans.Complete();
            }
        }

        /// <summary>
        /// 删除接收批次
        /// </summary>
        /// <param name="selectedIds">批次id</param>
        public virtual void DeleteReceiveLot(List<double> selectedIds)
        {
            DB.Delete<SparePartReceiveLot>().Where(p => selectedIds.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 删除接收序列号
        /// </summary>
        /// <param name="selectedIds">序列号id</param>
        public virtual void DeleteReceiveSn(List<double> selectedIds)
        {
            DB.Delete<SparePartReceiveSn>().Where(p => selectedIds.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 更新采购订单行
        /// </summary>
        /// <param name="orders">采购订单</param>
        /// <param name="orderItems">采购订单行</param>
        /// <param name="details">接收明细</param>
        private void UpdateOrderItem(EntityList<PurchaseOrder> orders, EntityList<PurchaseOrderItem> orderItems, List<SparePartReceiveDetail> details)
        {
            foreach (var detail in details)
            {
                var orderItem = orderItems.FirstOrDefault(p => p.Id == detail.PurchaseOrderItemId);
                if (orderItem == null)
                {
                    throw new ValidationException("找不到id为:{0}的采购订单行".L10nFormat(detail.PurchaseOrderItemId));
                }
                var order = orders.FirstOrDefault(p => p.Id == orderItem.PurchaseOrderId);
                if (order == null)
                {
                    throw new ValidationException("找不到id为:{0}的采购订单".L10nFormat(orderItem.PurchaseOrderId));
                }
                //更新对应采购订单行的【接收数量】为原来的值加上本次接收数量
                orderItem.ReciveQty += detail.Qty;
                orderItem.TotalReciveQty += detail.Qty;
                //更新后【接收数量+验收数量+入库数量】不能大于采购数量
                if (orderItem.ReciveQty + orderItem.AcceptanceQty + orderItem.InboundQty > orderItem.Qty)
                {
                    throw new ValidationException("采购订单：{0}，行号：{1}，收货数量不能大于采购数量".L10nFormat(orderItem.PurchaseOrder?.OrderNo, orderItem.LineNo));
                }
                //当【接收数量+验收数量+入库数量】大于0小于【采购数量】时，更新采购订单行的状态为【部分收货】，同时更新主表的订单状态为【部分收货】；
                if (orderItem.ReciveQty + orderItem.AcceptanceQty + orderItem.InboundQty < orderItem.Qty)
                {
                    orderItem.Status = PurchaseOrderStatus.PartRecive;
                    order.PurchaseOrderStatus = PurchaseOrderStatus.PartRecive;
                    RF.Save(order);
                }
                //当【接收数量 + 验收数量 + 入库数量】等于【采购数量】时，更新采购订单行的状态为【已收货】
                if (orderItem.ReciveQty + orderItem.AcceptanceQty + orderItem.InboundQty == orderItem.Qty)
                {
                    orderItem.Status = PurchaseOrderStatus.Recived;
                    //采购订单行全部状态为【已收货】时，更新主表状态为【已收货】
                    var allOrderItems = orderItems.Where(p => p.PurchaseOrderId == orderItem.PurchaseOrderId).ToList();
                    if (allOrderItems.All(p => p.Status == PurchaseOrderStatus.Recived))
                    {
                        order.PurchaseOrderStatus = PurchaseOrderStatus.Recived;
                    }
                    else
                    {
                        order.PurchaseOrderStatus = PurchaseOrderStatus.PartRecive;
                    }
                    RF.Save(order);
                }
                RF.Save(orderItem);
            }
        }

        /// <summary>
        /// 生成验收数据
        /// </summary>
        /// <param name="receive">接收单</param>
        /// <param name="details">接收明细</param>
        /// <param name="allLotList">批次明细</param>
        /// <param name="allSnList">序列号明细</param>
        private void GenerateAcceptance(SparePartReceive receive, List<SparePartReceiveDetail> details, EntityList<SparePartReceiveLot> allLotList,
            EntityList<SparePartReceiveSn> allSnList)
        {
            var acceptanceList = details.Where(p => !p.ExemptionInspect).ToList();
            var dicAcceptances = acceptanceList.GroupBy(p => new { p.SparePartId, p.SupplierId }).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var dicAccept in dicAcceptances)
            {
                var acceptance = new SparePartAcceptance();
                acceptance.FactoryId = receive.FactoryId;
                acceptance.DepartmentId = receive.DepartmentId;
                acceptance.AcceptanceNo = RT.Service.Resolve<CommonController>().GetNo<SparePartAcceptance>("备件验收");
                acceptance.ReceiveQty = dicAccept.Value.Sum(p => p.Qty);
                acceptance.SparePartId = dicAccept.Key.SparePartId;
                acceptance.SparePartReceiveId = receive.Id;
                acceptance.ApprovalStatus = ApprovalStatus.Draft;
                acceptance.SupplierId = dicAccept.Key.SupplierId;
                RF.Save(acceptance);
                foreach (var item in dicAccept.Value)
                {
                    var acceptanceDetail = new SparePartAcceptanceDetail();
                    acceptanceDetail.SparePartAcceptanceId = acceptance.Id;
                    acceptanceDetail.Price = item.Price;
                    acceptanceDetail.ReceiveQty = item.Qty;
                    acceptanceDetail.PurchaseOrderId = item.PurchaseOrderId;
                    acceptanceDetail.PurchaseOrderItemId = item.PurchaseOrderItemId;
                    acceptanceDetail.WarehouseId = item.WarehouseId;
                    RF.Save(acceptanceDetail);
                    var lotList = allLotList.Where(p => p.SparePartReceiveDetailId == item.Id).ToList();
                    foreach (var lot in lotList)
                    {
                        var acceptanceLot = new SparePartAcceptanceLot();
                        acceptanceLot.AccepDtlId = acceptanceDetail.Id;
                        acceptanceLot.LotNo = lot.LotNo;
                        acceptanceLot.Qty = lot.Qty;
                        RF.Save(acceptanceLot);
                    }
                    var snList = allSnList.Where(p => p.SparePartReceiveDetailId == item.Id).ToList();
                    foreach (var sn in snList)
                    {
                        var acceptanceSn = new SparePartAcceptanceSn();
                        acceptanceSn.AcceptDtlId = acceptanceDetail.Id;
                        acceptanceSn.Sn = sn.Sn;
                        acceptanceSn.OriginalSn = sn.OriginalSn;
                        RF.Save(acceptanceSn);
                    }
                }
            }
        }

        /// <summary>
        /// 生成入库数据
        /// </summary>
        /// <param name="receive">接收单</param>
        /// <param name="details">接收明细</param>
        /// <param name="allLotList">批次明细</param>
        /// <param name="allSnList">序列号明细</param>
        private void GenerateStore(SparePartReceive receive, List<SparePartReceiveDetail> details, EntityList<SparePartReceiveLot> allLotList,
            EntityList<SparePartReceiveSn> allSnList)
        {
            var storeList = details.Where(p => p.ExemptionInspect).ToList();
            var dicStores = storeList.GroupBy(p => new { p.SupplierId, p.WarehouseId }).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var dicStore in dicStores)
            {
                var sparePartStore = new SparePartStore();
                sparePartStore.StoreCode = RT.Service.Resolve<SparePartController>().GetStoreCode();
                sparePartStore.InboundType = GetInboundType(receive.ReceiveType);
                sparePartStore.ReceiveNo = receive.ReceiveNo;
                sparePartStore.InboundStatus = InboundStatus.ToBe;
                sparePartStore.WarehouseId = dicStore.Key.WarehouseId;
                sparePartStore.SupplierId = dicStore.Key.SupplierId;
                RF.Save(sparePartStore);
                var lineNo = 0;
                foreach (var item in dicStore.Value)
                {
                    var lotList = allLotList.Where(p => p.SparePartReceiveDetailId == item.Id).ToList();
                    var snList = allSnList.Where(p => p.SparePartReceiveDetailId == item.Id).ToList();
                    if (!lotList.Any() && !snList.Any())
                    {
                        lineNo++;
                        var storeDetail = GenerateStoreDetail(lineNo, item, sparePartStore.Id);
                        storeDetail.Number = item.RecivedQty;
                        RF.Save(storeDetail);
                    }
                    else
                    {
                        foreach (var lot in lotList)
                        {
                            lineNo++;
                            var storeDetail = GenerateStoreDetail(lineNo, item, sparePartStore.Id);
                            storeDetail.BatchNumber = lot.LotNo;
                            storeDetail.Number = lot.Qty;
                            RF.Save(storeDetail);
                        }
                        foreach (var sn in snList)
                        {
                            lineNo++;
                            var storeDetail = GenerateStoreDetail(lineNo, item, sparePartStore.Id);
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
        /// <param name="item">备件接收明细</param>
        /// <param name="sparePartStoreId">备件入库id</param>
        /// <returns>入库明细</returns>
        private StoreDetail GenerateStoreDetail(int lineNo, SparePartReceiveDetail item, double sparePartStoreId)
        {
            var storeDetail = new StoreDetail();
            storeDetail.LineNo = lineNo.ToString();
            storeDetail.UnitPrice = item.Price;
            storeDetail.SparePartId = item.SparePartId;
            storeDetail.SparePartStoreId = sparePartStoreId;
            storeDetail.QualityStatus = QualityStatus.Good;
            storeDetail.InboundStatus = InboundStatus.ToBe;
            storeDetail.PurchaseOrderNo = item.PurchaseOrderNo;
            storeDetail.PurchaseOrderLineNo = item.PurchaseOrderLine;
            return storeDetail;
        }


        /// <summary>
        /// 获取入库类型
        /// </summary>
        /// <param name="receiveType">接收类型</param>
        /// <returns>入库类型</returns>
        public virtual SparePartInboundType GetInboundType(ReceiveType receiveType)
        {
            switch (receiveType)
            {
                case ReceiveType.Purchase:
                    return SparePartInboundType.Po;
                case ReceiveType.Giveaway:
                    return SparePartInboundType.Gift;
                case ReceiveType.Lease:
                    return SparePartInboundType.Lease;
                case ReceiveType.Customer:
                    return SparePartInboundType.Guest;
                case ReceiveType.Outsourced:
                    return SparePartInboundType.Outsourced;
                default:
                    return SparePartInboundType.Other;
            }
        }

    }
}
