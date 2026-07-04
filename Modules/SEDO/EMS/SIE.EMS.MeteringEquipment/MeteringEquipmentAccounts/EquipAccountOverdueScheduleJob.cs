using SIE.Common.Schdules;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{

    /// <summary>
    /// 自动更新计量设备台账超期停用
    /// </summary>
    [Job("自动更新计量设备台账超期停用", typeof(JobParameter))]

    public class EquipAccountOverdueScheduleJob : JobBase
    {
        /// <summary>
        /// 自动更新计量设备台账超期停用
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                RT.Service.Resolve<MeteringEquipmentAccountController>().AtuoEditOverdueSchedule();
                AddLog($"执行成功 !");
            }
            catch (Exception msg)
            {
                AddLog($"执行失败，错误信息: {msg.Message}");
            }
        }
    }
}
