using SIE.Defects;
using SIE.Domain;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks
{

    /// <summary>
    /// 车间（非树形结构）控制器
    /// </summary>
    public class WorkShopController : DomainController
    {
        /// <summary>
        /// 获取企业模型
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<WorkShop> GetEnterprises(EnterpriseType type, PagingInfo pagingInfo, string keyword)
        {
            return Query<WorkShop>()
                .Where(p => p.Level.Type == type && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg))
                .Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo);
        }

        /// <summary>
        /// 获取缺陷
        /// </summary>
        /// <param name="defectCodes"></param>
        /// <returns></returns>
        public virtual EntityList<Defect> GetDefects(List<string> defectCodes)
        {
            if (!defectCodes.IsNotEmpty())
                return new EntityList<Defect>();

            var defectList = Query<Defect>().Where(c => defectCodes.Contains(c.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return defectList;

        }
    }
}
