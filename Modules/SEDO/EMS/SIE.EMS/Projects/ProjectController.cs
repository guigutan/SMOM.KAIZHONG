using SIE.Domain;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Projects
{
    /// <summary>
    /// 项目控制器
    /// </summary>
    public partial class ProjectController : DomainController
    {
        /// <summary>
        /// 根据id列表查询项目列表
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>项目列表</returns>
        public virtual EntityList<Project> GetProjectsByIds(List<double> ids)
        {
            return ids.SplitContains(id => Query<Project>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据工厂部门获取审核通过的项目
        /// </summary>
        /// <param name="factoryId">工厂</param>
        /// <param name="departmentId">部门</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">项目编码</param>
        /// <returns>项目列表</returns>
        public virtual EntityList<Project> GetAuditedProjects(double factoryId, double? departmentId, PagingInfo pagingInfo, string keyword)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(Project.PrincipalProperty);
            elo.LoadWithViewProperty();
            var query = Query<Project>();
            query.Where(p => p.FactoryId == factoryId && p.ApprovalStatus == ApprovalStatus.Audited && (p.ProjectStatus != Enums.ProjectStatus.Closed && p.ProjectStatus != Enums.ProjectStatus.Abort))
              .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword));
            if (departmentId.HasValue && departmentId > 0)
            {
                query.Where(p => p.DepartmentId == departmentId);
            }
            return query.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 获取审核通过的项目
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">项目编码</param>
        /// <returns>项目列表</returns>
        public virtual EntityList<Project> GetAuditedProjects(PagingInfo pagingInfo, string keyword)
        {
            return Query<Project>().Where(p => p.ApprovalStatus == ApprovalStatus.Audited)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)||p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取项目的关键事项
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>项目关键事项列表</returns>
        public virtual EntityList<ProjectKeyItem> GetProjectKeyItemsOfProject(double projectId, PagingInfo pagingInfo, string keyword)
        {
            return Query<ProjectKeyItem>()
                .Where(p => p.ProjectId == projectId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Description.Contains(keyword))
                .ToList(pagingInfo);
        }
    }
}
