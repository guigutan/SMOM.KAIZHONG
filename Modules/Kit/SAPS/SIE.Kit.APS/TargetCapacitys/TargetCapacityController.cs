using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.TargetCapacitys
{
    /// <summary>
    /// 目标产能控制器
    /// </summary>
    public class TargetCapacityController : DomainController
    {
        /// <summary>
        /// 查询目标产能
        /// </summary>
        /// <param name="criteria">目标产能列表</param>
        /// <returns></returns>
        public virtual EntityList<TargetCapacity> GetTargetCapacityList(TargetCapacityCriteria criteria)
        {
            var query = Query<TargetCapacity>();
            if (criteria.EnterpriseId > 0)
            {
                query.Where(p => p.EnterpriseId == criteria.EnterpriseId);
            }
            if (criteria.Year.IsNotEmpty())
            {
                query.Where(p => p.Year == criteria.Year);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(TargetCapacity.EnterpriseProperty);
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        ///  按年份获取对应的产能数据
        /// </summary>
        /// <param name="year">按年份</param>
        /// <returns>返回产能实体</returns>
        public virtual EntityList<TargetCapacity> getYearTargetCapacity(string year)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(TargetCapacity.EnterpriseProperty);
            return Query<TargetCapacity>().Where(x => x.Year == year).ToList(null, elo);
        }

        /// <summary>
        ///  按年份获取对应的产能数据
        /// </summary>
        /// <param name="years">按年份</param>
        /// <returns>返回产能实体</returns>
        public virtual EntityList<TargetCapacity> GetYearTargetCapacity(List<string> years)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(TargetCapacity.EnterpriseProperty);
            return Query<TargetCapacity>().Where(x => years.Contains(x.Year)).ToList();
        }

        /// <summary>
        /// 验证目标产能数据是否有交互
        /// </summary>
        /// <param name="taCapacity">目标产能对象</param>
        /// <returns></returns>
        public virtual bool ValidateTargetCapacity(TargetCapacity taCapacity)
        {
            var query = Query<TargetCapacity>().Where(p => p.Year == taCapacity.Year);
            if (taCapacity.Id > 0)
            {
                query.Where(p => p.Id != taCapacity.Id);
            }
            if (taCapacity.EnterpriseId > 0)
            {
                query.Where(p => p.EnterpriseId == taCapacity.EnterpriseId);
            }
            var list = query.ToList();
            if (list.Any()) return false;
            return true;
        }
    }
}
