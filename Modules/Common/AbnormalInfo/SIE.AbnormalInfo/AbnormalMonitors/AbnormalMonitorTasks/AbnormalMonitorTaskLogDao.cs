using SIE.AbnormalInfo.Common;
using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks
{
    /// <summary>
    /// 
    /// </summary>
    public class AbnormalMonitorTaskLogDao : BaseDao<AbnormalMonitorTaskLog>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalMonitorTaskId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<AbnormalMonitorTaskLog> GetList(double abnormalMonitorTaskId, PagingInfo pagingInfo)
        {
            var query = Query();
            query.Where(c => c.AbnormalMonitorTaskId == abnormalMonitorTaskId);
            query.OrderBy(c => c.CreateDate);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
      
    }
}
