using SIE.Common.Schdules;
using SIE.Dock.DockQueues.Service;
using System;

namespace SIE.Dock.DockJobs
{
    /// <summary>
    /// 月台排队推迟和签出调度Job
    /// </summary>
    [Job("月台排队推迟和签出调度", typeof(JobParameter))]
    public class DelayAndCheckOutJob : JobBase
    {
        /// <summary>
        /// 超期规则执行调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}]，当前身份[{RT.IdentityId}]\r\n");
            DateTime dt = DateTime.Now;
            try
            {
                RT.Service.Resolve<DockQueueService>().ExecuteDelayAndCheckOutData(dt);
                AddLog($"推迟和签出调度执行成功 !");
            }
            catch (Exception exMsg)
            {
                AddLog($"推迟和签出调度执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}