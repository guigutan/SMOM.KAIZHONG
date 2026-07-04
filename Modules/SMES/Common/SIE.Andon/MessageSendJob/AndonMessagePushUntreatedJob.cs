using SIE.Common.Schdules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.JobMessageSendJob
{
    /// <summary>
    /// 未处理
    /// </summary>
    [Job("推送安灯未处理数据", typeof(JobParameter))]
    internal class AndonMessagePushUntreatedJob : JobBase
    {
        protected override void ExecuteJob(object param)
        {
            throw new NotImplementedException();
        }
    }
}
