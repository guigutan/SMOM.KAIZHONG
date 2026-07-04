using SIE.Core.ApiModels;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.ProjectMaintains
{
    /// <summary>
    /// 项目维护控制器
    /// </summary>
    public class ProjectMaintainController : DomainController
    {
        /// <summary>
        /// 获取项目维护
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<ProjectMaintain> GetProjectMaintains(PagingInfo pagingInfo, string keyword)
        {
            return Query<ProjectMaintain>().Where(p => (p.Code.Contains(keyword) || p.Name.Contains(keyword)) && p.State == State.Enable).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询项目号基础信息
        /// </summary>
        /// <param name="keyword">api关键字</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetProjectMaintainBaseInfo(string keyword)
        {
            return Query<ProjectMaintain>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword))
                .Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                }).ToList<BaseDataInfo>().ToList();
        }
    }
}
