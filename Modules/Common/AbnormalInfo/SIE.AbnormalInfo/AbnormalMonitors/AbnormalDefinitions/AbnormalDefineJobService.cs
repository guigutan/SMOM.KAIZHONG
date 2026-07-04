using SIE.Common.Schdules;
using SIE.Core.Common.Service;
using SIE.Domain;
using System;
using System.Linq;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常定义调度服务
    /// </summary>
    public class AbnormalDefineJobService : DomainService
    {
        private readonly AbnormalDefineDao _abnormalDefineDao;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalDefineDao"></param>
        public  AbnormalDefineJobService(AbnormalDefineDao abnormalDefineDao)
        {
            _abnormalDefineDao = abnormalDefineDao;
        }

        /// <summary>
        /// 生成任务
        /// </summary>
        /// <param name="curTime"></param>
        /// <param name="jobConfigId">调度Id</param>
        /// <param name="jobParameter">参数</param>
        public virtual string GenerateTask(DateTime curTime, double jobConfigId, JobParameter jobParameter)
        {
            var list = _abnormalDefineDao.FindMany(c => c.JobConfigId == jobConfigId && c.State == State.Enable);
            StringBuilder msg = new StringBuilder();
            if (list.IsNotEmpty())
            {
                list.ForEach(c =>
                {
                    //用中间件进行解耦
                    var info = RT.Service.Resolve<AbnormalMonitorTaskService>().GenerateTaskEventHandle(c);
                    msg.Append($"规则[{c.MonitorName}]执行信息:{info}\r\n");
                });

            }
            return msg.ToString();
        }
    }
}
