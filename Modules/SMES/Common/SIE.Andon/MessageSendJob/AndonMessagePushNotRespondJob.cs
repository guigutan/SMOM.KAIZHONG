using SIE.Andon.MessageSendJob;
using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.MES.Andon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.JobMessageSendJob
{
    /// <summary>
    /// 推送安灯数据
    /// </summary>
    [Job("推送安灯数据", typeof(JobParameter))]
    internal class AndonMessagePushNotRespondJob : JobBase
    {
        //是否运行中,用来防止并发问题
        public static bool IsRun = false;
        protected override void ExecuteJob(object param)
        {
            if (AndonMessagePushNotRespondJob.IsRun == true)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());
            AndonMessagePushNotRespondJob.IsRun = true;

            try
            {
                RT.Service.Resolve<MessageSendController>().AndonMessagePushAsync();
                AddLog("调度执行结束！");
            }
            catch (Exception ex) 
            {
                AddLog(ex.GetBaseException().Message);
            }
            finally
            {
                AndonMessagePushNotRespondJob.IsRun = false;
            }
        }
    }
}
