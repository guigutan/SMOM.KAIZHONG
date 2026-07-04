using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Dock.DockMaintains.Dao
{
    /// <summary>
    /// 月台维护适用仓库数据访问
    /// </summary>
    public class DockMaintainWhDao : BaseDao<DockMaintainWh>
    {
        /// <summary>
        /// 获取适用仓库数据
        /// </summary>
        /// <param name="idlist">Id列表</param>
        /// <returns>适用仓库数据</returns>
        public virtual EntityList<DockMaintainWh> GetWhs(List<double> idlist)
        {
            return idlist.SplitContains((tmpIds) =>
            {
                return Query().Where(p => tmpIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 删除适用仓库方法
        /// </summary>
        /// <param name="whids">适用仓库列表</param>
        public virtual void DeleteDockWh(List<double> whids)
        {
            whids.SplitDataExecute(tmpIds =>
            {
                Delete().Where(t => tmpIds.Contains(t.Id)).Execute();
            });
        }
    }
}
