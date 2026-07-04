using SIE.Common.Schdules;
using SIE.DataPortal;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Ebs.Download.AccountAliases;
using SIE.ERPInterface.Ebs.Download.Items;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Smom.Download;
using System;
using System.Reflection;

namespace SIE.ERPInterface.Job.Download.Items
{
    /// <summary>
    /// Ebs物料下载
    /// </summary>
    [JobAttribute("5.Ebs物料下载", typeof(DLCommonParameter))]
    public class EbsDownloadItemJob : JobBase
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
            var p = jobParameter as DLCommonParameter;          


            var resultSmom = RT.Service.Resolve<EbsItemCateController>().Download(context.InvOrg);
            AddLog("分类基础表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));  
            
            resultSmom = RT.Service.Resolve<EbsUnitController>().Download(context.InvOrg);           //执行业务表下载
            AddLog("单位基础表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));

            if (p?.IsDownloadInf == true)
            {
                var resultInf = RT.Service.Resolve<EbsItemController>().DownloadToInf(context.InvOrg);                             //执行中间表下载
                AddLog("物料中间表结束下载{0}".L10nFormat(resultInf == null ? "。" : "，" + resultInf.Msg));
            }

            resultSmom = RT.Service.Resolve<DownloadItemController>().DownloadItemInfToBusiness();              //执行业务表下载
            AddLog("物料表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }

        protected override void ExecuteJob(object param)
        {

        }
    }
}
