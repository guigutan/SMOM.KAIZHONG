using SIE.Common.Schdules;
using SIE.DataPortal;
using SIE.ERPInterface.Ebs.Download.Items;
using System;
using System.Reflection;

namespace SIE.ERPInterface.Job.Download.Items
{
    /// <summary>
    /// EBS物料分类下载
    /// </summary>
    [JobAttribute("2.EBS物料分类下载", typeof(JobParameter))]
    public class EbsDownloadItemCateJob : JobBase
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
            var resultSmom = RT.Service.Resolve<EbsItemCateController>().Download(context.InvOrg);           //执行业务表下载
            AddLog("物料分类业务表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }

        protected override void ExecuteJob(object param)
        {
            //
        }
    }
}
