using SIE.Common.Schdules;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务下达邮件Job
    /// </summary>
    [Job("盘点任务下达邮件Job", typeof(JobParameter))]
    public class InventoryTaskMailJob : JobBase
    {
        /// <summary>
        /// 盘点任务下达邮件Job
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
            try
            {
                RT.Service.Resolve<InventoryTaskController>().ReleaseTaskJob();
                AddLog($"盘点任务下达邮件Job执行成功 !");
            }
            catch (Exception exMsg)
            {
                AddLog($"盘点任务下达邮件Job执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}
