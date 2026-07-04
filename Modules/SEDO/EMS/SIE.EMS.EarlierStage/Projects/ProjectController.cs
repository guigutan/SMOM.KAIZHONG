using SIE.Common.Import;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Budgets;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Common.Controller;
using SIE.EMS.EarlierStage.Enums;
using SIE.EMS.Projects.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目控制器
    /// </summary>
    public partial class ProjectController : DomainController
    {
        /// <summary>
        /// 根据id获取项目
        /// </summary>
        /// <param name="projectId">id</param>
        /// <returns>项目</returns>
        public virtual Project GetProjectById(double projectId)
        {
            return Query<Project>().Where(p => p.Id == projectId).FirstOrDefault();
        }

        /// <summary>
        /// 查询项目
        /// </summary>
        /// <param name="criteria">项目查询实体</param>
        /// <returns>项目</returns>
        public virtual EntityList<Project> CriteriaProjects(ProjectCriteria criteria)
        {
            var query = Query<Project>();
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
                query.Where(p => p.Code.Contains(criteria.No));
            }
            if (!criteria.BudgetNo.IsNullOrWhiteSpace())
            {
                query.Join<ProjectKeyItem>("b", (a, b) => a.Id == b.ProjectId)
                    .Join<ProjectKeyItem, Budgets.Budget>((b, c) => b.BudgetId == c.Id && c.BudgetNo.Contains(criteria.BudgetNo));
            }
            if (!criteria.ProjectType.IsNullOrWhiteSpace())
            {
                query.Where(p => p.ProjectType == criteria.ProjectType);
            }
            if (criteria.Year.HasValue)
            {
                query.Where(p => p.Year == criteria.Year);
            }
            if (criteria.ProjectStatus.HasValue)
            {
                query.Where(p => p.ProjectStatus == criteria.ProjectStatus.Value);
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
        /// 根据id列表获取项目列表
        /// </summary>
        /// <param name="projectIds">id列表</param>
        /// <returns>项目列表</returns>
        public virtual EntityList<Project> GetProjectsByIds(List<double> projectIds)
        {
            return projectIds.SplitContains(ids => Query<Project>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据预算id获取项目列表
        /// </summary>
        /// <param name="budgetId">预算id</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>项目列表</returns>
        public virtual EntityList<Project> GetProjectByBudgetId(double budgetId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<Project>().Join<ProjectKeyItem>((a, b) => a.Id == b.ProjectId && b.BudgetId == budgetId)
                .OrderBy(sortInfo).Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工厂+部门获取已有的项目
        /// </summary>
        /// <param name="factoryId">工厂</param>
        /// <param name="departmentId">部门</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>项目列表</returns>
        public virtual EntityList<Project> GetParentProjects(double factoryId, double? departmentId, PagingInfo pagingInfo, string keyword)
        {
            return Query<Project>().Where(p => p.FactoryId == factoryId && p.DepartmentId == departmentId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)||p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取可变更的项目列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">项目编码</param>
        /// <returns>可变更的项目列表</returns>
        public virtual EntityList<Project> GetChangeProjects(PagingInfo pagingInfo, string keyword)
        {
            return Query<Project>().Where(p => p.ApprovalStatus == ApprovalStatus.Audited &&
                (p.ProjectStatus == ProjectStatus.NotStarted || p.ProjectStatus == ProjectStatus.InProgress || p.ProjectStatus == ProjectStatus.Pause))
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)||p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可结项的项目列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">项目编码</param>
        /// <returns>可结项的项目列表</returns>
        public virtual EntityList<Project> GetCloseProjects(PagingInfo pagingInfo, string keyword)
        {
            return Query<Project>().Where(p => p.ApprovalStatus == ApprovalStatus.Audited &&
                (p.ProjectStatus == ProjectStatus.NotStarted || p.ProjectStatus == ProjectStatus.InProgress))
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)||p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取项目关键事项列表
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>项目关键事项列表</returns>
        public virtual EntityList<ProjectKeyItem> GetKeyItemsByIds(List<double?> ids)
        {
            return Query<ProjectKeyItem>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目id列表获取项目关键事项列表
        /// </summary>
        /// <param name="projectIds">项目id列表</param>
        /// <returns>项目关键事项列表</returns>
        public virtual EntityList<ProjectKeyItem> GetKeyItemsByProjectIds(List<double> projectIds)
        {
            return Query<ProjectKeyItem>().Where(p => projectIds.Contains(p.ProjectId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询项目关键事项
        /// </summary>
        /// <param name="criteria">项目关键事项查询实体</param>
        /// <returns>项目关键事项</returns>
        public virtual EntityList<ProjectKeyItem> CriteriaProjectKeyItems(ProjectKeyItemCriteria criteria)
        {
            var query = Query<ProjectKeyItem>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }
            if (!criteria.No.IsNullOrWhiteSpace() || !criteria.ProjectType.IsNullOrWhiteSpace() || criteria.Year.HasValue
                || criteria.ProjectStatus.HasValue || criteria.ApprovalStatus.HasValue)
            {
                query.Join<Project>((a, b) => a.ProjectId == b.Id)
                    .WhereIf<Project>(!criteria.No.IsNullOrWhiteSpace(), (a, b) => b.Code.Contains(criteria.No))
                    .WhereIf<Project>(!criteria.ProjectType.IsNullOrWhiteSpace(), (a, b) => b.ProjectType == criteria.ProjectType)
                    .WhereIf<Project>(criteria.Year.HasValue, (a, b) => b.Year == criteria.Year)
                    .WhereIf<Project>(criteria.ProjectStatus.HasValue, (a, b) => b.ProjectStatus == criteria.ProjectStatus)
                    .WhereIf<Project>(criteria.ApprovalStatus.HasValue, (a, b) => b.ApprovalStatus == criteria.ApprovalStatus);
            }
            if (criteria.CreateDate != null)
            {
                if (criteria.CreateDate.BeginValue.HasValue)
                {
                    query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
                }
                if (criteria.CreateDate.EndValue.HasValue)
                {
                    query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
                }
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目id列表获取项目成员列表
        /// </summary>
        /// <param name="projectIds">项目id列表</param>
        /// <returns>项目成员列表</returns>
        public virtual EntityList<ProjectMember> GetMembersByProjectIds(List<double> projectIds)
        {
            return Query<ProjectMember>().Where(p => projectIds.Contains(p.ProjectId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目id列表获取工作计划列表
        /// </summary>
        /// <param name="projectIds">项目id列表</param>
        /// <returns>工作计划列表</returns>
        public virtual EntityList<ProjectWorkItem> GetWorkItemsByProjectIds(List<double> projectIds)
        {
            return Query<ProjectWorkItem>().Where(p => projectIds.Contains(p.ProjectId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目id列表获取工作计划列表
        /// </summary>
        /// <param name="projectId">项目id列表</param>
        /// <returns>工作计划列表</returns>
        public virtual EntityList<ProjectWorkItem> GetWorkItemsByProjectIds(double projectId)
        {
            return Query<ProjectWorkItem>().Where(p => p.ProjectId == projectId).OrderBy(p => p.PlanStart).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目id列表获取工作计划列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>项目关键事项列表</returns>
        public virtual EntityList<ProjectWorkItem> GetWorkItemsByProjectIds(double projectId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<ProjectWorkItem>();
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.WorkItem.Contains(keyword));
            }
            return query.Where(p => p.ProjectId == projectId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取事项关联的【事项成员】
        /// </summary>
        /// <param name="projectKeyItemId">事项id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>事项关联的【事项成员】</returns>
        public virtual EntityList<Employee> GetKeyItemPlanPrincipals(double projectKeyItemId, PagingInfo pagingInfo, string keyword)
        {
            return Query<Employee>().Join<ProjectKeyItemMember>((a, b) => a.Id == b.EmployeeId && b.ProjectKeyItemId == projectKeyItemId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取项目关联的项目成员
        /// </summary>
        /// <param name="projectKeyItemId">事项id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>项目关联的项目成员</returns>
        public virtual EntityList<Employee> GetKeyItemMemberEmployees(double projectKeyItemId, PagingInfo pagingInfo, string keyword)
        {
            return Query<Employee>().Join<ProjectMember>("b", (a, b) => a.Id == b.EmployeeId)
                .Join<ProjectMember, ProjectKeyItem>((b, c) => b.ProjectId == c.ProjectId && c.Id == projectKeyItemId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 创建一个新的项目
        /// </summary>
        /// <returns>新的项目</returns>
        public virtual Project GetNewProject()
        {
            var now = RF.Find<Project>().GetDbTime();
            var project = new Project();
            project.Code = RT.Service.Resolve<CommonController>().GetNo<Project>("项目编码");
            project.Year = new DateTime(now.Year, 1, 1);
            project.ProjectStatus = ProjectStatus.NotStarted;
            project.ApprovalStatus = ApprovalStatus.Draft;
            project.PlanType = PlanType.In;
            project.ProjectDate = now;
            project.PrincipalId = RT.IdentityId;
            return project;
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="project">项目</param>
        public virtual void SaveProject(Project project)
        {
            if (project == null)
            {
                throw new ValidationException("保存项目失败，数据异常".L10N());
            }
            if (project.PersistenceStatus != PersistenceStatus.New)
            {
                //修改时，获取最新审核状态再次校验
                var oldProject = GetById<Project>(project.Id);
                if (oldProject == null)
                {
                    throw new ValidationException("保存项目失败，数据异常".L10N());
                }
                if (oldProject.ApprovalStatus != ApprovalStatus.Draft && oldProject.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("修改项目失败，此项目的审核状态已不是【待提交】、【驳回】".L10N());
                }
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(project);

                //保存后再更新项目预算
                var keyItems = GetKeyItemsByProjectIds(new List<double> { project.Id });
                project.Amount = keyItems.Sum(p => p.BudgetAmount);
                RF.Save(project);

                //计划内的项目，校验关联的预算是否为二级预算，二级预算只允许关联一个项目
                if (project.PlanType == PlanType.In)
                {
                    if (keyItems.Any(p => p.BudgetId == null))
                    {
                        throw new ValidationException("计划类型为【计划内】时，关键事项的预算必输".L10N());
                    }
                    var budgetIds = keyItems.Select(p => p.BudgetId).ToList();
                    var existBudgets = ExistOtherProjectBudget(budgetIds, project.Id);
                    if (existBudgets.Any())
                    {
                        throw new ValidationException("预算【{0}】为二级预算，不可关联多个项目".L10nFormat(existBudgets.FirstOrDefault().BudgetNo));
                    }
                }
                //校验责任人必须是项目成员中的员工
                var principalIds = GetWorkItemPrincipalIds(project.Id);
                var employeeIds = GetMemberEmployeeIds(project.Id);
                if (principalIds.Any(p => !employeeIds.Contains(p)))
                {
                    throw new ValidationException("工作计划的责任人必须是项目成员中的员工".L10N());
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 获取存在其他项目的二级预算
        /// </summary>
        /// <param name="budgetIds">预算id</param>
        /// <param name="projectId">项目id</param>
        /// <returns>预算列表</returns>
        public virtual EntityList<EMS.Budgets.Budget> ExistOtherProjectBudget(List<double?> budgetIds, double projectId)
        {
            return Query<EMS.Budgets.Budget>().Join<ProjectKeyItem>((a, b) => a.Id == b.BudgetId && budgetIds.Contains(a.Id)
            && a.BudgeGrade == BudgeGrade.SecondLevel && b.ProjectId != projectId).ToList();
        }

        /// <summary>
        /// 获取项目工作计划的责任人id列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns>责任人id列表</returns>
        public virtual IList<double> GetWorkItemPrincipalIds(double projectId)
        {
            return Query<ProjectWorkItem>().Where(p => p.ProjectId == projectId).Select(p => p.PrincipalId).ToList<double>();
        }

        /// <summary>
        /// 获取事项计划的责任人id列表
        /// </summary>
        /// <param name="projectKeyItemId">事项id</param>
        /// <returns>责任人id列表</returns>
        public virtual IList<double> GetKeyItemPlanPrincipalIds(double projectKeyItemId)
        {
            return Query<ProjectKeyItemPlan>().Where(p => p.ProjectKeyItemId == projectKeyItemId).Select(p => p.PrincipalId).ToList<double>();
        }

        /// <summary>
        /// 获取项目成员的员工id列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns>员工id列表</returns>
        public virtual IList<double> GetMemberEmployeeIds(double projectId)
        {
            return Query<ProjectMember>().Where(p => p.ProjectId == projectId).Select(p => p.EmployeeId).ToList<double>();
        }

        /// <summary>
        /// 获取项目事项成员的员工id列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns>员工id列表</returns>
        public virtual IList<double> GetKeyItemMemberEmployeeIds(double projectId)
        {
            return Query<ProjectKeyItemMember>().Join<ProjectKeyItem>((a, b) => a.ProjectKeyItemId == b.Id && b.ProjectId == projectId)
                .Select(p => p.EmployeeId).ToList<double>();
        }

        /// <summary>
        /// 删除前校验项目最新状态
        /// </summary>
        /// <param name="ids">项目id</param>
        public virtual void DeleteCheckProjectState(List<double> ids)
        {
            var projects = GetProjectsByIds(ids);
            if (projects.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有审核状态为【待提交】的数据才能删除".L10N());
            }
        }

        /// <summary>
        /// 提交项目
        /// </summary>
        /// <param name="projectIds">项目id</param>
        public virtual void SubmitProject(List<double> projectIds)
        {
            //配置文件
            var config = RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(typeof(Project));
            var projects = GetProjectsByIds(projectIds);
            if (projects.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            //获取关键事项和预算
            var allKeyItems = GetKeyItemsByProjectIds(projectIds);
            var budgetIds = new List<double>();
            var budgetNullIds = allKeyItems.Where(p => p.BudgetId != null).Select(p => p.BudgetId).ToList();
            budgetNullIds.ForEach(p => budgetIds.Add(p.Value));
            var budgets = RT.Service.Resolve<BudgetController>().GetBudgetsByIds(budgetIds);
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var project in projects)
                {
                    var keyItems = allKeyItems.Where(p => p.ProjectId == project.Id).ToList();
                    if (!keyItems.Any())
                    {
                        throw new ValidationException("请关联项目预算！".L10N());
                    }
                    foreach (var keyItem in keyItems)
                    {
                        if (keyItem.BudgetId == null)
                        {
                            continue;
                        }
                        var budget = budgets.FirstOrDefault(p => p.Id == keyItem.BudgetId);
                        if (budget == null)
                        {
                            throw new ValidationException("数据异常，找不到id为:{0}的预算信息".L10nFormat(keyItem.BudgetId));
                        }

                        //当项目的关键事项有关联预算时，需要占用预算的金额
                        budget.ReserveAmount += keyItem.BudgetAmount;
                        if (budget.ReserveAmount + budget.UsedAmount > budget.BudgetAmount)
                        {
                            throw new ValidationException("预算{0}预占金额加已使用金额大于预算金额".L10nFormat(budget.BudgetNo));
                        }
                        RF.Save(budget);
                    }
                    project.ApprovalStatus = ApprovalStatus.PendingReview;
                }
                RF.Save(projects);
                var now = RF.Find<Project>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(projectIds, typeof(Project).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    //审核
                    ExaminePassProjectInner(projectIds, "通过".L10N(), projects);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回/驳回项目
        /// </summary>
        /// <param name="projectIds">项目id</param>
        /// <param name="status">审核状态</param>
        /// <param name="result">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void CancelProject(List<double> projectIds, ApprovalStatus status, ApprovalResult result, string remark = "")
        {
            var projects = GetProjectsByIds(projectIds);
            if (projects.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            //获取关键事项和预算
            var allKeyItems = GetKeyItemsByProjectIds(projectIds);
            var budgetIds = new List<double>();
            var budgetNullIds = allKeyItems.Where(p => p.BudgetId != null).Select(p => p.BudgetId).ToList();
            budgetNullIds.ForEach(p => budgetIds.Add(p.Value));
            var budgets = RT.Service.Resolve<BudgetController>().GetBudgetsByIds(budgetIds);
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var project in projects)
                {
                    var keyItems = allKeyItems.Where(p => p.ProjectId == project.Id).ToList();
                    foreach (var keyItem in keyItems)
                    {
                        if (keyItem.BudgetId == null)
                        {
                            continue;
                        }
                        //当项目的关键事项有关联预算时，需要释放预算
                        var budget = budgets.FirstOrDefault(p => p.Id == keyItem.BudgetId);
                        if (budget == null)
                        {
                            throw new ValidationException("数据异常，找不到id为:{0}的预算信息".L10nFormat(keyItem.BudgetId));
                        }
                        budget.ReserveAmount -= keyItem.BudgetAmount;
                        RF.Save(budget);
                    }
                    //更新审核状态
                    project.ApprovalStatus = status;
                }
                RF.Save(projects);

                var now = RF.Find<Project>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(projectIds, typeof(Project).FullName, result, now, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="projectIds">项目id</param>
        /// <param name="remark">审核意见</param>
        public virtual void ExaminePassProject(List<double> projectIds, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExaminePassProjectInner(projectIds, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="projectIds">项目id</param>
        /// <param name="remark">审核意见</param>
        /// <param name="projects">数据组</param>
        public virtual void ExaminePassProjectInner(List<double> projectIds, string remark, EntityList<Project> projects = null)
        {
            if (projects == null)
            {
                projects = GetProjectsByIds(projectIds);
                if (!projects.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            if (projects.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            //更新审核状态为【已审批】
            projects.ForEach(p => p.ApprovalStatus = ApprovalStatus.Audited);
            RF.Save(projects);

            //往审核记录子表插入一条数据
            var now = RF.Find<Project>().GetDbTime();
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(projectIds, typeof(Project).FullName, ApprovalResult.Pass, now, remark);

        }

        /// <summary>
        /// 删除校验项目成员是否被关联
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="employeeId">员工id</param>
        /// <returns>是否被关联</returns>
        public virtual bool DeleteCheckMember(double projectId, double employeeId)
        {
            var principalIds = GetWorkItemPrincipalIds(projectId);
            var empIds = GetKeyItemMemberEmployeeIds(projectId);
            if (principalIds.Contains(employeeId) || empIds.Contains(employeeId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除校验事项成员是否被关联
        /// </summary>
        /// <param name="projectKeyItemId">事项id</param>
        /// <param name="employeeId">员工id</param>
        /// <returns>是否被关联</returns>
        public virtual bool DeleteCheckKeyItemMember(double projectKeyItemId, double employeeId)
        {
            var principalIds = GetKeyItemPlanPrincipalIds(projectKeyItemId);
            if (principalIds.Contains(employeeId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 计划开始
        /// </summary>
        /// <param name="id">行id</param>
        public virtual void StartWorkItem(double id)
        {
            var workItem = GetById<ProjectWorkItem>(id);
            if (workItem == null)
            {
                throw new ValidationException("数据异常，项目计划开始失败".L10N());
            }
            if (workItem.WorkStatus != WorkStatus.NotStarted)
            {
                throw new ValidationException("【未开始】的项目计划才允许开始".L10N());
            }
            var project = GetProjectById(workItem.ProjectId);
            if (project == null)
            {
                throw new ValidationException("数据异常，项目计划开始失败".L10N());
            }
            if (project.ApprovalStatus != ApprovalStatus.Audited)
            {
                throw new ValidationException("项目审核状态不为【已审批】".L10N());
            }
            if (project.ProjectStatus != ProjectStatus.NotStarted && project.ProjectStatus != ProjectStatus.InProgress)
            {
                throw new ValidationException("项目状态不为【未开始】或【进行中】".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //更新项目计划的状态为【进行中】，更新实际开始时间为当前时间,实际开始人
                workItem.WorkStatus = WorkStatus.InProgress;
                workItem.ActualStart = DateTime.Today;
                workItem.ActualStartPeopleId = RT.IdentityId;
                RF.Save(workItem);

                //若项目状态为【未开始】，则更新项目状态为【进行中】，更新【上一项目状态】
                if (project.ProjectStatus == ProjectStatus.NotStarted)
                {
                    project.ProjectStatus = ProjectStatus.InProgress;
                    project.PreviousStatus = ProjectStatus.NotStarted;
                    RF.Save(project);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 计划完成
        /// </summary>
        /// <param name="id">行id</param>
        public virtual void CompleteWorkItem(double id)
        {
            var workItem = GetById<ProjectWorkItem>(id);
            if (workItem == null)
            {
                throw new ValidationException("数据异常，项目计划完成失败".L10N());
            }
            if (workItem.WorkStatus != WorkStatus.InProgress)
            {
                throw new ValidationException("【进行中】的项目计划才允许完成".L10N());
            }
            var project = GetProjectById(workItem.ProjectId);
            if (project == null)
            {
                throw new ValidationException("数据异常，项目计划完成失败".L10N());
            }

            //更新项目计划的状态为【已完成】，更新实际结束时间为当前时间,实际结束人

            workItem.ActaulEnd = DateTime.Today;
            workItem.ActaulEndPeopleId = RT.IdentityId;
            workItem.WorkStatus = WorkStatus.Finish;
            RF.Save(workItem);
        }

        /// <summary>
        /// 计划开始
        /// </summary>
        /// <param name="id">行id</param>
        public virtual void StartKeyItemPlan(double id)
        {
            var keyItemPlan = GetById<ProjectKeyItemPlan>(id);
            if (keyItemPlan == null)
            {
                throw new ValidationException("数据异常，事项计划开始失败".L10N());
            }
            if (keyItemPlan.WorkStatus != WorkStatus.NotStarted)
            {
                throw new ValidationException("【未开始】的事项计划才允许开始".L10N());
            }
            var keyItem = GetById<ProjectKeyItem>(keyItemPlan.ProjectKeyItemId);
            if (keyItem == null)
            {
                throw new ValidationException("数据异常，事项计划开始失败".L10N());
            }
            var project = GetProjectById(keyItem.ProjectId);
            if (project == null)
            {
                throw new ValidationException("数据异常，事项计划开始失败".L10N());
            }
            if (project.ApprovalStatus != ApprovalStatus.Audited)
            {
                throw new ValidationException("项目审核状态不为【已审批】".L10N());
            }
            if (project.ProjectStatus != ProjectStatus.NotStarted && project.ProjectStatus != ProjectStatus.InProgress)
            {
                throw new ValidationException("项目状态不为【未开始】或【进行中】".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //更新事项计划的状态为【进行中】，更新实际开始时间为当前时间
                keyItemPlan.WorkStatus = WorkStatus.InProgress;
                keyItemPlan.ActualSart = RF.Find<ProjectWorkItem>().GetDbTime();
                keyItemPlan.ActualStartPeopleId = RT.IdentityId;
                RF.Save(keyItemPlan);

                //若事项关联的事项计划存在【进行中】的数据，则更新事项状态为【进行中】
                keyItem.WorkStatus = WorkStatus.InProgress;
                RF.Save(keyItem);

                //若项目状态为【未开始】，则更新项目状态为【进行中】
                if (project.ProjectStatus == ProjectStatus.NotStarted)
                {
                    project.ProjectStatus = ProjectStatus.InProgress;
                    project.PreviousStatus = ProjectStatus.NotStarted;
                    RF.Save(project);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 计划完成
        /// </summary>
        /// <param name="id">行id</param>
        public virtual void CompleteKeyItemPlan(double id)
        {
            var keyItemPlan = GetById<ProjectKeyItemPlan>(id);
            if (keyItemPlan == null)
            {
                throw new ValidationException("数据异常，事项计划完成失败".L10N());
            }
            if (keyItemPlan.WorkStatus != WorkStatus.InProgress)
            {
                throw new ValidationException("【进行中】的事项计划才允许完成".L10N());
            }
            var keyItem = GetById<ProjectKeyItem>(keyItemPlan.ProjectKeyItemId);
            if (keyItem == null)
            {
                throw new ValidationException("数据异常，事项计划完成失败".L10N());
            }
            var project = GetProjectById(keyItem.ProjectId);
            if (project == null)
            {
                throw new ValidationException("数据异常，项目计划完成失败".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //更新事项计划的状态为【已完成】，更新实际结束时间为当前时间
                keyItemPlan.WorkStatus = WorkStatus.Finish;
                keyItemPlan.ActualEnd = RF.Find<ProjectWorkItem>().GetDbTime();
                keyItemPlan.ActaulEndPeopleId = RT.IdentityId;
                RF.Save(keyItemPlan);

                //若事项下所有的事项计划都为已完成，则更新事项状态为【已完成】
                if (keyItem.PlanList.All(p => p.WorkStatus == WorkStatus.Finish))
                {
                    keyItem.WorkStatus = WorkStatus.Finish;
                    RF.Save(keyItem);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 批量保存行数据
        /// </summary>
        /// <param name="data"></param>
        public virtual List<ImportMessageResult> SaveImportKeyItmes(IList<RowData> data)
        {
            List<ImportMessageResult> messageList = new List<ImportMessageResult>();

            //验证二级预算是否重复使用
            ValidateUseSecondLevelBudget(data, messageList);

            //验证填写的项目计划是否正确
            if (data.Any(x => !(x.Entity as ProjectKeyItem).ProjectWorkItemName.IsNullOrEmpty()))
            {
                //获取所有项目的项目计划
                var projectIds = data.Select(x => (x.Entity as ProjectKeyItem).ProjectId).Distinct().ToList();

                var projectWorkItems = projectIds.SplitContains(tempIds =>
                {
                    return Query<ProjectWorkItem>().Where(x => projectIds.Contains(x.ProjectId)).ToList();
                });

                foreach (var rowData in data
                    .Where(x => !(x.Entity as ProjectKeyItem).ProjectWorkItemName.IsNullOrEmpty()))
                {
                    var projectKeyItem = rowData.Entity as ProjectKeyItem;
                    var projectWorkItem = projectWorkItems.FirstOrDefault(x => x.ProjectId == projectKeyItem.ProjectId
                         && x.WorkItem == projectKeyItem.ProjectWorkItemName);

                    if (projectWorkItem == null)
                    {
                        messageList.Add(new ImportMessageResult()
                        {
                            Message = "【{0}】不是项目【{1}】的项目计划".L10nFormat(projectKeyItem.ProjectWorkItemName, projectKeyItem.Project.Name),
                            MsgType = ImportMessageType.SaveFail,
                            RowNum = rowData.RowIndex + 1
                        });
                    }
                    else
                    {
                        projectKeyItem.ProjectWorkItemId = projectWorkItem.Id;
                    }
                }
            }

            IList<RowData> rowDatasOfValidation = new List<RowData>();

            foreach (var item in data)
            {
                //没有验证错误，则添加
                if (!messageList.Any(x => x.RowNum == item.RowIndex))
                {
                    rowDatasOfValidation.Add(item);
                }
            }

            if (rowDatasOfValidation.Any())
            {
                using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    messageList.AddRange(RT.Service.Resolve<ImportController>().Save2(rowDatasOfValidation));

                    var keyItems = new List<ProjectKeyItem>();

                    foreach (var rowData in rowDatasOfValidation)
                    {
                        //过滤掉导入失败的数据
                        if (messageList.Any(x => x.RowNum == rowData.RowIndex))
                        {
                            continue;
                        }

                        keyItems.Add(rowData.Entity as ProjectKeyItem);
                    }

                    var projectIds = keyItems.Select(x => x.ProjectId).Distinct().ToList();

                    UpdatePrjectBudgets(projectIds);

                    trans.Complete();
                }
            }

            return messageList;
        }

        private void ValidateUseSecondLevelBudget(IList<RowData> data, List<ImportMessageResult> messageList)
        {
            var dataUserSecondLevelBudgets = data
                .Where(x => (x.Entity as ProjectKeyItem).Budget != null
                    && (x.Entity as ProjectKeyItem).Budget.BudgeGrade == BudgeGrade.SecondLevel);

            var budgetIds = dataUserSecondLevelBudgets
                .Select(x => (x.Entity as ProjectKeyItem).BudgetId).Distinct().ToList();

            var projectKeyItems = GetProjectKeyItemsUseBudget(budgetIds);

            foreach (var rowData in dataUserSecondLevelBudgets)
            {
                var projectKeyItem = rowData.Entity as ProjectKeyItem;
                if (projectKeyItem == null)
                {
                    messageList.Add(new ImportMessageResult()
                    {
                        Message = "项目关键事项信息丢失".L10N(),
                        MsgType = ImportMessageType.SaveFail,
                        RowNum = rowData.RowIndex + 1
                    });

                    continue;
                }

                if (projectKeyItems.Any(x => x.BudgetId == projectKeyItem.BudgetId))
                {
                    messageList.Add(new ImportMessageResult()
                    {
                        Message = "预算【{0}】为二级预算，不可关联多个项目".L10nFormat(projectKeyItem.Budget.BudgetNo),
                        MsgType = ImportMessageType.SaveFail,
                        RowNum = rowData.RowIndex + 1
                    });

                    continue;
                }

                if (dataUserSecondLevelBudgets.Any(x => x.RowIndex != rowData.RowIndex
                     && (x.Entity as ProjectKeyItem).BudgetId == projectKeyItem.BudgetId))
                {
                    messageList.Add(new ImportMessageResult()
                    {
                        Message = "预算【{0}】为二级预算，不可关联多个项目".L10nFormat(projectKeyItem.Budget.BudgetNo),
                        MsgType = ImportMessageType.SaveFail,
                        RowNum = rowData.RowIndex + 1
                    });
                }
            }
        }


        /// <summary>
        /// 获取存在其他项目的二级预算
        /// </summary>
        /// <param name="budgetIds">预算id</param>        
        /// <returns>预算列表</returns>
        public virtual EntityList<ProjectKeyItem> GetProjectKeyItemsUseBudget(List<double?> budgetIds)
        {
            return budgetIds.SplitContains(tempIds =>
            {
                return Query<ProjectKeyItem>()
                .Join<EMS.Budgets.Budget>((a, b) => a.BudgetId == b.Id && tempIds.Contains(b.Id))
                .ToList();
            });
        }

        /// <summary>
        /// 更新项目的预算
        /// </summary>
        /// <param name="projectIds"></param>        
        private void UpdatePrjectBudgets(List<double> projectIds)
        {
            if (!projectIds.Any())
            {
                return;
            }

            var projects = projectIds.SplitContains(tempIds =>
            {
                return Query<Project>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            var projectItems = projectIds.SplitContains(tempIds =>
             {
                 return Query<ProjectKeyItem>()
                     .Where(x => tempIds.Contains(x.ProjectId))
                     .ToList();
             });

            foreach (var project in projects)
            {
                var prjectTotalBudgetAmount = projectItems.Where(x => x.ProjectId == project.Id).Sum(x => x.BudgetAmount);
                project.Amount = prjectTotalBudgetAmount;
            }

            RF.Save(projects);
        }
    }
}
