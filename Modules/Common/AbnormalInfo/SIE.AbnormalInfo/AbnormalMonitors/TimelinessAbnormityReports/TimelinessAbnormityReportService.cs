
using SIE.AbnormalInfo.AbnormalMonitors.Dao;
using SIE.Common.Configs;
using SIE.Core.Common.Service;
using SIE.Domain;

namespace SIE.AbnormalInfo.AbnormalMonitors.Service
{
    /// <summary>
    /// 异常时效看板控制类
    /// </summary>
    public class TimelinessAbnormityReportService : DomainService
    {
        private readonly AbnormalMonitorTaskDao _abnormalMonitorTaskDao;

        /// <summary>
        /// AbnormalDecisionRuleDao
        /// </summary>
        public TimelinessAbnormityReportService(AbnormalMonitorTaskDao abnormalMonitorTaskDao)
        {
            _abnormalMonitorTaskDao = abnormalMonitorTaskDao;

        }

        /// <summary>
        /// 异常时效看板获取图表数据
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<AbnormalMonitorTask> GetChartData(int timevalue, int buttonType) {
            return _abnormalMonitorTaskDao.GetChartData(timevalue,buttonType);
        }

    }
}
