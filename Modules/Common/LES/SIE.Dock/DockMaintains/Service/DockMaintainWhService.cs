using SIE.Core.Common.Service;
using SIE.Dock.DockMaintains.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;

namespace SIE.Dock.DockMaintains.Service
{
    /// <summary>
    /// 月台维护适用仓库服务
    /// </summary>
    public class DockMaintainWhService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台维护数据访问
        /// </summary>
        private readonly DockMaintainWhDao _dockwhDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DockMaintainWhService(DockMaintainWhDao dockwhDao)
        {
            _dockwhDao = dockwhDao;
        }
        #endregion

        /// <summary>
        /// 获取适用仓库数据
        /// </summary>
        /// <param name="idlist">Id列表</param>
        /// <returns>适用仓库数据</returns>
        public virtual EntityList<DockMaintainWh> GetWhs(List<double> idlist)
        {
            return _dockwhDao.GetWhs(idlist);
        }

        /// <summary>
        /// 删除适用仓库数据
        /// </summary>
        /// <param name="whids">适用仓库列表</param>
        public virtual void DeleteDockWh(List<double> whids)
        {
            _dockwhDao.DeleteDockWh(whids);
        }
    }
}
