using SIE.Common.Schdules;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Ebs.Download.ProductBoms;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Smom.Download;
using System;

namespace SIE.ERPInterface.Job.Download.ProductBoms
{
    /// <summary>
    /// 产品Bom明细下载
    /// </summary>
    [Job("产品Bom明细下载", typeof(DLCommonParameter))]
    public class DownloadProductBomDtlJob : JobBase
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
                var resultInf = RT.Service.Resolve<SoapProductBomController>().DownloadProductBomDtlERPToInf();                     //执行中间表下载
                AddLog("中间表结束下载{0}".L10nFormat(resultInf == null ? "。" : "，" + resultInf.Msg));
            }

            var resultSmom = RT.Service.Resolve<DownloadProductBomDtlController>().DownloadProductBomDtlInfToBusiness();           //执行业务表下载
            AddLog("业务表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }
    }
}
