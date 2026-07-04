using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Common.Controller;
using SIE.EMS.EarlierStage.Enums;
using SIE.EMS.Projects;
using SIE.EMS.Projects.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.Purchases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更结项控制器
    /// </summary>
    public partial class ProjectChangeController : DomainController
    {
        /// <summary>
        /// 查询项目变更
        /// </summary>
        /// <param name="criteria">项目变更查询实体</param>
        /// <returns>项目变更</returns>
        public virtual EntityList<ProjectChange> CriteriaProjectChanges(ProjectChangeCriteria criteria)
        {
            var query = Query<ProjectChange>();
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
            if (!criteria.No.IsNullOrWhiteSpace() || !criteria.ProjectType.IsNullOrWhiteSpace() || criteria.Year.HasValue)
            {
                query.Join<Project>((a, b) => a.ProjectId == b.Id)
                    .WhereIf<Project>(!criteria.No.IsNullOrWhiteSpace(), (a, b) => b.Code.Contains(criteria.No))
                    .WhereIf<Project>(!criteria.ProjectType.IsNullOrWhiteSpace(), (a, b) => b.ProjectType == criteria.ProjectType)
                    .WhereIf<Project>(criteria.Year.HasValue, (a, b) => b.Year == criteria.Year);
            }
            if (!criteria.BudgetNo.IsNullOrWhiteSpace())
            {
                //query.Join<ProjectKeyItem>("d", (a, d) => a.ProjectId == d.ProjectId)
                //    .Join<ProjectKeyItem, Budget>((d, c) => d.BudgetId == c.Id && c.BudgetNo.Contains(criteria.BudgetNo));
                query.Exists<ProjectKeyItem>((a, b) => b.Join<Budget>((c, d) => c.BudgetId == d.Id && d.BudgetNo.Contains(criteria.BudgetNo))
                            .Where(p => p.ProjectId == a.ProjectId));
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
        /// 根据id列表获取项目变更列表
        /// </summary>
        /// <param name="changeIds">id列表</param>
        /// <returns>项目变更列表</returns>
        public virtual EntityList<ProjectChange> GetProjectChangesByIds(List<double> changeIds)
        {
            return changeIds.SplitContains(ids => Query<ProjectChange>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据项目变更id列表获取项目变更关键事项列表
        /// </summary>
        /// <param name="changeIds">项目变更id列表</param>
        /// <returns>项目变更关键事项列表</returns>
        public virtual EntityList<ProjectChangeKeyItem> GetKeyItemsByChangeIds(List<double> changeIds)
        {
            return Query<ProjectChangeKeyItem>().Where(p => changeIds.Contains(p.ProjectChangeId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目变更id列表获取项目变更成员列表
        /// </summary>
        /// <param name="changeIds">项目变更id列表</param>
        /// <returns>项目变更成员列表</returns>
        public virtual EntityList<ProjectChangeMember> GetChangeMembersByChangeIds(List<double> changeIds)
        {
            return Query<ProjectChangeMember>().Where(p => changeIds.Contains(p.ProjectChangeId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目变更id列表获取项目变更计划列表
        /// </summary>
        /// <param name="changeIds">项目变更id列表</param>
        /// <returns>项目变更计划列表</returns>
        public virtual EntityList<ProjectChangeWorkItem> GetChangeWorkItemsByChangeIds(List<double> changeIds)
        {
            return Query<ProjectChangeWorkItem>().Where(p => changeIds.Contains(p.ProjectChangeId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目id获取项目变更内容列表
        /// </summary>
        /// <param name="proId">项目id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>项目变更内容列表</returns>
        public virtual EntityList<ProjectChangeContent> GetChangeContentsByProId(double proId, PagingInfo pagingInfo)
        {
            return Query<ProjectChangeContent>().Join<ProjectChange>((a, b) => a.ProjectChangeId == b.Id && b.ProjectId == proId)
                .OrderByDescending(p => p.CreateDate)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据项目id获取项目子列表信息
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns>项目子列表信息</returns>
        public virtual Tuple<EntityList<ProjectChangeKeyItem>, EntityList<ProjectChangeMember>, EntityList<ProjectChangeWorkItem>> GetChildInfoByProjectId(double projectId)
        {
            var project = GetById<Project>(projectId);
            if (project == null)
            {
                throw new ValidationException("数据异常，找不到id为:{0}的项目".L10nFormat(projectId));
            }
            var keyItems = new EntityList<ProjectChangeKeyItem>();
            var members = new EntityList<ProjectChangeMember>();
            var works = new EntityList<ProjectChangeWorkItem>();
            foreach (var item in project.ProjectWorkItemList)
            {
                var model = new ProjectChangeWorkItem();
                model.GenerateId();
                model.ProjectWorkItemId = item.Id;
                model.WorkItem = item.WorkItem;
                model.PlanStart = item.PlanStart;
                model.PlantEnd = item.PlantEnd;
                model.ActualStart = item.ActualStart;
                model.ActaulEnd = item.ActaulEnd;
                model.PrincipalId = item.PrincipalId;
                model.WorkStatus = item.WorkStatus;
                model.PrincipalName = item.PrincipalName;
                works.Add(model);
            }
            foreach (var item in project.KeyItemList)
            {
                var model = new ProjectChangeKeyItem();
                model.ProjectKeyItemId = item.Id;
                model.FactoryId = item.FactoryId;
                model.DepartmentId = item.DepartmentId;
                model.Description = item.Description;
                model.BudgetId = item.BudgetId;
                model.BudgetAmount = item.BudgetAmount;
                model.WorkStatus = item.WorkStatus;
                model.Remark = item.Remark;
                model.ActualCost = item.ActualCost;
                model.LaborCost = item.LaborCost;
                model.BudgetNo = item.BudgetNo;
                model.BudgetName = item.BudgetName;
                model.BudgetQty = item.BudgetQty;
                model.ReserveAmount = item.ReserveAmount;
                model.UsedAmount = item.UsedAmount;
                model.PlanType = project.PlanType;
                //根据原计划Id找对应创建新计划Id
                var newProjectWorkItem = works.FirstOrDefault(p => p.ProjectWorkItemId == item.ProjectWorkItemId);
                if (newProjectWorkItem != null)
                {
                    model.ProjectChangeWorkItemId = newProjectWorkItem.Id;
                    model.ProjectChangeWorkItem_Display = newProjectWorkItem.WorkItem;
                }
                keyItems.Add(model);
            }
            foreach (var item in project.ProjectMemberList)
            {
                var model = new ProjectChangeMember();
                model.ProjectMemberId = item.Id;
                model.Position = item.Position;
                model.Remark = item.Remark;
                model.EmployeeId = item.EmployeeId;
                model.MemberStatus = item.MemberStatus;
                model.EmployeeCode = item.EmployeeCode;
                model.EmployeeName = item.EmployeeName;
                model.Phone = item.Phone;
                members.Add(model);
            }

            return new Tuple<EntityList<ProjectChangeKeyItem>, EntityList<ProjectChangeMember>, EntityList<ProjectChangeWorkItem>>(keyItems, members, works);
        }

        /// <summary>
        /// 获取项目变更工作计划的责任人id列表
        /// </summary>
        /// <param name="changeId">项目变更id</param>
        /// <returns>责任人id列表</returns>
        public virtual IList<double> GetWorkItemPrincipalIds(double changeId)
        {
            return Query<ProjectChangeWorkItem>().Where(p => p.ProjectChangeId == changeId).Select(p => p.PrincipalId).ToList<double>();
        }

        /// <summary>
        /// 获取项目变更成员的员工id列表
        /// </summary>
        /// <param name="changeId">项目变更id</param>
        /// <returns>变更员工id列表</returns>
        public virtual IList<double> GetMemberEmployeeIds(double changeId)
        {
            return Query<ProjectChangeMember>().Where(p => p.ProjectChangeId == changeId).Select(p => p.EmployeeId).ToList<double>();
        }

        /// <summary>
        /// 获取未完结的变更申请数量
        /// </summary>
        /// <param name="changeId">变更id</param>
        /// <param name="proId">项目id</param>
        /// <returns>未完结的变更申请数量</returns>
        public virtual int GetNoAuditChangeCount(double changeId, double proId)
        {
            return Query<ProjectChange>().Where(p => p.ProjectId == proId && p.Id != changeId && p.ApprovalStatus != ApprovalStatus.Audited).Count();
        }

        /// <summary>
        /// 保存项目变更
        /// </summary>
        /// <param name="projectChange">项目变更</param>
        public virtual void SaveProjectChange(ProjectChange projectChange)
        {
            Project project = GetById<Project>(projectChange.ProjectId);
            ValidationData(projectChange, project);
            //新建对象重新赋值,单独保存因新建时子列表有未保存的数据互相引用
            ProjectChange newProjectChange = InitProjectChange(projectChange);
            
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //改动原因,在新建选择主项目编号时自动带出的数据都是临时数据未保存在数据库，但项目事项又关联项目计划,导致保存时引用关系保存错误,所以先保存主数据再一步步保存被引用的数据。
                RF.Save(newProjectChange);
                RF.Save(projectChange.ProjectMemberList);
                RF.Save(projectChange.ProjectWorkItemList);
                RF.Save(projectChange.KeyItemList);

                //保存之后再查询全部的新计划
                EntityList<ProjectChangeWorkItem> ProjectWorkItemList = GetChangeWorkItemsByChangeIds(new List<double>() { projectChange.Id });
                //如果保存时事项计划中关联的项目计划被删除，则自动清空事项计划关联的项目计划id
                foreach (var KeyItem in projectChange.KeyItemList)
                {
                    //无项目计划则清空全部事项关联的计划
                    if (ProjectWorkItemList.Any())
                    {
                        var changeWorkItem = ProjectWorkItemList.FirstOrDefault(x => x.Id == KeyItem.ProjectChangeWorkItemId);
                        if (changeWorkItem == null)
                        {
                            KeyItem.ProjectChangeWorkItemId = null;
                        }
                    }
                    else
                    {
                        KeyItem.ProjectChangeWorkItemId = null;
                    }
                }
                //清空后重新保存
                RF.Save(projectChange.KeyItemList);

                //保存后再更新项目预算
                var keyItems = GetKeyItemsByChangeIds(new List<double> { projectChange.Id });
                if (!keyItems.Any())
                {
                    throw new ValidationException("请维护项目关键事项".L10N());
                }
                if (keyItems.Any(p => p.ActualCost != null && p.BudgetAmount < p.ActualCost))
                {
                    throw new ValidationException("事项预算不能改为小于事项成本".L10N());
                }
                //为记录日志信息变更重新赋值给projectChange对象
                newProjectChange.Amount = keyItems.Sum(p => p.BudgetAmount);
                projectChange.Amount = newProjectChange.Amount;
                RF.Save(newProjectChange);

                //计划内的项目，校验关联的预算是否为二级预算，二级预算只允许关联一个项目
                if (project.PlanType == PlanType.In)
                {
                    if (keyItems.Any(p => p.BudgetId == null))
                    {
                        throw new ValidationException("计划类型为【计划内】时，关键事项的预算必输".L10N());
                    }
                    var budgetIds = keyItems.Select(p => p.BudgetId).ToList();
                    var existBudgets = RT.Service.Resolve<ProjectController>().ExistOtherProjectBudget(budgetIds, project.Id);
                    if (existBudgets.Any())
                    {
                        throw new ValidationException("预算【{0}】为二级预算，不可关联多个项目".L10nFormat(existBudgets.FirstOrDefault().BudgetNo));
                    }
                }
                //校验责任人必须是项目成员中的员工
                var principalIds = GetWorkItemPrincipalIds(projectChange.Id);
                var employeeIds = GetMemberEmployeeIds(projectChange.Id);
                if (principalIds.Any(p => !employeeIds.Contains(p)))
                {
                    throw new ValidationException("工作计划的责任人必须是项目成员中的员工".L10N());
                }

                //生成变更内容数据
                GenerateChangeContent(projectChange, project, keyItems);
                trans.Complete();
            }
        }

        /// <summary>
        /// 验证保存的项目变更数据是否合法
        /// </summary>
        /// <param name="projectChange"></param>
        public virtual void ValidationData(ProjectChange projectChange, Project project)
        {
            if (projectChange == null)
            {
                throw new ValidationException("保存项目变更失败，数据异常".L10N());
            }
            if (projectChange.IsSuspend == true && projectChange.SuspendReason.IsNullOrWhiteSpace())
            {
                throw new ValidationException("项目暂停勾选时，暂停原因必输".L10N());
            }
            if (projectChange.IsRecovery == true && projectChange.RecoveryExplain.IsNullOrWhiteSpace())
            {
                throw new ValidationException("项目恢复勾选时，恢复说明必输".L10N());
            }
            if (project == null)
            {
                throw new ValidationException("保存数据异常，找不到id为:{0}的项目".L10nFormat(projectChange.ProjectId));
            }
            var existCount = GetNoAuditChangeCount(projectChange.Id, project.Id);
            if (existCount > 0)
            {
                throw new ValidationException("一个项目不能同时存在2个未完结的变更申请".L10N());
            }
            var exist = GetProjectCloseCount(project.Id);
            if (exist > 0)
            {
                throw new ValidationException("项目处于结项申请中，不能变更".L10N());
            }
            if (projectChange.PersistenceStatus != PersistenceStatus.New)
            {
                //修改时，获取最新审核状态再次校验
                var oldProjectChange = GetById<ProjectChange>(projectChange.Id);
                if (oldProjectChange == null)
                {
                    throw new ValidationException("保存项目变更失败，数据异常".L10N());
                }
                if (oldProjectChange.ApprovalStatus != ApprovalStatus.Draft && oldProjectChange.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("修改项目变更失败，此项目变更的审核状态已不是【待提交】、【驳回】".L10N());
                }
            }
            else
            {
                projectChange.FactoryId = project.FactoryId;
                projectChange.DepartmentId = project.DepartmentId;
                projectChange.No = RT.Service.Resolve<CommonController>().GetNo<ProjectChange>("项目变更单号");
                projectChange.ApprovalStatus = ApprovalStatus.Draft;
            }
        }

        /// <summary>
        /// 生成项目变更对象
        /// </summary>
        /// <param name="projectChange"></param>
        public virtual ProjectChange InitProjectChange(ProjectChange projectChange)
        {
            ProjectChange newProjectChange = new ProjectChange()
            {
                Id = projectChange.Id,
                FactoryId = projectChange.FactoryId,
                ProjectId = projectChange.ProjectId,
                Amount = projectChange.Amount,
                ApprovalStatus = projectChange.ApprovalStatus,
                ApprovalTime = projectChange.ApprovalTime,
                IsRecovery = projectChange.IsRecovery,
                IsSuspend = projectChange.IsSuspend,
                No = projectChange.No,
                PersistenceStatus = projectChange.PersistenceStatus,
                RecoveryExplain = projectChange.RecoveryExplain,
                SuspendReason = projectChange.SuspendReason,
                UpdateBy = projectChange.UpdateBy,
                UpdateDate = projectChange.UpdateDate,
                DepartmentId = projectChange.DepartmentId,
                CreateDate = projectChange.CreateDate,
                CreateBy = projectChange.CreateBy,
            };
            return newProjectChange;
        }

        /// <summary>
        /// 生成变更内容数据
        /// </summary>
        /// <param name="projectChange">项目变更</param>
        /// <param name="project">项目</param>
        /// <param name="newKeyItems">新关键事项</param>
        private void GenerateChangeContent(ProjectChange projectChange, Project project, EntityList<ProjectChangeKeyItem> newKeyItems)
        {
            //修改变更申请，需要删除原变更数据重新生成一份新的
            DB.Delete<ProjectChangeContent>().Where(p => p.ProjectChangeId == projectChange.Id).Execute();
            var list = new EntityList<ProjectChangeContent>();

            //修改预算金额字段
            if (projectChange.Amount != project.Amount)
            {
                var str = "预算金额【{0}】修改为【{1}】".L10nFormat(project.Amount, projectChange.Amount);
                var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Modify, ChangeType.Amount, str);
                list.Add(entity);
            }
            //生成关键事项变更内容
            GenerateKeyItemChange(list, newKeyItems, projectChange, project);
            //生成项目成员变更内容
            GenerateMemberChange(list, projectChange, project);
            //生成工作计划变更内容
            GenerateWorkItemChange(list, projectChange, project);

            //修改项目状态
            if (projectChange.IsSuspend == true)
            {
                var str = "项目状态【{0}】修改为【暂停】，{1}".L10nFormat(project.ProjectStatus.ToLabel(), projectChange.SuspendReason);
                var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Suspend, ChangeType.ProjectStatus, str);
                list.Add(entity);
            }
            if (projectChange.IsRecovery == true)
            {
                var str = "项目状态【暂停】修改为【{0}】，{1}".L10nFormat(project.PreviousStatus.ToLabel(), projectChange.RecoveryExplain);
                var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Recovery, ChangeType.ProjectStatus, str);
                list.Add(entity);
            }
            if (!list.Any())
            {
                throw new ValidationException("变更内容不能为空".L10N());
            }
            RF.Save(list);
        }

        /// <summary>
        /// 生成关键事项变更内容
        /// </summary>
        /// <param name="list">变更内容</param>
        /// <param name="newKeyItems">新关键事项</param>
        /// <param name="projectChange">项目变更</param>
        /// <param name="project">项目</param>
        private void GenerateKeyItemChange(EntityList<ProjectChangeContent> list, EntityList<ProjectChangeKeyItem> newKeyItems, ProjectChange projectChange, Project project)
        {
            //增加关键事项
            var addKeyItems = newKeyItems.Where(p => p.ProjectKeyItemId == null).ToList();
            foreach (var addKeyItem in addKeyItems)
            {
                var str = "增加关键事项【事项说明：{0}；预算编码：{1}；事项预算：{2}；备注：{3}】"
                    .L10nFormat(addKeyItem.Description, addKeyItem.BudgetNo, addKeyItem.BudgetAmount, addKeyItem.Remark);
                var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Add, ChangeType.KeyItem, str);
                list.Add(entity);
            }
            //修改关键事项
            var oldKeyItems = RT.Service.Resolve<ProjectController>().GetKeyItemsByProjectIds(new List<double> { project.Id });
            var editKeyItems = newKeyItems.Where(p => p.ProjectKeyItemId != null).ToList();
            foreach (var newKeyItem in editKeyItems)
            {
                var oldKeyItem = oldKeyItems.FirstOrDefault(p => p.Id == newKeyItem.ProjectKeyItemId);
                if (oldKeyItem == null)
                {
                    throw new ValidationException("保存数据异常，找不到id为:{0}的关键事项".L10nFormat(newKeyItem.ProjectKeyItemId));
                }
                var str = string.Empty;
                if (oldKeyItem.Description != newKeyItem.Description)
                {
                    str += "事项说明【{0}】修改为【{1}】；".L10nFormat(oldKeyItem.Description, newKeyItem.Description);
                }
                if (oldKeyItem.BudgetId != newKeyItem.BudgetId)
                {
                    str += "预算编码【{0}】修改为【{1}】；".L10nFormat(oldKeyItem.BudgetNo, newKeyItem.BudgetNo);
                }
                if (oldKeyItem.BudgetAmount != newKeyItem.BudgetAmount)
                {
                    str += "事项预算【{0}】修改为【{1}】；".L10nFormat(oldKeyItem.BudgetAmount, newKeyItem.BudgetAmount);
                }
                if (oldKeyItem.ProjectWorkItemId != null && oldKeyItem.ProjectWorkItemId != newKeyItem.ProjectChangeWorkItem.ProjectWorkItemId)
                {
                    str += "事项计划【{0}】修改为【{1}】；".L10nFormat(oldKeyItem.ProjectWorkItem?.WorkItem, newKeyItem.ProjectChangeWorkItem?.WorkItem);
                }
                if (oldKeyItem.Remark != newKeyItem.Remark)
                {
                    str += "备注【{0}】修改为【{1}】；".L10nFormat(oldKeyItem.Remark, newKeyItem.Remark);
                }
                if (str.IsNotEmpty())
                {
                    var des = "关键事项【{0}】：".L10nFormat(oldKeyItem.Description);
                    str = des + str;
                    var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Modify, ChangeType.KeyItem, str);
                    list.Add(entity);
                }
            }
            //删除关键事项
            foreach (var oldKeyItem in oldKeyItems)
            {
                if (!editKeyItems.Any(p => p.ProjectKeyItemId == oldKeyItem.Id))
                {
                    var str = "删除关键事项【{0}】".L10nFormat(oldKeyItem.Description);
                    var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Delete, ChangeType.KeyItem, str);
                    list.Add(entity);
                }
            }
        }

        /// <summary>
        /// 生成项目成员变更内容
        /// </summary>
        /// <param name="list">变更内容</param>
        /// <param name="projectChange">项目变更</param>
        /// <param name="project">项目</param>
        private void GenerateMemberChange(EntityList<ProjectChangeContent> list, ProjectChange projectChange, Project project)
        {
            //增加项目成员
            var newMembers = GetChangeMembersByChangeIds(new List<double> { projectChange.Id });
            var addMembers = newMembers.Where(p => p.ProjectMemberId == null).ToList();
            foreach (var addMember in addMembers)
            {
                var str = "增加项目成员【工号：{0}；姓名：{1}；职位：{2}；备注：{3}】"
                    .L10nFormat(addMember.EmployeeCode, addMember.EmployeeName, addMember.Position, addMember.Remark);
                var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Add, ChangeType.Member, str);
                list.Add(entity);
            }
            //修改项目成员
            var oldMembers = RT.Service.Resolve<ProjectController>().GetMembersByProjectIds(new List<double> { project.Id });
            var editMembers = newMembers.Where(p => p.ProjectMemberId != null).ToList();
            foreach (var newMember in editMembers)
            {
                var oldMember = oldMembers.FirstOrDefault(p => p.Id == newMember.ProjectMemberId);
                if (oldMember == null)
                {
                    throw new ValidationException("数据异常，找不到id为:{0}的项目成员".L10nFormat(newMember.ProjectMemberId));
                }
                var str = string.Empty;
                if (oldMember.EmployeeId != newMember.EmployeeId)
                {
                    str += "工号姓名【{0}】修改为【{1}】；".L10nFormat(oldMember.EmployeeCode + oldMember.EmployeeName, newMember.EmployeeCode + newMember.EmployeeName);
                }
                if (oldMember.Position != newMember.Position)
                {
                    str += "职位【{0}】修改为【{1}】；".L10nFormat(oldMember.Position, newMember.Position);
                }
                if (oldMember.Remark != newMember.Remark)
                {
                    str += "备注【{0}】修改为【{1}】；".L10nFormat(oldMember.Remark, newMember.Remark);
                }
                if (oldMember.MemberStatus != newMember.MemberStatus)
                {
                    str += "状态【{0}】修改为【{1}】；".L10nFormat(oldMember.MemberStatus.ToLabel(), newMember.MemberStatus.ToLabel());
                }
                if (str.IsNotEmpty())
                {
                    var des = "项目成员【{0}】：".L10nFormat(oldMember.EmployeeCode + oldMember.EmployeeName);
                    str = des + str;
                    var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Modify, ChangeType.Member, str);
                    list.Add(entity);
                }
            }
            //删除项目成员
            foreach (var oldMember in oldMembers)
            {
                if (!editMembers.Any(p => p.ProjectMemberId == oldMember.Id))
                {
                    var str = "删除项目成员【{0}】".L10nFormat(oldMember.EmployeeCode + oldMember.EmployeeName);
                    var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Delete, ChangeType.Member, str);
                    list.Add(entity);
                }
            }
        }

        /// <summary>
        /// 生成工作计划变更内容
        /// </summary>
        /// <param name="list">变更内容</param>
        /// <param name="projectChange">项目变更</param>
        /// <param name="project">项目</param>
        private void GenerateWorkItemChange(EntityList<ProjectChangeContent> list, ProjectChange projectChange, Project project)
        {
            //增加项目计划
            var newWorkItems = GetChangeWorkItemsByChangeIds(new List<double> { projectChange.Id });
            var addWorkItems = newWorkItems.Where(p => p.ProjectWorkItemId == null).ToList();
            foreach (var addWorkItem in addWorkItems)
            {
                var str = "增加项目计划【计划节点：{0}；计划开始日期：{1}；计划结束日期：{2}；责任人：{3}】"
                    .L10nFormat(addWorkItem.WorkItem, addWorkItem.PlanStart, addWorkItem.PlantEnd, addWorkItem.PrincipalName);
                var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Add, ChangeType.WorkItem, str);
                list.Add(entity);
            }
            //修改项目计划
            var oldWorkItems = RT.Service.Resolve<ProjectController>().GetWorkItemsByProjectIds(new List<double> { project.Id });
            var editWorkItems = newWorkItems.Where(p => p.ProjectWorkItemId != null).ToList();
            foreach (var newWorkItem in editWorkItems)
            {
                var oldWorkItem = oldWorkItems.FirstOrDefault(p => p.Id == newWorkItem.ProjectWorkItemId);
                if (oldWorkItem == null)
                {
                    throw new ValidationException("数据异常，找不到id为:{0}的项目计划".L10nFormat(newWorkItem.ProjectWorkItemId));
                }
                var str = string.Empty;
                if (oldWorkItem.WorkItem != newWorkItem.WorkItem)
                {
                    str += "计划节点【{0}】修改为【{1}】；".L10nFormat(oldWorkItem.WorkItem, newWorkItem.WorkItem);
                }
                if (oldWorkItem.PlanStart != newWorkItem.PlanStart)
                {
                    str += "计划开始时间【{0}】修改为【{1}】；".L10nFormat(oldWorkItem.PlanStart, newWorkItem.PlanStart);
                }
                if (oldWorkItem.PlantEnd != newWorkItem.PlantEnd)
                {
                    str += "计划结束时间【{0}】修改为【{1}】；".L10nFormat(oldWorkItem.PlantEnd, newWorkItem.PlantEnd);
                }
                if (oldWorkItem.PrincipalId != newWorkItem.PrincipalId)
                {
                    str += "责任人【{0}】修改为【{1}】；".L10nFormat(oldWorkItem.PrincipalName, newWorkItem.PrincipalName);
                }
                if (str.IsNotEmpty())
                {
                    var des = "项目计划【{0}】：".L10nFormat(oldWorkItem.WorkItem);
                    str = des + str;
                    var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Modify, ChangeType.WorkItem, str);
                    list.Add(entity);
                }
            }
            //删除项目计划
            foreach (var oldWorkItem in oldWorkItems)
            {
                if (!editWorkItems.Any(p => p.ProjectWorkItemId == oldWorkItem.Id))
                {
                    var str = "删除项目计划【{0}】".L10nFormat(oldWorkItem.WorkItem);
                    var entity = CreateChangeContent(projectChange.Id, ChangeOperate.Delete, ChangeType.WorkItem, str);
                    list.Add(entity);
                }
            }
        }

        /// <summary>
        /// 创建变更内容
        /// </summary>
        /// <param name="changeId">变更id</param>
        /// <param name="operate">变更操作</param>
        /// <param name="type">变更类型</param>
        /// <param name="explain">变更说明</param>
        /// <returns>变更内容</returns>
        private ProjectChangeContent CreateChangeContent(double changeId, ChangeOperate operate, ChangeType type, string explain)
        {
            var entity = new ProjectChangeContent();
            entity.ProjectChangeId = changeId;
            entity.ChangeOperate = operate;
            entity.ChangeType = type;
            entity.ChangeExplain = explain;
            return entity;
        }

        /// <summary>
        /// 删除前校验项目变更最新状态
        /// </summary>
        /// <param name="ids">项目变更id</param>
        public virtual void DeleteCheckProjectChange(List<double> ids)
        {
            var projectChanges = GetProjectChangesByIds(ids);
            if (projectChanges.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能删除".L10N());
            }
            DB.Delete<ProjectChange>().Where(p => ids.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 删除校验项目变更成员是否被关联
        /// </summary>
        /// <param name="changeId">项目变更id</param>
        /// <param name="employeeId">员工id</param>
        /// <returns>是否被关联</returns>
        public virtual bool DeleteCheckChangeMember(double changeId, double employeeId)
        {
            var principalIds = GetWorkItemPrincipalIds(changeId);
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
        /// 提交项目变更
        /// </summary>
        /// <param name="changeIds">项目变更id</param>
        public virtual void SubmitProjectChange(List<double> changeIds)
        {
            //配置文件
            var config = RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(typeof(ProjectChange));
            var projectChanges = GetProjectChangesByIds(changeIds);
            if (projectChanges.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var allKeyItems = GetKeyItemsByChangeIds(changeIds);
            var now = RF.Find<ProjectChange>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var projectChange in projectChanges)
                {
                    //更新审核状态为【待审核】
                    projectChange.ApprovalStatus = ApprovalStatus.PendingReview;

                    //更新项目的项目状态为【变更中】
                    var project = GetById<Project>(projectChange.ProjectId);
                    if (project == null)
                    {
                        throw new ValidationException("提交数据异常，找不到id为:{0}的项目".L10nFormat(projectChange.ProjectId));
                    }
                    if (projectChange.IsRecovery == null || projectChange.IsRecovery == false)
                    {
                        project.PreviousStatus = project.ProjectStatus;
                    }
                    project.ProjectStatus = ProjectStatus.Changing;
                    RF.Save(project);

                    //更新关键事项对预算的占用
                    var keyItems = allKeyItems.Where(p => p.ProjectChangeId == projectChange.Id).ToList();
                    if (!keyItems.Any())
                    {
                        throw new ValidationException("请维护项目关键事项".L10N());
                    }
                    if (project.PlanType == PlanType.In)
                    {
                        if (keyItems.Any(p => p.BudgetId == null))
                        {
                            throw new ValidationException("计划类型为【计划内】时，关键事项的预算必输".L10N());
                        }
                        var budgetIds = keyItems.Select(p => p.BudgetId).ToList();
                        var existBudgets = RT.Service.Resolve<ProjectController>().ExistOtherProjectBudget(budgetIds, project.Id);
                        if (existBudgets.Any())
                        {
                            throw new ValidationException("预算【{0}】为二级预算，不可关联多个项目".L10nFormat(existBudgets.FirstOrDefault().BudgetNo));
                        }
                    }
                    UpdateKeyItemBudget(keyItems, true);
                }
                RF.Save(projectChanges);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(changeIds, typeof(ProjectChange).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    //审核
                    ExamineProjectChangeInner(changeIds, ApprovalResult.Pass, "通过".L10N(), projectChanges);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 更新关键事项对预算的占用
        /// </summary>
        /// <param name="keyItems">关键事项列表</param>
        /// <param name="isAdd">是否增加预算</param>
        private void UpdateKeyItemBudget(List<ProjectChangeKeyItem> keyItems, bool isAdd)
        {
            var oldKeyItemIds = keyItems.Select(p => p.ProjectKeyItemId).ToList();
            var oldKeyItems = RT.Service.Resolve<ProjectController>().GetKeyItemsByIds(oldKeyItemIds);
            var allBudgetIds = new List<double>();
            var budgetNullIds = keyItems.Where(p => p.BudgetId != null).Select(p => p.BudgetId).ToList();
            budgetNullIds.ForEach(p => allBudgetIds.Add(p.Value));
            var allBudgets = RT.Service.Resolve<BudgetController>().GetBudgetsByIds(allBudgetIds);
            foreach (var keyItem in keyItems)
            {
                if (keyItem.BudgetId == null)
                {
                    continue;
                }
                var budget = allBudgets.FirstOrDefault(p => p.Id == keyItem.BudgetId);
                if (budget == null)
                {
                    throw new ValidationException("数据异常，找不到id为:{0}的预算".L10nFormat(keyItem.BudgetId));
                }
                if (keyItem.ProjectKeyItemId.HasValue)// 修改关键事项
                {
                    var oldKeyItem = oldKeyItems.FirstOrDefault(p => p.Id == keyItem.ProjectKeyItemId.Value);
                    if (oldKeyItem == null)
                    {
                        throw new ValidationException("数据异常，找不到id为:{0}的关键事项".L10nFormat(keyItem.ProjectKeyItemId.Value));
                    }
                    //关键事项的预算编码没有变更，事项预算变大时
                    if (oldKeyItem.BudgetId == keyItem.BudgetId && keyItem.BudgetAmount > oldKeyItem.BudgetAmount)
                    {
                        //需要更新预算的【预占金额】为当前值加/减上【事项预算的差值】
                        var diffQty = keyItem.BudgetAmount - oldKeyItem.BudgetAmount;
                        UpdateReserveAmount(budget, isAdd, diffQty);
                    }
                    //关键事项的预算编码变更时
                    else
                    {
                        if (oldKeyItem.BudgetId != keyItem.BudgetId)
                        {
                            UpdateReserveAmount(budget, isAdd, keyItem.BudgetAmount);
                        }
                    }
                }
                else //新增了关键事项
                {
                    UpdateReserveAmount(budget, isAdd, keyItem.BudgetAmount);
                }
                //更新后，如果预算的【预占金额】+【已使用金额】大于【预算金额】则报错
                if (budget.ReserveAmount + budget.UsedAmount > budget.BudgetAmount)
                {
                    throw new ValidationException("预算{0}【预占金额】+【已使用金额】大于【预算金额】".L10nFormat(budget.BudgetNo));
                }
                if (budget.ReserveAmount < 0)
                {
                    throw new ValidationException("预算{0}【预占金额】不能小于0".L10nFormat(budget.BudgetNo));
                }
                RF.Save(budget);
            }
        }

        /// <summary>
        /// 更新预占金额
        /// </summary>
        /// <param name="budget">预算</param>
        /// <param name="isAdd">是否添加</param>
        /// <param name="diffQty">更新数量</param>
        private void UpdateReserveAmount(Budget budget, bool isAdd, decimal diffQty)
        {
            if (isAdd)
            {
                budget.ReserveAmount += diffQty;
            }
            else
            {
                budget.ReserveAmount -= diffQty;
            }
        }

        /// <summary>
        /// 撤回项目变更
        /// </summary>
        /// <param name="changeIds">项目变更id</param>
        public virtual void CancelProjectChange(List<double> changeIds)
        {
            var projectChanges = GetProjectChangesByIds(changeIds);
            if (projectChanges.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            var allKeyItems = GetKeyItemsByChangeIds(changeIds);
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var projectChange in projectChanges)
                {
                    //更新审核状态
                    projectChange.ApprovalStatus = ApprovalStatus.Draft;

                    //更新项目的项目状态为上一项目状态
                    var project = GetById<Project>(projectChange.ProjectId);
                    if (project == null)
                    {
                        throw new ValidationException("撤回异常，找不到id为:{0}的项目".L10nFormat(projectChange.ProjectId));
                    }
                    if (projectChange.IsRecovery == true)
                    {
                        project.ProjectStatus = ProjectStatus.Pause;
                        RF.Save(project);
                    }
                    else
                    {
                        if (project.PreviousStatus.HasValue)
                        {
                            project.ProjectStatus = project.PreviousStatus.Value;
                            project.PreviousStatus = null;
                            RF.Save(project);
                        }
                    }
                    //释放关键事项对预算的占用
                    var keyItems = allKeyItems.Where(p => p.ProjectChangeId == projectChange.Id).ToList();
                    UpdateKeyItemBudget(keyItems, false);
                }
                RF.Save(projectChanges);
                var now = RF.Find<ProjectChange>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(changeIds, typeof(ProjectChange).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核项目变更
        /// </summary>
        /// <param name="changeIds">项目变更id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExamineProjectChange(List<double> changeIds, ApprovalResult value, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineProjectChangeInner(changeIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核项目变更
        /// </summary>
        /// <param name="changeIds">项目变更id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        /// <param name="projectChanges">数据组</param>
        public virtual void ExamineProjectChangeInner(List<double> changeIds, ApprovalResult value, string remark, EntityList<ProjectChange> projectChanges = null)
        {
            if (projectChanges == null)
            {
                projectChanges = GetProjectChangesByIds(changeIds);
                if (!projectChanges.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            if (projectChanges.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            var allKeyItems = GetKeyItemsByChangeIds(changeIds);
            var now = RF.Find<ProjectChange>().GetDbTime();

            foreach (var projectChange in projectChanges)
            {
                var project = GetById<Project>(projectChange.ProjectId);
                if (project == null)
                {
                    throw new ValidationException("审核数据异常，找不到id为:{0}的项目".L10nFormat(projectChange.ProjectId));
                }
                var keyItems = allKeyItems.Where(p => p.ProjectChangeId == projectChange.Id).ToList();
                projectChange.ApprovalTime = now;
                if (value == ApprovalResult.Pass)
                {
                    if (project.PlanType == PlanType.In)
                    {
                        if (keyItems.Any(p => p.BudgetId == null))
                        {
                            throw new ValidationException("计划类型为【计划内】时，关键事项的预算必输".L10N());
                        }
                        var budgetIds = keyItems.Select(p => p.BudgetId).ToList();
                        var existBudgets = RT.Service.Resolve<ProjectController>().ExistOtherProjectBudget(budgetIds, project.Id);
                        if (existBudgets.Any())
                        {
                            throw new ValidationException("预算【{0}】为二级预算，不可关联多个项目".L10nFormat(existBudgets.FirstOrDefault().BudgetNo));
                        }
                    }
                    projectChange.ApprovalStatus = ApprovalStatus.Audited;
                    if (projectChange.IsSuspend == true)
                    {
                        project.ProjectStatus = ProjectStatus.Pause;
                    }
                    else
                    {
                        project.ProjectStatus = project.PreviousStatus ?? ProjectStatus.InProgress;
                        project.PreviousStatus = null;
                    }
                    RF.Save(project);

                    //更新预算的预占金额
                    ExamineUpdateBudget(keyItems);

                    //变更的内容进行生效
                    UpdateChangeContent(projectChange, project, keyItems);
                }
                else
                {
                    if (projectChange.IsRecovery == true)
                    {
                        project.ProjectStatus = ProjectStatus.Pause;
                        RF.Save(project);
                    }
                    else
                    {
                        if (project.PreviousStatus.HasValue)
                        {
                            project.ProjectStatus = project.PreviousStatus.Value;
                            project.PreviousStatus = null;
                            RF.Save(project);
                        }
                    }
                    projectChange.ApprovalStatus = ApprovalStatus.Reject;
                    //释放关键事项对预算的占用
                    UpdateKeyItemBudget(keyItems, false);
                }
            }
            RF.Save(projectChanges);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(changeIds, typeof(ProjectChange).FullName, value, now, remark);
        }

        /// <summary>
        /// 更新预算的预占金额
        /// </summary>
        /// <param name="keyItems">关键事项列表</param>
        private void ExamineUpdateBudget(List<ProjectChangeKeyItem> keyItems)
        {
            var oldKeyItemIds = keyItems.Select(p => p.ProjectKeyItemId).ToList();
            var oldKeyItems = RT.Service.Resolve<ProjectController>().GetKeyItemsByIds(oldKeyItemIds);
            var allBudgetIds = new List<double>();
            var budgetNullIds = keyItems.Where(p => p.BudgetId != null).Select(p => p.BudgetId).ToList();
            budgetNullIds.ForEach(p => allBudgetIds.Add(p.Value));
            var allBudgets = RT.Service.Resolve<BudgetController>().GetBudgetsByIds(allBudgetIds);
            foreach (var keyItem in keyItems)
            {
                if (keyItem.BudgetId == null)
                {
                    continue;
                }
                var budget = allBudgets.FirstOrDefault(p => p.Id == keyItem.BudgetId);
                if (budget == null)
                {
                    throw new ValidationException("数据异常，找不到id为:{0}的预算".L10nFormat(keyItem.BudgetId));
                }
                if (keyItem.ProjectKeyItemId.HasValue)
                {
                    var oldKeyItem = oldKeyItems.FirstOrDefault(p => p.Id == keyItem.ProjectKeyItemId.Value);
                    if (oldKeyItem == null)
                    {
                        throw new ValidationException("数据异常，找不到id为:{0}的关键事项".L10nFormat(keyItem.ProjectKeyItemId.Value));
                    }
                    //关键事项的预算编码没有变更，事项预算变小时
                    if (oldKeyItem.BudgetId == keyItem.BudgetId && keyItem.BudgetAmount < oldKeyItem.BudgetAmount)
                    {
                        //需要更新预算的【预占金额】为当前值减去【事项预算的差值】
                        var diffQty = oldKeyItem.BudgetAmount - keyItem.BudgetAmount;
                        budget.ReserveAmount -= diffQty;
                    }
                    //关键事项的预算编码变更时
                    if (oldKeyItem.BudgetId != keyItem.BudgetId)
                    {
                        var oldBudget = GetById<Budget>(oldKeyItem.BudgetId);
                        if (oldBudget == null)
                        {
                            throw new ValidationException("数据异常，找不到id为:{0}的预算".L10nFormat(oldKeyItem.BudgetId));
                        }
                        oldBudget.ReserveAmount -= oldKeyItem.BudgetAmount;
                        RF.Save(oldBudget);
                    }
                }
                RF.Save(budget);
            }
        }

        /// <summary>
        /// 变更的内容进行生效
        /// </summary>
        /// <param name="projectChange">项目变更</param>
        /// <param name="project">项目</param>
        /// <param name="newKeyItems">新关键事项</param> 
        private void UpdateChangeContent(ProjectChange projectChange, Project project, List<ProjectChangeKeyItem> newKeyItems)
        {
            project.Amount = projectChange.Amount;
            //工作计划变更的内容进行生效
            UpdateWorkItemChange(projectChange, project);
            //关键事项变更的内容进行生效
            UpdateKeyItemChange(newKeyItems, project);
            //项目成员变更的内容进行生效
            UpdateMemberChange(projectChange, project);
            RF.Save(project);
        }

        /// <summary>
        /// 关键事项变更的内容进行生效
        /// </summary>
        /// <param name="newKeyItems">新关键事项</param>
        /// <param name="project">项目</param>
        private void UpdateKeyItemChange(List<ProjectChangeKeyItem> newKeyItems, Project project)
        {
            var projectChangeWorkItemIds=newKeyItems.Select(p => p.ProjectChangeWorkItemId).Distinct().ToList();
            var projectChangeWorkItemList = Query<ProjectChangeWorkItem>().Where(p => projectChangeWorkItemIds.Contains(p.Id)).ToList();
            //增加关键事项
            var addKeyItems = newKeyItems.Where(p => p.ProjectKeyItemId == null).ToList();
            foreach (var addKeyItem in addKeyItems)
            {
                var projectWorkItem = projectChangeWorkItemList.FirstOrDefault(p => p.Id == addKeyItem.ProjectChangeWorkItemId);
                var entity = new ProjectKeyItem();
                entity.FactoryId = project.FactoryId;
                entity.DepartmentId = project.DepartmentId;
                entity.ProjectId = project.Id;
                entity.Description = addKeyItem.Description;
                entity.BudgetId = addKeyItem.BudgetId;
                entity.BudgetAmount = addKeyItem.BudgetAmount;
                entity.WorkStatus = WorkStatus.NotStarted;
                entity.Remark = addKeyItem.Remark;
                entity.ActualCost = addKeyItem.ActualCost;
                entity.LaborCost = addKeyItem.LaborCost;
                //项目计划
                entity.ProjectWorkItemId = projectWorkItem?.ProjectWorkItemId;

                RF.Save(entity);
                addKeyItem.ProjectKeyItemId = entity.Id;
                RF.Save(addKeyItem);
            }
            //修改关键事项
            var oldKeyItems = RT.Service.Resolve<ProjectController>().GetKeyItemsByProjectIds(new List<double> { project.Id });
            var editKeyItems = newKeyItems.Where(p => p.ProjectKeyItemId != null).ToList();
            foreach (var newKeyItem in editKeyItems)
            {
                var projectWorkItem = projectChangeWorkItemList.FirstOrDefault(p => p.Id == newKeyItem.ProjectChangeWorkItemId);
                var oldKeyItem = oldKeyItems.FirstOrDefault(p => p.Id == newKeyItem.ProjectKeyItemId);
                if (oldKeyItem == null)
                {
                    throw new ValidationException("数据异常，找不到id为:{0}的关键事项".L10nFormat(newKeyItem.ProjectKeyItemId));
                }
                oldKeyItem.Description = newKeyItem.Description;
                oldKeyItem.BudgetId = newKeyItem.BudgetId;
                oldKeyItem.BudgetAmount = newKeyItem.BudgetAmount;
                oldKeyItem.Remark = newKeyItem.Remark;
                oldKeyItem.ProjectWorkItemId = projectWorkItem?.ProjectWorkItemId;
                RF.Save(oldKeyItem);
            }
            //删除关键事项
            foreach (var oldKeyItem in oldKeyItems)
            {
                if (!newKeyItems.Any(p => p.ProjectKeyItemId == oldKeyItem.Id))
                {
                    oldKeyItem.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(oldKeyItem);
                }
            }
        }

        /// <summary>
        /// 项目成员变更的内容进行生效
        /// </summary>
        /// <param name="projectChange">项目变更</param>
        /// <param name="project">项目</param>
        private void UpdateMemberChange(ProjectChange projectChange, Project project)
        {
            //增加项目成员
            var newMembers = GetChangeMembersByChangeIds(new List<double> { projectChange.Id });
            var addMembers = newMembers.Where(p => p.ProjectMemberId == null).ToList();
            foreach (var addMember in addMembers)
            {
                var entity = new ProjectMember();
                entity.ProjectId = project.Id;
                entity.Position = addMember.Position;
                entity.EmployeeId = addMember.EmployeeId;
                entity.MemberStatus = addMember.MemberStatus;
                entity.Remark = addMember.Remark;
                RF.Save(entity);
                addMember.ProjectMemberId = entity.Id;
                RF.Save(addMember);
            }
            //修改项目成员
            var oldMembers = RT.Service.Resolve<ProjectController>().GetMembersByProjectIds(new List<double> { project.Id });
            var editMembers = newMembers.Where(p => p.ProjectMemberId != null).ToList();
            foreach (var newMember in editMembers)
            {
                var oldMember = oldMembers.FirstOrDefault(p => p.Id == newMember.ProjectMemberId);
                if (oldMember == null)
                {
                    throw new ValidationException("数据异常，找不到id为:{0}的项目成员".L10nFormat(newMember.ProjectMemberId));
                }
                oldMember.Position = newMember.Position;
                oldMember.EmployeeId = newMember.EmployeeId;
                oldMember.Remark = newMember.Remark;
                oldMember.MemberStatus = newMember.MemberStatus;
                RF.Save(oldMember);
            }
            //删除项目成员
            foreach (var oldMember in oldMembers)
            {
                if (!newMembers.Any(p => p.ProjectMemberId == oldMember.Id))
                {
                    oldMember.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(oldMember);
                }
            }
        }

        /// <summary>
        /// 工作计划变更的内容进行生效
        /// </summary>
        /// <param name="projectChange">项目变更</param>
        /// <param name="project">项目</param>
        private void UpdateWorkItemChange(ProjectChange projectChange, Project project)
        {
            //增加项目计划
            var newWorkItems = GetChangeWorkItemsByChangeIds(new List<double> { projectChange.Id });
            var addWorkItems = newWorkItems.Where(p => p.ProjectWorkItemId == null).ToList();
            foreach (var addWorkItem in addWorkItems)
            {
                var entity = new ProjectWorkItem();
                entity.ProjectId = project.Id;
                entity.WorkItem = addWorkItem.WorkItem;
                entity.PlanStart = addWorkItem.PlanStart;
                entity.PlantEnd = addWorkItem.PlantEnd;
                entity.PrincipalId = addWorkItem.PrincipalId;
                entity.WorkStatus = WorkStatus.NotStarted;
                RF.Save(entity);
                addWorkItem.ProjectWorkItemId = entity.Id;
                RF.Save(addWorkItem);
            }
            //修改项目计划
            var oldWorkItems = RT.Service.Resolve<ProjectController>().GetWorkItemsByProjectIds(new List<double> { project.Id });
            var editWorkItems = newWorkItems.Where(p => p.ProjectWorkItemId != null).ToList();
            foreach (var newWorkItem in editWorkItems)
            {
                var oldWorkItem = oldWorkItems.FirstOrDefault(p => p.Id == newWorkItem.ProjectWorkItemId);
                if (oldWorkItem == null)
                {
                    throw new ValidationException("数据异常，找不到id为:{0}的项目计划".L10nFormat(newWorkItem.ProjectWorkItemId));
                }
                oldWorkItem.WorkItem = newWorkItem.WorkItem;
                oldWorkItem.PlanStart = newWorkItem.PlanStart;
                oldWorkItem.PlantEnd = newWorkItem.PlantEnd;
                oldWorkItem.PrincipalId = newWorkItem.PrincipalId;
                RF.Save(oldWorkItem);
            }
            //删除项目计划
            foreach (var oldWorkItem in oldWorkItems)
            {
                if (!newWorkItems.Any(p => p.ProjectWorkItemId == oldWorkItem.Id))
                {
                    oldWorkItem.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(oldWorkItem);
                }
            }
        }

        /// <summary>
        /// 获取项目结项数量
        /// </summary>
        /// <param name="proId">项目id</param>
        /// <returns>项目结项数量</returns>
        public virtual int GetProjectCloseCount(double proId)
        {
            return Query<ProjectClose>().Where(p => p.ProjectId == proId).Count();
        }

        /// <summary>
        /// 获取项目结项
        /// </summary>
        /// <param name="id">结项id</param>
        /// <returns>项目结项</returns>
        public virtual ProjectClose GetProjectCloseById(double id)
        {
            return Query<ProjectClose>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询项目结项
        /// </summary>
        /// <param name="criteria">项目结项查询实体</param>
        /// <returns>项目结项</returns>
        public virtual EntityList<ProjectClose> CriteriaProjectCloses(ProjectCloseCriteria criteria)
        {
            var query = Query<ProjectClose>();
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
            if (!criteria.No.IsNullOrWhiteSpace() || !criteria.ProjectType.IsNullOrWhiteSpace() || criteria.Year.HasValue)
            {
                query.Join<Project>((a, b) => a.ProjectId == b.Id)
                    .WhereIf<Project>(!criteria.No.IsNullOrWhiteSpace(), (a, b) => b.Code.Contains(criteria.No))
                    .WhereIf<Project>(!criteria.ProjectType.IsNullOrWhiteSpace(), (a, b) => b.ProjectType == criteria.ProjectType)
                    .WhereIf<Project>(criteria.Year.HasValue, (a, b) => b.Year == criteria.Year);
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
        /// 根据id列表获取项目结项列表
        /// </summary>
        /// <param name="closeIds">id列表</param>
        /// <returns>项目结项列表</returns>
        public virtual EntityList<ProjectClose> GetProjectClosesByIds(List<double> closeIds)
        {
            return closeIds.SplitContains(ids => Query<ProjectClose>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 保存项目结项
        /// </summary>
        /// <param name="projectClose">项目结项</param>
        public virtual void SaveProjectClose(ProjectClose projectClose)
        {
            if (projectClose == null)
            {
                throw new ValidationException("保存项目结项失败，数据异常".L10N());
            }
            var project = GetById<Project>(projectClose.ProjectId);
            if (project == null)
            {
                throw new ValidationException("结项异常，找不到id为:{0}的项目".L10nFormat(projectClose.ProjectId));
            }
            var existCount = GetNoAuditChangeCount(0, project.Id);
            if (existCount > 0)
            {
                throw new ValidationException("项目存在未完结的变更申请，不能结项".L10N());
            }
            if (projectClose.PersistenceStatus != PersistenceStatus.New)
            {
                var oldProjectClose = GetById<ProjectClose>(projectClose.Id);
                if (oldProjectClose == null)
                {
                    throw new ValidationException("保存项目结项失败，数据异常".L10N());
                }
                if (oldProjectClose.ApprovalStatus != ApprovalStatus.Draft && oldProjectClose.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("保存项目结项失败，审核状态已不是【待提交】、【驳回】".L10N());
                }
            }
            else
            {
                var exist = GetProjectCloseCount(project.Id);
                if (exist > 0)
                {
                    throw new ValidationException("项目已存在结项数据".L10N());
                }
                projectClose.FactoryId = project.FactoryId;
                projectClose.DepartmentId = project.DepartmentId;
                projectClose.ApprovalStatus = ApprovalStatus.Draft;
            }
            RF.Save(projectClose);
        }

        /// <summary>
        /// 删除前校验项目结项最新状态
        /// </summary>
        /// <param name="ids">项目结项id</param>
        public virtual void DeleteCheckProjectClose(List<double> ids)
        {
            var projectCloses = GetProjectClosesByIds(ids);
            if (projectCloses.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能删除".L10N());
            }
        }

        /// <summary>
        /// 提交项目结项
        /// </summary>
        /// <param name="closeIds">项目结项id</param>
        public virtual void SubmitProjectClose(List<double> closeIds)
        {
            //验证
            //配置文件
            var config = RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(typeof(ProjectClose));
            var projectCloses = GetProjectClosesByIds(closeIds);
            if (projectCloses.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            foreach (var item in projectCloses)
            {
                var projectWorkItemList =  item.Project.ProjectWorkItemList.Select(p => p.WorkStatus == WorkStatus.InProgress || p.WorkStatus == WorkStatus.NotStarted).ToList();
                if (projectWorkItemList.Count>0 && projectWorkItemList.Any(p => p))
                {
                    throw new ValidationException("所提交的结项项目中项目工作计划列表存在未完成或未开始任务".L10N());
                }
            }
            var projectIds = projectCloses.Select(p => p.ProjectId).ToList();

            //校验项目编号所关联的采购申请对应的采购订单是否已经关闭
            List<string> PoOrderNoList = RT.Service.Resolve<IPurchases>().IsPurchaseOrderClose(projectIds);
            if (PoOrderNoList.Any())
            {
                string errmsg = string.Empty;
                PoOrderNoList.ForEach(e =>
                {
                    errmsg += e + ",";
                });
                if (errmsg.IsNotEmpty())
                {
                    errmsg = errmsg.Substring(0, errmsg.Length - 1);
                }
                throw new ValidationException("采购订单【{0}】还未完成或者未关闭".L10nFormat(errmsg));
            }
            var now = RF.Find<ProjectClose>().GetDbTime();
            projectCloses.ForEach(p => p.ApprovalStatus = ApprovalStatus.PendingReview);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(projectCloses);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(closeIds, typeof(ProjectClose).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    //审核
                    ExamineProjectCloseInner(closeIds, ApprovalResult.Pass, "通过".L10N(), projectCloses);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回项目结项
        /// </summary>
        /// <param name="closeIds">项目结项id</param>
        public virtual void CancelProjectClose(List<double> closeIds)
        {
            var projectCloses = GetProjectClosesByIds(closeIds);
            if (projectCloses.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                projectCloses.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(projectCloses);
                var now = RF.Find<ProjectClose>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(closeIds, typeof(ProjectClose).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核项目结项
        /// </summary>
        /// <param name="closeIds">项目结项id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExamineProjectClose(List<double> closeIds, ApprovalResult value, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineProjectCloseInner(closeIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核项目结项
        /// </summary>
        /// <param name="closeIds">项目结项id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        /// <param name="projectCloses">数据组</param>
        public virtual void ExamineProjectCloseInner(List<double> closeIds, ApprovalResult value, string remark, EntityList<ProjectClose> projectCloses = null)
        {
            var now = RF.Find<ProjectClose>().GetDbTime();
            //审核通过才计算
            EntityList<Budget> BudgetList = new EntityList<Budget>();

            if (projectCloses == null)
            {
                projectCloses = GetProjectClosesByIds(closeIds);
                if (!projectCloses.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            if (projectCloses.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            if (value == ApprovalResult.Pass)
            {
                //项目中止后的项目预算没有释放。项目结项功能的审核逻辑：审核通过时，该项目的所有项目事项，如果关联了预算且【事项成本】小于【事项预算】的，更新预算的【预占金额】为原来的值减上【事项预算-事项成本】
                //获取关键事项和不为空的预算
                //只处理项目状态为中止和结项的数据
                var projectIds = projectCloses.Select(p => p.ProjectId).ToList();
                //项目结项的结项类型只有项目中止,项目完结所以结项的数据都要验证
                var allKeyItems = RT.Service.Resolve<ProjectController>().GetKeyItemsByProjectIds(projectIds).Where(x => x.BudgetId != null).ToList();
                var budgetIds = new List<double>();
                var budgetNullIds = allKeyItems.Where(p => p.BudgetId != null).Select(p => p.BudgetId).Distinct().ToList();
                budgetNullIds.ForEach(p => budgetIds.Add(p.Value));
                BudgetList = RT.Service.Resolve<BudgetController>().GetBudgetsByIds(budgetIds);

                foreach (var keyItem in allKeyItems)
                {
                    if (keyItem.ActualCost == null)
                    {
                        keyItem.ActualCost = 0;
                    }
                    //事项预算大于事项成本才更新，事项预算小于或者等于事项成本则不变
                    if (keyItem.BudgetAmount > keyItem.ActualCost.Value && keyItem.BudgetId.HasValue)
                    {
                        var budget = BudgetList.FirstOrDefault(p => p.Id == keyItem.BudgetId.Value);
                        if (budget == null)
                        {
                            throw new ValidationException("找不到id为:{0}的预算".L10nFormat(keyItem.BudgetId.Value));
                        }
                        //事项占用的剩余金额= 事项预算-事项成本
                        var amount = keyItem.BudgetAmount - keyItem.ActualCost.Value;
                        //减少预占金额
                        budget.ReserveAmount -= amount;
                    }
                }
            }
            //保存预算数据
            if (BudgetList.Any())
            {
                RF.Save(BudgetList);
            }
            foreach (var projectClose in projectCloses)
            {
                projectClose.ApprovalTime = now;
                if (value == ApprovalResult.Pass)
                {
                    projectClose.ApprovalStatus = ApprovalStatus.Audited;
                    var project = GetById<Project>(projectClose.ProjectId);
                    if (project == null)
                    {
                        throw new ValidationException("数据异常，找不到id为:{0}的项目".L10nFormat(projectClose.ProjectId));
                    }
                    project.ProjectStatus = projectClose.CloseItemType == CloseItemType.ProjectClosing ? ProjectStatus.Closed : ProjectStatus.Abort;
                    RF.Save(project);
                }
                else
                {
                    projectClose.ApprovalStatus = ApprovalStatus.Reject;
                }
            }
            RF.Save(projectCloses);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(closeIds, typeof(ProjectClose).FullName, value, now, remark);
        }

        /// <summary>
        /// 根据项目id列表获取工作计划列表
        /// </summary>
        /// <param name="ProjectChangeId">项目id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>项目关键事项列表</returns>
        public virtual EntityList<ProjectChangeWorkItem> GetWorkItemsByProjectChangeIds(double ProjectChangeId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<ProjectChangeWorkItem>();
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.WorkItem.Contains(keyword));
            }
            return query.Where(p => p.ProjectChangeId == ProjectChangeId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
