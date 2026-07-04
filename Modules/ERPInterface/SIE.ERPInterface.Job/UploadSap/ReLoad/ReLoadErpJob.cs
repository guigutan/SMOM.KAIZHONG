using SIE.Common.Schdules;
using SIE.ERPInterface.Job.Common;
using SIE.Threading;
using System;

namespace SIE.ERPInterface.Job.Upload
{
    /// <summary>
    /// 自动重传Job
    /// </summary>
    [Job("自动重传Job", typeof(ReLoadCommonParameter))]
    public class ReLoadErpJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "自动重传-" + typeof(ReLoadErpJob).FullName + "!" + RT.InvOrg;

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            var p = param as ReLoadCommonParameter;

            if (!SyncLocker.IsLock(_jobKey))
            {
                using (SyncLocker.Lock("自动重传-" + typeof(ReLoadErpJob).FullName + "!" + RT.InvOrg))
                {
                    //上传ERP
                    //var result = RT.Service.Resolve<SoapPurchaseInController>().ReUploadToErp(p.MaxUploadCount);
                    //AddLog("更新重传标识结束。{0}".L10N().FormatArgs(string.Join(",", result?.SuccessMsg)));
                }
            }
            else
            {
                AddLog("任务正在运行中，不允许并发执行".L10N());
            }
        }
    }
}
