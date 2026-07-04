using SIE.Common.Schdules;
using SIE.MES.AutoJoinLineWarehouse;
using System;

namespace SIE.MES.Job.AutoJoinLineWarehouse
{
    /// <summary>
    /// 半成品自动入线边仓调度Job
    /// </summary>
    [Job("半成品自动入线边仓调度", typeof(JobParameter))]
    public class AutoJoinLineWarehouseJob : JobBase
    {
        /// <summary>
        /// 拉式备料调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}]，当前身份[{RT.IdentityId}]\r\n");

            try
            {
                 RT.Service.Resolve<AutoJoinLineWarehouseController>().AutoJoinLineWarehouse();
                AddLog("半成品自动入线边仓执行成功");
                    
            }
            catch (Exception exMsg)
            {
                AddLog($"半成品自动入线边仓执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}
