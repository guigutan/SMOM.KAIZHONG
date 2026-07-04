using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Enums;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.EarlierStage.Projects
{
    #region 项目成员
    /// <summary>
    /// 同一个项目下成员不能重复
    /// </summary>
    [DisplayName("同一个项目下成员不能重复")]
    [Description("同一个项目下成员不能重复")]
    public class ProjectMemberNotDuplicateRule : NotDuplicateRule<ProjectMember>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectMemberNotDuplicateRule()
        {
            Properties.Add(ProjectMember.ProjectIdProperty);
            Properties.Add(ProjectMember.EmployeeIdProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                return "同一个项目下成员不能重复".L10N();
            };
        }
    }

    /// <summary>
    /// 项目成员被关联后不能删除
    /// </summary>
    [DisplayName("项目成员被关联后不能删除")]
    [Description("项目成员被关联后不能删除")]
    public class ProjectMemberRule : EntityRule<ProjectMember>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectMemberRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">参数明细</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var member = entity as ProjectMember;
            if (member.PersistenceStatus == PersistenceStatus.Deleted)
            {
                var check = RT.Service.Resolve<ProjectController>().DeleteCheckMember(member.ProjectId, member.EmployeeId);
                if (check)
                {
                    e.BrokenDescription = "项目成员：{0}被关联后不能删除".L10nFormat(member.Employee?.Name);
                }
            }
        }
    }
    #endregion

    #region 项目计划

    /// <summary>
    /// 同一个项目下计划节点不能重复
    /// </summary>
    [DisplayName("同一个项目下计划节点不能重复")]
    [Description("同一个项目下计划节点不能重复")]
    public class ProjectWorkItemNotDuplicateRule : NotDuplicateRule<ProjectWorkItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectWorkItemNotDuplicateRule()
        {
            Properties.Add(ProjectWorkItem.ProjectIdProperty);
            Properties.Add(ProjectWorkItem.WorkItemProperty);
            Scope = EntityStatusScopes.Add;
            this.MessageBuilder = e =>
            {
                return "同一个项目下计划节点不能重复".L10N();
            };
        }
    }

    /// <summary>
    /// 项目计划验证规则
    /// </summary>
    [DisplayName("项目计划验证规则")]
    [Description("项目计划验证规则")]
    public class ProjectWorkItemRule : EntityRule<ProjectWorkItem>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">参数明细</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var workItem = entity as ProjectWorkItem;
            if (workItem.PlanStart >= workItem.PlantEnd)
            {
                e.BrokenDescription = "计划开始时间必须小于计划结束时间".L10N();
            }
        }
    }
    #endregion

    #region 事项成员
    /// <summary>
    /// 同一个事项下成员不能重复
    /// </summary>
    [DisplayName("同一个事项下成员不能重复")]
    [Description("同一个事项下成员不能重复")]
    public class ProjectKeyItemMemberNotDuplicateRule : NotDuplicateRule<ProjectKeyItemMember>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectKeyItemMemberNotDuplicateRule()
        {
            Properties.Add(ProjectKeyItemMember.ProjectKeyItemIdProperty);
            Properties.Add(ProjectKeyItemMember.EmployeeIdProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                return "同一个事项下成员不能重复".L10N();
            };
        }
    }

    /// <summary>
    /// 事项成员被关联后不能删除
    /// </summary>
    [DisplayName("事项成员被关联后不能删除")]
    [Description("事项成员被关联后不能删除")]
    public class ProjectKeyItemMemberRule : EntityRule<ProjectKeyItemMember>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectKeyItemMemberRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">参数明细</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var member = entity as ProjectKeyItemMember;
            if (member.PersistenceStatus == PersistenceStatus.Deleted)
            {
                var check = RT.Service.Resolve<ProjectController>().DeleteCheckKeyItemMember(member.ProjectKeyItemId, member.EmployeeId);
                if (check)
                {
                    e.BrokenDescription = "事项成员：{0}被关联后不能删除".L10nFormat(member.Employee.Name);
                }
            }
        }
    }
    #endregion

    #region 事项计划
    /// <summary>
    /// 事项计划验证规则
    /// </summary>
    [DisplayName("事项计划验证规则")]
    [Description("事项计划验证规则")]
    public class ProjectKeyItemPlanRule : EntityRule<ProjectKeyItemPlan>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectKeyItemPlanRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update | EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">参数明细</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var keyItemPlan = entity as ProjectKeyItemPlan;
            if (keyItemPlan.PersistenceStatus == PersistenceStatus.Deleted)
            {
                if (keyItemPlan.WorkStatus == WorkStatus.InProgress || keyItemPlan.WorkStatus == WorkStatus.Finish)
                {
                    e.BrokenDescription = "状态为【进行中】、【已完成】的事项不能删除".L10N();
                }
            }
            else
            {
                if (keyItemPlan.PlanStart >= keyItemPlan.PlanEnd)
                {
                    e.BrokenDescription = "计划开始时间必须小于计划结束时间".L10N();
                }
            }
        }
    }
    #endregion

    #region 项目变更成员
    /// <summary>
    /// 同一个项目下成员不能重复
    /// </summary>
    [DisplayName("同一个项目下成员不能重复")]
    [Description("同一个项目下成员不能重复")]
    public class ProjectChangeMemberNotDuplicateRule : NotDuplicateRule<ProjectChangeMember>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectChangeMemberNotDuplicateRule()
        {
            Properties.Add(ProjectChangeMember.ProjectChangeIdProperty);
            Properties.Add(ProjectChangeMember.EmployeeIdProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                return "同一个项目下成员不能重复".L10N();
            };
        }
    }

    /// <summary>
    /// 项目成员被关联后不能删除
    /// </summary>
    [DisplayName("项目成员被关联后不能删除")]
    [Description("项目成员被关联后不能删除")]
    public class ProjectChangeMemberRule : EntityRule<ProjectChangeMember>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectChangeMemberRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">参数明细</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var member = entity as ProjectChangeMember;
            if (member.PersistenceStatus == PersistenceStatus.Deleted)
            {
                var check = RT.Service.Resolve<ProjectChangeController>().DeleteCheckChangeMember(member.ProjectChangeId, member.EmployeeId);
                if (check)
                {
                    e.BrokenDescription = "项目成员：{0}被关联后不能删除".L10nFormat(member.Employee?.Name);
                }
            }
        }
    }
    #endregion
}
