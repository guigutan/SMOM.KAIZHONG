using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Common.Controller;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算变更控制器
    /// </summary>
    public partial class BudgetChangeController : DomainController
    {
        /// <summary>
        /// 查询预算变更
        /// </summary>
        /// <param name="criteria">预算变更查询实体</param>
        /// <returns>预算变更</returns>
        public virtual EntityList<BudgetChange> CriteriaBudgetChanges(BudgetChangeCriteria criteria)
        {
            var query = Query<BudgetChange>();
            if (!criteria.BudgetNo.IsNullOrWhiteSpace() || criteria.BudgeGrade.HasValue || !criteria.InvestClass.IsNullOrWhiteSpace() || criteria.Year.HasValue)
            {
                query.Join<Budget>((a, b) => a.BudgetId == b.Id)
                    .WhereIf<Budget>(!criteria.BudgetNo.IsNullOrWhiteSpace(), (a, b) => b.BudgetNo.Contains(criteria.BudgetNo))
                    .WhereIf<Budget>(criteria.BudgeGrade.HasValue, (a, b) => b.BudgeGrade == criteria.BudgeGrade)
                    .WhereIf<Budget>(!criteria.InvestClass.IsNullOrWhiteSpace(), (a, b) => b.InvestClass == criteria.InvestClass)
                    .WhereIf<Budget>(criteria.Year.HasValue, (a, b) => b.Year == criteria.Year);
            }
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
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
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取预算-变更记录
        /// </summary>
        /// <param name="budgetId">预算id</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>预算-变更记录</returns>
        public virtual EntityList<BudgetChange> GetChangeByBudgetId(double budgetId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<BudgetChange>().Where(p => p.BudgetId == budgetId).OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取预算变更列表
        /// </summary>
        /// <param name="budIds">id列表</param>
        /// <returns>预算变更列表</returns>
        public virtual EntityList<BudgetChange> GetBudgetChangesByIds(List<double> budIds)
        {
            return budIds.SplitContains(ids => Query<BudgetChange>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 保存预算变更
        /// </summary>
        /// <param name="budgetChange">预算变更</param>
        public virtual void SaveBudgetChange(BudgetChange budgetChange)
        {
            if (budgetChange == null)
            {
                throw new ValidationException("保存预算变更失败，数据异常".L10N());
            }
            var budget = GetById<Budget>(budgetChange.BudgetId);
            if (budget == null)
            {
                throw new ValidationException("数据异常，id为{0}的预算不存在".L10nFormat(budgetChange.BudgetId));
            }
            if (budgetChange.PersistenceStatus == PersistenceStatus.New)
            {
                //新增数据时，获取预算最新状态校验是否为【已审批】
                if (budget.ApprovalStatus != ApprovalStatus.Audited)
                {
                    throw new ValidationException("预算变更失败，预算{0}的状态已不是【已审批】".L10nFormat(budget.BudgetNo));
                }

                //新增数据时，校验预算是否已存在状态为待提交、待审核、审核中、驳回的变更数据
                var existCount = Query<BudgetChange>().Where(p => p.BudgetId == budget.Id && p.ApprovalStatus != ApprovalStatus.Audited).Count();
                if (existCount > 0)
                {
                    throw new ValidationException("预算:{0}已存在未完结的变更申请".L10nFormat(budget.BudgetNo));
                }

                budgetChange.FactoryId = budget.FactoryId;
                budgetChange.DepartmentId = budget.DepartmentId;
                budgetChange.ChangeContent = "预算".L10N();
                budgetChange.OriginalAmount = budget.BudgetAmount;
                budgetChange.ApprovalStatus = ApprovalStatus.Draft;
            }
            else
            {
                //修改数据时，获取预算变更的【审核状态】是否为：【待提交】、【驳回】
                var oldBudgetChange = GetById<BudgetChange>(budgetChange.Id);
                if (oldBudgetChange == null)
                {
                    throw new ValidationException("修改预算变更失败，id为{0}的预算不存在".L10nFormat(budgetChange.Id));
                }
                if (oldBudgetChange.ApprovalStatus != ApprovalStatus.Draft && oldBudgetChange.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("修改预算变更失败，此预算变更的状态已不是【待提交】、【驳回】".L10N());
                }
            }
            //校验变更后预算金额是否小于预算的【预占金额+已使用金额】（需要后台重新获取最新值）
            if (budgetChange.NewAmount < budget.ReserveAmount + budget.UsedAmount)
            {
                throw new ValidationException("变更后预算金额{0}不能小于预算的【预占金额{1}+已使用金额{2}】"
                    .L10nFormat(budgetChange.NewAmount, budget.ReserveAmount, budget.UsedAmount));
            }
            RF.Save(budgetChange);
        }

        /// <summary>
        /// 删除前校验预算变更最新状态
        /// </summary>
        /// <param name="budIds">预算变更id</param>
        public virtual void DeleteCheckBudgetChangetState(List<double> budIds)
        {
            var budgets = GetBudgetChangesByIds(budIds);
            if (budgets.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能删除".L10N());
            }
        }

        /// <summary>
        /// 提交预算变更
        /// </summary>
        /// <param name="budIds">预算变更id列表</param>
        public virtual void SubmitBudgetChange(List<double> budIds)
        {
            var budgetChanges = GetBudgetChangesByIds(budIds);
            if (budgetChanges.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }

            //获取最新的【预占金额】和【已使用金额】，校验变更后的预算金额是否小于【预占金额+已使用金额】
            if (budgetChanges.Any(p => p.NewAmount < p.ReserveAmount + p.UsedAmount))
            {
                throw new ValidationException("变更后金额不能小于【预占金额+已使用金额】".L10N());
            }

            //配置文件
            var config = RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(typeof(BudgetChange));
            var now = RF.Find<BudgetChange>().GetDbTime();
            //更新状态为【待审核】,增加审核子表一条数据
            budgetChanges.ForEach(p => p.ApprovalStatus = ApprovalStatus.PendingReview);
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                if (config.EnableAudit && config.EnableApproval)
                {
                    RT.Service.Resolve<IWorkFlow>().Submit(budIds);
                }
                RF.Save(budgetChanges);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(budIds, typeof(BudgetChange).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    //审核
                    ExamineBudgetChangeInner(budIds, ApprovalResult.Pass, "通过".L10N(), budgetChanges);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回预算变更
        /// </summary>
        /// <param name="budIds">预算id</param>
        public virtual void CancelBudgetChange(List<double> budIds)
        {
            var budgetChanges = GetBudgetChangesByIds(budIds);
            if (budgetChanges.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview && p.ApprovalStatus != ApprovalStatus.UnderReview))
            {
                throw new ValidationException("只有状态为【待审核】、【审核中】的数据才能撤回".L10N());
            }
            //配置文件
            var config = RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(typeof(BudgetChange));

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                if (config.EnableAudit && config.EnableApproval)
                {
                    RT.Service.Resolve<IWorkFlow>().Recall(budIds);
                }

                //更新审核状态为【待提交】
                budgetChanges.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(budgetChanges);

                //增加审核子表一条数据
                var now = RF.Find<Budget>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(budIds, typeof(BudgetChange).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核预算变更
        /// </summary>
        /// <param name="budIds">预算变更id</param>
        /// <param name="result">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void ExamineBudgetChange(List<double> budIds, ApprovalResult result, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineBudgetChangeInner(budIds, result, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核预算变更
        /// </summary>
        /// <param name="budIds">预算变更id</param>
        /// <param name="result">审核结果</param>
        /// <param name="remark">审核意见</param>
        /// <param name="budgetChanges">数据组</param>
        public virtual void ExamineBudgetChangeInner(List<double> budIds, ApprovalResult result, string remark, EntityList<BudgetChange> budgetChanges = null)
        {
            if (budgetChanges == null)
            {
                budgetChanges = GetBudgetChangesByIds(budIds);
                if (!budgetChanges.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            if (budgetChanges.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            //获取预算
            var budgetIds = budgetChanges.Select(p => p.BudgetId).ToList();
            var budgets = RT.Service.Resolve<BudgetController>().GetBudgetsByIds(budgetIds);
            var status = result == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var now = RF.Find<BudgetChange>().GetDbTime();

            foreach (var budgetChange in budgetChanges)
            {
                budgetChange.ApprovalStatus = status;
                budgetChange.ApprovalTime = now;

                //已审批时，更新预算的【预算金额】为【变更后预算金额】
                var budget = budgets.FirstOrDefault(p => p.Id == budgetChange.BudgetId);
                if (budget == null)
                {
                    throw new ValidationException("数据异常，找不到id为:{0}的预算信息".L10nFormat(budgetChange.BudgetId));
                }
                if (budgetChange.NewAmount < budget.ReserveAmount + budget.UsedAmount)
                {
                    throw new ValidationException("变更后金额不能小于【预占金额+已使用金额】".L10N());
                }
                budget.BudgetAmount = budgetChange.NewAmount;
                RF.Save(budget);
            }
            RF.Save(budgetChanges);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(budIds, typeof(BudgetChange).FullName, result, now, remark);
        }
    }
}
