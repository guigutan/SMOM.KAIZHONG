using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Job.KaiZhong
{
    /// <summary>
    /// 委外发货同步工厂数据
    /// </summary>
    [Job("委外发货同步工厂数据", typeof(JobParameter))]
    public class OutsourcingoutsSyncJob: JobBase
    {

        //是否运行中,用来防止并发问题
        public static bool IsRun = false;

        protected override void ExecuteJob(object param)
        {
            if (OutsourcingoutsSyncJob.IsRun == true)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());
            OutsourcingoutsSyncJob.IsRun = true;

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                RT.Service.Resolve<UploadBaseController>().OutsourcingoutsSync();
                //var curMsg = RT.Service.Resolve<GroupFactoryJobController>().CorpFactoryJobExecute(InfType.WorkCenter);
                AddLog("调度执行结束");

            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                OutsourcingoutsSyncJob.IsRun = false;
            }
        }
    }
}
