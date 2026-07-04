using SIE.Common.Schdules;
using SIE.ERPInterface.Download.InvOrgs;
using SIE.ERPInterface.Job.Common;
using System;

namespace SIE.ERPInterface.Job.Download.InvOrgs
{
    /// <summary>
    /// EBS库存组织下载
    /// </summary>
    [Job("1.EBS库存组织下载", typeof(JobParameter))]
    public class EbsDownloadInvOrgJob : JobBase
    {
        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {            
            var resultSmom = RT.Service.Resolve<InvOrgDownloadController>().Download();           //执行业务表下载
            AddLog("库存组织结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }
    }
}
