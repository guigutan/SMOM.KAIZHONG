using SIE.Andon.MessageSendJob;
using SIE.Common.Schdules;
using System;

namespace SIE.Andon.JobMessageSendJob
{
    /// <summary>
    /// 安灯管理消息推送Job
    /// </summary>
    [Job("安灯管理消息推送Job", typeof(JobParameter))]
    public class AndonMessageSendJob : JobBase
    {
        /// <summary>
        /// 安灯管理消息推送Job
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                //定时发送
                RT.Service.Resolve<MessageSendController>().SendMessage();
                AddLog($"安灯管理消息推送调度执行成功 !");
            }
            catch (Exception ex)
            {
                AddLog($"安灯管理消息推送调度执行失败，错误信息: {ex.Message}");
            }
        }
    }
}