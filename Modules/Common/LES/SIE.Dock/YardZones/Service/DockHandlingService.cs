using SIE.Core.Common.Service;
using SIE.Dock.YardZones.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardZones.Service
{
    /// <summary>
    /// 月台装卸能力服务
    /// </summary>
    public partial class DockHandlingService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台装卸能力数据访问
        /// </summary>
        private readonly DockHandlingDao _dockhandlingDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DockHandlingService(DockHandlingDao dockhandlingDao)
        {
            _dockhandlingDao = dockhandlingDao;
        }
        #endregion

        /// <summary>
        /// 获取月台装卸能力数据
        /// </summary>
        /// <param name="yardZoneIds">园片区Id</param>
        /// <returns>月台装卸能力数据</returns>
        public virtual EntityList<DockHandling> GetDockHandlings(List<double> yardZoneIds)
        {
            return _dockhandlingDao.GetDockHandlings(yardZoneIds);
        }

        /// <summary>
        /// 获取月台装卸能力列表
        /// </summary>
        /// <returns>返回月台装卸能力列表</returns>
        public virtual EntityList<DockHandling> GetDockHandlingsByDockIds(List<double> dockIds)
        {
            return _dockhandlingDao.GetDockHandlingsByDockIds(dockIds);
        }
    }
}
