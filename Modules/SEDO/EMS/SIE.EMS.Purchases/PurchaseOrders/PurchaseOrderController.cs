using SIE.Common.Sort;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.EMS.DevicePurs;
using SIE.EMS.Projects;
using SIE.EMS.Purchases.Common.Controller;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.EarlierStages;
using SIE.EventMessages.EMS.Purchases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购订单控制器
    /// </summary>
    public partial class PurchaseOrderController : DomainController, IPurchases
    {
        /// <summary>
        /// 查询采购订单
        /// </summary>
        /// <param name="criteria">采购订单查询实体</param>
        /// <returns>采购订单</returns>
        public virtual EntityList<PurchaseOrder> CriteriaPurchaseOrders(PurchaseOrderCriteria criteria)
        {
            var query = Query<PurchaseOrder>();
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
                query.Where(p => p.OrderNo.Contains(criteria.No));
            }
            if (!criteria.PurchaseCategroy.IsNullOrWhiteSpace())
            {
                query.Where(p => p.PurchaseCategroy == criteria.PurchaseCategroy);
            }
            if (criteria.PurchaseObjectType.HasValue)
            {
                query.Where(p => p.PurchaseObjectType == criteria.PurchaseObjectType.Value);
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId.Value);
            }
            if (criteria.PurchaseOrderStatus.HasValue)
            {
                query.Where(p => p.PurchaseOrderStatus == criteria.PurchaseOrderStatus.Value);
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
            var enumList = RT.Service.Resolve<DevicePurController>().GetUserPurchaseObjects(RT.Identity.UserId);
            query.Where(p => enumList.Contains(p.PurchaseObjectType));
            return query.Distinct().OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取采购订单列表
        /// </summary>
        /// <param name="purIds">id列表</param>
        /// <returns>采购订单列表</returns>
        public virtual EntityList<PurchaseOrder> GetPurchaseOrdersByIds(List<double> purIds)
        {
            return purIds.SplitContains(ids => Query<PurchaseOrder>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据供应商获取采购订单
        /// </summary>
        /// <param name="supplierId">供应商id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">订单号</param>
        /// <returns>采购订单</returns>
        public virtual EntityList<PurchaseOrder> GetPurchaseOrdersBySupplier(double supplierId, PagingInfo pagingInfo, string keyword)
        {
            return Query<PurchaseOrder>().Where(p => p.SupplierId == supplierId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.OrderNo.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取付款计划可选订单
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">订单号</param>
        /// <returns>付款计划可选订单</returns>
        public virtual EntityList<PurchaseOrder> GetPaymentPlanOrder(PagingInfo pagingInfo, string keyword)
        {
            return Query<PurchaseOrder>()
                .Join<PaymentTerms>((x,y)=>x.Id==y.PurchaseOrderId&& y.State == PaymentTermsState.NotApplied)
                .Where(p => p.ApprovalStatus == ApprovalStatus.Audited&&p.PurchaseOrderStatus!= PurchaseOrderStatus.Complete )
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.OrderNo.Contains(keyword))
                .Distinct().ToList(pagingInfo);
        }

        /// <summary>
        /// 明细获取采购订单
        /// </summary>
        /// <param name="factoryId">工厂</param>
        /// <param name="departmentId">部门</param>
        /// <param name="types">采购对象</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>采购订单</returns>
        public virtual EntityList<PurchaseOrder> DetailGetOrders(double factoryId, double departmentId, List<PurchaseObjectType> types, PagingInfo pagingInfo, string keyword)
        {
            return Query<PurchaseOrder>().Where(p => p.FactoryId == factoryId && p.DepartmentId == departmentId
                && types.Contains(p.PurchaseObjectType) && p.ApprovalStatus == ApprovalStatus.Audited
                && (p.PurchaseOrderStatus == PurchaseOrderStatus.TobeRecive || p.PurchaseOrderStatus == PurchaseOrderStatus.PartRecive))
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.OrderNo.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 明细获取采购订单(赠品)
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="departmentId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<PurchaseOrder> DetailGetOrders(double factoryId, double departmentId, PagingInfo pagingInfo, string keyword)
        {
            return Query<PurchaseOrder>().Where(p => p.FactoryId == factoryId && p.DepartmentId == departmentId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.OrderNo.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询历史订单
        /// </summary>
        /// <param name="criteria">历史订单查询实体</param>
        /// <returns>历史订单</returns>
        public virtual EntityList<HistoryOrderViewModel> CriteriaHistoryOrders(HistoryOrderViewModelCriteria criteria)
        {
            var query = Query<PurchaseOrder>();
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId.Value);
            }
            if (!criteria.SupplierName.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Supplier.Name.Contains(criteria.SupplierName));
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            var orders = query.Distinct().ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var orderIds = orders.Select(p => p.Id).ToList();
            var details = GetPurDetailsByPurIds(orderIds);
            var list = new EntityList<HistoryOrderViewModel>();
            foreach (var detail in details)
            {
                if (!criteria.ObjectCodeInfo.IsNullOrWhiteSpace() && detail.ObjectCode != criteria.ObjectCodeInfo)
                {
                    continue;
                }
                if (!criteria.ObjectName.IsNullOrWhiteSpace() && detail.ObjectName != criteria.ObjectName)
                {
                    continue;
                }
                var order = orders.FirstOrDefault(p => p.Id == detail.PurchaseOrderId);
                if (order == null)
                {
                    throw new ValidationException("找不到id为:{0}的采购订单".L10nFormat(detail.PurchaseOrderId));
                }
                var model = new HistoryOrderViewModel();
                model.Factory = order.FactoryName;
                model.Department = order.DepartmentName;
                model.OrderNo = order.OrderNo;
                model.SupplierCode = order.SupplierCode;
                model.SupplierName = order.SupplierName;
                model.LineNo = detail.LineNo;
                model.ObjectCode = detail.ObjectCode;
                model.Description = detail.Description;
                model.Specification = detail.Specification;
                model.Qty = detail.Qty;
                model.UnitName = detail.UnitName;
                model.Price = detail.Price;
                model.TaxRate = detail.TaxRate;
                model.PriceNoTax = detail.PriceNoTax;
                model.Amount = detail.Amount;
                model.Remark = detail.Remark;
                model.PurchaseDate = order.CreateDate;
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 获取采购订单
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns>采购订单</returns>
        public virtual PurchaseOrder GetPurchaseOrderById(double orderId)
        {
            return Query<PurchaseOrder>().Where(p => p.Id == orderId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取采购订单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns>采购订单</returns>
        public virtual PurchaseOrder GetPurchaseOrderByNo(string orderNo)
        {
            return Query<PurchaseOrder>().Where(p => p.OrderNo == orderNo).FirstOrDefault();
        }

        /// <summary>
        /// 根据采购订单id列表获取采购订单明细
        /// </summary>
        /// <param name="purIds">采购订单id列表</param>
        /// <returns>采购订单明细</returns>
        public virtual EntityList<PurchaseOrderItem> GetPurDetailsByPurIds(List<double> purIds)
        {
            return Query<PurchaseOrderItem>().Where(p => purIds.Contains(p.PurchaseOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取采购订单明细
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>采购订单明细</returns>
        public virtual EntityList<PurchaseOrderItem> GetPurDetailsByIds(List<double> ids)
        {
            return Query<PurchaseOrderItem>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 明细获取采购订单行
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="receiveType">接收类型</param>
        /// <returns>采购订单</returns>
        public virtual EntityList<PurchaseOrderItem> DetailGetOrderItems(double orderId, PagingInfo pagingInfo, string keyword,
            ReceiveType? receiveType = null)
        {
            var list = Query<PurchaseOrderItem>().Where(p => p.PurchaseOrderId == orderId && (p.Status == PurchaseOrderStatus.TobeRecive || p.Status == PurchaseOrderStatus.PartRecive))
               .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Remark.Contains(keyword))
               .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            //备件接收 当接收类型为赠品接收 单价设为0
            if (receiveType != null && receiveType == ReceiveType.Giveaway)
            {
                list.ForEach(x => x.Price = 0);
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<PurchaseOrderItem> DetailGetOrderItemsNoStatus(double orderId, PagingInfo pagingInfo, string keyword)
        {
            return Query<PurchaseOrderItem>().Where(p => p.PurchaseOrderId == orderId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Remark.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据采购订单id列表获取付款条件列表
        /// </summary>
        /// <param name="purIds">采购订单id列表</param>
        /// <returns>付款条件列表</returns>
        public virtual EntityList<PaymentTerms> GetPaymentTermsByPurIds(List<double> purIds)
        {
            return Query<PaymentTerms>().Where(p => purIds.Contains(p.PurchaseOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取付款计划列表
        /// </summary>
        /// <param name="termsIds">id列表</param>
        /// <returns>付款计划列表</returns>
        public virtual EntityList<PaymentTerms> GetPaymentTermsByIds(List<double> termsIds)
        {
            return termsIds.SplitContains(ids => Query<PaymentTerms>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据订单id获取付款条件列表
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>付款条件列表</returns>
        public virtual EntityList GetPaymentTermsByOrderId(double orderId, PagingInfo pagingInfo, string keyword)
        {
            var entityQueryer = Query<PaymentTerms>().As("pt")
                .Where(p => p.PurchaseOrderId == orderId)
                .Where(x => x.State == PaymentTermsState.NotApplied)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Phase.Contains(keyword));

            var iq = entityQueryer.ToQuery();

            var purchaseOrderIdProperty = entityQueryer.Repository.EntityMeta.ManagedProperties
                .FindProperty(PaymentTerms.PurchaseOrderIdProperty.Name);

            var indexProperty = entityQueryer.Repository.EntityMeta.ManagedProperties
                .FindProperty(SortExtension.INDEX_Property.Name);

            var purchaseOrderIdColumn = iq.MainTable.FindColumn(purchaseOrderIdProperty);
            var maintableindexColumn = iq.MainTable.FindColumn(indexProperty);

            var f = QueryFactory.Instance;

            var subQuery = DB.Query<PaymentTerms>("em")
                .Where(p => p.State == PaymentTermsState.NotApplied)
                .ToQuery();

            IColumnNode purchaseOrderIdColumnOfSubQuery = subQuery.MainTable.FindColumn(purchaseOrderIdProperty);
            IColumnNode indexColumnOfSubQuery = subQuery.MainTable.FindColumn(indexProperty);

            subQuery.Where = subQuery.Where.And(f.Constraint(purchaseOrderIdColumn, purchaseOrderIdColumnOfSubQuery));
            subQuery.Where = subQuery.Where.And(f.Constraint(indexColumnOfSubQuery, BinaryOp.Less, maintableindexColumn));

            iq.Where = f.And(iq.Where, f.Exists(subQuery, true));

            return entityQueryer.Repository.QueryList(iq, pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 创建一个新的采购订单
        /// </summary>
        /// <returns>新的采购订单</returns>
        public virtual PurchaseOrder GetNewPurchaseOrder()
        {
            var pur = new PurchaseOrder();
            pur.OrderNo = RT.Service.Resolve<CommonController>().GetNo<PurchaseOrder>("采购订单");
            pur.PurchaseOrderStatus = PurchaseOrderStatus.TobeRecive;
            pur.ApprovalStatus = ApprovalStatus.Draft;
            pur.Currency = Currency.CNY;
            pur.BuyerId = RT.IdentityId;
            var enumList = RT.Service.Resolve<DevicePurController>().GetUserPurchaseObjects(RT.Identity.UserId);
            if (enumList.Any())
            {
                pur.PurchaseObjectType = enumList.FirstOrDefault();
            }
            return pur;
        }

        /// <summary>
        /// 保存采购订单
        /// </summary>
        /// <param name="pur">采购订单</param>
        public virtual void SavePurchaseOrder(PurchaseOrder pur)
        {
            if (pur == null)
            {
                throw new ValidationException("保存采购订单失败，数据异常".L10N());
            }
            if (pur.PersistenceStatus != PersistenceStatus.New)
            {
                var oldPur = GetById<PurchaseOrder>(pur.Id);
                if (oldPur == null)
                {
                    throw new ValidationException("保存采购订单失败，数据异常".L10N());
                }
                if (oldPur.ApprovalStatus != ApprovalStatus.Draft && oldPur.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("保存采购订单失败，审核状态已不是【待提交】、【驳回】".L10N());
                }
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(pur);

                //保存后再更新品种数和总数量
                var details = GetPurDetailsByPurIds(new List<double> { pur.Id });
                pur.VarietyQuantity = details.GroupBy(p => new { p.ObjectCode, p.Description }).Count();
                pur.TotalQty = details.Sum(p => p.Qty);
                pur.TotalAmount = details.Sum(p => p.Amount);

                //校验付款条件
                var paymentTerms = GetPaymentTermsByPurIds(new List<double> { pur.Id });
                var amount = paymentTerms.Sum(p => p.Amount);
                if (amount > pur.TotalAmount)
                {
                    throw new ValidationException("付款条件累计金额:{0}不能大于订单的总金额:{1}".L10nFormat(amount, pur.TotalAmount));
                }
                var percent = paymentTerms.Sum(p => p.Percent);
                if (percent > 100)
                {
                    throw new ValidationException("付款条件累计比例(%):{0}不能大于100".L10nFormat(percent));
                }
                RF.Save(pur);
                trans.Complete();
            }
        }

        /// <summary>
        /// 删除前校验最新状态
        /// </summary>
        /// <param name="ids">实体id</param>
        public virtual void DeletePurchaseOrder(List<double> ids)
        {
            var purs = GetPurchaseOrdersByIds(ids);
            if (purs.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有审核状态为【待提交】的数据才能删除".L10N());
            }
        }

        /// <summary>
        /// 提交采购
        /// </summary>
        /// <param name="purIds">采购id</param>
        public virtual void SubmitPurOrder(List<double> purIds)
        {
            var config = RT.Service.Resolve<PurchasesApprovalController>().GetApprovalConfigValue(typeof(PurchaseOrder));
            var purs = GetPurchaseOrdersByIds(purIds);
            if (purs.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var ct = RT.Service.Resolve<PurchaseRequisitionController>();
            var details = GetPurDetailsByPurIds(purIds);
            var allPaymentTerms = GetPaymentTermsByPurIds(purIds);
            var allPurDetailIds = details.Select(p => p.PurchaseRequisitionItemId).ToList();
            var allPurDetails = ct.GetPurDetailsByIds(allPurDetailIds);
            var allProjectKeyItemIds = allPurDetails.Select(p => p.ProjectKeyItemId).ToList();
            var allProjectKeyItems = ct.GetProjectKeyItemsByIds(allProjectKeyItemIds);
            foreach (var pur in purs)
            {
                var detail = details.Where(p => p.PurchaseOrderId == pur.Id).ToList();
                if (!detail.Any())
                {
                    throw new ValidationException("采购明细不能为空".L10N());
                }
                var paymentTerms = allPaymentTerms.Where(p => p.PurchaseOrderId == pur.Id).ToList();
                var amount = paymentTerms.Sum(p => p.Amount);
                if (pur.TotalAmount != amount)
                {
                    throw new ValidationException("付款条件的累计金额:{0}不等于采购订单的总金额:{1}".L10nFormat(amount, pur.TotalAmount));
                }
                //验证提交采购
                SubmitExamine(detail, allPurDetails, allProjectKeyItems);
                pur.ApprovalStatus = ApprovalStatus.PendingReview;
            }
            var now = RF.Find<PurchaseOrder>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(purs);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(purIds, typeof(PurchaseOrder).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ExaminePurOrderInner(purIds, ApprovalResult.Pass, "通过".L10N(), purs);
                }
                trans.Complete();
            }
        }


        /// <summary>
        /// 提交采购验证
        /// </summary>
        /// <param name="details">采购订单明细</param>
        /// <param name="purDetails">采购申请明细</param>
        /// <param name="keyItems">项目关键事项</param>
        private void SubmitExamine(List<PurchaseOrderItem> details, EntityList<PurchaseRequisitionItem> purDetails, EntityList<ProjectKeyItem> keyItems)
        {
            foreach (var item in details)
            {
                var purDetail = purDetails.FirstOrDefault(p => p.Id == item.PurchaseRequisitionItemId);
                //关联了项目时
                if (purDetail.ProjectKeyItemId == null)
                {
                    continue;
                }
                var projectKeyItem = keyItems.FirstOrDefault(p => p.Id == purDetail.ProjectKeyItemId.Value);
                if (projectKeyItem == null)
                {
                    throw new ValidationException("找不到id为:{0}的项目事项".L10nFormat(purDetail.ProjectKeyItemId.Value));
                }
                if (projectKeyItem.ActualCost == null)
                {
                    projectKeyItem.ActualCost = item.Amount;
                }
                else
                {
                    projectKeyItem.ActualCost += item.Amount;
                }
                if (projectKeyItem.ActualCost > projectKeyItem.BudgetAmount)
                {
                    throw new ValidationException("事项的成本不能大于事项预算".L10N());
                }
            }
        }

        /// <summary>
        /// 撤回采购
        /// </summary>
        /// <param name="purIds">采购id</param>
        public virtual void CancelPurOrder(List<double> purIds)
        {
            var purs = GetPurchaseOrdersByIds(purIds);
            if (purs.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                purs.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(purs);
                var now = RF.Find<PurchaseOrder>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(purIds, typeof(PurchaseOrder).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核采购
        /// </summary>
        /// <param name="purIds">采购id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExaminePurOrder(List<double> purIds, ApprovalResult value, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExaminePurOrderInner(purIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核采购
        /// </summary>
        /// <param name="purIds">采购id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        /// <param name="purs">数据组</param>
        public virtual void ExaminePurOrderInner(List<double> purIds, ApprovalResult value, string remark, EntityList<PurchaseOrder> purs = null)
        {
            if (purs == null)
            {
                purs = GetPurchaseOrdersByIds(purIds);
                if (!purs.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            if (purs.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            var ct = RT.Service.Resolve<PurchaseRequisitionController>();
            var details = GetPurDetailsByPurIds(purIds);
            var allPurDetailIds = details.Select(p => p.PurchaseRequisitionItemId).ToList();
            var allPurDetails = ct.GetPurDetailsByIds(allPurDetailIds);
            var allProjectKeyItemIds = allPurDetails.Select(p => p.ProjectKeyItemId).ToList();
            var allProjectKeyItems = ct.GetProjectKeyItemsByIds(allProjectKeyItemIds);
            var allProjectIds = allProjectKeyItems.Select(p => p.ProjectId).ToList();
            var allProjects = RT.Service.Resolve<ProjectController>().GetProjectsByIds(allProjectIds);

            var now = RF.Find<PurchaseOrder>().GetDbTime();
            foreach (var pur in purs)
            {
                pur.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
                pur.ApprovedDate = now;
                if (value == ApprovalResult.Pass)
                {
                    var detailList = details.Where(p => p.PurchaseOrderId == pur.Id).ToList();
                    if (!detailList.Any())
                    {
                        throw new ValidationException("采购明细不能为空".L10N());
                    }
                    ExaminePass(detailList, allPurDetails, allProjectKeyItems, allProjects);
                }
            }
            RF.Save(purs);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(purIds, typeof(PurchaseOrder).FullName, value, now, remark);
        }


        /// <summary>
        /// 审核采购通过
        /// </summary>
        /// <param name="details">采购订单明细</param>
        /// <param name="purDetails">采购申请明细</param>
        /// <param name="keyItems">项目关键事项</param>
        /// <param name="projects">项目管理</param>
        private void ExaminePass(List<PurchaseOrderItem> details, EntityList<PurchaseRequisitionItem> purDetails, EntityList<ProjectKeyItem> keyItems, EntityList<Project> projects)
        {
            foreach (var item in details)
            {
                var purDetail = purDetails.FirstOrDefault(p => p.Id == item.PurchaseRequisitionItemId);
                if (purDetail == null)
                {
                    throw new ValidationException("找不到id为:{0}的采购申请明细".L10nFormat(item.PurchaseRequisitionItemId));
                }

                //反写采购申请单采购明细的【已采购数量】和【已采购价格】
                purDetail.PurchasedQty += item.Qty;
                purDetail.PurchasePrice += item.Amount;
                RF.Save(purDetail);

                //反写采购申请单的【中标金额】
                DB.Update<PurchaseRequisition>().Set(p => p.BidAmount, p => p.BidAmount + item.Amount).Where(p => p.Id == purDetail.PurchaseRequisitionId).Execute();

                //关联了项目时，更新项目事项的【事项成本】和项目的【中标金额】
                if (purDetail.ProjectKeyItemId == null)
                {
                    continue;
                }
                var projectKeyItem = keyItems.FirstOrDefault(p => p.Id == purDetail.ProjectKeyItemId.Value);
                if (projectKeyItem == null)
                {
                    throw new ValidationException("找不到id为:{0}的项目事项".L10nFormat(purDetail.ProjectKeyItemId.Value));
                }
                if (projectKeyItem.ActualCost == null)
                {
                    projectKeyItem.ActualCost = item.Amount;
                }
                else
                {
                    projectKeyItem.ActualCost += item.Amount;
                }
                if (projectKeyItem.ActualCost > projectKeyItem.BudgetAmount)
                {
                    throw new ValidationException("事项的成本不能大于事项预算".L10N());
                }
                RF.Save(projectKeyItem);

                //关联了项目时，更新项目的【中标金额】
                var project = projects.FirstOrDefault(p => p.Id == projectKeyItem.ProjectId);
                if (project == null)
                {
                    throw new ValidationException("找不到id为:{0}的项目".L10nFormat(projectKeyItem.ProjectId));
                }
                if (project.ActualAmount == null)
                {
                    project.ActualAmount = item.Amount;
                }
                else
                {
                    project.ActualAmount += item.Amount;
                }
                if (project.ActualAmount > project.Amount)
                {
                    throw new ValidationException("项目的中标金额不能大于项目金额".L10N());
                }
                RF.Save(project);

                //更新预算的【已使用金额】【预占金额】
                RT.Service.Resolve<IBudget>().UpdateBudgetUsedAmount(projectKeyItem.Id, item.Amount);
            }
        }

        /// <summary>
        /// 完成采购
        /// </summary>
        /// <param name="detailIds">明细id</param>
        public virtual void CompletePurDetail(List<double> detailIds)
        {
            var details = GetPurDetailsByIds(detailIds);
            if (!details.Any())
            {
                throw new ValidationException("采购订单明细数据异常".L10N());
            }
            if (details.Any(p => p.Status != PurchaseOrderStatus.TobeRecive && p.Status != PurchaseOrderStatus.PartRecive))
            {
                throw new ValidationException("只有状态为【待收货】、【部分收货】的数据才能完成".L10N());
            }
            if (details.Any(p => p.ReciveQty > 0 || p.AcceptanceQty > 0))
            {
                throw new ValidationException("只有接收数量和验收数量为空或为0时才能完成".L10N());
            }
            var pur = GetById<PurchaseOrder>(details.FirstOrDefault().PurchaseOrderId);
            if (pur == null)
            {
                throw new ValidationException("采购订单数据异常".L10N());
            }
            if (pur.PurchaseOrderStatus != PurchaseOrderStatus.TobeRecive && pur.PurchaseOrderStatus != PurchaseOrderStatus.PartRecive)
            {
                throw new ValidationException("只有状态为【待收货】、【部分收货】的订单数据才能完成".L10N());
            }
            if (pur.ApprovalStatus != ApprovalStatus.Audited)
            {
                throw new ValidationException("只有状态为【已审批】的订单数据才能完成".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                details.ForEach(p => p.Status = PurchaseOrderStatus.Close);
                RF.Save(details);
                if (pur.DetailList.All(p => p.Status == PurchaseOrderStatus.Close || p.Status == PurchaseOrderStatus.Complete))
                {
                    pur.PurchaseOrderStatus = PurchaseOrderStatus.Complete;
                    RF.Save(pur);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 更新采购订单入库状态
        /// </summary>
        /// <param name="updateInfos"></param>
        /// <returns></returns>

        public virtual void UpdatePurchasesInbound(IList<PurchasesUpdateInfo> updateInfos)
        {
            if (!updateInfos.Any())
            {
                return;
            }
            var purchaseOrderList = new EntityList<PurchaseOrder>();
            var poNos = updateInfos.Select(m => m.PoNo).ToList();
            if (poNos.Any())
            {
                purchaseOrderList = poNos.SplitContains(itemsCode =>
               {
                   return Query<PurchaseOrder>().Where(m => itemsCode.Contains(m.OrderNo)).ToList();
               });
            }
            if (!purchaseOrderList.Any())
            {
                return;
            }

            var purchaseOrderItemList = new EntityList<PurchaseOrderItem>();
            var poIds = purchaseOrderList.Select(m => m.Id).ToList();
            if (poIds.Any())
            {
                purchaseOrderItemList = poIds.SplitContains(itemsCode =>
                {
                    return Query<PurchaseOrderItem>().Where(m => itemsCode.Contains(m.PurchaseOrderId)).ToList();
                });
            }


            foreach (var info in updateInfos)
            {
                var po = purchaseOrderList.FirstOrDefault(m => m.OrderNo == info.PoNo);
                if (po == null)//采购单不存在
                {
                    continue;
                }

                var purchaseOrderItem = purchaseOrderItemList.FirstOrDefault(m => m.LineNo == info.PoNoLineNo && m.PurchaseOrderId == po.Id);
                if (purchaseOrderItem != null)
                {
                    purchaseOrderItem.InboundQty += info.InboundQty;
                    purchaseOrderItem.Status = PurchaseOrderStatus.PartRecive;
                    //当验收单号为空时，更新采购订单行的接收数量为原来的值减本次入库的数量
                    if (info.AccNo.IsNullOrEmpty())
                    {
                        purchaseOrderItem.ReciveQty -= info.InboundQty;
                    }
                    //当验收单号不为空时，更新采购订单行的验收数量为原来的值减本次入库的数量
                    if (!info.AccNo.IsNullOrEmpty())
                    {
                        purchaseOrderItem.AcceptanceQty -= info.InboundQty;
                    }
                    purchaseOrderItem.Status = PurchaseOrderStatus.PartRecive;
                    if (purchaseOrderItem.InboundQty == purchaseOrderItem.Qty)
                    {
                        purchaseOrderItem.Status = PurchaseOrderStatus.Complete;
                    }
                }
                if (purchaseOrderItemList.Where(m => m.PurchaseOrderId == po.Id)
                    .All(m => (m.Status == PurchaseOrderStatus.Close || m.Status == PurchaseOrderStatus.Complete)))
                {
                    po.PurchaseOrderStatus = PurchaseOrderStatus.Complete;
                }
            }
            RF.Save(purchaseOrderItemList);
            RF.Save(purchaseOrderList);
        }


        /// <summary>
        /// 校验项目编号所关联的采购申请对应的采购订单是否已经关闭
        /// </summary>
        /// <param name="projectIds"></param>
        /// <returns>返回采购订单号</returns>
        public virtual List<string> IsPurchaseOrderClose(IList<double> projectIds)
        {
            //待收货,部分收货，已收货，采购订单都不算关闭
            List<PurchaseOrderStatus> status = new List<PurchaseOrderStatus>() {
                    PurchaseOrderStatus.TobeRecive,
                    PurchaseOrderStatus.PartRecive,
                    PurchaseOrderStatus.Recived
            };
            var query = Query<PurchaseRequisition>()
                 .LeftJoin<PurchaseRequisitionItem>("pritem", (x, y) => x.Id == y.PurchaseRequisitionId)
                 .LeftJoin<PurchaseRequisitionItem, PurchaseOrderItem>("poitem", (x, y) => x.Id == y.PurchaseRequisitionItemId)
                 .LeftJoin<PurchaseOrderItem, PurchaseOrder>("po", (x, y) => x.PurchaseOrderId == y.Id)
                .Where(x => projectIds.Contains((double)x.ProjectId))
                .Where<PurchaseOrder>((x, y) => status.Contains(y.PurchaseOrderStatus));
            return query.Select<PurchaseOrder>((x, y) => new { y.OrderNo }).ToList<String>().ToList();
        }
    }
}
