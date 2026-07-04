using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Fixtures.Projects
{
    /// <summary>
    /// 工治具保养项目控制器
    /// </summary>
    public class MaintainProjectController : DomainController
    {
        /// <summary>
        /// 获取工治具保养列表
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<SIE.Fixtures.Projects.MaintainProject> GetMaintainProjects(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<SIE.Fixtures.Projects.MaintainProject>();
            return q.WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
