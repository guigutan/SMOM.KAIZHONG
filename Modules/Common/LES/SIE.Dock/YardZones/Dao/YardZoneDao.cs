using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardZones.Dao
{
    /// <summary>
    /// 园片区维护数据访问
    /// </summary>
    public partial class YardZoneDao : BaseDao<YardZone>
    {       
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回查询结果</returns>
        public virtual EntityList<YardZone> GetYardZones(YardZoneCriteria criteria)
        {
            var query = Query();

            //编码
            if (!string.IsNullOrEmpty(criteria.Code))
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }

            //名称
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }

            //园区
            if (criteria.YardMaintainNameId.HasValue)
            {
                query.Where(p => p.YardMaintainId == criteria.YardMaintainNameId.Value);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取可用园片区维护数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>园片区维护数据</returns>
        public virtual EntityList<YardZone> GetEnableYardZones(string keyword, PagingInfo pagingInfo)
        {
            var query = Query();
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }        
    }
}
