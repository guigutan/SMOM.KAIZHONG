using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Budgets;
using SIE.EMS.EarlierStage.Budgets.Configs;
using SIE.EMS.EarlierStage.Common.Controller;
using SIE.EMS.EarlierStage.Projects;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.EarlierStages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算控制器
    /// </summary>
    public partial class BudgetController : DomainController, IBudget
    {
        /// <summary>
        /// 根据id获取预算
        /// </summary>
        /// <param name="budgetId">id</param>
        /// <returns>预算</returns>
        public virtual Budget GetBudgetById(double budgetId)
        {
            return Query<Budget>().Where(p => p.Id == budgetId).FirstOrDefault();
        }

        /// <summary>
        /// 查询预算
        /// </summary>
        /// <param name="criteria">预算查询实体</param>
        /// <returns>预算</returns>
        public virtual EntityList<Budget> CriteriaBudgets(BudgetCriteria criteria)
        {
            var query = Query<Budget>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }
            if (!criteria.BudgetNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.BudgetNo.Contains(criteria.BudgetNo));
            }
            if (criteria.BudgeGrade.HasValue)
            {
                query.Where(p => p.BudgeGrade == criteria.BudgeGrade.Value);
            }
            if (!criteria.InvestClass.IsNullOrWhiteSpace())
            {
                query.Where(p => p.InvestClass == criteria.InvestClass);
            }
            if (criteria.Year.HasValue)
            {
                query.Where(p => p.Year == criteria.Year);
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
        /// 根据id列表获取预算列表
        /// </summary>
        /// <param name="budgetIds">id列表</param>
        /// <returns>预算列表</returns>
        public virtual EntityList<Budget> GetBudgetsByIds(List<double> budgetIds)
        {
            return budgetIds.SplitContains(ids => Query<Budget>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 获取可变更的预算
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">查询字符</param>
        /// <returns>可变更的预算</returns>
        public virtual EntityList<Budget> GetCanChangeBudgets(PagingInfo pagingInfo, string keyword)
        {
            return Query<Budget>().Where(p => p.ApprovalStatus == ApprovalStatus.Audited)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.BudgetNo.Contains(keyword)||p.BudgetName.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取项目关键事项预算列表
        /// </summary>
        /// <param name="factoryId">工厂</param>
        /// <param name="departmentId">部门</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">预算号</param>
        /// <returns>预算列表</returns>
        public virtual EntityList<Budget> GetKeyItemBudgets(double factoryId, double? departmentId, PagingInfo pagingInfo, string keyword)
        {
            var now = RF.Find<Budget>().GetDbTime();
            var year = GetFiscalYear(now);
            return Query<Budget>().Where(p => p.FactoryId == factoryId && (p.DepartmentId == departmentId)
            && p.ApprovalStatus == ApprovalStatus.Audited && p.Year >= year)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.BudgetNo.Contains(keyword)||p.BudgetName.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取预算管理配置
        /// </summary>
        /// <returns>预算管理配置</returns>
        public virtual BudgetManageConfigValue GetBudgetManageConfig()
        {
            var config = ConfigService.GetConfig(new BudgetManageConfig(), typeof(Budget));
            if (config == null)
            {
                throw new ValidationException("未找到预算管理配置,请检查规则配置".L10N());
            }
            return config;
        }

        /// <summary>
        /// 创建一个新的预算
        /// </summary>
        /// <returns>预算</returns>
        public virtual Budget GetNewBudget()
        {
            var budget = new Budget();
            budget.BudgetNo = RT.Service.Resolve<CommonController>().GetNo<Budget>("预算编号");
            budget.Year = new DateTime(DateTime.Today.Year, 1, 1);
            budget.ApprovalStatus = ApprovalStatus.Draft;
            budget.Currency = Currency.CNY;
            budget.BudgeGrade = BudgeGrade.FirstLevel;
            return budget;
        }

        /// <summary>
        /// 修改前校验预算最新状态
        /// </summary>
        /// <param name="budgetId">预算id</param>
        public virtual void EditCheckBudgetState(double budgetId)
        {
            var budget = GetById<Budget>(budgetId);
            if (budget == null)
            {
                throw new ValidationException("找不到id为{0}的预算信息".L10nFormat(budgetId));
            }
            if (budget.ApprovalStatus != ApprovalStatus.Draft && budget.ApprovalStatus != ApprovalStatus.Reject)
            {
                throw new ValidationException("预算状态为{0}，不能修改".L10nFormat(budget.ApprovalStatus.ToLabel()));
            }
        }

        /// <summary>
        /// 删除前校验预算最新状态
        /// </summary>
        /// <param name="budgetIds">预算id</param>
        public virtual void DeleteCheckBudgetState(List<double> budgetIds)
        {
            var budgets = GetBudgetsByIds(budgetIds);
            if (budgets.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能删除".L10N());
            }
        }

        /// <summary>
        /// 获取财年
        /// </summary>
        /// <param name="now">当前日期</param>
        /// <returns>财年</returns>
        public virtual DateTime GetFiscalYear(DateTime now)
        {
            var endDate = GetBudgetManageConfig().FiscalYearEndDate;
            var year = new DateTime(now.Year, 1, 1);
            var fiscalYearEndDate = new DateTime(now.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second);
            if (fiscalYearEndDate > now)
            {
                year = year.AddYears(-1);
            }
            return year;
        }

        /// <summary>
        /// 提交预算
        /// </summary>
        /// <param name="budgetIds">预算id</param>
        public virtual void SubmitBudget(List<double> budgetIds)
        {
            var config = RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(typeof(Budget));
            var budgets = GetBudgetsByIds(budgetIds);
            if (budgets.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            //校验预算的年度是否为当前财年之前
            var now = RF.Find<Budget>().GetDbTime();
            var year = GetFiscalYear(now);
            if (budgets.Any(p => p.Year < year))
            {
                throw new ValidationException("预算已失效".L10N());
            }
            //赋值待审核
            budgets.ForEach(p => p.ApprovalStatus = ApprovalStatus.PendingReview);
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(budgets);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(budgetIds, typeof(Budget).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    //审核
                    ExamineBudgetInner(budgetIds, ApprovalResult.Pass, "通过".L10N(), budgets);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回预算
        /// </summary>
        /// <param name="budgetIds">预算id</param>
        public virtual void RecallBudget(List<double> budgetIds)
        {
            var budgets = GetBudgetsByIds(budgetIds);
            if (budgets.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview && p.ApprovalStatus != ApprovalStatus.UnderReview))
            {
                throw new ValidationException("只有状态为【待审核】、【审核中】的数据才能撤回".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var config = RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(typeof(Budget));
                if (config.EnableAudit && config.EnableApproval)
                {
                    RT.Service.Resolve<IWorkFlow>().Recall(budgetIds);
                }

                //更新审核状态为【待提交】
                budgets.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(budgets);

                //增加审核子表一条数据
                var now = RF.Find<Budget>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(budgetIds, typeof(Budget).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核预算
        /// </summary>
        /// <param name="budgetIds">预算id</param>
        /// <param name="result">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void ExamineBudget(List<double> budgetIds, ApprovalResult result, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineBudgetInner(budgetIds, result, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核预算
        /// </summary>
        /// <param name="budgetIds">预算id</param>
        /// <param name="result">审核结果</param>
        /// <param name="remark">审核意见</param>
        /// <param name="budgets">数据组</param>
        public virtual void ExamineBudgetInner(List<double> budgetIds, ApprovalResult result, string remark, EntityList<Budget> budgets = null)
        {
            if (budgets == null)
            {
                budgets = GetBudgetsByIds(budgetIds);
                if (!budgets.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            if (budgets.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview && p.ApprovalStatus != ApprovalStatus.UnderReview))
            {
                throw new ValidationException("只有状态为【待审核】、【审核中】的数据才能审核".L10N());
            }

            //校验预算的年度是否为当前财年之前
            var now = RF.Find<Budget>().GetDbTime();
            var year = GetFiscalYear(now);
            if (budgets.Any(p => p.Year < year))
            {
                throw new ValidationException("预算已失效".L10N());
            }

            //更新审核状态为【已审批】或【驳回】
            if (result == ApprovalResult.Pass)
            {
                budgets.ForEach(p => p.ApprovalStatus = ApprovalStatus.Audited);
            }
            else
            {
                budgets.ForEach(p => p.ApprovalStatus = ApprovalStatus.Reject);
            }
            RF.Save(budgets);

            //往审核记录子表插入一条数据
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(budgetIds, typeof(Budget).FullName, result, now, remark);
        }

        /// <summary>
        /// 更新预算的【已使用金额】【预占金额】
        /// </summary>
        /// <param name="proKeyItemId">项目关键事项id</param>
        /// <param name="amount">金额</param>
        public virtual void UpdateBudgetUsedAmount(double proKeyItemId, decimal amount)
        {
            var keyItem = GetById<ProjectKeyItem>(proKeyItemId);
            if (keyItem == null)
            {
                throw new ValidationException("找不到id为:{0}的项目事项".L10nFormat(proKeyItemId));
            }
            if (keyItem.BudgetId.HasValue)
            {
                var budget = GetById<Budget>(keyItem.BudgetId.Value);
                if (budget == null)
                {
                    throw new ValidationException("找不到id为:{0}的预算".L10nFormat(keyItem.BudgetId.Value));
                }
                budget.UsedAmount += amount;
                budget.ReserveAmount -= amount;
                RF.Save(budget);
            }
        }

        /// <summary>
        /// 提交预算
        /// </summary>
        /// <param name="budget">预算</param>
        public virtual void EditSubmitBudget(Budget budget)
        {
            if (budget.Id != 0 && !budget.IsNew)
            {
                var budgetOfDb = RF.GetById<Budget>(budget.Id);

                if (budgetOfDb != null && budgetOfDb.ApprovalStatus != ApprovalStatus.Draft && budgetOfDb.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交，当前状态为【{0}】"
                        .L10nFormat(budgetOfDb.ApprovalStatus.ToLabel()));
                }
            }

            //校验预算的年度是否为当前财年之前
            var now = RF.Find<Budget>().GetDbTime();
            var year = GetFiscalYear(now);
            if (budget.Year < year)
            {
                throw new ValidationException("预算已失效".L10N());
            }
            var config = RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(typeof(Budget));

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //需要审批
                if (config.EnableAudit) {
                    budget.ApprovalStatus = ApprovalStatus.PendingReview;
                }
                //需要审批且需要启用审批流程
                if (config.EnableAudit && config.EnableApproval)
                {
                    budget.ApprovalStatus = ApprovalStatus.UnderReview;
                    RT.Service.Resolve<IWorkFlow>().Submit(new List<double> { budget.Id });
                }
                RT.Service.Resolve<WorkFlowRecordController>()
                   .CreateWorkFlowRecords(new List<double> { budget.Id }, typeof(Budget).FullName, ApprovalResult.Submit, now, "");

                //不需要审批,提交之后自动审批
                if (!config.EnableAudit)
                {
                    //自动审批
                    budget.ApprovalStatus = ApprovalStatus.Audited;
                    //往审核记录子表插入一条数据
                    RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(new List<double>() { budget.Id }, typeof(Budget).FullName, ApprovalResult.Pass, now, "通过".L10N());
                }

                RF.Save(budget);
                trans.Complete();
            }
        }
    }
}
