using SIE.Core.Common;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Packages.Packages
{
    /// <summary>
    /// 包装单位控制器
    /// </summary>
    public class PackingUnitController : DomainController
    {
        /// <summary>
        /// 获取除主单位的单位列表
        /// </summary>
        /// <returns>列表</returns>
        public virtual EntityList<PackingUnit> GetUnitExceptMaster(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<PackingUnit>().Where(p => !p.IsMasterUnit);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <returns>列表</returns>
        public virtual EntityList<PackingUnit> GetPackingUnits(List<double> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new EntityList<PackingUnit>();
            }
            return ids.SplitContains(sons =>
            {
                return Query<PackingUnit>().Where(p=>sons.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取主单位的单位
        /// </summary>
        /// <returns>列表</returns>
        public virtual PackingUnit GetUnitMaster()
        {
            var query = Query<PackingUnit>().Where(p => p.IsMasterUnit);            
            return query.FirstOrDefault();
        }
    }
}
