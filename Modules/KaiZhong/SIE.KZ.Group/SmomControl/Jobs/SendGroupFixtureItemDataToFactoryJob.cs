using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Group.SmomControl.Jobs
{
    /// <summary>
    /// 总控向子工厂下发工装与产品的关系数据
    /// </summary>
    [Job("总控向子工厂下发工装与产品的关系数据", typeof(JobParameter))]
    public class SendGroupFixtureItemDataToFactoryJob : JobBase
    {
        /// <summary>
        /// Job执行方法
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            var redisKey = "SendGroupFixtureItemDataToFactoryJob" + RT.InvOrg;
            string lockId = null;
            var locked = RT.Redis.Lock(redisKey, out lockId, 1800);
            if (!locked)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var curMsg = RT.Service.Resolve<GroupFactoryJobController>().CorpFactoryJobExecute(InfType.FixtureItem);
                AddLog(string.Join(",", curMsg));
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
