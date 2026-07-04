using SIE.Core.Common.Service;
using SIE.Dock.YardMaintains.Dao;
using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardMaintains.Service
{
    /// <summary>
    /// 园区维护服务
    /// </summary>
    public partial class YardMaintainService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 园区维护数据访问
        /// </summary>
        private readonly YardMaintainDao _yardmaintainDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public YardMaintainService(YardMaintainDao yardmaintainDao)
        {
            _yardmaintainDao = yardmaintainDao;
        }
        #endregion

        /// <summary>
        /// 获取园区维护数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>园区维护数据</returns>
        public virtual YardMaintain GetYardMaintain(double id, EagerLoadOptions elo)
        {
            return _yardmaintainDao.GetYardMaintain(id, elo);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回查询结果</returns>
        public virtual EntityList<YardMaintain> GetYardMaintains(YardMaintainCriteria criteria)
        {
            return _yardmaintainDao.GetYardMaintains(criteria);
        }

        /// <summary>
        /// 获取可用园区维护数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>园区维护数据</returns>
        public virtual EntityList<YardMaintain> GetEnableYardMaintains(string keyword, PagingInfo pagingInfo)
        {
            return _yardmaintainDao.GetEnableYardMaintains(keyword, pagingInfo);
        }
        /// <summary>
        /// 获取所有园区维护数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>园区维护数据</returns>
        public virtual EntityList<YardMaintain> GetYardMaintainList (string keyword, PagingInfo pagingInfo)
        {
            return _yardmaintainDao.GetYardMaintainList(keyword, pagingInfo);
        }
    }
}
