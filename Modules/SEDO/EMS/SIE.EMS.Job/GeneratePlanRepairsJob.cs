using SIE.Common.Schdules;
using SIE.EMS.EquipRepair.PlanRepairs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Job
{

    /// <summary>
    /// 自动生成计划维修任务调度Job
    /// </summary>
    [Job("自动生成计划维修调度", typeof(JobParameter))]
    public class GeneratePlanRepairsJob : JobBase
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
                var res = RT.Service.Resolve<PlanRepairsController>().SyncSchedulingAutoGeneratePlanRepairs();
                AddLog($"自动生成计划维修调度Job执行成功 !本次调度生成" + res.Count + "条计划维修数据");
            }
            catch (Exception exMsg)
            {
                AddLog($"自动生成计划维修调度Job执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}
