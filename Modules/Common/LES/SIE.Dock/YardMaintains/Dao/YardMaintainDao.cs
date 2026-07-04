using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardMaintains.Dao
{
    /// <summary>
    /// 园区维护数据访问
    /// </summary>
    public partial class YardMaintainDao : BaseDao<YardMaintain>
    {
        /// <summary>
        /// 获取园区维护数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>园区维护数据</returns>
        public virtual YardMaintain GetYardMaintain(double id, EagerLoadOptions elo)
        {
            return GetById(id);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回查询结果</returns>
        public virtual EntityList<YardMaintain> GetYardMaintains(YardMaintainCriteria criteria)
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

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取可用园区维护数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>园区维护数据</returns>
        public virtual EntityList<YardMaintain> GetEnableYardMaintains(string keyword, PagingInfo pagingInfo)
        {
            var query = Query().Where(p => p.State == State.Enable);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取所有园区维护数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>园区维护数据</returns>
        public virtual EntityList<YardMaintain> GetYardMaintainList(string keyword, PagingInfo pagingInfo)
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
