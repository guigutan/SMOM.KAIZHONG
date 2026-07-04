using SIE.Common.Schdules;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.APS.EngineerPlans
{
    /// <summary>
    /// 工程计划MI自动排计划
    /// </summary>
    [Job("工程计划MI自动排计划", typeof(JobParameter))]
    public class EngineerPlanAutoScheduleJob : JobBase
    {
        /// <summary>
        /// 设备点检计划自动生成
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                //RT.Service.Resolve<EngineerPlanController>().DoSchedule(DateTime.Now); 
                RT.Service.Resolve<EngineerPlanController>().DoSchedule();
                AddLog($"执行成功 !");
            }
            catch (Exception msg)
            {
                AddLog($"执行失败，错误信息: {msg.Message}");
            }
        }
    }
}
