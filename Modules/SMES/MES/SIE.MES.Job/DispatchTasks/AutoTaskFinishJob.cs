using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Job.DispatchTasks
{
    /// <summary>
    /// 任务单自动完工
    /// </summary>
    [Job("任务单自动完工", typeof(JobParameter))]
    public class AutoTaskFinishJob : JobBase
    {
        protected override void ExecuteJob(object param)
        {
            var redisKey = "AutoTaskFinishJob" + RT.InvOrg;
            string lockId = null;
            var locked = RT.Redis.Lock(redisKey, out lockId, 1800);
            if (!locked)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                var num = RT.Service.Resolve<DispatchController>().AutoTaskFinishJob();

                AddLog("调度执行成功,完成任务单数[{0}]".L10nFormat(num));

            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                RT.Redis.UnLock(redisKey, lockId);
            }
        }
    }
}
