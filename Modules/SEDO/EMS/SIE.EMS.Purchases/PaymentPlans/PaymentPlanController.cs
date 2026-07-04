using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Common.Controller;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.PaymentPlans
{
    /// <summary>
    /// 付款计划控制器
    /// </summary>
    public partial class PaymentPlanController : DomainController
    {
        /// <summary>
        /// 查询付款计划
        /// </summary>
        /// <param name="criteria">付款计划查询实体</param>
        /// <returns>付款计划</returns>
        public virtual EntityList<PaymentPlan> CriteriaPaymentPlans(PaymentPlanCriteria criteria)
        {
            var query = Query<PaymentPlan>();
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
                query.Where(p => p.PaymentOrderNo.Contains(criteria.No));
            }
            if (!criteria.PurchaseOrderNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.PurchaseOrder.OrderNo.Contains(criteria.PurchaseOrderNo));
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId.Value);
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
        /// 根据id列表获取付款计划列表
        /// </summary>
        /// <param name="planIds">id列表</param>
        /// <returns>付款计划列表</returns>
        public virtual EntityList<PaymentPlan> GetPaymentPlansByIds(List<double> planIds)
        {
            return planIds.SplitContains(ids => Query<PaymentPlan>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据订单id获取付款计划列表
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns>付款计划列表</returns>
        public virtual EntityList<PaymentPlan> GetPaymentPlansByOrderId(double orderId)
        {
            return Query<PaymentPlan>().Where(p => p.PurchaseOrderId == orderId).ToList();
        }

        /// <summary>
        /// 创建一个新的付款计划
        /// </summary>
        /// <returns>新的付款计划</returns>
        public virtual PaymentPlan GetNewPaymentPlan()
        {
            var plan = new PaymentPlan();
            plan.PaymentOrderNo = RT.Service.Resolve<CommonController>().GetNo<PaymentPlan>("付款计划");
            plan.ApprovalStatus = ApprovalStatus.Draft;
            return plan;
        }

        /// <summary>
        /// 删除前校验最新状态
        /// </summary>
        /// <param name="ids">实体id</param>
        public virtual void DeletePaymentPlan(List<double> ids)
        {
            var plans = GetPaymentPlansByIds(ids);
            if (plans.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有审核状态为【待提交】的数据才能删除".L10N());
            }
        }

        /// <summary>
        /// 保存付款计划
        /// </summary>
        /// <param name="plan">付款计划</param>
        public virtual void SavePaymentPlan(PaymentPlan plan)
        {
            if (plan == null)
            {
                throw new ValidationException("保存失败，数据异常".L10N());
            }
            if (plan.PersistenceStatus != PersistenceStatus.New)
            {
                var oldPlan = GetById<PaymentPlan>(plan.Id);
                if (oldPlan == null)
                {
                    throw new ValidationException("保存失败，数据异常".L10N());
                }
                if (oldPlan.ApprovalStatus != ApprovalStatus.Draft && oldPlan.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("保存失败，审核状态已不是【待提交】、【驳回】".L10N());
                }
            }
            if (plan.PurchaseOrderId == null && plan.Remark.IsNullOrWhiteSpace())
            {
                throw new ValidationException("采购订单未输入时,备注必输".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(plan);

                //一个采购订单已创建的付款计划的【本次付款金额】累计不能大于采购订单的总金额
                if (plan.PurchaseOrder != null)
                {
                    var plans = GetPaymentPlansByOrderId(plan.PurchaseOrderId.Value);
                    var qty = plans.Sum(p => p.Amount);
                    if (qty > plan.PurchaseOrder.TotalAmount)
                    {
                        throw new ValidationException("采购订单{0}付款金额累计不能大于总金额".L10nFormat(plan.PurchaseOrder.OrderNo));
                    }
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 提交付款计划
        /// </summary>
        /// <param name="planIds">计划id</param>
        public virtual void SubmitPaymentPlan(List<double> planIds)
        {
            var config = RT.Service.Resolve<PurchasesApprovalController>().GetApprovalConfigValue(typeof(PaymentPlan));
            var plans = GetPaymentPlansByIds(planIds);
            if (plans.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            foreach (var plan in plans)
            {
                plan.ApprovalStatus = ApprovalStatus.PendingReview;
            }
            var now = RF.Find<PaymentPlan>().GetDbTime();

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(plans);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(planIds, typeof(PaymentPlan).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ExaminePaymentPlanInner(planIds, ApprovalResult.Pass, "通过".L10N(), plans);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回付款计划
        /// </summary>
        /// <param name="planIds">计划id</param>
        public virtual void CancelPaymentPlan(List<double> planIds)
        {
            var plans = GetPaymentPlansByIds(planIds);
            if (plans.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                plans.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(plans);
                var now = RF.Find<PaymentPlan>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(planIds, typeof(PaymentPlan).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核付款计划
        /// </summary>
        /// <param name="planIds">计划id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExaminePaymentPlan(List<double> planIds, ApprovalResult value, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExaminePaymentPlanInner(planIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核付款计划
        /// </summary>
        /// <param name="planIds">计划id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        /// <param name="plans">数据组</param>
        public virtual void ExaminePaymentPlanInner(List<double> planIds, ApprovalResult value, string remark, EntityList<PaymentPlan> plans = null)
        {
            if (plans == null)
            {
                plans = GetPaymentPlansByIds(planIds);
                if (!plans.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            if (plans.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            var orderIds = plans.Where(p => p.PurchaseOrderId != null).Select(p => (double)p.PurchaseOrderId).Distinct().ToList();
            var orders = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrdersByIds(orderIds);
            var termsIds = plans.Where(p => p.PaymentTermsId != null).Select(p => (double)p.PaymentTermsId).Distinct().ToList();
            var termsList = RT.Service.Resolve<PurchaseOrderController>().GetPaymentTermsByIds(termsIds);

            var now = RF.Find<PaymentPlan>().GetDbTime();
            foreach (var plan in plans)
            {
                plan.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
                plan.ApprovedTime = now;
                if (value == ApprovalResult.Pass && plan.PurchaseOrderId.HasValue)
                {
                    //更新采购订单的已计划总额为原来的值加上本次付款计划的【本次付款金额】
                    var order = orders.FirstOrDefault(p => p.Id == plan.PurchaseOrderId.Value);
                    if (order == null)
                    {
                        throw new ValidationException("找不到id为:{0}的采购订单".L10nFormat(plan.PurchaseOrderId.Value));
                    }
                    order.TotalPlanned += plan.Amount;
                    RF.Save(order);

                    //更新采购订单付款计划的状态为【已申请】
                    var terms = termsList.FirstOrDefault(p => p.Id == plan.PaymentTermsId);
                    if (terms == null)
                    {
                        throw new ValidationException("找不到id为:{0}的付款条件".L10nFormat(plan.PaymentTermsId));
                    }
                    terms.State = PaymentTermsState.Applied;
                    RF.Save(terms);
                }
            }
            RF.Save(plans);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(planIds, typeof(PaymentPlan).FullName, value, now, remark);
        }
    }
}
