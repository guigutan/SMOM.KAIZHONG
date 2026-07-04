using SIE.Common.Schdules;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions
{
    /// <summary>
    /// 调度任务设置控制器
    /// </summary>
    public class AbnormalJobConfigController : DomainController
    {

        /// <summary>
        /// 获取【异常定义生成异常任务调度】列表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<JobConfig> GetJobConfigs(string keyword, PagingInfo pagingInfo)
        {
            var className = "SIE.AbnormalInfo.Job.AbnormalDefinitionJob,SIE.AbnormalInfo.Job";
            return Query<JobConfig>().Where(p => p.JobClass == className).WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword)).ToList(pagingInfo, null);
        }

        /// <summary>
        /// 获取调度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual JobConfig GetJobConfig(string key)
        {
            return Query<JobConfig>().Where(p => p.Key == key).FirstOrDefault();
        }
    }
}
