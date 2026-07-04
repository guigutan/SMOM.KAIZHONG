using SIE.Common.Schdules;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Ebs.Download.Items;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Smom.Download;
using System;

namespace SIE.ERPInterface.Job.Download.Items
{
    /// <summary>
    /// 物料分类下载
    /// </summary>
    [JobAttribute("物料分类下载", typeof(DLCommonParameter))]
    public class DownloadItemCateJob : JobBase
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
                var resultInf = RT.Service.Resolve<SoapItemCateController>().DownloadToInf();                     //执行中间表下载
                AddLog("中间表结束下载{0}".L10N().FormatArgs(resultInf == null ? "。" : "，" + resultInf.Msg));
            }

            var resultSmom = RT.Service.Resolve<DownloadItemCateController>().DownloadItemCateToBusiness();           //执行业务表下载
            AddLog("业务表结束下载{0}".L10N().FormatArgs(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }
    }
}
