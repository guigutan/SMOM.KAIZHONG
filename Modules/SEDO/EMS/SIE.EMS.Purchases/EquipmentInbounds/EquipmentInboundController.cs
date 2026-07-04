using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.EquipmentInbounds
{
    /// <summary>
    /// 设备入库控制器
    /// </summary>
    public partial class EquipmentInboundController : DomainController
    {
        /// <summary>
        /// 查询设备入库
        /// </summary>
        /// <param name="criteria">设备入库查询</param>
        /// <returns>设备入库</returns>
        public virtual EntityList<EquipmentInbound> CriteriaEquipmentInbounds(EquipmentInboundCriteria criteria)
        {
            var query = Query<EquipmentInbound>();
            if (!criteria.No.IsNullOrWhiteSpace())
            {
                query.Where(p => p.InboundNo.Contains(criteria.No));
            }
            if (criteria.InboundType.HasValue)
            {
                query.Where(p => p.InboundType == criteria.InboundType.Value);
            }
            if (!criteria.EquipmentCode.IsNullOrWhiteSpace())
            {
                query.Join<EquipmentInboundDetail>("b", (a, b) => a.Id == b.EquipmentInboundId && b.EquipmentCode.Contains(criteria.EquipmentCode));
            }
            if (criteria.EquipModelId.HasValue)
            {
                query.Where(p => p.EquipModelId == criteria.EquipModelId.Value);
            }
            if (!criteria.PurchaseOrderNo.IsNullOrWhiteSpace())
            {
                query.Join<EquipmentInboundDetail>("b1", (a, b1) => a.Id == b1.EquipmentInboundId)
                    .Join<EquipmentInboundDetail, PurchaseOrder>((b1, c) => b1.PurchaseOrderId == c.Id && c.OrderNo.Contains(criteria.PurchaseOrderNo));
            }
            if (!criteria.AcceptanceNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.AcceptanceNo.Contains(criteria.AcceptanceNo));
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId.Value);
            }
            if (criteria.CustomerId.HasValue)
            {
                query.Where(p => p.CustomerId == criteria.CustomerId.Value);
            }
            if (criteria.InboundStatus.HasValue)
            {
                query.Where(p => p.InboundStatus == criteria.InboundStatus.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            return query.Distinct().OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取入库明细
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList GetEquipmentInboundDetails(double id, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            var query = Query<EquipmentInboundDetail>()
                .Where(p => p.EquipmentInboundId == id);

            var list = query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var codes = list.Where(x => x.EquipAccountId == null).Select(x => x.EquipmentCode).ToList();
            var equipAccounts = RT.Service.Resolve<EquipAccountController>().GetEquipAccounts(codes);
            var equipAccountsDictionary = equipAccounts.ToDictionary(x => x.Code);


            foreach (var equipmentInboundDetail in list
                .Where(x => x.EquipAccountId == null && equipAccountsDictionary.ContainsKey(x.EquipmentCode)))
            {
                equipmentInboundDetail.EquipAccountName = equipAccountsDictionary[equipmentInboundDetail.EquipmentCode].Name;
            }

            return list;
        }

        /// <summary>
        /// 根据id列表获取设备入库信息
        /// </summary>
        /// <param name="inboundIds">id列表</param>
        /// <returns>设备入库信息</returns>
        public virtual EntityList<EquipmentInbound> GetEquipmentInboundsByIds(List<double> inboundIds)
        {
            return inboundIds.SplitContains(ids => Query<EquipmentInbound>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据设备入库id获取设备入库明细
        /// </summary>
        /// <param name="inboundIds">设备入库id</param>
        /// <returns>设备入库明细</returns>
        public virtual EntityList<EquipmentInboundDetail> GetDetailsByInboundIds(List<double> inboundIds)
        {
            return Query<EquipmentInboundDetail>().Where(p => inboundIds.Contains(p.EquipmentInboundId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="selectedIds">选择的id</param>
        public virtual void Warehousing(List<double> selectedIds)
        {
            var inbounds = GetEquipmentInboundsByIds(selectedIds);
            if (inbounds.Any(p => p.InboundStatus != InboundStatus.ToBe))
            {
                throw new ValidationException("只有状态为【待入库】的数据才能入库".L10N());
            }

            if (inbounds.Any(p => !p.WarehouseId.HasValue && !p.WorkshopId.HasValue))
            {
                throw new ValidationException("入库仓库与接收车间不能同时为空".L10N());
            }

            var warehouseNoNullIds = inbounds.Where(p => p.WarehouseId != null).Select(p => p.Id).ToList();

            var warehouseNoNullDetails = GetDetailsByInboundIds(warehouseNoNullIds);

            var allDetails = GetDetailsByInboundIds(selectedIds);

            if (warehouseNoNullDetails.Any(p => p.StorageLocationId == null))
            {
                throw new ValidationException("设备入库仓库时设备明细的库位不能为空".L10N());
            }

            var orderItemIds = allDetails.Where(p => p.PurchaseOrderItemId != null).Select(p => (double)p.PurchaseOrderItemId).Distinct().ToList();
            var orderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByIds(orderItemIds);
            var equipCode = allDetails.Select(p => p.EquipmentCode).ToList();
            var equips = RT.Service.Resolve<EquipAccountController>().GetEquipAccounts(equipCode);
            var now = RF.Find<EquipmentInbound>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var inbound in inbounds)
                {
                    var details = allDetails.Where(p => p.EquipmentInboundId == inbound.Id).ToList();
                    if (!details.Any())
                    {
                        throw new ValidationException("设备明细必须有数据".L10N());
                    }
                    //更新对应的仓库和库位
                    UpdateWarehouseLocation(inbound, details, equips);

                    //更新对应采购订单行的【验收数量】【入库数量】
                    UpdateOrderItem(inbound, details, orderItems);

                    //更新入库单
                    inbound.InboundStatus = InboundStatus.Done;
                    inbound.WarehouseOperatorId = RT.IdentityId;
                    inbound.InboundDateTime = now;
                }
                RF.Save(inbounds);
                trans.Complete();
            }
        }

        /// <summary>
        /// 更新对应的仓库和库位
        /// </summary>
        /// <param name="inbound">入库</param>
        /// <param name="details">入库明细</param>
        /// <param name="equips">设备信息</param>
        private void UpdateWarehouseLocation(EquipmentInbound inbound, List<EquipmentInboundDetail> details, EntityList<EquipAccount> equips)
        {
            foreach (var detail in details)
            {
                var equip = equips.FirstOrDefault(p => p.Code == detail.EquipmentCode);

                if (equip != null)
                {
                    equip.WarehouseId = inbound.WarehouseId;
                    equip.StorageLocationId = detail.StorageLocationId;
                    equip.WorkShopId = inbound.WorkshopId;

                    RF.Save(equip);
                }
                else
                {
                    DB.Update<EquipmentCard>()
                        .Set(p => p.WarehouseId, inbound.WarehouseId)
                        .Set(p => p.StorageLocationId, detail.StorageLocationId)
                        .Set(p => p.WorkShopId, inbound.WorkshopId)
                      .Where(p => p.Code == detail.EquipmentCode)
                      .Execute();
                }
            }
        }

        /// <summary>
        /// 更新对应采购订单行的【验收数量】【入库数量】
        /// </summary>
        /// <param name="inbound">入库</param>
        /// <param name="details">入库明细</param>
        /// <param name="orderItems">订单明细</param>
        private void UpdateOrderItem(EquipmentInbound inbound, List<EquipmentInboundDetail> details, EntityList<PurchaseOrderItem> orderItems)
        {
            if (inbound.ReceiveType == ReceiveType.Purchase)
            {
                var noGiveaways = details.Where(p => !p.Giveaway).ToList();
                foreach (var group in noGiveaways.GroupBy(p => p.PurchaseOrderItemId))
                {
                    //更新对应采购订单行的【验收数量】【入库数量】
                    var orderItem = orderItems.FirstOrDefault(p => p.Id == group.Key);
                    if (orderItem == null)
                    {
                        throw new ValidationException("找不到id为:{0}的采购订单行".L10nFormat(group.Key));
                    }
                    orderItem.AcceptanceQty -= group.Count();
                    orderItem.InboundQty += group.Count();
                    if (orderItem.InboundQty == orderItem.Qty)
                    {
                        orderItem.Status = Enums.PurchaseOrderStatus.Complete;
                    }
                    RF.Save(orderItem);

                }
                var dbOrderItemList = Query<PurchaseOrderItem>().Where(m => m.PurchaseOrderId == orderItems.First().PurchaseOrderId).ToList();
                if (dbOrderItemList.Any() && dbOrderItemList.All(m => m.Status == Enums.PurchaseOrderStatus.Complete))
                {
                    DB.Update<PurchaseOrder>().Set(p => p.PurchaseOrderStatus, Enums.PurchaseOrderStatus.Complete).Where(m => m.Id == orderItems.First().PurchaseOrderId).Execute();
                }
            }
        }

        /// <summary>
        /// 批量选择库位
        /// </summary>
        /// <param name="locationId">库位id</param>
        /// <param name="inboundId">入库id</param>
        public virtual void SelectLocation(double locationId, double inboundId)
        {
            var details = GetDetailsByInboundIds(new List<double> { inboundId });
            if (!details.Any())
            {
                throw new ValidationException("设备明细必须有数据".L10N());
            }
            foreach (var detail in details)
            {
                if (detail.StorageLocationId == null)
                {
                    detail.StorageLocationId = locationId;
                }
            }
            RF.Save(details);
        }
    }
}
