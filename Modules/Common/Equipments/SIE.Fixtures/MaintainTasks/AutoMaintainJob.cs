using SIE.Common.Schdules;
using SIE.Fixtures;
using SIE.Fixtures.MaintainTasks;
using System;

namespace SIE.Fixtures.MaintainTasks
{
    /// <summary>
    /// 工治具上线定期保养调度
    /// </summary>
    [Job("工治具上线定期保养", typeof(JobParameter))]
    public class AutoMaintainJob : JobBase
    {
        /// <summary>
        /// 工治具上线定期保养调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var message = RT.Service.Resolve<MaintainTaskController>().AutoMaintain();
                AddLog(message.IsNullOrEmpty() ? "工治具上线定期保养执行完成" : $"工治具上线定期保养执行失败，错误信息：{message}");
            }
            catch (Exception ex)
            {
                AddLog($"工治具上线定期保养执行失败，错误信息：{ex.Message}");
            }
        }
    }
}
