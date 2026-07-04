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
    /// 总控向子工厂下发工作中心数据
    /// </summary>
    [Job("总控向子工厂下发工作中心数据", typeof(JobParameter))]
    public class SendGroupWorkCenterDataToFactoryJob : JobBase
    {

        //是否运行中,用来防止并发问题
        public static bool IsRun = false;

        protected override void ExecuteJob(object param)
        {
            if (SendGroupWorkCenterDataToFactoryJob.IsRun == true)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());
            SendGroupWorkCenterDataToFactoryJob.IsRun = true;

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                var curMsg = RT.Service.Resolve<GroupFactoryJobController>().CorpFactoryJobExecute(InfType.WorkCenter);
                AddLog(string.Join(",", curMsg));

            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                SendGroupWorkCenterDataToFactoryJob.IsRun = false;
            }
        }
    }
}
