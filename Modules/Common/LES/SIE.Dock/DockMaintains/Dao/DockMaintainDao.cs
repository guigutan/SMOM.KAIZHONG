using SIE.Core.Common.Dao;
using SIE.Dock.DockAppoints;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Dock.DockMaintains.Dao
{
    /// <summary>
    /// 月台维护数据访问
    /// </summary>
    public partial class DockMaintainDao : BaseDao<DockMaintain>
    {
        /// <summary>
        /// 获取月台维护数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>月台维护数据</returns>
        public virtual DockMaintain GetDockMaintain(double id, EagerLoadOptions elo)
        {
            return GetById(id);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回查询结果</returns>
        public virtual EntityList<DockMaintain> GetDockMaintains(DockMaintainCriteria criteria)
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

            //是否收货月台
            if (criteria.IsReceive.HasValue)
            {
                query.Where(p => p.IsReceive == criteria.IsReceive);
            }

            //是否发货月台
            if (criteria.IsShip.HasValue)
            {
                query.Where(p => p.IsShip == criteria.IsShip);
            }

            //状态
            if (criteria.State.HasValue)
            {
                query.Where(p => p.State == criteria.State);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取月台数据
        /// </summary>
        /// <param name="dockIds">月台Id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>月台数据</returns>
        public virtual EntityList<DockMaintain> GetDockMaintains(List<double> dockIds, EagerLoadOptions elo)
        {
            return dockIds.SplitContains(tmpIds =>
            {
                var query = Query();
                return query.Where(p => tmpIds.Contains(p.Id)).ToList(null, elo);
            });
        }
       
        /// <summary>
        /// 获取月台数据
        /// </summary>
        /// <param name="parkId">园区Id</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="state">状态</param>
        /// <param name="elo">贪婪</param>
        /// <returns></returns>
        public virtual EntityList<DockMaintain> GetDockMaintains(double? parkId, double? warehouseId, int? state, EagerLoadOptions elo = null)
        {
            var query = Query();
            if (parkId > 0)
                query.Where(p => p.YardZone.YardMaintainId == parkId);
            if (state.HasValue)
                query.Where(p => p.State ==(State)state);
            if (warehouseId > 0)
                query.Exists<DockMaintainWh>((x, y) => y.Where(p => p.DockMaintainId == x.Id && p.WarehouseId == warehouseId));
            query.Where(p => p.IsShip || p.IsReceive);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 通过园片区获取可用的月台
        /// </summary>
        /// <param name="ZoneId">圆片区ID</param>
        /// <param name="Appoint">预约类型</param>
        /// <returns></returns>
        public virtual EntityList<DockMaintain> GetDockMaintainsByZones(double ZoneId, int Appoint)
        {
            var query = Query().Where(p => p.State == State.Enable).Where(p => p.YardZoneId == ZoneId);
            if ((AppointType)Appoint == AppointType.Delivery)
            {
                query.Where(p => p.IsReceive);
            }
            else if ((AppointType)Appoint == AppointType.PickUp)
            {
                query.Where(p => p.IsShip);
            }
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取园区的月台数据
        /// </summary>
        /// <param name="parkId">园区ID</param>
        /// <param name="keyword">查询关键子</param>
        /// <param name="isReceive">是否收货</param>
        /// <param name="isShip">是否发货</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>月台数据</returns>
        public virtual EntityList<DockMaintain> GetDockMaintainByParkIds(double parkId, string keyword, bool? isReceive, bool? isShip, PagingInfo pagingInfo)
        {
            var query = Query().Where(p => p.YardZoneId == parkId && p.State == State.Enable);

            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            if (isReceive.HasValue)
            {
                query.Where(p => p.IsReceive == isReceive.Value);
            }

            if (isShip.HasValue)
            {
                query.Where(p => p.IsShip == isShip.Value);
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取可用的月台维护数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>月台维护数据</returns>
        public virtual EntityList<DockMaintain> GetEnabelDockMaintains(string keyword, PagingInfo pagingInfo)
        {
            var query = Query().Where(p => p.State == State.Enable);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据月台ID集合获取月台数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<DockMaintain> GetDockMaintainsByIds(List<double> ids)
        {
            return ids.SplitContains(tmpIds =>
            {
                return Query().Where(p => tmpIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
    }
}
