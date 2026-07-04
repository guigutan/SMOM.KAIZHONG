using SIE.Common.Schdules;
using SIE.DataPortal;
using SIE.ERPInterface.Ebs.Download.Projects;
using System;
using System.Reflection;

namespace SIE.ERPInterface.Job.Download.Projects
{
    /// <summary>
    /// EBS项目下载
    /// </summary>
    [JobAttribute("10.EBS项目下载", typeof(JobParameter))]
    public class EbsDownloadProjectNoJob : JobBase
    {         
        //
        // 摘要:
        //     执行任务
        //
        // 参数:
        //   context:
        protected override void ExecuteJob(JobContext context)
        {
            AppRuntime.InvOrg = context.InvOrg;
            AppRuntime.Principal = new DataPortalPrincipal(context.IdentityId, 0.0, "");
            AppRuntime.InvOrg = context.InvOrg;
            JobParameter jobParameter = Activator.CreateInstance(Type.GetType(context.JobClass).GetCustomAttribute<JobAttribute>()?.ParameterType ?? typeof(JobParameter)) as JobParameter;
            jobParameter?.Initialize(context.Parameter);


            var resultSmom = RT.Service.Resolve<EbsProjectNoController>().Download(context.InvOrg);           //执行业务表下载
            AddLog("项目号基础表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            //
        }
    }
}
