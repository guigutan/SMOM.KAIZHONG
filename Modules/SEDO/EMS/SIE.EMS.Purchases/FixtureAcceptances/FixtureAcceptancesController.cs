using SIE.Common;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Common.Controller;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.Fixtures;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.Models.ApiModels;
using SIE.Fixtures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.Purchases.FixtureAcceptances
{
    /// <summary>
    /// 工治具验收控制器
    /// </summary>
    public class FixtureAcceptancesController : DomainController, IFixtureAcceptance
    {
        /// <summary>
        /// 获取工治具验收列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAcceptance> CriteriaFixtureAcceptances(FixtureAcceptanceCriteria criteria)
        {
            var query = Query<FixtureAcceptance>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }
            if (!criteria.No.IsNullOrWhiteSpace())
            {
                query.Exists<FixtureAcceptanceDetail>(
                    (x, y) => y.Where(p => x.Id == p.FixtureAcceptanceId
                    && p.FixtureReceiveDetail.PurchaseOrder.OrderNo.Contains(criteria.No)));
            }
            if (!criteria.FixtureEncodeCode.IsNullOrWhiteSpace())
            {
                query.Exists<FixtureAcceptanceDetail>(
                    (x, y) => y.Where(p => x.Id == p.FixtureAcceptanceId
                        && p.FixtureReceiveDetail.FixtureEncode.Code.Contains(criteria.FixtureEncodeCode)));
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
        /// 审核工治具验收
        /// </summary>
        /// <param name="acceptIds"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>
        public virtual void ExamineFixtureAcceptancesAccept(List<double> acceptIds, ApprovalResult value, string remark)
        {
            //调用通用审批逻辑
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineFixtureAcceptancesAcceptInner(acceptIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核工治具验收
        /// </summary>
        /// <param name="acceptIds"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>
        /// <param name="accepts">数据组</param>
        public virtual void ExamineFixtureAcceptancesAcceptInner(List<double> acceptIds, ApprovalResult value, string remark, EntityList<FixtureAcceptance> accepts = null)
        {
            if (accepts == null)
            {
                accepts = GetFixtureAcceptanceByIds(acceptIds);
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
            var orderItemIds = allDetails.Where(p => p.FixtureReceiveDetail.PurchaseOrderItem != null).Select(p => (double)p.FixtureReceiveDetail.PurchaseOrderItemId).Distinct().ToList();
            var orderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByIds(orderItemIds);
            var orderIds = orderItems.Select(p => p.PurchaseOrderId).Distinct().ToList();
            var orders = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrdersByIds(orderIds);
            var allOrderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByPurIds(orderIds);
            var now = RF.Find<PurchaseOrder>().GetDbTime();

            var inboundOrders = new EntityList<InboundOrder>();
            foreach (var accept in accepts)
            {
                //校验通过后更新审核状态为【通过】或【驳回】
                accept.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
                if (value == ApprovalResult.Pass)
                {
                    var details = allDetails.Where(p => p.FixtureAcceptanceId == accept.Id).ToList();
                    //更新对应采购订单行的【接收数量】【验收数量】
                    UpdateOrderItem(accept, details, orders, allOrderItems);
                    //编码管控的工治具更新编码类工治具台账的【待入库】字段为原来的值加上本次数量（台账没有这个工治具编码时，新增一条数据）
                    UpdateCodeManageMode(accept);

                    //创建入库单
                    if (accept.PassQty > 0)
                    {
                        var res = GenerateFixtureInbound(accept, details);
                        if (res.Any())
                        {
                            inboundOrders.AddRange(res);
                        }
                    }
                    //存在不合格的工治具编码时候删除已存在的台账
                    if (accept.UnqualifiedQty > 0 && accept.ManageMode == ManageMode.Number)
                    {
                        var detailSns = GetSns(accept);
                        if (detailSns.Any())
                        {
                            var sns = detailSns.Where(p => p.InspectionResult == InspectionResult.Fail).Select(s => s.Sn).ToList();
                            if (sns.Any())
                            {
                                DB.Delete<FixtureAccount>().Where(m => sns.Contains(m.Code)).Execute();
                            }
                        }
                    }
                }
            }
            RF.Save(accepts);
            if (inboundOrders.Any())
            {
                RF.Save(inboundOrders);
            }
            //往审批记录子表插入一条数据
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(FixtureAcceptance).FullName, value, now, remark);
        }

        /// <summary>
        /// 获取验收单SN明细
        /// </summary>
        /// <param name="accept"></param>
        /// <returns></returns>
        private EntityList<FixtureAcceptanceSn> GetSns(FixtureAcceptance accept)
        {
            return Query<FixtureAcceptanceSn>().Join<FixtureAcceptanceDetail>((x, y) => x.FixtureAcceptanceDetailId == y.Id && y.FixtureAcceptanceId == accept.Id).ToList();
        }

        /// <summary>
        /// 更新或新增管理模式为编码管理的工治具台账
        /// </summary>
        /// <param name="fixtureAcceptance"></param>
        private void UpdateCodeManageMode(FixtureAcceptance fixtureAcceptance)
        {
            if (fixtureAcceptance == null)
            {
                return;
            }
            if (fixtureAcceptance.ManageMode == ManageMode.Code)
            {
                var coreFixtureController = RT.Service.Resolve<CoreFixtureController>();
                var exsiedEntity = coreFixtureController.GetFixtureAccountByCodeOrRFID(fixtureAcceptance.FixtureEncodeCode);//获取工治具台账是否存在该编码作为编码类台账的数据 如果不存在则直接报错
                if (exsiedEntity == null)
                {
                    throw new ValidationException("工治具编码【{0}】的台账不存在".L10nFormat(fixtureAcceptance.FixtureEncodeCode));
                }
                //20220607 俊杰提出
                //工治具验收：审核通过时，更新编码类台账的【总数】为原来的值减去验收不合格的数量，
                //【待验收】为原来的值减去接收数（包含合格 + 不合格的），
                //【待入库】原来的值加上验收合格数 
                //【合格数量】增加验收合格数
                exsiedEntity.TotalQty -= fixtureAcceptance.UnqualifiedQty;
                exsiedEntity.ToAccepted -= fixtureAcceptance.ReceiveQty;
                exsiedEntity.WaitShelfQty += fixtureAcceptance.PassQty;
                exsiedEntity.PassQty += fixtureAcceptance.PassQty;
                exsiedEntity.QualityState = FixtureQualityState.Pass;
                RF.Save(exsiedEntity);
            }
        }


        /// <summary>
        /// 创建工治具入库单
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="details"></param>
        private EntityList<InboundOrder> GenerateFixtureInbound(FixtureAcceptance accept, List<FixtureAcceptanceDetail> details)
        {
            var inboundOrders = new EntityList<InboundOrder>();

            var dicExemptionDetail = details.Where(m => m.PassQty > 0).GroupBy(m => m.ReceiveWh).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var key in dicExemptionDetail.Keys)
            {
                var keyDetails = dicExemptionDetail[key].ToList();
                var result = GenarateInboundOrder(accept, keyDetails);
                if (result != null)
                {
                    inboundOrders.Add(result);
                }
            }
            return inboundOrders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="keyDetails"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private InboundOrder GenarateInboundOrder(FixtureAcceptance accept, List<FixtureAcceptanceDetail> keyDetails)
        {
            //接收类型 + 接收仓库相同的数据分组，每一组生成一条入库数据，字段取值：
            if (accept == null || keyDetails == null)
            {
                throw new ValidationException("参数异常".L10N());
            }

            var firstItem = keyDetails.First();
            var inboundType = GetInboundType(accept.ReceiveType);
            var inbound = GetInboundOrder(accept, firstItem, inboundType);
            var fixtureAcceptanceDetailIds = keyDetails.Select(m => m.Id).ToList();

            //如果有 获取本组明细的所有合格SN
            EntityList<FixtureAcceptanceSn> sns = GetPassSn(fixtureAcceptanceDetailIds);
            //生成入库明细
            if (accept.ManageMode == ManageMode.Code)
            {
                foreach (var keyDetail in keyDetails)
                {

                    inbound.InboundOrderFixtureCodeAccountList.Add(new InboundOrderFixtureCodeAccount()
                    {
                        Qty = keyDetail.PassQty,
                        InboundOrderId = inbound.Id,
                    });
                    if (keyDetail.FixtureReceiveDetail.PurchaseOrderId.HasValue)
                    {
                        inbound.InboundOrderPurchaseList.Add(new InboundOrderPurchase()
                        {
                            PoNo = keyDetail.PurOrderNo,
                            PoLine = keyDetail.OrderLineNo,
                            Qty = keyDetail.PassQty,
                            InboundOrderId = inbound.Id,
                            Price = keyDetail.Price,
                        });
                    }
                }
            }
            if (accept.ManageMode == ManageMode.Number)
            {
                //入库单明细
                var codes = sns.Select(m => m.Sn).ToList();
                if (!codes.Any())
                {
                    throw new ValidationException("方法参数,未找到序列号".L10N());
                }
                //获取回所有SN对应的工治具
                var fixtureIdAccounts = Query<FixtureAccount>().Where(m => codes.Contains(m.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var lineNo = 1;
                foreach (var sn in sns)
                {
                    var acccount = fixtureIdAccounts.FirstOrDefault(m => m.Code == sn.Sn);
                    if (acccount != null)
                    {
                        inbound.InboundOrderFixtureIdAccountList.Add(new InboundOrderFixtureIdAccount()
                        {
                            Qty = 1,
                            InboundOrderId = inbound.Id,
                            FixtureIDAccountId = acccount.Id,
                            LineNo = lineNo.ToString(),
                            PoNo = sn.PurOrderNo,
                            PoLineNo = sn.OrderLineNo,
                            Price = sn.Price,
                            Rfid = "",
                            OriginalSerialNumber = sn.OriginalSn,

                        });
                        acccount.QualityState = FixtureQualityState.Pass;
                    }
                    lineNo++;
                }
                if (fixtureIdAccounts.Any())
                {
                    RF.Save(fixtureIdAccounts);
                }
            }
            return inbound;
        }

        /// <summary>
        /// 生成入库单主表
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="firstItem"></param>
        /// <param name="inboundType"></param>
        /// <returns></returns>
        private InboundOrder GetInboundOrder(FixtureAcceptance accept, FixtureAcceptanceDetail firstItem, FixtureInboundType inboundType)
        {
            if (accept == null || firstItem == null)
            {
                throw new ValidationException("方法参数异常".L10N());
            }

            var fixtureReceiveDetail = firstItem.FixtureReceiveDetail;
            InboundOrder inboundOrder = new InboundOrder()
            {
                No = RT.Service.Resolve<CommonController>().GetNo<InboundOrder>("工治具入库单号"),
                SupplierId = fixtureReceiveDetail.SupplierId,
                CustomerId = fixtureReceiveDetail.CustomerId,
                FixtureEncodeId = fixtureReceiveDetail.FixtureEncodeId,
                InboundStatus = InboundStatus.ToBe,
                InboundType = inboundType,
                WarehouseId = fixtureReceiveDetail.WarehouseId,
                Proprietorship = GetProprietorship(inboundType),
                ReceiptOrderNo = accept.FixtureReceive.ReceiveNo,
                AcceptanceOrderNo = accept.AcceptanceNo,
                QualityState = FixtureQualityState.Pass,
                Qty = accept.PassQty,
            };
            inboundOrder.GenerateId();
            return inboundOrder;
        }

        /// <summary>
        /// 获取合格的SN
        /// </summary>
        /// <param name="fixtureAcceptanceDetailIds"></param>
        /// <returns></returns>
        private EntityList<FixtureAcceptanceSn> GetPassSn(List<double> fixtureAcceptanceDetailIds)
        {
            return Query<FixtureAcceptanceSn>().Where(m => fixtureAcceptanceDetailIds.Contains(m.FixtureAcceptanceDetailId)
            && m.InspectionResult == InspectionResult.Pass).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产权归属
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
        /// 获取出库类型转换
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
        /// 
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="details"></param>
        /// <param name="orders"></param>
        /// <param name="orderItems"></param>
        private void UpdateOrderItem(FixtureAcceptance accept, List<FixtureAcceptanceDetail> details, EntityList<PurchaseOrder> orders, EntityList<PurchaseOrderItem> orderItems)
        {
            if (accept.ReceiveType == ReceiveType.Purchase)
            {
                foreach (var group in details.GroupBy(p => p.FixtureReceiveDetail.PurchaseOrderItemId))
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
        /// 撤回
        /// </summary>
        /// <param name="acceptIds"></param>
        public virtual void CancelAccept(List<double> acceptIds)
        {
            var accepts = GetFixtureAcceptanceByIds(acceptIds);
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                accepts.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(accepts);
                var now = RF.Find<FixtureAcceptance>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(FixtureAcceptance).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 提交工治具验收
        /// </summary>
        /// <param name="acceptIds"></param>
        public virtual void SubmitFixtureAcceptances(List<double> acceptIds)
        {
            var config = RT.Service.Resolve<PurchasesApprovalController>().GetApprovalConfigValue(typeof(FixtureAcceptance));
            var accepts = GetFixtureAcceptanceByIds(acceptIds);
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var allDetails = GetDetailsByAcceptIds(acceptIds);
            var allItems = GetAcceptItemsByAcceptIds(acceptIds);
            var allAttachments = GetAttachmentByAcceptIds(acceptIds);
            var allSns = GetSnsByAcceptIds(acceptIds);
            foreach (var accept in accepts)
            {
                var details = allDetails.Where(p => p.FixtureAcceptanceId == accept.Id).ToList();
                if (!details.Any())
                {
                    throw new ValidationException("验收明细必须有数据".L10N());
                }
                if (details.Any(m => m.PassQty + m.UnqualifiedQty != m.ReceiveQty))
                {
                    throw new ValidationException("存在至少一条验收明细,合格数+不合格数不等于接收数,请检查".L10N());
                }

                //序列号管控的，校验序列号明细的验收状态全部有值，否则报错：请填写序列号明细的验收状态
                if (accept.ManageMode == ManageMode.Number)
                {
                    var acceptallSns = allSns.Where(p => details.Select(m => m.Id).Contains(p.FixtureAcceptanceDetailId)).ToList();
                    if (!acceptallSns.Any())
                    {
                        throw new ValidationException("序列号明细必须有数据".L10N());
                    }
                    if (acceptallSns.Any(m => !m.InspectionResult.HasValue || m.InspectionResult == 0))
                    {
                        throw new ValidationException("请填写序列号明细的验收结果".L10N());
                    }
                }
                var items = allItems.Where(p => p.FixtureAcceptanceId == accept.Id).ToList();
                var attachments = allAttachments.Where(p => p.OwnerId == accept.Id).ToList();
                if (!items.Any() && !attachments.Any())
                {
                    throw new ValidationException("验收项目和附件不能同时为空".L10N());
                }
                //修改状态待审核
                accept.ApprovalStatus = ApprovalStatus.PendingReview;
            }
            var now = RF.Find<FixtureAcceptance>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存工治具验收
                RF.Save(accepts);
                //生成提交记录
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(FixtureAcceptance).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ExamineFixtureAcceptancesAcceptInner(acceptIds, ApprovalResult.Pass, "通过".L10N(), accepts);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存工治具验收
        /// </summary>
        public virtual void SaveFixtureAcceptance(EntityList<FixtureAcceptance> accepts)
        {
            if (accepts == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("状态为【待提交】、【驳回】时才能修改".L10N());
            }

            // 序列号明细
            var editAcceptSns = new EntityList<FixtureAcceptanceSn>();
            // 获取序列号明细数据(这里只会获取修改过的序列号明细)
            GetAcceptanceSns(accepts, editAcceptSns);
            // 保存验收明细
            var saveDetails = new EntityList<FixtureAcceptanceDetail>();

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                // 先保存修改的序列号明细项
                RF.Save(editAcceptSns);
                // 计算验收明细的合格数和不合格数
                GetAcceptDetails(accepts, saveDetails);
                // 保存验收主表和验收明细
                RF.Save(accepts);
                RF.Save(saveDetails);
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存验收数据
        /// </summary>
        /// <param name="accepts">验收数据</param>
        /// <param name="acceptSns">验收序列号明细</param>
        private void GetAcceptanceSns(EntityList<FixtureAcceptance> accepts, EntityList<FixtureAcceptanceSn> acceptSns)
        {
            // 这里只会获取修改了的序列号明细
            foreach (var accept in accepts)
            {
                var values = accept.ExtValues.Values;
                foreach (var value in values)
                {
                    var snList = value as EntityList<FixtureAcceptanceSn>;
                    if (snList != null)
                    {
                        acceptSns.AddRange(snList);
                    }
                }
            }
        }

        /// <summary>
        /// 获取更新完序列号明细后的验收明细
        /// </summary>
        /// <param name="accepts"></param>
        /// <param name="updateDetails"></param>
        private void GetAcceptDetails(EntityList<FixtureAcceptance> accepts, EntityList<FixtureAcceptanceDetail> updateDetails)
        {
            //获取验收明细
            var acceptIds = accepts.Select(p => p.Id).ToList();
            var fixtureEncodeIds = accepts.Select(p => p.FixtureEncodeId).ToList();
            var fixtureBaseInfos = GetFixtureEncodeBaseInfos(fixtureEncodeIds);
            var allDetails = GetDetailsByAcceptIds(acceptIds);
            var allSns = GetSnsByAcceptIds(acceptIds);

            foreach (var accept in accepts)
            {
                var details = allDetails.Where(p => p.FixtureAcceptanceId == accept.Id).ToList();
                var detailIds = details.Select(p => p.Id).ToList();
                var sns = allSns.Where(p => detailIds.Contains(p.FixtureAcceptanceDetailId)).ToList();
                var fixture = fixtureBaseInfos.FirstOrDefault(p => p.Id == accept.FixtureEncodeId);
                foreach (var detail in details)
                {
                    if (fixture != null && fixture.ManageMode == ManageMode.Number)
                    {
                        detail.PassQty = sns.Where(m => m.FixtureAcceptanceDetailId == detail.Id).Count(p => p.InspectionResult == InspectionResult.Pass);
                        detail.UnqualifiedQty = sns.Where(m => m.FixtureAcceptanceDetailId == detail.Id).Count(p => p.InspectionResult == InspectionResult.Fail);
                        updateDetails.Add(detail);
                    }
                    else
                    {
                        if (detail.ReceiveQty < detail.PassQty || detail.PassQty < 0)
                        {
                            throw new ValidationException("保存失败，合格数应小于接收数量且大于或等于0".L10N());
                        }
                        if (detail.ReceiveQty < detail.UnqualifiedQty || detail.UnqualifiedQty < 0)
                        {
                            throw new ValidationException("保存失败，不合格数应小于接收数量且大于或等于0".L10N());
                        }
                    }
                }
                accept.PassQty = details.Sum(p => p.PassQty);
                accept.UnqualifiedQty = details.Sum(p => p.UnqualifiedQty);
            }
        }


        /// <summary>
        /// 获取工治具信息
        /// </summary>
        /// <param name="fixtureEncodeIds">工治具id</param>
        /// <returns></returns>
        public virtual List<FixtureEncodeBaseInfo> GetFixtureEncodeBaseInfos(List<double> fixtureEncodeIds)
        {
            List<FixtureEncodeBaseInfo> baseInfos = new List<FixtureEncodeBaseInfo>();
            fixtureEncodeIds.SplitDataExecute(temps =>
            {
                var list = Query<FixtureEncode>().LeftJoin<FixtureModel>((fe, fm) => fe.FixtureModelId == fm.Id)
                .Select<FixtureModel>((fe, fm) => new
                {
                    Id = fe.Id,
                    Code = fe.Code,
                    ManageMode = fm.ManageMode,
                }).ToList<FixtureEncodeBaseInfo>().ToList();
                baseInfos.AddRange(list);
            });
            return baseInfos;
        }

        /// <summary>
        /// 根据验收ID获取验收明细
        /// </summary>
        /// <param name="acceptIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAcceptanceDetail> GetDetailsByAcceptIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<FixtureAcceptanceDetail>().Where(p => ids.Contains(p.FixtureAcceptanceId))
            .ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(FixtureAcceptanceDetail.FixtureReceiveDetailProperty)));
        }

        /// <summary>
        /// 根据验收ID获取验收附件
        /// </summary>
        /// <param name="acceptIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAcceptanceAttachment> GetAttachmentByAcceptIds(List<double> acceptIds)
        {
            var nullableIds = acceptIds.Cast<double?>();
            return nullableIds.SplitContains(ids => Query<FixtureAcceptanceAttachment>().Where(p => ids.Contains(p.OwnerId)).ToList());
        }

        /// <summary>
        /// 根据验收ID获取验收SN
        /// </summary>
        /// <param name="acceptIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAcceptanceSn> GetSnsByAcceptIds(List<double> acceptIds)
        {
            return Query<FixtureAcceptanceSn>().Exists<FixtureAcceptanceDetail>((a, b) => b.Where(p => p.Id == a.FixtureAcceptanceDetailId
            && acceptIds.Contains(p.FixtureAcceptanceId))).ToList();
        }

        /// <summary>
        /// 根据验收ID获取验收项目
        /// </summary>
        /// <param name="acceptIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAcceptanceItem> GetAcceptItemsByAcceptIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<FixtureAcceptanceItem>().Where(p => ids.Contains(p.FixtureAcceptanceId))
            .ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据IDS获取工治具接收
        /// </summary>
        /// <param name="acceptIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAcceptance> GetFixtureAcceptanceByIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<FixtureAcceptance>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }


        /// <summary>
        /// 获取验收SN信息
        /// </summary>
        /// <param name="acceptId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAcceptanceSn> GetAcceptanceSnInfo(double acceptId, PagingInfo pagingInfo)
        {
            return Query<FixtureAcceptanceSn>().Exists<FixtureAcceptanceDetail>((a, b) => b.Where(p => p.Id == a.FixtureAcceptanceDetailId && p.FixtureAcceptanceId == acceptId))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取待验收状态的合格数信息
        /// </summary>
        /// <param name="fixtureEncodeIds"></param>
        /// <returns></returns>
        public virtual List<FixtureAcceptanceInfo> GetTobeAcceptanceInfos(List<double> fixtureEncodeIds)
        {
            return Query<FixtureAcceptance>().Where(m => fixtureEncodeIds.Contains(m.FixtureEncodeId)
            && m.ApprovalStatus != ApprovalStatus.Audited).Select(p => new
            {
                Id = p.Id,
                FixtureEncodeId = p.FixtureEncodeId,
                PassQty = p.ReceiveQty
            }).ToList<FixtureAcceptanceInfo>().ToList();
        }
    }
}
