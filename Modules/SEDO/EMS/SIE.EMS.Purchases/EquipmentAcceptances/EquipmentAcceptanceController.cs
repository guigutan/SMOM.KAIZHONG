using SIE.Common;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Common.Controller;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.EquipmentAcceptances.ViewModels;
using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 设备开箱验收控制器
    /// </summary>
    public partial class EquipmentAcceptanceController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria">设备开箱验收查询实体</param>
        /// <returns>设备开箱验收实体列表</returns>
        public virtual EntityList<EquipmentAcceptance> CriteriaEquipmentAcceptances(EquipmentAcceptanceCriteria criteria)
        {
            var query = Query<EquipmentAcceptance>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }
            if (criteria.EquipModelId.HasValue)
            {
                query.Where(p => p.EquipModelId == criteria.EquipModelId.Value);
            }
            if (!criteria.EquipAccountCode.IsNullOrWhiteSpace() && !criteria.No.IsNullOrWhiteSpace())
            {
                query.Exists<EquipmentAcceptanceDetail>(
                    (x, y) => y.Where(p => x.Id == p.EquipmentAcceptanceId
                    && p.EquipmentCode.Contains(criteria.EquipAccountCode)
                    && p.PurchaseOrder.OrderNo.Contains(criteria.No)));
            }
            else
            {
                if (!criteria.EquipAccountCode.IsNullOrWhiteSpace())
                {
                    query.Exists<EquipmentAcceptanceDetail>(
                        (x, y) => y.Where(p => x.Id == p.EquipmentAcceptanceId
                            && p.EquipmentCode.Contains(criteria.EquipAccountCode)));
                }
                if (!criteria.No.IsNullOrWhiteSpace())
                {
                    query.Exists<EquipmentAcceptanceDetail>(
                        (x, y) => y.Where(p => x.Id == p.EquipmentAcceptanceId
                            && p.PurchaseOrder.OrderNo.Contains(criteria.No)));
                }
            }
            if (criteria.ReceiveType.HasValue)
            {
                query.Where(p => p.ReceiveType == criteria.ReceiveType.Value);
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId.Value);
            }
            if (criteria.CustomerId.HasValue)
            {
                query.Where(p => p.CustomerId == criteria.CustomerId.Value);
            }
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
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
        /// 根据id列表获取设备验收信息
        /// </summary>
        /// <param name="acceptIds">id列表</param>
        /// <returns>设备验收信息</returns>
        public virtual EntityList<EquipmentAcceptance> GetEquipAcceptsByIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<EquipmentAcceptance>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据设备验收id列表获取验收明细列表
        /// </summary>
        /// <param name="acceptIds">设备验收id列表</param>
        /// <returns>设备明细列表</returns>
        public virtual EntityList<EquipmentAcceptanceDetail> GetDetailsByAcceptIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<EquipmentAcceptanceDetail>().Where(p => ids.Contains(p.EquipmentAcceptanceId))
            .ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据设备验收id列表获取验收项目列表
        /// </summary>
        /// <param name="acceptIds">设备验收id列表</param>
        /// <returns>验收项目列表</returns>
        public virtual EntityList<EquipmentAcceptanceItem> GetAcceptItemsByAcceptIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<EquipmentAcceptanceItem>().Where(p => ids.Contains(p.EquipmentAcceptanceId))
            .ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取设备附件列表
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <returns>设备附件列表</returns>
        public virtual EntityList<EquipmentAcceptanceAttachment> GetAttachmentByAcceptIds(List<double> acceptIds)
        {
            var nullableIds = acceptIds.Cast<double?>();

            return nullableIds.SplitContains(ids =>
            {
                return Query<EquipmentAcceptanceAttachment>().Where(p => ids.Contains(p.OwnerId)).ToList();
            });
        }

        /// <summary>
        /// 获取设备开箱验收设备明细
        /// </summary>
        /// <param name="equipmentAcceptanceId">设备开箱验收主表的ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>        
        public virtual EntityList<EquipmentAcceptanceDetailViewModel> GetEquipmentAcceptanceDetailViewModels(
            double equipmentAcceptanceId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<EquipmentAcceptanceDetail>()
                .Where(p => p.EquipmentAcceptanceId == equipmentAcceptanceId);

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(x => x.EquipmentCode.Contains(keyword));
            }

            var equipmentAcceptanceDetails = query.ToList(pagingInfo);

            EntityList<EquipmentAcceptanceDetailViewModel> viewModels =
                new EntityList<EquipmentAcceptanceDetailViewModel>();

            foreach (var item in equipmentAcceptanceDetails)
            {
                viewModels.Add(new EquipmentAcceptanceDetailViewModel()
                {
                    Id = item.Id.ToString(),
                    EquipmentCode = item.EquipmentCode,
                });
            }

            viewModels.SetTotalCount(equipmentAcceptanceDetails.TotalCount);

            return viewModels;
        }

        /// <summary>
        /// 提交设备验收
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        public virtual void SubmitEquipAccept(List<double> acceptIds)
        {
            var config = RT.Service.Resolve<PurchasesApprovalController>().GetApprovalConfigValue(typeof(EquipmentAcceptance));
            var accepts = GetEquipAcceptsByIds(acceptIds);
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var allDetails = GetDetailsByAcceptIds(acceptIds);
            var allItems = GetAcceptItemsByAcceptIds(acceptIds);
            var allAttachments = GetAttachmentByAcceptIds(acceptIds);
            foreach (var accept in accepts)
            {
                var details = allDetails.Where(p => p.EquipmentAcceptanceId == accept.Id).ToList();
                if (!details.Any())
                {
                    throw new ValidationException("设备明细必须有数据".L10N());
                }
                if (details.Any(p => p.AcceptanceStatus == null))
                {
                    throw new ValidationException("请填写设备明细的验收状态".L10N());
                }
                var items = allItems.Where(p => p.EquipmentAcceptanceId == accept.Id).ToList();
                var attachments = allAttachments.Where(p => p.OwnerId == accept.Id).ToList();
                if (!items.Any() && !attachments.Any())
                {
                    throw new ValidationException("验收项目和附件不能同时为空".L10N());
                }
                accept.ApprovalStatus = ApprovalStatus.PendingReview;
            }
            var now = RF.Find<EquipmentAcceptance>().GetDbTime();

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(accepts);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(EquipmentAcceptance).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ExamineEquipAcceptInner(acceptIds, ApprovalResult.Pass, "通过".L10N(), accepts);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回设备验收
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        public virtual void CancelEquipAccept(List<double> acceptIds)
        {
            var accepts = GetEquipAcceptsByIds(acceptIds);
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                accepts.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(accepts);
                var now = RF.Find<EquipmentAcceptance>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(EquipmentAcceptance).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核设备开箱验收
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExamineEquipAccept(List<double> acceptIds, ApprovalResult value, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineEquipAcceptInner(acceptIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核设备开箱验收
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        /// <param name="accepts">数据组</param>
        public virtual void ExamineEquipAcceptInner(List<double> acceptIds, ApprovalResult value, string remark, EntityList<EquipmentAcceptance> accepts = null)
        {
            if (accepts == null)
            {
                accepts = GetEquipAcceptsByIds(acceptIds);
                if (!accepts.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            var allDetails = GetDetailsByAcceptIds(acceptIds);
            var orderItemIds = allDetails.Where(p => p.PurchaseOrderItemId != null).Select(p => (double)p.PurchaseOrderItemId).Distinct().ToList();
            var orderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByIds(orderItemIds);
            var orderIds = orderItems.Select(p => p.PurchaseOrderId).Distinct().ToList();
            var orders = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrdersByIds(orderIds);
            var allOrderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByPurIds(orderIds);
            var now = RF.Find<EquipmentAcceptance>().GetDbTime();

            foreach (var accept in accepts)
            {
                //校验通过后更新审核状态为【通过】或【驳回】
                accept.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
                if (value == ApprovalResult.Pass)
                {
                    var details = allDetails.Where(p => p.EquipmentAcceptanceId == accept.Id).ToList();

                    //更新对应采购订单行的【接收数量】【验收数量】
                    UpdateOrderItem(accept, details, orders, allOrderItems);

                    //更新删除设备数据
                    UpdateDeleteEquipAccount(accept, details);

                    //保存设备入库单
                    SaveEquipmentInbound(accept, details);
                }
            }
            RF.Save(accepts);
            //往审批记录子表插入一条数据
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(EquipmentAcceptance).FullName, value, now, remark);
        }

        /// <summary>
        /// 更新对应采购订单行的【接收数量】【验收数量】
        /// </summary>
        /// <param name="accept">验收</param>
        /// <param name="details">验收明细</param>
        /// <param name="orders">采购订单</param>
        /// <param name="orderItems">订单明细</param>
        private void UpdateOrderItem(EquipmentAcceptance accept, List<EquipmentAcceptanceDetail> details, EntityList<PurchaseOrder> orders, EntityList<PurchaseOrderItem> orderItems)
        {
            //存在接收类型为【采购接收】且不是赠品时
            if (accept.ReceiveType == ReceiveType.Purchase)
            {
                var noGiveaways = details.Where(p => !p.Giveaway).ToList();
                //将采购订单和采购订单行相同的数据进行分组
                foreach (var group in noGiveaways.GroupBy(p => p.PurchaseOrderItemId))
                {
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
                    //更新对应采购订单行的【接收数量】为原来的值减去本组的数量
                    orderItem.ReciveQty -= group.Count();
                    //更新【验收数量】为原来的值加上本组数据中【验收状态】为【合格】的数量
                    orderItem.AcceptanceQty += group.Count(p => p.AcceptanceStatus == InspectionResult.Pass);
                    orderItem.TotalAcceptanceQty += group.Count(p => p.AcceptanceStatus == InspectionResult.Pass);
                    //更新【拒收数量】为原来的值加上本组数据中【验收状态】为【不合格】的数量
                    orderItem.RejectQty += group.Count(p => p.AcceptanceStatus == InspectionResult.Fail);
                    //更新后的【接收数量+验收数量+入库数量】等于0时,更新采购订单行的状态为【待收货】
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
        /// 更新删除设备数据
        /// </summary>
        /// <param name="accept">验收</param>
        /// <param name="details">验收明细</param>
        private void UpdateDeleteEquipAccount(EquipmentAcceptance accept, List<EquipmentAcceptanceDetail> details)
        {
            var passCodes = details.Where(p => p.AcceptanceStatus == InspectionResult.Pass).Select(p => p.EquipmentCode).Distinct().ToList();
            var failCodes = details.Where(p => p.AcceptanceStatus == InspectionResult.Fail).Select(p => p.EquipmentCode).Distinct().ToList();
            if (accept.ReceiveType == ReceiveType.Outsourced)
            {
                //接收类型为【委外返厂】时,【合格】的设备编码的管理状态改成【闲置】
                var count = DB.Update<EquipAccount>().Set(p => p.UseState, Core.Enums.AccountUseState.InIdle).Where(p => passCodes.Contains(p.Code)).Execute();
                if (count != passCodes.Count)
                {
                    throw new ValidationException("数据异常，设备台账数据缺失".L10N());
                }
            }
            else
            {
                var enableEquipCard = RT.Service.Resolve<EquipAccountController>().GetUseCard();
                if (enableEquipCard)
                {
                    //需要立卡时，删除验收状态【不合格】的立卡数据，更新验收状态【合格】的【开箱验收】为【是】
                    var deleteCount = DB.Delete<EquipmentCard>().Where(p => failCodes.Contains(p.Code)).Execute();
                    if (deleteCount != failCodes.Count)
                    {
                        throw new ValidationException("数据异常，设备立卡数据缺失".L10N());
                    }
                    var count = DB.Update<EquipmentCard>().Set(p => p.NeedAcceptance, true).Where(p => passCodes.Contains(p.Code)).Execute();
                    if (count != passCodes.Count)
                    {
                        throw new ValidationException("数据异常，设备立卡数据缺失".L10N());
                    }
                }
                else
                {
                    //不需要立卡时，删除验收状态【不合格】的设备台账数据，更新验收状态【合格】的设备台账的【管理状态】为【闲置】
                    var deleteCount = DB.Delete<EquipAccount>().Where(p => failCodes.Contains(p.Code)).Execute();
                    if (deleteCount != failCodes.Count)
                    {
                        throw new ValidationException("数据异常，设备台账数据缺失".L10N());
                    }
                    var count = DB.Update<EquipAccount>().Set(p => p.UseState, Core.Enums.AccountUseState.InIdle).Where(p => passCodes.Contains(p.Code)).Execute();
                    if (count != passCodes.Count)
                    {
                        throw new ValidationException("数据异常，设备台账数据缺失".L10N());
                    }
                }
            }
        }

        /// <summary>
        /// 获取入库类型
        /// </summary>
        /// <param name="receiveType">接收类型</param>
        /// <returns>入库类型</returns>
        public virtual InboundType GetInboundType(ReceiveType receiveType)
        {
            switch (receiveType)
            {
                case ReceiveType.Purchase:
                    return InboundType.Po;
                case ReceiveType.Giveaway:
                    return InboundType.Gift;
                case ReceiveType.Lease:
                    return InboundType.Lease;
                case ReceiveType.Customer:
                    return InboundType.Guest;
                case ReceiveType.Outsourced:
                    return InboundType.Outsourced;
                default:
                    return InboundType.Other;
            }
        }

        /// <summary>
        /// 保存设备入库单
        /// </summary>
        /// <param name="accept">验收</param>
        /// <param name="details">验收明细</param>
        private void SaveEquipmentInbound(EquipmentAcceptance accept, List<EquipmentAcceptanceDetail> details)
        {
            var passInfos = details.Where(p => p.AcceptanceStatus == InspectionResult.Pass).ToList();

            var equipCode = passInfos.Select(p => p.EquipmentCode).ToList();

            var equips = RT.Service.Resolve<EquipAccountController>().GetEquipAccounts(equipCode);

            //创建接收单在保存时，有校验接收车间和接收仓库不能同时有值

            //接收仓库有值的
            foreach (var group in passInfos.Where(x => x.WarehouseId != null).GroupBy(p => p.WarehouseId))
            {
                CreateEquipmentInbound(accept, equips, group.ToList(), warehouseId: group.Key, workshopId: null);
            }

            //接收车间有值的
            foreach (var group in passInfos.Where(x => x.WorkshopId != null).GroupBy(p => p.WorkshopId))
            {
                CreateEquipmentInbound(accept, equips, group.ToList(), warehouseId: null, workshopId: group.Key);
            }

            //接收车间和接收仓库都为空的不生成入库单
        }

        /// <summary>
        /// 生成入库单
        /// </summary>
        /// <param name="accept">设备验收</param>
        /// <param name="equips">设备列表</param>
        /// <param name="equipmentAcceptanceDetails">设备验收明细列表</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="workshopId">车间ID</param>
        private void CreateEquipmentInbound(EquipmentAcceptance accept, EntityList<EquipAccount> equips,
            List<EquipmentAcceptanceDetail> equipmentAcceptanceDetails, double? warehouseId, double? workshopId)
        {
            var inbound = new EquipmentInbound();
            inbound.InboundNo = RT.Service.Resolve<CommonController>().GetNo<EquipmentInbound>("设备入库");
            inbound.InboundType = GetInboundType(accept.ReceiveType);
            inbound.AcceptanceNo = accept.AcceptanceNo;
            inbound.Qty = equipmentAcceptanceDetails.Count;
            inbound.EquipModelId = accept.EquipModelId;
            inbound.ReceiveType = accept.ReceiveType;

            //接收仓库
            inbound.WarehouseId = warehouseId;

            //接收车间
            inbound.WorkshopId = workshopId;

            inbound.InboundStatus = InboundStatus.ToBe;
            inbound.SupplierId = accept.SupplierId;
            inbound.CustomerId = accept.CustomerId;

            RF.Save(inbound);

            foreach (var code in equipmentAcceptanceDetails)
            {
                var inboundDetail = new EquipmentInboundDetail();
                inboundDetail.EquipmentInboundId = inbound.Id;
                inboundDetail.Giveaway = code.Giveaway;
                inboundDetail.EquipmentCode = code.EquipmentCode;

                var equip = equips.FirstOrDefault(p => p.Code == code.EquipmentCode);

                if (equip != null)
                {
                    inboundDetail.EquipAccountId = equip.Id;
                }

                inboundDetail.Price = code.Price;
                inboundDetail.PurchaseOrderItemId = code.PurchaseOrderItemId;
                inboundDetail.PurchaseOrderId = code.PurchaseOrderId;
                inboundDetail.EquipmentAcceptanceDetailId = code.Id;

                //默认入库库位
                if (warehouseId.HasValue)
                {
                    var storageLocation = RT.Service.Resolve<WarehouseController>().GetStageStorageLocation(warehouseId.Value);

                    if (storageLocation != null)
                    {
                        inboundDetail.StorageLocationId = storageLocation.Id;
                    }
                }

                RF.Save(inboundDetail);
            }
        }

        /// <summary>
        /// 校验验收单的状态
        /// </summary>
        /// <param name="acceptId">验收单id</param>
        public virtual void DetermineEquipAccept(double acceptId)
        {
            var accept = GetById<EquipmentAcceptance>(acceptId);
            if (accept == null)
            {
                throw new ValidationException("数据异常，找不到验收单".L10N());
            }
            if (accept.ApprovalStatus != ApprovalStatus.Draft && accept.ApprovalStatus != ApprovalStatus.Reject)
            {
                throw new ValidationException("此验收单状态已不是【待提交】、【驳回】".L10N());
            }
        }

        /// <summary>
        /// 保存验收
        /// </summary>
        /// <param name="accept">验收单</param>
        public virtual void SaveEquipAccept(EquipmentAcceptance accept)
        {
            if (accept == null)
            {
                throw new ValidationException("保存失败，数据异常".L10N());
            }
            var oldAccept = GetById<EquipmentAcceptance>(accept.Id);
            if (oldAccept == null)
            {
                throw new ValidationException("保存失败，数据异常".L10N());
            }
            if (oldAccept.ApprovalStatus != ApprovalStatus.Draft && oldAccept.ApprovalStatus != ApprovalStatus.Reject)
            {
                throw new ValidationException("保存失败，审核状态已不是【待提交】、【驳回】".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(accept);

                //保存后再更新合格不合格数量
                var allDetails = GetDetailsByAcceptIds(new List<double> { accept.Id });
                accept.PassQty = allDetails.Count(p => p.AcceptanceStatus == InspectionResult.Pass);
                accept.UnqualifiedQty = allDetails.Count(p => p.AcceptanceStatus == InspectionResult.Fail);
                RF.Save(accept);
                trans.Complete();
            }
        }
    }
}