using SIE.Common.Schdules;
using SIE.MES.QTimes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Job
{
    /// <summary>
    /// QT超时报表消息推送调度
    /// </summary>
    [Job("QT超时报表消息推送调度", typeof(JobParameter))]
    public class QTimeMessageSendJob : JobBase
    {
        /// <summary>
        /// 调度
        /// </summary>
        /// <param name="param"></param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                //定时发送
                RT.Service.Resolve<QTimeReportService>().QTimeMessageSend();
                AddLog($"QT超时报表消息推送调度执行成功 !");
            }
            catch (Exception ex)
            {
                AddLog($"QT超时报表消息推送调度执行失败，错误信息: {ex.Message}");
            }
        }
    }
}
