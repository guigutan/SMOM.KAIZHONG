using SIE.ERPInterface.Smom.Download;
using SIE.Web.Command;
using SIE.Web.ERPInterface.DownloadManual.Common.ViewModels;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.Shipments.Commands
{
    /// <summary>
    /// 手动下载数据
    /// </summary>
    public class DlShippingOrderCommand : ViewCommand
    {
        /// <summary>
        /// 执行，最新EBS采用的是推式下载，拉式读取目前已失效，202403
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<DownloadViewModel>();
            var result = RT.Service.Resolve<DownloadShipmentController>().DownloadManual(data.KeyWord);

            if (result.IsNullOrEmpty())
                return "OK";
            else
                return result;
        }
    }
}
