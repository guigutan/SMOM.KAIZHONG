using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Common.Schdules;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.Job
{
    /// <summary>
    /// 异常预警自动推送调度
    /// </summary>
    [Job("异常预警自动推送调度", typeof(JobParameter))]
    public class AbnormalSendMessageJob : JobBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        protected override void ExecuteJob(object param)
        {
            var curDate = DateTime.Now;
            try
            {
                RT.Service.Resolve<PushTargetController>().PushMessageJob(curDate);
                AddLog("历史时点:{0}异常预警自动推送调度完成。".L10nFormat(curDate.ToString()));
            }
            catch (Exception ex)
            {
                AddLog($"异常预警自动推送调度执行失败，错误信息: {ex.Message}");
            }
        }
    }
}
