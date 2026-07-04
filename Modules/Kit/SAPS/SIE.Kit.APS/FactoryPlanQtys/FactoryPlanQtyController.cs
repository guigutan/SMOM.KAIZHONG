using SIE.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.FactoryPlanQtys
{
    /// <summary>
    /// 工厂计划数配置控制器
    /// </summary>
    public class FactoryPlanQtyController : DomainController
    {

        /// <summary>
        /// 获取尺寸分组顺序
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<FactoryPlanQty> GetFactoryPlanQtyList(FactoryPlanQtyCriteria criteria)
        {
            var query = Query<FactoryPlanQty>();
            if (criteria.FactoryId > 0)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取尺寸分组顺序
        /// </summary>
        /// <param name="FactoryIds">工厂Id集合</param>
        /// <returns></returns>
        public virtual EntityList<FactoryPlanQty> GetFactoryPlanQtyList()
        {
            return Query<FactoryPlanQty>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
