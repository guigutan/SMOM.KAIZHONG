using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.SchedulingInfs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Job.SchedulingInfs
{
    /// <summary>
    /// MES排程导入中间表校验
    /// </summary>
    [Job("MES排程导入中间表校验", typeof(JobParameter))]
    public class SchedulingInfValidJob : JobBase
    {
        //是否运行中,用来防止并发问题
        public static bool IsRun = false;

        protected override void ExecuteJob(object param)
        {
            if (SchedulingInfValidJob.IsRun == true)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());
            SchedulingInfValidJob.IsRun = true;

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                RT.Service.Resolve<SchedulingInfController>().SchedulingInfValidJob();

                AddLog("调度执行成功");

            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                SchedulingInfValidJob.IsRun = false;
            }
        }
    }
}
