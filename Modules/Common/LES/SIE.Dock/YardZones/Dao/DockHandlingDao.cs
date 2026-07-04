using SIE.Core.Common.Dao;
using SIE.Dock.DockMaintains;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardZones.Dao
{
    /// <summary>
    /// 月台装卸能力数据访问
    /// </summary>
    public partial class DockHandlingDao : BaseDao<DockHandling>
    {
        /// <summary>
        /// 获取月台装卸能力列表
        /// </summary>
        /// <returns>返回月台装卸能力列表</returns>
        public virtual EntityList<DockHandling> GetDockHandlings(List<double> yardZoneIds)
        {
            return Query().Where(p => yardZoneIds.Contains(p.YardZoneId)).ToList();
        }

        /// <summary>
        /// 获取月台装卸能力列表
        /// </summary>
        /// <returns>返回月台装卸能力列表</returns>
        public virtual EntityList<DockHandling> GetDockHandlingsByDockIds(List<double> dockIds)
        {
            return Query().Join<DockMaintain>((x, y) => x.YardZoneId == y.YardZoneId).Where<DockMaintain>((x, y) => dockIds.Contains(y.Id)).Distinct().ToList();
        }
    }
}
