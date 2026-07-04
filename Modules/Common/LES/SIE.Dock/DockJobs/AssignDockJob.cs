using SIE.Common.Schdules;
using SIE.Dock.DockQueues.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Dock.DockJobs
{
    /// <summary>
    /// 月台排队数据分配月台调度Job
    /// </summary>
    [Job("月台排队数据分配月台调度", typeof(JobParameter))]
    public class AssignDockJob : JobBase
    {
        /// <summary>
        /// 超期规则执行调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}]，当前身份[{RT.IdentityId}]\r\n");
            var dockQueues = RT.Service.Resolve<DockQueueService>().GetDockQueueForJob();
            if (dockQueues.Any())
            {
                try
                {
                    RT.Service.Resolve<DockQueueService>().ExecuteAssignDockQueues(dockQueues);
                    AddLog($"分配月台执行成功 !");
                }
                catch (Exception exMsg)
                {
                    AddLog($"分配月台执行失败，错误信息: {exMsg.Message}");
                }
            }
            else
            {
                AddLog($"分配月台执行失败，错误信息: [未找到符合条件的数据]");
            }
        }
    }
}
