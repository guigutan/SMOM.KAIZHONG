using SIE.Common.Schdules;
using SIE.EMS.SpareParts;
using System;

namespace SIE.EMS.Job
{


    /// <summary>
    /// 自动生成备件库综合统计报表调度Job
    /// </summary>
    [Job("备件库存综合统计日常调度Job", typeof(JobParameter))]
    public class SpartPartMixReportJob : JobBase
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
                RT.Service.Resolve<SpartPareReportJobController>().SyncSchedulingAutoStatistics();
                AddLog($"备件库存综合统计日常调度Job执行成功 !");
            }
            catch (Exception exMsg)
            {
                AddLog($"备件库存综合统计日常调度Job执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}
