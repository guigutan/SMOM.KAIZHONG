using SIE.Common.Schdules;
using SIE.Fixtures;
using System;

namespace SIE.EMS.Job
{
    /// <summary>
    /// 工单自动生成工治具需求调度Job
    /// </summary>
    [Job("工单自动生成工治具需求调度Job", typeof(JobParameter))]
    public class FixtureDemandJob : JobBase
    {
        /// <summary>
        /// 执行JOB
        /// </summary>
        /// <param name="param"></param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

            try
            {
                RT.Service.Resolve<CoreFixtureController>().SyncSchedulingAutoCreateFixtureDemand();
                AddLog($"工单自动生成工治具需求调度Job执行成功 !");
            }
            catch (Exception exMsg)
            {
                AddLog($"工单自动生成工治具需求调度Job执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}
