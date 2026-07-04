using SIE.Common.Schdules;
using SIE.MES.Statistics.WIP;
using System;

namespace SIE.MES.Job
{
    /// <summary>
    /// 更新生产日进度Job
    /// </summary>
    [Job("更新生产日进度Job", typeof(JobParameter))]
    public class CallUpdateDailySchedulesJob : JobBase
    {
        /// <summary>
        /// 更新生产日进度Job执行方法
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var curMsg = RT.Service.Resolve<WipStatisticsController>().RunUpdateDailySchedules();
                AddLog(curMsg);
            }
            catch (Exception exMsg)
            {
                AddLog($"报工执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}