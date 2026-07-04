using SIE.Common.Schdules;
using System;

namespace SIE.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 自动生成特种设备定检计划
    /// </summary>
    [Job("自动生成特种设备定检计划", typeof(JobParameter))]
    internal class RegularInsAutoScheduleJob : JobBase
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
                RT.Service.Resolve<RegularInspectionController>().AtuoSchedule();
                AddLog($"执行成功 !");
            }
            catch (Exception msg)
            {
                AddLog($"执行失败，错误信息: {msg.Message}");
            }
        }
    }
}
