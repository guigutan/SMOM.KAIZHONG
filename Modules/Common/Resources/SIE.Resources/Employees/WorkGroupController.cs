using SIE.Domain;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 班组控制器
    /// </summary>
    public partial class WorkGroupController : DomainController
    {
        /// <summary>
        /// 获取班组集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>班组集合</returns>
        public virtual EntityList<WorkGroup> GetWorkGroups(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WorkGroup>();
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
