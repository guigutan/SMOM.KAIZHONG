using SIE.Core.Common.Dao;
using SIE.Dock.DockMaintains;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Dock.DockRunMts.Dao
{
    /// <summary>
    /// 月台运行维护数据访问
    /// </summary>
    public class DockRunMtDao : BaseDao<DockRunMt>
    {
        /// <summary>
        /// 获取月台运行维护数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>月台维护数据</returns>
        public virtual DockRunMt GetDockRunMt(double id, EagerLoadOptions elo)
        {
            return GetById(id);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回查询结果</returns>
        public virtual EntityList<DockRunMt> GetDockRunMts(DockRunMtCriteria criteria)
        {
            var query = Query();
            query.Join<DockMaintain>((x, y) => x.DockMaintainId == y.Id);

            //月台编码
            if (!string.IsNullOrEmpty(criteria.Code))
            {
                query.Where<DockMaintain>((x, y) => y.Code.Contains(criteria.Code));
            }

            //月台名称
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                query.Where<DockMaintain>((x, y) => y.Name.Contains(criteria.Name));
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 根据月台ID获取月台运行数据
        /// </summary>
        /// <param name="dockIds">月台ID集合</param>
        /// <returns>月台运行数据</returns>
        public virtual EntityList<DockRunMt> GetDockRunMtByDockIds(List<double> dockIds)
        {
            return dockIds.SplitContains(tmpIds =>
            {
                return Query().Where(p => tmpIds.Contains(p.DockMaintainId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
    }
}
