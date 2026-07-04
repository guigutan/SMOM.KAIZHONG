using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.MES.Job.SchedulingInfs;
using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Job.SyncAndEmpRes
{
    /// <summary>
    /// 同步安灯区域资源到员工资源
    /// </summary>
    [Job("同步安灯区域资源到员工资源", typeof(JobParameter))]
    internal class SyncAndEmpResJob : JobBase
    {
        //是否运行中,用来防止并发问题
        public static bool IsRun = false;
        protected override void ExecuteJob(object param)
        {
            if (SyncAndEmpResJob.IsRun == true)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());
            SyncAndEmpResJob.IsRun = true;

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                //var curMsg = RT.Service.Resolve<SchedulingInfController>().SchedulingInfGenerateTaskJob();
                var curMsg = RT.Service.Resolve<EmployeeController>().SyncAndEmpJob();

                if (!curMsg.IsNullOrEmpty())
                    AddLog(curMsg);

            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                SyncAndEmpResJob.IsRun = false;
            }
        }
    }
}
