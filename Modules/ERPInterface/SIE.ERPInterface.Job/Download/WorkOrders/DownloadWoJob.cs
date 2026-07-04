using SIE.Common.Schdules;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Download.WorkOrders;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Smom.Download;
using System;

namespace SIE.ERPInterface.Job.Download.WorkOrders
{
    /// <summary>
    /// 工单下载
    /// </summary>
    [Job("工单下载", typeof(DLCommonParameter))]
    public class DownloadWoJob : JobBase
    {
        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            var p = param as DLCommonParameter;
            if (p?.IsDownloadInf == true)
            {
                var resultInf = RT.Service.Resolve<SoapWorkOrderController>().DownloadToInf();                     //执行中间表下载
                AddLog("中间表结束下载{0}".L10nFormat(resultInf == null ? "。" : "，" + resultInf.Msg));
            }

            var resultSmom = RT.Service.Resolve<DownloadWorkOrderController>().DownloadWorkOrderInfToBusiness();           //执行业务表下载
            AddLog("业务表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }
    }
}
