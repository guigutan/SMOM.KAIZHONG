using SIE.Common.Schdules;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 设备点检计划自动生成
    /// </summary>
    [Job("设备点检计划自动生成", typeof(CheckPlanCrtJobParameter))]
    public class CheckPlanCreateJob : JobBase
    {
        /// <summary>
        /// 设备点检计划自动生成
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                var jobParam = param as CheckPlanCrtJobParameter;
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                RT.Service.Resolve<CheckPlanController>().AutoCreateCheckPlans(jobParam);
                AddLog($"设备点检计划自动生成执行成功 !");
            }
            catch (Exception msg)
            {
                AddLog($"设备点检计划自动生成执行信息: {msg.Message}");
            }
        }
    }
}
