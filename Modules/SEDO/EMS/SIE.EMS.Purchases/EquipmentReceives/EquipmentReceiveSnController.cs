using SIE.Common.Prints;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Warehouses;
using SIE.Equipments.Enums;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收序列号控制器
    /// </summary>
    public partial class EquipmentReceiveSnController : DomainController
    {
        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="receiveId">接收id</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>序列号明细</returns>
        public virtual EntityList<EquipmentReceiveSn> GetReceiveSnInfo(double receiveId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<EquipmentReceiveSn>().Join<EquipmentReceiveDetail>((a, b) => a.EquipmentReceiveDetailId == b.Id && b.EquipmentReceiveId == receiveId)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="receiveIds">接收id</param>
        /// <returns>序列号明细</returns>
        public virtual EntityList<EquipmentReceiveSn> GetReceiveSnList(List<double> receiveIds)
        {
            return Query<EquipmentReceiveSn>().Join<EquipmentReceiveDetail>((a, b) => a.EquipmentReceiveDetailId == b.Id && receiveIds.Contains(b.EquipmentReceiveId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="snIds">序列号明细id</param>
        /// <returns>序列号明细</returns>
        public virtual EntityList<EquipmentReceiveSn> GetEquipmentReceiveSnsByIds(List<double> snIds)
        {
            return snIds.SplitContains(ids => Query<EquipmentReceiveSn>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="entityType">打印条码类型</param>
        /// <param name="info">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <returns>打印模板列表</returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplatesByType(string entityType, PagingInfo info = null, string keyword = "")
        {
            var query = Query<PrintTemplate>().Where(p => p.EntityType.Contains(entityType));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.FileName.Contains(keyword));
            }
            return query.ToList(info);
        }

        /// <summary>
        /// 明细获取接收仓库
        /// </summary>
        /// <param name="isCost">非成本仓</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>接收仓库</returns>
        public virtual EntityList<Warehouse> DetailGetWarehouses(bool isCost, PagingInfo pagingInfo, string keyword)
        {
            return Query<Warehouse>()
                .WhereIf(isCost,p => p.GetProperty(WarehouseExtension.IsZeroCostProperty) == isCost)
                .WhereIf(!isCost, p => p.GetProperty(WarehouseExtension.IsZeroCostProperty) == isCost|| p.GetProperty(WarehouseExtension.IsZeroCostProperty) == null)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 生成开箱验收数据
        /// </summary>
        /// <param name="receive">设备接收</param>
        public virtual EquipmentAcceptance GetNewEquipmentAcceptance(EquipmentReceive receive)
        {
            if (receive == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var acceptance = new EquipmentAcceptance();
            acceptance.FactoryId = receive.FactoryId;
            acceptance.DepartmentId = receive.DepartmentId;
            acceptance.AcceptanceNo = RT.Service.Resolve<CommonController>().GetNo<EquipmentAcceptance>("设备开箱验收");
            acceptance.ApprovalStatus = ApprovalStatus.Draft;
            acceptance.ReceiveType = receive.ReceiveType;
            return acceptance;
        }

        /// <summary>
        /// 提交设备接收
        /// </summary>
        /// <param name="receiveIds">接收id</param>
        public virtual void SubmitEquipmentReceive(List<double> receiveIds)
        {
            var receives = RT.Service.Resolve<EquipmentReceiveController>().GetEquipmentReceivesByIds(receiveIds);
            //只有状态为【待提交】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (receives.Any(p => p.ReceiveBillStatus != ReceiveBillStatus.ToBeSubmitted))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能提交".L10N());
            }
            var allDetails = RT.Service.Resolve<EquipmentReceiveController>().GetDetailsByReceiveIds(receiveIds);
            var orderItemIds = allDetails.Where(p => p.PurchaseOrderItemId != null).Select(p => (double)p.PurchaseOrderItemId).Distinct().ToList();
            var orderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByIds(orderItemIds);
            var orderIds = orderItems.Select(p => p.PurchaseOrderId).Distinct().ToList();
            var orders = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrdersByIds(orderIds);
            var allOrderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByPurIds(orderIds);
            var now = RF.Find<EquipmentReceive>().GetDbTime();
            var allSnList = GetReceiveSnList(receiveIds);
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var receive in receives)
                {
                    var details = allDetails.Where(p => p.EquipmentReceiveId == receive.Id).ToList();
                    //提交时校验接收明细必须有数据
                    if (!details.Any())
                    {
                        throw new ValidationException("接收明细必须有数据".L10N());
                    }
                    //校验接收明细的已接收数量等于接收数量
                    if (details.Any(p => p.RecivedQty != p.Qty))
                    {
                        throw new ValidationException("接收明细的已接收数量必须等于接收数量".L10N());
                    }
                    //点击后更新状态为【已完成】
                    receive.ReceiveBillStatus = ReceiveBillStatus.Completed;
                    if (receive.ReceiveType == ReceiveType.Purchase)
                    {
                        //接收类型为【采购接收】且不是赠品的数据,更新采购订单行
                        UpdateOrderItem(orders, allOrderItems, details);
                    }
                    //更新主表的接收人和接收时间
                    receive.ReceiverId = RT.IdentityId;
                    receive.ReceiveDateTime = now;
                    //生成开箱验收数据
                    GenerateAcceptance(receive, details, allSnList);
                }
                RF.Save(receives);
                trans.Complete();
            }
        }

        /// <summary>
        /// 更新采购订单行
        /// </summary>
        /// <param name="orders">采购订单</param>
        /// <param name="orderItems">采购订单行</param>
        /// <param name="details">接收明细</param>
        private void UpdateOrderItem(EntityList<PurchaseOrder> orders, EntityList<PurchaseOrderItem> orderItems, List<EquipmentReceiveDetail> details)
        {
            //接收类型为【采购接收】且不是赠品的数据
            var noGiveaways = details.Where(p => !p.Giveaway).ToList();
            foreach (var noGiveaway in noGiveaways)
            {
                var orderItem = orderItems.FirstOrDefault(p => p.Id == noGiveaway.PurchaseOrderItemId);
                if (orderItem == null)
                {
                    throw new ValidationException("找不到id为:{0}的采购订单行".L10nFormat(noGiveaway.PurchaseOrderItemId));
                }
                var order = orders.FirstOrDefault(p => p.Id == orderItem.PurchaseOrderId);
                if (order == null)
                {
                    throw new ValidationException("找不到id为:{0}的采购订单".L10nFormat(orderItem.PurchaseOrderId));
                }
                //更新采购订单行的【接收数量】
                orderItem.ReciveQty += noGiveaway.Qty;
                orderItem.TotalReciveQty += noGiveaway.Qty;
                if (orderItem.ReciveQty + orderItem.AcceptanceQty + orderItem.InboundQty > orderItem.Qty)
                {
                    throw new ValidationException("采购订单：{0}，行号：{1}，收货数量不能大于采购数量".L10nFormat(orderItem.PurchaseOrder?.OrderNo, orderItem.LineNo));
                }
                //更新后【接收数量+验收数量+入库数量】小于【采购数量】时更新对应采购订单行的【状态】为【部分接收】；更新主表状态为【部分接收】
                if (orderItem.ReciveQty + orderItem.AcceptanceQty + orderItem.InboundQty < orderItem.Qty)
                {
                    orderItem.Status = PurchaseOrderStatus.PartRecive;
                    order.PurchaseOrderStatus = PurchaseOrderStatus.PartRecive;
                    RF.Save(order);
                }
                //更新后【接收数量+验收数量+入库数量】等于【采购数量】时更新对应采购订单行的【状态】为【已收货】
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
        /// 生成开箱验收数据
        /// </summary>
        /// <param name="receive">设备接收</param>
        /// <param name="details">接收明细</param>
        /// <param name="allSnList">序列号信息</param>
        private void GenerateAcceptance(EquipmentReceive receive, List<EquipmentReceiveDetail> details, EntityList<EquipmentReceiveSn> allSnList)
        {
            if (receive.AcceptanceType == AcceptanceType.Single)
            {
                //每个接收明细的一个设备就生成一个验收单
                foreach (var detail in details)
                {
                    var snList = allSnList.Where(p => p.EquipmentReceiveDetailId == detail.Id).ToList();
                    foreach (var sn in snList)
                    {
                        var acceptance = GetNewEquipmentAcceptance(receive);
                        acceptance.SupplierId = detail.SupplierId;
                        acceptance.EquipModelId = detail.EquipModelId;
                        acceptance.ReceiveQty = 1;

                        //【设备入库】功能的客户信息，能从【客供接收】带过来
                        if (receive.ReceiveType == ReceiveType.Customer)
                        {
                            acceptance.CustomerId = detail.CustomerId;
                        }

                        RF.Save(acceptance);
                        SaveEquipmentAcceptanceDetail(sn, detail, acceptance.Id);
                    }
                }
            }
            else
            {
                if (receive.ReceiveType == ReceiveType.Customer)
                {
                    //客供接收
                    //根据设备型号编码+客户编码相同的子表数据分组，每一组生成一条开箱验收主表数据，序列号明细的数据对应生成开箱验收行表数据，
                    var dicDetails = details.GroupBy(p => new { p.EquipModelId, p.CustomerId }).ToDictionary(p => p.Key, p => p.ToList());
                    foreach (var dicDetail in dicDetails)
                    {
                        var acceptance = GetNewEquipmentAcceptance(receive);
                        acceptance.CustomerId = dicDetail.Key.CustomerId;
                        acceptance.EquipModelId = dicDetail.Key.EquipModelId;
                        acceptance.ReceiveQty = dicDetail.Value.Sum(p => p.Qty);

                        RF.Save(acceptance);

                        var detailIds = dicDetail.Value.Select(p => p.Id).ToList();
                        var snList = allSnList.Where(p => detailIds.Contains(p.EquipmentReceiveDetailId)).ToList();

                        foreach (var sn in snList)
                        {
                            var detail = dicDetail.Value.FirstOrDefault(p => p.Id == sn.EquipmentReceiveDetailId);

                            if (detail == null)
                            {
                                throw new ValidationException("数据异常，找不到id为:{0}的设备接收明细".L10nFormat(sn.EquipmentReceiveDetailId));
                            }

                            SaveEquipmentAcceptanceDetail(sn, detail, acceptance.Id);
                        }
                    }
                }
                else
                {
                    //根据设备型号编码+供应商编码相同的子表数据分组，每一组生成一条开箱验收主表数据，序列号明细的数据对应生成开箱验收行表数据，
                    var dicDetails = details.GroupBy(p => new { p.EquipModelId, p.SupplierId }).ToDictionary(p => p.Key, p => p.ToList());
                    foreach (var dicDetail in dicDetails)
                    {
                        var acceptance = GetNewEquipmentAcceptance(receive);
                        acceptance.SupplierId = dicDetail.Key.SupplierId;
                        acceptance.EquipModelId = dicDetail.Key.EquipModelId;
                        acceptance.ReceiveQty = dicDetail.Value.Sum(p => p.Qty);

                        RF.Save(acceptance);

                        var detailIds = dicDetail.Value.Select(p => p.Id).ToList();
                        var snList = allSnList.Where(p => detailIds.Contains(p.EquipmentReceiveDetailId)).ToList();

                        foreach (var sn in snList)
                        {
                            var detail = dicDetail.Value.FirstOrDefault(p => p.Id == sn.EquipmentReceiveDetailId);
                            if (detail == null)
                            {
                                throw new ValidationException("数据异常，找不到id为:{0}的设备接收明细".L10nFormat(sn.EquipmentReceiveDetailId));
                            }

                            SaveEquipmentAcceptanceDetail(sn, detail, acceptance.Id);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 生成设备开箱验收明细
        /// </summary>
        /// <param name="sn">序列号数据</param>
        /// <param name="detail">接收明细</param>
        /// <param name="accId">验收id</param>
        private void SaveEquipmentAcceptanceDetail(EquipmentReceiveSn sn, EquipmentReceiveDetail detail, double accId)
        {
            var accDetail = new EquipmentAcceptanceDetail();
            accDetail.EquipmentCode = sn.EquipmentCode;
            accDetail.Giveaway = detail.Giveaway;
            accDetail.Price = detail.Price;
            accDetail.OriginalSn = sn.OriginalSn;
            accDetail.PurchaseOrderItemId = detail.PurchaseOrderItemId;
            accDetail.AcceptanceStatus = null;
            accDetail.WarehouseId = detail.WarehouseId;
            accDetail.WorkshopId = detail.WorkshopId;
            accDetail.PurchaseOrderId = detail.PurchaseOrderId;
            accDetail.EquipmentAcceptanceId = accId;
            RF.Save(accDetail);
        }
    }
}
