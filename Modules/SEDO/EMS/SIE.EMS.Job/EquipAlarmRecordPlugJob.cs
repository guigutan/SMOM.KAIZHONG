using SIE.Alert;
using SIE.Common.Schdules;
using SIE.EMS.AlertPlugs;
using SIE.EMS.Equipments;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SIE.EMS.Job
{
    /// <summary>
    /// 设备报警记录同步Job
    /// </summary>
    [Job("设备报警记录同步Job", typeof(JobParameter))]
    public class EquipAlarmRecordPlugJob : JobBase
    {
        /// <summary>
        /// 设备报警记录同步Job
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

            try
            {
                RT.Service.Resolve<EquipController>().SyncEquipAlarmRecord();
                AddLog($"设备报警记录同步Job执行成功 !");
            }
            catch (Exception exMsg)
            {
                AddLog($"设备报警记录同步Job执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}
