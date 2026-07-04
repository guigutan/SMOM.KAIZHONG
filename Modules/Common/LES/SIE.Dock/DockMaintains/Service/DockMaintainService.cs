using SIE.Core.Common.Service;
using SIE.Dock.DockMaintains.Dao;
using SIE.Domain;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.DockMaintains.Service
{
    /// <summary>
    /// 月台维护服务
    /// </summary>
    public partial class DockMaintainService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台维护数据访问
        /// </summary>
        private readonly DockMaintainDao _dockmaintainDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DockMaintainService(DockMaintainDao dockmaintainDao)
        {
            _dockmaintainDao = dockmaintainDao;
        }
        #endregion

        /// <summary>
        /// 获取月台维护数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>月台维护数据</returns>
        public virtual DockMaintain GetDockMaintain(double id, EagerLoadOptions elo)
        {
            return _dockmaintainDao.GetDockMaintain(id, elo);
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
            return _dockmaintainDao.GetDockMaintainByParkIds(parkId, keyword, isReceive, isShip, pagingInfo);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回查询结果</returns>
        public virtual EntityList<DockMaintain> GetDockMaintains(DockMaintainCriteria criteria)
        {
            return _dockmaintainDao.GetDockMaintains(criteria);
        }

        /// <summary>
        /// 获取月台数据
        /// </summary>
        /// <param name="dockIds">月台Id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>月台数据</returns>
        public virtual EntityList<DockMaintain> GetDockMaintains(List<double> dockIds, EagerLoadOptions elo = null)
        {
            return _dockmaintainDao.GetDockMaintains(dockIds, elo);
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
            return _dockmaintainDao.GetDockMaintains(parkId, warehouseId, state, elo);
        }

        /// <summary>
        /// 获取可用的月台维护数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>月台维护数据</returns>
        public virtual EntityList<DockMaintain> GetEnabelDockMaintains(string keyword, PagingInfo pagingInfo)
        {
            return _dockmaintainDao.GetEnabelDockMaintains(keyword, pagingInfo);
        }

        /// <summary>
        /// 通过排队地点ID获取月台
        /// </summary>
        /// <param name="ZonesId">园区ID</param>
        /// <param name="Appoint">排队状态</param>
        /// <returns></returns>
        public virtual EntityList<DockMaintain> GetDockMaintainsByZones(double ZonesId, int Appoint)
        {
            return _dockmaintainDao.GetDockMaintainsByZones(ZonesId, Appoint);
        }

        /// <summary>
        /// 预约图提交检查时间是否正确
        /// </summary>      
        public virtual bool PraseDate(string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt) && dt > RF.Find<DockMaintain>().GetDbTime();
        }

        /// <summary>
        /// 通过月台ID集合获取月台数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<DockMaintain> GetDockMaintainsByIds(List<double> ids)
        {
            return _dockmaintainDao.GetDockMaintainsByIds(ids);
        }
    }
}
