using SIE.Core.Common.Service;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks
{
    /// <summary>
    /// 
    /// </summary>
    public class AbnormalMonitorTaskLogService : DomainService
    {
        private readonly AbnormalMonitorTaskLogDao _abnormalMonitorTaskLogDao;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalMonitorTaskLogDao"></param>
        public AbnormalMonitorTaskLogService(AbnormalMonitorTaskLogDao abnormalMonitorTaskLogDao)
        {
            _abnormalMonitorTaskLogDao = abnormalMonitorTaskLogDao;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalMonitorTaskId"></param>
        /// <returns></returns>
        public virtual EntityList<AbnormalMonitorTaskLog> GetList(double abnormalMonitorTaskId, PagingInfo pagingInfo)
        {
            return _abnormalMonitorTaskLogDao.GetList(abnormalMonitorTaskId, pagingInfo);
        }
    }
}
