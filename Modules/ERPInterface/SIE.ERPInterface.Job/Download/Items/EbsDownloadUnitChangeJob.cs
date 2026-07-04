using SIE.Common.Schdules;
using SIE.DataPortal;
using SIE.ERPInterface.Ebs.Download.Items;
using System;
using System.Reflection;

namespace SIE.ERPInterface.Job.Download.Items
{
    /// <summary>
    /// EBS单位转换下载
    /// </summary>
    [JobAttribute("4.EBS单位转换下载", typeof(JobParameter))]
    public class EbsDownloadUnitChangeJob : JobBase
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


            var resultSmom = RT.Service.Resolve<EbsItemUnitChangeController>().Download(context.InvOrg);           //执行业务表下载
            AddLog("物料单位转换基础表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
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
